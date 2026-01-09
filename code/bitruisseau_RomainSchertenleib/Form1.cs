using BitRuisseau;

namespace bitruisseau_RomainSchertenleib
{
    public partial class Form1 : Form
    {
        private static string appDirectory = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "BitRuisseau");
        private string SavefileName = "data.txt";
        private Protocol _protocol;
        private static MqttCommunicator _mqttCommunicator;
        private static System.Timers.Timer _timer;
        public Form1(MqttCommunicator mqttCommunicator, Protocol protocol)
        {
            InitializeComponent();
            _protocol = protocol;
            _mqttCommunicator = mqttCommunicator;

            if (!Directory.Exists(appDirectory))
            {
                Directory.CreateDirectory(appDirectory);

            }

            string SavefilePath = Path.Combine(appDirectory, SavefileName);
            txtPath.Text = File.ReadAllText(SavefilePath);
            label1.Font = new Font("Arial", 22, FontStyle.Regular);
            _protocol.CatalogReceived += Protocol_CatalogReceived;
            _mqttCommunicator.OnlineMessageresived += OnlineMessageresived;
            _mqttCommunicator.Catalogresived += Mqtt_Catalogresived;
            LoadMp3Files(txtPath.Text);

            _timer = new System.Timers.Timer(20000);
            _timer.Elapsed += (sender, e) => protocol.GetOnlineMediatheque();
            _timer.AutoReset = true;
            _timer.Enabled = true;
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
        private void Protocol_CatalogReceived(List<song> catalog)
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
        private void DisplayCatalog(List<song> catalog)
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
                dataGridView2.Rows[rowIndex].Tag = song;
            }
        }

        private void dataGridView2_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                DataGridViewRow row = dataGridView2.Rows[e.RowIndex];

                song selectedSong = row.Tag as song;

                if (treeView1.SelectedNode != null && selectedSong != null)
                {
                    string recipientName = treeView1.SelectedNode.Text;

                    _protocol.AskMedia(selectedSong, recipientName, 0, -1);

                    MessageBox.Show($"Demande envoyée pour '{selectedSong.Title}' à {recipientName}");
                }
            }
        }
        private void button1_Click(object sender, EventArgs e)
        {
            _protocol.SayOnline();
        }

        private void OnlineMessageresived(string senders)
        {
            if (treeView1.InvokeRequired)
            {
                treeView1.Invoke(new Action(() => OnlineMessageresived(senders)));
                return;
            }

            // Vérifier si le sender n'est pas déjà dans la liste pour éviter les doublons
            bool existeDeja = false;
            foreach (TreeNode node in treeView1.Nodes)
            {
                if (node.Text == senders)
                {
                    existeDeja = true;
                    break;
                }
            }
            if (senders == "romain")
            {
                return;
            }
            if (!existeDeja)
            {
                treeView1.Nodes.Add(new TreeNode(senders));
            }
        }
        private void Mqtt_Catalogresived(string sender, List<song> songs)
        {
            if (this.InvokeRequired)
            {
                this.Invoke(new Action(() => Mqtt_Catalogresived(sender, songs)));
                return;
            }

            // On vérifie que les données ne sont pas nulles
            if (songs == null) return;

            // Optionnel : On peut vérifier si le catalogue reçu correspond 
            // bien à l'utilisateur actuellement sélectionné

            foreach (var s in songs)
            {
                string titre = s.Title ?? "Inconnu";
                string artiste = s.Artist ?? "Inconnu";
                int annee = s.Year;
                string duration = s.Duration.ToString(@"mm\:ss");
                string taille = (s.Size / 1024f / 1024f).ToString("0.00") + " Mo";
                string featuring = s.Featuring != null ? string.Join(", ", s.Featuring) : "";

                dataGridView2.Rows.Add(titre, artiste, annee, duration, taille, featuring);
            }
        }

        private void treeView1_DoubleClick(object sender, EventArgs e)
        {
            if (treeView1.SelectedNode != null)
            {
                // On récupère le nom du sender (le texte du nœud)
                string selectedUser = treeView1.SelectedNode.Text;

                // On vide le tableau avant la nouvelle requête
                dataGridView2.Rows.Clear();

                // On appelle la méthode du protocole pour envoyer le message "askCatalog"
                _protocol.AskCatalog(selectedUser);
            }
        }
    }
}
