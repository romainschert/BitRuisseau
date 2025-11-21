namespace bitruisseau_RomainSchertenleib
{
    public partial class Form1 : Form
    {
        private static string appDirectory = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "BitRuisseau");
        private string SavefileName = "data.txt";
        public Form1()
        {
            InitializeComponent(); 
            if (!Directory.Exists(appDirectory))
            {
                Directory.CreateDirectory(appDirectory);
            }

            string SavefilePath = Path.Combine(appDirectory, SavefileName);
            txtPath.Text = File.ReadAllText(SavefilePath);
            label1.Font = new Font("Arial", 22, FontStyle.Regular);

            LoadMp3Files(txtPath.Text);
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            SaveText(txtPath.Text);
            LoadMp3Files(txtPath.Text);
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
            string filePath = Path.Combine(appDirectory, SavefileName);

            File.WriteAllText(filePath, text);
        }

        private void LoadMp3Files(string folderPath)
        {
            if (!Directory.Exists(folderPath)) return;

            dataGridView1.Rows.Clear();

            string[] files = Directory.GetFiles(folderPath, "*.mp3");
            foreach (string file in files)
            {
                try
                {
                    var tfile = TagLib.File.Create(file);

                    string titre = tfile.Tag.Title ?? Path.GetFileNameWithoutExtension(file);
                    string artiste = tfile.Tag.FirstPerformer ?? "Inconnu";
                    uint annee = tfile.Tag.Year;
                    string duration = tfile.Properties.Duration.ToString(@"mm\:ss");
                    string taille = (new FileInfo(file).Length / 1024f / 1024f).ToString("0.00") + " Mo";
                    string featuring = tfile.Tag.JoinedPerformers; // si featuring est dans performers

                    dataGridView1.Rows.Add(titre, artiste, annee, duration, taille, featuring);
                }
                catch
                {
                    continue;
                }
            }
        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void label1_Click(object sender, EventArgs e)
        {

        }
    }
}
