using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Diji_CS.Datas.StirrupData
{
    class StirrupTypeData2 : IStirrupTypeData
    {
        public StirrupTypeData2()
        {
 
        }
        public StirrupTypeData2(double b, double h, int m, int n)
        {
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
