using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RasterStats.Stats
{
    public class TimePieces
    {
        public TimePieces()
        {
            TileSize = 0;
            PngSize = 0;
            TiffSize = 0;
            TiffTime = TimeSpan.Zero;
            PngTime = TimeSpan.Zero;
            CopyTime = TimeSpan.Zero;
            OfMeasures = 0;
            Pieces = new List<TimePieces>();

        }

        public long OfMeasures { get; set; }
        public int TileSize { get; set; }
        public long PngSize { get; set; }
        public long TiffSize { get; set; }
        
        public TimeSpan CopyTime { get; set; }

        public TimeSpan TiffTime { get; set; }
        public TimeSpan PngTime { get; set; }

        public TileVariance source { get; set; }
        
        public List<TimePieces> Pieces { get; set; }
    }
}
