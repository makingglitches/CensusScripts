// TestCPlus.cpp : This file contains the 'main' function. Program execution begins and ends there.
//

#include <iostream>
//#include "gdal.h"
#include "gdal_priv.h"
#include "cpl_conv.h"

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
        flp.feature->DumpReadable();
    }

    GDALClose((GDALDatasetH) poDataset);

    // see its shit like this that annoys the fuck out of me.
    // i can segment and tile the damn image into smaller pngs.
    // the calculator i found would reduce the file size in png lossless format to
    // 9.78 GB at its present resolution of 161190 x 104424
    // that is a 9.82  GB saved thats is pretty decent.
    // but nooooo i have to keep redisovering during the worst period of my life
    // not like they could just let this stand and leave my shit alone.
    // nope. that would be too goddamn much to ask apparently.

}

