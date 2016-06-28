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

    public class ProjectSetup : IExternalCommand
    {
        #region PrivateVariables
        String clipboardText = String.Empty;
        String roomsWithTagsAddedString = String.Empty;
        #endregion

        public Autodesk.Revit.UI.Result Execute(ExternalCommandData commandData, ref string message, ElementSet elements)
        {

            UIDocument uidoc = commandData.Application.ActiveUIDocument;

            RevitLinkInstance rvtLink = ConfirmLinkInstanceSelected(uidoc);
            if (rvtLink == null) return Result.Failed;

            List<ElementId> views = GetAllLinkedRoomsVisibleInView(rvtLink.GetLinkDocument(), uidoc.ActiveView);

            TaskDialog.Show("Thereare", String.Format("{0} rooms in this view", views.Count()));

            ReTagRoomsInLink(commandData.Application.ActiveUIDocument, rvtLink);

            return Result.Succeeded;
            //if (ReTagRoomsInLink(commandData.Application.ActiveUIDocument) )
            //{
            //    return Result.Succeeded;
            //}
            //else
            //{
            //    return Result.Failed;
            //}
            
        }


        //Method which will re-tag all rooms in a linked model in any view which is on a sheet
        public bool ReTagRoomsInLink(UIDocument uiDoc, RevitLinkInstance rvtLink)
        {
            Document _document = uiDoc.Document;
            UIDocument _uiDocument = uiDoc;
            Document _linkDoc = rvtLink.GetLinkDocument();
            FilteredElementCollector viewCollector = new FilteredElementCollector(_document).OfCategory(BuiltInCategory.OST_Views).OfClass(typeof(ViewPlan));

            Transaction t = new Transaction(_document, "Re-do RoomTags");
            t.Start();
            foreach (ElementId elemId in viewCollector.ToElementIds())
            {
                ViewPlan view = _document.GetElement(elemId) as ViewPlan;

                //If the view is null, or the view is not on a sheet, move on.
                if (view == null) continue;
                if (view.IsTemplate) continue;
                if (view.LookupParameter("Detail Number").AsString() == "-" || view.LookupParameter("Detail Number").AsString() == null) continue;
                

                //copy or remove all of the existing room tags
                //Go one by one through each tag and does a test of copying the tag.
                //If this test fails then the tag is removed
                FilteredElementCollector roomTagcollector = new FilteredElementCollector(_document, view.Id).OfCategory(BuiltInCategory.OST_RoomTags);
                foreach (ElementId testc in roomTagcollector.ToElementIds())
                {
                    SubTransaction subT = new SubTransaction(_document);
                    subT.Start();
                    bool successTest = false;
                    try
                    {
                        ElementTransformUtils.CopyElement(_document, testc, new XYZ(0, 0, 0));
                        successTest = true;
                    }
                    catch
                    {

                    }
                    subT.RollBack();
                    if (!successTest) _document.Delete(testc);
                }

                //Create a HasSet (unique List) of all of the rooms tagged by roomTags visible in the current view.
                HashSet<ElementId> taggedRooms = new HashSet<ElementId>();
                FilteredElementCollector allRoomTagcollector = new FilteredElementCollector(_document, view.Id).OfCategory(BuiltInCategory.OST_RoomTags);
                foreach (ElementId eId in allRoomTagcollector.ToElementIds())
                {
                    RoomTag rt = _document.GetElement(eId) as RoomTag;
                    if (rt == null) continue;
                    taggedRooms.Add(GetTaggedLinkedRoom(rt, _linkDoc));
                }


                //FilteredElementCollector linkedRoomCollector = new FilteredElementCollector(_linkDoc).OfClass(typeof(SpatialElement));
                //double numberOfSpatialElements = linkedRoomCollector.Count();
                foreach (ElementId linkedRoomId in GetAllLinkedRoomsVisibleInView(_linkDoc, view)) //linkedRoomCollector.ToElementIds())
                {
                    try
                    {
                        Room room = _linkDoc.GetElement(linkedRoomId) as Room;
                        LocationPoint locPnt = room.Location as LocationPoint;
                        XYZ linkRoomPnt = new XYZ(locPnt.Point.X, locPnt.Point.Y, locPnt.Point.Z);
                        Room nearestRoom = _linkDoc.GetRoomAtPoint(linkRoomPnt);

                        ElementId nearestRoomId = nearestRoom.Id;
                        bool roomIdIsInTagged = taggedRooms.Contains(nearestRoomId);
                        if (!roomIdIsInTagged)
                        {
                            LocationPoint pnt = _linkDoc.GetElement(linkedRoomId).Location as LocationPoint;
                            UV roomUV = new UV(pnt.Point.X, pnt.Point.Y);
                            _document.Create.NewRoomTag(new LinkElementId(rvtLink.Id, linkedRoomId), roomUV, view.Id);
                            roomsWithTagsAddedString += room.Id.IntegerValue.ToString() + ',';
                        }
                    }
                    catch (NullReferenceException)
                    {
                        //spatial element may not be a room
                        //nearest room may not exist, especially if rooms in arch are not placed.
                        clipboardText += linkedRoomId.IntegerValue.ToString() + ',';
                        continue;
                    }
                }
            }

            t.Commit();

            //put copy string on clipboard.  Remove trailing ',' if present
            //clipboardText.TrimEnd(',');
            Clipboard.SetText(roomsWithTagsAddedString);
            return true;
        }   

        public ElementId GetTaggedLinkedRoom(RoomTag roomTag, Document linkDoc)
        {
            if (roomTag.Room != null)
            {
                return roomTag.Room.Id;
            }
            
            try
            {
                LocationPoint pnt = roomTag.Location as LocationPoint;
                Room r = linkDoc.GetRoomAtPoint(new XYZ(pnt.Point.X, pnt.Point.Y, pnt.Point.Z));
                if (r != null)
                {
                    return r.Id;
                    //taggedRoomsString += r.Id.IntegerValue.ToString() + ',';
                }
            }
            catch
            {

            }
            return null;
        }

        public List<ElementId> GetAllLinkedRoomsVisibleInView(Document linkDoc, Autodesk.Revit.DB.View view)
        {
            List<ElementId> allRooms = new List<ElementId>();
            FilteredElementCollector linkedSpatialCollector = new FilteredElementCollector(linkDoc).OfClass(typeof(SpatialElement));

            BoundingBoxXYZ bBox = view.get_BoundingBox(view);
            XYZ minCorner = new XYZ(Math.Min(bBox.Min.X, bBox.Max.X), Math.Min(bBox.Min.Y, bBox.Max.Y), Math.Min(bBox.Min.Z, bBox.Max.Z));
            XYZ maxCorner = new XYZ(Math.Max(bBox.Min.X, bBox.Max.X), Math.Max(bBox.Min.Y, bBox.Max.Y), Math.Max(bBox.Min.Z, bBox.Max.Z));

            Outline outline = new Outline(minCorner, maxCorner);
            BoundingBoxIntersectsFilter bBoxFilter = new BoundingBoxIntersectsFilter(outline);
            FilteredElementCollector linkedIntersectCollector = new FilteredElementCollector(linkDoc).OfClass(typeof(SpatialElement)).WherePasses(bBoxFilter);

            foreach (SpatialElement spatial in linkedIntersectCollector)
            {
                Room room = spatial as Room;
                if (room == null) continue;
                
                allRooms.Add(room.Id);
            }
            return allRooms;
        }

        public RevitLinkInstance ConfirmLinkInstanceSelected(UIDocument uidoc)
        {

            RevitLinkInstance rvtLink = null;
            Document _document = uidoc.Application.ActiveUIDocument.Document;
            try
            {
                rvtLink = _document.GetElement(uidoc.Selection.GetElementIds().First()) as RevitLinkInstance;
                return rvtLink;
            }
            catch
            {
                TaskDialog.Show("No Link Instance", "Please select a Revit Link Instance before running this command");
                return rvtLink;
            }
            
        }
        
    }
}

