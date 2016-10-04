echo off
cls

if exist TestDir (
  rmdir /s /q TestDir
)

if exist Dest (
  rmdir /s /q Dest
)

mkdir TestDir && cd TestDir
type nul > text1.txt
type nul > 2.txt
type nul > img.bmp

mkdir SubDir && cd SubDir
type nul > sub.txt
type nul > sub.bmp

cd ..
cd ..

echo on
cls



cd TestDir
dir
pause
cd SubDir
dir
pause

cd ..
cd ..

dir
pause

ArchComp1 test.txt \TestDir \Dest 
pause

ArchComp1 2.txt \TestDir \Dest 
dir
pause

cd Dest
dir
pause

cd ..

ArchComp1 \TestDir \Dest --overwrite

cd Dest
dir
pause

cd ..

ArchComp1 \TestDir \Dest --overwrite -a *.txt *.bmp

cd Dest
dir
pause
cd SubDir
dir
pause

