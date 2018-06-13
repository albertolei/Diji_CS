using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Diji_CS.Datas.StirrupData
{
    class StirrupData1 : IStirrupData
    {
        public StirrupData1()
        {
            this.Stirrup_type = TYPE.TYPE1;
            this.B = 800;
            this.H = 800;
            this.M = 4;
            this.N = 4;
        }
        public StirrupData1(double b, double h, int m, int n)
        {
            this.Stirrup_type = TYPE.TYPE1;
            this.B = b;
            this.H = h;
            this.M = m;
            this.N = n;
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

        int m;
        public int M
        {
            get { return m; }
            set { m = value; }
        }

        int n;
        public int N
        {
            get { return n; }
            set { n = value; }
        }
        
    }
}
