using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;

using Bentley.Interop.MicroStationDGN;
using Bentley.MicroStation.InteropServices;
using Bentley.Interop.TFCom;
using Diji_CS.Utils;
using Diji_CS.UI;
using Diji_CS.Datas.StirrupData;

namespace Diji_CS
{
    class RCCommand : IPrimitiveCommandEvents
    {
        private Bentley.Interop.MicroStationDGN.Application app = Utilities.ComApp;
        static StirrupForm stf = null;
        static IStirrupData stirrupData;

        public void Cleanup()
        {

        }

        public void DataPoint(ref Point3d Point, Bentley.Interop.MicroStationDGN.View View)
        {
            Element column = null, foundation = null, foundation_rebars = null, column_longitudinal_rebars = null, column_stirrups = null;
            stirrupData = read_Data();
            switch (stf.Type)
            {
                case TYPE.TYPE1:
                    {
                        StirrupData1 data = (StirrupData1)stirrupData;
                        column = RCUtil.create_column(data.B, data.H, 1500, 400);
                        foundation = RCUtil.create_foundation(1500, 1500, 400);
                        column_longitudinal_rebars = LongitudinalBarUtil.create_column_longitudinal_rebars(data.B, data.H, 1500, 1500, 1500, 400, 50, stf.Type, data.M, data.N);
                        column_stirrups = StirrupUtil.create_column_stirrups(data.B, data.H, 1500, 400, stf.Type, data.M, data.N);
                        foundation_rebars = FootingSlabBarUtil.create_foundation_rebars(1500, 1500, 400);
                        break;
                    }
                case TYPE.TYPE2:
                    {
                        StirrupData2 data = (StirrupData2)stirrupData;
                        column = RCUtil.create_column(data.B, data.H, 1500, 400);
                        foundation = RCUtil.create_foundation(1500, 1500, 400);
                        column_longitudinal_rebars = LongitudinalBarUtil.create_column_longitudinal_rebars(data.B, data.H, 1500, 1500, 1500, 400, 50, stf.Type);
                        column_stirrups = StirrupUtil.create_column_stirrups(data.B, data.H, 1500, 400, stf.Type);
                        foundation_rebars = FootingSlabBarUtil.create_foundation_rebars(1500, 1500, 400);
                        break;
                    }
                case TYPE.TYPE3:
                    {

                        break;
                    }
                case TYPE.TYPE4:
                    {
                        break;
                    }
                case TYPE.TYPE5:
                    {
                        break;
                    }
                case TYPE.TYPE6:
                    {
                        StirrupData6 data = (StirrupData6)stirrupData;
                        column = RCUtil.create_column(data.D, 1500, 400);
                        foundation = RCUtil.create_foundation(1500, 1500, 400);
                        column_longitudinal_rebars = LongitudinalBarUtil.create_column_longitudinal_rebars(data.D ,1500, 1500, 1500, 400, 50, stf.Type);
                        column_stirrups = StirrupUtil.create_column_stirrups(data.D, 1500, 400, stf.Type);
                        foundation_rebars = FootingSlabBarUtil.create_foundation_rebars(1500, 1500, 400);
                        break;
                    }
                case TYPE.TYPE7:
                    {
                        StirrupData7 data = (StirrupData7)stirrupData;
                        column = RCUtil.create_column(data.D, 1500, 400);
                        foundation = RCUtil.create_foundation(1500, 1500, 400);
                        column_longitudinal_rebars = LongitudinalBarUtil.create_column_longitudinal_rebars(data.D, 1500, 1500, 1500, 400, 50, stf.Type);
                        column_stirrups = StirrupUtil.create_column_stirrups(data.D, 1500, 400, stf.Type);
                        foundation_rebars = FootingSlabBarUtil.create_foundation_rebars(1500, 1500, 400);
                        break;
                    }
            }

            TFPartRef tfpart_ref = TFPartUtil.create_tfpart_ref("Ceiling", "Metal");
            TFPartUtil.add_part_to_element(ref column, tfpart_ref);
            TFPartUtil.add_part_to_element(ref foundation, tfpart_ref);
            TFPartUtil.add_part_to_element(ref column_longitudinal_rebars, tfpart_ref);
            TFPartUtil.add_part_to_element(ref column_stirrups, tfpart_ref);
            TFPartUtil.add_part_to_element(ref foundation_rebars, tfpart_ref);

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

            draw_Elements(elements.ToArray(), "diji");
        }

        public void Dynamics(ref Point3d Point, Bentley.Interop.MicroStationDGN.View View, MsdDrawingMode DrawMode)
        {
            Element column = null, foundation = null;
            stirrupData = read_Data();
            switch (stf.Type) 
            {
                case TYPE.TYPE1:
                    {
                        StirrupData1 data = (StirrupData1)stirrupData;
                        column = RCUtil.create_column(data.B, data.H, 1500, 400);
                        foundation = RCUtil.create_foundation(1500, 1500, 400);
                        break;
                    }
                case TYPE.TYPE2:
                    {
                        StirrupData2 data = (StirrupData2)stirrupData;
                        column = RCUtil.create_column(data.B, data.H, 1500, 400);
                        foundation = RCUtil.create_foundation(1500, 1500, 400);
                        break;
                    }
                case TYPE.TYPE3: 
                    {
                        
                        break;
                    }
                case TYPE.TYPE4:
                    {
                        break;
                    }
                case TYPE.TYPE5:
                    {
                        break;
                    }
                case TYPE.TYPE6:
                    {
                        StirrupData6 data = (StirrupData6)stirrupData;
                        column = RCUtil.create_column(data.D, 1500, 400);
                        foundation = RCUtil.create_foundation(1500, 1500, 400);
                        break;
                    }
                case TYPE.TYPE7:
                    {
                        StirrupData7 data = (StirrupData7)stirrupData;
                        column = RCUtil.create_column(data.D, 1500, 400);
                        foundation = RCUtil.create_foundation(1500, 1500, 400);
                        break;
                    }
            }

            //动态移动的时候不显示配筋，只有点击的时候才画配筋
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
            stf.Hide();
            app.CommandState.StartDefaultCommand();
        }

        public void Start()
        {
            app.CommandState.EnableAccuSnap();
            app.CommandState.StartDynamics();
            app.ShowPrompt("请选择地基放置的中心点");
            stf = new StirrupForm();
            stf.Show();
            
        }

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
        //读取界面数据
        public IStirrupData read_Data() 
        {
            IStirrupData stirrup_data = null;
            switch (stf.Type) 
            {
                case TYPE.TYPE1: 
                    {
                        TextBox textbox_b = (TextBox)stf.Controls.Find("textbox_b", true)[0];
                        double b = Double.Parse(textbox_b.Text);
                        TextBox textbox_h = (TextBox)stf.Controls.Find("textbox_h", true)[0];
                        double h = Double.Parse(textbox_h.Text);
                        ComboBox combobox_m = (ComboBox)stf.Controls.Find("combobox_m", true)[0];
                        int m = int.Parse(combobox_m.SelectedItem.ToString());
                        ComboBox combobox_n = (ComboBox)stf.Controls.Find("combobox_n", true)[0];
                        int n = int.Parse(combobox_n.SelectedItem.ToString());
                        stirrup_data = new StirrupData1(b, h, m, n);
                        break;
                    }
                case TYPE.TYPE2:
                    {
                        TextBox textbox_b = (TextBox)stf.Controls.Find("textbox_b", true)[0];
                        double b = Double.Parse(textbox_b.Text);
                        TextBox textbox_h = (TextBox)stf.Controls.Find("textbox_h", true)[0];
                        double h = Double.Parse(textbox_h.Text);
                        stirrup_data = new StirrupData2(b, h);
                        break; 
                    }
                case TYPE.TYPE3:
                    {
                        break;
                    }
                case TYPE.TYPE4:
                    {
                        break;
                    }
                case TYPE.TYPE5:
                    {
                        break;
                    }
                case TYPE.TYPE6:
                    {
                        TextBox textbox_d = (TextBox)stf.Controls.Find("textbox_d", true)[0];
                        double d = Double.Parse(textbox_d.Text);
                        stirrup_data = new StirrupData6(d);
                        break;
                    }
                case TYPE.TYPE7:
                    {
                        TextBox textbox_d = (TextBox)stf.Controls.Find("textbox_d", true)[0];
                        double d = Double.Parse(textbox_d.Text);
                        stirrup_data = new StirrupData7(d);
                        break;
                    }
            }
            return stirrup_data;
        }
    }
}
