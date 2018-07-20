When downloading these files, make sure the file path is correct due to the fact that the addin files depend on where the folders of code have been saved for each plugin.

In my instance, the folders with c# code for Visual Studio are saved here:

<Assembly>C:\Samples\HelloWorld\HelloWorld\bin\Debug\HelloWorld.dll</Assembly>

And the addin files are as follows:

C:\ProgramData\Autodesk\Revit\Addins\2018\HelloWorld.addin

An AddIn manifest is a file located in a specific location checked by Revit when the application starts. The manifest includes information used by Revit to load and run the plug-in. The file path to insert the addin file will vary depending on the location of the Revit instillation. Again, the usual path is as follows: 

C:\ProgramData\Autodesk\Revit\Addins\2018\HelloWorld.addin

The year 2018 may vary as well, depending on which version of Revit the user has. I do not recommend using a different year other than 2018 for these specific plug-ins due to the fact it may cause a myriad of errors/problems down the road(It may work fine as well, I dont fucking know, what am I a pantomath?).

If you are rather a beginner to file navigation as well, then you may notice that “ProgramData” does not appear in the “C:\” drive options. The reason some files/folders are automatically marked as hidden is because, unlike other data like your pictures and documents, they're not files that you should be changing, deleting, or moving around. These are often important operating system-related files.

To open this folder simply type “C:\ProgramData” as the file path and click enter.

Currently there are 4 addin files:
HelloWorld.addin

CopyPasteGroup.addin

RetrieveSelectedElements.addin

RetrieveFilteredElements.addin
