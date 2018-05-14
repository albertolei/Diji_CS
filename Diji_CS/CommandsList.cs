using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;

using Bentley.Interop.MicroStationDGN;
using Bentley.Interop.TFCom;
using Bentley.MicroStation.InteropServices;

namespace Diji_CS
{
    class CommandsList
    {
        public static Bentley.Interop.MicroStationDGN.Application app = Utilities.ComApp;
        
        public static void test(string unparsed)
        {
            app.CommandState.StartDefaultCommand();
            app.CommandState.StartPrimitive(new RCCommand());
        }
    }
}
