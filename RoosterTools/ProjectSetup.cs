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

    public class RoomLabels : IExternalCommand
    {
        #region PrivateVariables
        String taggingRoomsReturnedNullString = String.Empty;
        String roomsWithTagsAddedString = String.Empty;
        #endregion

        public Autodesk.Revit.UI.Result Execute(ExternalCommandData commandData, ref string message, ElementSet elements)
        {
            Document _document = commandData.Application.ActiveUIDocument.Document;
            RevitLinkInstance rvtLink;
            Document _linkDoc;
            try
            {
                rvtLink = _document.GetElement(commandData.Application.ActiveUIDocument.Selection.GetElementIds().First()) as RevitLinkInstance;
                _linkDoc = rvtLink.GetLinkDocument();
            }
            catch
            {
                TaskDialog.Show("No Link Instance", "Please select a Revit Link Instance before running this command");
                return Result.Failed;
            }
            
            FilteredElementCollector viewCollector = new FilteredElementCollector(_document).OfCategory(BuiltInCategory.OST_Views).OfClass(typeof(ViewPlan));

            Transaction t = new Transaction(_document, "Re-do RoomTags");
            t.Start();
            foreach (ElementId elemId in viewCollector.ToElementIds())
            {
                ViewPlan view = _document.GetElement(elemId) as ViewPlan;

                //If the view is null, or the view is not on a sheet, move on.
                if (view == null) continue;
                if (view.IsTemplate) continue;
                if (view.LookupParameter("Detail Number").AsString() == "" || view.LookupParameter("Detail Number").AsString() == null) continue;

      

                //copy or remove all of the existing room tags
                FilteredElementCollector roomTagcollector = new FilteredElementCollector(_document, view.Id).OfCategory(BuiltInCategory.OST_RoomTags);
                ICollection<ElementId> originalTagIds = roomTagcollector.ToElementIds();

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

                HashSet<ElementId> taggedRooms = new HashSet<ElementId>();

                FilteredElementCollector allRoomTagcollector = new FilteredElementCollector(_document, view.Id).OfCategory(BuiltInCategory.OST_RoomTags);
                foreach (ElementId eId in allRoomTagcollector.ToElementIds())
                {
                    RoomTag rt = _document.GetElement(eId) as RoomTag;
                    if (rt == null) continue;
                    if (rt.Room != null)
                    {
                        taggedRooms.Add(rt.Room.Id);
                    }
                    else
                    {
                        try
                        {
                            LocationPoint pnt = rt.Location as LocationPoint;
                            Room r = _linkDoc.GetRoomAtPoint(new XYZ(pnt.Point.X, pnt.Point.Y, pnt.Point.Z));
                            if (r != null)
                            {
                                taggedRooms.Add(r.Id);
                                //taggedRoomsString += r.Id.IntegerValue.ToString() + ',';
                            }
                        }
                        catch
                        {

                        }
                    }
                }

                
                FilteredElementCollector linkedRoomCollector = new FilteredElementCollector(_linkDoc, view.Id).OfClass(typeof(SpatialElement));
                double numberOfSpatialElements = linkedRoomCollector.Count();
                foreach ( ElementId linkedRoomId in linkedRoomCollector.ToElementIds())
                {
                    try {
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
                        taggingRoomsReturnedNullString += linkedRoomId.IntegerValue.ToString() + ',';
                        continue;
                    }
                }

                //foreach (ElementId roomId in roomCollector.ToElementIds())
                //{
                //    LocationPoint pnt = _linkDoc.GetElement(roomId).Location as LocationPoint;
                //    UV roomUV = new UV(pnt.Point.X, pnt.Point.Y);
                //    UV tagUV;
                //    XYZ moveTag = new XYZ(0,0,0);
                //    bool leader = false;
                //    if (rtInfoList.Select(i => i.RoomID).Contains(roomId)) {
                //        tagUV = rtInfoList.Where(ri => ri.RoomID == roomId).Select(ri => ri.RoomUV).First();
                //        UV diffUV = tagUV.Subtract(roomUV);
                //        moveTag = new XYZ(diffUV.U, diffUV.V, 0);
                //        leader = rtInfoList.Where(ri => ri.RoomID == roomId).Select(ri => ri.HasLeader).First();
                //    }
                //    else
                //    {
                //        roomUV = new UV(pnt.Point.X, pnt.Point.Y);
                //    }

                //    RoomTag rTag = _document.Create.NewRoomTag(new LinkElementId(rvtLink.Id, roomId), roomUV , view.Id);
                //    if (leader) rTag.HasLeader = true;
                //    ElementTransformUtils.MoveElement(_document, rTag.Id, moveTag);
                //}
            }

            t.Commit();

            //put copy string on clipboard.  Remove trailing ',' if present
            taggingRoomsReturnedNullString.TrimEnd(',');
            Clipboard.SetText(taggingRoomsReturnedNullString);


            return Autodesk.Revit.UI.Result.Succeeded;
        }

        //    public List<RoomTagInfo> GetRoomTagInfo(Document doc, Document linkDoc, ElementId viewPlanId)
        //    {
        //        List<RoomTagInfo> roomTags = new List<RoomTagInfo>();

        //        FilteredElementCollector roomTagCollector = new FilteredElementCollector(doc, viewPlanId).OfCategory(BuiltInCategory.OST_RoomTags);

        //        foreach ( RoomTag rt in roomTagCollector)
        //        {
        //            LocationPoint pnt = rt.Location as LocationPoint;
        //            UV nuv = new UV(pnt.Point.X, pnt.Point.Y);
        //            ElementId eId = null;

        //            if (rt.Room != null)
        //            {
        //                eId = rt.Room.Id;
        //            }
        //            else
        //            {
        //                try {
        //                    Room rm = linkDoc.GetRoomAtPoint(pnt.Point);
        //                    eId = rm.Id;
        //                }
        //                catch
        //                {
        //                    //TaskDialog.Show("no room", String.Format("BadRoomTagId: {0}", rt.Id));
        //                    copyString += String.Format("{0},", rt.Id.IntegerValue.ToString());
        //                }
        //            }

        //            if (eId != null && nuv != null)
        //            {
        //                RoomTagInfo rtInfo = new RoomTagInfo(eId, nuv, rt.HasLeader);
        //                roomTags.Add(rtInfo);
        //            }

        //        }
        //        return roomTags;
        //    }

        //    public class RoomTagInfo
        //    {
        //        public RoomTagInfo(ElementId roomid, UV roomuv, bool hasleader)
        //        {
        //            RoomID = roomid;
        //            RoomUV = roomuv;
        //            HasLeader = hasleader;
        //        }

        //        public ElementId RoomID { get; set; }
        //        public UV RoomUV { get; set; }
        //        public bool HasLeader { get; set; }
        //    }
        //}
        
    }
}

