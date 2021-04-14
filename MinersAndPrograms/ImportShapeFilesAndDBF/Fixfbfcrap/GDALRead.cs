using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OSGeo.GDAL;
using System.Data;
using System.Drawing;
using System.Drawing.Imaging;

namespace Fixfbfcrap
{
    public class GDALRead
    {

        public class GDALSizes
        {
            public double GeoLeft;
            public double GepTop;
            public double GeoPerPixelX;
            public double GeoPerPixelY;
        }

        // i think to them time we invest in our projects represents time they don't think they'll have to do anything
        

        public int XTiles(int res)
        {
            return (int)Math.Ceiling((double)RasterImg.RasterXSize / res);
        }

        public int YTiles(int res)
        {
            return (int)Math.Ceiling((double)RasterImg.RasterYSize / res);
        }

        public int remainderX(int res)
        {
            return (int)(RasterImg.RasterXSize % res);
        }

        public int remaindery(int res)
        {
            return (int)(RasterImg.RasterYSize % res);
        }

        public int TileSizeX;
        public int TileSizeY;

        public string filename;
        public OSGeo.GDAL.Dataset RasterImg;

        public GDALRead(string fname)
        {
            filename = fname;
        }

        public void OpenFile()
        {
            RasterImg = Gdal.Open(filename,Access.GA_ReadOnly);
        }

        public Bitmap CopyToBitmap(int band, int width, int height, int startx, int starty, int readw, int readh)
        {
            Bitmap b = new Bitmap(width, height);
            
            var imgband = RasterImg.GetRasterBand(band);
            var colortable = imgband.GetColorTable();

            byte[] bytes = new byte[readw * readh];

            

            var getdatares = imgband.ReadRaster(startx, starty, readw, readh, bytes, readw, readh, 0, 0);

            for (int x=startx; x < startx+readw; x++)
            {
                for (int y=starty; y < starty+readh;y++)
                {
                    var index = bytes[(y-starty) * readh + (x-startx)];
                    var col = colortable.GetColorEntry(index);
                    var c = Color.FromArgb(col.c4, col.c1, col.c2, col.c3);
                    b.SetPixel(x-startx, y-starty, c);
                }
            }

            return b;
        }



    }
}
