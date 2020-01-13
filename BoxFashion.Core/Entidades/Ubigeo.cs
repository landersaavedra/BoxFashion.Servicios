using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BoxFashion.Core.Entidades
{
    public class Ubigeo
    {
        public Int32 IDNUBIGEO { get; set; }
        public Int32 IDNCITY { get; set; }
        public Int32 NUBIGEO { get; set; }
        public String SCITY_DISTR { get; set; }
        public Int32 NPOPULATION { get; set; }
        public Int32 NSURFACE { get; set; }
        public Int64 NLATITUDE { get; set; }
        public Int64 NLONOGITUDE { get; set; }
        public Int64 NY_X { get; set; }

    }
}
