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
    class StirrupUtil
    {
        private static Bentley.Interop.MicroStationDGN.Application app = Utilities.ComApp;

        //画柱箍筋，参数分别为柱子的长、宽、高和基础高度、类型
        public static Element create_column_stirrups(double b, double h, double l, double foundation_height, string type)
        {
            Element stirrups = null;

            int n = (int)Math.Ceiling((l - Data.stirrup_diameter - 50) / Data.stirrup_spacing) + 1;
            double real_stirrup_spacing = (l - Data.stirrup_diameter - 50) / (n - 1);
            //柱箍筋
            Element single_stirrup = null;
            for (int i = 0; i < n; i++)
            {
                switch (type)
                {
                    case "1":
                        single_stirrup = StirrupUtil.create_stirrup_type1(b - Data.protective_layer_thinckness * 2 - Data.stirrup_diameter * 2 - Data.longitudinal_rebar_diameter, h - Data.protective_layer_thinckness * 2 - Data.stirrup_diameter * 2 - Data.longitudinal_rebar_diameter, 8, 8);
                        break;
                    case "2":
                        single_stirrup = StirrupUtil.create_stirrup_type2(b - Data.protective_layer_thinckness * 2 - Data.stirrup_diameter * 2 - Data.longitudinal_rebar_diameter, h - Data.protective_layer_thinckness * 2 - Data.stirrup_diameter * 2 - Data.longitudinal_rebar_diameter);
                        break;
                }
                single_stirrup.Move(app.Point3dFromXYZ(0, 0, foundation_height / 2 + Data.stirrup_diameter / 2 + 50 + i * real_stirrup_spacing));
                if (i == 0)
                {
                    stirrups = single_stirrup;
                }
                else
                {
                    stirrups = app.SmartSolid.SolidUnion(stirrups.AsSmartSolidElement, single_stirrup.AsSmartSolidElement);
                }
            }
            //伸入基础箍筋
            Element stirrup = null;
            stirrup = StirrupUtil.create_stirrup_type2(b - Data.protective_layer_thinckness * 2 - Data.stirrup_diameter * 2 - Data.longitudinal_rebar_diameter, h - Data.protective_layer_thinckness * 2 - Data.stirrup_diameter * 2 - Data.longitudinal_rebar_diameter);
            stirrup.Move(app.Point3dFromXYZ(0, 0, foundation_height / 2 - 100));
            stirrups = app.SmartSolid.SolidUnion(stirrups.AsSmartSolidElement, stirrup.AsSmartSolidElement);
            stirrup = StirrupUtil.create_stirrup_type2(b - Data.protective_layer_thinckness * 2 - Data.stirrup_diameter * 2 - Data.longitudinal_rebar_diameter, h - Data.protective_layer_thinckness * 2 - Data.stirrup_diameter * 2 - Data.longitudinal_rebar_diameter);
            stirrup.Move(app.Point3dFromXYZ(0, 0, -foundation_height / 2 + Data.down_protective_layer_thinckness + Data.x_down_rebar_diameter + Data.y_down_rebar_diameter + 1 + Data.longitudinal_rebar_diameter / 2 + Data.anchor_bending_rebar_radius + Data.longitudinal_rebar_diameter));
            stirrups = app.SmartSolid.SolidUnion(stirrups.AsSmartSolidElement, stirrup.AsSmartSolidElement);
            return stirrups;
        }
        //画柱箍筋，参数分别为柱子的直径、高和基础高度、类型
        public static Element create_column_stirrups(double d, double l, double foundation_height, string type)
        {
            Element stirrups = null;
            int n = (int)Math.Ceiling((l - Data.stirrup_diameter - 50) / Data.stirrup_spacing) + 1;
            double real_stirrup_spacing = (l - Data.stirrup_diameter - 50) / (n - 1);
            //柱箍筋
            Element single_stirrup = null;
            double angle_r = 0, diameter = 0, xzlength = 0, length = 0, angle = 0;

            for (int i = 0; i < n; i++)
            {
                switch (type)
                {
                    case "6":
                        angle_r = Data.lap_length / (Math.PI * d) * Data.ANGLE_360 / 2;
                        single_stirrup = StirrupUtil.create_stirrup_type6(d - Data.protective_layer_thinckness * 2 - Data.stirrup_diameter , angle_r);
                        break;
                    case "7":
                        diameter = d - Data.protective_layer_thinckness * 2 - Data.stirrup_diameter;
                        xzlength = d / 3 >= 250 ? d / 3 : 250;
                        angle_r = Math.Acos(xzlength / 2 / (diameter / 2 - Data.stirrup_diameter / 2 - Data.longitudinal_rebar_diameter / 2));
                        length = (diameter / 2 - Data.stirrup_diameter / 2 - Data.longitudinal_rebar_diameter / 2) * Math.Sin(angle_r) * 2;
                        angle = Data.ANGLE_180 - angle_r / Math.PI * Data.ANGLE_180 * 2;
                        single_stirrup = StirrupUtil.create_stirrup_type7(length, xzlength, diameter, angle);
                        break;
                        
                }
                single_stirrup.Move(app.Point3dFromXYZ(0, 0, foundation_height / 2 + Data.stirrup_diameter / 2 + 50 + i * real_stirrup_spacing));
                if (i == 0)
                {
                    stirrups = single_stirrup;
                }
                else
                {
                    stirrups = app.SmartSolid.SolidUnion(stirrups.AsSmartSolidElement, single_stirrup.AsSmartSolidElement);
                }
            }
            //伸入基础箍筋
            Element stirrup = null;
            switch (type)
            {
                case "6":
                    stirrup = StirrupUtil.create_stirrup_type6(d - Data.protective_layer_thinckness * 2 - Data.stirrup_diameter, angle_r);
                    stirrup.Move(app.Point3dFromXYZ(0, 0, foundation_height / 2 - 100));
                    stirrups = app.SmartSolid.SolidUnion(stirrups.AsSmartSolidElement, stirrup.AsSmartSolidElement);
                    stirrup = StirrupUtil.create_stirrup_type6(d - Data.protective_layer_thinckness * 2 - Data.stirrup_diameter, angle_r);
                    stirrup.Move(app.Point3dFromXYZ(0, 0, -foundation_height / 2 + Data.down_protective_layer_thinckness + Data.x_down_rebar_diameter + Data.y_down_rebar_diameter + 1 + Data.longitudinal_rebar_diameter / 2 + Data.anchor_bending_rebar_radius + Data.longitudinal_rebar_diameter));
                    break;
                case "7":
                    stirrup = StirrupUtil.create_stirrup_type7(length, xzlength, diameter, angle);
                    stirrup.Move(app.Point3dFromXYZ(0, 0, foundation_height / 2 - 100));
                    stirrups = app.SmartSolid.SolidUnion(stirrups.AsSmartSolidElement, stirrup.AsSmartSolidElement);
                    stirrup = StirrupUtil.create_stirrup_type7(length, xzlength, diameter, angle);
                    stirrup.Move(app.Point3dFromXYZ(0, 0, -foundation_height / 2 + Data.down_protective_layer_thinckness + Data.x_down_rebar_diameter + Data.y_down_rebar_diameter + 1 + Data.longitudinal_rebar_diameter / 2 + Data.anchor_bending_rebar_radius + Data.longitudinal_rebar_diameter));
                    break;
                    
            }
            stirrups = app.SmartSolid.SolidUnion(stirrups.AsSmartSolidElement, stirrup.AsSmartSolidElement);
            return stirrups;
        }
        
        public static Element create_stirrup_type1(double length, double width, int m, int n)
        {
            Data.bending_length = Data.stirrup_diameter * Data.STIRRUPMUTIPLE > 75 ? Data.stirrup_diameter * Data.STIRRUPMUTIPLE : 75;   //箍筋平直段长度
            Element type2 = create_stirrup_type2(length, width);
            Element y_stirrups = null;
            //m表示y向箍筋
            switch (m)
            {
                case 3:
                    {
                        Element y_stirrup = create_single_stirrup(width);
                        y_stirrups = y_stirrup;
                        break;
                    }
                case 4:
                    {
                        Element y_stirrup = create_closed_stirrup(length / 3, width);
                        y_stirrup.Transform(app.Transform3dFromLineAndRotationAngle(app.Point3dFromXYZ(0, 0, 0), app.Point3dFromXYZ(1, 0, 0), Math.PI));
                        y_stirrups = y_stirrup;
                        break;
                    }
                case 5:
                    {
                        Element y_stirrup1 = create_single_stirrup(width);
                        y_stirrup1.Move(app.Point3dFromXY(- length / 4, 0));
                        Element y_stirrup2 = create_closed_stirrup(length / 4, width);
                        y_stirrup2.Transform(app.Transform3dFromLineAndRotationAngle(app.Point3dFromXYZ(0, 0, 0), app.Point3dFromXYZ(1, 0, 0), Math.PI));
                        y_stirrup2.Move(app.Point3dFromXY(length / 4 / 2, 0));
                        y_stirrups = app.SmartSolid.SolidUnion(y_stirrup1.AsSmartSolidElement, y_stirrup2.AsSmartSolidElement);
                        break;
                    }
                case 6:
                    {
                        Element y_stirrup1 = create_closed_stirrup(length / 5, width);
                        y_stirrup1.Move(app.Point3dFromXY( - length / 5, 0));
                        Element y_stirrup2 = create_closed_stirrup(length / 5, width);
                        y_stirrup2.Transform(app.Transform3dFromLineAndRotationAngle(app.Point3dFromXYZ(0, 0, 0), app.Point3dFromXYZ(1, 0, 0),Math.PI));
                        y_stirrup2.Move(app.Point3dFromXY(length / 5, 0));
                        y_stirrups = app.SmartSolid.SolidUnion(y_stirrup1.AsSmartSolidElement, y_stirrup2.AsSmartSolidElement);
                        break;    
                    }
                case 7:
                    {
                        Element y_stirrup1 = create_closed_stirrup(length / 6, width);
                        y_stirrup1.Transform(app.Transform3dFromLineAndRotationAngle(app.Point3dFromXY(1, 0), app.Point3dFromXY(0, 0), Math.PI));
                        y_stirrup1.Move(app.Point3dFromXY( - length / 6 * 1.5, 0));
                        Element y_stirrup2 = create_single_stirrup(width);
                        y_stirrup2 = create_single_stirrup(width);
                        Element y_stirrup3 = create_closed_stirrup(length / 6, width);
                        y_stirrup3.Transform(app.Transform3dFromLineAndRotationAngle(app.Point3dFromXY(1, 0), app.Point3dFromXY(0, 0), Math.PI));
                        y_stirrup3.Move(app.Point3dFromXY(length / 6 * 1.5, 0));
                        y_stirrups = app.SmartSolid.SolidUnion(y_stirrup1.AsSmartSolidElement, y_stirrup2.AsSmartSolidElement);
                        y_stirrups = app.SmartSolid.SolidUnion(y_stirrups.AsSmartSolidElement, y_stirrup3.AsSmartSolidElement);
                        break;
                    }
                case 8:
                    {
                        Element y_stirrup1 = create_closed_stirrup(length / 7, width);
                        y_stirrup1.Transform(app.Transform3dFromLineAndRotationAngle(app.Point3dFromXY(0, 0), app.Point3dFromXY(1, 0), Math.PI));
                        y_stirrup1.Move(app.Point3dFromXY(-length / 7 * 2, 0));
                        Element y_stirrup2 = create_closed_stirrup(length / 7, width);
                        Element y_stirrup3 = create_closed_stirrup(length / 7 , width);
                        y_stirrup3.Transform(app.Transform3dFromLineAndRotationAngle(app.Point3dFromXY(0, 0), app.Point3dFromXY(1, 0), Math.PI));
                        y_stirrup3.Move(app.Point3dFromXY(length / 7 * 2, 0));
                        y_stirrups = app.SmartSolid.SolidUnion(y_stirrup1.AsSmartSolidElement, y_stirrup2.AsSmartSolidElement);
                        y_stirrups = app.SmartSolid.SolidUnion(y_stirrups.AsSmartSolidElement, y_stirrup3.AsSmartSolidElement);
                        break;
                    }
                    
            }
            type2 = app.SmartSolid.SolidUnion(type2.AsSmartSolidElement, y_stirrups.AsSmartSolidElement);
            Element x_stirrups = null;
            //n表示x向箍筋
            switch (n)
            {
                case 3:
                    {
                        Element x_stirrup = create_single_stirrup(length);
                        x_stirrup.Transform(app.Transform3dFromMatrix3d(app.Matrix3dFromRotationBetweenVectors(app.Point3dFromXY(0, 1), app.Point3dFromXY(1, 0))));
                        x_stirrups = x_stirrup;
                        break;
                    }
                case 4:
                    {
                        Element x_stirrup = create_closed_stirrup(length, width / 3);
                        x_stirrup.Transform(app.Transform3dFromLineAndRotationAngle(app.Point3dFromXYZ(0, 0, 0), app.Point3dFromXYZ(0, 0, 1), Math.PI));
                        x_stirrups = x_stirrup;
                        break;
                    }
                case 5:
                    {
                        Element x_stirrup1 = create_single_stirrup(length);
                        x_stirrup1.Transform(app.Transform3dFromMatrix3d(app.Matrix3dFromRotationBetweenVectors(app.Point3dFromXY(0, 1), app.Point3dFromXY(1, 0))));
                        x_stirrup1.Move(app.Point3dFromXY(0, width / 4));
                        Element x_stirrup2 = create_closed_stirrup(length, width / 4);
                        x_stirrup2.Transform(app.Transform3dFromLineAndRotationAngle(app.Point3dFromXYZ(0, 0, 0), app.Point3dFromXYZ(0, 0, 1), Math.PI));
                        x_stirrup2.Move(app.Point3dFromXY(0, - width / 4 / 2));
                        x_stirrups = app.SmartSolid.SolidUnion(x_stirrup1.AsSmartSolidElement, x_stirrup2.AsSmartSolidElement);
                        break;
                    }
                case 6:
                    {
                        Element x_stirrup1 = create_closed_stirrup(length, width / 5);
                        x_stirrup1.Transform(app.Transform3dFromMatrix3d(app.Matrix3dFromRotationBetweenVectors(app.Point3dFromXY(1, 0), app.Point3dFromXY(-1, 0))));
                        x_stirrup1.Move(app.Point3dFromXY(0, width / 5));
                        Element x_stirrup2 = create_closed_stirrup(length, width / 5);
                        x_stirrup2.Transform(app.Transform3dFromMatrix3d(app.Matrix3dFromRotationBetweenVectors(app.Point3dFromXY(1, 0), app.Point3dFromXY(-1, 0))));
                        x_stirrup2.Move(app.Point3dFromXY(0, - width / 5));
                        x_stirrups = app.SmartSolid.SolidUnion(x_stirrup1.AsSmartSolidElement, x_stirrup2.AsSmartSolidElement);
                        break;
                    }
                case 7:
                    {
                        Element x_stirrup1 = create_closed_stirrup(length, width / 6);
                        x_stirrup1.Transform(app.Transform3dFromMatrix3d(app.Matrix3dFromRotationBetweenVectors(app.Point3dFromXY(1, 0), app.Point3dFromXY(-1, 0))));
                        x_stirrup1.Move(app.Point3dFromXY(0, width / 6 * 1.5));

                        Element x_stirrup2 = create_single_stirrup(length);
                        x_stirrup2.Transform(app.Transform3dFromMatrix3d(app.Matrix3dFromRotationBetweenVectors(app.Point3dFromXY(0, 1), app.Point3dFromXY(-1, 0))));
                        
                        Element x_stirrup3 = create_closed_stirrup(length, width / 6);
                        x_stirrup3.Transform(app.Transform3dFromMatrix3d(app.Matrix3dFromRotationBetweenVectors(app.Point3dFromXY(1, 0), app.Point3dFromXY(-1, 0))));
                        x_stirrup3.Move(app.Point3dFromXY(0, - width / 6 * 1.5));
                        x_stirrups = app.SmartSolid.SolidUnion(x_stirrup1.AsSmartSolidElement, x_stirrup2.AsSmartSolidElement);
                        x_stirrups = app.SmartSolid.SolidUnion(x_stirrups.AsSmartSolidElement, x_stirrup3.AsSmartSolidElement);
                        break;
                    }

                case 8:
                    {
                        Element x_stirrup1 = create_closed_stirrup(length, width / 7);
                        x_stirrup1.Transform(app.Transform3dFromMatrix3d(app.Matrix3dFromRotationBetweenVectors(app.Point3dFromXY(1, 0), app.Point3dFromXY(-1, 0))));
                        x_stirrup1.Move(app.Point3dFromXY(0, length / 7 * 2));
                        Element x_stirrup2 = create_closed_stirrup(length, width / 7);
                        x_stirrup2.Transform(app.Transform3dFromLineAndRotationAngle(app.Point3dFromXY(0, 0), app.Point3dFromXY(1, 0), Math.PI));
                        Element x_stirrup3 = create_closed_stirrup(length, width / 7);
                        x_stirrup3.Transform(app.Transform3dFromMatrix3d(app.Matrix3dFromRotationBetweenVectors(app.Point3dFromXY(1, 0), app.Point3dFromXY(-1, 0))));
                        x_stirrup3.Move(app.Point3dFromXY(0, - length / 7 * 2));
                        x_stirrups = app.SmartSolid.SolidUnion(x_stirrup1.AsSmartSolidElement, x_stirrup2.AsSmartSolidElement);
                        x_stirrups = app.SmartSolid.SolidUnion(x_stirrups.AsSmartSolidElement, x_stirrup3.AsSmartSolidElement);
                        break;
                    }
                    
            }
            type2 = app.SmartSolid.SolidUnion(type2.AsSmartSolidElement, x_stirrups.AsSmartSolidElement);
            return type2;
        }
        public static Element create_stirrup_type2(double length, double width)
        {
            Element single_stirrup = create_closed_stirrup(length, width);
            return single_stirrup;
        }
        public static Element create_stirrup_type3(double b, double h)
        {
            return null;
        }
        public static Element create_stirrup_type4(double b, double h)
        {
            return null;
        }
        public static Element create_stirrup_type5(double b, double h, int m, int n)
        {
            return null;
        }
        public static Element create_stirrup_type6(double diameter, double angle_r)
        {
            Element element = create_circle_stirrup(diameter, angle_r, 0);
            return element;
        }
        public static Element create_stirrup_type7(double length, double xzlength, double diameter, double angle)
        {
            Element x_stirrup = create_closed_stirrup(length, xzlength, diameter, angle);
            Element y_stirrup = create_closed_stirrup(length, xzlength, diameter, angle);
            y_stirrup.Transform(app.Transform3dFromMatrix3d(app.Matrix3dFromRotationBetweenVectors(app.Point3dFromXY(-1, 0), app.Point3dFromXY(0, -1))));
            Element circle_stirrup = create_circle_stirrup(diameter, angle);
            Element xz_stirrup = create_closed_stirrup(xzlength, xzlength);


            Element ret = app.SmartSolid.SolidUnion(x_stirrup.AsSmartSolidElement, y_stirrup.AsSmartSolidElement);
            ret = app.SmartSolid.SolidUnion(ret.AsSmartSolidElement, circle_stirrup.AsSmartSolidElement);
            ret = app.SmartSolid.SolidUnion(ret.AsSmartSolidElement, xz_stirrup.AsSmartSolidElement);
            return ret;
        }

        public static Element create_single_stirrup(double length)
        {
            //竖直段
            Element element = app.SmartSolid.CreateCylinder(null, Data.stirrup_diameter / 2, length);
            element.Transform(app.Transform3dFromMatrix3d(app.Matrix3dFromRotationBetweenVectors(app.Point3dFromXYZ(0, 0, 1), app.Point3dFromXYZ(0, 1, 0))));
            element.Move(app.Point3dFromXYZ(- Data.stirrup_diameter / 2 - Data.longitudinal_rebar_diameter / 2, 0, 0));
            Element arc = app.SmartSolid.CreateTorus(null, Data.longitudinal_rebar_diameter / 2 + Data.stirrup_diameter / 2, Data.stirrup_diameter/ 2, Data.ANGLE_135);
            //上弯折弧
            arc.Transform(app.Transform3dFromMatrix3d(app.Matrix3dFromRotationBetweenVectors(app.Point3dFromXYZ(1, 0, 0), app.Point3dFromXYZ(1, 1, 0))));
            arc.Move(app.Point3dFromXYZ(0, length / 2, 0));
            element = app.SmartSolid.SolidUnion(element.AsSmartSolidElement, arc.AsSmartSolidElement);
            //上弯折直段
            Element bending_rebar = app.SmartSolid.CreateCylinder(null, Data.stirrup_diameter / 2, Data.bending_length);
            bending_rebar.Transform(app.Transform3dFromMatrix3d(app.Matrix3dFromRotationBetweenVectors(app.Point3dFromXYZ(0, 0, 1), app.Point3dFromXYZ(-1, 1, 0))));
            bending_rebar.Move(app.Point3dFromXYZ(Data.bending_length / 2 / Math.Sqrt(2.0) , - Data.bending_length / 2 / Math.Sqrt(2.0), 0));
            bending_rebar.Move(app.Point3dFromXYZ((Data.longitudinal_rebar_diameter + Data.stirrup_diameter) / 2 / Math.Sqrt(2.0), length / 2 + (Data.longitudinal_rebar_diameter + Data.stirrup_diameter) / 2 / Math.Sqrt(2.0), 0));
            element = app.SmartSolid.SolidUnion(element.AsSmartSolidElement, bending_rebar.AsSmartSolidElement);
            //下弯折弧
            arc = app.SmartSolid.CreateTorus(null, Data.longitudinal_rebar_diameter / 2 + Data.stirrup_diameter / 2, Data.stirrup_diameter / 2, Data.ANGLE_135);
            arc.Transform(app.Transform3dFromMatrix3d(app.Matrix3dFromRotationBetweenVectors(app.Point3dFromXYZ(-1, 1, 0), app.Point3dFromXYZ(1, -1, 0))));
            arc.Move(app.Point3dFromXYZ(0, - length / 2, 0));
            element = app.SmartSolid.SolidUnion(element.AsSmartSolidElement, arc.AsSmartSolidElement);
            //下弯折直段
            bending_rebar = app.SmartSolid.CreateCylinder(null, Data.stirrup_diameter / 2, Data.bending_length);
            bending_rebar.Transform(app.Transform3dFromMatrix3d(app.Matrix3dFromRotationBetweenVectors(app.Point3dFromXYZ(0, 0, 1), app.Point3dFromXYZ(1, 1, 0))));
            bending_rebar.Move(app.Point3dFromXYZ(Data.bending_length/ 2 / Math.Sqrt(2.0), Data.bending_length  / 2 / Math.Sqrt(2.0), 0));
            bending_rebar.Move(app.Point3dFromXYZ((Data.longitudinal_rebar_diameter + Data.stirrup_diameter) / 2 / Math.Sqrt(2.0), -length / 2 - (Data.longitudinal_rebar_diameter + Data.stirrup_diameter) / 2 / Math.Sqrt(2.0), 0));
            element = app.SmartSolid.SolidUnion(element.AsSmartSolidElement, bending_rebar.AsSmartSolidElement);
            return element;
        }
        public static Element create_closed_stirrup(double length, double width)
        {
            Element element = null;
            
            //第一段弯折直段钢筋
            Element bending = app.SmartSolid.CreateCylinder(null, Data.stirrup_diameter / 2, Data.bending_length);
            bending.Transform(app.Transform3dFromMatrix3d(app.Matrix3dFromRotationBetweenVectors(app.Point3dFromXYZ(0, 0, 1), app.Point3dFromXYZ(1, 1, 0))));
            bending.Move(app.Point3dFromXY(length/ 2 - Data.bending_length / 2 / Math.Sqrt(2.0), width / 2 - Data.bending_length / 2 / Math.Sqrt(2.0)));
            bending.Move(app.Point3dFromXY(-(Data.longitudinal_rebar_diameter / 2 + Data.stirrup_diameter / 2) / Math.Sqrt(2.0), (Data.longitudinal_rebar_diameter / 2 + Data.stirrup_diameter / 2) / Math.Sqrt(2.0)));

            //右上角第一段弧
            Element up_right = app.SmartSolid.CreateTorus(null, Data.longitudinal_rebar_diameter / 2 + Data.stirrup_diameter / 2, Data.stirrup_diameter/ 2, 135);
            up_right.Move(app.Point3dFromXY(length / 2, width / 2));

            //右侧直段钢筋
            Element right = app.SmartSolid.CreateCylinder(null, Data.stirrup_diameter / 2, width);
            right.Transform(app.Transform3dFromMatrix3d(app.Matrix3dFromRotationBetweenVectors(app.Point3dFromXYZ(0, 0, 1), app.Point3dFromXYZ(0, 1, 0))));
            right.Move(app.Point3dFromXYZ(length / 2 + Data.longitudinal_rebar_diameter / 2 + Data.stirrup_diameter / 2, 0, 0));

            //右下角弧
            Element down_right = app.SmartSolid.CreateTorus(null, Data.longitudinal_rebar_diameter / 2 + Data.stirrup_diameter / 2, Data.stirrup_diameter / 2, 90);
            down_right.Transform(app.Transform3dFromMatrix3d(app.Matrix3dFromRotationBetweenVectors(app.Point3dFromXY(1, 0), app.Point3dFromXY(0, -1))));
            down_right.Move(app.Point3dFromXY(length / 2, -width / 2));

            //下部直段钢筋
            Element down = app.SmartSolid.CreateCylinder(null, Data.stirrup_diameter / 2, length);
            down.Transform(app.Transform3dFromMatrix3d(app.Matrix3dFromRotationBetweenVectors(app.Point3dFromXYZ(0, 0, 1), app.Point3dFromXYZ(1, 0, 0))));
            down.Move(app.Point3dFromXYZ(0, - width / 2 - Data.longitudinal_rebar_diameter / 2 - Data.stirrup_diameter / 2, 0));
            
            //左下角弧
            Element down_left = app.SmartSolid.CreateTorus(null, Data.longitudinal_rebar_diameter / 2 + Data.stirrup_diameter / 2, Data.stirrup_diameter / 2, 90);
            down_left.Transform(app.Transform3dFromMatrix3d(app.Matrix3dFromRotationBetweenVectors(app.Point3dFromXY(1, 0), app.Point3dFromXY(-1, 0))));
            down_left.Move(app.Point3dFromXY(- length / 2, -width / 2));

            //左侧直段钢筋
            Element left = app.SmartSolid.CreateCylinder(null, Data.stirrup_diameter / 2, width);
            left.Transform(app.Transform3dFromMatrix3d(app.Matrix3dFromRotationBetweenVectors(app.Point3dFromXYZ(0, 0, 1), app.Point3dFromXYZ(0, 1, 0))));
            left.Move(app.Point3dFromXYZ(- length / 2 - Data.longitudinal_rebar_diameter / 2 - Data.stirrup_diameter / 2, 0, 0));

            //左上角弧
            Element up_left = app.SmartSolid.CreateTorus(null, Data.longitudinal_rebar_diameter / 2 + Data.stirrup_diameter / 2, Data.stirrup_diameter / 2, 90);
            up_left.Transform(app.Transform3dFromMatrix3d(app.Matrix3dFromRotationBetweenVectors(app.Point3dFromXY(1, 0),app.Point3dFromXY(0, 1))));
            up_left.Move(app.Point3dFromXY(-length / 2, width / 2));

            //上部直段钢筋
            Element up = app.SmartSolid.CreateCylinder(null, Data.stirrup_diameter / 2, length);
            up.Transform(app.Transform3dFromMatrix3d(app.Matrix3dFromRotationBetweenVectors(app.Point3dFromXYZ(0, 0, 1), app.Point3dFromXYZ(1, 0, 0))));
            up.Move(app.Point3dFromXYZ(0, width / 2 + Data.longitudinal_rebar_diameter / 2 + Data.stirrup_diameter / 2, 0));

            //右上角第二段弧
            Element up_right2 = app.SmartSolid.CreateTorus(null, Data.longitudinal_rebar_diameter / 2 + Data.stirrup_diameter / 2, Data.stirrup_diameter / 2, 135);
            up_right2.Transform(app.Transform3dFromMatrix3d(app.Matrix3dFromRotationBetweenVectors(app.Point3dFromXY(1, 0), app.Point3dFromXY(1, -1))));
            up_right2.Move(app.Point3dFromXY(length / 2, width / 2));

            //第二段弯折直段钢筋
            Element bending2 = app.SmartSolid.CreateCylinder(null, Data.stirrup_diameter / 2, Data.bending_length);
            bending2.Transform(app.Transform3dFromMatrix3d(app.Matrix3dFromRotationBetweenVectors(app.Point3dFromXYZ(0, 0, 1), app.Point3dFromXYZ(1, 1, 0))));
            bending2.Move(app.Point3dFromXY(length / 2 - Data.bending_length / 2 / Math.Sqrt(2.0), width / 2 - Data.bending_length / 2 / Math.Sqrt(2.0)));
            bending2.Move(app.Point3dFromXY((Data.longitudinal_rebar_diameter / 2 + Data.stirrup_diameter / 2) / Math.Sqrt(2.0), -(Data.longitudinal_rebar_diameter / 2 + Data.stirrup_diameter / 2) / Math.Sqrt(2.0)));

            element = app.SmartSolid.SolidUnion(bending.AsSmartSolidElement, up_right.AsSmartSolidElement);
            element = app.SmartSolid.SolidUnion(element.AsSmartSolidElement, right.AsSmartSolidElement);
            element = app.SmartSolid.SolidUnion(element.AsSmartSolidElement, down_right.AsSmartSolidElement);
            element = app.SmartSolid.SolidUnion(element.AsSmartSolidElement, down.AsSmartSolidElement);
            element = app.SmartSolid.SolidUnion(element.AsSmartSolidElement, down_left.AsSmartSolidElement);
            element = app.SmartSolid.SolidUnion(element.AsSmartSolidElement, left.AsSmartSolidElement);
            element = app.SmartSolid.SolidUnion(element.AsSmartSolidElement, up_left.AsSmartSolidElement);
            element = app.SmartSolid.SolidUnion(element.AsSmartSolidElement, up.AsSmartSolidElement);
            element = app.SmartSolid.SolidUnion(element.AsSmartSolidElement, up_right2.AsSmartSolidElement);
            element = app.SmartSolid.SolidUnion(element.AsSmartSolidElement, bending2.AsSmartSolidElement);
            return element;
        }
        //两侧为弧的封闭箍筋, diameter表示箍筋直径，angle表示箍筋弧的角度
        public static Element create_closed_stirrup(double length, double xzlength, double diameter, double angle)
        {
            Point3d down_left, down_right, up_left, up_right;
            up_left = app.Point3dFromXY(-(diameter / 2 - Data.stirrup_diameter / 2 - Data.longitudinal_rebar_diameter / 2) * Math.Cos(angle / 2 / Data.ANGLE_180 * Math.PI), (diameter / 2 - Data.stirrup_diameter / 2 - Data.longitudinal_rebar_diameter / 2) * Math.Sin(angle / 2 / Data.ANGLE_180 * Math.PI));
            up_right = app.Point3dFromXY((diameter / 2 - Data.stirrup_diameter / 2 - Data.longitudinal_rebar_diameter / 2) * Math.Cos(angle / 2 / Data.ANGLE_180 * Math.PI), (diameter / 2 - Data.stirrup_diameter / 2 - Data.longitudinal_rebar_diameter / 2) * Math.Sin(angle / 2 / Data.ANGLE_180 * Math.PI));
            down_left = app.Point3dFromXY(-(diameter / 2 - Data.stirrup_diameter / 2 - Data.longitudinal_rebar_diameter / 2) * Math.Cos(angle / 2 / Data.ANGLE_180 * Math.PI), -(diameter / 2 - Data.stirrup_diameter / 2 - Data.longitudinal_rebar_diameter / 2) * Math.Sin(angle / 2 / Data.ANGLE_180 * Math.PI));
            down_right = app.Point3dFromXY((diameter / 2 - Data.stirrup_diameter / 2 - Data.longitudinal_rebar_diameter / 2) * Math.Cos(angle / 2 / Data.ANGLE_180 * Math.PI), -(diameter / 2 - Data.stirrup_diameter / 2 - Data.longitudinal_rebar_diameter / 2) * Math.Sin(angle / 2 / Data.ANGLE_180 * Math.PI));

            Element down_left_bending = app.SmartSolid.CreateCylinder(null, Data.stirrup_diameter / 2, Data.bending_length);
            down_left_bending.Transform(app.Transform3dFromMatrix3d(app.Matrix3dFromRotationBetweenVectors(app.Point3dFromXYZ(0, 0, 1), app.Point3dFromXYZ(1, 0, 0))));
            down_left_bending.Transform(app.Transform3dFromLineAndRotationAngle(app.Point3dFromXYZ(0, 0, 0), app.Point3dFromXYZ(0, 0, 1), Data.ANGLE_45 / Data.ANGLE_180 * Math.PI));
            down_left_bending.Move(down_left);
            down_left_bending.Move(app.Point3dFromXY(-(Data.longitudinal_rebar_diameter / 2 + Data.stirrup_diameter / 2) / Math.Sqrt(2.0), (Data.longitudinal_rebar_diameter / 2 + Data.stirrup_diameter / 2) / Math.Sqrt(2.0)));
            down_left_bending.Move(app.Point3dFromXY(Data.bending_length / 2 / Math.Sqrt(2.0), Data.bending_length / 2 / Math.Sqrt(2.0)));

            Element down_left_bending1 = app.SmartSolid.CreateCylinder(null, Data.stirrup_diameter / 2, Data.bending_length);
            down_left_bending1.Transform(app.Transform3dFromMatrix3d(app.Matrix3dFromRotationBetweenVectors(app.Point3dFromXYZ(0, 0, 1), app.Point3dFromXYZ(1, 0, 0))));
            down_left_bending1.Transform(app.Transform3dFromLineAndRotationAngle(app.Point3dFromXYZ(0, 0, 0), app.Point3dFromXYZ(0, 0, 1), Data.ANGLE_45 / Data.ANGLE_180 * Math.PI));
            down_left_bending1.Move(down_left);
            down_left_bending1.Move(app.Point3dFromXY((Data.longitudinal_rebar_diameter / 2 + Data.stirrup_diameter / 2) / Math.Sqrt(2.0), -(Data.longitudinal_rebar_diameter / 2 + Data.stirrup_diameter / 2) / Math.Sqrt(2.0)));
            down_left_bending1.Move(app.Point3dFromXY(Data.bending_length / 2 / Math.Sqrt(2.0), Data.bending_length / 2 / Math.Sqrt(2.0)));

            Element down_left_arc = app.SmartSolid.CreateTorus(null, Data.longitudinal_rebar_diameter / 2 + Data.stirrup_diameter / 2, Data.stirrup_diameter / 2, Data.ANGLE_135);
            down_left_arc.Transform(app.Transform3dFromLineAndRotationAngle(app.Point3dFromXYZ(0, 0, 0), app.Point3dFromXYZ(0, 0, 1), Data.ANGLE_135/ Data.ANGLE_180 * Math.PI));
            down_left_arc.Move(down_left);

            Element down_left_arc1 = app.SmartSolid.CreateTorus(null, Data.longitudinal_rebar_diameter / 2 + Data.stirrup_diameter / 2, Data.stirrup_diameter / 2, (Data.ANGLE_180 - angle) / 2 + Data.ANGLE_45);
            down_left_arc1.Transform(app.Transform3dFromLineAndRotationAngle(app.Point3dFromXYZ(0, 0, 0), app.Point3dFromXYZ(0, 0, 1), -((Data.ANGLE_180 - angle) / 2 + Data.ANGLE_90) / Data.ANGLE_180 * Math.PI));
            down_left_arc1.Move(down_left);

            Element down_right_arc = app.SmartSolid.CreateTorus(null, Data.longitudinal_rebar_diameter / 2 + Data.stirrup_diameter / 2, Data.stirrup_diameter / 2, (Data.ANGLE_180 - angle) / 2);
            down_right_arc.Transform(app.Transform3dFromLineAndRotationAngle(app.Point3dFromXYZ(0, 0, 0), app.Point3dFromXYZ(0, 0, 1), -Data.ANGLE_90 / Data.ANGLE_180 * Math.PI));
            down_right_arc.Move(down_right);
            
            Element up_left_arc = app.SmartSolid.CreateTorus(null, Data.longitudinal_rebar_diameter / 2 + Data.stirrup_diameter / 2, Data.stirrup_diameter / 2, (Data.ANGLE_180 - angle) / 2);
            up_left_arc.Transform(app.Transform3dFromLineAndRotationAngle(app.Point3dFromXYZ(0, 0, 0), app.Point3dFromXYZ(0,0,1), Data.ANGLE_90 / Data.ANGLE_180 * Math.PI));
            up_left_arc.Move(up_left);
            
            Element up_right_arc = app.SmartSolid.CreateTorus(null, Data.longitudinal_rebar_diameter / 2 + Data.stirrup_diameter / 2, Data.stirrup_diameter / 2, (Data.ANGLE_180 - angle) / 2);
            up_right_arc.Transform(app.Transform3dFromLineAndRotationAngle(app.Point3dFromXYZ(0, 0, 0), app.Point3dFromXYZ(0, 0, 1), angle / 2 / Data.ANGLE_180 * Math.PI));
            up_right_arc.Move(up_right);

            Element up = app.SmartSolid.CreateCylinder(null, Data.stirrup_diameter / 2, length);
            up.Transform(app.Transform3dFromMatrix3d(app.Matrix3dFromRotationBetweenVectors(app.Point3dFromXYZ(0, 0, 1), app.Point3dFromXYZ(1, 0, 0))));
            up.Move(app.Point3dFromXY(0, xzlength / 2 + Data.longitudinal_rebar_diameter / 2 + Data.stirrup_diameter / 2));
            Element down = app.SmartSolid.CreateCylinder(null, Data.stirrup_diameter / 2, length);
            down.Transform(app.Transform3dFromMatrix3d(app.Matrix3dFromRotationBetweenVectors(app.Point3dFromXYZ(0, 0, 1), app.Point3dFromXYZ(1, 0, 0))));
            down.Move(app.Point3dFromXY(0, -(xzlength / 2 + Data.longitudinal_rebar_diameter / 2 + Data.stirrup_diameter / 2)));
            Element right = app.SmartSolid.CreateTorus(null, diameter / 2, Data.stirrup_diameter / 2, angle);
            right.Transform(app.Transform3dFromLineAndRotationAngle(app.Point3dFromXYZ(0, 0, 0), app.Point3dFromXYZ(0, 0, 1), - angle / 2 / 180 * Math.PI));
            Element left = app.SmartSolid.CreateTorus(null, diameter / 2, Data.stirrup_diameter / 2, angle);
            left.Transform(app.Transform3dFromLineAndRotationAngle(app.Point3dFromXYZ(0, 0, 0), app.Point3dFromXYZ(0, 0, 1), (- angle / 2 + Data.ANGLE_180) / 180 * Math.PI));
            Element ret = app.SmartSolid.SolidUnion(up.AsSmartSolidElement, down.AsSmartSolidElement);
            
            ret = app.SmartSolid.SolidUnion(ret.AsSmartSolidElement, right.AsSmartSolidElement);
            ret = app.SmartSolid.SolidUnion(ret.AsSmartSolidElement, left.AsSmartSolidElement);
            ret = app.SmartSolid.SolidUnion(ret.AsSmartSolidElement, down_left_arc.AsSmartSolidElement);
            ret = app.SmartSolid.SolidUnion(ret.AsSmartSolidElement, down_left_arc1.AsSmartSolidElement);
            ret = app.SmartSolid.SolidUnion(ret.AsSmartSolidElement, down_right_arc.AsSmartSolidElement);
            ret = app.SmartSolid.SolidUnion(ret.AsSmartSolidElement, up_left_arc.AsSmartSolidElement);
            ret = app.SmartSolid.SolidUnion(ret.AsSmartSolidElement, up_right_arc.AsSmartSolidElement);
            ret = app.SmartSolid.SolidUnion(ret.AsSmartSolidElement, down_left_bending.AsSmartSolidElement);
            ret = app.SmartSolid.SolidUnion(ret.AsSmartSolidElement, down_left_bending1.AsSmartSolidElement);
            return ret;
        }
        //类型6外侧圆弧
        public static Element create_circle_stirrup(double diameter, double angle_r, double rotation)
        {
            //大弧
            Element element = app.SmartSolid.CreateTorus(null, diameter / 2, Data.stirrup_diameter / 2, Data.ANGLE_360 + angle_r * 2);
            element.Transform(app.Transform3dFromLineAndRotationAngle(app.Point3dFromXYZ(0, 0, 0), app.Point3dFromXYZ(0, 0, 1), (angle_r + Data.ANGLE_270) / 180 * Math.PI));
            //右侧弯折直段部分
            Element right_bending = app.SmartSolid.CreateCylinder(null, Data.stirrup_diameter / 2, Data.bending_length);
            right_bending.Transform(app.Transform3dFromMatrix3d(app.Matrix3dFromRotationBetweenVectors(app.Point3dFromXYZ(0, 0, 1), app.Point3dFromXYZ(1, 0, 0))));
            right_bending.Transform(app.Transform3dFromLineAndRotationAngle(app.Point3dFromXYZ(0, 0, 0), app.Point3dFromXYZ(0, 0, 1), (Data.ANGLE_135 + angle_r) / 180 * Math.PI));
            right_bending.Move(app.Point3dFromXY((diameter / 2 - Data.stirrup_diameter / 2 - Data.longitudinal_rebar_diameter / 2) * Math.Sin(angle_r / 180 * Math.PI), -(diameter / 2 - Data.stirrup_diameter / 2 - Data.longitudinal_rebar_diameter / 2) * Math.Cos(angle_r / 180 * Math.PI)));
            right_bending.Move(app.Point3dFromXY((Data.longitudinal_rebar_diameter / 2 + Data.stirrup_diameter / 2) * Math.Sin((Data.ANGLE_45 - angle_r) / 180 * Math.PI), (Data.longitudinal_rebar_diameter / 2 + Data.stirrup_diameter / 2) * Math.Cos((Data.ANGLE_45 - angle_r) / 180 * Math.PI)));
            right_bending.Move(app.Point3dFromXY(-Data.bending_length / 2 * Math.Cos((Data.ANGLE_45 - angle_r) / 180 * Math.PI), Data.bending_length / 2 * Math.Sin((Data.ANGLE_45 - angle_r) / 180 * Math.PI)));

            //右侧弯折弧
            Element right_arc = app.SmartSolid.CreateTorus(null, Data.longitudinal_rebar_diameter / 2 + Data.stirrup_diameter / 2, Data.stirrup_diameter / 2, Data.ANGLE_135);
            right_arc.Transform(app.Transform3dFromLineAndRotationAngle(app.Point3dFromXYZ(0, 0, 0), app.Point3dFromXYZ(0, 0, 1), (Data.ANGLE_270 + angle_r) / 180 * Math.PI));
            right_arc.Move(app.Point3dFromXY((diameter / 2 - Data.stirrup_diameter / 2 - Data.longitudinal_rebar_diameter / 2) * Math.Sin(angle_r / 180 * Math.PI), -(diameter / 2 - Data.stirrup_diameter / 2 - Data.longitudinal_rebar_diameter / 2) * Math.Cos(angle_r / 180 * Math.PI)));
            //左侧弯折弧
            Element left_arc = app.SmartSolid.CreateTorus(null, Data.longitudinal_rebar_diameter / 2 + Data.stirrup_diameter / 2, Data.stirrup_diameter / 2, Data.ANGLE_135);
            left_arc.Transform(app.Transform3dFromLineAndRotationAngle(app.Point3dFromXYZ(0, 0, 0), app.Point3dFromXYZ(0, 0, 1), (Data.ANGLE_135 - angle_r) / 180 * Math.PI));
            left_arc.Move(app.Point3dFromXY(-(diameter / 2 - Data.stirrup_diameter / 2 - Data.longitudinal_rebar_diameter / 2) * Math.Sin(angle_r / 180 * Math.PI), -(diameter / 2 - Data.stirrup_diameter / 2 - Data.longitudinal_rebar_diameter / 2) * Math.Cos(angle_r / 180 * Math.PI)));

            //左侧弯折直段部分
            Element left_bending = app.SmartSolid.CreateCylinder(null, Data.stirrup_diameter / 2, Data.bending_length);
            left_bending.Transform(app.Transform3dFromMatrix3d(app.Matrix3dFromRotationBetweenVectors(app.Point3dFromXYZ(0, 0, 1), app.Point3dFromXYZ(1, 0, 0))));
            left_bending.Transform(app.Transform3dFromLineAndRotationAngle(app.Point3dFromXYZ(0, 0, 0), app.Point3dFromXYZ(0, 0, 1), (Data.ANGLE_45 - angle_r) / 180 * Math.PI));
            left_bending.Move(app.Point3dFromXY(-(diameter / 2 - Data.stirrup_diameter / 2 - Data.longitudinal_rebar_diameter / 2) * Math.Sin(angle_r / 180 * Math.PI), -(diameter / 2 - Data.stirrup_diameter / 2 - Data.longitudinal_rebar_diameter / 2) * Math.Cos(angle_r / 180 * Math.PI)));
            left_bending.Move(app.Point3dFromXY(-(Data.longitudinal_rebar_diameter / 2 + Data.stirrup_diameter / 2) * Math.Sin((Data.ANGLE_45 - angle_r) / 180 * Math.PI), (Data.longitudinal_rebar_diameter / 2 + Data.stirrup_diameter / 2) * Math.Cos((Data.ANGLE_45 - angle_r) / 180 * Math.PI)));
            left_bending.Move(app.Point3dFromXY(Data.bending_length / 2 * Math.Cos((Data.ANGLE_45 - angle_r) / 180 * Math.PI), Data.bending_length / 2 * Math.Sin((Data.ANGLE_45 - angle_r) / 180 * Math.PI)));

            element = app.SmartSolid.SolidUnion(element.AsSmartSolidElement, right_bending.AsSmartSolidElement);
            element = app.SmartSolid.SolidUnion(element.AsSmartSolidElement, right_arc.AsSmartSolidElement);
            element = app.SmartSolid.SolidUnion(element.AsSmartSolidElement, left_arc.AsSmartSolidElement);
            element = app.SmartSolid.SolidUnion(element.AsSmartSolidElement, left_bending.AsSmartSolidElement);
            element.Transform(app.Transform3dFromLineAndRotationAngle(app.Point3dFromXYZ(0, 0, 0), app.Point3dFromXYZ(0, 0, 1), rotation));
            return element;
        }
        //类型7外侧圆弧
        public static Element create_circle_stirrup(double diameter, double angle)
        {
            Point3d down_left = app.Point3dFromXY(-(diameter / 2 - Data.stirrup_diameter / 2 - Data.longitudinal_rebar_diameter / 2) * Math.Cos(angle / 2 / Data.ANGLE_180 * Math.PI), -(diameter / 2 - Data.stirrup_diameter / 2 - Data.longitudinal_rebar_diameter / 2) * Math.Sin(angle / 2 / Data.ANGLE_180 * Math.PI));

            Element down_left_bending = app.SmartSolid.CreateCylinder(null, Data.stirrup_diameter / 2, Data.bending_length);
            down_left_bending.Transform(app.Transform3dFromMatrix3d(app.Matrix3dFromRotationBetweenVectors(app.Point3dFromXYZ(0, 0, 1), app.Point3dFromXYZ(1, 0, 0))));
            down_left_bending.Transform(app.Transform3dFromLineAndRotationAngle(app.Point3dFromXYZ(0, 0, 0), app.Point3dFromXYZ(0, 0, 1), Data.ANGLE_45 / Data.ANGLE_180 * Math.PI));
            down_left_bending.Move(down_left);
            down_left_bending.Move(app.Point3dFromXY(-(Data.longitudinal_rebar_diameter / 2 + Data.stirrup_diameter / 2) / Math.Sqrt(2.0), (Data.longitudinal_rebar_diameter / 2 + Data.stirrup_diameter / 2) / Math.Sqrt(2.0)));
            down_left_bending.Move(app.Point3dFromXY(Data.bending_length / 2 / Math.Sqrt(2.0), Data.bending_length / 2 / Math.Sqrt(2.0)));

            Element down_left_bending1 = app.SmartSolid.CreateCylinder(null, Data.stirrup_diameter / 2, Data.bending_length);
            down_left_bending1.Transform(app.Transform3dFromMatrix3d(app.Matrix3dFromRotationBetweenVectors(app.Point3dFromXYZ(0, 0, 1), app.Point3dFromXYZ(1, 0, 0))));
            down_left_bending1.Transform(app.Transform3dFromLineAndRotationAngle(app.Point3dFromXYZ(0, 0, 0), app.Point3dFromXYZ(0, 0, 1), Data.ANGLE_45 / Data.ANGLE_180 * Math.PI));
            down_left_bending1.Move(down_left);
            down_left_bending1.Move(app.Point3dFromXY((Data.longitudinal_rebar_diameter / 2 + Data.stirrup_diameter / 2) / Math.Sqrt(2.0), -(Data.longitudinal_rebar_diameter / 2 + Data.stirrup_diameter / 2) / Math.Sqrt(2.0)));
            down_left_bending1.Move(app.Point3dFromXY(Data.bending_length / 2 / Math.Sqrt(2.0), Data.bending_length / 2 / Math.Sqrt(2.0)));

            Element down_left_arc = app.SmartSolid.CreateTorus(null, Data.longitudinal_rebar_diameter / 2 + Data.stirrup_diameter / 2, Data.stirrup_diameter / 2, Data.ANGLE_135);
            down_left_arc.Transform(app.Transform3dFromLineAndRotationAngle(app.Point3dFromXYZ(0, 0, 0), app.Point3dFromXYZ(0, 0, 1), Data.ANGLE_135 / Data.ANGLE_180 * Math.PI));
            down_left_arc.Move(down_left);

            Element down_left_arc1 = app.SmartSolid.CreateTorus(null, Data.longitudinal_rebar_diameter / 2 + Data.stirrup_diameter / 2, Data.stirrup_diameter / 2, (Data.ANGLE_180 - angle) / 2 + Data.ANGLE_45);
            down_left_arc1.Transform(app.Transform3dFromLineAndRotationAngle(app.Point3dFromXYZ(0, 0, 0), app.Point3dFromXYZ(0, 0, 1), -((Data.ANGLE_180 - angle) / 2 + Data.ANGLE_90) / Data.ANGLE_180 * Math.PI));
            down_left_arc1.Move(down_left);

            Element circle = app.SmartSolid.CreateTorus(null, diameter / 2, Data.stirrup_diameter / 2, Data.ANGLE_360);

            Element ret = app.SmartSolid.SolidUnion(down_left_bending.AsSmartSolidElement, down_left_bending1.AsSmartSolidElement);
            ret = app.SmartSolid.SolidUnion(ret.AsSmartSolidElement, down_left_arc.AsSmartSolidElement);
            ret = app.SmartSolid.SolidUnion(ret.AsSmartSolidElement, down_left_arc1.AsSmartSolidElement);
            ret = app.SmartSolid.SolidUnion(ret.AsSmartSolidElement, circle.AsSmartSolidElement);
            return ret;
        }
    }
}
