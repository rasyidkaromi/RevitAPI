using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Imaging;
using Autodesk.Revit.Attributes;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using Autodesk.Revit.UI.Selection;

/* If this were C++ this would be my - Main()
 * 
 */

namespace TagByCatVersions
{
    public class TabApp : IExternalApplication
    {
        // Both OnStartup and OnShutdown must be implemented as public method
        public Result OnStartup(UIControlledApplication a)
        {

            //Allow for the folderpath to connect dll file as well as images 
            //Do this for efficiency
            string folderPath = @"C:\Samples\TagByCatVersions\TagByCatVersions\bin\Debug";
            string dll = Path.Combine(folderPath, "TagByCatVersions.dll");

            //Create Custom Tab
            string TheRibbon = "-- TAG TAB --";
            a.CreateRibbonTab(TheRibbon);

            //Create Seperated Panels
            RibbonPanel panelA = a.CreateRibbonPanel(TheRibbon, "Tag Category Versions");
            RibbonPanel panelB = a.CreateRibbonPanel(TheRibbon, "B");
            RibbonPanel panelC = a.CreateRibbonPanel(TheRibbon, "C");
            RibbonPanel panelD = a.CreateRibbonPanel(TheRibbon, "D");
            RibbonPanel panelE = a.CreateRibbonPanel(TheRibbon, "E");
            RibbonPanel panelF = a.CreateRibbonPanel(TheRibbon, "F");


            // Button TagByCat1
            PushButton btnOne = (PushButton) panelA.AddItem(new PushButtonData("One", "TagByCat1", dll, "TagByCatVersions.TagByCat1"));
            //btnOne.LargeImage = new BitmapImage(new Uri(Path.Combine(folderPath, "image32.png"), UriKind.Absolute));
            Uri uriImage = new Uri(@"C:\Samples\TagByCatVersions\TagByCatVersions\bin\Debug\32Groot.png");
            BitmapImage largeImage = new BitmapImage(uriImage);
            btnOne.LargeImage = largeImage; //Actually attaches image to button one - panel A
            btnOne.ToolTip = "Tag Color (Halftone)";
            btnOne.LongDescription = "This is a lonnnnnnnnnnnnng description!";

            // Button TagByCat2
            PushButton btnTwo = (PushButton) panelA.AddItem(new PushButtonData("Two", "TagByCat2", dll, "TagByCatVersions.TagByCat2"));
            Uri uriImage2 = new Uri(@"C:\Samples\TagByCatVersions\TagByCatVersions\bin\Debug\32Hacker.png");
            BitmapImage largeImage2 = new BitmapImage(uriImage2);
            btnTwo.LargeImage = largeImage2;
            btnTwo.ToolTip = "Tag Placement (Points)";
            btnTwo.LongDescription = "This is a lonnnnnnnnnnnnng description!";

            // Button TagByCat3
            PushButton btnThree = (PushButton) panelA.AddItem(new PushButtonData("Three", "TagByCat3", dll, "TagByCatVersions.TagByCat3"));
            Uri uriImage3 = new Uri(@"C:\Samples\TagByCatVersions\TagByCatVersions\bin\Debug\32Ironman.png");
            BitmapImage largeImage3 = new BitmapImage(uriImage3);
            btnThree.LargeImage = largeImage3;
            btnThree.ToolTip = "Functional";
            btnThree.LongDescription = "This is a lonnnnnnnnnnnnng description!";

            // Button TagByCat4
            PushButton btnFour = (PushButton)panelA.AddItem(new PushButtonData("Four", "TagByCat4", dll, "TagByCatVersions.TagByCat4"));
            btnFour.LargeImage = new BitmapImage(new Uri(Path.Combine(folderPath, "32Ironman.png"), UriKind.Absolute));
            //            PushButton btnFour = (PushButton) panelA.AddItem(new PushButtonData("Four", "TagByCat4", dll, "TagByCatVersions.TagByCat4"));
            //            Uri uriImage4 = new Uri(@"C:\Samples\TagByCatVersions\TagByCatVersions\bin\Debug\24Penguin.png");
            //            BitmapImage largeImage4 = new BitmapImage(uriImage4);
            //            btnFour.LargeImage = largeImage4;
            //            btnFour.ToolTip = "Cleaned up";
            //            btnFour.LongDescription = "This is a lonnnnnnnnnnnnng description!";


            // Button CreateLine
            PushButton CreateLine = (PushButton) panelB.AddItem(new PushButtonData("Four", "CreateLine", dll, "TagByCatVersions.CreateLine"));
            Uri uriImage6 = new Uri(@"C:\Samples\TagByCatVersions\TagByCatVersions\bin\Debug\24Penguin.png");
            BitmapImage largeImage6 = new BitmapImage(uriImage6);
            CreateLine.LargeImage = largeImage6;
            CreateLine.ToolTip = "Line Creation";
            CreateLine.LongDescription = "This is a lonnnnnnnnnnnnng description!";



            return Result.Succeeded;
        }

        public Result OnShutdown(UIControlledApplication application)
        {
            // nothing to clean up in this simple case
            return Result.Succeeded;
        }

    } //

}//end App
