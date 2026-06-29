namespace EBookManagerApp
{
    partial class BookmarkMemoForm
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
            lblMemo = new Label();
            txtMemo = new TextBox();
            btnSave = new Button();
            btnDelete = new Button();
            btnCancel = new Button();
            mainLayout = new TableLayoutPanel();
            buttonPanel = new FlowLayoutPanel();
            mainLayout.SuspendLayout();
            buttonPanel.SuspendLayout();
            SuspendLayout();
            // 
            // lblMemo
            // 
            lblMemo.Dock = DockStyle.Fill;
            lblMemo.Location = new Point(13, 10);
            lblMemo.Name = "lblMemo";
            lblMemo.Size = new Size(459, 30);
            lblMemo.TabIndex = 0;
            lblMemo.Text = "ブックマークメモ";
            lblMemo.TextAlign = ContentAlignment.MiddleLeft;
            // 
            // txtMemo
            // 
            txtMemo.Dock = DockStyle.Fill;
            txtMemo.Location = new Point(13, 43);
            txtMemo.Multiline = true;
            txtMemo.Name = "txtMemo";
            txtMemo.ScrollBars = ScrollBars.Vertical;
            txtMemo.Size = new Size(459, 154);
            txtMemo.TabIndex = 1;
            // 
            // btnSave
            // 
            btnSave.Location = new Point(189, 6);
            btnSave.Margin = new Padding(3, 6, 8, 3);
            btnSave.Name = "btnSave";
            btnSave.Size = new Size(80, 28);
            btnSave.TabIndex = 2;
            btnSave.Text = "保存";
            btnSave.UseVisualStyleBackColor = true;
            btnSave.Click += btnSave_Click;
            // 
            // btnDelete
            // 
            btnDelete.Location = new Point(280, 6);
            btnDelete.Margin = new Padding(3, 6, 8, 3);
            btnDelete.Name = "btnDelete";
            btnDelete.Size = new Size(80, 28);
            btnDelete.TabIndex = 3;
            btnDelete.Text = "削除";
            btnDelete.UseVisualStyleBackColor = true;
            btnDelete.Click += btnDelete_Click;
            // 
            // btnCancel
            // 
            btnCancel.Location = new Point(371, 6);
            btnCancel.Margin = new Padding(3, 6, 0, 3);
            btnCancel.Name = "btnCancel";
            btnCancel.Size = new Size(80, 28);
            btnCancel.TabIndex = 4;
            btnCancel.Text = "キャンセル";
            btnCancel.UseVisualStyleBackColor = true;
            btnCancel.Click += btnCancel_Click;
            // 
            // mainLayout
            // 
            mainLayout.ColumnCount = 1;
            mainLayout.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));
            mainLayout.Controls.Add(lblMemo, 0, 0);
            mainLayout.Controls.Add(txtMemo, 0, 1);
            mainLayout.Controls.Add(buttonPanel, 0, 2);
            mainLayout.Dock = DockStyle.Fill;
            mainLayout.Location = new Point(0, 0);
            mainLayout.Name = "mainLayout";
            mainLayout.Padding = new Padding(10);
            mainLayout.RowCount = 3;
            mainLayout.RowStyles.Add(new RowStyle(SizeType.Absolute, 30F));
            mainLayout.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));
            mainLayout.RowStyles.Add(new RowStyle(SizeType.Absolute, 42F));
            mainLayout.Size = new Size(485, 252);
            mainLayout.TabIndex = 5;
            // 
            // buttonPanel
            // 
            buttonPanel.Controls.Add(btnCancel);
            buttonPanel.Controls.Add(btnDelete);
            buttonPanel.Controls.Add(btnSave);
            buttonPanel.Dock = DockStyle.Fill;
            buttonPanel.FlowDirection = FlowDirection.RightToLeft;
            buttonPanel.Location = new Point(13, 203);
            buttonPanel.Name = "buttonPanel";
            buttonPanel.Size = new Size(459, 36);
            buttonPanel.TabIndex = 6;
            buttonPanel.WrapContents = false;
            // 
            // BookmarkMemoForm
            // 
            AcceptButton = btnSave;
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            CancelButton = btnCancel;
            ClientSize = new Size(485, 252);
            Controls.Add(mainLayout);
            FormBorderStyle = FormBorderStyle.FixedDialog;
            MaximizeBox = false;
            MinimizeBox = false;
            Name = "BookmarkMemoForm";
            StartPosition = FormStartPosition.CenterParent;
            Text = "ブックマーク";
            mainLayout.ResumeLayout(false);
            mainLayout.PerformLayout();
            buttonPanel.ResumeLayout(false);
            ResumeLayout(false);
        }

        #endregion

        private Label lblMemo;
        private TextBox txtMemo;
        private Button btnSave;
        private Button btnDelete;
        private Button btnCancel;
        private TableLayoutPanel mainLayout;
        private FlowLayoutPanel buttonPanel;
    }
}