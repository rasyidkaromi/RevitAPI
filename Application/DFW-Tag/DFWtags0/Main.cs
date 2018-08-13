using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Imaging;
using Autodesk.Revit.UI;

namespace DFWtags0
{
    public class Main : IExternalApplication
    {
        // Both OnStartup and OnShutdown must be implemented as public method
        public Result OnStartup(UIControlledApplication a)
        {

            // Allow for the folderpath to connect dll file as well as images 
            // Do this for efficiency
            string folderPath = @"C:\Samples\DFWtags0\DFWtags0\bin\Debug";
            string dll = Path.Combine(folderPath, "DFWtags0.dll");

            // Create Custom Tab
            string TheRibbon = "DFW-CGI-Tag";
            a.CreateRibbonTab(TheRibbon);

            // Create Seperated Panels
            RibbonPanel panelA = a.CreateRibbonPanel(TheRibbon, "Tag Function");
            RibbonPanel panelB = a.CreateRibbonPanel(TheRibbon, "B");

            // Below is two seperate ways to create the same button with different code. The second way being more efficient
            // Create Button btnOne
            PushButton btnOne = (PushButton)panelA.AddItem(new PushButtonData("One", "Tag", dll, "DFWtags0.TagFunction"));
            Uri uriImage = new Uri(@"C:\Samples\TagByCatVersions\TagByCatVersions\bin\Debug\32Groot.png");
            BitmapImage largeImage = new BitmapImage(uriImage);
            btnOne.LargeImage = largeImage; //Actually attaches image to button one - panel A
            btnOne.ToolTip = "Lets hope this tag works...";
            btnOne.LongDescription = "This is a lonnnnnnnnnnnnng description!";

            // Create Button btnTwo
            PushButton btnTwo = (PushButton)panelA.AddItem(new PushButtonData("Two", "Get Element Info", dll, "DFWtags0.ElementInfo"));
            btnTwo.LargeImage = new BitmapImage(new Uri(Path.Combine(folderPath, "32Ironman.png"), UriKind.Absolute));



            return Result.Succeeded;
        }

        public Result OnShutdown(UIControlledApplication application)
        {
            // Nothing to clean up in this simple case
            return Result.Succeeded;
        }

    }// End Main

}//end App