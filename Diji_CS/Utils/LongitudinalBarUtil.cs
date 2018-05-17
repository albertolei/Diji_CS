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
                    column_longitudinal_rebars = create_longitudinal_bar_type1(column_length, column_width, column_height, foundation_length, foundation_width, foundation_height, column_height + foundation_height - Data.down_protective_layer_thinckness - Data.x_down_rebar_diameter - Data.y_down_rebar_diameter - 1 - Data.longitudinal_rebar_diameter / 2 - 1, bending_length, 8, 8);
                    break;
                case "2":
                    column_longitudinal_rebars = create_longitudinal_bar_type2(column_length, column_width, column_height, foundation_length, foundation_width, foundation_height, column_height + foundation_height - Data.down_protective_layer_thinckness - Data.x_down_rebar_diameter - Data.y_down_rebar_diameter - 1 - Data.longitudinal_rebar_diameter / 2 - 1, bending_length);
                    break;
            }
            return column_longitudinal_rebars;
        }
        //画柱纵筋, 参数分别为柱子的直径、高，基础的长、宽、高，弯折长度，箍筋类型
        public static Element create_column_longitudinal_rebars(double column_diameter, double column_height, double foundation_length, double foundation_width, double foundation_height, double bending_length, string type)
        {
            Element column_longitudinal_rebars = null;
            double angle_r = 0, diameter = 0, xzlength = 0, length = 0, angle = 0;
            switch (type)
            {
                case "6":
                    column_longitudinal_rebars = create_longitudinal_bar_type6(column_diameter, column_height, foundation_length, foundation_width, foundation_height, column_height + foundation_height - Data.down_protective_layer_thinckness - Data.x_down_rebar_diameter - Data.y_down_rebar_diameter - 1 - Data.longitudinal_rebar_diameter / 2 - 1, bending_length);
                    break;
                case "7":
                    diameter = column_diameter - Data.protective_layer_thinckness * 2 - Data.stirrup_diameter;
                    xzlength = column_diameter / 3 >= 250 ? column_diameter / 3 : 250;
                    angle_r = Math.Acos(xzlength / 2 / (diameter / 2 - Data.stirrup_diameter / 2 - Data.longitudinal_rebar_diameter / 2));
                    angle = Data.ANGLE_180 - angle_r / Math.PI * Data.ANGLE_180 * 2;
                    column_longitudinal_rebars = create_longitudinal_bar_type7(diameter, angle, xzlength, column_height,foundation_length, foundation_width, foundation_height, column_height + foundation_height - Data.down_protective_layer_thinckness - Data.x_down_rebar_diameter - Data.y_down_rebar_diameter - 1 - Data.longitudinal_rebar_diameter / 2 - 1, bending_length);
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
                        Element left_bar1 = create_single_longitudinal_bar(length, bending_length, Data.ANGLE_270);
                        Element left_bar2 = create_single_longitudinal_bar(length, bending_length, Data.ANGLE_270);
                        Element right_bar1 = create_single_longitudinal_bar(length, bending_length, Data.ANGLE_90);
                        Element right_bar2 = create_single_longitudinal_bar(length, bending_length, Data.ANGLE_90);
                        Point3d left_point1 = app.Point3dFromXYZ(-column_length / 2 + Data.protective_layer_thinckness + Data.stirrup_diameter + Data.longitudinal_rebar_diameter / 2, (column_width - Data.protective_layer_thinckness * 2 - Data.stirrup_diameter * 2 - Data.longitudinal_rebar_diameter) / 3 / 2, foundation_height / 2 + column_height - length / 2);
                        Point3d left_point2 = app.Point3dFromXYZ(-column_length / 2 + Data.protective_layer_thinckness + Data.stirrup_diameter + Data.longitudinal_rebar_diameter / 2, -(column_width - Data.protective_layer_thinckness * 2 - Data.stirrup_diameter * 2 - Data.longitudinal_rebar_diameter) / 3 / 2, foundation_height / 2 + column_height - length / 2);
                        Point3d right_point1 = app.Point3dFromXYZ(column_length / 2 - Data.protective_layer_thinckness - Data.stirrup_diameter - Data.longitudinal_rebar_diameter / 2, (column_width - Data.protective_layer_thinckness * 2 - Data.stirrup_diameter * 2 - Data.longitudinal_rebar_diameter) / 3 / 2, foundation_height / 2 + column_height - length / 2);
                        Point3d right_point2 = app.Point3dFromXYZ(column_length / 2 - Data.protective_layer_thinckness - Data.stirrup_diameter - Data.longitudinal_rebar_diameter / 2, -(column_width - Data.protective_layer_thinckness * 2- Data.stirrup_diameter * 2 - Data.longitudinal_rebar_diameter) / 3 / 2, foundation_height / 2 + column_height - length / 2);
                        left_bar1.Move(left_point1);
                        left_bar2.Move(left_point2);
                        right_bar1.Move(right_point1);
                        right_bar2.Move(right_point2);
                        y_longitudinal_bars = app.SmartSolid.SolidUnion(left_bar1.AsSmartSolidElement, left_bar2.AsSmartSolidElement);
                        y_longitudinal_bars = app.SmartSolid.SolidUnion(y_longitudinal_bars.AsSmartSolidElement, right_bar1.AsSmartSolidElement);
                        y_longitudinal_bars = app.SmartSolid.SolidUnion(y_longitudinal_bars.AsSmartSolidElement, right_bar2.AsSmartSolidElement);
                        break;
                    }
                case 5:
                    {
                        Element left_bar1 = create_single_longitudinal_bar(length, bending_length, Data.ANGLE_270);
                        Element left_bar2 = create_single_longitudinal_bar(length, bending_length, Data.ANGLE_270);
                        Element left_bar3 = create_single_longitudinal_bar(length, bending_length, Data.ANGLE_270);
                        Element right_bar1 = create_single_longitudinal_bar(length, bending_length, Data.ANGLE_90);
                        Element right_bar2 = create_single_longitudinal_bar(length, bending_length, Data.ANGLE_90);
                        Element right_bar3 = create_single_longitudinal_bar(length, bending_length, Data.ANGLE_90);
                        Point3d left_point1 = app.Point3dFromXYZ(-column_length / 2 + Data.protective_layer_thinckness + Data.stirrup_diameter + Data.longitudinal_rebar_diameter / 2, (column_width - Data.protective_layer_thinckness * 2 - Data.stirrup_diameter * 2 - Data.longitudinal_rebar_diameter) / 4, foundation_height / 2 + column_height - length / 2);
                        Point3d left_point2 = app.Point3dFromXYZ(-column_length / 2 + Data.protective_layer_thinckness + Data.stirrup_diameter + Data.longitudinal_rebar_diameter / 2, 0, foundation_height / 2 + column_height - length / 2);
                        Point3d left_point3 = app.Point3dFromXYZ(-column_length / 2 + Data.protective_layer_thinckness + Data.stirrup_diameter + Data.longitudinal_rebar_diameter / 2, -(column_width - Data.protective_layer_thinckness * 2 - Data.stirrup_diameter * 2 - Data.longitudinal_rebar_diameter) / 4, foundation_height / 2 + column_height - length / 2);
                        Point3d right_point1 = app.Point3dFromXYZ(column_length / 2 - Data.protective_layer_thinckness - Data.stirrup_diameter - Data.longitudinal_rebar_diameter / 2, (column_width - Data.protective_layer_thinckness * 2 - Data.stirrup_diameter * 2 - Data.longitudinal_rebar_diameter) / 4, foundation_height / 2 + column_height - length / 2);
                        Point3d right_point2 = app.Point3dFromXYZ(column_length / 2 - Data.protective_layer_thinckness - Data.stirrup_diameter - Data.longitudinal_rebar_diameter / 2, 0, foundation_height / 2 + column_height - length / 2);
                        Point3d right_point3 = app.Point3dFromXYZ(column_length / 2 - Data.protective_layer_thinckness - Data.stirrup_diameter - Data.longitudinal_rebar_diameter / 2, -(column_width - Data.protective_layer_thinckness * 2 - Data.stirrup_diameter * 2 - Data.longitudinal_rebar_diameter) / 4, foundation_height / 2 + column_height - length / 2);
                        left_bar1.Move(left_point1);
                        left_bar2.Move(left_point2);
                        left_bar3.Move(left_point3);
                        right_bar1.Move(right_point1);
                        right_bar2.Move(right_point2);
                        right_bar3.Move(right_point3);
                        y_longitudinal_bars = app.SmartSolid.SolidUnion(left_bar1.AsSmartSolidElement, left_bar2.AsSmartSolidElement);
                        y_longitudinal_bars = app.SmartSolid.SolidUnion(y_longitudinal_bars.AsSmartSolidElement, left_bar3.AsSmartSolidElement);
                        y_longitudinal_bars = app.SmartSolid.SolidUnion(y_longitudinal_bars.AsSmartSolidElement, right_bar1.AsSmartSolidElement);
                        y_longitudinal_bars = app.SmartSolid.SolidUnion(y_longitudinal_bars.AsSmartSolidElement, right_bar2.AsSmartSolidElement);
                        y_longitudinal_bars = app.SmartSolid.SolidUnion(y_longitudinal_bars.AsSmartSolidElement, right_bar3.AsSmartSolidElement);
                        break;
                    }
                case 6:
                    {
                        Element left_bar1 = create_single_longitudinal_bar(length, bending_length, Data.ANGLE_270);
                        Element left_bar2 = create_single_longitudinal_bar(length, bending_length, Data.ANGLE_270);
                        Element left_bar3 = create_single_longitudinal_bar(length, bending_length, Data.ANGLE_270);
                        Element left_bar4 = create_single_longitudinal_bar(length, bending_length, Data.ANGLE_270);
                        Element right_bar1 = create_single_longitudinal_bar(length, bending_length, Data.ANGLE_90);
                        Element right_bar2 = create_single_longitudinal_bar(length, bending_length, Data.ANGLE_90);
                        Element right_bar3 = create_single_longitudinal_bar(length, bending_length, Data.ANGLE_90);
                        Element right_bar4 = create_single_longitudinal_bar(length, bending_length, Data.ANGLE_90);
                        Point3d left_point1 = app.Point3dFromXYZ(-column_length / 2 + Data.protective_layer_thinckness + Data.stirrup_diameter + Data.longitudinal_rebar_diameter / 2, (column_width - Data.protective_layer_thinckness * 2 - Data.stirrup_diameter * 2 - Data.longitudinal_rebar_diameter) / 5 * 1.5, foundation_height / 2 + column_height - length / 2);
                        Point3d left_point2 = app.Point3dFromXYZ(-column_length / 2 + Data.protective_layer_thinckness + Data.stirrup_diameter + Data.longitudinal_rebar_diameter / 2, (column_width - Data.protective_layer_thinckness * 2 - Data.stirrup_diameter * 2 - Data.longitudinal_rebar_diameter) / 5 * 0.5, foundation_height / 2 + column_height - length / 2);
                        Point3d left_point3 = app.Point3dFromXYZ(-column_length / 2 + Data.protective_layer_thinckness + Data.stirrup_diameter + Data.longitudinal_rebar_diameter / 2, -(column_width - Data.protective_layer_thinckness * 2 - Data.stirrup_diameter * 2 - Data.longitudinal_rebar_diameter) / 5 * 0.5, foundation_height / 2 + column_height - length / 2);
                        Point3d left_point4 = app.Point3dFromXYZ(-column_length / 2 + Data.protective_layer_thinckness + Data.stirrup_diameter + Data.longitudinal_rebar_diameter / 2, -(column_width - Data.protective_layer_thinckness * 2 - Data.stirrup_diameter * 2 - Data.longitudinal_rebar_diameter) / 5 * 1.5, foundation_height / 2 + column_height - length / 2);
                        Point3d right_point1 = app.Point3dFromXYZ(column_length / 2 - Data.protective_layer_thinckness - Data.stirrup_diameter - Data.longitudinal_rebar_diameter / 2, (column_width - Data.protective_layer_thinckness * 2 - Data.stirrup_diameter * 2 - Data.longitudinal_rebar_diameter) / 5 * 1.5, foundation_height / 2 + column_height - length / 2);
                        Point3d right_point2 = app.Point3dFromXYZ(column_length / 2 - Data.protective_layer_thinckness - Data.stirrup_diameter - Data.longitudinal_rebar_diameter / 2, (column_width - Data.protective_layer_thinckness * 2 - Data.stirrup_diameter * 2 - Data.longitudinal_rebar_diameter) / 5 * 0.5, foundation_height / 2 + column_height - length / 2);
                        Point3d right_point3 = app.Point3dFromXYZ(column_length / 2 - Data.protective_layer_thinckness - Data.stirrup_diameter - Data.longitudinal_rebar_diameter / 2, -(column_width - Data.protective_layer_thinckness * 2 - Data.stirrup_diameter * 2 - Data.longitudinal_rebar_diameter) / 5 * 0.5, foundation_height / 2 + column_height - length / 2);
                        Point3d right_point4 = app.Point3dFromXYZ(column_length / 2 - Data.protective_layer_thinckness - Data.stirrup_diameter - Data.longitudinal_rebar_diameter / 2, -(column_width - Data.protective_layer_thinckness * 2 - Data.stirrup_diameter * 2 - Data.longitudinal_rebar_diameter) / 5 * 1.5, foundation_height / 2 + column_height - length / 2);
                        left_bar1.Move(left_point1);
                        left_bar2.Move(left_point2);
                        left_bar3.Move(left_point3);
                        left_bar4.Move(left_point4);
                        right_bar1.Move(right_point1);
                        right_bar2.Move(right_point2);
                        right_bar3.Move(right_point3);
                        right_bar4.Move(right_point4);
                        y_longitudinal_bars = app.SmartSolid.SolidUnion(left_bar1.AsSmartSolidElement, left_bar2.AsSmartSolidElement);
                        y_longitudinal_bars = app.SmartSolid.SolidUnion(y_longitudinal_bars.AsSmartSolidElement, left_bar3.AsSmartSolidElement);
                        y_longitudinal_bars = app.SmartSolid.SolidUnion(y_longitudinal_bars.AsSmartSolidElement, left_bar4.AsSmartSolidElement);
                        y_longitudinal_bars = app.SmartSolid.SolidUnion(y_longitudinal_bars.AsSmartSolidElement, right_bar1.AsSmartSolidElement);
                        y_longitudinal_bars = app.SmartSolid.SolidUnion(y_longitudinal_bars.AsSmartSolidElement, right_bar2.AsSmartSolidElement);
                        y_longitudinal_bars = app.SmartSolid.SolidUnion(y_longitudinal_bars.AsSmartSolidElement, right_bar3.AsSmartSolidElement);
                        y_longitudinal_bars = app.SmartSolid.SolidUnion(y_longitudinal_bars.AsSmartSolidElement, right_bar4.AsSmartSolidElement);
                        break;
                    }
                case 7:
                    {
                        Element left_bar1 = create_single_longitudinal_bar(length, bending_length, Data.ANGLE_270);
                        Element left_bar2 = create_single_longitudinal_bar(length, bending_length, Data.ANGLE_270);
                        Element left_bar3 = create_single_longitudinal_bar(length, bending_length, Data.ANGLE_270);
                        Element left_bar4 = create_single_longitudinal_bar(length, bending_length, Data.ANGLE_270);
                        Element left_bar5 = create_single_longitudinal_bar(length, bending_length, Data.ANGLE_270);
                        Element right_bar1 = create_single_longitudinal_bar(length, bending_length, Data.ANGLE_90);
                        Element right_bar2 = create_single_longitudinal_bar(length, bending_length, Data.ANGLE_90);
                        Element right_bar3 = create_single_longitudinal_bar(length, bending_length, Data.ANGLE_90);
                        Element right_bar4 = create_single_longitudinal_bar(length, bending_length, Data.ANGLE_90);
                        Element right_bar5 = create_single_longitudinal_bar(length, bending_length, Data.ANGLE_90);
                        Point3d left_point1 = app.Point3dFromXYZ(-column_length / 2 + Data.protective_layer_thinckness + Data.stirrup_diameter + Data.longitudinal_rebar_diameter / 2, (column_width - Data.protective_layer_thinckness * 2 - Data.stirrup_diameter * 2 - Data.longitudinal_rebar_diameter) / 6 * 2, foundation_height / 2 + column_height - length / 2);
                        Point3d left_point2 = app.Point3dFromXYZ(-column_length / 2 + Data.protective_layer_thinckness + Data.stirrup_diameter + Data.longitudinal_rebar_diameter / 2, (column_width - Data.protective_layer_thinckness * 2 - Data.stirrup_diameter * 2 - Data.longitudinal_rebar_diameter) / 6, foundation_height / 2 + column_height - length / 2);
                        Point3d left_point3 = app.Point3dFromXYZ(-column_length / 2 + Data.protective_layer_thinckness + Data.stirrup_diameter + Data.longitudinal_rebar_diameter / 2, 0, foundation_height / 2 + column_height - length / 2);
                        Point3d left_point4 = app.Point3dFromXYZ(-column_length / 2 + Data.protective_layer_thinckness + Data.stirrup_diameter + Data.longitudinal_rebar_diameter / 2, -(column_width - Data.protective_layer_thinckness * 2 - Data.stirrup_diameter * 2 - Data.longitudinal_rebar_diameter) / 6, foundation_height / 2 + column_height - length / 2);
                        Point3d left_point5 = app.Point3dFromXYZ(-column_length / 2 + Data.protective_layer_thinckness + Data.stirrup_diameter + Data.longitudinal_rebar_diameter / 2, -(column_width - Data.protective_layer_thinckness * 2 - Data.stirrup_diameter * 2 - Data.longitudinal_rebar_diameter) / 6 * 2, foundation_height / 2 + column_height - length / 2);
                        Point3d right_point1 = app.Point3dFromXYZ(column_length / 2 - Data.protective_layer_thinckness - Data.stirrup_diameter - Data.longitudinal_rebar_diameter / 2, (column_width - Data.protective_layer_thinckness * 2 - Data.stirrup_diameter * 2 - Data.longitudinal_rebar_diameter) / 6 * 2, foundation_height / 2 + column_height - length / 2);
                        Point3d right_point2 = app.Point3dFromXYZ(column_length / 2 - Data.protective_layer_thinckness - Data.stirrup_diameter - Data.longitudinal_rebar_diameter / 2, (column_width - Data.protective_layer_thinckness * 2 - Data.stirrup_diameter * 2 - Data.longitudinal_rebar_diameter) / 6, foundation_height / 2 + column_height - length / 2);
                        Point3d right_point3 = app.Point3dFromXYZ(column_length / 2 - Data.protective_layer_thinckness - Data.stirrup_diameter - Data.longitudinal_rebar_diameter / 2, 0, foundation_height / 2 + column_height - length / 2);
                        Point3d right_point4 = app.Point3dFromXYZ(column_length / 2 - Data.protective_layer_thinckness - Data.stirrup_diameter - Data.longitudinal_rebar_diameter / 2, -(column_width - Data.protective_layer_thinckness * 2 - Data.stirrup_diameter * 2 - Data.longitudinal_rebar_diameter) / 6, foundation_height / 2 + column_height - length / 2);
                        Point3d right_point5 = app.Point3dFromXYZ(column_length / 2 - Data.protective_layer_thinckness - Data.stirrup_diameter - Data.longitudinal_rebar_diameter / 2, -(column_width - Data.protective_layer_thinckness * 2 - Data.stirrup_diameter * 2 - Data.longitudinal_rebar_diameter) / 6 * 2, foundation_height / 2 + column_height - length / 2);
                        left_bar1.Move(left_point1);
                        left_bar2.Move(left_point2);
                        left_bar3.Move(left_point3);
                        left_bar4.Move(left_point4);
                        left_bar5.Move(left_point5);
                        right_bar1.Move(right_point1);
                        right_bar2.Move(right_point2);
                        right_bar3.Move(right_point3);
                        right_bar4.Move(right_point4);
                        right_bar5.Move(right_point5);
                        y_longitudinal_bars = app.SmartSolid.SolidUnion(left_bar1.AsSmartSolidElement, left_bar2.AsSmartSolidElement);
                        y_longitudinal_bars = app.SmartSolid.SolidUnion(y_longitudinal_bars.AsSmartSolidElement, left_bar3.AsSmartSolidElement);
                        y_longitudinal_bars = app.SmartSolid.SolidUnion(y_longitudinal_bars.AsSmartSolidElement, left_bar4.AsSmartSolidElement);
                        y_longitudinal_bars = app.SmartSolid.SolidUnion(y_longitudinal_bars.AsSmartSolidElement, left_bar5.AsSmartSolidElement);
                        y_longitudinal_bars = app.SmartSolid.SolidUnion(y_longitudinal_bars.AsSmartSolidElement, right_bar1.AsSmartSolidElement);
                        y_longitudinal_bars = app.SmartSolid.SolidUnion(y_longitudinal_bars.AsSmartSolidElement, right_bar2.AsSmartSolidElement);
                        y_longitudinal_bars = app.SmartSolid.SolidUnion(y_longitudinal_bars.AsSmartSolidElement, right_bar3.AsSmartSolidElement);
                        y_longitudinal_bars = app.SmartSolid.SolidUnion(y_longitudinal_bars.AsSmartSolidElement, right_bar4.AsSmartSolidElement);
                        y_longitudinal_bars = app.SmartSolid.SolidUnion(y_longitudinal_bars.AsSmartSolidElement, right_bar5.AsSmartSolidElement);
                        break;
                    }
                case 8:
                    {
                        Element left_bar1 = create_single_longitudinal_bar(length, bending_length, Data.ANGLE_270);
                        Element left_bar2 = create_single_longitudinal_bar(length, bending_length, Data.ANGLE_270);
                        Element left_bar3 = create_single_longitudinal_bar(length, bending_length, Data.ANGLE_270);
                        Element left_bar4 = create_single_longitudinal_bar(length, bending_length, Data.ANGLE_270);
                        Element left_bar5 = create_single_longitudinal_bar(length, bending_length, Data.ANGLE_270);
                        Element left_bar6 = create_single_longitudinal_bar(length, bending_length, Data.ANGLE_270);
                        Element right_bar1 = create_single_longitudinal_bar(length, bending_length, Data.ANGLE_90);
                        Element right_bar2 = create_single_longitudinal_bar(length, bending_length, Data.ANGLE_90);
                        Element right_bar3 = create_single_longitudinal_bar(length, bending_length, Data.ANGLE_90);
                        Element right_bar4 = create_single_longitudinal_bar(length, bending_length, Data.ANGLE_90);
                        Element right_bar5 = create_single_longitudinal_bar(length, bending_length, Data.ANGLE_90);
                        Element right_bar6 = create_single_longitudinal_bar(length, bending_length, Data.ANGLE_90);
                        Point3d left_point1 = app.Point3dFromXYZ(-column_length / 2 + Data.protective_layer_thinckness + Data.stirrup_diameter + Data.longitudinal_rebar_diameter / 2, (column_width - Data.protective_layer_thinckness * 2 - Data.stirrup_diameter * 2 - Data.longitudinal_rebar_diameter) / 7 * 2.5, foundation_height / 2 + column_height - length / 2);
                        Point3d left_point2 = app.Point3dFromXYZ(-column_length / 2 + Data.protective_layer_thinckness + Data.stirrup_diameter + Data.longitudinal_rebar_diameter / 2, (column_width - Data.protective_layer_thinckness * 2 - Data.stirrup_diameter * 2 - Data.longitudinal_rebar_diameter) / 7 * 1.5, foundation_height / 2 + column_height - length / 2);
                        Point3d left_point3 = app.Point3dFromXYZ(-column_length / 2 + Data.protective_layer_thinckness + Data.stirrup_diameter + Data.longitudinal_rebar_diameter / 2, (column_width - Data.protective_layer_thinckness * 2 - Data.stirrup_diameter * 2 - Data.longitudinal_rebar_diameter) / 7 * 0.5, foundation_height / 2 + column_height - length / 2);
                        Point3d left_point4 = app.Point3dFromXYZ(-column_length / 2 + Data.protective_layer_thinckness + Data.stirrup_diameter + Data.longitudinal_rebar_diameter / 2, -(column_width - Data.protective_layer_thinckness * 2 - Data.stirrup_diameter * 2 - Data.longitudinal_rebar_diameter) / 7 * 0.5, foundation_height / 2 + column_height - length / 2);
                        Point3d left_point5 = app.Point3dFromXYZ(-column_length / 2 + Data.protective_layer_thinckness + Data.stirrup_diameter + Data.longitudinal_rebar_diameter / 2, -(column_width - Data.protective_layer_thinckness * 2 - Data.stirrup_diameter * 2 - Data.longitudinal_rebar_diameter) / 7 * 1.5, foundation_height / 2 + column_height - length / 2);
                        Point3d left_point6 = app.Point3dFromXYZ(-column_length / 2 + Data.protective_layer_thinckness + Data.stirrup_diameter + Data.longitudinal_rebar_diameter / 2, -(column_width - Data.protective_layer_thinckness * 2 - Data.stirrup_diameter * 2 - Data.longitudinal_rebar_diameter) / 7 * 2.5, foundation_height / 2 + column_height - length / 2);
                        Point3d right_point1 = app.Point3dFromXYZ(column_length / 2 - Data.protective_layer_thinckness - Data.stirrup_diameter - Data.longitudinal_rebar_diameter / 2, (column_width - Data.protective_layer_thinckness * 2 - Data.stirrup_diameter * 2 - Data.longitudinal_rebar_diameter) / 7 * 2.5, foundation_height / 2 + column_height - length / 2);
                        Point3d right_point2 = app.Point3dFromXYZ(column_length / 2 - Data.protective_layer_thinckness - Data.stirrup_diameter - Data.longitudinal_rebar_diameter / 2, (column_width - Data.protective_layer_thinckness * 2 - Data.stirrup_diameter * 2 - Data.longitudinal_rebar_diameter) / 7 * 1.5, foundation_height / 2 + column_height - length / 2);
                        Point3d right_point3 = app.Point3dFromXYZ(column_length / 2 - Data.protective_layer_thinckness - Data.stirrup_diameter - Data.longitudinal_rebar_diameter / 2, (column_width - Data.protective_layer_thinckness * 2 - Data.stirrup_diameter * 2 - Data.longitudinal_rebar_diameter) / 7 * 0.5, foundation_height / 2 + column_height - length / 2);
                        Point3d right_point4 = app.Point3dFromXYZ(column_length / 2 - Data.protective_layer_thinckness - Data.stirrup_diameter - Data.longitudinal_rebar_diameter / 2, -(column_width - Data.protective_layer_thinckness * 2 - Data.stirrup_diameter * 2 - Data.longitudinal_rebar_diameter) / 7 * 0.5, foundation_height / 2 + column_height - length / 2);
                        Point3d right_point5 = app.Point3dFromXYZ(column_length / 2 - Data.protective_layer_thinckness - Data.stirrup_diameter - Data.longitudinal_rebar_diameter / 2, -(column_width - Data.protective_layer_thinckness * 2 - Data.stirrup_diameter * 2 - Data.longitudinal_rebar_diameter) / 7 * 1.5, foundation_height / 2 + column_height - length / 2);
                        Point3d right_point6 = app.Point3dFromXYZ(column_length / 2 - Data.protective_layer_thinckness - Data.stirrup_diameter - Data.longitudinal_rebar_diameter / 2, -(column_width - Data.protective_layer_thinckness * 2 - Data.stirrup_diameter * 2 - Data.longitudinal_rebar_diameter) / 6 * 2.5, foundation_height / 2 + column_height - length / 2);
                        left_bar1.Move(left_point1);
                        left_bar2.Move(left_point2);
                        left_bar3.Move(left_point3);
                        left_bar4.Move(left_point4);
                        left_bar5.Move(left_point5);
                        left_bar6.Move(left_point6);
                        right_bar1.Move(right_point1);
                        right_bar2.Move(right_point2);
                        right_bar3.Move(right_point3);
                        right_bar4.Move(right_point4);
                        right_bar5.Move(right_point5);
                        right_bar6.Move(right_point6);
                        y_longitudinal_bars = app.SmartSolid.SolidUnion(left_bar1.AsSmartSolidElement, left_bar2.AsSmartSolidElement);
                        y_longitudinal_bars = app.SmartSolid.SolidUnion(y_longitudinal_bars.AsSmartSolidElement, left_bar3.AsSmartSolidElement);
                        y_longitudinal_bars = app.SmartSolid.SolidUnion(y_longitudinal_bars.AsSmartSolidElement, left_bar4.AsSmartSolidElement);
                        y_longitudinal_bars = app.SmartSolid.SolidUnion(y_longitudinal_bars.AsSmartSolidElement, left_bar5.AsSmartSolidElement);
                        y_longitudinal_bars = app.SmartSolid.SolidUnion(y_longitudinal_bars.AsSmartSolidElement, left_bar6.AsSmartSolidElement);
                        y_longitudinal_bars = app.SmartSolid.SolidUnion(y_longitudinal_bars.AsSmartSolidElement, right_bar1.AsSmartSolidElement);
                        y_longitudinal_bars = app.SmartSolid.SolidUnion(y_longitudinal_bars.AsSmartSolidElement, right_bar2.AsSmartSolidElement);
                        y_longitudinal_bars = app.SmartSolid.SolidUnion(y_longitudinal_bars.AsSmartSolidElement, right_bar3.AsSmartSolidElement);
                        y_longitudinal_bars = app.SmartSolid.SolidUnion(y_longitudinal_bars.AsSmartSolidElement, right_bar4.AsSmartSolidElement);
                        y_longitudinal_bars = app.SmartSolid.SolidUnion(y_longitudinal_bars.AsSmartSolidElement, right_bar5.AsSmartSolidElement);
                        y_longitudinal_bars = app.SmartSolid.SolidUnion(y_longitudinal_bars.AsSmartSolidElement, right_bar6.AsSmartSolidElement);
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
                        Element up_bar1 = create_single_longitudinal_bar(length, bending_length, Data.ANGLE_180);
                        Element up_bar2 = create_single_longitudinal_bar(length, bending_length, Data.ANGLE_180);
                        Element down_bar1 = create_single_longitudinal_bar(length, bending_length, Data.ANGLE_0);
                        Element down_bar2 = create_single_longitudinal_bar(length, bending_length, Data.ANGLE_0);
                        Point3d up_point1 = app.Point3dFromXYZ((column_length - Data.protective_layer_thinckness * 2 - Data.stirrup_diameter * 2 - Data.longitudinal_rebar_diameter) / 3 / 2, column_width / 2 - Data.protective_layer_thinckness - Data.stirrup_diameter - Data.longitudinal_rebar_diameter / 2, foundation_height / 2 + column_height - length / 2);
                        Point3d up_point2 = app.Point3dFromXYZ(-(column_length - Data.protective_layer_thinckness * 2 - Data.stirrup_diameter * 2 - Data.longitudinal_rebar_diameter) / 3 / 2, column_width / 2 - Data.protective_layer_thinckness - Data.stirrup_diameter - Data.longitudinal_rebar_diameter / 2, foundation_height / 2 + column_height - length / 2);
                        Point3d down_point1 = app.Point3dFromXYZ((column_length - Data.protective_layer_thinckness * 2 - Data.stirrup_diameter * 2 - Data.longitudinal_rebar_diameter) / 3 / 2, -column_width / 2 + Data.protective_layer_thinckness + Data.stirrup_diameter + Data.longitudinal_rebar_diameter / 2, foundation_height / 2 + column_height - length / 2);
                        Point3d down_point2 = app.Point3dFromXYZ(-(column_length - Data.protective_layer_thinckness * 2 - Data.stirrup_diameter * 2 - Data.longitudinal_rebar_diameter) / 3 / 2, -column_width / 2 + Data.protective_layer_thinckness + Data.stirrup_diameter + Data.longitudinal_rebar_diameter / 2, foundation_height / 2 + column_height - length / 2);
                        up_bar1.Move(up_point1);
                        up_bar2.Move(up_point2);
                        down_bar1.Move(down_point1);
                        down_bar2.Move(down_point2);
                        x_longitudinal_bars = app.SmartSolid.SolidUnion(up_bar1.AsSmartSolidElement, up_bar2.AsSmartSolidElement);
                        x_longitudinal_bars = app.SmartSolid.SolidUnion(x_longitudinal_bars.AsSmartSolidElement, down_bar1.AsSmartSolidElement);
                        x_longitudinal_bars = app.SmartSolid.SolidUnion(x_longitudinal_bars.AsSmartSolidElement, down_bar2.AsSmartSolidElement);
                        break;
                    }
                case 5:
                    {
                        Element up_bar1 = create_single_longitudinal_bar(length, bending_length, Data.ANGLE_180);
                        Element up_bar2 = create_single_longitudinal_bar(length, bending_length, Data.ANGLE_180);
                        Element up_bar3 = create_single_longitudinal_bar(length, bending_length, Data.ANGLE_180);
                        Element down_bar1 = create_single_longitudinal_bar(length, bending_length, Data.ANGLE_0);
                        Element down_bar2 = create_single_longitudinal_bar(length, bending_length, Data.ANGLE_0);
                        Element down_bar3 = create_single_longitudinal_bar(length, bending_length, Data.ANGLE_0);
                        Point3d up_point1 = app.Point3dFromXYZ((column_length - Data.protective_layer_thinckness * 2 - Data.stirrup_diameter * 2 - Data.longitudinal_rebar_diameter) / 4, column_width / 2 - Data.protective_layer_thinckness - Data.stirrup_diameter - Data.longitudinal_rebar_diameter / 2, foundation_height / 2 + column_height - length / 2);
                        Point3d up_point2 = app.Point3dFromXYZ(0, column_width / 2 - Data.protective_layer_thinckness - Data.stirrup_diameter - Data.longitudinal_rebar_diameter / 2, foundation_height / 2 + column_height - length / 2);
                        Point3d up_point3 = app.Point3dFromXYZ(-(column_length - Data.protective_layer_thinckness * 2 - Data.stirrup_diameter * 2 - Data.longitudinal_rebar_diameter) / 4, column_width / 2 - Data.protective_layer_thinckness - Data.stirrup_diameter - Data.longitudinal_rebar_diameter / 2, foundation_height / 2 + column_height - length / 2);
                        Point3d down_point1 = app.Point3dFromXYZ((column_length - Data.protective_layer_thinckness * 2 - Data.stirrup_diameter * 2 - Data.longitudinal_rebar_diameter) / 4, -column_width / 2 + Data.protective_layer_thinckness + Data.stirrup_diameter + Data.longitudinal_rebar_diameter / 2, foundation_height / 2 + column_height - length / 2);
                        Point3d down_point2 = app.Point3dFromXYZ(0, -column_width / 2 + Data.protective_layer_thinckness + Data.stirrup_diameter + Data.longitudinal_rebar_diameter / 2, foundation_height / 2 + column_height - length / 2);
                        Point3d down_point3 = app.Point3dFromXYZ(-(column_length - Data.protective_layer_thinckness * 2 - Data.stirrup_diameter * 2 - Data.longitudinal_rebar_diameter) / 4, -column_width / 2 + Data.protective_layer_thinckness + Data.stirrup_diameter + Data.longitudinal_rebar_diameter / 2, foundation_height / 2 + column_height - length / 2);
                        up_bar1.Move(up_point1);
                        up_bar2.Move(up_point2);
                        up_bar3.Move(up_point3);
                        down_bar1.Move(down_point1);
                        down_bar2.Move(down_point2);
                        down_bar3.Move(down_point3);
                        x_longitudinal_bars = app.SmartSolid.SolidUnion(up_bar1.AsSmartSolidElement, up_bar2.AsSmartSolidElement);
                        x_longitudinal_bars = app.SmartSolid.SolidUnion(x_longitudinal_bars.AsSmartSolidElement, up_bar3.AsSmartSolidElement);
                        x_longitudinal_bars = app.SmartSolid.SolidUnion(x_longitudinal_bars.AsSmartSolidElement, down_bar1.AsSmartSolidElement);
                        x_longitudinal_bars = app.SmartSolid.SolidUnion(x_longitudinal_bars.AsSmartSolidElement, down_bar2.AsSmartSolidElement);
                        x_longitudinal_bars = app.SmartSolid.SolidUnion(x_longitudinal_bars.AsSmartSolidElement, down_bar3.AsSmartSolidElement);
                        break;
                    }
                case 6:
                    {
                        Element up_bar1 = create_single_longitudinal_bar(length, bending_length, Data.ANGLE_180);
                        Element up_bar2 = create_single_longitudinal_bar(length, bending_length, Data.ANGLE_180);
                        Element up_bar3 = create_single_longitudinal_bar(length, bending_length, Data.ANGLE_180);
                        Element up_bar4 = create_single_longitudinal_bar(length, bending_length, Data.ANGLE_180);
                        Element down_bar1 = create_single_longitudinal_bar(length, bending_length, Data.ANGLE_0);
                        Element down_bar2 = create_single_longitudinal_bar(length, bending_length, Data.ANGLE_0);
                        Element down_bar3 = create_single_longitudinal_bar(length, bending_length, Data.ANGLE_0);
                        Element down_bar4 = create_single_longitudinal_bar(length, bending_length, Data.ANGLE_0);
                        Point3d up_point1 = app.Point3dFromXYZ((column_length - Data.protective_layer_thinckness * 2 - Data.stirrup_diameter * 2 - Data.longitudinal_rebar_diameter) / 5 * 1.5, column_width / 2 - Data.protective_layer_thinckness - Data.stirrup_diameter - Data.longitudinal_rebar_diameter / 2, foundation_height / 2 + column_height - length / 2);
                        Point3d up_point2 = app.Point3dFromXYZ((column_length - Data.protective_layer_thinckness * 2 - Data.stirrup_diameter * 2 - Data.longitudinal_rebar_diameter) / 5 * 0.5, column_width / 2 - Data.protective_layer_thinckness - Data.stirrup_diameter - Data.longitudinal_rebar_diameter / 2, foundation_height / 2 + column_height - length / 2);
                        Point3d up_point3 = app.Point3dFromXYZ(-(column_length - Data.protective_layer_thinckness * 2 - Data.stirrup_diameter * 2 - Data.longitudinal_rebar_diameter) / 5 * 0.5, column_width / 2 - Data.protective_layer_thinckness - Data.stirrup_diameter - Data.longitudinal_rebar_diameter / 2, foundation_height / 2 + column_height - length / 2);
                        Point3d up_point4 = app.Point3dFromXYZ(-(column_length - Data.protective_layer_thinckness * 2 - Data.stirrup_diameter * 2 - Data.longitudinal_rebar_diameter) / 5 * 1.5, column_width / 2 - Data.protective_layer_thinckness - Data.stirrup_diameter - Data.longitudinal_rebar_diameter / 2, foundation_height / 2 + column_height - length / 2);
                        Point3d down_point1 = app.Point3dFromXYZ((column_length - Data.protective_layer_thinckness * 2 - Data.stirrup_diameter * 2 - Data.longitudinal_rebar_diameter) / 5 * 1.5, -column_width / 2 + Data.protective_layer_thinckness + Data.stirrup_diameter + Data.longitudinal_rebar_diameter / 2, foundation_height / 2 + column_height - length / 2);
                        Point3d down_point2 = app.Point3dFromXYZ((column_length - Data.protective_layer_thinckness * 2 - Data.stirrup_diameter * 2 - Data.longitudinal_rebar_diameter) / 5 * 0.5, -column_width / 2 + Data.protective_layer_thinckness + Data.stirrup_diameter + Data.longitudinal_rebar_diameter / 2, foundation_height / 2 + column_height - length / 2);
                        Point3d down_point3 = app.Point3dFromXYZ(-(column_length - Data.protective_layer_thinckness * 2 - Data.stirrup_diameter * 2 - Data.longitudinal_rebar_diameter) / 5 * 0.5, -column_width / 2 + Data.protective_layer_thinckness + Data.stirrup_diameter + Data.longitudinal_rebar_diameter / 2, foundation_height / 2 + column_height - length / 2);
                        Point3d down_point4 = app.Point3dFromXYZ(-(column_length - Data.protective_layer_thinckness * 2 - Data.stirrup_diameter * 2 - Data.longitudinal_rebar_diameter) / 5 * 1.5, -column_width / 2 + Data.protective_layer_thinckness + Data.stirrup_diameter + Data.longitudinal_rebar_diameter / 2, foundation_height / 2 + column_height - length / 2);
                        up_bar1.Move(up_point1);
                        up_bar2.Move(up_point2);
                        up_bar3.Move(up_point3);
                        up_bar4.Move(up_point4);
                        down_bar1.Move(down_point1);
                        down_bar2.Move(down_point2);
                        down_bar3.Move(down_point3);
                        down_bar4.Move(down_point4);
                        x_longitudinal_bars = app.SmartSolid.SolidUnion(up_bar1.AsSmartSolidElement, up_bar2.AsSmartSolidElement);
                        x_longitudinal_bars = app.SmartSolid.SolidUnion(x_longitudinal_bars.AsSmartSolidElement, up_bar3.AsSmartSolidElement);
                        x_longitudinal_bars = app.SmartSolid.SolidUnion(x_longitudinal_bars.AsSmartSolidElement, up_bar4.AsSmartSolidElement);
                        x_longitudinal_bars = app.SmartSolid.SolidUnion(x_longitudinal_bars.AsSmartSolidElement, down_bar1.AsSmartSolidElement);
                        x_longitudinal_bars = app.SmartSolid.SolidUnion(x_longitudinal_bars.AsSmartSolidElement, down_bar2.AsSmartSolidElement);
                        x_longitudinal_bars = app.SmartSolid.SolidUnion(x_longitudinal_bars.AsSmartSolidElement, down_bar3.AsSmartSolidElement);
                        x_longitudinal_bars = app.SmartSolid.SolidUnion(x_longitudinal_bars.AsSmartSolidElement, down_bar4.AsSmartSolidElement);
                        break;
                    }
                case 7:
                    {
                        Element up_bar1 = create_single_longitudinal_bar(length, bending_length, Data.ANGLE_180);
                        Element up_bar2 = create_single_longitudinal_bar(length, bending_length, Data.ANGLE_180);
                        Element up_bar3 = create_single_longitudinal_bar(length, bending_length, Data.ANGLE_180);
                        Element up_bar4 = create_single_longitudinal_bar(length, bending_length, Data.ANGLE_180);
                        Element up_bar5 = create_single_longitudinal_bar(length, bending_length, Data.ANGLE_180);
                        Element down_bar1 = create_single_longitudinal_bar(length, bending_length, Data.ANGLE_0);
                        Element down_bar2 = create_single_longitudinal_bar(length, bending_length, Data.ANGLE_0);
                        Element down_bar3 = create_single_longitudinal_bar(length, bending_length, Data.ANGLE_0);
                        Element down_bar4 = create_single_longitudinal_bar(length, bending_length, Data.ANGLE_0);
                        Element down_bar5 = create_single_longitudinal_bar(length, bending_length, Data.ANGLE_0);
                        Point3d up_point1 = app.Point3dFromXYZ((column_length - Data.protective_layer_thinckness * 2 - Data.stirrup_diameter * 2 - Data.longitudinal_rebar_diameter) / 6 * 2, column_width / 2 - Data.protective_layer_thinckness - Data.stirrup_diameter - Data.longitudinal_rebar_diameter / 2, foundation_height / 2 + column_height - length / 2);
                        Point3d up_point2 = app.Point3dFromXYZ((column_length - Data.protective_layer_thinckness * 2 - Data.stirrup_diameter * 2 - Data.longitudinal_rebar_diameter) / 6, column_width / 2 - Data.protective_layer_thinckness - Data.stirrup_diameter - Data.longitudinal_rebar_diameter / 2, foundation_height / 2 + column_height - length / 2);
                        Point3d up_point3 = app.Point3dFromXYZ(0, column_width / 2 - Data.protective_layer_thinckness - Data.stirrup_diameter - Data.longitudinal_rebar_diameter / 2, foundation_height / 2 + column_height - length / 2);
                        Point3d up_point4 = app.Point3dFromXYZ(-(column_length - Data.protective_layer_thinckness * 2 - Data.stirrup_diameter * 2 - Data.longitudinal_rebar_diameter) / 6, column_width / 2 - Data.protective_layer_thinckness - Data.stirrup_diameter - Data.longitudinal_rebar_diameter / 2, foundation_height / 2 + column_height - length / 2);
                        Point3d up_point5 = app.Point3dFromXYZ(-(column_length - Data.protective_layer_thinckness * 2 - Data.stirrup_diameter * 2 - Data.longitudinal_rebar_diameter) / 6 * 2, column_width / 2 - Data.protective_layer_thinckness - Data.stirrup_diameter - Data.longitudinal_rebar_diameter / 2, foundation_height / 2 + column_height - length / 2);
                        Point3d down_point1 = app.Point3dFromXYZ((column_length - Data.protective_layer_thinckness * 2 - Data.stirrup_diameter * 2 - Data.longitudinal_rebar_diameter) / 6 * 2, -column_width / 2 + Data.protective_layer_thinckness + Data.stirrup_diameter + Data.longitudinal_rebar_diameter / 2, foundation_height / 2 + column_height - length / 2);
                        Point3d down_point2 = app.Point3dFromXYZ((column_length - Data.protective_layer_thinckness * 2 - Data.stirrup_diameter * 2 - Data.longitudinal_rebar_diameter) / 6, -column_width / 2 + Data.protective_layer_thinckness + Data.stirrup_diameter + Data.longitudinal_rebar_diameter / 2, foundation_height / 2 + column_height - length / 2);
                        Point3d down_point3 = app.Point3dFromXYZ(0, -column_width / 2 + Data.protective_layer_thinckness + Data.stirrup_diameter + Data.longitudinal_rebar_diameter / 2, foundation_height / 2 + column_height - length / 2);
                        Point3d down_point4 = app.Point3dFromXYZ(-(column_length - Data.protective_layer_thinckness * 2 - Data.stirrup_diameter * 2 - Data.longitudinal_rebar_diameter) / 6, -column_width / 2 + Data.protective_layer_thinckness + Data.stirrup_diameter + Data.longitudinal_rebar_diameter / 2, foundation_height / 2 + column_height - length / 2);
                        Point3d down_point5 = app.Point3dFromXYZ(-(column_length - Data.protective_layer_thinckness * 2 - Data.stirrup_diameter * 2 - Data.longitudinal_rebar_diameter) / 6 * 2, -column_width / 2 + Data.protective_layer_thinckness + Data.stirrup_diameter + Data.longitudinal_rebar_diameter / 2, foundation_height / 2 + column_height - length / 2);
                        up_bar1.Move(up_point1);
                        up_bar2.Move(up_point2);
                        up_bar3.Move(up_point3);
                        up_bar4.Move(up_point4);
                        up_bar5.Move(up_point5);
                        down_bar1.Move(down_point1);
                        down_bar2.Move(down_point2);
                        down_bar3.Move(down_point3);
                        down_bar4.Move(down_point4);
                        down_bar5.Move(down_point5);
                        x_longitudinal_bars = app.SmartSolid.SolidUnion(up_bar1.AsSmartSolidElement, up_bar2.AsSmartSolidElement);
                        x_longitudinal_bars = app.SmartSolid.SolidUnion(x_longitudinal_bars.AsSmartSolidElement, up_bar3.AsSmartSolidElement);
                        x_longitudinal_bars = app.SmartSolid.SolidUnion(x_longitudinal_bars.AsSmartSolidElement, up_bar4.AsSmartSolidElement);
                        x_longitudinal_bars = app.SmartSolid.SolidUnion(x_longitudinal_bars.AsSmartSolidElement, up_bar5.AsSmartSolidElement);
                        x_longitudinal_bars = app.SmartSolid.SolidUnion(x_longitudinal_bars.AsSmartSolidElement, down_bar1.AsSmartSolidElement);
                        x_longitudinal_bars = app.SmartSolid.SolidUnion(x_longitudinal_bars.AsSmartSolidElement, down_bar2.AsSmartSolidElement);
                        x_longitudinal_bars = app.SmartSolid.SolidUnion(x_longitudinal_bars.AsSmartSolidElement, down_bar3.AsSmartSolidElement);
                        x_longitudinal_bars = app.SmartSolid.SolidUnion(x_longitudinal_bars.AsSmartSolidElement, down_bar4.AsSmartSolidElement);
                        x_longitudinal_bars = app.SmartSolid.SolidUnion(x_longitudinal_bars.AsSmartSolidElement, down_bar5.AsSmartSolidElement);
                        break;
                    }
                case 8:
                    {
                        Element up_bar1 = create_single_longitudinal_bar(length, bending_length, Data.ANGLE_180);
                        Element up_bar2 = create_single_longitudinal_bar(length, bending_length, Data.ANGLE_180);
                        Element up_bar3 = create_single_longitudinal_bar(length, bending_length, Data.ANGLE_180);
                        Element up_bar4 = create_single_longitudinal_bar(length, bending_length, Data.ANGLE_180);
                        Element up_bar5 = create_single_longitudinal_bar(length, bending_length, Data.ANGLE_180);
                        Element up_bar6 = create_single_longitudinal_bar(length, bending_length, Data.ANGLE_180);
                        Element down_bar1 = create_single_longitudinal_bar(length, bending_length, Data.ANGLE_0);
                        Element down_bar2 = create_single_longitudinal_bar(length, bending_length, Data.ANGLE_0);
                        Element down_bar3 = create_single_longitudinal_bar(length, bending_length, Data.ANGLE_0);
                        Element down_bar4 = create_single_longitudinal_bar(length, bending_length, Data.ANGLE_0);
                        Element down_bar5 = create_single_longitudinal_bar(length, bending_length, Data.ANGLE_0);
                        Element down_bar6 = create_single_longitudinal_bar(length, bending_length, Data.ANGLE_0);
                        Point3d up_point1 = app.Point3dFromXYZ((column_length - Data.protective_layer_thinckness * 2 - Data.stirrup_diameter * 2 - Data.longitudinal_rebar_diameter) / 7 * 2.5, column_width / 2 - Data.protective_layer_thinckness - Data.stirrup_diameter - Data.longitudinal_rebar_diameter / 2, foundation_height / 2 + column_height - length / 2);
                        Point3d up_point2 = app.Point3dFromXYZ((column_length - Data.protective_layer_thinckness * 2 - Data.stirrup_diameter * 2 - Data.longitudinal_rebar_diameter) / 7 * 1.5, column_width / 2 - Data.protective_layer_thinckness - Data.stirrup_diameter - Data.longitudinal_rebar_diameter / 2, foundation_height / 2 + column_height - length / 2);
                        Point3d up_point3 = app.Point3dFromXYZ((column_length - Data.protective_layer_thinckness * 2 - Data.stirrup_diameter * 2 - Data.longitudinal_rebar_diameter) / 7 * 0.5, column_width / 2 - Data.protective_layer_thinckness - Data.stirrup_diameter - Data.longitudinal_rebar_diameter / 2, foundation_height / 2 + column_height - length / 2);
                        Point3d up_point4 = app.Point3dFromXYZ(-(column_length - Data.protective_layer_thinckness * 2 - Data.stirrup_diameter * 2 - Data.longitudinal_rebar_diameter) / 7 * 0.5, column_width / 2 - Data.protective_layer_thinckness - Data.stirrup_diameter - Data.longitudinal_rebar_diameter / 2, foundation_height / 2 + column_height - length / 2);
                        Point3d up_point5 = app.Point3dFromXYZ(-(column_length - Data.protective_layer_thinckness * 2 - Data.stirrup_diameter * 2 - Data.longitudinal_rebar_diameter) / 7 * 1.5, column_width / 2 - Data.protective_layer_thinckness - Data.stirrup_diameter - Data.longitudinal_rebar_diameter / 2, foundation_height / 2 + column_height - length / 2);
                        Point3d up_point6 = app.Point3dFromXYZ(-(column_length - Data.protective_layer_thinckness * 2 - Data.stirrup_diameter * 2 - Data.longitudinal_rebar_diameter) / 7 * 2.5, column_width / 2 - Data.protective_layer_thinckness - Data.stirrup_diameter - Data.longitudinal_rebar_diameter / 2, foundation_height / 2 + column_height - length / 2);
                        Point3d down_point1 = app.Point3dFromXYZ((column_length - Data.protective_layer_thinckness * 2 - Data.stirrup_diameter * 2 - Data.longitudinal_rebar_diameter) / 7 * 2.5, -column_width / 2 + Data.protective_layer_thinckness + Data.stirrup_diameter + Data.longitudinal_rebar_diameter / 2, foundation_height / 2 + column_height - length / 2);
                        Point3d down_point2 = app.Point3dFromXYZ((column_length - Data.protective_layer_thinckness * 2 - Data.stirrup_diameter * 2 - Data.longitudinal_rebar_diameter) / 7 * 1.5, -column_width / 2 + Data.protective_layer_thinckness + Data.stirrup_diameter + Data.longitudinal_rebar_diameter / 2, foundation_height / 2 + column_height - length / 2);
                        Point3d down_point3 = app.Point3dFromXYZ((column_length - Data.protective_layer_thinckness * 2 - Data.stirrup_diameter * 2 - Data.longitudinal_rebar_diameter) / 7 * 0.5, -column_width / 2 + Data.protective_layer_thinckness + Data.stirrup_diameter + Data.longitudinal_rebar_diameter / 2, foundation_height / 2 + column_height - length / 2);
                        Point3d down_point4 = app.Point3dFromXYZ(-(column_length - Data.protective_layer_thinckness * 2 - Data.stirrup_diameter * 2 - Data.longitudinal_rebar_diameter) / 7 * 0.5, -column_width / 2 + Data.protective_layer_thinckness + Data.stirrup_diameter + Data.longitudinal_rebar_diameter / 2, foundation_height / 2 + column_height - length / 2);
                        Point3d down_point5 = app.Point3dFromXYZ(-(column_length - Data.protective_layer_thinckness * 2 - Data.stirrup_diameter * 2 - Data.longitudinal_rebar_diameter) / 7 * 1.5, -column_width / 2 + Data.protective_layer_thinckness + Data.stirrup_diameter + Data.longitudinal_rebar_diameter / 2, foundation_height / 2 + column_height - length / 2);
                        Point3d down_point6 = app.Point3dFromXYZ(-(column_length - Data.protective_layer_thinckness * 2 - Data.stirrup_diameter * 2 - Data.longitudinal_rebar_diameter) / 7 * 2.5, -column_width / 2 + Data.protective_layer_thinckness + Data.stirrup_diameter + Data.longitudinal_rebar_diameter / 2, foundation_height / 2 + column_height - length / 2);
                        up_bar1.Move(up_point1);
                        up_bar2.Move(up_point2);
                        up_bar3.Move(up_point3);
                        up_bar4.Move(up_point4);
                        up_bar5.Move(up_point5);
                        up_bar6.Move(up_point6);
                        down_bar1.Move(down_point1);
                        down_bar2.Move(down_point2);
                        down_bar3.Move(down_point3);
                        down_bar4.Move(down_point4);
                        down_bar5.Move(down_point5);
                        down_bar6.Move(down_point6);
                        x_longitudinal_bars = app.SmartSolid.SolidUnion(up_bar1.AsSmartSolidElement, up_bar2.AsSmartSolidElement);
                        x_longitudinal_bars = app.SmartSolid.SolidUnion(x_longitudinal_bars.AsSmartSolidElement, up_bar3.AsSmartSolidElement);
                        x_longitudinal_bars = app.SmartSolid.SolidUnion(x_longitudinal_bars.AsSmartSolidElement, up_bar4.AsSmartSolidElement);
                        x_longitudinal_bars = app.SmartSolid.SolidUnion(x_longitudinal_bars.AsSmartSolidElement, up_bar5.AsSmartSolidElement);
                        x_longitudinal_bars = app.SmartSolid.SolidUnion(x_longitudinal_bars.AsSmartSolidElement, up_bar6.AsSmartSolidElement);
                        x_longitudinal_bars = app.SmartSolid.SolidUnion(x_longitudinal_bars.AsSmartSolidElement, down_bar1.AsSmartSolidElement);
                        x_longitudinal_bars = app.SmartSolid.SolidUnion(x_longitudinal_bars.AsSmartSolidElement, down_bar2.AsSmartSolidElement);
                        x_longitudinal_bars = app.SmartSolid.SolidUnion(x_longitudinal_bars.AsSmartSolidElement, down_bar3.AsSmartSolidElement);
                        x_longitudinal_bars = app.SmartSolid.SolidUnion(x_longitudinal_bars.AsSmartSolidElement, down_bar4.AsSmartSolidElement);
                        x_longitudinal_bars = app.SmartSolid.SolidUnion(x_longitudinal_bars.AsSmartSolidElement, down_bar5.AsSmartSolidElement);
                        x_longitudinal_bars = app.SmartSolid.SolidUnion(x_longitudinal_bars.AsSmartSolidElement, down_bar6.AsSmartSolidElement);
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
        public static Element create_longitudinal_bar_type6(double column_d, double column_height, double foundation_length, double foundation_width, double foundation_height, double length, double bending_length)
        {
            double angle_r = Data.lap_length / (Math.PI * column_d) * Data.ANGLE_360 / 2;
            Point3d down_right, down_left;
            down_right = app.Point3dFromXYZ((column_d / 2 - Data.protective_layer_thinckness - Data.stirrup_diameter - Data.longitudinal_rebar_diameter / 2) * Math.Sin(angle_r / 180 * Math.PI), -(column_d / 2 - Data.protective_layer_thinckness - Data.stirrup_diameter - Data.longitudinal_rebar_diameter / 2) * Math.Cos(angle_r / 180 * Math.PI), foundation_height / 2 + column_height - length / 2);
            down_left = app.Point3dFromXYZ(-(column_d / 2 - Data.protective_layer_thinckness - Data.stirrup_diameter - Data.longitudinal_rebar_diameter / 2) * Math.Sin(angle_r / 180 * Math.PI), -(column_d / 2 - Data.protective_layer_thinckness - Data.stirrup_diameter - Data.longitudinal_rebar_diameter / 2) * Math.Cos(angle_r / 180 * Math.PI), foundation_height / 2 + column_height - length / 2);
            Element down_right_bar = create_single_longitudinal_bar(length, bending_length, angle_r);
            Element down_left_bar = create_single_longitudinal_bar(length, bending_length, -angle_r);
            down_right_bar.Move(down_right);
            down_left_bar.Move(down_left);
            Element ret = app.SmartSolid.SolidUnion(down_left_bar.AsSmartSolidElement, down_right_bar.AsSmartSolidElement);
            return ret;
        }
        public static Element create_longitudinal_bar_type7(double diameter, double angle, double xzlength, double column_height, double foundation_length, double foundation_width, double foundation_height, double length, double bending_length)
        {
            Point3d down_left, down_right, up_left, up_right;
            up_left = app.Point3dFromXYZ(-(diameter / 2 - Data.stirrup_diameter / 2 - Data.longitudinal_rebar_diameter / 2) * Math.Cos(angle / 2 / Data.ANGLE_180 * Math.PI), (diameter / 2 - Data.stirrup_diameter / 2 - Data.longitudinal_rebar_diameter / 2) * Math.Sin(angle / 2 / Data.ANGLE_180 * Math.PI), foundation_height / 2 + column_height - length / 2);
            up_right = app.Point3dFromXYZ((diameter / 2 - Data.stirrup_diameter / 2 - Data.longitudinal_rebar_diameter / 2) * Math.Cos(angle / 2 / Data.ANGLE_180 * Math.PI), (diameter / 2 - Data.stirrup_diameter / 2 - Data.longitudinal_rebar_diameter / 2) * Math.Sin(angle / 2 / Data.ANGLE_180 * Math.PI), foundation_height / 2 + column_height - length / 2);
            down_left = app.Point3dFromXYZ(-(diameter / 2 - Data.stirrup_diameter / 2 - Data.longitudinal_rebar_diameter / 2) * Math.Cos(angle / 2 / Data.ANGLE_180 * Math.PI), -(diameter / 2 - Data.stirrup_diameter / 2 - Data.longitudinal_rebar_diameter / 2) * Math.Sin(angle / 2 / Data.ANGLE_180 * Math.PI), foundation_height / 2 + column_height - length / 2);
            down_right = app.Point3dFromXYZ((diameter / 2 - Data.stirrup_diameter / 2 - Data.longitudinal_rebar_diameter / 2) * Math.Cos(angle / 2 / Data.ANGLE_180 * Math.PI), -(diameter / 2 - Data.stirrup_diameter / 2 - Data.longitudinal_rebar_diameter / 2) * Math.Sin(angle / 2 / Data.ANGLE_180 * Math.PI), foundation_height / 2 + column_height - length / 2);
            Element longitudinal_up_left = create_single_longitudinal_bar(length, bending_length, (Data.ANGLE_180 - angle) / 2 + Data.ANGLE_180);
            longitudinal_up_left.Move(up_left);
            Element longitudinal_up_right = create_single_longitudinal_bar(length, bending_length, (Data.ANGLE_180 - angle) / 2 + angle * 2);
            longitudinal_up_right.Move(up_right);
            Element longitudinal_down_right = create_single_longitudinal_bar(length, bending_length, (Data.ANGLE_180 - angle) / 2);
            longitudinal_down_right.Move(down_right);
            Element longitudinal_down_left = create_single_longitudinal_bar(length, bending_length, - (Data.ANGLE_180 - angle) / 2);
            longitudinal_down_left.Move(down_left);
            Element x_longitudinal = app.SmartSolid.SolidUnion(longitudinal_up_left.AsSmartSolidElement, longitudinal_up_right.AsSmartSolidElement);
            x_longitudinal = app.SmartSolid.SolidUnion(x_longitudinal.AsSmartSolidElement, longitudinal_down_right.AsSmartSolidElement);
            x_longitudinal = app.SmartSolid.SolidUnion(x_longitudinal.AsSmartSolidElement, longitudinal_down_left.AsSmartSolidElement);
            Element y_longitudinal = app.SmartSolid.SolidUnion(longitudinal_up_left.AsSmartSolidElement, longitudinal_up_right.AsSmartSolidElement);
            y_longitudinal = app.SmartSolid.SolidUnion(x_longitudinal.AsSmartSolidElement, longitudinal_down_right.AsSmartSolidElement);
            y_longitudinal = app.SmartSolid.SolidUnion(x_longitudinal.AsSmartSolidElement, longitudinal_down_left.AsSmartSolidElement);
            y_longitudinal.Transform(app.Transform3dFromMatrix3d(app.Matrix3dFromRotationBetweenVectors(app.Point3dFromXY(1, 0), app.Point3dFromXY(0, 1))));

            Element xz_up_left = create_single_longitudinal_bar(length, bending_length, Data.ANGLE_225);
            xz_up_left.Move(app.Point3dFromXYZ(-xzlength / 2, xzlength / 2, foundation_height / 2 + column_height - length / 2));

            Element xz_up_right = create_single_longitudinal_bar(length, bending_length, Data.ANGLE_135);
            xz_up_right.Move(app.Point3dFromXYZ(xzlength / 2, xzlength / 2, foundation_height / 2 + column_height - length / 2));

            Element xz_down_right = create_single_longitudinal_bar(length, bending_length, Data.ANGLE_45);
            xz_down_right.Move(app.Point3dFromXYZ(xzlength / 2, -xzlength / 2, foundation_height / 2 + column_height - length / 2));

            Element xz_down_left = create_single_longitudinal_bar(length, bending_length, - Data.ANGLE_45);
            xz_down_left.Move(app.Point3dFromXYZ(-xzlength / 2, -xzlength / 2, foundation_height / 2 + column_height - length / 2));

            Element xz_longitudinal = app.SmartSolid.SolidUnion(xz_up_left.AsSmartSolidElement, xz_up_right.AsSmartSolidElement);
            xz_longitudinal = app.SmartSolid.SolidUnion(xz_longitudinal.AsSmartSolidElement, xz_down_right.AsSmartSolidElement);
            xz_longitudinal = app.SmartSolid.SolidUnion(xz_longitudinal.AsSmartSolidElement, xz_down_left.AsSmartSolidElement);

            Element ret = app.SmartSolid.SolidUnion(x_longitudinal.AsSmartSolidElement, y_longitudinal.AsSmartSolidElement);
            ret = app.SmartSolid.SolidUnion(ret.AsSmartSolidElement, xz_longitudinal.AsSmartSolidElement);
            return ret;
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
