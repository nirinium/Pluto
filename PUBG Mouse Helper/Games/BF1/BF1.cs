using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LiteDB;

namespace Pluto.Games.BF1
{
    internal static class BF1
    {
        public class Weapons
        {
            public int Id { get; set; }
            public string Name { get; set; }
            public string[] RecoilX { get; set; }
            public string[] RecoilY { get; set; }
            public string[] Guns { get; set; }
            public string GunTypes { get; set; }
            public bool IsActive { get; set; }
        }
    }
}
