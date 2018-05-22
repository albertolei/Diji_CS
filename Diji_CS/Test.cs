using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Diji_CS.Utils;
using Bentley.Interop.MicroStationDGN;
using Bentley.MicroStation.InteropServices;

namespace Diji_CS
{
    class Test
    {
        //柱配筋的保护层厚度，纵筋直径，箍筋直径, 弯折长度
        private static double protective_layer_thinckness = 50, longitudinal_rebar_diameter = 22, stirrup_diameter = 10, stirrup_spacing = 100, bending_length = 75, lae = 300, anchor_bending_rebar_radius = 20;
        //基础配筋的上面和侧面的保护层厚度、下部的保护层厚度、x向钢筋直径、x向钢筋间距、y向钢筋直径、y向钢筋间距
        private static double up_side_protective_layer_thinckness = 0, down_protective_layer_thinckness = 0, x_rebar_diameter = 25, x_rebar_spacing = 100, y_rebar_diameter = 25, y_rebar_spacing = 100;

        private static double length = 785, width = 785;

        private static int STIRRUPMUTIPLE = 10;

        public static void sayHello(string unparsed)
        {
            
            Application app = Utilities.ComApp;
            //TextElement text = app.CreateTextElement1(null, "数字化设计部", app.Point3dFromXYZ(0, 0, 0), app.Matrix3dZero());
            //app.ActiveModelReference.AddElement(text);
            //app.ActiveModelReference.AddElement(element);
            //Element element = StirrupUtil.create_closed_stirrup(1000, 400);
            //Element element = LongitudinalBarUtil.create_longitudinal_bar(25, 1500 - 50, 200 - 50, Data.ANGLE_90);
            //Element element = LongitudinalBarUtil.create_longitudinal_bar_type2(800, 800, 1500, 1500, 1500, 400, 1500-50, 200-50);
            //Element element = RCUtil.create_column(800, 1500, 400);
            //Element element = StirrupUtil.create_stirrup_type6(800);
            //Element element = LongitudinalBarUtil.create_longitudinal_bar_type6(800,1500,1500,1500,400, 1500- Data.down_protective_layer_thinckness - Data.x_down_rebar_diameter - 1 - Data.y_down_rebar_diameter - 1 - Data.longitudinal_rebar_diameter / 2, 200-50);
            //Element element = StirrupUtil.create_closed_stirrup(800, 300, 500, 40);
            //Element element = null;
            //BsplineCurveElement bspCurveElement =
            //app.ActiveModelReference.AddElement(element);

            //Point3d basePt = app.Point3dFromXYZ(2, -23, 0);
            //Point3d topPt = app.Point3dFromXYZ(2, -20, 0);
            //Matrix3d rMatrix = app.Matrix3dFromAxisAndRotationAngle(0, app.Pi() / 6);
            //ConeElement oCone = app.CreateConeElement1(null, 2, ref basePt, 1, ref topPt, ref rMatrix);
            //oCone.Color = 0;
            //app.ActiveModelReference.AddElement(oCone);

            //Point3d[] aFitPnts = new Point3d[4];
            //InterpolationCurve oFitCurve = new InterpolationCurveClass();
            //BsplineCurve[] aCurves = new BsplineCurve[3];

            //aFitPnts[0] = app.Point3dFromXYZ(5.9, -21, -0.5);
            //aFitPnts[1] = app.Point3dFromXYZ(6.9, -20, 1);
            //aFitPnts[2] = app.Point3dFromXYZ(7.9, -20.3, 1.3);
            //aFitPnts[3] = app.Point3dFromXYZ(8.9, -20.8, 0.3);
            //oFitCurve.SetFitPoints(ref aFitPnts);
            //oFitCurve.BesselTangents = true;
            //aCurves[0] = new BsplineCurveClass();
            //aCurves[0].FromInterpolationCurve(oFitCurve);

            //aFitPnts[0] = app.Point3dFromXYZ(6.4, -22, 0);
            //aFitPnts[1] = app.Point3dFromXYZ(7.1, -21.2, 0.7);
            //aFitPnts[2] = app.Point3dFromXYZ(7.7, -21, 1);
            //aFitPnts[3] = app.Point3dFromXYZ(8.4, -21.7, -0.2);
            //oFitCurve.SetFitPoints(ref aFitPnts);
            //oFitCurve.BesselTangents = true;
            //aCurves[1] = new BsplineCurveClass();
            //aCurves[1].FromInterpolationCurve(oFitCurve);

            //aFitPnts[0] = app.Point3dFromXYZ(5.9, -23, 0);
            //aFitPnts[1] = app.Point3dFromXYZ(7.2, -23.1, 1.2);
            //aFitPnts[2] = app.Point3dFromXYZ(7.8, -23.3, 0.8);
            //aFitPnts[3] = app.Point3dFromXYZ(8.7, -22.8, 0.2);
            //oFitCurve.SetFitPoints(ref aFitPnts);
            //oFitCurve.BesselTangents = true;
            //aCurves[2] = new BsplineCurveClass();
            //aCurves[2].FromInterpolationCurve(oFitCurve);

            //BsplineSurface oBsplineSurface = new BsplineSurfaceClass();
            //oBsplineSurface.FromCrossSections(ref aCurves, MsdBsplineSurfaceDirection.V, 4, true, true);
            //BsplineSurfaceElement oSurfaceElm = app.CreateBsplineSurfaceElement1(null, oBsplineSurface);
            //oSurfaceElm.Color = 1;
            //app.ActiveModelReference.AddElement(oSurfaceElm);

            //Point3d[] points = new Point3d[5];
            //points[0] = app.Point3dFromXYZ(50, 0, 0);
            //points[1] = app.Point3dFromXYZ(0, 50, 20);
            //points[2] = app.Point3dFromXYZ(-50, 0, 40);
            //points[3] = app.Point3dFromXYZ(0, -50, 60);
            //points[4] = app.Point3dFromXYZ(50, 0, 0);

            //InterpolationCurve intercurve = new InterpolationCurveClass();
            //intercurve.SetFitPoints(points);
            //BsplineCurveElement bsp = app.CreateBsplineCurveElement2(null, intercurve);

            //CurveElement ce = app.CreateCurveElement2(null, points);
            
            //double r = 500;
            //Point3d[] pts = new Point3d[37];
            //List<LineElement> lines = new List<LineElement>();
            //pts[0] = app.Point3dFromXY(r, 0);
            //for (int i = 1; i <= 36; i++)
            //{
            //    Point3d p = app.Point3dFromXY(r * Math.Cos(10.0 * i / 180.0 * Math.PI), r * Math.Sin(10.0 * i / 180.0 * Math.PI));
            //    pts[i] = p;
            //    LineElement line = app.CreateLineElement2(null, pts[i - 1], pts[i]);
            //    lines.Add(line);
            //    //app.ActiveModelReference.AddElement(line);
            //}
            //CellElement result = app.CreateCellElement1("circle", lines.ToArray(),app.Point3dFromXY(0,0));
            //app.ActiveModelReference.AddElement(result);

            //BsplineCurve bsp = new BsplineCurveClass();
            //Segment3d axis = app.Segment3dFromXYZXYZStartEnd(0,0,0,0,0,2000);
            //Point3d startPoint = app.Point3dZero();
            //bsp.Helix(100, 100, ref startPoint, ref axis, 0);
            //BsplineCurveElement bspelement = app.CreateBsplineCurveElement1(null, bsp);
            //app.ActiveModelReference.AddElement(bspelement);

            Point3d center = app.Point3dZero();
            Matrix3d rotation = app.Matrix3dFromRotationBetweenVectors(app.Point3dFromXYZ(0, 0, 1), app.Point3dFromXYZ(1, 0, 0));
            Element top_horizontal = app.CreateArcElement2(null, ref center, 100.0, 100.0, ref rotation, 0, 540 / Data.ANGLE_180 * Math.PI);
            top_horizontal.Transform(app.Transform3dFromLineAndRotationAngle(app.Point3dZero(), app.Point3dFromXYZ(0, 1, 0), Math.PI / 2));

            Element bottom_horizontal = app.CreateArcElement2(null, ref center, 100.0, 100.0, ref rotation, 0, 540 / Data.ANGLE_180 * Math.PI);
            bottom_horizontal.Transform(app.Transform3dFromLineAndRotationAngle(app.Point3dZero(), app.Point3dFromXYZ(0, 1, 0), Math.PI / 2));

            Element path = null;
            double radius0 = 100.0, radius1 = 100.0;
            Segment3d axis = new Segment3d();
            axis.StartPoint = app.Point3dZero();
            axis.EndPoint = app.Point3dFromXYZ(0, 0, 100);
            top_horizontal.Move(axis.EndPoint);
            
            ////Point3d startPoint = app.Point3dCrossProduct(app.Point3dFromXYZ(0, 0, 1), app.Point3dSubtract(axis.StartPoint, axis.EndPoint));
            Point3d startPoint = app.Point3dFromXYZ(1, 0, 0);
            BsplineCurve bspCurve = new BsplineCurveClass();
            bspCurve.Helix(radius0, radius1, startPoint, axis, 5.0, true);
            path = app.CreateBsplineCurveElement1(null, bspCurve);

            Element circle = app.CreateEllipseElement2(null, app.Point3dFromXYZ(100, 0, 0), 5.0, 5.0, app.Matrix3dFromRotationBetweenVectors(app.Point3dFromXYZ(0, 0, 1), app.Point3dFromXYZ(0, 1, 0)));
            Element stirrup = app.SmartSolid.SweepProfileAlongPath(circle, path);

            app.ActiveModelReference.AddElement(top_horizontal);
            app.ActiveModelReference.AddElement(bottom_horizontal);
            app.ActiveModelReference.AddElement(path);
            //app.ActiveModelReference.AddElement(circle);
            app.ActiveModelReference.AddElement(stirrup);
        }

        //public static void sayHello(string unparsed)
        //{
        //    Application app = Utilities.ComApp;
        //    double anchor_bending_rebar_length = longitudinal_rebar_diameter * 6 > 150 ? longitudinal_rebar_diameter * 6 : 150;
        //    double angle = Math.Atan(length / width);
        //    double dx = Math.Cos(angle) * anchor_bending_rebar_length / 2, dy = Math.Sin(angle) * anchor_bending_rebar_length / 2;
        //    Element anchor_bending_rebar_arc = app.SmartSolid.CreateTorus(null, Data.anchor_bending_rebar_radius, longitudinal_rebar_diameter / 2, 90);
        //    //Transform3d transform3d = app.Transform3dFromMatrix3d(app.Matrix3dFromRotationBetweenVectors(app.Point3dFromXYZ(1, 0, 0), app.Point3dFromXYZ(0, 0, -1)));
        //    //anchor_bending_rebar_arc.Transform(transform3d);
        //    //transform3d = app.Transform3dFromMatrix3d(app.Matrix3dFromRotationBetweenVectors(app.Point3dFromXYZ(0, -1, 0), app.Point3dFromXYZ(dx, dy, 0)));
        //    //anchor_bending_rebar_arc.Transform(transform3d);
        //    if (dx >= dy)
        //    {
        //        dx = 1;
        //        dy = dy / dx;
        //    }
        //    else
        //    {
        //        dx = dx / dy;
        //        dy = 1;
        //    }
        //    Transform3d transform3d = app.Transform3dFromMatrix3d(app.Matrix3dFromRotationBetweenVectors(app.Point3dFromXYZ(1, 0, 0), app.Point3dFromXYZ(dx, dy, -1)));
        //    anchor_bending_rebar_arc.Transform(transform3d);
        //    anchor_bending_rebar_arc.Move(app.Point3dFromXYZ(0, -Data.anchor_bending_rebar_radius, 0));
        //    app.ActiveModelReference.AddElement(anchor_bending_rebar_arc);
        //}
        //public static void sayHello(string unparsed)
        //{
        //    Application app = Utilities.ComApp;
        //    ////该函数为画一个三维圆环，第2个函数为圆环的半径，第3个参数为环的半径，第4个参数为环转过的角度（以3点钟方向为起点，逆时针转过的角度）
        //    //Element element = app.SmartSolid.CreateTorus(null, 1500, 1000, 90);
        //    //element.RotateAboutZ(app.Point3dZero(),45);
        //    //app.ActiveModelReference.AddElement(element);

        //    Element single_stirrup = null;
        //    double primary_radiu = longitudinal_rebar_diameter / 2 + stirrup_diameter / 2, profile_radiu = stirrup_diameter / 2;

        //    bending_length = stirrup_diameter * STIRRUPMUTIPLE > 75 ? stirrup_diameter * STIRRUPMUTIPLE : 75;   //箍筋平直段长度
        //    //画第一段平直段
        //    Element first_bending = app.SmartSolid.CreateCylinder(null, stirrup_diameter / 2, bending_length);
        //    Point3d position = app.Point3dFromXYZ(length / 2 - protective_layer_thinckness - stirrup_diameter - longitudinal_rebar_diameter / 2 - bending_length / 2 / Math.Sqrt(2.0) - (longitudinal_rebar_diameter / 2 + stirrup_diameter / 2) / Math.Sqrt(2.0), width / 2 - protective_layer_thinckness - stirrup_diameter - longitudinal_rebar_diameter / 2 - bending_length / 2 / Math.Sqrt(2.0) + (longitudinal_rebar_diameter / 2 + stirrup_diameter / 2) / Math.Sqrt(2.0), 0);
        //    first_bending.Transform(app.Transform3dFromMatrix3d(app.Matrix3dFromRotationBetweenVectors(app.Point3dFromXYZ(0, 0, 1), app.Point3dFromXYZ(1, 1, 0))));
        //    first_bending.Move(position);
        //    single_stirrup = first_bending;
        //    //右上角第一段弧
        //    Element up_right_first_arc = app.SmartSolid.CreateTorus(null, primary_radiu, profile_radiu, 135);
        //    up_right_first_arc.Move(app.Point3dFromXYZ(length / 2 - protective_layer_thinckness - stirrup_diameter - longitudinal_rebar_diameter / 2, width / 2 - protective_layer_thinckness - stirrup_diameter - longitudinal_rebar_diameter / 2, 0));
        //    single_stirrup = app.SmartSolid.SolidUnion(first_bending.AsSmartSolidElement, up_right_first_arc.AsSmartSolidElement);
        //    //app.ActiveModelReference.AddElement(up_right_first_arc);
        //    //右侧箍筋
        //    Transform3d transform3d = app.Transform3dFromMatrix3d(app.Matrix3dFromRotationBetweenVectors(app.Point3dFromXYZ(0, 0, 1), app.Point3dFromXYZ(0, 1, 0)));
        //    position = app.Point3dFromXYZ(length / 2 - protective_layer_thinckness - stirrup_diameter / 2, 0, 0);
        //    Element right = app.SmartSolid.CreateCylinder(null, stirrup_diameter / 2, width - protective_layer_thinckness * 2 - stirrup_diameter * 2 - longitudinal_rebar_diameter);
        //    right.Transform(transform3d);
        //    right.Move(position);
        //    single_stirrup = app.SmartSolid.SolidUnion(single_stirrup.AsSmartSolidElement, right.AsSmartSolidElement);
        //    //app.ActiveModelReference.AddElement(right);
        //    //右下角弧
        //    Element down_right_arc = app.SmartSolid.CreateTorus(null, primary_radiu, profile_radiu, 90);
        //    transform3d = app.Transform3dFromMatrix3d(app.Matrix3dFromRotationBetweenVectors(app.Point3dFromXYZ(1, 0, 0), app.Point3dFromXYZ(0, -1, 0)));
        //    down_right_arc.Transform(transform3d);
        //    down_right_arc.Move(app.Point3dFromXYZ(length / 2 - protective_layer_thinckness - stirrup_diameter - longitudinal_rebar_diameter / 2, -width / 2 + protective_layer_thinckness + stirrup_diameter + longitudinal_rebar_diameter / 2, 0));
        //    single_stirrup = app.SmartSolid.SolidUnion(single_stirrup.AsSmartSolidElement, down_right_arc.AsSmartSolidElement);
        //    //app.ActiveModelReference.AddElement(down_right_arc);
        //    //下方箍筋
        //    transform3d = app.Transform3dFromMatrix3d(app.Matrix3dFromRotationBetweenVectors(app.Point3dFromXYZ(0, 0, 1), app.Point3dFromXYZ(1, 0, 0)));
        //    position = app.Point3dFromXYZ(0, -width / 2 + protective_layer_thinckness + stirrup_diameter / 2, 0);
        //    Element down = app.SmartSolid.CreateCylinder(null, stirrup_diameter / 2, length - protective_layer_thinckness * 2 - stirrup_diameter * 2 - longitudinal_rebar_diameter);
        //    down.Transform(transform3d);
        //    down.Move(position);
        //    single_stirrup = app.SmartSolid.SolidUnion(single_stirrup.AsSmartSolidElement, down.AsSmartSolidElement);
        //    //app.ActiveModelReference.AddElement(down);
        //    //左下角弧
        //    Element down_left_arc = app.SmartSolid.CreateTorus(null, primary_radiu, profile_radiu, 90);
        //    transform3d = app.Transform3dFromMatrix3d(app.Matrix3dFromRotationBetweenVectors(app.Point3dFromXYZ(1, 0, 0), app.Point3dFromXYZ(-1, 0, 0)));
        //    down_left_arc.Transform(transform3d);
        //    down_left_arc.Move(app.Point3dFromXYZ(-length / 2 + protective_layer_thinckness + stirrup_diameter + longitudinal_rebar_diameter / 2, -width / 2 + protective_layer_thinckness + stirrup_diameter + longitudinal_rebar_diameter / 2, 0));
        //    single_stirrup = app.SmartSolid.SolidUnion(single_stirrup.AsSmartSolidElement, down_left_arc.AsSmartSolidElement);
        //    //app.ActiveModelReference.AddElement(down_left_arc);
        //    //左侧箍筋
        //    transform3d = app.Transform3dFromMatrix3d(app.Matrix3dFromRotationBetweenVectors(app.Point3dFromXYZ(0, 0, 1), app.Point3dFromXYZ(0, 1, 0)));
        //    position = app.Point3dFromXYZ(-length / 2 + protective_layer_thinckness + stirrup_diameter / 2, 0, 0);
        //    Element left = app.SmartSolid.CreateCylinder(null, stirrup_diameter / 2, width - protective_layer_thinckness * 2 - stirrup_diameter * 2 - longitudinal_rebar_diameter);
        //    left.Transform(transform3d);
        //    left.Move(position);
        //    single_stirrup = app.SmartSolid.SolidUnion(single_stirrup.AsSmartSolidElement, left.AsSmartSolidElement);
        //    //app.ActiveModelReference.AddElement(left);
        //    //左上角弧
        //    Element up_left_arc = app.SmartSolid.CreateTorus(null, primary_radiu, profile_radiu, 90);
        //    transform3d = app.Transform3dFromMatrix3d(app.Matrix3dFromRotationBetweenVectors(app.Point3dFromXYZ(1, 0, 0), app.Point3dFromXYZ(0, 1, 0)));
        //    up_left_arc.Transform(transform3d);
        //    up_left_arc.Move(app.Point3dFromXYZ(-length / 2 + protective_layer_thinckness + stirrup_diameter + longitudinal_rebar_diameter / 2, width / 2 - protective_layer_thinckness - stirrup_diameter - longitudinal_rebar_diameter / 2, 0));
        //    single_stirrup = app.SmartSolid.SolidUnion(single_stirrup.AsSmartSolidElement, up_left_arc.AsSmartSolidElement);
        //    //app.ActiveModelReference.AddElement(up_left_arc);
        //    //上方箍筋
        //    transform3d = app.Transform3dFromMatrix3d(app.Matrix3dFromRotationBetweenVectors(app.Point3dFromXYZ(0, 0, 1), app.Point3dFromXYZ(-1, 0, 0)));
        //    position = app.Point3dFromXYZ(0, width / 2 - protective_layer_thinckness - stirrup_diameter / 2, 0);
        //    Element up = app.SmartSolid.CreateCylinder(null, stirrup_diameter / 2, length - protective_layer_thinckness * 2 - stirrup_diameter * 2 - longitudinal_rebar_diameter);
        //    up.Transform(transform3d);
        //    up.Move(position);
        //    single_stirrup = app.SmartSolid.SolidUnion(single_stirrup.AsSmartSolidElement, up.AsSmartSolidElement);
        //    //app.ActiveModelReference.AddElement(up);
        //    //右上角第二段弧
        //    Element up_right_second_arc = app.SmartSolid.CreateTorus(null, primary_radiu, profile_radiu, 135);
        //    transform3d = app.Transform3dFromMatrix3d(app.Matrix3dFromRotationBetweenVectors(app.Point3dFromXYZ(1, 0, 0), app.Point3dFromXYZ(1, -1, 0)));
        //    up_right_second_arc.Transform(transform3d);
        //    up_right_second_arc.Move(app.Point3dFromXYZ(length / 2 - protective_layer_thinckness - stirrup_diameter - longitudinal_rebar_diameter / 2, width / 2 - protective_layer_thinckness - stirrup_diameter - longitudinal_rebar_diameter / 2, 0));
        //    single_stirrup = app.SmartSolid.SolidUnion(single_stirrup.AsSmartSolidElement, up_right_second_arc.AsSmartSolidElement);
        //    //画第二段平直段
        //    Element second_bending = app.SmartSolid.CreateCylinder(null, stirrup_diameter / 2, bending_length);
        //    position = app.Point3dFromXYZ(length / 2 - protective_layer_thinckness - stirrup_diameter - longitudinal_rebar_diameter / 2 - bending_length / 2 / Math.Sqrt(2.0) + (longitudinal_rebar_diameter / 2 + stirrup_diameter / 2) / Math.Sqrt(2.0), width / 2 - protective_layer_thinckness - stirrup_diameter - longitudinal_rebar_diameter / 2 - bending_length / 2 / Math.Sqrt(2.0) - (longitudinal_rebar_diameter / 2 + stirrup_diameter / 2) / Math.Sqrt(2.0), 0);
        //    second_bending.Transform(app.Transform3dFromMatrix3d(app.Matrix3dFromRotationBetweenVectors(app.Point3dFromXYZ(0, 0, 1), app.Point3dFromXYZ(1, 1, 0))));
        //    second_bending.Move(position);
        //    single_stirrup = app.SmartSolid.SolidUnion(single_stirrup.AsSmartSolidElement, second_bending.AsSmartSolidElement);
        //    //app.ActiveModelReference.AddElement(up_right_second_arc);
        //    app.ActiveModelReference.AddElement(single_stirrup);
        //}
    }
}
