
#include<gdal.h>

#pragma once
 class GdalSegmenter
{
	 public:
		GdalSegmenter(char* inputfile, char* outputbase, char*outputdir, char*outputindex)
		{
			_inputfilename = inputfile;
			_outputbasename = outputbase;
			_outputdir = outputdir;
		}

	private:
		char* _inputfilename;
		char* _outputbasename;
		char* _outputdir;
		char* _outputindexfile;

};

