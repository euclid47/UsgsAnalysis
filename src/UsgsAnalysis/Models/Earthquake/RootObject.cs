using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace UsgsAnalysis.Models.Earthquake
{
    public class RootObject
    {
        public string type { get; set; }
        public Metadata metadata { get; set; }
        public List<Feature> features { get; set; }
        public List<double> bbox { get; set; }
    }
}
