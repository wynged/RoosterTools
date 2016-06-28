using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RoosterTools;

namespace DuctSizing
{
    public class DuctSizing
    {

        internal DuctSizing()
        {

        }

        public static double EquivalentDiameter(double PD, double flow)
        {
            return SizingToolkit.DuctDiameterFromPDandQ(PD, flow);
        }

        public static double EquivalentDiameterOfRectangle(double width, double height)
        {
            return SizingToolkit.DuctEquivalentDiameter(width, height);
        }



    }
}
