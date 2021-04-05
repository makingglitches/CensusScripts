// TestCPlus.cpp : This file contains the 'main' function. Program execution begins and ends there.
//

#include <iostream>
#include <iomanip>
//#include "gdal.h"
#include "gdal_priv.h"
#include "cpl_conv.h"

#include "ogr_core.h"
#include "ogr_feature.h"
#include "ogrsf_frmts.h"

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

    band->GetBlockSize(&blockx, &blocky);

    std::cout << "This is probably all repeated. Actually sure it is. Remember blocky.\n"
        << "Blocksize x: " << blockx
        << "  Blocksize y: " << blocky << "\n";

    std::cout << "Raster Band Type: " << GDALGetDataTypeName(band->GetRasterDataType()) << std::endl;
    std::cout << "Color Interpretation Name: " << GDALGetColorInterpretationName(band->GetColorInterpretation()) << std::endl;

    // i find this interesting apparently the free and delete operators in std c++ cause issues with msvc.
    // they apparently migrated to garbage collection.
    GDALClose((GDALDatasetH) poDataset);

    // seriously today was just another day of mind and body and soul pollution.
    // really was.
    // i wish these people werent monsters.
    // every night i'm here i'm just reminded that these weird fucked up creatures
    // are mirrored by their younger prettier better hidden counterparts
    // and almost just as worthless.
}

