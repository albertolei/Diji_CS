using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Bentley.Interop.MicroStationDGN;
using Bentley.MicroStation.InteropServices;
using Bentley.Interop.TFCom;

namespace Diji_CS.Datas
{
    class Data
    {
        //柱配筋的保护层厚度，纵筋直径，箍筋直径, 箍筋间距，弯折长度，受拉钢筋抗震锚固长度lae, 锚固钢筋弯折弧半径
        public static double protective_layer_thinckness = 50, longitudinal_rebar_diameter = 22, stirrup_diameter = 10, stirrup_spacing = 100, bending_length = 75, lae = 300, anchor_bending_rebar_radius = 20;
        //基础配筋的上面和侧面的保护层厚度、下部的保护层厚度
        public static double up_side_protective_layer_thinckness = 30, down_protective_layer_thinckness = 70;
        //底板顶部x向钢筋直径、底板顶部x向钢筋间距、底板顶部y向钢筋直径、底板顶部y向钢筋间距
        public static double x_up_rebar_diameter = 25, x_up_rebar_spacing = 100, y_up_rebar_diameter = 25, y_up_rebar_spacing = 100;
        //底板底部x向钢筋直径、底板底部x向钢筋间距、底板底部y向钢筋直径、底板底部y向钢筋间距
        public static double x_down_rebar_diameter = 25, x_down_rebar_spacing = 100, y_down_rebar_diameter = 25, y_down_rebar_spacing = 100;
        public static int STIRRUPMUTIPLE = 10;
        //圆形箍筋搭接长度
        public static double lap_length = 300;
        
        public static double ANGLE_0 = 0;
        public static double ANGLE_45 = 45;
        public static double ANGLE_90 = 90;
        public static double ANGLE_135 = 135;
        public static double ANGLE_180 = 180;
        public static double ANGLE_225 = 225;
        public static double ANGLE_270 = 270;
        public static double ANGLE_315 = 315;
        public static double ANGLE_360 = 360;
    }
}
