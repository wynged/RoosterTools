using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Autodesk.DesignScript.Geometry;
using Autodesk.DesignScript.Runtime;
using Autodesk.Revit.DB;
using Autodesk.Revit.DB.Mechanical;


namespace DynaMEP
{

    public static class DuctAndPipe
    {

        public static Duct DrawDuctPlaceholder(Document document, ElementId SystemTypeId, ElementId DuctTypeId, ElementId LevelId, Autodesk.DesignScript.Geometry.Point p1, Autodesk.DesignScript.Geometry.Point p2)
        {
            XYZ xyz1 = new XYZ(p1.X, p1.Y, p1.Z);
            XYZ xyz2 = new XYZ(p2.X, p2.Y, p2.Z);
            Duct newDuctPlaceholder = Duct.CreatePlaceholder(document, SystemTypeId, DuctTypeId, LevelId, xyz1, xyz2);
            return newDuctPlaceholder;
        }



        
    }
}
