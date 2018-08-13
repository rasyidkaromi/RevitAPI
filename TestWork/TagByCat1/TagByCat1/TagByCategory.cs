using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Imaging;
using Autodesk.Revit.Attributes;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using Autodesk.Revit.UI.Selection;

namespace TagByCat1
{
    public class TagByCategory : IExternalApplication
    {
        // Both OnStartup and OnShutdown must be implemented as public method
        public Result OnStartup(UIControlledApplication application)
        {
            // Add a new ribbon panel
            RibbonPanel ribbonPanel = application.CreateRibbonPanel("Tag By Category");

            // Create a push button to trigger a command add it to the ribbon panel.
            string thisAssemblyPath = Assembly.GetExecutingAssembly().Location;
            PushButtonData buttonData = new PushButtonData("cmdClass1", "Speak friend and Enter", thisAssemblyPath, "TagByCat1.Class1");

            PushButton pushButton = ribbonPanel.AddItem(buttonData) as PushButton;

            // Optionally, other properties may be assigned to the button
            // a) tool-tip
            pushButton.ToolTip = "Hopefully this will tag things...";

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

            CreateIndependentTag(doc, e);

            trans.Commit();
            
            return Result.Succeeded;

        }// end Execute

        public IndependentTag CreateIndependentTag(Document document, Element wall)
        {
            TaskDialog.Show("Create Independent Tag Method", "Start Of Method Dialog");
            // make sure active view is not a 3D view
            var view = document.ActiveView;

            // define tag mode and tag orientation for new tag
            var tagMode = TagMode.TM_ADDBY_CATEGORY;
            var tagorn = TagOrientation.Horizontal;

            // Add the tag to the middle of the wall
            var wallLoc = wall.Location as LocationCurve;
            var wallStart = wallLoc.Curve.GetEndPoint(0);
            var wallEnd = wallLoc.Curve.GetEndPoint(1);
            var wallMid = wallLoc.Curve.Evaluate(0.5, true);
            var wallRef = new Reference(wall);

            ////
            
//            var newTag = IndependentTag.Create(document, view.Id, wallRef, true, tagMode, tagorn, wallMid) as object;
//            newTag = new OverrideGraphicSettings().SetHalftone(true);

            var newTag = IndependentTag.Create(document, view.Id, wallRef, true, tagMode, tagorn, wallMid);

//            newTag = new OverrideGraphicSettings().SetHalftone(true);
            
            // Element does not define who I am, only what I do. And what i do defines who I am. So i guess it does define me lmao
//            Element Elmnt; //This is vey difficult to understand lmaoooooo
//            Object cc = new Object();



            if (null == newTag) throw new Exception("Create IndependentTag Failed.");

            // newTag.TagText is read-only, so we change the Type Mark type parameter to 
            // set the tag text.  The label parameter for the tag family determines
            // what type parameter is used for the tag text.
            
//            var type = wall.WallType;
//            var foundParameter = type.LookupParameter("Type Mark");
//            var result = foundParameter.Set("Hello");

            // set leader mode free
            // otherwise leader end point move with elbow point

            newTag.LeaderEndCondition = LeaderEndCondition.Free;
            var elbowPnt = wallMid + new XYZ(5.0, 5.0, 0.0);
            newTag.LeaderElbow = elbowPnt;
            var headerPnt = wallMid + new XYZ(10.0, 10.0, 0.0);
            newTag.TagHeadPosition = headerPnt;

            int x = wall.CreatedPhaseId.IntegerValue;
            // string y = x.ToString();
            TaskDialog.Show("Revit", " " + x);

            // STUCK RIGHT HERE UGH HELPPPPPPPPPPPPPPP
//            Object temp = new OverrideGraphicSettings().SetHalftone(true);


            TaskDialog.Show("Test5", "BEFORE");

//            Object test = new Reference(newTag);
//            test = new OverrideGraphicSettings().SetHalftone(true);
//            var refer = new Reference(newTag) as Object;
//            refer = new OverrideGraphicSettings().SetHalftone(true);
//            Element refer = newTag;
//            refer = new Reference(newTag) as object;
//            refer = new OverrideGraphicSettings().SetHalftone(true);

            Object myObject = new Object();

            

//            OverrideGraphicSettings tmp;
            


            Object er = new OverrideGraphicSettings().SetHalftone(true);
            TaskDialog.Show("Test5", "AFTER");

            if (wall.CreatedPhaseId.IntegerValue == 32440)// 32440 is existing (white) 
            {
                string xc = newTag.GetTaggedLocalElement().ToString();
                TaskDialog.Show("Test5", "?? - " + xc);

                //newTag

            }
            else //(wall.CreatedPhaseId.IntegerValue == 21885)// 21885 is new (grey) 
            {

            }

            TaskDialog.Show("Create Independent Tag Method", "End Of Method Dialog");

            return newTag;

            // To Move the tag, in this case 1 foot in the X direction and 1 foot in the Y direction use the element transform utilities:
            // XYZ translation = new XYZ(1, 1, 0);
            // ElementTransformUtils.MoveElement(document, tag.Id, translation);
        }


    }// end Class1
}
