using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Diji_CS.Datas.StirrupData
{
    class StirrupTypeData3 : IStirrupTypeData
    {
        public StirrupTypeData3()
        {
            this.Stirrup_type = TYPE.TYPE3;
            this.B = 800;
            this.H = 800;
        }
        public StirrupTypeData3(double b, double h, double b1, double h1)
        {
            this.Stirrup_type = TYPE.TYPE3;
            this.B = b;
            this.H = h;
            this.B1 = b1;
            this.H1 = h1;
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

        double b1; //b边上八边形边长
        public double B1
        {
            get { return b1; }
            set { b1 = value; }
        }

        double h1; //h边上八边形边长
        public double H1
        {
            get { return h1; }
            set { h1 = value; }
        }

    }
}
