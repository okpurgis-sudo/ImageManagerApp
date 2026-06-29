using EBookManagerApp.Services;

namespace EBookManagerApp
{
    public partial class BookmarkMemoForm : Form
    {
        public string MemoText { get; private set; } = "";
        public bool IsDeleteRequested { get; private set; } = false;

        public BookmarkMemoForm(string currentMemo)
        {
            InitializeComponent();

            SetupFormSettings();

            txtMemo.Text = currentMemo;

            AppThemeService.ApplyTheme(this);
        }

        private void SetupFormSettings()
        {
            FormBorderStyle = FormBorderStyle.FixedDialog;
            MaximizeBox = false;
            MinimizeBox = false;
            StartPosition = FormStartPosition.CenterParent;
        }

        private void btnSave_Click(object? sender, EventArgs e)
        {
            MemoText = txtMemo.Text.Trim();
            IsDeleteRequested = false;

            DialogResult = DialogResult.OK;
            Close();
        }

        private void btnDelete_Click(object? sender, EventArgs e)
        {
            var result = MessageBox.Show(
                "このページのブックマークを削除しますか？",
                "ブックマーク削除",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Question
            );

            if (result != DialogResult.Yes)
            {
                return;
            }

            IsDeleteRequested = true;

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