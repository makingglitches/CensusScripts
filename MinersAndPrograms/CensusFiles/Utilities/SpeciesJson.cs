using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CensusFiles.Utilities
{
    public class SpeciesJson
    {
        public string DlLink { get; set; }
        public string Name { get; set; }
        public string Taxonomy { get; set; }
        public string ScientificName { get; set; }
        public string RangeArchiveName { get; set; }
        public bool Downloaded { get; set; }
        public string DownloadGuid { get; set; }
        public string ServerRangeArchiveName { get; set; }
    }
}
