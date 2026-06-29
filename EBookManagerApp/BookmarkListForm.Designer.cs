namespace EBookManagerApp
{
    partial class BookmarkListForm
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
            dgvBookmarks = new DataGridView();
            lblStatus = new Label();
            btnOpen = new Button();
            btnClose = new Button();
            mainLayout = new TableLayoutPanel();
            bottomPanel = new Panel();
            ((System.ComponentModel.ISupportInitialize)dgvBookmarks).BeginInit();
            mainLayout.SuspendLayout();
            bottomPanel.SuspendLayout();
            SuspendLayout();
            // 
            // dgvBookmarks
            // 
            dgvBookmarks.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dgvBookmarks.Dock = DockStyle.Fill;
            dgvBookmarks.Location = new Point(11, 11);
            dgvBookmarks.Name = "dgvBookmarks";
            dgvBookmarks.Size = new Size(762, 386);
            dgvBookmarks.TabIndex = 0;
            // 
            // lblStatus
            // 
            lblStatus.AutoSize = true;
            lblStatus.Location = new Point(8, 13);
            lblStatus.Name = "lblStatus";
            lblStatus.Size = new Size(86, 15);
            lblStatus.TabIndex = 1;
            lblStatus.Text = "0件のブックマーク";
            // 
            // btnOpen
            // 
            btnOpen.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            btnOpen.Location = new Point(575, 7);
            btnOpen.Name = "btnOpen";
            btnOpen.Size = new Size(90, 28);
            btnOpen.TabIndex = 2;
            btnOpen.Text = "開く";
            btnOpen.UseVisualStyleBackColor = true;
            // 
            // btnClose
            // 
            btnClose.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            btnClose.Location = new Point(671, 7);
            btnClose.Name = "btnClose";
            btnClose.Size = new Size(90, 28);
            btnClose.TabIndex = 3;
            btnClose.Text = "閉じる";
            btnClose.UseVisualStyleBackColor = true;
            // 
            // mainLayout
            // 
            mainLayout.ColumnCount = 1;
            mainLayout.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));
            mainLayout.Controls.Add(dgvBookmarks, 0, 0);
            mainLayout.Controls.Add(bottomPanel, 0, 1);
            mainLayout.Dock = DockStyle.Fill;
            mainLayout.Location = new Point(0, 0);
            mainLayout.Name = "mainLayout";
            mainLayout.Padding = new Padding(8);
            mainLayout.RowCount = 2;
            mainLayout.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));
            mainLayout.RowStyles.Add(new RowStyle(SizeType.Absolute, 44F));
            mainLayout.Size = new Size(784, 452);
            mainLayout.TabIndex = 4;
            // 
            // bottomPanel
            // 
            bottomPanel.Controls.Add(lblStatus);
            bottomPanel.Controls.Add(btnOpen);
            bottomPanel.Controls.Add(btnClose);
            bottomPanel.Dock = DockStyle.Fill;
            bottomPanel.Location = new Point(11, 403);
            bottomPanel.Name = "bottomPanel";
            bottomPanel.Size = new Size(762, 38);
            bottomPanel.TabIndex = 5;
            // 
            // BookmarkListForm
            // 
            AcceptButton = btnOpen;
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            CancelButton = btnClose;
            ClientSize = new Size(784, 452);
            Controls.Add(mainLayout);
            MinimumSize = new Size(700, 420);
            Name = "BookmarkListForm";
            StartPosition = FormStartPosition.CenterParent;
            Text = "ブックマーク一覧";
            ((System.ComponentModel.ISupportInitialize)dgvBookmarks).EndInit();
            mainLayout.ResumeLayout(false);
            bottomPanel.ResumeLayout(false);
            bottomPanel.PerformLayout();
            ResumeLayout(false);
        }

        #endregion

        private DataGridView dgvBookmarks;
        private Label lblStatus;
        private Button btnOpen;
        private Button btnClose;
        private TableLayoutPanel mainLayout;
        private Panel bottomPanel;
    }
}