using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OSGeo.GDAL;
using System.Data;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;

namespace ImageStatisticsandPoc
{
    public class GDALRead
    {

        /// <summary>
        /// Represents pertinent time and size results from compressing data read from the raster
        /// </summary>
        public class TimePieces
        {
            public long PngSize { get; set; }
            public long TiffSize { get; set; }
            public TimeSpan CopyTime { get; set; }
            public TimeSpan PngTime { get; set; }
            public TimeSpan TiffTime { get; set; }
        }

        // i think to them time we invest in our projects represents time they don't think they'll have to do anything
        


        /// <summary>
        /// Returns the number of tile columns at a specific tile size.
        /// </summary>
        /// <param name="res"></param>
        /// <returns></returns>
        public int XTiles(int res)
        {
            return (int)Math.Ceiling((double)RasterImg.RasterXSize / res);
        }

        /// <summary>
        /// Returns the number of tile rows at a specific tile size
        /// </summary>
        /// <param name="res"></param>
        /// <returns></returns>
        public int YTiles(int res)
        {
            return (int)Math.Ceiling((double)RasterImg.RasterYSize / res);
        }


        /// <summary>
        /// Returns the width in pixels of the last tile column of the raster, if not evenly divisable by tile size.
        /// </summary>
        /// <param name="res"></param>
        /// <returns></returns>
        public int remainderX(int res)
        {
            return (int)(RasterImg.RasterXSize % res);
        }

        /// <summary>
        /// For oddly divisable raster heights, returns the remainder of pixels height-wise, that represent the height of the last tile row in pixels
        /// </summary>
        /// <param name="res"></param>
        /// <returns></returns>
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

        /// <summary>
        /// This opens the raster file and uses the gdal sharp wrapper to create the main dataobject which it stores in the RasterImg field.
        /// </summary>
        public void OpenFile()
        {
            RasterImg = Gdal.Open(filename,Access.GA_ReadOnly);
        }

        /// <summary>
        /// This creates a bitmap object from raster data at specific locations in the raster.
        /// </summary>
        /// <param name="band"></param>
        /// <param name="width"></param>
        /// <param name="height"></param>
        /// <param name="startx"></param>
        /// <param name="starty"></param>
        /// <param name="readw"></param>
        /// <param name="readh"></param>
        /// <returns></returns>
        public Bitmap CopyToBitmap(int band, int width, int height, int startx, int starty, int readw, int readh)
        {
            Bitmap b = new Bitmap(width, height);
            
            var imgband = RasterImg.GetRasterBand(band);
            var colortable = imgband.GetColorTable();

            byte[] bytes = new byte[readw * readh];

            var getdatares = imgband.ReadRaster(startx, starty, readw, readh, bytes, readw, readh, 0, 0);

            for (int x=0; x < readw; x++)
            {
                for (int y=0; y < readh;y++)
                {
                    var index = bytes[y * readw + x];

                    var col = colortable.GetColorEntry(index);
                    var c = Color.FromArgb(col.c4, col.c1, col.c2, col.c3);
                    b.SetPixel(x, y, c);
                }
            }

            return b;
        }

        /// <summary>
        /// This function performs data tracking on one tile at a specific tile size and location for the compression test
        /// Also taking into mind processing time for each major step.
        /// </summary>
        /// <param name="tilex"></param>
        /// <param name="tiley"></param>
        /// <param name="tilesize"></param>
        /// <returns></returns>
        public TimePieces GetTileTest(int tilex,int tiley, int tilesize)
        {

            TimePieces t = new TimePieces();
 
            int tilewidth = (tilex+1) * tilesize > RasterImg.RasterXSize ? remainderX(tilesize) : tilesize;
            int tileheight = (tiley+1) * tilesize > RasterImg.RasterYSize ? remaindery(tilesize) : tilesize;

            DateTime start = DateTime.Now;

            int startx = tilex * tilesize;
            int starty = tiley * tilesize;

            Bitmap b = CopyToBitmap(1, tilewidth, tileheight, startx ,starty, tilewidth, tileheight);
           
            DateTime end = DateTime.Now;

            t.CopyTime = end - start;


            start = DateTime.Now;

            MemoryStream ms = new MemoryStream();
            b.Save(ms, ImageFormat.Png);

            end = DateTime.Now;
            
            t.PngTime = end - start;
            
            t.PngSize =  ms.Length;

            start = DateTime.Now;
            ms = new MemoryStream();

            //    var codec = ImageCodecInfo.GetImageEncoders().Where(o => o.FormatID.Equals(ImageFormat.Tiff)).First();
            b.Save(ms, ImageFormat.Tiff);

            end = DateTime.Now;
            
            t.TiffSize= ms.Length;
            t.TiffTime = start - end;

            return t;
        }

    }
}
