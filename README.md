# Basic Revit API Coding
This is a repository for those learning the Revit API, Revit Macros, Revit Commands, and Revit Applications.
This inlcudes, but not limited to, Simple functioning add-ins, Adding new panels, Retrieve selected/filted elements, read/write from Revit, etc.

It will be updated as I continue to work on bits & pieces and I will try to comment as much as possible so new users will understand.

## Software Needed
Before doing anything please install Revit 2018 as well as Microsoft Visual Studio Community 2017. Make sure the download for Visual 
Studio inlcudes the .NET Framework 4.7.1 Version. This will be the version I am using. I also have downloaded Resharper, a visual studio extension that is free for students, though this is not a requirement to have.

##### -- Essential --
[Revit 2018](https://www.autodesk.com/education/free-software/revit "Free for students") - Free for students.

[Microsoft Visual Studio Community 2017](https://visualstudio.microsoft.com/downloads/ "Free :)") - Free/great software everyone should have.

##### -- Recommended but unessential --
[Revit 2018.2 SDK](https://www.autodesk.com/developer-network/platform-technologies/revit "Very Helpful :)") - (Software Development Kit) Is very helpful for debugging the Revit Database and understanding the elements and how their parameters operate.

[ReSharper](https://www.jetbrains.com/student/ "Free for students") - Free for students/Visual studio extension.


## Setup Macros
TBD

## Setup Commands/Applications
When downloading these files, make sure the file path is correct due to the fact that the addin files depend on where the folders of code have been saved for each plugin.

In my instance, the folders with c# code for Visual Studio are saved here:

C:\Samples\HelloWorld\HelloWorld\bin\Debug\HelloWorld.dll

And the AddIn files are as follows:

C:\ProgramData\Autodesk\Revit\Addins\2018\HelloWorld.addin

An AddIn manifest is a file located in a specific location checked by Revit when the application starts. The manifest includes information used by Revit to load and run the plug-in. The file path to insert the AddIn file will vary depending on the location of the Revit instillation. Again, the usual path is as follows:

C:\ProgramData\Autodesk\Revit\Addins\2018\HelloWorld.addin

The year 2018 in the above file path may vary as well, depending on which version of Revit the user has. I do not recommend using a different year other than 2018 for these specific plug-ins due to the fact it may cause a myriad of errors/problems down the road (It may work fine as well, I dont know, what am I a pantomath?).

If you are rather a beginner to file navigation as well, then you may notice that “ProgramData” does not appear in the “C:\” drive options. The reason some files/folders are automatically marked as hidden is because, unlike other data like your pictures and documents, they're not files that you should be changing, deleting, or moving around. These are often important operating system-related files.

To open this folder simply type “C:\ProgramData” as the file path and click enter.

Currently there are 4 AddIn files. Each can be accessed/used in Revit 2018 by navigating to the AddIns tab, then clicking external tools on the far left of the screen. External tools will have a drop-down list of each AddIn command. Each AddIn functionality is described as below.

## Copyright
If you plan on using any or all of this code please check out the licence conditions, they are pretty easy to abide by. :)


