#define _CRT_SECURE_NO_WARNINGS

#include <iostream>
#include <iomanip>
#include "gdal_priv.h"
#include "cpl_conv.h"
#include "proj.h"
#include "ogr_core.h"
#include "ogr_feature.h"
#include "ogrsf_frmts.h"
#include <windows.graphics.h>
#include <math.h>
#include "png.h"



void write_row_callback(png_structp png_ptr , png_uint_32 row,
    int pass)
{
    /* put your code here */
}

int main()
{
    
    const char* filename = R"(C:\Users\John\Documents\CensusProject\CensusShapeFileData\TreeCanopy\nlcd_2016_treecanopy_2019_08_31\nlcd_2016_treecanopy_2019_08_31.img)";
       
    GDALDataset* poDataset;
    GDALAllRegister();
    
    poDataset = (GDALDataset*)GDALOpen(filename, GA_ReadOnly);
    
    std::cout << "Raster Count: " << poDataset->GetRasterCount() << "\n";

    std::cout << "Projection Reference: " <<  poDataset->GetProjectionRef() << "\n";
    std::cout << "Driver: " << poDataset->GetDriver()->GetDescription() << "\n";

    GDALDataset::Bands b =   poDataset->GetBands();
    
    std::cout << "Bands Size: " << b.size() << "\n";

    std::cout << poDataset->GetFileList() << "\n";

    std::cout << poDataset->GetGCPProjection() << "\n";

    std::cout << "Raster Count: " << poDataset->GetRasterCount() << "\n";

    GDALDataset::Features f = poDataset->GetFeatures();
    

    for (auto&& flp  : poDataset->GetFeatures())
    {
        std::cout << "Feature of layer " <<
          flp.layer->GetName()  << std::endl;  
    }
    
    double coordbounds[6];

    std::cout << "Raster Size (X,Y): "<<  poDataset->GetRasterXSize() << ", " << poDataset->GetRasterYSize() << "\n";

    int rasterx = poDataset->GetRasterXSize();
    int rastery = poDataset->GetRasterYSize();


    if (poDataset->GetGeoTransform(coordbounds) == CE_None)
    {
        printf("Upper Left GeoX: %.5f\n", coordbounds[0]);
        printf("Upper Left GeoY: %.5f\n", coordbounds[3]);
        std::cout << "GeoX / Pixel:" << coordbounds[1] << std::endl;
        std::cout << "GeoY / Pixel:" << coordbounds[5] << std::endl;
     }


    int blockx, blocky;
    
    GDALRasterBand* band = poDataset->GetRasterBand(1);

    band->GetBlockSize(&blockx, &blocky);

    std::cout << "This is probably all repeated. Actually sure it is. Remember blocky.\n"
        << "Blocksize x: " << blockx
        << "  Blocksize y: " << blocky << "\n";

    
    
    std::cout << "Raster Band Type: " << GDALGetDataTypeName(band->GetRasterDataType()) << std::endl;
    std::cout << "Color Interpretation Name: " << GDALGetColorInterpretationName(band->GetColorInterpretation()) << std::endl;
    
    GDALColorTable* colors =   band->GetColorTable();
        
    GDALColorEntry* c = new GDALColorEntry();

    int ret = colors->GetColorEntryAsRGB(85, c);


    void* buffer = new byte[512 * 512];


    int x = 0;
    int y = 0;


    float tilesx = rasterx / 512.0;
    float tilesy = rastery / 512.0;
    
    float excessx = rasterx - 512.0*trunc(tilesx);
    float excessy = rastery - 512.0 * trunc(tilesy);

    int whol_tilesx = (int)ceilf(tilesx);
    int whol_tilesy = (int)ceilf(tilesy);

 
    std::cout << "Need to read: " << whol_tilesx << " x " << whol_tilesy << std::endl;
    std::cout << "The last column will have: " << excessx << std::endl;
    std::cout << "The last row will have: " << excessy << std::endl;


    // retrieve a sample block of data from the raster.
    CPLErr res = band->RasterIO(GF_Read, 0, 0, 512, 512, buffer, 512, 512, GDT_Byte, 1, 0, NULL);

    buffer = new byte[512 * 512];

    // populate the buffer object with a 512 block of data from the specific rasters UL corner
    res = band->RasterIO(GF_Read, 161190-512, 104424-512,512,512,buffer, 512,512,GDT_Byte, 1, 0, NULL);

    // convenience typed pointer to buffer.
    byte* bufbyte = (byte*)buffer;

 
    // open a png file for writing.
    FILE* pf =   fopen("test.png", "wb");

    // initialize the png write session.
    png_structp png_ptr = png_create_write_struct
    (PNG_LIBPNG_VER_STRING, NULL,
        NULL,NULL);

    // error out if the pointer is null
    if (!png_ptr)
        return (ERROR);

    // create a png info pointer further configuring the code
    png_infop info_ptr = png_create_info_struct(png_ptr);

    //again if the pointer is null error out
    if (!info_ptr)
    {
        png_destroy_write_struct(&png_ptr,
            (png_infopp)NULL);
        return (ERROR);
    }

    // i forget what this does :P
    if (setjmp(png_jmpbuf(png_ptr)))
    {
        png_destroy_write_struct(&png_ptr, &info_ptr);
        fclose(pf);
        return (ERROR);
    }

    // a bunch of png initialization routines, blah blah as per the manual section 5.3
    png_init_io(png_ptr, pf);

    // this gets called i believe when a row is called to indicate status.
    png_set_write_status_fn(png_ptr, write_row_callback);

    // in general this is a row by row comparison/ deduction operation meant to make the compression algorithm more efficient
    // i left everything on, we'll see if that functions.
    png_set_filter(png_ptr, 0, PNG_ALL_FILTERS);

    // choosing best compression, duh. trying to get that *****
    // 20 GB file down considerably !
    // each tile minus metadata is a 512^2 byte matrix of values relating to 
    // the palette. a downsize is i think png's compose a 4 byte per pixel or 32 bpp 
    // for full color, all i'll need is a 3 byte however or 24 bpp
    // because the alpha channel is always 255.
    png_set_compression_level(png_ptr, 9);
       

    // https://www.euccas.me/zlib/#deflate_sliding_window
    //https://refspecs.linuxbase.org/LSB_4.0.0/LSB-Desktop-generic/LSB-Desktop-generic/libpng-png-set-compression-window-bits-1.html
    png_set_compression_window_bits(png_ptr, 10);


    png_set_compression_method(png_ptr, 8);

    // in the zlib deflateinit2()  description it says to set this for filtered data.
    png_set_compression_strategy(png_ptr, PNG_Z_DEFAULT_STRATEGY);
  

    // ths is the input buffer size just tagged a meg more on top of it, this isnt a comp from the 1980s
    // and only one of these processes will be running at a time.
    png_set_compression_buffer_size(png_ptr, 1008192);


    //section 4.3 of libpng-1.4.0 manual defines this pretty welll.
    // we're stripping the alpha channel
    // bit-depth signifies the individual color component possible values.
    // which in this case seem to be limited 256x256x256 so, 8.
    // the interlace value i'm kind of curious about
    // as would this make zooming better or faster if i decide to display this data ?
    // for now i'll set it to adam7
    // documentation indicates to set the last two arguments to there defaults, makes me wonder why they are there at all.
    png_set_IHDR(png_ptr, info_ptr, 512, 512, 8, PNG_COLOR_TYPE_RGB, PNG_INTERLACE_ADAM7, PNG_COMPRESSION_TYPE_DEFAULT, PNG_FILTER_TYPE_DEFAULT);

    png_colorp pal = new png_color[256];


    // copy the rasters pallette to the png pallete, this should allow values to be simply copied over.
    for (int x = 0; x < 256; x++)
    {
        int ret = colors->GetColorEntryAsRGB(x, c);

        // simple conversion.
        pal[x].red = (png_byte) c->c1;
        pal[x].green = (png_byte) c->c2;
        pal[x].blue = (png_byte) c->c3;
    }
   
    
    // set the png pallete
    png_set_PLTE(png_ptr, info_ptr, pal, 256);


    // set the comments section, for shits and giggles
    png_textp comments = new png_text[3];
    comments[0].compression = PNG_TEXT_COMPRESSION_NONE;
    comments[0].text = (png_charp)"Treecanopy.img";
    comments[0].text_length = 14; // strlen((char*)comments[1].text);
    comments[0].key = (png_charp)"File";


    comments[1].compression = PNG_TEXT_COMPRESSION_NONE;
    comments[1].key = (png_charp) "Size";
    comments[1].text = (png_charp)"512x512";
    comments[1].text_length = 7; // strlen(comments[1].text);

    comments[2].compression = PNG_TEXT_COMPRESSION_NONE;
    comments[2].key = (png_charp) "TilePos";
    comments[2].text = (png_charp)"1x1";
    comments[2].text_length = 3;// strlen(comments[2].text);


    png_set_text(png_ptr, info_ptr, comments, 3);
 
    png_color_16*  backcol = new png_color_16();
    
    backcol->red = (png_uint_16)pal[0].red;
    backcol->green = (png_uint_16)pal[0].green;
    backcol->blue = (png_uint_16)pal[0].blue;

    png_set_bKGD(png_ptr, info_ptr, backcol);

    png_bytepp imgrows = new png_bytep[512];
  
    std::cout << "Copying sample tile to image rows:  ";

    for (int y = 0; y < 512; y++)
    {
        for (int x = 0; x < 512; x++)
        {
            imgrows[y] = new png_byte[512];
            imgrows[y][x]=(png_byte)bufbyte[512 * y];
        }
    }

    std::cout << "done" << std::endl;
     
    png_set_rows(png_ptr, info_ptr,imgrows );

    png_write_png(png_ptr, info_ptr, PNG_TRANSFORM_IDENTITY, NULL);
  
 
    GDALClose((GDALDatasetH) poDataset);

}

