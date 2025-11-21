namespace bitruisseau_RomainSchertenleib
{ 
    public partial class Form1 : Form
    {
        private static string appDirectory = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "BitRuisseau");
        private string selectedDirectory = appDirectory;
        private string fileName = "data.txt";
        public Form1()
        {
            InitializeComponent();
            string filePath = Path.Combine(selectedDirectory, fileName);
            txtPath.Text = File.ReadAllText(filePath);
            if (!Directory.Exists(appDirectory))
            {
                Directory.CreateDirectory(appDirectory);
            }

        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            SaveText(txtPath.Text);
        }

        private void btnModifier_Click(object sender, EventArgs e)
        {
            using (FolderBrowserDialog dialog = new FolderBrowserDialog())
            {
                dialog.Description = "Choisissez un dossier";

                if (dialog.ShowDialog() == DialogResult.OK)
                {
                    txtPath.Text = dialog.SelectedPath;

                   
                }
            }
        }
        private void SaveText(string text)
        {
            string filePath = Path.Combine(selectedDirectory, fileName);

            if (!Directory.Exists(selectedDirectory))
                Directory.CreateDirectory(selectedDirectory);

            File.WriteAllText(filePath, text);
        }
    }
}
