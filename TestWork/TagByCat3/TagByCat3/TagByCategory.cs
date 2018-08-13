using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mime;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Imaging;
using Autodesk.Revit.Attributes;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using Autodesk.Revit.UI.Selection;

/*  Version TagByCat2 will focus on the leader length as well as landing length. Basic Functionality is here and will not worry about color
 *  of the tag, only functionality.
 */

namespace TagByCat3
{
    public class TagByCategory : IExternalApplication
    {
        // Both OnStartup and OnShutdown must be implemented as public method
        public Result OnStartup(UIControlledApplication application)
        {
            // Add a new ribbon panel
            RibbonPanel ribbonPanel = application.CreateRibbonPanel("Tag By Category 3");
            // Create a push button to trigger a command add it to the ribbon panel.
            string thisAssemblyPath = Assembly.GetExecutingAssembly().Location;
            PushButtonData buttonData = new PushButtonData("cmdClass1", "Tag Line Test", thisAssemblyPath, "TagByCat3.Class1");
            PushButton pushButton = ribbonPanel.AddItem(buttonData) as PushButton;

            // Optionally, other properties may be assigned to the button
            // a) tool-tip
            pushButton.ToolTip = "Hopefully this will tag things...";

            // b) large bitmap
            Uri uriImage = new Uri(@"C:\Samples\FilterRibbonButton\FilterRibbonButton\bin\Debug\door_icon.png");
            BitmapImage largeImage = new BitmapImage(uriImage);
            pushButton.LargeImage = largeImage;


            ///////////////////////////////////////////FOR SECOND CLASS




            return Result.Succeeded;
        }

        public Result OnShutdown(UIControlledApplication application)
        {
            // nothing to clean up in this simple case
            return Result.Succeeded;
        }

    }// end TagByCategory

    [Transaction(TransactionMode.Manual)]
    public class Class1 : IExternalCommand
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
            ObjectSnapTypes snapTypes = ObjectSnapTypes.Perpendicular;
            //            Reference pickedref = null;
            //            Selection sel = uiapp.ActiveUIDocument.Selection;
            //            XYZ point = sel.PickPoint(snapTypes, "Select an end point or intersection");

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

            if (null == newTag) throw new Exception("Create IndependentTag Failed.");

            // newTag.TagText is read-only, so we change the Type Mark type parameter to 
            // set the tag text.  The label parameter for the tag family determines
            // what type parameter is used for the tag text.
            // var result = foundParameter.Set("Hello");
            // set leader mode free


//            newTag.LeaderEndCondition = LeaderEndCondition.Free;
//
//            Selection sel2 = uiapp.ActiveUIDocument.Selection;
//            XYZ point2 = sel2.PickPoint(snapTypes, "Select an end point or intersection");
//            var leadEnd = point2; 
//            newTag.LeaderEnd = leadEnd;
//
//            Selection sel3 = uiapp.ActiveUIDocument.Selection;
//            XYZ point3 = sel3.PickPoint(snapTypes, "Select an end point or intersection");
//            var elbowPnt = point3;
//            newTag.LeaderElbow = elbowPnt;
//
//            Selection sel4 = uiapp.ActiveUIDocument.Selection;
//            XYZ point4 = sel4.PickPoint(snapTypes, "Select an end point or intersection");
//            var headerPnt = point4;
//            newTag.TagHeadPosition = headerPnt;
//
//            newTag.LeaderEndCondition = LeaderEndCondition.Attached;

              TaskDialog.Show("Create Lines", "Begin");


            Selection sell1 = uiapp.ActiveUIDocument.Selection;
            XYZ p1 = sell1.PickPoint(snapTypes, "Select an end point or intersection");

            Selection sell2 = uiapp.ActiveUIDocument.Selection;
            XYZ p2 = sell2.PickPoint(snapTypes, "Select an end point or intersection");

            Line L1 = Line.CreateBound(p1,p2);
            uidoc.Create.NewDetailCurve(view,L1);

            TaskDialog.Show("Create Independent Tag Method", "End Of Method Dialog");
            return newTag;


        }// end 

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


    }// end Class1


//    public class Command : IExternalCommand
//    {
//        public Result Execute(ExternalCommandData commandData, ref string message, ElementSet elements)
//        {
//            UIApplication uiapp = commandData.Application;
//            UIDocument uidoc = uiapp.ActiveUIDocument;
//            //                MediaTypeNames.Application app = uiapp.Application;
//            Document doc = uidoc.Document;
//
//            Selection sel1 = uidoc.Selection;
//            List<XYZ> tempXYZ = new List<XYZ>(1);
//            XYZ p1 = sel1.PickPoint();
//            XYZ p2 = null;
//
//
//            //tempXYZ.Add(p3);
//
//            ModelCurve visualLine = null;
//            using (TransactionGroup tGroup = new TransactionGroup(doc))
//            {
//                tGroup.Start();
//
//                Redraw:
//                using (Transaction t = new Transaction(doc))
//                {
//                    t.Start("Step 1");
//
//                    Line line = Line.CreateBound(p1, getP3(uidoc));
//
//                    Plane geomPlane = Plane.CreateByNormalAndOrigin(doc.ActiveView.ViewDirection, doc.ActiveView.Origin);
//                    SketchPlane sketch = SketchPlane.Create(doc, geomPlane);
//                    visualLine = doc.Create.NewModelCurve(line, sketch) as ModelCurve;
//                    doc.Regenerate();
//                    uidoc.RefreshActiveView();
//                    goto Redraw;
//
//                    t.Commit();
//                }
//                tGroup.Commit();
//            }
//            return Result.Succeeded;
//        }
//
//        private XYZ getP3(UIDocument uidoc)
//        {
//            UIView uiview = GetActiveUiView(uidoc);
//            Rectangle rect = uiview.GetWindowRectangle();
//            System.Drawing.Point p = System.Windows.Forms.Cursor.Position;
//            System.Windows.Forms.Cursor.Position = new System.Drawing.Point(p.X, p.Y);
//            double dx = (double)(p.X - rect.Left) / (rect.Right - rect.Left);
//            double dy = (double)(p.Y - rect.Bottom) / (rect.Top - rect.Bottom);
//            IList<XYZ> corners = uiview.GetZoomCorners();
//            XYZ a = corners[0];
//            XYZ b = corners[1];
//            XYZ v = b - a;
//            XYZ p3 = a + dx * v.X * XYZ.BasisX + dy * v.Y * XYZ.BasisY;
//            return p3;
//        }
//        //Convert Document hiện hành thành UIView
//        private UIView GetActiveUiView(UIDocument uidoc)
//        {
//            Document doc = uidoc.Document;
//            View view = doc.ActiveView;
//            IList<UIView> uiviews = uidoc.GetOpenUIViews();
//            UIView uiview = null;
//
//            foreach (UIView uv in uiviews)
//            {
//                if (uv.ViewId.Equals(view.Id))
//                {
//                    uiview = uv;
//                    break;
//                }
//            }
//            return uiview;
//        }
//    }




}
