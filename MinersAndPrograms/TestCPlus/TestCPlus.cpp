// TestCPlus.cpp : This file contains the 'main' function. Program execution begins and ends there.
//

#include <iostream>
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
    
    GDALRasterBand* band = poDataset->GetRasterBand(1);

   

    std::cout << poDataset->GetRasterCount() << "\n";

    std::cout << poDataset->GetProjectionRef() << "\n";
    std::cout << poDataset->GetDriver()->GetDescription() << "\n";

    GDALDataset::Bands b =   poDataset->GetBands();
    
    std::cout << b.size() << "\n";
    std::cout << poDataset->GetFileList() << "\n";

    std::cout << poDataset->GetGCPProjection() << "\n";
    std::cout << poDataset->GetRasterXSize() << ", " << poDataset->GetRasterYSize() << "\n";
    std::cout << poDataset->GetRasterCount() << "\n";

    GDALDataset::Features f = poDataset->GetFeatures();
    
    for (auto&& flp  : poDataset->GetFeatures())
    {
        std::cout << "Feature of layer " <<
          flp.layer->GetName()  << std::endl;  
    }

    double coordbounds[6];

    if (poDataset->GetGeoTransform(coordbounds) == CE_None)
    {
        std::cout << "UL GeoX: " << coordbounds[0] << std::endl;
        std::cout << "UL GeoY: " << coordbounds[3] << std::endl;
        std::cout << "GeoX / Pixel:" << coordbounds[1] << std::endl;
        std::cout << "GeoY / Pixel:" << coordbounds[5] << std::endl;
     }

    GDALClose((GDALDatasetH) poDataset);

    // i need to download a bunch of porn clips
    // use the song leitbild
    // and change the color and lighting around and the speed and combine clips with some other images like you'd find 
    // in war marches and the like and make a drug-like video that stimulates the best parts at once ;P
    // maybe if they watched that enough they'd not be fucked up lol
    // lookie here, titties. and hard industrial. lol

    // see its shit like this that annoys the fuck out of me.
    // i can segment and tile the damn image into smaller pngs.
    // the calculator i found would reduce the file size in png lossless format to
    // 9.78 GB at its present resolution of 161190 x 104424
    // that is a 9.82  GB saved thats is pretty decent.
    // but nooooo i have to keep redisovering during the worst period of my life
    // not like they could just let this stand and leave my shit alone.
    // nope. that would be too goddamn much to ask apparently.

}

