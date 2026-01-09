using BitRuisseau;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ProtocolMessage = BitRuisseau.Message;

namespace bitruisseau_RomainSchertenleib
{
    public class Protocol : IProtocol
    {
        // 1. Déclaration des champs de classe (Pour _communicator et _senderHostname)
        private readonly MqttCommunicator _communicator;
        private readonly string _senderHostname;

        // 2. Déclaration de la constante (Pour GlobalRecipient)
        private const string GlobalRecipient = "0.0.0.0";
        public event Action<List<song>> CatalogReceived;
        // 3. Constructeur pour initialiser les champs
        public Protocol(MqttCommunicator communicator, string senderHostname)
        {
            _communicator = communicator;
            _senderHostname = senderHostname;
        }
        public List<song> AskCatalog(string name)
        {
            var message = new ProtocolMessage
            {
                Recipient = name, // Destinataire spécifique
                Sender = _senderHostname,
                Action = "askCatalog",
                StartByte = null,
                EndByte = null,
                SongList = null,
                SongData = null,
                Hash = null
            };
            _communicator.Send(message);

            // L'implémentation complète devrait attendre la réponse 'sendCatalog' 
            // de la médiathèque 'name'.
            return new List<song>();
        }


        public void AskMedia(song song, string name, int startByte, int endByte)
        {
            var message = new ProtocolMessage
            {
                Recipient = name, // Destinataire spécifique
                Sender = _senderHostname,
                Action = "askMedia",
                StartByte = startByte,
                EndByte = endByte,
                SongList = null,
                SongData = null,
                Hash = song.Hash // Utilise le Hash de l'ISong
            };
            _communicator.Send(message);

        }

        public string[] GetOnlineMediatheque()
        {
            // 1. Envoyer le message askOnline
            var message = new ProtocolMessage
            {
                Recipient = GlobalRecipient,
                Sender = _senderHostname,
                Action = "askOnline",
                StartByte = null,
                EndByte = null,
                SongList = null,
                SongData = null,
                Hash = null
            };
            _communicator.Send(message);

            return new string[1];
        }

        public void SayOnline()
        {
            var message = new ProtocolMessage
            {
                Recipient = GlobalRecipient,
                Sender = _senderHostname,
                Action = "online",
                StartByte = null,
                EndByte = null,
                SongList = null,
                SongData = null,
                Hash = null
            };
            _communicator.Send(message);
        }
        private List<song> GetLocalCatalog()
        {
            string appDirectory = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "BitRuisseau");
            string SavefileName = "data.txt";
            string SavefilePath = Path.Combine(appDirectory, SavefileName);
           
            List<song> catalog = new List<song>();
            string folderPath = File.ReadAllText(SavefilePath);

            string[] files = Directory.GetFiles(folderPath, "*.mp3");
            foreach (string file in files)
            {
                try
                {
                    var tfile = TagLib.File.Create(file);
                    FileInfo info = new FileInfo(file);

                    string title = tfile.Tag.Title ?? Path.GetFileNameWithoutExtension(file);
                    string artist = tfile.Tag.FirstPerformer ?? "Inconnu";
                    int year = (int)tfile.Tag.Year;
                    TimeSpan duration = tfile.Properties.Duration;

                    // Taille en bytes
                    int sizeInBytes = (int)info.Length;

                    // Featuring en tableau
                    string[] featuring = tfile.Tag.Performers;

                    // Hash du fichier (SHA256)
                    string hash = Helper.HashFile(file);

                    // Extension (toujours .mp3 ici)
                    string extension = info.Extension;

                    // Création de l'objet Song
                    song song = new song(hash, extension)
                    {
                        Title = title,
                        Artist = artist,
                        Year = year,
                        Duration = duration,
                        Size = sizeInBytes,
                        Featuring = featuring
                    };

                    catalog.Add(song);
                }
                catch
                {
                    continue;
                }
            }
            return catalog; 
        }
        public void SendCatalog(string name)
        {
            var myLocalSongList = GetLocalCatalog();

            var message = new ProtocolMessage
            {
                Recipient = name, // Destinataire spécifique
                Sender = _senderHostname,
                Action = "sendCatalog",
                StartByte = null,
                EndByte = null,
                SongList = myLocalSongList, // Le catalogue de chansons
                SongData = null,
                Hash = null
            };
            _communicator.Send(message);
        }

        private string ReadAndEncodeMediaChunk(song song, int startByte, int endByte)
        {
            // TODO: Lire le fichier local correspondant au hash de la chanson,
            // extraire les bytes de startByte à endByte, et les encoder en Base64.
            // Simuler un morceau de données encodé en Base64
            return $"Base64Data_Hash:{song.Hash}_Start:{startByte}_End:{endByte}";
        }
        public void SendMedia(song song, string name, int startByte, int endByte)
        {
            var songDataChunk = ReadAndEncodeMediaChunk(song, startByte, endByte);

            var message = new ProtocolMessage
            {
                Recipient = name, // Destinataire spécifique
                Sender = _senderHostname,
                Action = "sendMedia",
                StartByte = startByte,
                EndByte = endByte,
                SongList = null,
                SongData = songDataChunk, // Les données audio encodées
                Hash = song.Hash // Utilise le Hash de l'ISong
            };
            _communicator.Send(message);
        }
    }
}
