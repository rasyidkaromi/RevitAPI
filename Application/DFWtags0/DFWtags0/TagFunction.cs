using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mime;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Imaging;
using Autodesk.Revit.Attributes;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using Autodesk.Revit.UI.Selection;

// New: 21885 - Gray Duct - Black Tag
// Existing: 32440 - White Duct - Halftone Tag

namespace DFWtags0
{
    [Transaction(TransactionMode.Manual)]
    public class TagFunction : IExternalCommand
    {
        public Result Execute(ExternalCommandData commandData, ref string message, ElementSet elements)
        {

            UIDocument uidoc = commandData.Application.ActiveUIDocument;
            Document doc = uidoc.Document;

            Reference myRef = uidoc.Selection.PickObject(ObjectType.PointOnElement);
            Element e1 = doc.GetElement(myRef);
            // var elementREF = doc.GetElement(reference);

            if (e1 == null || !(e1 is Element e))
            {
                TaskDialog.Show("Error", "Selected element was not a wall");
                return Result.Failed;
            }

            Transaction trans = new Transaction(doc, "Creating tag");
            // Transaction starts and calls CreateIndependentTag method
            trans.Start();
            CreateIndependentTag(doc, e, commandData, myRef);
            trans.Commit();

            return Result.Succeeded;

        }// end Execute



        public IndependentTag CreateIndependentTag(Document document, Element duct, ExternalCommandData commandData, Reference myRef)
        {
            // TaskDialog.Show("Create Independent Tag Method", "Start Of Method Dialog");
            // Make sure active view is not a 3D view
            var view = document.ActiveView;

            UIApplication uiapp = commandData.Application;
            Document uidoc = uiapp.ActiveUIDocument.Document;
             
            var my = myRef.GlobalPoint;
            // TaskDialog.Show("Global Point", "GP - " + my);            
            
            // GetEdges(duct);

            // define tag mode and tag orientation for new tag
            // Tag Mode - lets tag know what category we are tagging
            var tagMode = TagMode.TM_ADDBY_CATEGORY;
            // Tag Orientation - Vertical/Horizontal
            var tagorn = TagOrientation.Horizontal;

            var ductLoc = duct.Location as LocationCurve;
            // var ductStart = ductLoc.Curve.GetEndPoint(0);
            // var ductEnd = ductLoc.Curve.GetEndPoint(1);
            var ductMid = ductLoc.Curve.Evaluate(.5, true);
            
            // Create a reference to the duct selected so we can pass said reference into the independentTag Parameter
            var ductRef = new Reference(duct);

            // Create New Tag
            var newTag = IndependentTag.Create(document, view.Id, ductRef, true, tagMode, tagorn, my);

            if (null == newTag) throw new Exception("Create IndependentTag Failed.");

            // newTag.TagText is read-only, so we change the Type Mark type parameter to 
            // set the tag text.  The label parameter for the tag family determines
            // what type parameter is used for the tag text.
            // var result = foundParameter.Set("Hello");

            // set leader mode free
            newTag.LeaderEndCondition = LeaderEndCondition.Free;

//            Selection sel2 = uiapp.ActiveUIDocument.Selection;
//            XYZ point2 = sel2.PickPoint();
//            var leadEnd = point2;
//            newTag.LeaderEnd = my;
//            TaskDialog.Show("Point2", "P2 - " + point2);

            double aa = my.X;
            double bb = ductMid.Y;
            double cc = ductMid.Z;
            var LeadEnd = new XYZ(aa, bb, cc);
            newTag.LeaderEnd = LeadEnd;


            Selection sel3 = uiapp.ActiveUIDocument.Selection;
            XYZ point3 = sel3.PickPoint();
            var elbowPnt = point3;

            // Force positive number;
            double variance = Math.Abs(LeadEnd.X - point3.X);
            // TaskDialog.Show("Variance", "Variacne = " + variance);

            // This if statement allows for right angles to be created. The user will try to set a right angle
            // and if it is close enough it will set the right angle for you... THIS WILL CHANGE IN THE FUTURE
            if (variance < 1.0)
            {
                double a = LeadEnd.X;
                double b = point3.Y;
                double c = point3.Z;
                elbowPnt = new XYZ(a, b, c);
                newTag.LeaderElbow = elbowPnt;
            }
            else
            {
                newTag.LeaderElbow = elbowPnt;
            }

            double samePoint = elbowPnt.Y;

            Selection sel4 = uiapp.ActiveUIDocument.Selection;
            XYZ point4 = sel4.PickPoint();

            double x = point4.X;
            double y = samePoint;
            double z = point4.Z;
            var headerPnt = new XYZ(x, y + .03, z);
            newTag.TagHeadPosition = headerPnt;


            // Call ElementOverride method if the DUCT is an EXISTING DUCT        
            if (duct.CreatedPhaseId.IntegerValue == 32440) // 32440 is existing (white) - need halftone
            {
                ElementId i = newTag.Id;
                // Call element override
                ElementOverride(commandData, i);
            }

            // TaskDialog.Show("Create Independent Tag Method", "End Of Method Dialog");
            return newTag;

        }// end CreateIndependentTag


        // GetEdges will return each edge length from the chosen element (Like a duct)
        private void GetEdges(Element wall)
        {
            String EdgeInfo = "";

            Options opt = new Options();
            GeometryElement geomElem = wall.get_Geometry(opt);
            foreach (GeometryObject geomObj in geomElem)
            {
                Solid geomSolid = geomObj as Solid;
                if (null != geomSolid)
                {
                    int edges = 0;
                    double totalLength = 0;
                    foreach (Edge geomEdge in geomSolid.Edges)
                    {
                        // get wall's geometry edges
                        edges++;
                        EdgeInfo += "Edge " + edges + " Length: " + geomEdge.ApproximateLength + "\n";
                        totalLength += geomEdge.ApproximateLength;
                    }
                }
            }
            TaskDialog.Show("Revit", EdgeInfo);

        }// End GetEdges  


        // ElementOverride allows for the user to change cosmetic/physical parts of an element (In this case we change tag color to halftone)
        public void ElementOverride(ExternalCommandData commandData, ElementId i)
        {
            UIDocument uidoc = commandData.Application.ActiveUIDocument;
            Document doc = uidoc.Document;
            ElementId id = i; // = uidoc.Selection.PickObject(ObjectType.Element, "Select an element").ElementId;
            OverrideGraphicSettings ogs = new OverrideGraphicSettings();

            ogs.SetHalftone(true);
            // Transaction already created...
            // Transaction t = new Transaction(doc, "Set Element Override");           
            // t.Start();
            doc.ActiveView.SetElementOverrides(id, ogs);
            // t.Commit();

        }// End ElementOverride



    }// end Class1

}// End Namespace
