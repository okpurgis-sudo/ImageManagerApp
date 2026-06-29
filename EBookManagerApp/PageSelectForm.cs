using EBookManagerApp.Models;
using EBookManagerApp.Services;
using System.Drawing.Drawing2D;
using System.Threading;

namespace EBookManagerApp
{
    public partial class PageSelectForm : Form
    {
        private readonly List<string> _imageFiles;
        private readonly int _currentPageIndex;
        private readonly HashSet<int> _bookmarkPageIndexes;
        private readonly List<BookmarkPage> _bookmarkPages;

        private Label? _footerLabel;
        private Button? _bookmarkListButton;
        private ThumbnailGridControl? _thumbnailGrid;

        public int SelectedPageIndex { get; private set; } = -1;

        public PageSelectForm(
            List<string> imageFiles,
            int currentPageIndex,
            HashSet<int>? bookmarkPageIndexes = null,
            List<BookmarkPage>? bookmarkPages = null)
        {
            InitializeComponent();

            _imageFiles = imageFiles;
            _currentPageIndex = currentPageIndex;
            _bookmarkPageIndexes = bookmarkPageIndexes ?? new HashSet<int>();
            _bookmarkPages = bookmarkPages ?? new List<BookmarkPage>();

            Text = "ページ一覧";
            StartPosition = FormStartPosition.CenterParent;
            Size = new Size(1300, 900);
            MinimumSize = new Size(900, 650);

            BuildLayout();

            AppThemeService.ApplyTheme(this);
        }

        private void BuildLayout()
        {
            Controls.Clear();

            var mainLayout = new TableLayoutPanel
            {
                Dock = DockStyle.Fill,
                ColumnCount = 1,
                RowCount = 2,
                Margin = new Padding(0),
                Padding = new Padding(0)
            };

            mainLayout.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));
            mainLayout.RowStyles.Add(new RowStyle(SizeType.Absolute, 42F));

            _thumbnailGrid = new ThumbnailGridControl(
                _imageFiles,
                _currentPageIndex,
                _bookmarkPageIndexes
            )
            {
                Dock = DockStyle.Fill
            };

            _thumbnailGrid.PageSelected += pageIndex =>
            {
                SelectedPageIndex = pageIndex;
                DialogResult = DialogResult.OK;
                Close();
            };

            var footerPanel = new Panel
            {
                Dock = DockStyle.Fill,
                Margin = new Padding(0),
                Padding = new Padding(0)
            };

            _footerLabel = new Label
            {
                AutoSize = true,
                Text = $"{_imageFiles.Count}ページ / ブックマーク {_bookmarkPages.Count}件"
            };

            _bookmarkListButton = new Button
            {
                Text = "BM一覧",
                Width = 90,
                Height = 28,
                TabStop = false
            };

            _bookmarkListButton.Click += btnBookmarkList_Click;

            footerPanel.Controls.Add(_footerLabel);
            footerPanel.Controls.Add(_bookmarkListButton);

            void LayoutFooter()
            {
                if (_footerLabel == null || _bookmarkListButton == null)
                {
                    return;
                }

                int margin = 8;
                int centerY = footerPanel.ClientSize.Height / 2;

                _footerLabel.Location = new Point(
                    margin,
                    centerY - _footerLabel.Height / 2
                );

                _bookmarkListButton.Location = new Point(
                    footerPanel.ClientSize.Width - margin - _bookmarkListButton.Width,
                    centerY - _bookmarkListButton.Height / 2
                );
            }

            footerPanel.Resize += (sender, e) => LayoutFooter();

            mainLayout.Controls.Add(_thumbnailGrid, 0, 0);
            mainLayout.Controls.Add(footerPanel, 0, 1);

            Controls.Add(mainLayout);

            LayoutFooter();
        }

        private void btnBookmarkList_Click(object? sender, EventArgs e)
        {
            OpenBookmarkList();
        }

        private void OpenBookmarkList()
        {
            if (_bookmarkPages.Count == 0)
            {
                MessageBox.Show("この作品にはブックマークがありません。");
                return;
            }

            using var form = new BookmarkListForm(_bookmarkPages);

            if (form.ShowDialog(this) != DialogResult.OK)
            {
                return;
            }

            if (form.SelectedPageIndex < 0 ||
                form.SelectedPageIndex >= _imageFiles.Count)
            {
                return;
            }

            SelectedPageIndex = form.SelectedPageIndex;
            DialogResult = DialogResult.OK;
            Close();
        }

        protected override void OnFormClosed(FormClosedEventArgs e)
        {
            _thumbnailGrid?.DisposeThumbnails();

            base.OnFormClosed(e);
        }

        private class ThumbnailGridControl : ScrollableControl
        {
            private readonly List<string> _imageFiles;
            private readonly int _currentPageIndex;
            private readonly HashSet<int> _bookmarkPageIndexes;

            private readonly Dictionary<int, Image> _thumbnailCache = new Dictionary<int, Image>();
            private readonly HashSet<int> _loadingIndexes = new HashSet<int>();
            private readonly HashSet<int> _failedIndexes = new HashSet<int>();

            private readonly CancellationTokenSource _cancellationTokenSource = new CancellationTokenSource();
            private readonly SemaphoreSlim _thumbnailLoadSemaphore = new SemaphoreSlim(2);

            private const int MaxThumbnailCacheCount = 120;

            private const int CellWidth = 320;
            private const int CellHeight = 460;
            private const int ThumbnailWidth = 270;
            private const int ThumbnailHeight = 370;
            private const int PaddingSize = 16;

            private int _columns = 1;

            public event Action<int>? PageSelected;

            public ThumbnailGridControl(
                List<string> imageFiles,
                int currentPageIndex,
                HashSet<int> bookmarkPageIndexes)
            {
                _imageFiles = imageFiles;
                _currentPageIndex = currentPageIndex;
                _bookmarkPageIndexes = bookmarkPageIndexes;

                AutoScroll = true;
                DoubleBuffered = true;
                BackColor = AppThemeService.FormBackColor;

                UpdateScrollSize();
            }

            protected override void OnResize(EventArgs e)
            {
                base.OnResize(e);

                UpdateScrollSize();
                Invalidate();
            }

            private void UpdateScrollSize()
            {
                int usableWidth = Math.Max(1, ClientSize.Width - PaddingSize * 2);

                _columns = Math.Max(1, usableWidth / CellWidth);

                int rows = (int)Math.Ceiling((double)_imageFiles.Count / _columns);
                int totalHeight = PaddingSize * 2 + rows * CellHeight;

                AutoScrollMinSize = new Size(0, totalHeight);
            }

            protected override void OnPaint(PaintEventArgs e)
            {
                base.OnPaint(e);

                e.Graphics.SmoothingMode = SmoothingMode.HighQuality;
                e.Graphics.InterpolationMode = InterpolationMode.HighQualityBicubic;
                e.Graphics.PixelOffsetMode = PixelOffsetMode.HighQuality;

                var visibleVirtualRect = new Rectangle(
                    -AutoScrollPosition.X,
                    -AutoScrollPosition.Y,
                    ClientSize.Width,
                    ClientSize.Height
                );

                int startRow = Math.Max(0, (visibleVirtualRect.Top - PaddingSize) / CellHeight);
                int endRow = Math.Min(
                    (int)Math.Ceiling((double)_imageFiles.Count / _columns),
                    (visibleVirtualRect.Bottom - PaddingSize) / CellHeight + 2
                );

                int startIndex = startRow * _columns;
                int endIndex = Math.Min(_imageFiles.Count - 1, endRow * _columns - 1);

                for (int i = startIndex; i <= endIndex; i++)
                {
                    if (i < 0 || i >= _imageFiles.Count)
                    {
                        continue;
                    }

                    DrawPageItem(e.Graphics, i);
                    RequestThumbnailIfNeeded(i);
                }
            }

            private void DrawPageItem(Graphics graphics, int pageIndex)
            {
                var virtualRect = GetItemVirtualBounds(pageIndex);

                var rect = new Rectangle(
                    virtualRect.X + AutoScrollPosition.X,
                    virtualRect.Y + AutoScrollPosition.Y,
                    virtualRect.Width,
                    virtualRect.Height
                );

                if (rect.Right < 0 ||
                    rect.Bottom < 0 ||
                    rect.Left > ClientSize.Width ||
                    rect.Top > ClientSize.Height)
                {
                    return;
                }

                using var backgroundBrush = new SolidBrush(AppThemeService.FormBackColor);
                graphics.FillRectangle(backgroundBrush, rect);

                Color borderColor = pageIndex == _currentPageIndex
                    ? AppThemeService.AccentColor
                    : AppThemeService.BorderColor;

                using var borderPen = new Pen(borderColor, pageIndex == _currentPageIndex ? 3 : 1);
                graphics.DrawRectangle(borderPen, rect.X, rect.Y, rect.Width - 1, rect.Height - 1);

                var thumbnailRect = new Rectangle(
                    rect.X + (rect.Width - ThumbnailWidth) / 2,
                    rect.Y + 14,
                    ThumbnailWidth,
                    ThumbnailHeight
                );

                DrawThumbnailArea(graphics, pageIndex, thumbnailRect);

                if (_bookmarkPageIndexes.Contains(pageIndex))
                {
                    DrawBookmarkMark(graphics, thumbnailRect);
                }

                string pageText = $"{pageIndex + 1}ページ";

                using var pageFont = new Font("Yu Gothic UI", 10, FontStyle.Regular);
                using var textBrush = new SolidBrush(AppThemeService.TextColor);

                var pageTextRect = new Rectangle(
                    rect.X + 10,
                    thumbnailRect.Bottom + 8,
                    rect.Width - 20,
                    26
                );

                DrawCenteredText(graphics, pageText, pageFont, textBrush, pageTextRect);

                string statusText = GetStatusText(pageIndex);

                using var statusFont = new Font("Yu Gothic UI", 9, FontStyle.Regular);
                using var statusBrush = new SolidBrush(AppThemeService.SubTextColor);

                var statusTextRect = new Rectangle(
                    rect.X + 10,
                    pageTextRect.Bottom + 4,
                    rect.Width - 20,
                    24
                );

                DrawCenteredText(graphics, statusText, statusFont, statusBrush, statusTextRect);
            }

            private void DrawBookmarkMark(Graphics graphics, Rectangle thumbnailRect)
            {
                var markRect = new Rectangle(
                    thumbnailRect.Left + 8,
                    thumbnailRect.Top + 8,
                    42,
                    28
                );

                using var backgroundBrush = new SolidBrush(AppThemeService.ControlBackColor);
                graphics.FillRectangle(backgroundBrush, markRect);

                using var borderPen = new Pen(AppThemeService.AccentColor, 2);
                graphics.DrawRectangle(
                    borderPen,
                    markRect.X,
                    markRect.Y,
                    markRect.Width - 1,
                    markRect.Height - 1
                );

                using var font = new Font("Yu Gothic UI", 10, FontStyle.Bold);
                using var brush = new SolidBrush(AppThemeService.AccentColor);

                DrawCenteredText(graphics, "BM", font, brush, markRect);
            }

            private void DrawThumbnailArea(Graphics graphics, int pageIndex, Rectangle thumbnailRect)
            {
                if (_thumbnailCache.TryGetValue(pageIndex, out var thumbnail))
                {
                    graphics.DrawImage(thumbnail, thumbnailRect);
                    return;
                }

                Color fillColor;

                if (_failedIndexes.Contains(pageIndex))
                {
                    fillColor = AppThemeService.IsDarkMode
                        ? Color.FromArgb(80, 40, 40)
                        : Color.MistyRose;
                }
                else
                {
                    fillColor = AppThemeService.IsDarkMode
                        ? Color.FromArgb(55, 55, 55)
                        : Color.Gainsboro;
                }

                using var fillBrush = new SolidBrush(fillColor);
                graphics.FillRectangle(fillBrush, thumbnailRect);

                using var borderPen = new Pen(AppThemeService.BorderColor);
                graphics.DrawRectangle(
                    borderPen,
                    thumbnailRect.X,
                    thumbnailRect.Y,
                    thumbnailRect.Width - 1,
                    thumbnailRect.Height - 1
                );

                string text;

                if (_failedIndexes.Contains(pageIndex))
                {
                    text = "読込失敗";
                }
                else if (_loadingIndexes.Contains(pageIndex))
                {
                    text = "読込中...";
                }
                else
                {
                    text = "読込待ち";
                }

                using var font = new Font("Yu Gothic UI", 11, FontStyle.Regular);
                using var brush = new SolidBrush(AppThemeService.SubTextColor);

                DrawCenteredText(graphics, text, font, brush, thumbnailRect);
            }

            private string GetStatusText(int pageIndex)
            {
                if (_thumbnailCache.ContainsKey(pageIndex))
                {
                    return "読込完了";
                }

                if (_failedIndexes.Contains(pageIndex))
                {
                    return "読込失敗";
                }

                if (_loadingIndexes.Contains(pageIndex))
                {
                    return "読込中...";
                }

                return "読込待ち";
            }

            private void DrawCenteredText(Graphics graphics, string text, Font font, Brush brush, Rectangle rect)
            {
                using var format = new StringFormat
                {
                    Alignment = StringAlignment.Center,
                    LineAlignment = StringAlignment.Center
                };

                graphics.DrawString(text, font, brush, rect, format);
            }

            private Rectangle GetItemVirtualBounds(int pageIndex)
            {
                int row = pageIndex / _columns;
                int column = pageIndex % _columns;

                int x = PaddingSize + column * CellWidth;
                int y = PaddingSize + row * CellHeight;

                return new Rectangle(x, y, CellWidth - 16, CellHeight - 16);
            }

            protected override void OnMouseDown(MouseEventArgs e)
            {
                base.OnMouseDown(e);

                int virtualX = e.X - AutoScrollPosition.X;
                int virtualY = e.Y - AutoScrollPosition.Y;

                int pageIndex = HitTest(virtualX, virtualY);

                if (pageIndex < 0)
                {
                    return;
                }

                PageSelected?.Invoke(pageIndex);
            }

            private int HitTest(int virtualX, int virtualY)
            {
                int row = (virtualY - PaddingSize) / CellHeight;
                int column = (virtualX - PaddingSize) / CellWidth;

                if (row < 0 || column < 0)
                {
                    return -1;
                }

                int pageIndex = row * _columns + column;

                if (pageIndex < 0 || pageIndex >= _imageFiles.Count)
                {
                    return -1;
                }

                var rect = GetItemVirtualBounds(pageIndex);

                if (!rect.Contains(virtualX, virtualY))
                {
                    return -1;
                }

                return pageIndex;
            }

            private void RequestThumbnailIfNeeded(int pageIndex)
            {
                if (_thumbnailCache.ContainsKey(pageIndex))
                {
                    return;
                }

                if (_loadingIndexes.Contains(pageIndex))
                {
                    return;
                }

                if (_failedIndexes.Contains(pageIndex))
                {
                    return;
                }

                _loadingIndexes.Add(pageIndex);

                string imagePath = _imageFiles[pageIndex];

                Task.Run(() =>
                {
                    Image? thumbnail = null;
                    bool semaphoreEntered = false;

                    try
                    {
                        _thumbnailLoadSemaphore.Wait(_cancellationTokenSource.Token);
                        semaphoreEntered = true;

                        if (_cancellationTokenSource.IsCancellationRequested)
                        {
                            return;
                        }

                        thumbnail = TryCreateThumbnail(imagePath);
                    }
                    catch (OperationCanceledException)
                    {
                        return;
                    }
                    finally
                    {
                        if (semaphoreEntered)
                        {
                            _thumbnailLoadSemaphore.Release();
                        }
                    }

                    if (_cancellationTokenSource.IsCancellationRequested)
                    {
                        thumbnail?.Dispose();
                        return;
                    }

                    try
                    {
                        BeginInvoke(new Action(() =>
                        {
                            if (IsDisposed)
                            {
                                thumbnail?.Dispose();
                                return;
                            }

                            _loadingIndexes.Remove(pageIndex);

                            if (thumbnail == null)
                            {
                                _failedIndexes.Add(pageIndex);
                            }
                            else
                            {
                                AddThumbnailToCache(pageIndex, thumbnail);
                            }

                            InvalidatePage(pageIndex);
                        }));
                    }
                    catch
                    {
                        thumbnail?.Dispose();
                    }
                });
            }

            private void AddThumbnailToCache(int pageIndex, Image thumbnail)
            {
                if (_thumbnailCache.ContainsKey(pageIndex))
                {
                    thumbnail.Dispose();
                    return;
                }

                _thumbnailCache[pageIndex] = thumbnail;

                TrimThumbnailCacheAround(pageIndex);
            }

            private void TrimThumbnailCacheAround(int centerPageIndex)
            {
                if (_thumbnailCache.Count <= MaxThumbnailCacheCount)
                {
                    return;
                }

                var removeTargets = _thumbnailCache.Keys
                    .OrderByDescending(index => Math.Abs(index - centerPageIndex))
                    .Take(_thumbnailCache.Count - MaxThumbnailCacheCount)
                    .ToList();

                foreach (var index in removeTargets)
                {
                    if (_thumbnailCache.TryGetValue(index, out var image))
                    {
                        image.Dispose();
                        _thumbnailCache.Remove(index);
                    }
                }
            }

            private Image? TryCreateThumbnail(string imagePath)
            {
                try
                {
                    if (!File.Exists(imagePath))
                    {
                        return null;
                    }

                    using var stream = new FileStream(
                        imagePath,
                        FileMode.Open,
                        FileAccess.Read,
                        FileShare.ReadWrite
                    );

                    using var originalImage = Image.FromStream(stream);

                    double ratioX = (double)ThumbnailWidth / originalImage.Width;
                    double ratioY = (double)ThumbnailHeight / originalImage.Height;
                    double ratio = Math.Min(ratioX, ratioY);

                    int width = Math.Max(1, (int)(originalImage.Width * ratio));
                    int height = Math.Max(1, (int)(originalImage.Height * ratio));

                    var thumbnail = new Bitmap(ThumbnailWidth, ThumbnailHeight);

                    using var graphics = Graphics.FromImage(thumbnail);

                    graphics.Clear(Color.White);
                    graphics.InterpolationMode = InterpolationMode.HighQualityBicubic;
                    graphics.SmoothingMode = SmoothingMode.HighQuality;
                    graphics.PixelOffsetMode = PixelOffsetMode.HighQuality;

                    int x = (ThumbnailWidth - width) / 2;
                    int y = (ThumbnailHeight - height) / 2;

                    graphics.DrawImage(originalImage, x, y, width, height);

                    return thumbnail;
                }
                catch
                {
                    return null;
                }
            }

            private void InvalidatePage(int pageIndex)
            {
                var virtualRect = GetItemVirtualBounds(pageIndex);

                var screenRect = new Rectangle(
                    virtualRect.X + AutoScrollPosition.X,
                    virtualRect.Y + AutoScrollPosition.Y,
                    virtualRect.Width,
                    virtualRect.Height
                );

                Invalidate(screenRect);
            }

            public void DisposeThumbnails()
            {
                _cancellationTokenSource.Cancel();

                foreach (var image in _thumbnailCache.Values)
                {
                    image.Dispose();
                }

                _thumbnailCache.Clear();
                _loadingIndexes.Clear();
                _failedIndexes.Clear();

                _thumbnailLoadSemaphore.Dispose();
                _cancellationTokenSource.Dispose();
            }
        }
    }
}