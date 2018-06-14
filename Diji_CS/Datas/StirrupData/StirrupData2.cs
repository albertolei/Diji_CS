using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Diji_CS.Datas.StirrupData
{
    class StirrupData2 : IStirrupData
    {
        public StirrupData2()
        {
            this.Stirrup_type = TYPE.TYPE2;
            this.B = 800;
            this.H = 800;

        }
        public StirrupData2(double b, double h)
        {
            this.Stirrup_type = TYPE.TYPE2;
            this.B = b;
            this.H = h;
        }

        double b;
        public double B
        {
            get { return b; }
            set { b = value; }
        }

        double h;
        public double H
        {
            get { return h; }
            set { h = value; }
        }
    }
}
