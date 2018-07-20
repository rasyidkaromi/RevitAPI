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

            ////////////////////////////////////
            




            

            ////////////////////////////////////
            
            return Result.Succeeded;

        }//end Execute

//        public ICollection<Element> CreateLogicAndFilter(Document document)
//        {
//            // Find all door instances in the project by finding all elements that both belong to the door 
//            // category and are family instances.
//            ElementClassFilter familyInstanceFilter = new ElementClassFilter(typeof(FamilyInstance));
//
//            // Create a category filter for Doors
//            ElementCategoryFilter doorsCategoryfilter = new ElementCategoryFilter(BuiltInCategory.OST_Doors);
//
//            // Create a logic And filter for all Door FamilyInstances
//            LogicalAndFilter doorInstancesFilter = new LogicalAndFilter(familyInstanceFilter, doorsCategoryfilter);
//
//            // Apply the filter to the elements in the active document
//            FilteredElementCollector collector = new FilteredElementCollector(document);
//            IList<Element> doors = collector.WherePasses(doorInstancesFilter).ToElements();
//
//            string tmp = "HALF LIFE TEST: ";
//            foreach (Element item in doors)
//            {
//                tmp += "\n\t" + item.Name;
//            }
//
//            TaskDialog.Show("Revit", tmp);
//
//            return doors;
//        }

    }

    

}