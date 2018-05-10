using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Diji_CS.Utils;

using Bentley.Interop.MicroStationDGN;
using Bentley.MicroStation.InteropServices;
using Bentley.Interop.TFCom;


namespace Diji_CS
{
    class RCUtil : IPrimitiveCommandEvents
    {
        //柱配筋的保护层厚度，纵筋直径，箍筋直径, 箍筋间距，弯折长度，受拉钢筋抗震锚固长度lae, 锚固钢筋弯折弧半径
        private double protective_layer_thinckness = 50, longitudinal_rebar_diameter = 22, stirrup_diameter = 10, stirrup_spacing = 100, bending_length = 75, lae = 300, anchor_bending_rebar_radius = 20;
        
        //基础配筋的上面和侧面的保护层厚度、下部的保护层厚度
        private double up_side_protective_layer_thinckness = 30, down_protective_layer_thinckness = 70;
        
        //底板顶部x向钢筋直径、底板顶部x向钢筋间距、底板顶部y向钢筋直径、底板顶部y向钢筋间距
        private double x_up_rebar_diameter = 25, x_up_rebar_spacing = 100, y_up_rebar_diameter = 25, y_up_rebar_spacing = 100;
        
        //底板底部x向钢筋直径、底板底部x向钢筋间距、底板底部y向钢筋直径、底板底部y向钢筋间距
        private double x_down_rebar_diameter = 25, x_down_rebar_spacing = 100, y_down_rebar_diameter = 25, y_down_rebar_spacing = 100; 
        private static RCUtil rc_util = null;
        private Bentley.Interop.MicroStationDGN.Application app = Utilities.ComApp;

        private static int STIRRUPMUTIPLE = 10;

        public static RCUtil create_instance()
        {
            if (rc_util == null)
            {
                rc_util = new RCUtil();
            }
            return rc_util;
        }
        /* 暂时没用
        public RCUtil draw_a_individual_footing_column(Point3d position, MsdDrawingMode draw_mode)
        {
            RCUtil rcUtil = new RCUtil();

            Element column = rcUtil.create_column(400, 400, 1500, 400), rebars = rcUtil.create_foundation_rebars(785, 785, 400), foundation = rcUtil.create_foundation(785, 785, 400);

            TFPartRef tfpart_ref = rcUtil.create_tfpart_ref("Ceiling", "Metal");
            rcUtil.add_part_to_element(ref column, tfpart_ref);
            rcUtil.add_part_to_element(ref foundation, tfpart_ref);
            rcUtil.add_part_to_element(ref rebars, tfpart_ref);

            column.Move(ref position);
            rebars.Move(ref position);
            foundation.Move(ref position);
            if (draw_mode == MsdDrawingMode.Normal)
            {
                rcUtil.draw_Element(column);
                rcUtil.draw_Element(foundation);
                rcUtil.draw_Element(rebars);
            }
            return rcUtil;
        }
        */
        //添加元素
        public void draw_Elements(Element[] elements, string name)
        {
            TFFrameList frame_list = new TFFrameListClass();
            TFFrame frame = frame_list.AsTFFrame;
            for (int i = 0; i < elements.Length; i++)
            {
                frame.Add3DElement(elements[i]);
            }
            frame.SetName(name);
            TFApplicationList tfapp_list = new TFApplicationList();
            TFApplication tfapp = tfapp_list.TFApplication;
            tfapp.ModelReferenceAddFrame(app.ActiveModelReference, frame);
        }

        //画基础
        public Element create_foundation(double length, double width, double height)
        {
            Element foundation = app.SmartSolid.CreateSlab(null, length, width, height);
            return foundation;
        }
        //画长方形柱
        public Element create_column(double length, double width, double height, double foundation_height)
        {
            Element column = app.SmartSolid.CreateSlab(null, length, width, height);
            Point3d position = app.Point3dFromXYZ(0, 0, height / 2 + foundation_height / 2);
            column.Move(position);
            return column;
        }
        //画基础配筋，参数分别为基础的长、宽、高
        public Element create_foundation_rebars(double length, double width, double height)
        {
            //x向钢筋，y向钢筋，总钢筋
            Element x_up_rebars = null, y_up_rebars = null, x_down_rebars = null, y_down_rebars = null, rebars;
            //定义坐标转换
            Matrix3d matrix3d = app.Matrix3dFromRotationBetweenVectors(app.Point3dFromXYZ(0, 0, 1), app.Point3dFromXYZ(1, 0, 0));
            Transform3d transform = app.Transform3dFromMatrix3d(matrix3d);
            //画x向上部配筋
            int x_up_rebar_num = (int)Math.Ceiling((width - up_side_protective_layer_thinckness * 2 - x_up_rebar_diameter) / x_up_rebar_spacing) + 1;
            double real_x_up_rebar_spacing = (width - up_side_protective_layer_thinckness * 2 - x_up_rebar_diameter) / (x_up_rebar_num - 1);
            for (int i = 0; i < x_up_rebar_num; i++)
            {
                Element x_up_rebar = app.SmartSolid.CreateCylinder(null, x_up_rebar_diameter / 2, length - up_side_protective_layer_thinckness * 2);
                x_up_rebar.Transform(transform);
                Point3d position = app.Point3dFromXYZ(0, - width / 2 + up_side_protective_layer_thinckness + x_up_rebar_diameter / 2 + real_x_up_rebar_spacing * i, height / 2 - up_side_protective_layer_thinckness - x_up_rebar_diameter / 2);
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
            int x_down_rebar_num = (int)Math.Ceiling((width - up_side_protective_layer_thinckness * 2 - x_down_rebar_diameter) / x_down_rebar_spacing) + 1;
            double real_x_down_rebar_spacing = (width - up_side_protective_layer_thinckness * 2 - x_down_rebar_diameter) / (x_down_rebar_num - 1);
            for (int i = 0; i < x_down_rebar_num; i++)
            {
                Element x_down_rebar = app.SmartSolid.CreateCylinder(null, x_down_rebar_diameter / 2, length - up_side_protective_layer_thinckness * 2);
                x_down_rebar.Transform(transform);
                Point3d position = app.Point3dFromXYZ(0, - width / 2 + up_side_protective_layer_thinckness + x_up_rebar_diameter / 2 + real_x_up_rebar_spacing * i, - height / 2 + down_protective_layer_thinckness + x_up_rebar_diameter / 2);
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
            int y_up_rebar_num = (int)Math.Ceiling((length - up_side_protective_layer_thinckness * 2 - y_up_rebar_diameter) / y_up_rebar_spacing) + 1;
            double real_y_up_rebar_spacing = (length - up_side_protective_layer_thinckness * 2 - y_up_rebar_diameter) / (y_up_rebar_num - 1);
            for (int i = 0; i < y_up_rebar_num; i++)
            {
                Element y_up_rebar = app.SmartSolid.CreateCylinder(null, y_up_rebar_diameter / 2, width - up_side_protective_layer_thinckness * 2);
                y_up_rebar.Transform(transform);
                Point3d position = app.Point3dFromXYZ(- length / 2 + up_side_protective_layer_thinckness + y_up_rebar_diameter / 2 + real_y_up_rebar_spacing * i, 0, height / 2 - up_side_protective_layer_thinckness - y_up_rebar_diameter / 2);
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
            int y_down_rebar_num = (int)Math.Ceiling((length - up_side_protective_layer_thinckness * 2 - y_down_rebar_diameter) / y_down_rebar_spacing) + 1;
            double real_y_down_rebar_spacing = (length - up_side_protective_layer_thinckness * 2 - y_down_rebar_diameter) / (y_down_rebar_num - 1);
            for (int i = 0; i < y_down_rebar_num; i++)
            {
                Element y_down_rebar = app.SmartSolid.CreateCylinder(null, y_up_rebar_diameter / 2, width - up_side_protective_layer_thinckness * 2);
                y_down_rebar.Transform(transform);
                Point3d position = app.Point3dFromXYZ(-length / 2 + up_side_protective_layer_thinckness + y_up_rebar_diameter / 2 + real_y_down_rebar_spacing * i, 0, - height / 2 + down_protective_layer_thinckness + y_up_rebar_diameter / 2);
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
            double move_distance = x_up_rebar_diameter / 2 + y_up_rebar_diameter / 2 + 1;
            if (length > width)
            {
                x_up_rebars.Move(app.Point3dFromXYZ(0, 0, - move_distance));
                x_down_rebars.Move(app.Point3dFromXYZ(0, 0, move_distance));
            }
            else
            {
                y_up_rebars.Move(app.Point3dFromXYZ(0, 0, - move_distance));
                y_down_rebars.Move(app.Point3dFromXYZ(0, 0, move_distance));
            }
            rebars = app.SmartSolid.SolidUnion(x_up_rebars.AsSmartSolidElement, y_up_rebars.AsSmartSolidElement);
            rebars = app.SmartSolid.SolidUnion(rebars.AsSmartSolidElement, y_down_rebars.AsSmartSolidElement);
            rebars = app.SmartSolid.SolidUnion(rebars.AsSmartSolidElement, x_down_rebars.AsSmartSolidElement);
            return rebars;
        }
        //画柱纵筋,参数分别为柱子的长、宽、高和基础高度
        public Element create_column_longitudinal_rebars(double length, double width, double height, double foundation_height)
        {
            Element longitudinal_rebars = null, anchor_bars = null, anchor_bending_rebars = null, anchor_bending_rebar_arcs = null, rebars = null;
            //画纵筋
            Point3d up_right, up_left, down_right, down_left;
            up_right = app.Point3dFromXYZ(length / 2 - protective_layer_thinckness - stirrup_diameter - longitudinal_rebar_diameter / 2, width / 2 - protective_layer_thinckness - stirrup_diameter - longitudinal_rebar_diameter / 2, height / 2 + foundation_height / 2);
            up_left = app.Point3dFromXYZ(-length / 2 + protective_layer_thinckness + stirrup_diameter + longitudinal_rebar_diameter / 2, width / 2 - protective_layer_thinckness - stirrup_diameter - longitudinal_rebar_diameter / 2, height / 2 + foundation_height / 2);
            down_right = app.Point3dFromXYZ(length / 2 - protective_layer_thinckness - stirrup_diameter - longitudinal_rebar_diameter / 2, -width / 2 + protective_layer_thinckness + stirrup_diameter + longitudinal_rebar_diameter / 2, height / 2 + foundation_height / 2);
            down_left = app.Point3dFromXYZ(-length / 2 + protective_layer_thinckness + stirrup_diameter + longitudinal_rebar_diameter / 2, -width / 2 + protective_layer_thinckness + stirrup_diameter + longitudinal_rebar_diameter / 2, height / 2 + foundation_height / 2);

            Point3d[] longitudinal_rebar_positions = new Point3d[4];
            longitudinal_rebar_positions[0] = up_right;
            longitudinal_rebar_positions[1] = up_left;
            longitudinal_rebar_positions[2] = down_right;
            longitudinal_rebar_positions[3] = down_left;
            for (int i = 0; i < 4; i++)
            {
                Element longitudinal_rebar = app.SmartSolid.CreateCylinder(null, longitudinal_rebar_diameter / 2, height);
                longitudinal_rebar.Move(longitudinal_rebar_positions[i]);
                if (i == 0)
                {
                    longitudinal_rebars = longitudinal_rebar;
                }
                else
                {
                    longitudinal_rebars = app.SmartSolid.SolidUnion(longitudinal_rebars.AsSmartSolidElement, longitudinal_rebar.AsSmartSolidElement);
                }
            }
            rebars = longitudinal_rebars;
            //画插入基础的锚固钢筋
            up_right = app.Point3dFromXYZ(length / 2 - protective_layer_thinckness - stirrup_diameter - longitudinal_rebar_diameter / 2, width / 2 - protective_layer_thinckness - stirrup_diameter - longitudinal_rebar_diameter / 2, (down_protective_layer_thinckness + x_down_rebar_diameter + 1 + y_down_rebar_diameter + longitudinal_rebar_diameter / 2 + anchor_bending_rebar_radius) / 2);
            up_left = app.Point3dFromXYZ(-length / 2 + protective_layer_thinckness + stirrup_diameter + longitudinal_rebar_diameter / 2, width / 2 - protective_layer_thinckness - stirrup_diameter - longitudinal_rebar_diameter / 2, (down_protective_layer_thinckness + x_down_rebar_diameter + 1 + y_down_rebar_diameter + longitudinal_rebar_diameter / 2 + anchor_bending_rebar_radius) / 2);
            down_right = app.Point3dFromXYZ(length / 2 - protective_layer_thinckness - stirrup_diameter - longitudinal_rebar_diameter / 2, -width / 2 + protective_layer_thinckness + stirrup_diameter + longitudinal_rebar_diameter / 2, (down_protective_layer_thinckness + x_down_rebar_diameter + 1 + y_down_rebar_diameter + longitudinal_rebar_diameter / 2 + anchor_bending_rebar_radius) / 2);
            down_left = app.Point3dFromXYZ(-length / 2 + protective_layer_thinckness + stirrup_diameter + longitudinal_rebar_diameter / 2, -width / 2 + protective_layer_thinckness + stirrup_diameter + longitudinal_rebar_diameter / 2, (down_protective_layer_thinckness + x_down_rebar_diameter + 1 + y_down_rebar_diameter + longitudinal_rebar_diameter / 2 + anchor_bending_rebar_radius) / 2);
            Point3d[] anchor_bar_positions = new Point3d[4];
            anchor_bar_positions[0] = up_right;
            anchor_bar_positions[1] = up_left;
            anchor_bar_positions[2] = down_right;
            anchor_bar_positions[3] = down_left;
            for (int i = 0; i < 4; i++)
            {
                Element anchor_bar = app.SmartSolid.CreateCylinder(null, longitudinal_rebar_diameter / 2, foundation_height - (down_protective_layer_thinckness + x_down_rebar_diameter  + 1 + y_down_rebar_diameter + longitudinal_rebar_diameter / 2 + anchor_bending_rebar_radius));
                anchor_bar.Move(anchor_bar_positions[i]);
                if (i == 0)
                {
                    anchor_bars = anchor_bar;
                }
                else
                {
                    anchor_bars = app.SmartSolid.SolidUnion(anchor_bars.AsSmartSolidElement, anchor_bar.AsSmartSolidElement);
                }
            }
            rebars = app.SmartSolid.SolidUnion(rebars.AsSmartSolidElement, anchor_bars.AsSmartSolidElement);
            //画弯折弧
            double angle = Math.Atan(length / width);
            double anchor_bending_rebar_length = longitudinal_rebar_diameter * 6 > 150 ? longitudinal_rebar_diameter * 6 : 150;
            double dx = Math.Cos(angle) * ((anchor_bending_rebar_length - anchor_bending_rebar_radius) / 2 + anchor_bending_rebar_radius), dy = Math.Sin(angle) * ((anchor_bending_rebar_length - anchor_bending_rebar_radius) / 2 + anchor_bending_rebar_radius);
            Transform3d transform3d = app.Transform3dFromMatrix3d(app.Matrix3dFromRotationBetweenVectors(app.Point3dFromXYZ(1, 0, 0), app.Point3dFromXYZ(0, 0, -1)));
            Transform3d[] transform3ds = new Transform3d[4];
            transform3ds[0] = app.Transform3dFromMatrix3d(app.Matrix3dFromRotationBetweenVectors(app.Point3dFromXYZ(0, -1, 0), app.Point3dFromXYZ(dx, dy, 0)));
            transform3ds[1] = app.Transform3dFromMatrix3d(app.Matrix3dFromRotationBetweenVectors(app.Point3dFromXYZ(0, -1, 0), app.Point3dFromXYZ(dx, -dy, 0)));
            transform3ds[2] = app.Transform3dFromMatrix3d(app.Matrix3dFromRotationBetweenVectors(app.Point3dFromXYZ(0, -1, 0), app.Point3dFromXYZ(-dx, -dy, 0)));
            transform3ds[3] = app.Transform3dFromMatrix3d(app.Matrix3dFromRotationBetweenVectors(app.Point3dFromXYZ(0, -1, 0), app.Point3dFromXYZ(-dx, dy, 0)));
            Point3d[] anchor_bending_rebar_arc_positions = new Point3d[4];
            anchor_bending_rebar_arc_positions[0] = app.Point3dFromXYZ(up_right.X, up_right.Y, - foundation_height / 2 + down_protective_layer_thinckness + x_down_rebar_diameter + 1 + y_down_rebar_diameter + longitudinal_rebar_diameter / 2 + anchor_bending_rebar_radius);
            anchor_bending_rebar_arc_positions[1] = app.Point3dFromXYZ(down_right.X, down_right.Y, - foundation_height / 2 + down_protective_layer_thinckness + x_down_rebar_diameter + 1 + y_down_rebar_diameter + longitudinal_rebar_diameter / 2 + anchor_bending_rebar_radius);
            anchor_bending_rebar_arc_positions[2] = app.Point3dFromXYZ(down_left.X, down_left.Y, - foundation_height / 2 + down_protective_layer_thinckness + x_down_rebar_diameter + 1 + y_down_rebar_diameter + longitudinal_rebar_diameter / 2 + anchor_bending_rebar_radius);
            anchor_bending_rebar_arc_positions[3] = app.Point3dFromXYZ(up_left.X, up_left.Y, -foundation_height / 2 + down_protective_layer_thinckness + x_down_rebar_diameter + 1 + y_down_rebar_diameter + longitudinal_rebar_diameter / 2 + anchor_bending_rebar_radius);
            for (int i = 0; i < 4; i++)
            {
                Element anchor_bending_rebar_arc = app.SmartSolid.CreateTorus(null, anchor_bending_rebar_radius, longitudinal_rebar_diameter / 2, 90);
                anchor_bending_rebar_arc.Transform(transform3d);
                anchor_bending_rebar_arc.Move(app.Point3dFromXYZ(0, -anchor_bending_rebar_radius, 0));  //调整弯折弧中心
                anchor_bending_rebar_arc.Transform(transform3ds[i]);
                anchor_bending_rebar_arc.Move(anchor_bending_rebar_arc_positions[i]);                   //调整弯折弧位置
                if (i == 0)
                {
                    anchor_bending_rebar_arcs = anchor_bending_rebar_arc;
                }
                else
                {
                    anchor_bending_rebar_arcs = app.SmartSolid.SolidUnion(anchor_bending_rebar_arcs.AsSmartSolidElement, anchor_bending_rebar_arc.AsSmartSolidElement);
                }
            }
            rebars = app.SmartSolid.SolidUnion(rebars.AsSmartSolidElement, anchor_bending_rebar_arcs.AsSmartSolidElement);

            //画锚固钢筋弯折直段部分
            transform3ds = new Transform3d[4];
            transform3ds[0] = app.Transform3dFromMatrix3d(app.Matrix3dFromRotationBetweenVectors(app.Point3dFromXYZ(0, 0, 1), app.Point3dFromXYZ(1, 1, 0)));
            transform3ds[1] = app.Transform3dFromMatrix3d(app.Matrix3dFromRotationBetweenVectors(app.Point3dFromXYZ(0, 0, 1), app.Point3dFromXYZ(1, -1, 0)));
            transform3ds[2] = app.Transform3dFromMatrix3d(app.Matrix3dFromRotationBetweenVectors(app.Point3dFromXYZ(0, 0, 1), app.Point3dFromXYZ(-1, -1, 0)));
            transform3ds[3] = app.Transform3dFromMatrix3d(app.Matrix3dFromRotationBetweenVectors(app.Point3dFromXYZ(0, 0, 1), app.Point3dFromXYZ(-1, 1, 0)));
            up_right = app.Point3dFromXYZ(length / 2 - protective_layer_thinckness - stirrup_diameter - longitudinal_rebar_diameter / 2 + dx, width / 2 - protective_layer_thinckness - stirrup_diameter - longitudinal_rebar_diameter / 2 + dy,  - foundation_height / 2 + down_protective_layer_thinckness + x_down_rebar_diameter + y_down_rebar_diameter + 1 + longitudinal_rebar_diameter / 2);
            up_left = app.Point3dFromXYZ(-length / 2 + protective_layer_thinckness + stirrup_diameter + longitudinal_rebar_diameter / 2 - dx, width / 2 - protective_layer_thinckness - stirrup_diameter - longitudinal_rebar_diameter / 2 + dy, -foundation_height / 2 + down_protective_layer_thinckness + x_down_rebar_diameter + y_down_rebar_diameter + 1 + longitudinal_rebar_diameter / 2);
            down_right = app.Point3dFromXYZ(length / 2 - protective_layer_thinckness - stirrup_diameter - longitudinal_rebar_diameter / 2 + dx, -width / 2 + protective_layer_thinckness + stirrup_diameter + longitudinal_rebar_diameter / 2 - dy, -foundation_height / 2 + down_protective_layer_thinckness + x_down_rebar_diameter + y_down_rebar_diameter + 1 + longitudinal_rebar_diameter / 2);
            down_left = app.Point3dFromXYZ(-length / 2 + protective_layer_thinckness + stirrup_diameter + longitudinal_rebar_diameter / 2 - dx, -width / 2 + protective_layer_thinckness + stirrup_diameter + longitudinal_rebar_diameter / 2 - dy, -foundation_height / 2 + down_protective_layer_thinckness + x_down_rebar_diameter + y_down_rebar_diameter + 1 + longitudinal_rebar_diameter / 2);
            Point3d[] anchor_bending_rebar_positions = new Point3d[4];
            anchor_bending_rebar_positions[0] = up_right;
            anchor_bending_rebar_positions[1] = down_right;
            anchor_bending_rebar_positions[2] = down_left;
            anchor_bending_rebar_positions[3] = up_left;
            for (int i = 0; i < 4; i++)
            {
                Element anchor_bending_rebar = app.SmartSolid.CreateCylinder(null, longitudinal_rebar_diameter / 2, anchor_bending_rebar_length - anchor_bending_rebar_radius);
                anchor_bending_rebar.Transform(transform3ds[i]);
                anchor_bending_rebar.Move(anchor_bending_rebar_positions[i]);
                if (i == 0)
                {
                    anchor_bending_rebars = anchor_bending_rebar;
                }
                else
                {
                    anchor_bending_rebars = app.SmartSolid.SolidUnion(anchor_bending_rebars.AsSmartSolidElement, anchor_bending_rebar.AsSmartSolidElement);
                }
            }
            rebars = app.SmartSolid.SolidUnion(rebars.AsSmartSolidElement, anchor_bending_rebars.AsSmartSolidElement);
            return rebars;
        }
        //画柱纵筋，从上而下每一个为一根
        public Element create_column_longitudinal_rebars(double length, double width, double height, double foundation_height, double lae)
        {

            return null;
        }
        //画柱箍筋，参数分别为柱子的长、宽、高和基础高度
        public Element create_column_stirrups(double length, double width, double height, double foundation_height)
        {
            Element stirrups = null;
            int n = (int)Math.Ceiling((height - stirrup_diameter - 50) / (stirrup_spacing)) + 1;
            double real_stirrup_spacing = (height - stirrup_diameter - 50) / (n - 1);
            for (int i = 0; i < n; i++)
            {
                Element single_stirrup = create_single_stirrup(length, width, "1");
                single_stirrup.Move(app.Point3dFromXYZ(0, 0, foundation_height / 2 + stirrup_diameter / 2 + 50 + i * real_stirrup_spacing));
                if (i == 0)
                {
                    stirrups = single_stirrup;
                }
                else
                {
                    stirrups = app.SmartSolid.SolidUnion(stirrups.AsSmartSolidElement, single_stirrup.AsSmartSolidElement);
                }
            }
            Element stirrup = create_single_stirrup(length, width, "2");
            stirrup.Move(app.Point3dFromXYZ(0, 0, foundation_height / 2 - 100));
            stirrups = app.SmartSolid.SolidUnion(stirrups.AsSmartSolidElement, stirrup.AsSmartSolidElement);
            stirrup = create_single_stirrup(length, width, "2");
            stirrup.Move(app.Point3dFromXYZ(0, 0, - foundation_height / 2 + down_protective_layer_thinckness + x_down_rebar_diameter + y_down_rebar_diameter + 1 + longitudinal_rebar_diameter / 2 + anchor_bending_rebar_radius + longitudinal_rebar_diameter));
            stirrups = app.SmartSolid.SolidUnion(stirrups.AsSmartSolidElement, stirrup.AsSmartSolidElement);
            return stirrups;
        }
        //画单个箍筋，参数分别为柱子的长、宽和箍筋类型
        private Element create_single_stirrup(double b, double h, string type)
        {
            Element single_stirrup = null;
            switch (type)
            {
                case "1":
                    single_stirrup = StirrupUtil.create_stirrup_type1(b - Data.protective_layer_thinckness * 2 - Data.stirrup_diameter * 2 - Data.longitudinal_rebar_diameter, h - Data.protective_layer_thinckness * 2 - Data.stirrup_diameter * 2 - Data.longitudinal_rebar_diameter, 8, 8);
                    break;
                case "2":
                    single_stirrup = StirrupUtil.create_stirrup_type2(b - Data.protective_layer_thinckness * 2 - Data.stirrup_diameter * 2 - Data.longitudinal_rebar_diameter, h - Data.protective_layer_thinckness * 2 - Data.stirrup_diameter * 2 - Data.longitudinal_rebar_diameter);
                    break;
            }
            return single_stirrup;
        }

        //为元素添加样式
        public void add_part_to_element(ref Element element, TFPartRef tfpart_ref)
        {
            try
            {
                Level level;
                int color, weight, style;
                string level_str;

                TFPartList tfpart_list = new TFPartList();
                tfpart_ref.GetPart(out tfpart_list);
                TFPart tfpart = tfpart_list.AsTFPart;
                //提取样式
                tfpart.GetModelLevelNameW(out level_str);
                if ((level = app.ActiveDesignFile.Levels.Find(level_str)) == null)
                {
                    level = app.ActiveDesignFile.AddNewLevel(level_str);
                }
                tfpart.GetModelSymbologyValues(out color, out weight, out style);
                //为元素赋予样式
                try
                {
                    element.Color = color;
                    element.LineWeight = weight;
                    element.LineStyle = app.ActiveDesignFile.LineStyles[style];
                }
                catch
                { }

                TFElementList tfelement_list = new TFElementList();
                tfelement_list.InitFromElement(element);
                TFElement tfelement = tfelement_list.AsTFElement;
                tfelement.SetPartRef(tfpart_ref);
                tfelement.GetElement(out element);
            }
            catch
            {

            }
        }

        //创建样式
        public TFPartRef create_tfpart_ref(string family, string part)
        {
            TFPartRefList tfpart_ref_list = new TFPartRefList();
            TFPartRef tfpart_ref = tfpart_ref_list.AsTFPartRef;
            tfpart_ref.SetPartFamilyName(family);
            tfpart_ref.SetPartName(part);
            return tfpart_ref;
        }




        public void Cleanup()
        {

        }

        public void DataPoint(ref Point3d Point, Bentley.Interop.MicroStationDGN.View View)
        {
            Element column, foundation, foundation_rebars, column_longitudinal_rebars, column_stirrups;
            column = rc_util.create_column(800, 800, 1500, 400); 
            foundation = rc_util.create_foundation(1500, 1500, 400);

            column_longitudinal_rebars = rc_util.create_column_longitudinal_rebars(800, 800, 1500, 400);
            column_stirrups = rc_util.create_column_stirrups(800, 800, 1500, 400);
            foundation_rebars = rc_util.create_foundation_rebars(1500, 1500, 400); 
            
            TFPartRef tfpart_ref = rc_util.create_tfpart_ref("Ceiling", "Metal");
            rc_util.add_part_to_element(ref column, tfpart_ref);
            rc_util.add_part_to_element(ref foundation, tfpart_ref);
            rc_util.add_part_to_element(ref column_longitudinal_rebars, tfpart_ref);
            rc_util.add_part_to_element(ref column_stirrups, tfpart_ref);
            rc_util.add_part_to_element(ref foundation_rebars, tfpart_ref);
            column.Move(ref Point);
            foundation.Move(ref Point);
            column_longitudinal_rebars.Move(ref Point);
            column_stirrups.Move(ref Point);
            foundation_rebars.Move(ref Point);

            List<Element> elements = new List<Element>();
            elements.Add(column);
            elements.Add(foundation);
            elements.Add(column_longitudinal_rebars);
            elements.Add(column_stirrups);
            elements.Add(foundation_rebars);

            rc_util.draw_Elements(elements.ToArray(), "diji");
        }

        public void Dynamics(ref Point3d Point, Bentley.Interop.MicroStationDGN.View View, MsdDrawingMode DrawMode)
        {
            //动态移动的时候不显示配筋，只有点击的时候才画配筋
            Element column = rc_util.create_column(800, 800, 1500, 400), foundation = rc_util.create_foundation(1500, 1500, 400);
            column.Move(ref Point);
            foundation.Move(ref Point);

            //必须redraw，否侧元素不显示
            column.Redraw(DrawMode);
            foundation.Redraw(DrawMode);
        }

        public void Keyin(string Keyin)
        {

        }

        public void Reset()
        {
            app.CommandState.StartDefaultCommand();
        }

        public void Start()
        {
            app.CommandState.EnableAccuSnap();
            app.CommandState.StartDynamics();
            app.ShowPrompt("请选择地基放置的中心点");
        }
    }
}
