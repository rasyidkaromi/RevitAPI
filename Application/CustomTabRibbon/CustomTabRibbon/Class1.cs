using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Imaging;
using Autodesk.Revit.Attributes;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;

namespace CustomTabRibbon
{


    [Transaction(TransactionMode.Manual)]
    public class Help : IExternalCommand
    {

        public Result Execute(ExternalCommandData commandData, ref string message, ElementSet elementset)
        {
            
            return Result.Succeeded;
        }

    }//end One

    [Transaction(TransactionMode.Manual)]
    public class One : IExternalCommand
    {

        public Result Execute(ExternalCommandData commandData, ref string message, ElementSet elementset)
        {
            TaskDialog.Show("Revit", "Test 1");
            return Result.Succeeded;
        }
    
    }//end One

    [Transaction(TransactionMode.Manual)]
    public class Two : IExternalCommand
    {

        public Result Execute(ExternalCommandData commandData, ref string message, ElementSet elementset)
        {
            TaskDialog.Show("Revit", "Test 2");
            return Result.Succeeded;
        }

    }//end Two

    [Transaction(TransactionMode.Manual)]
    public class Three : IExternalCommand
    {

        public Result Execute(ExternalCommandData commandData, ref string message, ElementSet elementset)
        {
            TaskDialog.Show("Revit", "Test 3");
            return Result.Succeeded;
        }

    }//end Three

    [Transaction(TransactionMode.Manual)]
    public class help : IExternalCommand
    {

        public Result Execute(ExternalCommandData commandData, ref string message, ElementSet elementset)
        {
            TaskDialog.Show("Revit", "This is all the help you get!!");
            return Result.Succeeded;
        }

    }//end help

    [Transaction(TransactionMode.Manual)]
    public class about : IExternalCommand
    {

        public Result Execute(ExternalCommandData commandData, ref string message, ElementSet elementset)
        {
            TaskDialog.Show("Revit", "Do not ask about us...");
            return Result.Succeeded;
        }

    }//end about

    [Transaction(TransactionMode.Manual)]
    public class feedback : IExternalCommand
    {

        public Result Execute(ExternalCommandData commandData, ref string message, ElementSet elementset)
        {
            TaskDialog.Show("Revit", "Sorry, no feedback to give.");
            return Result.Succeeded;
        }

    }//end feedback


    [Transaction(TransactionMode.Manual)]
    public class Class1 : IExternalApplication
    {

        public Result OnStartup(UIControlledApplication a)
        {
            //Allow for the folderpath to connect dll file as well as images 
            //Do this for efficiency
            string folderPath = @"C:\Samples\CustomTabRibbon\CustomTabRibbon\bin\Debug";
            string dll = Path.Combine(folderPath, "CustomTabRibbon.dll");

            //Create Custom Tab
            string TheRibbon = "Custom TAB";
            a.CreateRibbonTab(TheRibbon);

            //Create Seperated Panels
            RibbonPanel panelA = a.CreateRibbonPanel(TheRibbon, "A");
            RibbonPanel panelB = a.CreateRibbonPanel(TheRibbon, "B");
            RibbonPanel panelC = a.CreateRibbonPanel(TheRibbon, "C");
            RibbonPanel panelD = a.CreateRibbonPanel(TheRibbon, "D");
            RibbonPanel panelE = a.CreateRibbonPanel(TheRibbon, "E");
            RibbonPanel panelF = a.CreateRibbonPanel(TheRibbon, "F");


/////////////
            //STANDARD BUTTONS One and Two on Panel A
/////////////
            //--LargeImage--32x32 image seen in panel
            //--Image--16x16 when added to quick access tool bar
            //--ToolTipImage-- unknown png size
            PushButton btnOne = (PushButton)panelA.AddItem(new PushButtonData("One", "One", dll, "CustomTabRibbon.One"));
            //btnOne.LargeImage = new BitmapImage(new Uri(Path.Combine(folderPath, "image32.png"), UriKind.Absolute));
            Uri uriImage = new Uri(@"C:\Samples\CustomTabRibbon\CustomTabRibbon\bin\Debug\32Groot.png");
            BitmapImage largeImage = new BitmapImage(uriImage);
            btnOne.LargeImage = largeImage; //Actually attaches image to button one - panel A
            btnOne.ToolTip = "This is a tool tip!";
            btnOne.LongDescription = "This is a lonnnnnnnnnnnnng description!";

            PushButton btnTwo = (PushButton)panelA.AddItem(new PushButtonData("Two", "Two", dll, "CustomTabRibbon.Two"));
            Uri uriImage2 = new Uri(@"C:\Samples\CustomTabRibbon\CustomTabRibbon\bin\Debug\32Hacker.png");
            BitmapImage largeImage2 = new BitmapImage(uriImage2);
            btnTwo.LargeImage = largeImage2;
            btnTwo.ToolTip = "This is a tool tip!";
            btnTwo.LongDescription = "This is a lonnnnnnnnnnnnng description!";

            PushButton btnThree = (PushButton)panelA.AddItem(new PushButtonData("Three", "Three", dll, "CustomTabRibbon.Two"));
            Uri uriImage11 = new Uri(@"C:\Samples\CustomTabRibbon\CustomTabRibbon\bin\Debug\32Ironman.png");
            BitmapImage largeImage11 = new BitmapImage(uriImage11);
            btnThree.LargeImage = largeImage11;
            btnThree.ToolTip = "This is a tool tip!";
            btnThree.LongDescription = "This is a lonnnnnnnnnnnnng description!";

/////////////
            //PULLDOWN LIST
/////////////
            PulldownButtonData pullDownData = new PulldownButtonData("My PullDown", "My PullDown");
            PulldownButton pullDownButton = panelB.AddItem(pullDownData) as PulldownButton;
            pullDownButton.LargeImage = new BitmapImage(new Uri(Path.Combine(folderPath, "24Penguin.png"), UriKind.Absolute));

//            Uri uriImage3 = new Uri(@"C:\Samples\AddPanel\AddPanel\bin\Debug\hacker_icon.png");
//            BitmapImage largeImage3 = new BitmapImage(uriImage3);
//            pullDownButton.LargeImage = largeImage3;

            PushButton btnTagAllByCategory = pullDownButton.AddPushButton(new PushButtonData("One", "Item One", dll, "CustomTabRibbon.One"));
            PushButton btnTagAllBySelection = pullDownButton.AddPushButton(new PushButtonData("Two", "Item Two", dll, "CustomTabRibbon.Two"));
            PushButton btnTagAllByFamily = pullDownButton.AddPushButton(new PushButtonData("Three", "Item Three", dll, "CustomTabRibbon.Three"));

/////////////
            //STACKED LIST
/////////////
            PushButtonData dataHelp = new PushButtonData("Help", "Help", dll, "CustomTabRibbon.help");
            PushButtonData dataAbout = new PushButtonData("About", "About", dll, "CustomTabRibbon.about");
            PushButtonData dataFeedback = new PushButtonData("Feedback", "Feedback", dll, "CustomTabRibbon.feedback");

            IList<RibbonItem> stackedList = panelC.AddStackedItems(dataHelp, dataAbout, dataFeedback);

            PushButton btnHelp = (PushButton)stackedList[0];
            btnHelp.Image = new BitmapImage(new Uri(Path.Combine(folderPath, "16Smileface.png"), UriKind.Absolute));
//            Uri uriImage4 = new Uri(@"C:\Samples\AddPanel\AddPanel\bin\Debug\hacker_icon.png");
//            BitmapImage largeImage4 = new BitmapImage(uriImage4);
//            btnHelp.LargeImage = largeImage4;
//            BitmapImage smallImage = new BitmapImage(uriImage4);
//            btnHelp.Image = smallImage;
            btnHelp.ToolTip = "Click for help!";

            PushButton btnAbout = (PushButton)stackedList[1];
            btnAbout.Image = new BitmapImage(new Uri(Path.Combine(folderPath, "16Snapghost.png"), UriKind.Absolute));
//            Uri uriImage5 = new Uri(@"C:\Samples\AddPanel\AddPanel\bin\Debug\hacker_icon.png");
//            BitmapImage largeImage5 = new BitmapImage(uriImage5);
//            btnAbout.LargeImage = largeImage5;
//            BitmapImage smallImage = new BitmapImage(uriImage4);
//            btnHelp.Image = smallImage;
            btnAbout.ToolTip = "About these tools";

            PushButton btnFeedback = (PushButton)stackedList[2];
            btnFeedback.Image = new BitmapImage(new Uri(Path.Combine(folderPath, "16Tesseract.png"), UriKind.Absolute));
//            Uri uriImage6 = new Uri(@"C:\Samples\AddPanel\AddPanel\bin\Debug\hacker_icon.png");
//            BitmapImage largeImage6 = new BitmapImage(uriImage6);
//            btnFeedback.LargeImage = largeImage6;
//            BitmapImage smallImage = new BitmapImage(uriImage4);
//            btnHelp.Image = smallImage;
            btnFeedback.ToolTip = "Want some feedback?";


/////////////
            //RADIO BUTTONS
/////////////
            RadioButtonGroupData radioData = new RadioButtonGroupData("radioGroup");
            RadioButtonGroup radioButtonGroup = panelD.AddItem(radioData) as RadioButtonGroup;

            // create toggle buttons and add to radio button group
            ToggleButtonData tb1 = new ToggleButtonData("toggleButton1", "Red");
            tb1.ToolTip = "Red Option";
            tb1.LargeImage = new BitmapImage(new Uri(@"C:\Samples\CustomTabRibbon\CustomTabRibbon\bin\Debug\24red.png"));

            ToggleButtonData tb2 = new ToggleButtonData("toggleButton2", "Green");
            tb2.ToolTip = "Green Option";
            tb2.LargeImage = new BitmapImage(new Uri(@"C:\Samples\CustomTabRibbon\CustomTabRibbon\bin\Debug\24green.png"));

            ToggleButtonData tb3 = new ToggleButtonData("toggleButton3", "Yellow");
            tb3.ToolTip = "Yellow Option";
            tb3.LargeImage = new BitmapImage(new Uri(@"C:\Samples\CustomTabRibbon\CustomTabRibbon\bin\Debug\24yellow.png"));

            radioButtonGroup.AddItem(tb1);
            radioButtonGroup.AddItem(tb2);
            radioButtonGroup.AddItem(tb3);

/////////////
            //SPLIT BUTTON
/////////////
            string assembly = @"C:\Samples\CustomTabRibbon\CustomTabRibbon\bin\Debug\CustomTabRibbon.dll";

            // create push buttons for split button drop down
            PushButtonData bOne = new PushButtonData("ButtonNameA", "Option One", assembly, "CustomTabRibbon.One");
            bOne.LargeImage = new BitmapImage(new Uri(@"C:\Samples\CustomTabRibbon\CustomTabRibbon\bin\Debug\8One.png"));

            PushButtonData bTwo = new PushButtonData("ButtonNameB", "Option Two", assembly, "CustomTabRibbon.Two");
            bTwo.LargeImage = new BitmapImage(new Uri(@"C:\Samples\CustomTabRibbon\CustomTabRibbon\bin\Debug\8one.png"));

            PushButtonData bThree = new PushButtonData("ButtonNameC", "Option Three", assembly, "CustomTabRibbon.Three");
            bThree.LargeImage = new BitmapImage(new Uri(@"C:\Samples\CustomTabRibbon\CustomTabRibbon\bin\Debug\8one.png"));

            SplitButtonData sb1 = new SplitButtonData("splitButton1", "Split");
            SplitButton sb = panelE.AddItem(sb1) as SplitButton;
            sb.AddPushButton(bOne);
            sb.AddPushButton(bTwo);
            sb.AddPushButton(bThree);

/////////////
            //Search
/////////////
            //PushButton btnFive = (PushButton)panelF.AddItem(new PushButtonData("Five", "Five", dll, "CustomTabRibbon.SelectFilterWalls"));


            return Result.Succeeded;
        } 


        public Result OnShutdown(UIControlledApplication a)
        {

            return Result.Succeeded;
        }

    }//end Class1

}
