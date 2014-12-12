@echo off
SETLOCAL ENABLEDELAYEDEXPANSION

:: set the correct path to the the app
if not defined ProgramFiles(x86). (
  echo 32-bit OS detected
  :: set ttPath=%CommonProgramFiles%\Microsoft Shared\TextTemplating\1.2\
  set ttPath=%CommonProgramFiles%\Microsoft Shared\TextTemplating\11.0\
) else (
  echo 64-bit OS detected
  :: set ttPath=%CommonProgramFiles(x86)%\Microsoft Shared\TextTemplating\1.2\
  set ttPath=%CommonProgramFiles(x86)%\Microsoft Shared\TextTemplating\11.0\
)

:: set the working dir (default to current dir)
if not (%1)==() pushd %~dp1

:: set the file extension (default to vb)
set ext=%2
if /i %ext:~1%==vbproj (
  set ext=vb
) else if /i %ext:~1%==csproj (
  set ext=cs
) else if /i [%ext%]==[] (
  set ext=vb
)

:: create a list of all the T4 templates in the working dir
echo Running TextTransform from %cd%
dir *.tt /b /s | findstr /vi obj > t4list.txt

:: transform all the templates
set blank=.
for /f "delims=" %%d in (t4list.txt) do (
  set file_name=%%d
  set file_name=!file_name:~0,-3!.%ext%
  echo:  \--^> !!file_name:%cd%=%blank%!
  "%ttPath%TextTransform.exe" -out "!file_name!" "%%d"
)

:: delete T4 list and return to previous directory
del t4list.txt
popd

echo T4 transformation complete