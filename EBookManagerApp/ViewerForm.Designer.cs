namespace EBookManagerApp
{
    partial class ViewerForm
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            pictureBoxMain = new PictureBox();
            lblPage = new Label();
            btnGoPage = new Button();
            ((System.ComponentModel.ISupportInitialize)pictureBoxMain).BeginInit();
            SuspendLayout();
            // 
            // pictureBoxMain
            // 
            pictureBoxMain.Location = new Point(12, 12);
            pictureBoxMain.Name = "pictureBoxMain";
            pictureBoxMain.Size = new Size(776, 453);
            pictureBoxMain.SizeMode = PictureBoxSizeMode.Zoom;
            pictureBoxMain.TabIndex = 0;
            pictureBoxMain.TabStop = false;
            // 
            // lblPage
            // 
            lblPage.AutoSize = true;
            lblPage.Location = new Point(377, 474);
            lblPage.Name = "lblPage";
            lblPage.Size = new Size(30, 15);
            lblPage.TabIndex = 1;
            lblPage.Text = "0 / 0";
            // 
            // btnGoPage
            // 
            btnGoPage.Location = new Point(296, 471);
            btnGoPage.Name = "btnGoPage";
            btnGoPage.Size = new Size(75, 23);
            btnGoPage.TabIndex = 3;
            btnGoPage.Text = "移動";
            btnGoPage.UseVisualStyleBackColor = true;
            btnGoPage.Click += btnGoPage_Click;
            // 
            // ViewerForm
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(800, 506);
            Controls.Add(btnGoPage);
            Controls.Add(lblPage);
            Controls.Add(pictureBoxMain);
            Name = "ViewerForm";
            Text = "ViewerForm";
            ((System.ComponentModel.ISupportInitialize)pictureBoxMain).EndInit();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private PictureBox pictureBoxMain;
        private Label lblPage;
        private Button btnGoPage;
    }
}