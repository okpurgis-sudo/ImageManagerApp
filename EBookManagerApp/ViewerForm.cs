using EBookManagerApp.Models;
using EBookManagerApp.Service;
using EBookManagerApp.Services;
using System.Drawing.Drawing2D;

namespace EBookManagerApp
{
    public partial class ViewerForm : Form
    {
        private readonly List<Book> _books;
        private readonly DatabaseService _databaseService = new DatabaseService();

        private int _currentBookIndex;
        private int _currentPageIndex = 0;

        private List<string> _imageFiles = new List<string>();

        private readonly Dictionary<string, int> _rotationDegreesByImagePath = new Dictionary<string, int>();

        private HashSet<int> _bookmarkPageIndexes = new HashSet<int>();

        private readonly Button btnRotateLeft = new Button();
        private readonly Button btnRotateRight = new Button();
        private readonly Button btnBookmarkPage = new Button();
        private readonly Button btnBookmarkList = new Button();

        private Book CurrentBook => _books[_currentBookIndex];

        public ViewerForm(List<Book> books, int startBookIndex)
        {
            InitializeComponent();

            _books = books;
            _currentBookIndex = startBookIndex;

            _databaseService.Initialize();

            SetupViewerLayout();

            StartPosition = FormStartPosition.CenterScreen;
            WindowState = FormWindowState.Maximized;

            KeyPreview = true;

            AppThemeService.ApplyTheme(this);

            LoadCurrentBook();
        }

        private void SetupViewerLayout()
        {
            MinimumSize = new Size(760, 500);

            SuspendLayout();

            Controls.Remove(pictureBoxMain);
            Controls.Remove(lblPage);
            Controls.Remove(btnGoPage);

            var mainLayout = new TableLayoutPanel
            {
                Dock = DockStyle.Fill,
                ColumnCount = 1,
                RowCount = 2,
                Margin = new Padding(0),
                Padding = new Padding(0)
            };

            mainLayout.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));
            mainLayout.RowStyles.Add(new RowStyle(SizeType.Absolute, 48F));

            pictureBoxMain.Dock = DockStyle.Fill;
            pictureBoxMain.SizeMode = PictureBoxSizeMode.Zoom;
            pictureBoxMain.Margin = new Padding(8);
            pictureBoxMain.TabStop = false;

            var bottomPanel = new Panel
            {
                Dock = DockStyle.Fill,
                Height = 48,
                Margin = new Padding(0),
                Padding = new Padding(0)
            };

            btnGoPage.Text = "ページ一覧";
            btnGoPage.Width = 110;
            btnGoPage.Height = 28;
            btnGoPage.TabStop = false;

            btnRotateLeft.Text = "左回転";
            btnRotateLeft.Width = 80;
            btnRotateLeft.Height = 28;
            btnRotateLeft.TabStop = false;

            btnRotateRight.Text = "右回転";
            btnRotateRight.Width = 80;
            btnRotateRight.Height = 28;
            btnRotateRight.TabStop = false;

            btnBookmarkPage.Text = "ブックマーク";
            btnBookmarkPage.Width = 120;
            btnBookmarkPage.Height = 28;
            btnBookmarkPage.TabStop = false;

            btnBookmarkList.Text = "BM一覧";
            btnBookmarkList.Width = 90;
            btnBookmarkList.Height = 28;
            btnBookmarkList.TabStop = false;

            lblPage.AutoSize = true;

            btnGoPage.Click -= btnGoPage_Click;
            btnGoPage.Click += btnGoPage_Click;

            btnRotateLeft.Click -= btnRotateLeft_Click;
            btnRotateLeft.Click += btnRotateLeft_Click;

            btnRotateRight.Click -= btnRotateRight_Click;
            btnRotateRight.Click += btnRotateRight_Click;

            btnBookmarkPage.Click -= btnBookmarkPage_Click;
            btnBookmarkPage.Click += btnBookmarkPage_Click;

            btnBookmarkList.Click -= btnBookmarkList_Click;
            btnBookmarkList.Click += btnBookmarkList_Click;

            bottomPanel.Controls.Add(btnGoPage);
            bottomPanel.Controls.Add(btnRotateLeft);
            bottomPanel.Controls.Add(btnRotateRight);
            bottomPanel.Controls.Add(btnBookmarkPage);
            bottomPanel.Controls.Add(btnBookmarkList);
            bottomPanel.Controls.Add(lblPage);

            void LayoutBottomControls()
            {
                int spacing = 10;

                int totalWidth =
                    btnGoPage.Width +
                    spacing +
                    btnRotateLeft.Width +
                    spacing +
                    btnRotateRight.Width +
                    spacing +
                    btnBookmarkPage.Width +
                    spacing +
                    btnBookmarkList.Width +
                    spacing +
                    lblPage.Width;

                int startX = Math.Max(0, (bottomPanel.ClientSize.Width - totalWidth) / 2);
                int centerY = bottomPanel.ClientSize.Height / 2;

                btnGoPage.Location = new Point(startX, centerY - btnGoPage.Height / 2);

                btnRotateLeft.Location = new Point(
                    btnGoPage.Right + spacing,
                    centerY - btnRotateLeft.Height / 2
                );

                btnRotateRight.Location = new Point(
                    btnRotateLeft.Right + spacing,
                    centerY - btnRotateRight.Height / 2
                );

                btnBookmarkPage.Location = new Point(
                    btnRotateRight.Right + spacing,
                    centerY - btnBookmarkPage.Height / 2
                );

                btnBookmarkList.Location = new Point(
                    btnBookmarkPage.Right + spacing,
                    centerY - btnBookmarkList.Height / 2
                );

                lblPage.Location = new Point(
                    btnBookmarkList.Right + spacing,
                    centerY - lblPage.Height / 2
                );
            }

            bottomPanel.Resize += (sender, e) => LayoutBottomControls();
            lblPage.TextChanged += (sender, e) => LayoutBottomControls();
            btnBookmarkPage.TextChanged += (sender, e) => LayoutBottomControls();
            btnBookmarkList.TextChanged += (sender, e) => LayoutBottomControls();

            mainLayout.Controls.Add(pictureBoxMain, 0, 0);
            mainLayout.Controls.Add(bottomPanel, 0, 1);

            Controls.Add(mainLayout);

            LayoutBottomControls();

            ResumeLayout();
        }

        private void LoadCurrentBook()
        {
            if (_books.Count == 0)
            {
                MessageBox.Show("作品一覧が空です。");
                Close();
                return;
            }

            if (_currentBookIndex < 0)
            {
                _currentBookIndex = 0;
            }

            if (_currentBookIndex >= _books.Count)
            {
                _currentBookIndex = _books.Count - 1;
            }

            Text = CurrentBook.Name;

            LoadImages(CurrentBook.FolderPath);

            _bookmarkPageIndexes = _databaseService.GetBookmarkPageIndexes(CurrentBook);

            _currentPageIndex = GetSafeLastReadPageIndex();

            ShowPage();
        }

        private void LoadImages(string folderPath)
        {
            if (!Directory.Exists(folderPath))
            {
                MessageBox.Show("フォルダが存在しません。");
                _imageFiles.Clear();
                return;
            }

            _imageFiles = ImageFileService.GetImageFiles(folderPath);
        }

        private int GetSafeLastReadPageIndex()
        {
            if (_imageFiles.Count == 0)
            {
                return 0;
            }

            int lastReadPageIndex = _databaseService.GetLastReadPageIndex(CurrentBook);

            if (lastReadPageIndex < 0)
            {
                return 0;
            }

            if (lastReadPageIndex >= _imageFiles.Count)
            {
                return _imageFiles.Count - 1;
            }

            return lastReadPageIndex;
        }

        private void ShowPage()
        {
            if (_imageFiles.Count == 0)
            {
                DisposeCurrentImage();

                lblPage.Text = "0 / 0";
                Text = CurrentBook.Name;

                UpdateBookmarkButton();

                MessageBox.Show("画像ファイルが見つかりません。");
                return;
            }

            if (_currentPageIndex < 0)
            {
                _currentPageIndex = 0;
            }

            if (_currentPageIndex >= _imageFiles.Count)
            {
                _currentPageIndex = _imageFiles.Count - 1;
            }

            string imagePath = _imageFiles[_currentPageIndex];

            DisposeCurrentImage();

            try
            {
                pictureBoxMain.Image = CreateDisplayImage(imagePath);
            }
            catch
            {
                MessageBox.Show("画像の読み込みに失敗しました。");
                return;
            }

            lblPage.Text = $"{_currentPageIndex + 1} / {_imageFiles.Count}";

            UpdateBookmarkButton();
            UpdateWindowTitle();
        }

        private Image CreateDisplayImage(string imagePath)
        {
            using var stream = new FileStream(
                imagePath,
                FileMode.Open,
                FileAccess.Read,
                FileShare.ReadWrite
            );

            using var originalImage = Image.FromStream(stream);

            int rotationDegrees = GetRotationDegrees(imagePath);

            var targetSize = CalculateDisplayImageSize(
                originalImage.Width,
                originalImage.Height,
                rotationDegrees
            );

            var bitmap = new Bitmap(targetSize.Width, targetSize.Height);

            using var graphics = Graphics.FromImage(bitmap);

            graphics.Clear(Color.Black);
            graphics.InterpolationMode = InterpolationMode.HighQualityBicubic;
            graphics.SmoothingMode = SmoothingMode.HighQuality;
            graphics.PixelOffsetMode = PixelOffsetMode.HighQuality;

            DrawRotatedImage(
                graphics,
                originalImage,
                rotationDegrees,
                targetSize
            );

            return bitmap;
        }

        private Size CalculateDisplayImageSize(int originalWidth, int originalHeight, int rotationDegrees)
        {
            bool isSideways = rotationDegrees == 90 || rotationDegrees == 270;

            int imageWidth = isSideways ? originalHeight : originalWidth;
            int imageHeight = isSideways ? originalWidth : originalHeight;

            int maxWidth = pictureBoxMain.ClientSize.Width - 16;
            int maxHeight = pictureBoxMain.ClientSize.Height - 16;

            if (maxWidth < 100)
            {
                maxWidth = Math.Max(100, ClientSize.Width - 32);
            }

            if (maxHeight < 100)
            {
                maxHeight = Math.Max(100, ClientSize.Height - 96);
            }

            double ratioX = (double)maxWidth / imageWidth;
            double ratioY = (double)maxHeight / imageHeight;

            double ratio = Math.Min(ratioX, ratioY);

            if (ratio > 1.0)
            {
                ratio = 1.0;
            }

            int width = Math.Max(1, (int)(imageWidth * ratio));
            int height = Math.Max(1, (int)(imageHeight * ratio));

            return new Size(width, height);
        }

        private void DrawRotatedImage(Graphics graphics, Image originalImage, int rotationDegrees, Size targetSize)
        {
            switch (rotationDegrees)
            {
                case 90:
                    graphics.TranslateTransform(targetSize.Width, 0);
                    graphics.RotateTransform(90);
                    graphics.DrawImage(
                        originalImage,
                        new Rectangle(0, 0, targetSize.Height, targetSize.Width)
                    );
                    break;

                case 180:
                    graphics.TranslateTransform(targetSize.Width, targetSize.Height);
                    graphics.RotateTransform(180);
                    graphics.DrawImage(
                        originalImage,
                        new Rectangle(0, 0, targetSize.Width, targetSize.Height)
                    );
                    break;

                case 270:
                    graphics.TranslateTransform(0, targetSize.Height);
                    graphics.RotateTransform(270);
                    graphics.DrawImage(
                        originalImage,
                        new Rectangle(0, 0, targetSize.Height, targetSize.Width)
                    );
                    break;

                default:
                    graphics.DrawImage(
                        originalImage,
                        new Rectangle(0, 0, targetSize.Width, targetSize.Height)
                    );
                    break;
            }

            graphics.ResetTransform();
        }

        private void SaveCurrentReadingProgress()
        {
            if (_imageFiles.Count == 0)
            {
                return;
            }

            if (_currentPageIndex < 0 || _currentPageIndex >= _imageFiles.Count)
            {
                return;
            }

            string imagePath = _imageFiles[_currentPageIndex];

            _databaseService.SaveReadingProgress(
                CurrentBook,
                _currentPageIndex,
                imagePath
            );
        }

        private void UpdateWindowTitle()
        {
            if (_imageFiles.Count == 0)
            {
                Text = CurrentBook.Name;
                return;
            }

            string imagePath = _imageFiles[_currentPageIndex];

            int currentRotation = GetRotationDegrees(imagePath);
            bool isBookmark = _bookmarkPageIndexes.Contains(_currentPageIndex);

            var title = $"{CurrentBook.Name}  -  {_currentPageIndex + 1} / {_imageFiles.Count}";

            if (isBookmark)
            {
                title += "  -  ブックマーク";
            }

            if (currentRotation != 0)
            {
                title += $"  -  {currentRotation}度回転";
            }

            Text = title;
        }

        private void UpdateBookmarkButton()
        {
            if (_imageFiles.Count == 0)
            {
                btnBookmarkPage.Enabled = false;
                btnBookmarkPage.Text = "ブックマーク";
                return;
            }

            btnBookmarkPage.Enabled = true;

            if (_bookmarkPageIndexes.Contains(_currentPageIndex))
            {
                btnBookmarkPage.Text = "メモ編集";
            }
            else
            {
                btnBookmarkPage.Text = "ブックマーク";
            }
        }

        private void OpenBookmarkMemo()
        {
            if (_imageFiles.Count == 0)
            {
                return;
            }

            string imagePath = _imageFiles[_currentPageIndex];

            string currentMemo = _databaseService.GetBookmarkMemo(
                CurrentBook,
                _currentPageIndex
            );

            using var form = new BookmarkMemoForm(currentMemo);

            if (form.ShowDialog(this) != DialogResult.OK)
            {
                return;
            }

            if (form.IsDeleteRequested)
            {
                _databaseService.DeleteBookmarkPage(
                    CurrentBook,
                    _currentPageIndex
                );

                _bookmarkPageIndexes.Remove(_currentPageIndex);

                UpdateBookmarkButton();
                UpdateWindowTitle();

                return;
            }

            _databaseService.SaveBookmarkPage(
                CurrentBook,
                _currentPageIndex,
                imagePath,
                form.MemoText
            );

            _bookmarkPageIndexes.Add(_currentPageIndex);

            UpdateBookmarkButton();
            UpdateWindowTitle();
        }

        private void OpenBookmarkList()
        {
            if (_imageFiles.Count == 0)
            {
                return;
            }

            var bookmarks = _databaseService.GetBookmarkPages(CurrentBook);

            if (bookmarks.Count == 0)
            {
                MessageBox.Show("この作品にはブックマークがありません。");
                return;
            }

            using var form = new BookmarkListForm(bookmarks);

            if (form.ShowDialog(this) != DialogResult.OK)
            {
                return;
            }

            if (form.SelectedPageIndex < 0 ||
                form.SelectedPageIndex >= _imageFiles.Count)
            {
                return;
            }

            _currentPageIndex = form.SelectedPageIndex;
            ShowPage();
        }

        private int GetRotationDegrees(string imagePath)
        {
            if (_rotationDegreesByImagePath.TryGetValue(imagePath, out int degrees))
            {
                return degrees;
            }

            return 0;
        }

        private void SetRotationDegrees(string imagePath, int degrees)
        {
            degrees %= 360;

            if (degrees < 0)
            {
                degrees += 360;
            }

            if (degrees == 0)
            {
                _rotationDegreesByImagePath.Remove(imagePath);
                return;
            }

            _rotationDegreesByImagePath[imagePath] = degrees;
        }

        private void RotateCurrentPageLeft()
        {
            if (_imageFiles.Count == 0)
            {
                return;
            }

            string imagePath = _imageFiles[_currentPageIndex];

            int currentDegrees = GetRotationDegrees(imagePath);
            int nextDegrees = currentDegrees - 90;

            SetRotationDegrees(imagePath, nextDegrees);

            ShowPage();
        }

        private void RotateCurrentPageRight()
        {
            if (_imageFiles.Count == 0)
            {
                return;
            }

            string imagePath = _imageFiles[_currentPageIndex];

            int currentDegrees = GetRotationDegrees(imagePath);
            int nextDegrees = currentDegrees + 90;

            SetRotationDegrees(imagePath, nextDegrees);

            ShowPage();
        }

        private void NextPage()
        {
            if (_imageFiles.Count == 0)
            {
                return;
            }

            if (_currentPageIndex >= _imageFiles.Count - 1)
            {
                return;
            }

            _currentPageIndex++;
            ShowPage();
        }

        private void PreviousPage()
        {
            if (_imageFiles.Count == 0)
            {
                return;
            }

            if (_currentPageIndex <= 0)
            {
                return;
            }

            _currentPageIndex--;
            ShowPage();
        }

        private void MoveToNextBook()
        {
            if (_books.Count == 0)
            {
                return;
            }

            if (_currentBookIndex >= _books.Count - 1)
            {
                return;
            }

            SaveCurrentReadingProgress();

            _currentBookIndex++;
            LoadCurrentBook();
        }

        private void MoveToPreviousBook()
        {
            if (_books.Count == 0)
            {
                return;
            }

            if (_currentBookIndex <= 0)
            {
                return;
            }

            SaveCurrentReadingProgress();

            _currentBookIndex--;
            LoadCurrentBook();
        }

        protected override bool ProcessCmdKey(ref Message msg, Keys keyData)
        {
            if (keyData == Keys.D || keyData == Keys.Right)
            {
                NextPage();
                return true;
            }

            if (keyData == Keys.A || keyData == Keys.Left)
            {
                PreviousPage();
                return true;
            }

            if (keyData == Keys.Down)
            {
                MoveToNextBook();
                return true;
            }

            if (keyData == Keys.Up)
            {
                MoveToPreviousBook();
                return true;
            }

            if (keyData == Keys.Q)
            {
                RotateCurrentPageLeft();
                return true;
            }

            if (keyData == Keys.E)
            {
                RotateCurrentPageRight();
                return true;
            }

            if (keyData == Keys.B)
            {
                OpenBookmarkMemo();
                return true;
            }

            if (keyData == Keys.L)
            {
                OpenBookmarkList();
                return true;
            }

            return base.ProcessCmdKey(ref msg, keyData);
        }

        private void btnGoPage_Click(object? sender, EventArgs e)
        {
            if (_imageFiles.Count == 0)
            {
                MessageBox.Show("ページ一覧を表示できる画像がありません。");
                return;
            }

            var bookmarkPages = _databaseService.GetBookmarkPages(CurrentBook);

            using var pageSelectForm = new PageSelectForm(
                _imageFiles,
                _currentPageIndex,
                _bookmarkPageIndexes,
                bookmarkPages
            );

            if (pageSelectForm.ShowDialog(this) != DialogResult.OK)
            {
                return;
            }

            if (pageSelectForm.SelectedPageIndex < 0 ||
                pageSelectForm.SelectedPageIndex >= _imageFiles.Count)
            {
                return;
            }

            _currentPageIndex = pageSelectForm.SelectedPageIndex;
            ShowPage();
        }

        private void btnRotateLeft_Click(object? sender, EventArgs e)
        {
            RotateCurrentPageLeft();
        }

        private void btnRotateRight_Click(object? sender, EventArgs e)
        {
            RotateCurrentPageRight();
        }

        private void btnBookmarkPage_Click(object? sender, EventArgs e)
        {
            OpenBookmarkMemo();
        }

        private void btnBookmarkList_Click(object? sender, EventArgs e)
        {
            OpenBookmarkList();
        }

        private void DisposeCurrentImage()
        {
            if (pictureBoxMain.Image != null)
            {
                pictureBoxMain.Image.Dispose();
                pictureBoxMain.Image = null;
            }
        }

        protected override void OnFormClosed(FormClosedEventArgs e)
        {
            SaveCurrentReadingProgress();

            DisposeCurrentImage();

            base.OnFormClosed(e);
        }
    }
}