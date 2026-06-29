using EBookManagerApp.Models;
using EBookManagerApp.Services;

namespace EBookManagerApp
{
    public partial class EditBookForm : Form
    {
        private readonly Book _book;

        public EditBookForm(Book book)
        {
            InitializeComponent();

            _book = book;

            SetupFormSettings();
            LoadBookData();
            SetupEvents();

            AppThemeService.ApplyTheme(this);
        }

        private void SetupFormSettings()
        {
            FormBorderStyle = FormBorderStyle.FixedDialog;
            MaximizeBox = false;
            MinimizeBox = false;
            StartPosition = FormStartPosition.CenterParent;
        }

        private void LoadBookData()
        {
            txtName.Text = _book.Name;
            txtSeries.Text = _book.Series;
            txtAuthor.Text = _book.Author;
            txtGenre.Text = _book.Genre;
            txtNicePoint.Text = _book.NicePoint;
            txtTags.Text = _book.Tags;
        }

        private void SetupEvents()
        {
            btnSave.Click -= btnSave_Click;
            btnSave.Click += btnSave_Click;

            btnCancel.Click -= btnCancel_Click;
            btnCancel.Click += btnCancel_Click;
        }

        private void btnSave_Click(object? sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtName.Text))
            {
                MessageBox.Show("名前は必須です。");
                return;
            }

            _book.Name = txtName.Text.Trim();
            _book.Series = txtSeries.Text.Trim();
            _book.Author = txtAuthor.Text.Trim();
            _book.Genre = txtGenre.Text.Trim();
            _book.NicePoint = txtNicePoint.Text.Trim();
            _book.Tags = txtTags.Text.Trim();

            DialogResult = DialogResult.OK;
            Close();
        }

        private void btnCancel_Click(object? sender, EventArgs e)
        {
            DialogResult = DialogResult.Cancel;
            Close();
        }
    }
}