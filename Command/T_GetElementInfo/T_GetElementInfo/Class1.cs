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

namespace T_GetElementInfo
{
    [Autodesk.Revit.Attributes.Transaction(Autodesk.Revit.Attributes.TransactionMode.ReadOnly)]
    public class Class1 : IExternalCommand //Command NOT Application
    {
        public Result Execute(ExternalCommandData commandData, ref string message, ElementSet elements)
        {

            //UIDocument is infomation about how the revit user interacts
            //with the RVT file. Info about view windows and selection of elements
            UIDocument uidoc = commandData.Application.ActiveUIDocument;

            //Database Document - stores info about walls, floors, parameters, ect
            //Do not need this doc but I do anyway...
            Document doc = uidoc.Document;

            // Get the element selection of current document.  -- PickObjects
            // Reference myReff = uidoc.Selection.PickObject(ObjectType.Element);
            ICollection<Reference> myRef = uidoc.Selection.PickObjects(ObjectType.Element);

            //show count
            TaskDialog.Show("Revit", "Number of selected elements: " + myRef.Count.ToString());

            string data = "Current Element Info: ";
            var temp = 0;
            Element e;

            foreach (Reference c in myRef)
            {
                // e = uidoc.Document.GetElement(c);
                e = doc.GetElement(c);
                data += "\n\t" + temp + "\nCategory: " + e.Category + "\nUniqueId: " + e.UniqueId + "\nCreatedPhaseID: " + e.CreatedPhaseId + "\nName: " + e.Name + "\nID: " + e.Id + "\n --END-- ";
                temp++;
            }

            TaskDialog.Show("Revit4343", data);
            
            return Result.Succeeded;
        }

    }
}
