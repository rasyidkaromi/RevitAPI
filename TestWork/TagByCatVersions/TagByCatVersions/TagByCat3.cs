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

namespace TagByCatVersions
{

    [Transaction(TransactionMode.Manual)]
    public class TagByCat3 : IExternalCommand
    {
        public Result Execute(ExternalCommandData commandData, ref string message, ElementSet elements)
        {

            UIDocument uidoc = commandData.Application.ActiveUIDocument;
            Document doc = uidoc.Document;

            Reference reference;
            try
            {
                reference = uidoc.Selection.PickObject(ObjectType.Element, "Pick an element");
            }
            catch
            {
                return Result.Cancelled;
            }

            var elementREF = doc.GetElement(reference);

            if (elementREF == null || !(elementREF is Element e))
            {
                TaskDialog.Show("Error", "Selected element was not a wall");
                return Result.Failed;
            }

            Transaction trans = new Transaction(doc, "Creating tag");

            trans.Start();

            CreateIndependentTag(doc, e, commandData);

            trans.Commit();

            return Result.Succeeded;

        }// end Execute

        

        public IndependentTag CreateIndependentTag(Document document, Element duct, ExternalCommandData commandData)
        {
            // TaskDialog.Show("Create Independent Tag Method", "Start Of Method Dialog");
            // make sure active view is not a 3D view
            var view = document.ActiveView;

            UIApplication uiapp = commandData.Application;
            Document uidoc = uiapp.ActiveUIDocument.Document;
            // ObjectSnapTypes snapTypes = ObjectSnapTypes.Perpendicular;
            // Reference pickedref = null;
            // Selection sel = uiapp.ActiveUIDocument.Selection;
            // XYZ point = sel.PickPoint(snapTypes, "Select an end point or intersection");

            //            GetEdges(duct);

            // define tag mode and tag orientation for new tag
            var tagMode = TagMode.TM_ADDBY_CATEGORY;
            var tagorn = TagOrientation.Horizontal;

            // Add the tag to the middle of the duct
            var ductLoc = duct.Location as LocationCurve;
            var ductStart = ductLoc.Curve.GetEndPoint(0);
            var ductEnd = ductLoc.Curve.GetEndPoint(1);
            var ductMid = ductLoc.Curve.Evaluate(.5, true);


            var ductRef = new Reference(duct);
            var newTag = IndependentTag.Create(document, view.Id, ductRef, true, tagMode, tagorn, ductMid);


//            Object newTag = new OverrideGraphicSettings().SetHalftone(true);
//            var newTag = IndependentTag.Create(document, view.Id, ductRef, true, tagMode, tagorn, ductMid);
//            Object hold = new OverrideGraphicSettings();
           


            if (null == newTag) throw new Exception("Create IndependentTag Failed.");

            // newTag.TagText is read-only, so we change the Type Mark type parameter to 
            // set the tag text.  The label parameter for the tag family determines
            // what type parameter is used for the tag text.
            // var result = foundParameter.Set("Hello");
            // set leader mode free

            
            newTag.LeaderEndCondition = LeaderEndCondition.Free;

            Selection sel2 = uiapp.ActiveUIDocument.Selection;
            XYZ point2 = sel2.PickPoint();
            var leadEnd = point2; 
            newTag.LeaderEnd = leadEnd;

            Selection sel3 = uiapp.ActiveUIDocument.Selection;
            XYZ point3 = sel3.PickPoint();
            var elbowPnt = point3;
            //            newTag.LeaderElbow = elbowPnt;

            // Attempt to make leaderEnd and Elbow vertical if wanted
            // force positive number;
            double variance = Math.Abs(point2.X - point3.X);         

            // TaskDialog.Show("Variance", "Variacne = " + variance);

            if (variance < 1.0)
            {
                double a = point2.X;
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
            // var headerPnt = point4;
            double x = point4.X;
            double y = samePoint;
            double z = point4.Z;
            var headerPnt = new XYZ(x, y + .03, z);
            newTag.TagHeadPosition = headerPnt;

//            newTag.LeaderEndCondition = LeaderEndCondition.Attached;


            //call halftone method if the DUCT is an EXISTING DUCT        
            if(duct.CreatedPhaseId.IntegerValue == 32440) // 32440 is existing (white) 
            {
                ElementId i = newTag.Id;
                ElementOverride(commandData, i);
            }

            //TaskDialog.Show("Create Lines", "Begin");

            //            Selection sell1 = uiapp.ActiveUIDocument.Selection;
            //            XYZ p1 = sell1.PickPoint(snapTypes, "Select an end point or intersection");
            //
            //            Selection sell2 = uiapp.ActiveUIDocument.Selection;
            //            XYZ p2 = sell2.PickPoint(snapTypes, "Select an end point or intersection");
            //
            //            Line L1 = Line.CreateBound(p1, p2);
            //            uidoc.Create.NewDetailCurve(view, L1);

            // Document doc = application.ActiveUIDocument.Document;


            // TaskDialog.Show("Create Independent Tag Method", "End Of Method Dialog");
            return newTag;


        }// end CreateIndependentTag

        private void GetEdges(Element wall)
        {
            //            String faceInfo = "";
            String EdgeInfo = "";

            Options opt = new Options();
            GeometryElement geomElem = wall.get_Geometry(opt);
            foreach (GeometryObject geomObj in geomElem)
            {
                Solid geomSolid = geomObj as Solid;
                if (null != geomSolid)
                {
                    //                    int faces = 0;
                    //                    double totalArea = 0;
                    //                    foreach (Face geomFace in geomSolid.Faces)
                    //                    {
                    //                        faces++;
                    //                        faceInfo += "Face " + faces + " area: " + geomFace.Area.ToString() + "\n";
                    //                        totalArea += geomFace.Area;
                    //                    }
                    //                    faceInfo += "Number of faces: " + faces + "\n";
                    //                    faceInfo += "Total area: " + totalArea.ToString() + "\n";

                    int edges = 0;
                    double totalLength = 0;
                    foreach (Edge geomEdge in geomSolid.Edges)
                    {
                        // get wall's geometry edges
                        edges++;
                        EdgeInfo += "Edge " + edges + " Length: " + geomEdge.ApproximateLength.ToString() + "\n";
                        totalLength += geomEdge.ApproximateLength;

                    }
                }
            }
            TaskDialog.Show("Revit", EdgeInfo);

        }// End GetEdges   

        public void ElementOverride(ExternalCommandData commandData, ElementId i)
        {
            UIDocument uidoc = commandData.Application.ActiveUIDocument;
            Document doc = uidoc.Document;
            ElementId id = i;
                // = uidoc.Selection.PickObject(ObjectType.Element, "Select an element").ElementId;
            OverrideGraphicSettings ogs = new OverrideGraphicSettings();

            ogs.SetHalftone(true);
            // Transaction t = new Transaction(doc, "Set Element Override");           
            // t.Start();
            doc.ActiveView.SetElementOverrides(id, ogs);
            // t.Commit();
            
        }// End ElementOverride

    }// end Class1
   

}// End Namespace
