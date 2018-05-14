using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Bentley.Interop.MicroStationDGN;
using Bentley.Interop.TFCom;
using Bentley.MicroStation.InteropServices;

namespace Diji_CS.Utils
{
    class FootingSlabBarUtil
    {
        private static Bentley.Interop.MicroStationDGN.Application app = Utilities.ComApp;
        //画基础配筋，参数分别为基础的长、宽、高
        public static Element create_foundation_rebars(double length, double width, double height)
        {
            //x向钢筋，y向钢筋，总钢筋
            Element x_up_rebars = null, y_up_rebars = null, x_down_rebars = null, y_down_rebars = null, rebars;
            //定义坐标转换
            Matrix3d matrix3d = app.Matrix3dFromRotationBetweenVectors(app.Point3dFromXYZ(0, 0, 1), app.Point3dFromXYZ(1, 0, 0));
            Transform3d transform = app.Transform3dFromMatrix3d(matrix3d);
            //画x向上部配筋
            int x_up_rebar_num = (int)Math.Ceiling((width - Data.up_side_protective_layer_thinckness * 2 - Data.x_up_rebar_diameter) / Data.x_up_rebar_spacing) + 1;
            double real_x_up_rebar_spacing = (width - Data.up_side_protective_layer_thinckness * 2 - Data.x_up_rebar_diameter) / (x_up_rebar_num - 1);
            for (int i = 0; i < x_up_rebar_num; i++)
            {
                Element x_up_rebar = app.SmartSolid.CreateCylinder(null, Data.x_up_rebar_diameter / 2, length - Data.up_side_protective_layer_thinckness * 2);
                x_up_rebar.Transform(transform);
                Point3d position = app.Point3dFromXYZ(0, -width / 2 + Data.up_side_protective_layer_thinckness + Data.x_up_rebar_diameter / 2 + real_x_up_rebar_spacing * i, height / 2 - Data.up_side_protective_layer_thinckness - Data.x_up_rebar_diameter / 2);
                x_up_rebar.Move(ref position);
                if (i == 0)
                {
                    x_up_rebars = x_up_rebar;
                }
                else
                {
                    x_up_rebars = app.SmartSolid.SolidUnion(x_up_rebars.AsSmartSolidElement, x_up_rebar.AsSmartSolidElement);
                }
            }
            //画x向下部配筋
            int x_down_rebar_num = (int)Math.Ceiling((width - Data.up_side_protective_layer_thinckness * 2 - Data.x_down_rebar_diameter) / Data.x_down_rebar_spacing) + 1;
            double real_x_down_rebar_spacing = (width - Data.up_side_protective_layer_thinckness * 2 - Data.x_down_rebar_diameter) / (x_down_rebar_num - 1);
            for (int i = 0; i < x_down_rebar_num; i++)
            {
                Element x_down_rebar = app.SmartSolid.CreateCylinder(null, Data.x_down_rebar_diameter / 2, length - Data.up_side_protective_layer_thinckness * 2);
                x_down_rebar.Transform(transform);
                Point3d position = app.Point3dFromXYZ(0, -width / 2 + Data.up_side_protective_layer_thinckness + Data.x_up_rebar_diameter / 2 + real_x_up_rebar_spacing * i, -height / 2 + Data.down_protective_layer_thinckness + Data.x_up_rebar_diameter / 2);
                x_down_rebar.Move(ref position);
                if (i == 0)
                {
                    x_down_rebars = x_down_rebar;
                }
                else
                {
                    x_down_rebars = app.SmartSolid.SolidUnion(x_down_rebars.AsSmartSolidElement, x_down_rebar.AsSmartSolidElement);
                }
            }
            matrix3d = app.Matrix3dFromRotationBetweenVectors(app.Point3dFromXYZ(0, 0, 1), app.Point3dFromXYZ(0, 1, 0));
            transform = app.Transform3dFromMatrix3d(matrix3d);
            //画y向上部配筋
            int y_up_rebar_num = (int)Math.Ceiling((length - Data.up_side_protective_layer_thinckness * 2 - Data.y_up_rebar_diameter) / Data.y_up_rebar_spacing) + 1;
            double real_y_up_rebar_spacing = (length - Data.up_side_protective_layer_thinckness * 2 - Data.y_up_rebar_diameter) / (y_up_rebar_num - 1);
            for (int i = 0; i < y_up_rebar_num; i++)
            {
                Element y_up_rebar = app.SmartSolid.CreateCylinder(null, Data.y_up_rebar_diameter / 2, width - Data.up_side_protective_layer_thinckness * 2);
                y_up_rebar.Transform(transform);
                Point3d position = app.Point3dFromXYZ(-length / 2 + Data.up_side_protective_layer_thinckness + Data.y_up_rebar_diameter / 2 + real_y_up_rebar_spacing * i, 0, height / 2 - Data.up_side_protective_layer_thinckness - Data.y_up_rebar_diameter / 2);
                y_up_rebar.Move(ref position);
                if (i == 0)
                {
                    y_up_rebars = y_up_rebar;
                }
                else
                {
                    y_up_rebars = app.SmartSolid.SolidUnion(y_up_rebars.AsSmartSolidElement, y_up_rebar.AsSmartSolidElement);
                }
            }
            //画y向下部配筋
            int y_down_rebar_num = (int)Math.Ceiling((length - Data.up_side_protective_layer_thinckness * 2 - Data.y_down_rebar_diameter) / Data.y_down_rebar_spacing) + 1;
            double real_y_down_rebar_spacing = (length - Data.up_side_protective_layer_thinckness * 2 - Data.y_down_rebar_diameter) / (y_down_rebar_num - 1);
            for (int i = 0; i < y_down_rebar_num; i++)
            {
                Element y_down_rebar = app.SmartSolid.CreateCylinder(null, Data.y_up_rebar_diameter / 2, width - Data.up_side_protective_layer_thinckness * 2);
                y_down_rebar.Transform(transform);
                Point3d position = app.Point3dFromXYZ(-length / 2 + Data.up_side_protective_layer_thinckness + Data.y_up_rebar_diameter / 2 + real_y_down_rebar_spacing * i, 0, -height / 2 + Data.down_protective_layer_thinckness + Data.y_up_rebar_diameter / 2);
                y_down_rebar.Move(ref position);
                if (i == 0)
                {
                    y_down_rebars = y_down_rebar;
                }
                else
                {
                    y_down_rebars = app.SmartSolid.SolidUnion(y_down_rebars.AsSmartSolidElement, y_down_rebar.AsSmartSolidElement);
                }
            }
            double move_distance = Data.x_up_rebar_diameter / 2 + Data.y_up_rebar_diameter / 2 + 1;
            if (length > width)
            {
                x_up_rebars.Move(app.Point3dFromXYZ(0, 0, -move_distance));
                x_down_rebars.Move(app.Point3dFromXYZ(0, 0, move_distance));
            }
            else
            {
                y_up_rebars.Move(app.Point3dFromXYZ(0, 0, -move_distance));
                y_down_rebars.Move(app.Point3dFromXYZ(0, 0, move_distance));
            }
            rebars = app.SmartSolid.SolidUnion(x_up_rebars.AsSmartSolidElement, y_up_rebars.AsSmartSolidElement);
            rebars = app.SmartSolid.SolidUnion(rebars.AsSmartSolidElement, y_down_rebars.AsSmartSolidElement);
            rebars = app.SmartSolid.SolidUnion(rebars.AsSmartSolidElement, x_down_rebars.AsSmartSolidElement);
            return rebars;
        }

    }
}
