using BitRuisseau;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ProtocolMessage = BitRuisseau.Message;

namespace bitruisseau_RomainSchertenleib
{
    internal class Protocol : IProtocol
    {
        // 1. Déclaration des champs de classe (Pour _communicator et _senderHostname)
        private readonly MqttCommunicator _communicator;
        private readonly string _senderHostname;

        // 2. Déclaration de la constante (Pour GlobalRecipient)
        private const string GlobalRecipient = "0.0.0.0";

        // 3. Constructeur pour initialiser les champs
        public Protocol(MqttCommunicator communicator, string senderHostname)
        {
            _communicator = communicator;
            _senderHostname = senderHostname;
        }
        public List<ISong> AskCatalog(string name)
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
            throw new NotImplementedException("La gestion de la réception de la réponse 'sendCatalog' n'est pas implémentée.");
        }


        public void AskMedia(ISong song, string name, int startByte, int endByte)
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

            // L'implémentation complète devrait attendre la réponse 'sendMedia'.
            // Cela se ferait également via OnMessageReceived.
            throw new NotImplementedException();
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

            // 2. L'implémentation complète devrait attendre les réponses 'online' 
            //    via OnMessageReceived du MqttCommunicator.
            //    Pour l'instant, on retourne un tableau vide et on lève une exception pour signaler 
            //    que la partie réception n'est pas implémentée.
            throw new NotImplementedException("La gestion de la réception des messages 'online' et l'attente des réponses ne sont pas implémentées.");
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
        private List<ISong> GetLocalCatalog()
        {
            // TODO: Remplacer par l'accès réel à vos données de catalogue
            return new List<ISong>(); // Retourne une liste vide pour l'exemple
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

        private string ReadAndEncodeMediaChunk(ISong song, int startByte, int endByte)
        {
            // TODO: Lire le fichier local correspondant au hash de la chanson,
            // extraire les bytes de startByte à endByte, et les encoder en Base64.
            // Simuler un morceau de données encodé en Base64
            return $"Base64Data_Hash:{song.Hash}_Start:{startByte}_End:{endByte}";
        }
        public void SendMedia(ISong song, string name, int startByte, int endByte)
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
