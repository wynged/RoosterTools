using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using Autodesk.Revit.DB.Architecture;
using System.Windows.Forms;


namespace RoosterTools
{
    //
    //This class ...
    //
    [Autodesk.Revit.Attributes.Transaction(Autodesk.Revit.Attributes.TransactionMode.Manual)]
    [Autodesk.Revit.Attributes.Regeneration(Autodesk.Revit.Attributes.RegenerationOption.Manual)]
    [Autodesk.Revit.Attributes.Journaling(Autodesk.Revit.Attributes.JournalingMode.NoCommandData)]

    public class TestCommand : IExternalCommand
    {
        #region PrivateVariables
        String copyString = String.Empty;
        #endregion

        public Autodesk.Revit.UI.Result Execute(ExternalCommandData commandData, ref string message, ElementSet elements)
        {
            TaskDialog.Show("TestSript", "TestingGround");
            //Prepare _doc, _sel, _av
            Document _doc = commandData.Application.ActiveUIDocument.Document;
            ICollection<ElementId> _sel = commandData.Application.ActiveUIDocument.Selection.GetElementIds();
            Autodesk.Revit.DB.View _av = commandData.Application.ActiveUIDocument.ActiveView;
            String testResults = String.Empty;


            //Create and Start Transaction
            Transaction t = new Transaction(_doc, "Testing");
            t.Start();

            //
            //Do the testing code here.
            //
            FilteredElementCollector roomCol = new FilteredElementCollector(_doc).OfCategory(BuiltInCategory.OST_Rooms);
            RevitLinkInstance link = _doc.GetElement(_sel.First()) as RevitLinkInstance;
            Document linkDoc = link.GetLinkDocument();

            foreach (Room room in roomCol)
            {
                LocationPoint rmLocation = room.Location as LocationPoint;
                if (rmLocation == null) continue;
                Room roomInLink = linkDoc.GetRoomAtPoint(rmLocation.Point);
                testResults += String.Format("{0}", roomInLink.Id.IntegerValue.ToString());
            }

            //Commit Transaction
            t.Commit();

            TaskDialog.Show("TestResults", testResults);
            return Result.Succeeded;
        }
    }
}
