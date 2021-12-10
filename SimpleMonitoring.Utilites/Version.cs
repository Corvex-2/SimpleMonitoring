using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimpleMonitoring.Utilites
{
    public class Version
    {
        public string Major { get; private set; }
        public string Minor { get; private set; }
        public string Patch { get; private set; }

        public Version(string Major, string Minor, string Patch)
        {
            this.Major = Major;
            this.Minor = Minor;
            this.Patch = Patch;
        }

        public string Complete 
        { 
            get
            {
                return string.Concat(Major, ".", Minor, ".", Patch); ;
            } 
        }
    }
}
