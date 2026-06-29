namespace EBookManagerApp.Services
{
    public enum AppThemeMode
    {
        Light,
        Dark
    }

    public static class AppThemeService
    {
        private static readonly string SettingsFolder = Path.Combine(
            Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData),
            "EBookManagerApp"
        );

        private static readonly string ThemeFilePath = Path.Combine(
            SettingsFolder,
            "theme.txt"
        );

        public static AppThemeMode CurrentTheme { get; private set; } = AppThemeMode.Light;

        public static bool IsDarkMode => CurrentTheme == AppThemeMode.Dark;

        public static Color FormBackColor =>
            IsDarkMode ? Color.FromArgb(30, 30, 30) : SystemColors.Control;

        public static Color PanelBackColor =>
            IsDarkMode ? Color.FromArgb(37, 37, 38) : SystemColors.Control;

        public static Color ControlBackColor =>
            IsDarkMode ? Color.FromArgb(45, 45, 48) : Color.White;

        public static Color TextColor =>
            IsDarkMode ? Color.White : Color.Black;

        public static Color SubTextColor =>
            IsDarkMode ? Color.FromArgb(235, 235, 235) : Color.DimGray;

        public static Color BorderColor =>
            IsDarkMode ? Color.FromArgb(90, 90, 90) : Color.Gray;

        public static Color AccentColor =>
            IsDarkMode ? Color.DeepSkyBlue : Color.DodgerBlue;

        public static Color GridBackColor =>
            IsDarkMode ? Color.FromArgb(24, 24, 24) : Color.White;

        public static Color GridAltBackColor =>
            IsDarkMode ? Color.FromArgb(32, 32, 32) : Color.FromArgb(245, 245, 245);

        public static Color GridHeaderBackColor =>
            IsDarkMode ? Color.FromArgb(42, 42, 42) : SystemColors.Control;

        public static Color SelectionBackColor =>
            IsDarkMode ? Color.FromArgb(0, 120, 215) : SystemColors.Highlight;

        public static Color SelectionForeColor =>
            Color.White;

        public static void Load()
        {
            Directory.CreateDirectory(SettingsFolder);

            if (!File.Exists(ThemeFilePath))
            {
                CurrentTheme = AppThemeMode.Light;
                return;
            }

            string text = File.ReadAllText(ThemeFilePath).Trim();

            if (string.Equals(text, "Dark", StringComparison.OrdinalIgnoreCase))
            {
                CurrentTheme = AppThemeMode.Dark;
            }
            else
            {
                CurrentTheme = AppThemeMode.Light;
            }
        }

        public static void SetTheme(AppThemeMode theme)
        {
            CurrentTheme = theme;

            Directory.CreateDirectory(SettingsFolder);
            File.WriteAllText(ThemeFilePath, theme.ToString());
        }

        public static void ApplyThemeToOpenForms()
        {
            var forms = new List<Form>();

            foreach (Form form in Application.OpenForms)
            {
                forms.Add(form);
            }

            foreach (var form in forms)
            {
                ApplyTheme(form);
            }
        }

        public static void ApplyTheme(Form form)
        {
            form.BackColor = FormBackColor;
            form.ForeColor = TextColor;

            ApplyThemeToControl(form);

            form.Invalidate(true);
        }

        private static void ApplyThemeToControl(Control control)
        {
            if (control is Form)
            {
                control.BackColor = FormBackColor;
                control.ForeColor = TextColor;
            }
            else if (control is DataGridView grid)
            {
                ApplyThemeToDataGridView(grid);
            }
            else if (control is TextBoxBase)
            {
                control.BackColor = ControlBackColor;
                control.ForeColor = TextColor;
            }
            else if (control is ComboBox)
            {
                control.BackColor = ControlBackColor;
                control.ForeColor = TextColor;
            }
            else if (control is Button button)
            {
                // ボタンはライト/ダークで見やすい色にする
                button.UseVisualStyleBackColor = false;

                if (IsDarkMode)
                {
                    button.BackColor = Color.FromArgb(85, 85, 85);
                    button.ForeColor = Color.White;
                }
                else
                {
                    button.BackColor = Color.White;
                    button.ForeColor = Color.Black;
                }

                button.FlatStyle = FlatStyle.Standard;
            }

            else if (control is Label)
            {
                control.BackColor = Color.Transparent;
                control.ForeColor = TextColor;
            }
            else if (control is Panel || control is TableLayoutPanel)
            {
                control.BackColor = PanelBackColor;
                control.ForeColor = TextColor;
            }
            else if (control is PictureBox)
            {
                control.BackColor = IsDarkMode ? Color.Black : Color.White;
            }
            else
            {
                control.BackColor = FormBackColor;
                control.ForeColor = TextColor;
            }

            foreach (Control child in control.Controls)
            {
                ApplyThemeToControl(child);
            }
        }

        private static void ApplyThemeToDataGridView(DataGridView grid)
        {
            grid.BackgroundColor = GridBackColor;
            grid.GridColor = BorderColor;
            grid.ForeColor = TextColor;

            grid.EnableHeadersVisualStyles = false;

            grid.ColumnHeadersDefaultCellStyle.BackColor = GridHeaderBackColor;
            grid.ColumnHeadersDefaultCellStyle.ForeColor = TextColor;
            grid.ColumnHeadersDefaultCellStyle.SelectionBackColor = GridHeaderBackColor;
            grid.ColumnHeadersDefaultCellStyle.SelectionForeColor = TextColor;

            grid.DefaultCellStyle.BackColor = GridBackColor;
            grid.DefaultCellStyle.ForeColor = TextColor;
            grid.DefaultCellStyle.SelectionBackColor = SelectionBackColor;
            grid.DefaultCellStyle.SelectionForeColor = SelectionForeColor;

            grid.AlternatingRowsDefaultCellStyle.BackColor = GridAltBackColor;
            grid.AlternatingRowsDefaultCellStyle.ForeColor = TextColor;
            grid.AlternatingRowsDefaultCellStyle.SelectionBackColor = SelectionBackColor;
            grid.AlternatingRowsDefaultCellStyle.SelectionForeColor = SelectionForeColor;

            grid.RowHeadersDefaultCellStyle.BackColor = GridHeaderBackColor;
            grid.RowHeadersDefaultCellStyle.ForeColor = TextColor;
        }
    }
}