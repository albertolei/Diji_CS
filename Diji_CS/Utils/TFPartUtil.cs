﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Bentley.Interop.MicroStationDGN;
using Bentley.MicroStation.InteropServices;
using Bentley.Interop.TFCom;

namespace Diji_CS.Utils
{
    class TFPartUtil
    {
        private static Bentley.Interop.MicroStationDGN.Application app = Utilities.ComApp;
        
        //为元素添加样式
        public static void add_part_to_element(ref Element element, TFPartRef tfpart_ref)
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
        public static TFPartRef create_tfpart_ref(string family, string part)
        {
            TFPartRefList tfpart_ref_list = new TFPartRefList();
            TFPartRef tfpart_ref = tfpart_ref_list.AsTFPartRef;
            tfpart_ref.SetPartFamilyName(family);
            tfpart_ref.SetPartName(part);
            return tfpart_ref;
        }
    }
}
