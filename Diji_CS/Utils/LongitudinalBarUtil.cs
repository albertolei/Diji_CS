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

        //画柱纵筋, 参数分别为柱子的长、宽、高，基础的长、宽、高，弯折长度，箍筋类型
        public static Element create_column_longitudinal_rebars(double column_length, double column_width, double column_height, double foundation_length, double foundation_width, double foundation_height, double bending_length, string type)
        {
            Element column_longitudinal_rebars = null;
            switch (type)
            {
                case "1":
                    column_longitudinal_rebars = LongitudinalBarUtil.create_longitudinal_bar_type1(column_length, column_width, column_height, foundation_length, foundation_width, foundation_height, column_height + foundation_height - Data.down_protective_layer_thinckness - Data.x_down_rebar_diameter - Data.y_down_rebar_diameter - 1 - Data.longitudinal_rebar_diameter / 2 - 1, bending_length, 3, 3);
                    break;
                case "2":
                    column_longitudinal_rebars = LongitudinalBarUtil.create_longitudinal_bar_type2(column_length, column_width, column_height, foundation_length, foundation_width, foundation_height, column_height + foundation_height - Data.down_protective_layer_thinckness - Data.x_down_rebar_diameter - Data.y_down_rebar_diameter - 1 - Data.longitudinal_rebar_diameter / 2 - 1, bending_length);
                    break;
            }
            return column_longitudinal_rebars;
        }
        public static Element create_longitudinal_bar_type1(double column_length, double column_width, double column_height, double foundation_length, double foundation_width, double foundation_height, double length, double bending_length, int m, int n)
        {
            Element angle_bar = create_longitudinal_bar_type2(column_length, column_width, column_height, foundation_length, foundation_width, foundation_height, length, bending_length);
            //m表示y方向上的纵筋
            Element y_longitudinal_bars = null;
            switch (m)
            {
                case 3:
                    {
                        Element left_bar = create_single_longitudinal_bar(length, bending_length, Data.ANGLE_270);
                        Element right_bar = create_single_longitudinal_bar(length, bending_length, Data.ANGLE_90);
                        Point3d left_point = app.Point3dFromXYZ(-column_length / 2 + Data.protective_layer_thinckness + Data.stirrup_diameter + Data.longitudinal_rebar_diameter / 2, 0, foundation_height / 2 + column_height - length / 2);
                        Point3d right_point = app.Point3dFromXYZ(column_length / 2 - Data.protective_layer_thinckness - Data.stirrup_diameter - Data.longitudinal_rebar_diameter / 2, 0, foundation_height / 2 + column_height - length / 2);
                        left_bar.Move(left_point);
                        right_bar.Move(right_point);
                        y_longitudinal_bars = app.SmartSolid.SolidUnion(left_bar.AsSmartSolidElement, right_bar.AsSmartSolidElement);
                        break;
                    }
                case 4:
                    {
                        
                        break;
                    }
                case 5:
                    {
                        break;
                    }
                case 6:
                    {
                        break;
                    }
                case 7:
                    {
                        break;
                    }
                case 8:
                    {
                        break;
                    }
            }
            Element x_longitudinal_bars = null;
            //n表示x方向上的纵筋
            switch (n)
            {
                case 3:
                    {
                        Element up_bar = create_single_longitudinal_bar(length, bending_length, Data.ANGLE_180);
                        Element down_bar = create_single_longitudinal_bar(length, bending_length, Data.ANGLE_0);
                        Point3d up_point = app.Point3dFromXYZ(0, column_width / 2 - Data.protective_layer_thinckness - Data.stirrup_diameter - Data.longitudinal_rebar_diameter / 2, foundation_height / 2 + column_height - length / 2);
                        Point3d down_point = app.Point3dFromXYZ(0, -column_width / 2 + Data.protective_layer_thinckness + Data.stirrup_diameter + Data.longitudinal_rebar_diameter / 2, foundation_height / 2 + column_height - length / 2);
                        up_bar.Move(up_point);
                        down_bar.Move(down_point);
                        x_longitudinal_bars = app.SmartSolid.SolidUnion(up_bar.AsSmartSolidElement, down_bar.AsSmartSolidElement);
                        break;
                    }
                case 4:
                    {
                        break;
                    }
                case 5:
                    {
                        break;
                    }
                case 6:
                    {
                        break;
                    }
                case 7:
                    {
                        break;
                    }
                case 8:
                    {
                        break;
                    }
            }
            Element ret = app.SmartSolid.SolidUnion(angle_bar.AsSmartSolidElement, y_longitudinal_bars.AsSmartSolidElement);
            ret = app.SmartSolid.SolidUnion(ret.AsSmartSolidElement, x_longitudinal_bars.AsSmartSolidElement);
            return ret;
        }
        public static Element create_longitudinal_bar_type2(double column_length, double column_width, double column_height, double foundation_length, double foundation_width, double foundation_height, double length, double bending_length)
        {
            Point3d up_right, up_left, down_right, down_left;
            up_right = app.Point3dFromXYZ(column_length / 2 - Data.protective_layer_thinckness - Data.stirrup_diameter - Data.longitudinal_rebar_diameter / 2, column_width / 2 - Data.protective_layer_thinckness - Data.stirrup_diameter - Data.longitudinal_rebar_diameter / 2, foundation_height / 2 + column_height - length / 2);
            up_left = app.Point3dFromXYZ(-column_length / 2 + Data.protective_layer_thinckness + Data.stirrup_diameter + Data.longitudinal_rebar_diameter / 2, column_width / 2 - Data.protective_layer_thinckness - Data.stirrup_diameter - Data.longitudinal_rebar_diameter / 2, foundation_height / 2 + column_height - length / 2);
            down_right = app.Point3dFromXYZ(column_length / 2 - Data.protective_layer_thinckness - Data.stirrup_diameter - Data.longitudinal_rebar_diameter / 2, -column_width / 2 + Data.protective_layer_thinckness + Data.stirrup_diameter + Data.longitudinal_rebar_diameter / 2, foundation_height / 2 + column_height - length / 2);
            down_left = app.Point3dFromXYZ(-column_length / 2 + Data.protective_layer_thinckness + Data.stirrup_diameter + Data.longitudinal_rebar_diameter / 2, -column_width / 2 + Data.protective_layer_thinckness + Data.stirrup_diameter + Data.longitudinal_rebar_diameter / 2, foundation_height / 2 + column_height - length / 2);
            Element up_right_bar = create_single_longitudinal_bar(length, bending_length, Data.ANGLE_135);
            Element up_left_bar = create_single_longitudinal_bar(length, bending_length, Data.ANGLE_225);
            Element down_right_bar = create_single_longitudinal_bar(length, bending_length, Data.ANGLE_45);
            Element down_left_bar = create_single_longitudinal_bar(length, bending_length, Data.ANGLE_315);
            up_right_bar.Move(up_right);
            up_left_bar.Move(up_left);
            down_right_bar.Move(down_right);
            down_left_bar.Move(down_left);
            Element ret = app.SmartSolid.SolidUnion(up_right_bar.AsSmartSolidElement, up_left_bar.AsSmartSolidElement);
            ret = app.SmartSolid.SolidUnion(ret.AsSmartSolidElement, down_right_bar.AsSmartSolidElement);
            ret = app.SmartSolid.SolidUnion(ret.AsSmartSolidElement, down_left_bar.AsSmartSolidElement);
            return ret;
        }
        public static Element create_longitudinal_bar_type3(double column_length, double column_width, double column_height, double foundation_length, double foundation_width, double foundation_height, double length, double bending_length)
        {
            return null;
        }
        public static Element create_longitudinal_bar_type4(double column_length, double column_width, double column_height, double foundation_length, double foundation_width, double foundation_height, double length, double bending_length)
        {
            return null;
        }
        public static Element create_longitudinal_bar_type5(double column_length, double column_width, double column_height, double foundation_length, double foundation_width, double foundation_height, double length, double bending_length, int m, int n)
        {
            return null;
        }
        public static Element create_longitudinal_bar_type6(double column_length, double column_width, double column_height, double foundation_length, double foundation_width, double foundation_height, double length, double bending_length)
        {
            return null;
        }
        public static Element create_longitudinal_bar_type7(double column_length, double column_width, double column_height, double foundation_length, double foundation_width, double foundation_height, double length, double bending_length)
        {

            return null;
        }
        public static Element create_single_longitudinal_bar(double length, double bending_length, double angle)
        {
            Element vertical_bar = app.SmartSolid.CreateCylinder(null, Data.longitudinal_rebar_diameter / 2, length - Data.anchor_bending_rebar_radius);
            vertical_bar.Move(app.Point3dFromXYZ(0, 0, (length - Data.anchor_bending_rebar_radius) / 2));
            Element anchor_arc = app.SmartSolid.CreateTorus(null, Data.anchor_bending_rebar_radius, Data.longitudinal_rebar_diameter / 2, Data.ANGLE_90);
            Transform3d transform = app.Transform3dFromMatrix3d(app.Matrix3dFromRotationBetweenVectors(app.Point3dFromXYZ(1, 0, 0), app.Point3dFromXYZ(0, 0, - 1)));
            anchor_arc.Transform(transform);
            anchor_arc.Move(app.Point3dFromXYZ(0, - Data.anchor_bending_rebar_radius, 0));
            
            Element anchor_bending = app.SmartSolid.CreateCylinder(null, Data.longitudinal_rebar_diameter / 2, bending_length - Data.anchor_bending_rebar_radius);
            transform = app.Transform3dFromMatrix3d(app.Matrix3dFromRotationBetweenVectors(app.Point3dFromXYZ(0, 0, 1), app.Point3dFromXYZ(0, 1, 0)));
            anchor_bending.Transform(transform);
            anchor_bending.Move(app.Point3dFromXYZ(0, - bending_length / 2 - Data.anchor_bending_rebar_radius / 2, -Data.anchor_bending_rebar_radius));

            Element longitudinal_bar = app.SmartSolid.SolidUnion(vertical_bar.AsSmartSolidElement, anchor_arc.AsSmartSolidElement);
            longitudinal_bar = app.SmartSolid.SolidUnion(longitudinal_bar.AsSmartSolidElement, anchor_bending.AsSmartSolidElement);
            transform = app.Transform3dFromMatrix3d(app.Matrix3dFromRotationBetweenVectors(app.Point3dFromXYZ(0, -1, 0), app.Point3dFromXYZ(Math.Sin(angle / 180 * Math.PI), -Math.Cos(angle / 180 * Math.PI), 0)));
            longitudinal_bar.Transform(transform);
            longitudinal_bar.Move(app.Point3dFromXYZ(0, 0, - (length / 2 - Data.anchor_bending_rebar_radius)));
            return longitudinal_bar;
        }
    }
}
