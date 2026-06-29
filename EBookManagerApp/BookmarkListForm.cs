using EBookManagerApp.Models;
using EBookManagerApp.Services;

namespace EBookManagerApp
{
    public partial class BookmarkListForm : Form
    {
        private readonly List<BookmarkPage> _bookmarks;

        public int SelectedPageIndex { get; private set; } = -1;

        public BookmarkListForm(List<BookmarkPage> bookmarks)
        {
            InitializeComponent();

            _bookmarks = bookmarks;

            SetupFormSettings();
            SetupDataGridView();
            SetupEvents();
            RefreshGrid();

            AppThemeService.ApplyTheme(this);

        }

        private void SetupFormSettings()
        {
            StartPosition = FormStartPosition.CenterParent;

            FormBorderStyle = FormBorderStyle.FixedDialog;
            MaximizeBox = false;
            MinimizeBox = false;

            ClientSize = new Size(784, 452);
        }

        private void SetupDataGridView()
        {
            dgvBookmarks.AutoGenerateColumns = false;
            dgvBookmarks.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dgvBookmarks.MultiSelect = false;
            dgvBookmarks.ReadOnly = true;
            dgvBookmarks.AllowUserToAddRows = false;
            dgvBookmarks.AllowUserToDeleteRows = false;
            dgvBookmarks.RowHeadersVisible = false;
            dgvBookmarks.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dgvBookmarks.BackgroundColor = Color.White;
            dgvBookmarks.BorderStyle = BorderStyle.FixedSingle;

            dgvBookmarks.Columns.Clear();

            dgvBookmarks.Columns.Add(new DataGridViewTextBoxColumn
            {
                HeaderText = "ページ",
                DataPropertyName = "PageNumber",
                FillWeight = 60
            });

            dgvBookmarks.Columns.Add(new DataGridViewTextBoxColumn
            {
                HeaderText = "メモ",
                DataPropertyName = "Memo",
                FillWeight = 260
            });

            dgvBookmarks.Columns.Add(new DataGridViewTextBoxColumn
            {
                HeaderText = "更新日時",
                DataPropertyName = "UpdatedAt",
                FillWeight = 120
            });
        }

        private void SetupEvents()
        {
            btnOpen.Click -= btnOpen_Click;
            btnOpen.Click += btnOpen_Click;

            btnClose.Click -= btnClose_Click;
            btnClose.Click += btnClose_Click;

            dgvBookmarks.CellDoubleClick -= dgvBookmarks_CellDoubleClick;
            dgvBookmarks.CellDoubleClick += dgvBookmarks_CellDoubleClick;
        }

        private void RefreshGrid()
        {
            dgvBookmarks.DataSource = null;
            dgvBookmarks.DataSource = _bookmarks;

            lblStatus.Text = $"{_bookmarks.Count}件のブックマーク";

            btnOpen.Enabled = _bookmarks.Count > 0;

            if (dgvBookmarks.Rows.Count > 0)
            {
                dgvBookmarks.Rows[0].Selected = true;
                dgvBookmarks.CurrentCell = dgvBookmarks.Rows[0].Cells[0];
            }
        }

        private BookmarkPage? GetSelectedBookmark()
        {
            if (dgvBookmarks.CurrentRow == null)
            {
                return null;
            }

            return dgvBookmarks.CurrentRow.DataBoundItem as BookmarkPage;
        }

        private void OpenSelectedBookmark()
        {
            var bookmark = GetSelectedBookmark();

            if (bookmark == null)
            {
                MessageBox.Show("移動するブックマークを選択してください。");
                return;
            }

            SelectedPageIndex = bookmark.PageIndex;

            DialogResult = DialogResult.OK;
            Close();
        }

        private void btnOpen_Click(object? sender, EventArgs e)
        {
            OpenSelectedBookmark();
        }

        private void btnClose_Click(object? sender, EventArgs e)
        {
            DialogResult = DialogResult.Cancel;
            Close();
        }

        private void dgvBookmarks_CellDoubleClick(object? sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0)
            {
                return;
            }

            OpenSelectedBookmark();
        }
    }
}