namespace EBookManagerApp
{
    partial class MainForm
    {
        private System.ComponentModel.IContainer components = null;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }

            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        private void InitializeComponent()
        {
            btnSelectFolder = new Button();
            btnEditBook = new Button();
            lblStatus = new Label();
            lblSearchName = new Label();
            txtSearchName = new TextBox();
            lblSearchSeries = new Label();
            txtSearchSeries = new TextBox();
            lblSearchAuthor = new Label();
            txtSearchAuthor = new TextBox();
            lblSearchGenre = new Label();
            txtSearchGenre = new TextBox();
            lblSearchNicePoint = new Label();
            txtSearchNicePoint = new TextBox();
            lblSearchTags = new Label();
            txtSearchTags = new TextBox();
            cmbSearchMode = new ComboBox();
            btnSearch = new Button();
            btnClearSearch = new Button();
            dgvBooks = new DataGridView();
            ((System.ComponentModel.ISupportInitialize)dgvBooks).BeginInit();
            SuspendLayout();
            // 
            // btnSelectFolder
            // 
            btnSelectFolder.Location = new Point(8, 8);
            btnSelectFolder.Name = "btnSelectFolder";
            btnSelectFolder.Size = new Size(110, 28);
            btnSelectFolder.TabIndex = 0;
            btnSelectFolder.Text = "フォルダ読込";
            btnSelectFolder.UseVisualStyleBackColor = true;
            // 
            // btnEditBook
            // 
            btnEditBook.Location = new Point(126, 8);
            btnEditBook.Name = "btnEditBook";
            btnEditBook.Size = new Size(80, 28);
            btnEditBook.TabIndex = 1;
            btnEditBook.Text = "編集";
            btnEditBook.UseVisualStyleBackColor = true;
            // 
            // lblStatus
            // 
            lblStatus.AutoSize = true;
            lblStatus.Location = new Point(214, 14);
            lblStatus.Name = "lblStatus";
            lblStatus.Size = new Size(43, 15);
            lblStatus.TabIndex = 2;
            lblStatus.Text = "未読込";
            // 
            // lblSearchName
            // 
            lblSearchName.AutoSize = true;
            lblSearchName.Location = new Point(8, 52);
            lblSearchName.Name = "lblSearchName";
            lblSearchName.Size = new Size(31, 15);
            lblSearchName.TabIndex = 3;
            lblSearchName.Text = "名前";
            // 
            // txtSearchName
            // 
            txtSearchName.Location = new Point(47, 48);
            txtSearchName.Name = "txtSearchName";
            txtSearchName.Size = new Size(180, 23);
            txtSearchName.TabIndex = 4;
            // 
            // lblSearchSeries
            // 
            lblSearchSeries.AutoSize = true;
            lblSearchSeries.Location = new Point(235, 52);
            lblSearchSeries.Name = "lblSearchSeries";
            lblSearchSeries.Size = new Size(43, 15);
            lblSearchSeries.TabIndex = 5;
            lblSearchSeries.Text = "シリーズ";
            // 
            // txtSearchSeries
            // 
            txtSearchSeries.Location = new Point(286, 48);
            txtSearchSeries.Name = "txtSearchSeries";
            txtSearchSeries.Size = new Size(180, 23);
            txtSearchSeries.TabIndex = 6;
            // 
            // lblSearchAuthor
            // 
            lblSearchAuthor.AutoSize = true;
            lblSearchAuthor.Location = new Point(474, 52);
            lblSearchAuthor.Name = "lblSearchAuthor";
            lblSearchAuthor.Size = new Size(31, 15);
            lblSearchAuthor.TabIndex = 7;
            lblSearchAuthor.Text = "作者";
            // 
            // txtSearchAuthor
            // 
            txtSearchAuthor.Location = new Point(513, 48);
            txtSearchAuthor.Name = "txtSearchAuthor";
            txtSearchAuthor.Size = new Size(180, 23);
            txtSearchAuthor.TabIndex = 8;
            // 
            // lblSearchGenre
            // 
            lblSearchGenre.AutoSize = true;
            lblSearchGenre.Location = new Point(8, 88);
            lblSearchGenre.Name = "lblSearchGenre";
            lblSearchGenre.Size = new Size(44, 15);
            lblSearchGenre.TabIndex = 9;
            lblSearchGenre.Text = "ジャンル";
            // 
            // txtSearchGenre
            // 
            txtSearchGenre.Location = new Point(60, 84);
            txtSearchGenre.Name = "txtSearchGenre";
            txtSearchGenre.Size = new Size(150, 23);
            txtSearchGenre.TabIndex = 10;
            // 
            // lblSearchTags
            // 
            lblSearchTags.AutoSize = true;
            lblSearchTags.Location = new Point(218, 88);
            lblSearchTags.Name = "lblSearchTags";
            lblSearchTags.Size = new Size(26, 15);
            lblSearchTags.TabIndex = 11;
            lblSearchTags.Text = "タグ";
            // 
            // txtSearchTags
            // 
            txtSearchTags.Location = new Point(252, 84);
            txtSearchTags.Name = "txtSearchTags";
            txtSearchTags.Size = new Size(150, 23);
            txtSearchTags.TabIndex = 12;
            // 
            // lblSearchNicePoint
            // 
            lblSearchNicePoint.AutoSize = true;
            lblSearchNicePoint.Location = new Point(410, 88);
            lblSearchNicePoint.Name = "lblSearchNicePoint";
            lblSearchNicePoint.Size = new Size(81, 15);
            lblSearchNicePoint.TabIndex = 13;
            lblSearchNicePoint.Text = "ナイスポイント";
            // 
            // txtSearchNicePoint
            // 
            txtSearchNicePoint.Location = new Point(499, 84);
            txtSearchNicePoint.Name = "txtSearchNicePoint";
            txtSearchNicePoint.Size = new Size(250, 23);
            txtSearchNicePoint.TabIndex = 14;
            // 
            // cmbSearchMode
            // 
            cmbSearchMode.DropDownStyle = ComboBoxStyle.DropDownList;
            cmbSearchMode.FormattingEnabled = true;
            cmbSearchMode.Location = new Point(756, 84);
            cmbSearchMode.Name = "cmbSearchMode";
            cmbSearchMode.Size = new Size(70, 23);
            cmbSearchMode.TabIndex = 15;
            // 
            // btnSearch
            // 
            btnSearch.Location = new Point(834, 84);
            btnSearch.Name = "btnSearch";
            btnSearch.Size = new Size(75, 28);
            btnSearch.TabIndex = 16;
            btnSearch.Text = "検索";
            btnSearch.UseVisualStyleBackColor = true;
            // 
            // btnClearSearch
            // 
            btnClearSearch.Location = new Point(917, 84);
            btnClearSearch.Name = "btnClearSearch";
            btnClearSearch.Size = new Size(75, 28);
            btnClearSearch.TabIndex = 17;
            btnClearSearch.Text = "クリア";
            btnClearSearch.UseVisualStyleBackColor = true;
            // 
            // dgvBooks
            // 
            dgvBooks.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dgvBooks.Location = new Point(8, 124);
            dgvBooks.Name = "dgvBooks";
            dgvBooks.Size = new Size(984, 488);
            dgvBooks.TabIndex = 18;
            // 
            // MainForm
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(1000, 620);
            Controls.Add(dgvBooks);
            Controls.Add(btnClearSearch);
            Controls.Add(btnSearch);
            Controls.Add(cmbSearchMode);
            Controls.Add(txtSearchNicePoint);
            Controls.Add(lblSearchNicePoint);
            Controls.Add(txtSearchTags);
            Controls.Add(lblSearchTags);
            Controls.Add(txtSearchGenre);
            Controls.Add(lblSearchGenre);
            Controls.Add(txtSearchAuthor);
            Controls.Add(lblSearchAuthor);
            Controls.Add(txtSearchSeries);
            Controls.Add(lblSearchSeries);
            Controls.Add(txtSearchName);
            Controls.Add(lblSearchName);
            Controls.Add(lblStatus);
            Controls.Add(btnEditBook);
            Controls.Add(btnSelectFolder);
            MinimumSize = new Size(1000, 620);
            Name = "MainForm";
            Text = "電子書籍管理";
            ((System.ComponentModel.ISupportInitialize)dgvBooks).EndInit();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private Button btnSelectFolder;
        private Button btnEditBook;
        private Label lblStatus;
        private Label lblSearchName;
        private TextBox txtSearchName;
        private Label lblSearchSeries;
        private TextBox txtSearchSeries;
        private Label lblSearchAuthor;
        private TextBox txtSearchAuthor;
        private Label lblSearchGenre;
        private TextBox txtSearchGenre;
        private Label lblSearchNicePoint;
        private TextBox txtSearchNicePoint;
        private Label lblSearchTags;
        private TextBox txtSearchTags;
        private ComboBox cmbSearchMode;
        private Button btnSearch;
        private Button btnClearSearch;
        private DataGridView dgvBooks;
    }
}