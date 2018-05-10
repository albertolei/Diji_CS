using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Bentley.Interop.MicroStationDGN;
using Bentley.MicroStation.InteropServices;
using Bentley.Interop.TFCom;

namespace Diji_CS.Utils
{
    class SpreadFootingUtil
    {
        private static Bentley.Interop.MicroStationDGN.Application app = Utilities.ComApp;

        //画基础
        public static Element create_foundation(double length, double width, double height)
        {
            Element foundation = app.SmartSolid.CreateSlab(null, length, width, height);
            return foundation;
        }
        //画长方形柱
        public static Element create_column(double length, double width, double height, double foundation_height)
        {
            Element column = app.SmartSolid.CreateSlab(null, length, width, height);
            Point3d position = app.Point3dFromXYZ(0, 0, height / 2 + foundation_height / 2);
            column.Move(position);
            return column;
        }
    }
}
