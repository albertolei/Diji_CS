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
            Element column, foundation, foundation_rebars, column_longitudinal_rebars, column_stirrups;
            column = RCUtil.create_column(((StirrupData1)stirrupData).B, ((StirrupData1)stirrupData).H, 1500, 400);
            foundation = RCUtil.create_foundation(1500, 1500, 400);


            //column_longitudinal_rebars = LongitudinalBarUtil.create_column_longitudinal_rebars(800, 800, 1500, 1500, 1500, 400, 50, "2");
            //column_stirrups = StirrupUtil.create_column_stirrups(800, 800, 1500, 400, "2");
            //foundation_rebars = FootingSlabBarUtil.create_foundation_rebars(1500, 1500, 400);

            TFPartRef tfpart_ref = TFPartUtil.create_tfpart_ref("Ceiling", "Metal");
            TFPartUtil.add_part_to_element(ref column, tfpart_ref);
            TFPartUtil.add_part_to_element(ref foundation, tfpart_ref);
            //TFPartUtil.add_part_to_element(ref column_longitudinal_rebars, tfpart_ref);
            //TFPartUtil.add_part_to_element(ref column_stirrups, tfpart_ref);
            //TFPartUtil.add_part_to_element(ref foundation_rebars, tfpart_ref);
            column.Move(ref Point);
            foundation.Move(ref Point);
            //column_longitudinal_rebars.Move(ref Point);
            //column_stirrups.Move(ref Point);
            //foundation_rebars.Move(ref Point);

            List<Element> elements = new List<Element>();
            elements.Add(column);
            elements.Add(foundation);
            //elements.Add(column_longitudinal_rebars);
            //elements.Add(column_stirrups);
            //elements.Add(foundation_rebars);

            draw_Elements(elements.ToArray(), "diji");


            TextBox textbox_b = (TextBox)stf.Controls.Find("textbox_b", true)[0];
            string text_b = textbox_b.Text;
            MessageBox.Show(text_b);
        }

        public void Dynamics(ref Point3d Point, Bentley.Interop.MicroStationDGN.View View, MsdDrawingMode DrawMode)
        {
            TextBox textbox_b = (TextBox)stf.Controls.Find("textbox_b", true)[0];
            double b = Double.Parse(textbox_b.Text);
            TextBox textbox_h = (TextBox)stf.Controls.Find("textbox_h", true)[0];
            double h = Double.Parse(textbox_h.Text);
            ComboBox combobox_m = (ComboBox)stf.Controls.Find("combobox_m", true)[0];
            int m = int.Parse(combobox_m.SelectedItem.ToString());
            ComboBox combobox_n = (ComboBox)stf.Controls.Find("combobox_n", true)[0];
            int n = int.Parse(combobox_n.SelectedItem.ToString());
            stirrupData = new StirrupData1(b, h, m, n);


            //动态移动的时候不显示配筋，只有点击的时候才画配筋
            Element column = RCUtil.create_column(b, h, 1500, 400), foundation = RCUtil.create_foundation(1500, 1500, 400);
            //Element column = RCUtil.create_column(800, 1500, 400), foundation = RCUtil.create_foundation(1500, 1500, 400);
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
    }
}
