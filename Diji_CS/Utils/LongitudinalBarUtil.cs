using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Diji_CS;
using Bentley.Interop.MicroStationDGN;
using Bentley.MicroStation.InteropServices;
using Bentley.Interop.TFCom;

namespace Diji_CS.Utils
{
    class LongitudinalBarUtil
    {
        private static Bentley.Interop.MicroStationDGN.Application app = Utilities.ComApp;

        public static Element create_longitudinal_bar(double diameter, double length, double bending_length, double angle)
        {
            Element vertical_bar = app.SmartSolid.CreateCylinder(null, diameter / 2, length - Data.anchor_bending_rebar_radius);
            vertical_bar.Move(app.Point3dFromXYZ(0, 0, (length - Data.anchor_bending_rebar_radius) / 2));
            Element anchor_arc = app.SmartSolid.CreateTorus(null, Data.anchor_bending_rebar_radius, diameter / 2, Data.ANGLE_90);
            Transform3d transform = app.Transform3dFromMatrix3d(app.Matrix3dFromRotationBetweenVectors(app.Point3dFromXYZ(1, 0, 0), app.Point3dFromXYZ(0, 0, - 1)));
            anchor_arc.Transform(transform);
            anchor_arc.Move(app.Point3dFromXYZ(0, - Data.anchor_bending_rebar_radius, 0));
            Element anchor_bending = app.SmartSolid.CreateCylinder(null, diameter / 2, bending_length - Data.anchor_bending_rebar_radius);
            transform = app.Transform3dFromMatrix3d(app.Matrix3dFromRotationBetweenVectors(app.Point3dFromXYZ(0, 0, 1), app.Point3dFromXYZ(0, 1, 0)));
            anchor_bending.Transform(transform);
            anchor_bending.Move(app.Point3dFromXYZ(0, - bending_length / 2 - Data.anchor_bending_rebar_radius / 2, -Data.anchor_bending_rebar_radius));
            Element longitudinal_bar = app.SmartSolid.SolidUnion(vertical_bar.AsSmartSolidElement, anchor_arc.AsSmartSolidElement);
            longitudinal_bar = app.SmartSolid.SolidUnion(longitudinal_bar.AsSmartSolidElement, anchor_bending.AsSmartSolidElement);
            transform = app.Transform3dFromMatrix3d(app.Matrix3dFromRotationBetweenVectors(app.Point3dFromXYZ(0, -1, 0), app.Point3dFromXYZ(Math.Sin(angle / 180 * Math.PI), -Math.Cos(angle / 180 * Math.PI), 0)));
            longitudinal_bar.Transform(transform);
            return longitudinal_bar;
        }
    }
}
