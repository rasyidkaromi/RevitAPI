using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Imaging;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;

namespace T_App_HelloWorld
{
    public class Class1 : IExternalApplication
    {
        public Result OnStartup(UIControlledApplication application)
        {
            // Add a new ribbon panel
            RibbonPanel ribbonPanel = application.CreateRibbonPanel("HWribbonPanel");

            // Create a push button to trigger a command add it to the ribbon panel.
            string thisAssemblyPath = Assembly.GetExecutingAssembly().Location;
            PushButtonData buttonData = new PushButtonData("cmdHelloWorld", "T_App_HelloWorld", thisAssemblyPath, "T_App_HelloWorld.HelloWorld");

            PushButton pushButton = ribbonPanel.AddItem(buttonData) as PushButton;

            // Optionally, other properties may be assigned to the button
            // a) tool-tip
            pushButton.ToolTip = "Say hello to the entire world.";

            // b) large bitmap
            Uri uriImage = new Uri(@"C:\Samples\AddPanel\AddPanel\bin\Debug\hacker_icon.png");
            BitmapImage largeImage = new BitmapImage(uriImage);
            pushButton.LargeImage = largeImage;

            return Result.Succeeded;
        }// end OnStartup

        public Result OnShutdown(UIControlledApplication application)
        {
            // Nothing
            return Result.Succeeded;
        }// end OnShutDown


    }// end Class1

    [Autodesk.Revit.Attributes.Transaction(Autodesk.Revit.Attributes.TransactionMode.Manual)]
    public class HelloWorld : IExternalCommand
    {
        public Result Execute(ExternalCommandData revit, ref string message, ElementSet elements)
        {
            TaskDialog.Show("Title - 343 is a garbage company", "Hello World! The new star wars trilogy sucks");
            return Result.Succeeded;
        }

    }


}// end namespace T_App_HelloWorld
