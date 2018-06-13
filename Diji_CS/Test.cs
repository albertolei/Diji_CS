using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Diji_CS.Utils;
using Bentley.Interop.MicroStationDGN;
using Bentley.MicroStation.InteropServices;

using Diji_CS.Datas.StirrupData;

namespace Diji_CS
{
    class Test
    {
        private static Bentley.Interop.MicroStationDGN.Application app = Utilities.ComApp;

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
            //Segment3d axis = app.Segment3dFromXYZXYZStartEnd(0, 0, 0, 0, 0, 2000);
            //Point3d startPoint = app.Point3dZero();
            //bsp.Helix(100, 100, ref startPoint, ref axis, 0);
            //BsplineCurveElement bspelement = app.CreateBsplineCurveElement1(null, bsp);
            //app.ActiveModelReference.AddElement(bspelement);

            //Point3d center = app.Point3dZero();
            //Matrix3d rotation = app.Matrix3dFromRotationBetweenVectors(app.Point3dFromXYZ(0, 0, 1), app.Point3dFromXYZ(1, 0, 0));
            //Element top_horizontal = app.CreateArcElement2(null, ref center, 100.0, 100.0, ref rotation, 0, 540 / Data.ANGLE_180 * Math.PI);
            //top_horizontal.Transform(app.Transform3dFromLineAndRotationAngle(app.Point3dZero(), app.Point3dFromXYZ(0, 1, 0), Math.PI / 2));

            //Element bottom_horizontal = app.CreateArcElement2(null, ref center, 100.0, 100.0, ref rotation, 0, 540 / Data.ANGLE_180 * Math.PI);
            //bottom_horizontal.Transform(app.Transform3dFromLineAndRotationAngle(app.Point3dZero(), app.Point3dFromXYZ(0, 1, 0), Math.PI / 2));

            //Element path = null;
            //double radius0 = 100.0, radius1 = 100.0;
            //Segment3d axis = new Segment3d();
            //axis.StartPoint = app.Point3dZero();
            //axis.EndPoint = app.Point3dFromXYZ(0, 0, 1000);
            //top_horizontal.Move(axis.EndPoint);

            //Point3d startPoint = app.Point3dFromXYZ(1, 0, 0);
            //BsplineCurve bspCurve = new BsplineCurveClass();
            //bspCurve.Helix(radius0, radius1, startPoint, axis, 5.0, true);
            //path = app.CreateBsplineCurveElement1(null, bspCurve);

            //Element circle = app.CreateEllipseElement2(null, app.Point3dFromXYZ(100, 0, 0), 5.0, 5.0, app.Matrix3dFromRotationBetweenVectors(app.Point3dFromXYZ(0, 0, 1), app.Point3dFromXYZ(0, 1, 0)));
            //Element spiral_stirrup = app.SmartSolid.SweepProfileAlongPath(circle, path);


            //Element bottom_stirrup = app.SmartSolid.CreateTorus(null, 100, 5, 540);
            //Element top_stirrup = app.SmartSolid.CreateTorus(null, 100, 5, 540);
            //top_stirrup.Move(axis.EndPoint);
            //Element bottom_stirrup = app.SmartSolid.SweepProfileAlongPath(circle, bottom_horizontal);
            //app.ActiveModelReference.AddElement(top_horizontal);
            //app.ActiveModelReference.AddElement(bottom_horizontal);
            //app.ActiveModelReference.AddElement(path);
            //app.ActiveModelReference.AddElement(circle);
            //Element stirrup = app.SmartSolid.SolidUnion(spiral_stirrup.AsSmartSolidElement, bottom_stirrup.AsSmartSolidElement);
            //stirrup = app.SmartSolid.SolidUnion(stirrup.AsSmartSolidElement, top_stirrup.AsSmartSolidElement);
            //app.ActiveModelReference.AddElement(stirrup);
            //app.ActiveModelReference.AddElement(spiral_stirrup);
            //app.ActiveModelReference.AddElement(bottom_stirrup);
            //Element stirrup = StirrupUtil.create_spiral_stirrup(1200.0, 200.0, 200.0, 100.0, 400.0, 100.0, 200.0);
            Element stirrup = StirrupUtil.create_column_stirrups(800, 800, 1500, 400, "5");
            Element longitudinal = LongitudinalBarUtil.create_column_longitudinal_rebars(800, 800, 1500, 1500, 1500, 400, 50, "5");
            app.ActiveModelReference.AddElement(stirrup);
            app.ActiveModelReference.AddElement(longitudinal);
        }

        public static void test(string unparsed)
        {
            Element element = app.SmartSolid.CreateSlab(null, 1200, 1000, 800);
            long elementID = element.ID;
            DataBlock db = new DataBlockClass();
            db.Offset = 0;
            string testDataBlock = "test datablock";
            db.CopyString(ref testDataBlock, true);
            element.AddUserAttributeData(10000, db);
            app.ActiveModelReference.AddElement(element);

            string outstr = "";
            DataBlock[] dbs = element.GetUserAttributeData(10000);
            dbs[0].CopyString(ref outstr, false);
            System.Windows.Forms.MessageBox.Show(outstr);
        }
    }
}
