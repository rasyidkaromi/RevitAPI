using System;
using Autodesk.Revit;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI.Selection;
using Autodesk.Revit.UI;
using Autodesk.Revit.Attributes;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Windows.Media.Imaging;

namespace RetrieveFilteredElements
{

    [Transaction(TransactionMode.ReadOnly)]
    public class Class1 : IExternalCommand
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

    }//end Class1

}//end Namespace