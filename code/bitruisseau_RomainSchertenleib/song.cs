using BitRuisseau;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace bitruisseau_RomainSchertenleib
{
    public class song : ISong
    {
        public string Title { get; set; }
        public string Artist { get; set; }
        public int Year { get; set; }
        public TimeSpan Duration { get; set; }
        public int Size { get; set; }
        public string[] Featuring { get; set; }
        public string Hash { get; set; }
        public string Extension { get; set; }

        public song(string hash, string extension)
        {
            Hash = hash;
            Extension = extension;
        }

        public song() { }
    }
}
