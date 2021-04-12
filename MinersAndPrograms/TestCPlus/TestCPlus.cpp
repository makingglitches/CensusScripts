// TestCPlus.cpp : This file contains the 'main' function. Program execution begins and ends there.
//

#define _CRT_SECURE_NO_WARNINGS

#include <iostream>
#include <iomanip>
//#include "gdal.h"
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

    // so seriously all they need to do is make sure i leave before i get trapped. so thats before 2017, move on or move on now really
    // they're just pretending to be threatened because they spent x number of years of my life rubbing this shit in my face 
    // which led to alot of very bad things for them and for me.
    // circular living. what a fucking great idea !
    // i get stuck because they engineered it so.
    // they want me to stay. dont know why because they wont be getting shit.
    // and btw, i've seen plenty of people who do not swear or cuss or use profanity who are complete dullards
    // i'm pissed off and tired and i hate everyone here and dont feel like working on code i already figured out before
    // and btw the c# version of this worked fucking fine !
   
    std::cout << "Hello World!\n";
    
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

    // this confuses me, and its empty anyway
    // but there appears to be privately contained implementations of the iterator class
    // so the implementer cant access them..
    GDALDataset::Features f = poDataset->GetFeatures();
    

    // i am actually wondering what files would have these.
    // raster data is fairly space intensive so i may not find out for awhile.
    for (auto&& flp  : poDataset->GetFeatures())
    {
        std::cout << "Feature of layer " <<
          flp.layer->GetName()  << std::endl;  
    }

    
    // this is asking for a wrapper class for code neatness.
    double coordbounds[6];

    std::cout << "Raster Size (X,Y): "<<  poDataset->GetRasterXSize() << ", " << poDataset->GetRasterYSize() << "\n";

    int rasterx = poDataset->GetRasterXSize();
    int rastery = poDataset->GetRasterYSize();


    if (poDataset->GetGeoTransform(coordbounds) == CE_None)
    {
        // wasted a lot of time once looking up how to fucking use cout with formatters
        // and somehow floats and digits were skipped.
        printf("Upper Left GeoX: %.5f\n", coordbounds[0]);
        printf("Upper Left GeoY: %.5f\n", coordbounds[3]);
        std::cout << "GeoX / Pixel:" << coordbounds[1] << std::endl;
        std::cout << "GeoY / Pixel:" << coordbounds[5] << std::endl;
     }


    // fetch some raster information
    // the goal is to segment this enormous fucking file into a much nicer COMPRESSED series of small files
    // and then create a spatial index so implementing programs can fetch only the data they need 
    // however sparing the 20 GB to something closer and more space efficient and tying them
    // against every gis object in the database.
    // which in general is the idea.
    // then the user code drives which entities get pulled back based on these indexes and their boundary selections.

    // of course if displaying these, scale should probably factor.
    // which qgis seems to do very very well.

    int blockx, blocky;
    
    GDALRasterBand* band = poDataset->GetRasterBand(1);

    // this retrieves the raster block organization.
    band->GetBlockSize(&blockx, &blocky);

    std::cout << "This is probably all repeated. Actually sure it is. Remember blocky.\n"
        << "Blocksize x: " << blockx
        << "  Blocksize y: " << blocky << "\n";

    
    
    std::cout << "Raster Band Type: " << GDALGetDataTypeName(band->GetRasterDataType()) << std::endl;
    std::cout << "Color Interpretation Name: " << GDALGetColorInterpretationName(band->GetColorInterpretation()) << std::endl;
    
    GDALColorTable* colors =   band->GetColorTable();
        
    GDALColorEntry* c = new GDALColorEntry();

    // returns rgba, a seems to always be 255.
    // palette contains colors from 0 to 255
    int ret = colors->GetColorEntryAsRGB(85, c);


    void* buffer = new byte[512 * 512];

    // tile by tile

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


    // kind of hoping its not use the geox,geoy
    CPLErr res = band->RasterIO(GF_Read, 0, 0, 512, 512, buffer, 512, 512, GDT_Byte, 1, 0, NULL);

    buffer = new byte[512 * 512];

    // so checking here.
    // its the spacing argument that lets qgis read super fast I bet !
    res = band->RasterIO(GF_Read, 161190-512, 104424-512,512,512,buffer, 512,512,GDT_Byte, 1, 0, NULL);

    byte* bufbyte = (byte*)buffer;

    // the above works well enough.
    
    
    // need to make sure the warnings about stdio ops are paid attention to here, the compiler doesnt want to 
    // to build atm because it doesnt like fopen which is ansi c... no idea why.
    // also seem to remember fopen should be returning an integer not a struct, we'll see if it works.
    FILE* pf =   fopen("test.png", "wb");

    png_structp png_ptr = png_create_write_struct
    (PNG_LIBPNG_VER_STRING, NULL,
        NULL,NULL);

    if (!png_ptr)
        return (ERROR);

    png_infop info_ptr = png_create_info_struct(png_ptr);

    if (!info_ptr)
    {
        png_destroy_write_struct(&png_ptr,
            (png_infopp)NULL);
        return (ERROR);
    }

    if (setjmp(png_jmpbuf(png_ptr)))
    {
        png_destroy_write_struct(&png_ptr, &info_ptr);
        fclose(pf);
        return (ERROR);
    }

    png_init_io(png_ptr, pf);

    // this gets called i believe when a row is called to indicate status.
    png_set_write_status_fn(png_ptr, write_row_callback);

    // in general this is a row by row comparison/ deduction operation meant to make the compression algorithm more efficient
    // i left everything on, we'll see if that functions.
    png_set_filter(png_ptr, 0, PNG_ALL_FILTERS);

    // choosing best compression, duh. trying to get that fucking 
    // 20 GB file down considerably !
    // each tile minus metadata is a 512^2 byte matrix of values relating to 
    // the palette. a downsize is i think png's compose a 4 byte per pixel or 32 bpp 
    // for full color, all i'll need is a 3 byte however or 24 bpp
    // because the alpha channel is always 255.
    png_set_compression_level(png_ptr, 9);
       

    // fucking goddamn it !!!!

    // should write a benchmark once this is up and working
    // following an algorithm where it tiles EVERY part of the raster at a few select tile sizes 
    // and at different bit window and compression filters and styles
    // and records the final size and then deletes the destination file
    // so an idea of what is optimal is possible
    // note that in the tree canopy raster it uses only shades of green
    // overall, which keeps some of the rgb componets below a certain threshold, limiting the value
    // ranges, this may not be the case with satellite and meterological satellie data.
    // would be nice to have an idea of how to get a sense of variance from large datasets.
    // i suppose an sd/mean might kind of work. data would hve to be interpreted as 3 color component values
    // however which are seperate, or 4 if the alpha channel is on.
    // and thats just for color values, there could be god knows what being stored in whatever data format
    // in what has been indiocated are fuck ton large rasters where 20gb is the dream value.
    // 
    // the power of 2 to indicate the size of the compression window
    // which basically is the size of the pattern being searched for.
    // at 512^2 each tile is only 262 kb in size raw, and in many cases the
    // data is going to vary considerably.
    // i would think 1024 bytes which is 2^10 or 1 KB should be enough
    // https://www.euccas.me/zlib/#deflate_sliding_window
    //https://refspecs.linuxbase.org/LSB_4.0.0/LSB-Desktop-generic/LSB-Desktop-generic/libpng-png-set-compression-window-bits-1.html
    png_set_compression_window_bits(png_ptr, 10);


    png_set_compression_method(png_ptr, 8);

    // in the zlib deflateinit2()  description it says to set this for filtered data.
    png_set_compression_strategy(png_ptr, PNG_Z_DEFAULT_STRATEGY);
  
    // TRY THIS TO SEE HOW MUCH OF A DIFFERENVE IT MAKES.
    //png_set_compression_strategy(png_ptr, Z_FILTERED);


    // ths is the input buffer size just tagged a meg more on top of it, this isnt a comp from the 1980s
    // and only one of these processes will be running at a time.
    png_set_compression_buffer_size(png_ptr, 1008192);

    // this is being defined in pngconfig.h
    // i don't know why they wanted this added.
    // we'll see if something fucks up, but the #define is already included.
    //extern PNG_EXPORT(void, png_set_zbuf_size)

    // obviously the good ole feds werent exceedingly helpful since either the original trump or biden gov was in power at the time
    // so the damn corrupt ass administrators werent hearing about actual change or adding tools with the time investment already finished added
    // no instead these fucks let abominations like the ruby programming language through ! seriously ? and while python is a nice enough language
    // its also overrated imho !

    //section 4.3 of libpng-1.4.0 manual defines this pretty welll.
    // we're stripping the alpha channel
    // bit-depth signifies the individual color component possible values.
    // which in this case seem to be limited 256x256x256 so, 8.
    // the interlace value i'm kind of curious about
    // as would this make zooming better or faster if i decide to display this data ?
    // for now i'll set it to adam7
    // documentation indicates to set the last two arguments to there defaults, makes me wonder why they are there at all.
    png_set_IHDR(png_ptr, info_ptr, 512, 512, 8, PNG_COLOR_TYPE_RGB, PNG_INTERLACE_ADAM7, PNG_COMPRESSION_TYPE_DEFAULT, PNG_FILTER_TYPE_DEFAULT);

    // left off on page 35
    // its cold in them their hills atm
    // gonna retreat.
    // pallette can be directly constructed using the above gdal methods
    // there are 256 colors according to qgis in the raster file.
    // this is going to end up just working for a specific raster atm, but can be made far more generic later.

    png_colorp pal = new png_color[256];

    for (int x = 0; x < 256; x++)
    {
        int ret = colors->GetColorEntryAsRGB(x, c);

        // simple conversion.
        pal[x].red = (png_byte) c->c1;
        pal[x].green = (png_byte) c->c2;
        pal[x].blue = (png_byte) c->c3;
    }
   
    
    png_set_PLTE(png_ptr, info_ptr, pal, 256);


    png_textp comments = new png_text[3];
    comments[0].compression = PNG_TEXT_COMPRESSION_zTXt;
    //comments[1].key = (png_charp)malloc(3);
    //comments[1].text = (png_charp)"Treecanopy.img";
    //comments[1].text_length = strlen(comments[1].text);

    comments[1].compression = PNG_TEXT_COMPRESSION_zTXt;
    //comments[1].key = (png_charp) "Size";
    //comments[1].text = (png_charp)"512x512";
    //comments[1].text_length = strlen(comments[1].text);

    comments[2].compression = PNG_TEXT_COMPRESSION_zTXt;
    //comments[2].key = (png_charp) "TilePos";
    //comments[2].text = (png_charp)"1x1";
    //comments[2].text_length = strlen(comments[2].text);


   // png_set_text(png_ptr, info_ptr, comments, 3);

    //// see if setting a pallette histogram makes any difference or even shows up in a reader
    //// could make a difference in selecting tiles without scanning all its pixels.

    //

    //// set background color.

    //// oh look same bug in libpng
    //// what fun.
    //// this throws an access violation in libpng16
    
    png_color_16*  backcol = new png_color_16();
    
    backcol->red = (png_uint_16)pal[0].red;
    backcol->green = (png_uint_16)pal[0].green;
    backcol->blue = (png_uint_16)pal[0].blue;

    png_set_bKGD(png_ptr, info_ptr, backcol);

    //// if i remember correctly this is a bogus binary causing the problem.
    //// wish it was easier just to build all these libs under windows !
    png_bytepp imgrows = new png_bytep[512];

    // i may just skip this as I think performance will be poor.
  
    std::cout << "Copying sample tile to image rows:  ";

    for (int y = 0; y < 512; y++)
    {
        for (int x = 0; x < 512; x++)
        {
            imgrows[y] = new png_byte[512];
            memcpy(imgrows[y], &bufbyte[512 * y], 512);

            //imgrows[y][x] = (png_byte)bufbyte[y * 512 + x];
        }
    }

    std::cout << "done" << std::endl;
     
    png_set_rows(png_ptr, info_ptr,imgrows );

    png_write_png(png_ptr, info_ptr, PNG_TRANSFORM_IDENTITY, NULL);
    png_write_flush(png_ptr);

    fclose(pf);
    /// bahahhahahahahaha



   // png_write_flush(png_ptr);
    

    // i find this interesting apparently the free and delete operators in std c++ cause issues with msvc.
    // they apparently migrated to garbage collection.
    GDALClose((GDALDatasetH) poDataset);

   // // seriously today was just another day of mind and body and soul pollution.
   // // really was.
   // // i wish these people werent monsters.
   // // every night i'm here i'm just reminded that these weird fucked up creatures
   // // are mirrored by their younger prettier better hidden counterparts
   // // and almost just as worthless.
}

