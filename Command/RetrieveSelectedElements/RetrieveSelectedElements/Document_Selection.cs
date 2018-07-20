using System;
using Autodesk.Revit;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI.Selection;
using Autodesk.Revit.UI;
using Autodesk.Revit.Attributes;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace RetrieveSelectedElements
{
    [Autodesk.Revit.Attributes.Transaction(Autodesk.Revit.Attributes.TransactionMode.ReadOnly)]
    public class Document_Selection : IExternalCommand
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

                if (0 == selectedIds.Count)
                {
                    // If no elements selected.
                    TaskDialog.Show("Revit", "You haven't selected any elements dude, what are you doing with your life?");
                }
                else
                {
                    String info = "IDs of selected elements in the document are: ";
                    foreach (ElementId id in selectedIds)
                    {
                        info += "\n\t" + id.IntegerValue;
                    }

                    TaskDialog.Show("Revit", info);
                }
            }
            catch (Exception e)
            {
                message = e.Message;
                return Result.Failed;
            }
            
            return Result.Succeeded;

        }//end Execute


    }
    

}
