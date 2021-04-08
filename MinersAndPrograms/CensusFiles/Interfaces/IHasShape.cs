using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ShapeUtilities;

namespace CensusFiles
{
    public interface IHasShape
    {
        BaseShapeRecord Shape { get; set; }
    }
}
