using BitRuisseau;

namespace bitruisseau_RomainSchertenleib
{
    public partial class Form1 : Form
    {
        private static string appDirectory = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "BitRuisseau");
        private string SavefileName = "data.txt";
        private Protocol _protocol;
        public Form1(Protocol protocol)
        {
            InitializeComponent();
            _protocol = protocol;
            if (!Directory.Exists(appDirectory))
            {
                Directory.CreateDirectory(appDirectory);

            }

            string SavefilePath = Path.Combine(appDirectory, SavefileName);
            txtPath.Text = File.ReadAllText(SavefilePath);
            label1.Font = new Font("Arial", 22, FontStyle.Regular);
            _protocol.CatalogReceived += Protocol_CatalogReceived;
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
        private void Protocol_CatalogReceived(List<ISong> catalog)
        {
            // Vérifie si l'appel vient d'un autre thread
            if (this.dataGridView2.InvokeRequired)
            {
                // Utilise Invoke pour exécuter l'opération sur le thread UI
                this.dataGridView2.Invoke(new Action(() => DisplayCatalog(catalog)));
            }
            else
            {
                // Appelle directement si on est déjà sur le thread UI
                DisplayCatalog(catalog);
            }
        }
        private void DisplayCatalog(List<ISong> catalog)
        {

            foreach (var song in catalog)
            {
                // Extrait les informations de l'objet ISong (vous pourriez avoir besoin de caster)
                var songImpl = song as song; // Si vous avez une implémentation concrète 'song'

                string titre = song.Title ?? "Inconnu";
                string artiste = song.Artist ?? "Inconnu";
                uint annee = (uint)song.Year;
                string duration = song.Duration.ToString(@"mm\:ss");
                string taille = (song.Size / 1024f / 1024f).ToString("0.00") + " Mo";
                string featuring = song.Featuring != null ? string.Join(", ", song.Featuring) : "";

                // Assurez-vous que les colonnes de votre DataGridView correspondent à cet ordre
                dataGridView2.Rows.Add(titre, artiste, annee, duration, taille, featuring);
            }
        }

        private void dataGridView2_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            _protocol.SayOnline();
        }
    }
}
