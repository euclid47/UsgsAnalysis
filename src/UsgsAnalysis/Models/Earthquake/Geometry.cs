using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace UsgsAnalysis.Models.Earthquake
{
    public class Geometry
    {
        public string type { get; set; }
        public List<double> coordinates { get; set; }
    }
}
