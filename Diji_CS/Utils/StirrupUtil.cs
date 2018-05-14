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

        //画柱箍筋，参数分别为柱子的长、宽、高和基础高度
        public static Element create_column_stirrups(double length, double width, double height, double foundation_height, string type)
        {
            Element stirrups = null;

            int n = (int)Math.Ceiling((height - Data.stirrup_diameter - 50) / Data.stirrup_spacing) + 1;
            double real_stirrup_spacing = (height - Data.stirrup_diameter - 50) / (n - 1);
            //柱箍筋
            Element single_stirrup = null;
            for (int i = 0; i < n; i++)
            {
                switch (type)
                {
                    case "1":
                        single_stirrup = StirrupUtil.create_stirrup_type1(length - Data.protective_layer_thinckness - Data.stirrup_diameter - Data.longitudinal_rebar_diameter / 2, width - Data.protective_layer_thinckness - Data.stirrup_diameter - Data.longitudinal_rebar_diameter / 2, 3, 3);
                        break;
                    case "2":
                        single_stirrup = StirrupUtil.create_stirrup_type2(length - Data.protective_layer_thinckness - Data.stirrup_diameter - Data.longitudinal_rebar_diameter / 2, width - Data.protective_layer_thinckness - Data.stirrup_diameter - Data.longitudinal_rebar_diameter / 2);
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
            Element stirrup = StirrupUtil.create_stirrup_type2(length - Data.protective_layer_thinckness - Data.stirrup_diameter - Data.longitudinal_rebar_diameter / 2, width - Data.protective_layer_thinckness - Data.stirrup_diameter - Data.longitudinal_rebar_diameter / 2);
            stirrup.Move(app.Point3dFromXYZ(0, 0, foundation_height / 2 - 100));
            stirrups = app.SmartSolid.SolidUnion(stirrups.AsSmartSolidElement, stirrup.AsSmartSolidElement);
            stirrup = StirrupUtil.create_stirrup_type2(length - Data.protective_layer_thinckness - Data.stirrup_diameter - Data.longitudinal_rebar_diameter / 2, width - Data.protective_layer_thinckness - Data.stirrup_diameter - Data.longitudinal_rebar_diameter / 2);
            stirrup.Move(app.Point3dFromXYZ(0, 0, -foundation_height / 2 + Data.down_protective_layer_thinckness + Data.x_down_rebar_diameter + Data.y_down_rebar_diameter + 1 + Data.longitudinal_rebar_diameter / 2 + Data.anchor_bending_rebar_radius + Data.longitudinal_rebar_diameter));
            stirrups = app.SmartSolid.SolidUnion(stirrups.AsSmartSolidElement, stirrup.AsSmartSolidElement);
            return stirrups;
        }
        public static Element create_stirrup_type1(double b, double h, int m, int n)
        {
            Data.bending_length = Data.stirrup_diameter * Data.STIRRUPMUTIPLE > 75 ? Data.stirrup_diameter * Data.STIRRUPMUTIPLE : 75;   //箍筋平直段长度
            Element type2 = create_stirrup_type2(b, h);
            Element y_stirrups = null;
            //m表示y向箍筋
            switch (m)
            {
                case 3:
                    {
                        Element y_stirrup = create_single_stirrup(h);
                        y_stirrups = y_stirrup;
                        break;
                    }
                case 4:
                    {
                        Element y_stirrup = create_closed_stirrup(b / 3, h);
                        y_stirrup.Transform(app.Transform3dFromLineAndRotationAngle(app.Point3dFromXYZ(0, 0, 0), app.Point3dFromXYZ(1, 0, 0), Math.PI));
                        y_stirrups = y_stirrup;
                        break;
                    }
                case 5:
                    {
                        Element y_stirrup1 = create_single_stirrup(h);
                        y_stirrup1.Move(app.Point3dFromXY(- b / 4, 0));
                        Element y_stirrup2 = create_closed_stirrup(b / 4, h);
                        y_stirrup2.Transform(app.Transform3dFromLineAndRotationAngle(app.Point3dFromXYZ(0, 0, 0), app.Point3dFromXYZ(1, 0, 0), Math.PI));
                        y_stirrup2.Move(app.Point3dFromXY(b / 4 / 2, 0));
                        y_stirrups = app.SmartSolid.SolidUnion(y_stirrup1.AsSmartSolidElement, y_stirrup2.AsSmartSolidElement);
                        break;
                    }
                case 6:
                    {
                        Element y_stirrup1 = create_closed_stirrup(b / 5, h);
                        y_stirrup1.Move(app.Point3dFromXY( - b / 5, 0));
                        Element y_stirrup2 = create_closed_stirrup(b / 5, h);
                        y_stirrup2.Transform(app.Transform3dFromLineAndRotationAngle(app.Point3dFromXYZ(0, 0, 0), app.Point3dFromXYZ(1, 0, 0),Math.PI));
                        y_stirrup2.Move(app.Point3dFromXY(b / 5, 0));
                        y_stirrups = app.SmartSolid.SolidUnion(y_stirrup1.AsSmartSolidElement, y_stirrup2.AsSmartSolidElement);
                        break;    
                    }
                case 7:
                    {
                        Element y_stirrup1 = create_closed_stirrup(b / 6, h);
                        y_stirrup1.Transform(app.Transform3dFromLineAndRotationAngle(app.Point3dFromXY(1, 0), app.Point3dFromXY(0, 0), Math.PI));
                        y_stirrup1.Move(app.Point3dFromXY( - b / 6 * 1.5, 0));
                        Element y_stirrup2 = create_single_stirrup(h);
                        y_stirrup2 = create_single_stirrup(h);
                        Element y_stirrup3 = create_closed_stirrup(b / 6, h);
                        y_stirrup3.Transform(app.Transform3dFromLineAndRotationAngle(app.Point3dFromXY(1, 0), app.Point3dFromXY(0, 0), Math.PI));
                        y_stirrup3.Move(app.Point3dFromXY(b / 6 * 1.5, 0));
                        y_stirrups = app.SmartSolid.SolidUnion(y_stirrup1.AsSmartSolidElement, y_stirrup2.AsSmartSolidElement);
                        y_stirrups = app.SmartSolid.SolidUnion(y_stirrups.AsSmartSolidElement, y_stirrup3.AsSmartSolidElement);
                        break;
                    }
                case 8:
                    {
                        Element y_stirrup1 = create_closed_stirrup(b / 7, h);
                        y_stirrup1.Transform(app.Transform3dFromLineAndRotationAngle(app.Point3dFromXY(0, 0), app.Point3dFromXY(1, 0), Math.PI));
                        y_stirrup1.Move(app.Point3dFromXY(-b / 7 * 2, 0));
                        Element y_stirrup2 = create_closed_stirrup(b / 7, h);
                        Element y_stirrup3 = create_closed_stirrup(b / 7 , h);
                        y_stirrup3.Transform(app.Transform3dFromLineAndRotationAngle(app.Point3dFromXY(0, 0), app.Point3dFromXY(1, 0), Math.PI));
                        y_stirrup3.Move(app.Point3dFromXY(b / 7 * 2, 0));
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
                        Element x_stirrup = create_single_stirrup(b);
                        x_stirrup.Transform(app.Transform3dFromMatrix3d(app.Matrix3dFromRotationBetweenVectors(app.Point3dFromXY(0, 1), app.Point3dFromXY(1, 0))));
                        x_stirrups = x_stirrup;
                        break;
                    }
                case 4:
                    {
                        Element x_stirrup = create_closed_stirrup(b, h / 3);
                        x_stirrup.Transform(app.Transform3dFromLineAndRotationAngle(app.Point3dFromXYZ(0, 0, 0), app.Point3dFromXYZ(0, 0, 1), Math.PI));
                        x_stirrups = x_stirrup;
                        break;
                    }
                case 5:
                    {
                        Element x_stirrup1 = create_single_stirrup(b);
                        x_stirrup1.Transform(app.Transform3dFromMatrix3d(app.Matrix3dFromRotationBetweenVectors(app.Point3dFromXY(0, 1), app.Point3dFromXY(1, 0))));
                        x_stirrup1.Move(app.Point3dFromXY(0, h / 4));
                        Element x_stirrup2 = create_closed_stirrup(b, h / 4);
                        x_stirrup2.Transform(app.Transform3dFromLineAndRotationAngle(app.Point3dFromXYZ(0, 0, 0), app.Point3dFromXYZ(0, 0, 1), Math.PI));
                        x_stirrup2.Move(app.Point3dFromXY(0, - h / 4 / 2));
                        x_stirrups = app.SmartSolid.SolidUnion(x_stirrup1.AsSmartSolidElement, x_stirrup2.AsSmartSolidElement);
                        break;
                    }
                case 6:
                    {
                        Element x_stirrup1 = create_closed_stirrup(b, h / 5);
                        x_stirrup1.Transform(app.Transform3dFromMatrix3d(app.Matrix3dFromRotationBetweenVectors(app.Point3dFromXY(1, 0), app.Point3dFromXY(-1, 0))));
                        x_stirrup1.Move(app.Point3dFromXY(0, h / 5));
                        Element x_stirrup2 = create_closed_stirrup(b, h / 5);
                        x_stirrup2.Transform(app.Transform3dFromMatrix3d(app.Matrix3dFromRotationBetweenVectors(app.Point3dFromXY(1, 0), app.Point3dFromXY(-1, 0))));
                        x_stirrup2.Move(app.Point3dFromXY(0, - h / 5));
                        x_stirrups = app.SmartSolid.SolidUnion(x_stirrup1.AsSmartSolidElement, x_stirrup2.AsSmartSolidElement);
                        break;
                    }
                case 7:
                    {
                        Element x_stirrup1 = create_closed_stirrup(b, h / 6);
                        x_stirrup1.Transform(app.Transform3dFromMatrix3d(app.Matrix3dFromRotationBetweenVectors(app.Point3dFromXY(1, 0), app.Point3dFromXY(-1, 0))));
                        x_stirrup1.Move(app.Point3dFromXY(0, h / 6 * 1.5));

                        Element x_stirrup2 = create_single_stirrup(b);
                        x_stirrup2.Transform(app.Transform3dFromMatrix3d(app.Matrix3dFromRotationBetweenVectors(app.Point3dFromXY(0, 1), app.Point3dFromXY(-1, 0))));
                        
                        Element x_stirrup3 = create_closed_stirrup(b, h / 6);
                        x_stirrup3.Transform(app.Transform3dFromMatrix3d(app.Matrix3dFromRotationBetweenVectors(app.Point3dFromXY(1, 0), app.Point3dFromXY(-1, 0))));
                        x_stirrup3.Move(app.Point3dFromXY(0, - h / 6 * 1.5));
                        x_stirrups = app.SmartSolid.SolidUnion(x_stirrup1.AsSmartSolidElement, x_stirrup2.AsSmartSolidElement);
                        x_stirrups = app.SmartSolid.SolidUnion(x_stirrups.AsSmartSolidElement, x_stirrup3.AsSmartSolidElement);
                        break;
                    }

                case 8:
                    {
                        Element x_stirrup1 = create_closed_stirrup(b, h / 7);
                        x_stirrup1.Transform(app.Transform3dFromMatrix3d(app.Matrix3dFromRotationBetweenVectors(app.Point3dFromXY(1, 0), app.Point3dFromXY(-1, 0))));
                        x_stirrup1.Move(app.Point3dFromXY(0, b / 7 * 2));
                        Element x_stirrup2 = create_closed_stirrup(b, h / 7);
                        x_stirrup2.Transform(app.Transform3dFromLineAndRotationAngle(app.Point3dFromXY(0, 0), app.Point3dFromXY(1, 0), Math.PI));
                        Element x_stirrup3 = create_closed_stirrup(b, h / 7);
                        x_stirrup3.Transform(app.Transform3dFromMatrix3d(app.Matrix3dFromRotationBetweenVectors(app.Point3dFromXY(1, 0), app.Point3dFromXY(-1, 0))));
                        x_stirrup3.Move(app.Point3dFromXY(0, - b / 7 * 2));
                        x_stirrups = app.SmartSolid.SolidUnion(x_stirrup1.AsSmartSolidElement, x_stirrup2.AsSmartSolidElement);
                        x_stirrups = app.SmartSolid.SolidUnion(x_stirrups.AsSmartSolidElement, x_stirrup3.AsSmartSolidElement);
                        break;
                    }
                    
            }
            type2 = app.SmartSolid.SolidUnion(type2.AsSmartSolidElement, x_stirrups.AsSmartSolidElement);
            return type2;
        }
        public static Element create_stirrup_type2(double b, double h)
        {
            Element single_stirrup = create_closed_stirrup(b, h);
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
        public static Element create_stirrup_type6(double d)
        {

            return null;
        }
        public static Element create_stirrup_type7(double d)
        {
            return null;
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
    }
}
