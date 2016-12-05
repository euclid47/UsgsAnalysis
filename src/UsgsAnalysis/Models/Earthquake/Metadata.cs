using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace UsgsAnalysis.Models.Earthquake
{
    public class Metadata
    {
        public long generated { get; set; }
        public string url { get; set; }
        public string title { get; set; }
        public int status { get; set; }
        public string api { get; set; }
        public int count { get; set; }
    }
}
