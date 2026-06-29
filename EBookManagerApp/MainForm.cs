using EBookManagerApp.Models;
using EBookManagerApp.Services;

namespace EBookManagerApp
{
    public partial class MainForm : Form
    {
        private readonly BookScanService _bookScanService = new BookScanService();
        private readonly BookSearchService _bookSearchService = new BookSearchService();
        private readonly DatabaseService _databaseService = new DatabaseService();

        private List<Book> _books = new List<Book>();
        private List<Book> _displayBooks = new List<Book>();

        // 起動時に前回フォルダを自動読込したかどうか
        private bool _hasTriedAutoLoad = false;

        // テーマ切り替え用
        private readonly ComboBox cmbTheme = new ComboBox();
        private bool _isChangingThemeCombo = false;

        // 画面タイトル用ラベル
        private readonly Label lblAppTitle = new Label();
        private readonly Label lblAppSubtitle = new Label();

        // 見出し用ラベル
        private readonly Label lblActionTitle = new Label();
        private readonly Label lblSearchTitle = new Label();

        // 現在のソート状態
        private string _sortPropertyName = "";
        private bool _sortAscending = true;

        // ソート三角表示用に元のヘッダー文字を保存する
        private readonly Dictionary<string, string> _columnHeaderTexts = new Dictionary<string, string>();

        public MainForm()
        {
            InitializeComponent();

            AppThemeService.Load();

            _databaseService.Initialize();

            SetupHeaderControls();
            SetupMainLayout();
            SetupSearchControls();
            SetupThemeControl();
            SetupDataGridView();

            AppThemeService.ApplyTheme(this);

            Shown -= MainForm_Shown;
            Shown += MainForm_Shown;
        }

        private void SetupHeaderControls()
        {
            Text = "画像管理アプリ";

            btnSelectFolder.Text = "画像フォルダ読込";
            btnEditBook.Text = "情報編集";

            lblSearchName.Text = "作品名";
            lblSearchSeries.Text = "シリーズ";
            lblSearchAuthor.Text = "作者";
            lblSearchGenre.Text = "ジャンル";
            lblSearchTags.Text = "タグ";
            lblSearchNicePoint.Text = "ナイスポイント";

            lblAppTitle.Text = "画像管理アプリ";
            lblAppTitle.AutoSize = true;
            lblAppTitle.Font = new Font("Yu Gothic UI", 18F, FontStyle.Bold);

            lblAppSubtitle.Text = "画像ビューアー兼タグ付け検索アプリ";
            lblAppSubtitle.AutoSize = true;
            lblAppSubtitle.Font = new Font("Yu Gothic UI", 10F, FontStyle.Regular);

            lblActionTitle.Text = "操作";
            lblActionTitle.AutoSize = true;
            lblActionTitle.Font = new Font("Yu Gothic UI", 10F, FontStyle.Bold);

            lblSearchTitle.Text = "検索条件";
            lblSearchTitle.AutoSize = true;
            lblSearchTitle.Font = new Font("Yu Gothic UI", 10F, FontStyle.Bold);

            AddControlIfNeeded(lblAppTitle);
            AddControlIfNeeded(lblAppSubtitle);
            AddControlIfNeeded(lblActionTitle);
            AddControlIfNeeded(lblSearchTitle);
        }

        private void AddControlIfNeeded(Control control)
        {
            if (Controls.Contains(control))
            {
                return;
            }

            Controls.Add(control);
        }

        private void SetupMainLayout()
        {
            MinimumSize = new Size(1120, 720);

            lblAppTitle.Anchor = AnchorStyles.Top | AnchorStyles.Left;
            lblAppSubtitle.Anchor = AnchorStyles.Top | AnchorStyles.Left;
            lblActionTitle.Anchor = AnchorStyles.Top | AnchorStyles.Left;
            lblSearchTitle.Anchor = AnchorStyles.Top | AnchorStyles.Left;

            btnSelectFolder.Anchor = AnchorStyles.Top | AnchorStyles.Left;
            btnEditBook.Anchor = AnchorStyles.Top | AnchorStyles.Left;
            lblStatus.Anchor = AnchorStyles.Top | AnchorStyles.Left;

            lblSearchName.Anchor = AnchorStyles.Top | AnchorStyles.Left;
            txtSearchName.Anchor = AnchorStyles.Top | AnchorStyles.Left;

            lblSearchSeries.Anchor = AnchorStyles.Top | AnchorStyles.Left;
            txtSearchSeries.Anchor = AnchorStyles.Top | AnchorStyles.Left;

            lblSearchAuthor.Anchor = AnchorStyles.Top | AnchorStyles.Left;
            txtSearchAuthor.Anchor = AnchorStyles.Top | AnchorStyles.Left;

            lblSearchGenre.Anchor = AnchorStyles.Top | AnchorStyles.Left;
            txtSearchGenre.Anchor = AnchorStyles.Top | AnchorStyles.Left;

            lblSearchNicePoint.Anchor = AnchorStyles.Top | AnchorStyles.Left;
            txtSearchNicePoint.Anchor = AnchorStyles.Top | AnchorStyles.Left;

            lblSearchTags.Anchor = AnchorStyles.Top | AnchorStyles.Left;
            txtSearchTags.Anchor = AnchorStyles.Top | AnchorStyles.Left;

            cmbSearchMode.Anchor = AnchorStyles.Top | AnchorStyles.Left;
            btnSearch.Anchor = AnchorStyles.Top | AnchorStyles.Left;
            btnClearSearch.Anchor = AnchorStyles.Top | AnchorStyles.Left;

            if (!Controls.Contains(cmbTheme))
            {
                Controls.Add(cmbTheme);
            }

            cmbTheme.Anchor = AnchorStyles.Top | AnchorStyles.Right;

            dgvBooks.Anchor = AnchorStyles.Top | AnchorStyles.Left;

            Resize -= MainForm_Resize;
            Resize += MainForm_Resize;

            LayoutMainControls();
        }

        private void MainForm_Resize(object? sender, EventArgs e)
        {
            LayoutMainControls();
        }

        private void LayoutMainControls()
        {
            int margin = 16;

            int formWidth = ClientSize.Width;
            int formHeight = ClientSize.Height;

            if (formWidth <= 0 || formHeight <= 0)
            {
                return;
            }

            // =========================
            // ヘッダー
            // =========================
            lblAppTitle.Location = new Point(
                margin,
                10
            );

            lblAppSubtitle.Location = new Point(
                margin + 2,
                46
            );

            cmbTheme.SetBounds(
                Math.Max(margin, formWidth - 144),
                16,
                128,
                28
            );

            // =========================
            // 操作エリア
            // =========================
            lblActionTitle.Location = new Point(
                margin,
                82
            );

            btnSelectFolder.SetBounds(
                margin,
                106,
                140,
                30
            );

            btnEditBook.SetBounds(
                btnSelectFolder.Right + 8,
                106,
                90,
                30
            );

            lblStatus.AutoSize = true;
            lblStatus.Location = new Point(
                btnEditBook.Right + 16,
                113
            );

            // =========================
            // 検索エリア見出し
            // =========================
            lblSearchTitle.Location = new Point(
                margin,
                154
            );

            // =========================
            // 検索エリア
            // =========================
            int labelWidth = 72;
            int textHeight = 24;

            int row1Y = 184;
            int row2Y = 224;
            int row3Y = 264;

            int x1 = margin;
            int x2 = 318;
            int x3 = 620;

            // =========================
            // 検索エリア 1段目
            // 作品名 / シリーズ / 作者
            // =========================
            lblSearchName.SetBounds(
                x1,
                row1Y + 4,
                labelWidth,
                textHeight
            );

            txtSearchName.SetBounds(
                x1 + labelWidth,
                row1Y,
                210,
                textHeight
            );

            lblSearchSeries.SetBounds(
                x2,
                row1Y + 4,
                labelWidth,
                textHeight
            );

            txtSearchSeries.SetBounds(
                x2 + labelWidth,
                row1Y,
                210,
                textHeight
            );

            lblSearchAuthor.SetBounds(
                x3,
                row1Y + 4,
                labelWidth,
                textHeight
            );

            txtSearchAuthor.SetBounds(
                x3 + labelWidth,
                row1Y,
                210,
                textHeight
            );

            // =========================
            // 検索エリア 2段目
            // ジャンル / タグ / ナイスポイント
            // =========================
            lblSearchGenre.SetBounds(
                x1,
                row2Y + 4,
                labelWidth,
                textHeight
            );

            txtSearchGenre.SetBounds(
                x1 + labelWidth,
                row2Y,
                170,
                textHeight
            );

            lblSearchTags.SetBounds(
                x2,
                row2Y + 4,
                labelWidth,
                textHeight
            );

            txtSearchTags.SetBounds(
                x2 + labelWidth,
                row2Y,
                170,
                textHeight
            );

            lblSearchNicePoint.SetBounds(
                x3,
                row2Y + 4,
                100,
                textHeight
            );

            txtSearchNicePoint.SetBounds(
                x3 + 100,
                row2Y,
                240,
                textHeight
            );

            // =========================
            // 検索エリア 3段目
            // OR / 検索 / クリア
            // =========================
            cmbSearchMode.SetBounds(
                x1 + labelWidth,
                row3Y,
                70,
                24
            );

            btnSearch.SetBounds(
                cmbSearchMode.Right + 8,
                row3Y - 2,
                75,
                28
            );

            btnClearSearch.SetBounds(
                btnSearch.Right + 8,
                row3Y - 2,
                75,
                28
            );

            // =========================
            // 一覧エリア
            // =========================
            int gridTop = 312;
            int gridLeft = margin;
            int gridWidth = Math.Max(100, formWidth - margin * 2);
            int gridHeight = Math.Max(100, formHeight - gridTop - margin);

            dgvBooks.SetBounds(
                gridLeft,
                gridTop,
                gridWidth,
                gridHeight
            );
        }

        private void SetupThemeControl()
        {
            _isChangingThemeCombo = true;

            cmbTheme.DropDownStyle = ComboBoxStyle.DropDownList;

            cmbTheme.Items.Clear();
            cmbTheme.Items.Add("ライト");
            cmbTheme.Items.Add("ダーク");

            cmbTheme.SelectedIndex = AppThemeService.CurrentTheme == AppThemeMode.Dark
                ? 1
                : 0;

            _isChangingThemeCombo = false;

            cmbTheme.SelectedIndexChanged -= cmbTheme_SelectedIndexChanged;
            cmbTheme.SelectedIndexChanged += cmbTheme_SelectedIndexChanged;
        }

        private void cmbTheme_SelectedIndexChanged(object? sender, EventArgs e)
        {
            if (_isChangingThemeCombo)
            {
                return;
            }

            var theme = cmbTheme.SelectedIndex == 1
                ? AppThemeMode.Dark
                : AppThemeMode.Light;

            AppThemeService.SetTheme(theme);
            AppThemeService.ApplyThemeToOpenForms();

            // テーマ変更後もヘッダーの▲▼表示を維持する
            UpdateSortGlyph();
        }

        private void SetupSearchControls()
        {
            cmbSearchMode.DropDownStyle = ComboBoxStyle.DropDownList;

            cmbSearchMode.Items.Clear();
            cmbSearchMode.Items.Add("OR");
            cmbSearchMode.Items.Add("AND");
            cmbSearchMode.SelectedIndex = 0;

            btnSelectFolder.Click -= btnSelectFolder_Click;
            btnSelectFolder.Click += btnSelectFolder_Click;

            btnEditBook.Click -= btnEditBook_Click;
            btnEditBook.Click += btnEditBook_Click;

            btnSearch.Click -= btnSearch_Click;
            btnSearch.Click += btnSearch_Click;

            btnClearSearch.Click -= btnClearSearch_Click;
            btnClearSearch.Click += btnClearSearch_Click;

            RemoveSearchKeyDownEvents();
            AddSearchKeyDownEvents();
        }

        private void AddSearchKeyDownEvents()
        {
            txtSearchName.KeyDown += SearchTextBox_KeyDown;
            txtSearchSeries.KeyDown += SearchTextBox_KeyDown;
            txtSearchAuthor.KeyDown += SearchTextBox_KeyDown;
            txtSearchGenre.KeyDown += SearchTextBox_KeyDown;
            txtSearchNicePoint.KeyDown += SearchTextBox_KeyDown;
            txtSearchTags.KeyDown += SearchTextBox_KeyDown;
        }

        private void RemoveSearchKeyDownEvents()
        {
            txtSearchName.KeyDown -= SearchTextBox_KeyDown;
            txtSearchSeries.KeyDown -= SearchTextBox_KeyDown;
            txtSearchAuthor.KeyDown -= SearchTextBox_KeyDown;
            txtSearchGenre.KeyDown -= SearchTextBox_KeyDown;
            txtSearchNicePoint.KeyDown -= SearchTextBox_KeyDown;
            txtSearchTags.KeyDown -= SearchTextBox_KeyDown;
        }

        private void SearchTextBox_KeyDown(object? sender, KeyEventArgs e)
        {
            if (e.KeyCode != Keys.Enter)
            {
                return;
            }

            ApplySearch();

            e.SuppressKeyPress = true;
        }

        private void SetupDataGridView()
        {
            dgvBooks.AutoGenerateColumns = false;
            dgvBooks.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dgvBooks.MultiSelect = false;
            dgvBooks.ReadOnly = true;
            dgvBooks.AllowUserToAddRows = false;
            dgvBooks.AllowUserToDeleteRows = false;

            dgvBooks.RowHeadersVisible = false;
            dgvBooks.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dgvBooks.BackgroundColor = Color.White;
            dgvBooks.BorderStyle = BorderStyle.FixedSingle;

            dgvBooks.Columns.Clear();

            dgvBooks.Columns.Add(new DataGridViewTextBoxColumn
            {
                HeaderText = "名前",
                DataPropertyName = "Name",
                FillWeight = 180
            });

            dgvBooks.Columns.Add(new DataGridViewTextBoxColumn
            {
                HeaderText = "最終閲覧",
                DataPropertyName = "LastReadPageText",
                FillWeight = 80
            });

            dgvBooks.Columns.Add(new DataGridViewTextBoxColumn
            {
                HeaderText = "BM数",
                DataPropertyName = "BookmarkCountText",
                FillWeight = 60
            });

            dgvBooks.Columns.Add(new DataGridViewTextBoxColumn
            {
                HeaderText = "シリーズ",
                DataPropertyName = "Series",
                FillWeight = 110
            });

            dgvBooks.Columns.Add(new DataGridViewTextBoxColumn
            {
                HeaderText = "作者",
                DataPropertyName = "Author",
                FillWeight = 110
            });

            dgvBooks.Columns.Add(new DataGridViewTextBoxColumn
            {
                HeaderText = "ジャンル",
                DataPropertyName = "Genre",
                FillWeight = 110
            });

            dgvBooks.Columns.Add(new DataGridViewTextBoxColumn
            {
                HeaderText = "ナイスポイント",
                DataPropertyName = "NicePoint",
                FillWeight = 180
            });

            dgvBooks.Columns.Add(new DataGridViewTextBoxColumn
            {
                HeaderText = "タグ",
                DataPropertyName = "Tags",
                FillWeight = 150
            });

            dgvBooks.Columns.Add(new DataGridViewTextBoxColumn
            {
                HeaderText = "フォルダ",
                DataPropertyName = "FolderPath",
                FillWeight = 260
            });

            dgvBooks.CellDoubleClick -= dgvBooks_CellDoubleClick;
            dgvBooks.CellDoubleClick += dgvBooks_CellDoubleClick;

            dgvBooks.ColumnHeaderMouseClick -= dgvBooks_ColumnHeaderMouseClick;
            dgvBooks.ColumnHeaderMouseClick += dgvBooks_ColumnHeaderMouseClick;

            foreach (DataGridViewColumn column in dgvBooks.Columns)
            {
                column.SortMode = DataGridViewColumnSortMode.Programmatic;
            }

            SaveColumnHeaderTexts();

            EnableDataGridViewDoubleBuffering(dgvBooks);
        }

        private void EnableDataGridViewDoubleBuffering(DataGridView grid)
        {
            try
            {
                typeof(DataGridView).InvokeMember(
                    "DoubleBuffered",
                    System.Reflection.BindingFlags.NonPublic |
                    System.Reflection.BindingFlags.Instance |
                    System.Reflection.BindingFlags.SetProperty,
                    null,
                    grid,
                    new object[] { true }
                );
            }
            catch
            {
                // 環境によっては設定できない場合があるため無視
            }
        }

        private void SaveColumnHeaderTexts()
        {
            _columnHeaderTexts.Clear();

            foreach (DataGridViewColumn column in dgvBooks.Columns)
            {
                if (string.IsNullOrWhiteSpace(column.DataPropertyName))
                {
                    continue;
                }

                _columnHeaderTexts[column.DataPropertyName] = column.HeaderText;
            }
        }

        private void dgvBooks_ColumnHeaderMouseClick(object? sender, DataGridViewCellMouseEventArgs e)
        {
            if (e.ColumnIndex < 0)
            {
                return;
            }

            var column = dgvBooks.Columns[e.ColumnIndex];

            if (string.IsNullOrWhiteSpace(column.DataPropertyName))
            {
                return;
            }

            string clickedPropertyName = column.DataPropertyName;

            if (_sortPropertyName == clickedPropertyName)
            {
                _sortAscending = !_sortAscending;
            }
            else
            {
                _sortPropertyName = clickedPropertyName;
                _sortAscending = true;
            }

            RefreshGrid();
            ResetGridViewState();

            lblStatus.Text = $"{_displayBooks.Count} / {_books.Count}件表示";
        }

        private void MainForm_Shown(object? sender, EventArgs e)
        {
            if (_hasTriedAutoLoad)
            {
                return;
            }

            _hasTriedAutoLoad = true;

            LoadLastFolderIfExists();
        }

        private void LoadLastFolderIfExists()
        {
            string lastFolderPath = AppSettingsService.GetLastRootFolder();

            if (string.IsNullOrWhiteSpace(lastFolderPath))
            {
                return;
            }

            if (!Directory.Exists(lastFolderPath))
            {
                lblStatus.Text = "前回のフォルダが見つかりません";
                return;
            }

            LoadBooksFromFolder(
                lastFolderPath,
                clearSearch: true,
                statusText: "前回フォルダを読み込みました"
            );
        }

        private void LoadBooksFromFolder(string folderPath, bool clearSearch, string statusText)
        {
            _books = _bookScanService.ScanBooks(folderPath);

            _databaseService.MergeSavedData(_books);
            _databaseService.ApplyBookListStatus(_books);

            _displayBooks = _books.ToList();

            if (clearSearch)
            {
                ClearSearchFields();
                cmbSearchMode.SelectedIndex = 0;
            }

            RefreshGrid();
            ResetGridViewState();

            lblStatus.Text = $"{statusText}：{_books.Count}件";
        }

        private void btnSelectFolder_Click(object? sender, EventArgs e)
        {
            using var dialog = new FolderBrowserDialog();

            dialog.Description = "画像フォルダを選択してください";

            if (dialog.ShowDialog() != DialogResult.OK)
            {
                return;
            }

            AppSettingsService.SaveLastRootFolder(dialog.SelectedPath);

            LoadBooksFromFolder(
                dialog.SelectedPath,
                clearSearch: true,
                statusText: "画像フォルダを読み込みました"
            );
        }

        private void btnEditBook_Click(object? sender, EventArgs e)
        {
            var selectedBook = GetSelectedBook();

            if (selectedBook == null)
            {
                MessageBox.Show("編集する作品を選択してください。");
                return;
            }

            using var editForm = new EditBookForm(selectedBook);

            if (editForm.ShowDialog(this) != DialogResult.OK)
            {
                return;
            }

            _databaseService.SaveBook(selectedBook);

            ReapplySearchAndRefresh();
        }

        private void btnSearch_Click(object? sender, EventArgs e)
        {
            ApplySearch();
        }

        private void btnClearSearch_Click(object? sender, EventArgs e)
        {
            ClearSearchFields();
            cmbSearchMode.SelectedIndex = 0;

            // ソート状態を初期化する
            _sortPropertyName = "";
            _sortAscending = true;

            _displayBooks = _books.ToList();

            RefreshGrid();

            // ヘッダー文字、列幅、スクロール位置を初期状態に戻す
            ResetGridColumnLayout();
            ResetGridViewState();

            lblStatus.Text = $"{_displayBooks.Count} / {_books.Count}件表示";
        }

        private void ResetGridColumnLayout()
        {
            // ヘッダーの ▲ / ▼ を消して、元の文字に戻す
            foreach (DataGridViewColumn column in dgvBooks.Columns)
            {
                column.HeaderCell.SortGlyphDirection = SortOrder.None;

                if (!string.IsNullOrWhiteSpace(column.DataPropertyName) &&
                    _columnHeaderTexts.TryGetValue(column.DataPropertyName, out string baseHeaderText))
                {
                    column.HeaderText = baseHeaderText;
                }
            }

            // 列幅を初期状態に戻す
            foreach (DataGridViewColumn column in dgvBooks.Columns)
            {
                switch (column.DataPropertyName)
                {
                    case "Name":
                        column.FillWeight = 180;
                        break;

                    case "LastReadPageText":
                        column.FillWeight = 80;
                        break;

                    case "BookmarkCountText":
                        column.FillWeight = 60;
                        break;

                    case "Series":
                        column.FillWeight = 110;
                        break;

                    case "Author":
                        column.FillWeight = 110;
                        break;

                    case "Genre":
                        column.FillWeight = 110;
                        break;

                    case "NicePoint":
                        column.FillWeight = 180;
                        break;

                    case "Tags":
                        column.FillWeight = 150;
                        break;

                    case "FolderPath":
                        column.FillWeight = 260;
                        break;
                }
            }

            dgvBooks.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
        }

        private void ApplySearch()
        {
            if (_books.Count == 0)
            {
                MessageBox.Show("先に画像フォルダを読み込んでください。");
                return;
            }

            var mode = cmbSearchMode.Text == "AND"
                ? SearchMode.And
                : SearchMode.Or;

            _displayBooks = _bookSearchService.Search(
                _books,
                GetSearchCriteria(),
                mode
            );

            RefreshGrid();
            ResetGridViewState();

            lblStatus.Text = $"{_displayBooks.Count} / {_books.Count}件表示";
        }

        private void ReapplySearchAndRefresh()
        {
            if (_books.Count == 0)
            {
                RefreshGrid();
                ResetGridViewState();
                return;
            }

            var mode = cmbSearchMode.Text == "AND"
                ? SearchMode.And
                : SearchMode.Or;

            _displayBooks = _bookSearchService.Search(
                _books,
                GetSearchCriteria(),
                mode
            );

            RefreshGrid();
            ResetGridViewState();

            lblStatus.Text = $"{_displayBooks.Count} / {_books.Count}件表示";
        }

        private BookSearchCriteria GetSearchCriteria()
        {
            return new BookSearchCriteria
            {
                Name = txtSearchName.Text,
                Series = txtSearchSeries.Text,
                Author = txtSearchAuthor.Text,
                Genre = txtSearchGenre.Text,
                NicePoint = txtSearchNicePoint.Text,
                Tags = txtSearchTags.Text
            };
        }

        private void ClearSearchFields()
        {
            txtSearchName.Text = "";
            txtSearchSeries.Text = "";
            txtSearchAuthor.Text = "";
            txtSearchGenre.Text = "";
            txtSearchNicePoint.Text = "";
            txtSearchTags.Text = "";
        }

        private Book? GetSelectedBook()
        {
            if (dgvBooks.CurrentRow == null)
            {
                return null;
            }

            return dgvBooks.CurrentRow.DataBoundItem as Book;
        }

        private void RefreshGrid()
        {
            ApplyCurrentSort();

            dgvBooks.DataSource = null;
            dgvBooks.DataSource = _displayBooks;

            UpdateSortGlyph();

            dgvBooks.ClearSelection();
        }

        private void ApplyCurrentSort()
        {
            if (string.IsNullOrWhiteSpace(_sortPropertyName))
            {
                return;
            }

            _displayBooks = SortBooks(_displayBooks, _sortPropertyName, _sortAscending);
        }

        private List<Book> SortBooks(List<Book> books, string propertyName, bool ascending)
        {
            switch (propertyName)
            {
                case "LastReadPageText":
                    return SortByLastReadPage(books, ascending);

                case "BookmarkCountText":
                    return ascending
                        ? books.OrderBy(book => book.BookmarkCount).ToList()
                        : books.OrderByDescending(book => book.BookmarkCount).ToList();

                case "Name":
                    return SortByText(books, book => book.Name, ascending);

                case "Series":
                    return SortByText(books, book => book.Series, ascending);

                case "Author":
                    return SortByText(books, book => book.Author, ascending);

                case "Genre":
                    return SortByText(books, book => book.Genre, ascending);

                case "NicePoint":
                    return SortByText(books, book => book.NicePoint, ascending);

                case "Tags":
                    return SortByText(books, book => book.Tags, ascending);

                case "FolderPath":
                    return SortByText(books, book => book.FolderPath, ascending);

                default:
                    return books;
            }
        }

        private List<Book> SortByText(
            List<Book> books,
            Func<Book, string> selector,
            bool ascending)
        {
            var comparer = new NaturalStringComparer();

            if (ascending)
            {
                return books
                    .OrderBy(book => selector(book), comparer)
                    .ToList();
            }

            return books
                .OrderByDescending(book => selector(book), comparer)
                .ToList();
        }

        private List<Book> SortByLastReadPage(List<Book> books, bool ascending)
        {
            if (ascending)
            {
                return books
                    .OrderBy(book => book.LastReadPageIndex < 0 ? 1 : 0)
                    .ThenBy(book => book.LastReadPageIndex)
                    .ToList();
            }

            return books
                .OrderBy(book => book.LastReadPageIndex < 0 ? 1 : 0)
                .ThenByDescending(book => book.LastReadPageIndex)
                .ToList();
        }

        private void UpdateSortGlyph()
        {
            foreach (DataGridViewColumn column in dgvBooks.Columns)
            {
                column.HeaderCell.SortGlyphDirection = SortOrder.None;

                if (string.IsNullOrWhiteSpace(column.DataPropertyName))
                {
                    continue;
                }

                if (!_columnHeaderTexts.TryGetValue(column.DataPropertyName, out string baseHeaderText))
                {
                    baseHeaderText = column.HeaderText
                        .Replace(" ▲", "")
                        .Replace(" ▼", "");
                }

                column.HeaderText = baseHeaderText;
            }

            if (string.IsNullOrWhiteSpace(_sortPropertyName))
            {
                return;
            }

            foreach (DataGridViewColumn column in dgvBooks.Columns)
            {
                if (column.DataPropertyName != _sortPropertyName)
                {
                    continue;
                }

                if (!_columnHeaderTexts.TryGetValue(column.DataPropertyName, out string baseHeaderText))
                {
                    baseHeaderText = column.HeaderText
                        .Replace(" ▲", "")
                        .Replace(" ▼", "");
                }

                string mark = _sortAscending ? " ▲" : " ▼";

                column.HeaderText = baseHeaderText + mark;

                break;
            }
        }

        private void ResetGridViewState()
        {
            if (dgvBooks.Rows.Count == 0)
            {
                return;
            }

            dgvBooks.ClearSelection();

            try
            {
                dgvBooks.FirstDisplayedScrollingRowIndex = 0;

                if (dgvBooks.Columns.Count > 0)
                {
                    dgvBooks.FirstDisplayedScrollingColumnIndex = 0;
                }

                dgvBooks.CurrentCell = null;
            }
            catch
            {
                // DataGridViewの描画タイミングによって失敗することがあるため無視
            }
        }

        private void dgvBooks_CellDoubleClick(object? sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0)
            {
                return;
            }

            if (_displayBooks.Count == 0)
            {
                return;
            }

            if (e.RowIndex >= _displayBooks.Count)
            {
                return;
            }

            var viewerForm = new ViewerForm(_displayBooks, e.RowIndex);

            viewerForm.FormClosed += (closedSender, closedEventArgs) =>
            {
                _databaseService.ApplyBookListStatus(_books);
                ReapplySearchAndRefresh();
            };

            viewerForm.Show();
        }
    }
}