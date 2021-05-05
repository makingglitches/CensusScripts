using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShapeUtilities
{
   public interface IHasPoints
    {
        List<ShpPoint> Points { get; set; }
    }
}
