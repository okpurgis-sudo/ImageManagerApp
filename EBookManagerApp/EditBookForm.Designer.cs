namespace EBookManagerApp
{
    partial class EditBookForm
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
            lblName = new Label();
            txtName = new TextBox();
            lblSeries = new Label();
            txtSeries = new TextBox();
            lblAuthor = new Label();
            txtAuthor = new TextBox();
            lblGenre = new Label();
            txtGenre = new TextBox();
            lblNicePoint = new Label();
            txtNicePoint = new TextBox();
            lblTags = new Label();
            txtTags = new TextBox();
            btnSave = new Button();
            btnCancel = new Button();
            mainLayout = new TableLayoutPanel();
            buttonPanel = new FlowLayoutPanel();
            mainLayout.SuspendLayout();
            buttonPanel.SuspendLayout();
            SuspendLayout();
            // 
            // lblName
            // 
            lblName.Dock = DockStyle.Fill;
            lblName.Location = new Point(13, 10);
            lblName.Name = "lblName";
            lblName.Size = new Size(104, 34);
            lblName.TabIndex = 0;
            lblName.Text = "名前";
            lblName.TextAlign = ContentAlignment.MiddleLeft;
            // 
            // txtName
            // 
            txtName.Dock = DockStyle.Fill;
            txtName.Location = new Point(123, 15);
            txtName.Margin = new Padding(3, 5, 3, 3);
            txtName.Name = "txtName";
            txtName.Size = new Size(449, 23);
            txtName.TabIndex = 1;
            // 
            // lblSeries
            // 
            lblSeries.Dock = DockStyle.Fill;
            lblSeries.Location = new Point(13, 44);
            lblSeries.Name = "lblSeries";
            lblSeries.Size = new Size(104, 34);
            lblSeries.TabIndex = 2;
            lblSeries.Text = "シリーズ";
            lblSeries.TextAlign = ContentAlignment.MiddleLeft;
            // 
            // txtSeries
            // 
            txtSeries.Dock = DockStyle.Fill;
            txtSeries.Location = new Point(123, 49);
            txtSeries.Margin = new Padding(3, 5, 3, 3);
            txtSeries.Name = "txtSeries";
            txtSeries.Size = new Size(449, 23);
            txtSeries.TabIndex = 3;
            // 
            // lblAuthor
            // 
            lblAuthor.Dock = DockStyle.Fill;
            lblAuthor.Location = new Point(13, 78);
            lblAuthor.Name = "lblAuthor";
            lblAuthor.Size = new Size(104, 34);
            lblAuthor.TabIndex = 4;
            lblAuthor.Text = "作者";
            lblAuthor.TextAlign = ContentAlignment.MiddleLeft;
            // 
            // txtAuthor
            // 
            txtAuthor.Dock = DockStyle.Fill;
            txtAuthor.Location = new Point(123, 83);
            txtAuthor.Margin = new Padding(3, 5, 3, 3);
            txtAuthor.Name = "txtAuthor";
            txtAuthor.Size = new Size(449, 23);
            txtAuthor.TabIndex = 5;
            // 
            // lblGenre
            // 
            lblGenre.Dock = DockStyle.Fill;
            lblGenre.Location = new Point(13, 112);
            lblGenre.Name = "lblGenre";
            lblGenre.Size = new Size(104, 34);
            lblGenre.TabIndex = 6;
            lblGenre.Text = "ジャンル";
            lblGenre.TextAlign = ContentAlignment.MiddleLeft;
            // 
            // txtGenre
            // 
            txtGenre.Dock = DockStyle.Fill;
            txtGenre.Location = new Point(123, 117);
            txtGenre.Margin = new Padding(3, 5, 3, 3);
            txtGenre.Name = "txtGenre";
            txtGenre.Size = new Size(449, 23);
            txtGenre.TabIndex = 7;
            // 
            // lblNicePoint
            // 
            lblNicePoint.Dock = DockStyle.Fill;
            lblNicePoint.Location = new Point(13, 146);
            lblNicePoint.Name = "lblNicePoint";
            lblNicePoint.Size = new Size(104, 100);
            lblNicePoint.TabIndex = 8;
            lblNicePoint.Text = "ナイスポイント";
            lblNicePoint.TextAlign = ContentAlignment.MiddleLeft;
            // 
            // txtNicePoint
            // 
            txtNicePoint.Dock = DockStyle.Fill;
            txtNicePoint.Location = new Point(123, 151);
            txtNicePoint.Margin = new Padding(3, 5, 3, 3);
            txtNicePoint.Multiline = true;
            txtNicePoint.Name = "txtNicePoint";
            txtNicePoint.ScrollBars = ScrollBars.Vertical;
            txtNicePoint.Size = new Size(449, 92);
            txtNicePoint.TabIndex = 9;
            // 
            // lblTags
            // 
            lblTags.Dock = DockStyle.Fill;
            lblTags.Location = new Point(13, 246);
            lblTags.Name = "lblTags";
            lblTags.Size = new Size(104, 34);
            lblTags.TabIndex = 10;
            lblTags.Text = "タグ";
            lblTags.TextAlign = ContentAlignment.MiddleLeft;
            // 
            // txtTags
            // 
            txtTags.Dock = DockStyle.Fill;
            txtTags.Location = new Point(123, 251);
            txtTags.Margin = new Padding(3, 5, 3, 3);
            txtTags.Name = "txtTags";
            txtTags.Size = new Size(449, 23);
            txtTags.TabIndex = 11;
            // 
            // btnSave
            // 
            btnSave.Location = new Point(258, 6);
            btnSave.Margin = new Padding(3, 6, 8, 3);
            btnSave.Name = "btnSave";
            btnSave.Size = new Size(90, 28);
            btnSave.TabIndex = 12;
            btnSave.Text = "保存";
            btnSave.UseVisualStyleBackColor = true;
            // 
            // btnCancel
            // 
            btnCancel.Location = new Point(359, 6);
            btnCancel.Margin = new Padding(3, 6, 0, 3);
            btnCancel.Name = "btnCancel";
            btnCancel.Size = new Size(90, 28);
            btnCancel.TabIndex = 13;
            btnCancel.Text = "キャンセル";
            btnCancel.UseVisualStyleBackColor = true;
            // 
            // mainLayout
            // 
            mainLayout.ColumnCount = 2;
            mainLayout.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 110F));
            mainLayout.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));
            mainLayout.Controls.Add(lblName, 0, 0);
            mainLayout.Controls.Add(txtName, 1, 0);
            mainLayout.Controls.Add(lblSeries, 0, 1);
            mainLayout.Controls.Add(txtSeries, 1, 1);
            mainLayout.Controls.Add(lblAuthor, 0, 2);
            mainLayout.Controls.Add(txtAuthor, 1, 2);
            mainLayout.Controls.Add(lblGenre, 0, 3);
            mainLayout.Controls.Add(txtGenre, 1, 3);
            mainLayout.Controls.Add(lblNicePoint, 0, 4);
            mainLayout.Controls.Add(txtNicePoint, 1, 4);
            mainLayout.Controls.Add(lblTags, 0, 5);
            mainLayout.Controls.Add(txtTags, 1, 5);
            mainLayout.Controls.Add(buttonPanel, 1, 6);
            mainLayout.Dock = DockStyle.Fill;
            mainLayout.Location = new Point(0, 0);
            mainLayout.Name = "mainLayout";
            mainLayout.Padding = new Padding(10);
            mainLayout.RowCount = 7;
            mainLayout.RowStyles.Add(new RowStyle(SizeType.Absolute, 34F));
            mainLayout.RowStyles.Add(new RowStyle(SizeType.Absolute, 34F));
            mainLayout.RowStyles.Add(new RowStyle(SizeType.Absolute, 34F));
            mainLayout.RowStyles.Add(new RowStyle(SizeType.Absolute, 34F));
            mainLayout.RowStyles.Add(new RowStyle(SizeType.Absolute, 100F));
            mainLayout.RowStyles.Add(new RowStyle(SizeType.Absolute, 34F));
            mainLayout.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));
            mainLayout.Size = new Size(585, 330);
            mainLayout.TabIndex = 14;
            // 
            // buttonPanel
            // 
            buttonPanel.Controls.Add(btnCancel);
            buttonPanel.Controls.Add(btnSave);
            buttonPanel.Dock = DockStyle.Fill;
            buttonPanel.FlowDirection = FlowDirection.RightToLeft;
            buttonPanel.Location = new Point(123, 283);
            buttonPanel.Name = "buttonPanel";
            buttonPanel.Size = new Size(449, 34);
            buttonPanel.TabIndex = 15;
            buttonPanel.WrapContents = false;
            // 
            // EditBookForm
            // 
            AcceptButton = btnSave;
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            CancelButton = btnCancel;
            ClientSize = new Size(585, 330);
            Controls.Add(mainLayout);
            FormBorderStyle = FormBorderStyle.FixedDialog;
            MaximizeBox = false;
            MinimizeBox = false;
            MinimumSize = new Size(520, 360);
            Name = "EditBookForm";
            StartPosition = FormStartPosition.CenterParent;
            Text = "作品情報の編集";
            mainLayout.ResumeLayout(false);
            mainLayout.PerformLayout();
            buttonPanel.ResumeLayout(false);
            ResumeLayout(false);
        }

        #endregion

        private Label lblName;
        private TextBox txtName;
        private Label lblSeries;
        private TextBox txtSeries;
        private Label lblAuthor;
        private TextBox txtAuthor;
        private Label lblGenre;
        private TextBox txtGenre;
        private Label lblNicePoint;
        private TextBox txtNicePoint;
        private Label lblTags;
        private TextBox txtTags;
        private Button btnSave;
        private Button btnCancel;
        private TableLayoutPanel mainLayout;
        private FlowLayoutPanel buttonPanel;
    }
}