using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Imaging;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using Autodesk.Revit.UI.Selection;

namespace CustomTabRibbon
{
    class SelectFilterWalls : IExternalApplication
    {
        // Both OnStartup and OnShutdown must be implemented as public method
        public Result OnStartup(UIControlledApplication application)
        {
            // Add a new ribbon panel
            RibbonPanel ribbonPanel = application.CreateRibbonPanel("Filter RibbonPanel");

            // Create a push button to trigger a command add it to the ribbon panel.
            string thisAssemblyPath = Assembly.GetExecutingAssembly().Location;
            PushButtonData buttonData = new PushButtonData("cmdHelper", "Select & Filter \nElements", thisAssemblyPath, "CustomTabRibbon.Helper");

            PushButton pushButton = ribbonPanel.AddItem(buttonData) as PushButton;

            // Optionally, other properties may be assigned to the button
            // a) tool-tip
            pushButton.ToolTip = "Filter those elements man!.";

            // b) large bitmap
            Uri uriImage = new Uri(@"C:\Samples\FilterRibbonButton\FilterRibbonButton\bin\Debug\door_icon.png");
            BitmapImage largeImage = new BitmapImage(uriImage);
            pushButton.LargeImage = largeImage;

            return Result.Succeeded;
        }

        public Result OnShutdown(UIControlledApplication application)
        {
            // nothing to clean up in this simple case
            return Result.Succeeded;
        }
    }//end SelectFilterWalls


    [Autodesk.Revit.Attributes.Transaction(Autodesk.Revit.Attributes.TransactionMode.ReadOnly)]
    public class Helper : IExternalCommand
    {
        public Result Execute(ExternalCommandData commandData, ref string message, ElementSet elements)
        {
            try
            {
                // Select some elements in Revit before invoking this command

                // Get the handle of current document.
                UIDocument uidoc = commandData.Application.ActiveUIDocument;

                // Get the element selection of current document.
                Selection selection = uidoc.Selection;
                ICollection<ElementId> selectedIds = uidoc.Selection.GetElementIds();

                // Display current number of selected elements
                TaskDialog.Show("Revit", "Number of selected elements: " + selectedIds.Count.ToString());

                // Go through the selected items and filter out walls only.
                ICollection<ElementId> selectedWallIds = new List<ElementId>();

                foreach (ElementId id in selectedIds)
                {
                    Element elementss = uidoc.Document.GetElement(id);
                    if (elementss is Wall)
                    {
                        selectedWallIds.Add(id);
                    }
                }

                // Set the created element set as current select element set.
                uidoc.Selection.SetElementIds(selectedWallIds);

                // Give the user some information.
                if (0 != selectedWallIds.Count)
                {
                    TaskDialog.Show("Revit", selectedWallIds.Count.ToString() + " Walls are selected!");
                }
                else
                {
                    TaskDialog.Show("Revit", "No Walls have been selected!");
                }
            }
            catch (Exception e)
            {
                message = e.Message;
                return Result.Failed;
            }

            //TaskDialog.Show("Revit", "Hello World WAAAAAHOOOOOO");
            return Result.Succeeded;

        }//end Execute

    }//end Helper



}
