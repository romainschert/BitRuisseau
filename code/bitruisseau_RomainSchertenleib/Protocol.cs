using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace bitruisseau_RomainSchertenleib
{
    internal class Protocol : IProtocol
    {

        public List<ISong> AskCatalog(string name)
        {
            throw new NotImplementedException();
        }

        public void AskMedia(string name, int startByte, int endByte)
        {
            throw new NotImplementedException();
        }

        public string[] GetOnlineMediatheque()
        {
            throw new NotImplementedException();
        }

        public void SayOnline()
        {
            throw new NotImplementedException();
        }

        public void SendCatalog(string name)
        {
            throw new NotImplementedException();
        }

        public void SendMedia(string name, int startByte, int endByte)
        {
            throw new NotImplementedException();
        }
    }
}
