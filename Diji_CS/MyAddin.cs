using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Diji_CS
{
    [Bentley.MicroStation.AddInAttribute(KeyinTree = "Diji_CS.commands.xml", MdlTaskID = "Diji_CS")]
    internal sealed class MyAddin : Bentley.MicroStation.AddIn
    {
        private MyAddin(System.IntPtr mdlDesc) : base(mdlDesc)
        {

        }
        protected override int Run(string[] commandLine)
        {
            return 0;
        }
    }
}
