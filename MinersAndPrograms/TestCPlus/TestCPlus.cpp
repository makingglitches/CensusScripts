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

    // so once again jupyter for learning this seems smart.
    // i always have so many scattered thoughts and drawing them together with gradual meat 
    // and bone example seems useful still....
    // course the tool may not have quite the functionality i'm looking for though...
    // to be useful to me it would seem to need to be able to represent a whole range of things
    // and pull together at the very least c++ and mssql connections+queries so that
    // and allow me to add links to useful informations surrounding the concept
    // as i am remembering looking at this its unclear if it was useless or if i just didnt 
    // push enough in between them wasting my time and bitching at each other.

    // god so fucking sleepy.

    // the jupyter notebooks arent far enough developed yet for use with c++
    // and i dont see much use in python atm except for specific tasks
    // js is just fine for json parsing
    // and besides vs seems to offer a way of genning classes from json anyway.
    // right now just to process raster data the c++ lib will probably be fine.
    // c++ is not my native language but its familiar enough 
    // and its robust and i just really want to limit the number of languages this project uses.
    // we alredy have electron, c# and node and c++ and sql.
    // that seems ike enough

}

