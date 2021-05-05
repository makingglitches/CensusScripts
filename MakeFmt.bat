@echo off
echo Syntax:  MakeFmt.bat [table with schema] [ database name] [file name]
echo creating format file %3 
bcp  %1 format nul -T -d %2 -f %3