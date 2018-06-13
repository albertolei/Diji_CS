using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Diji_CS.Datas.StirrupData
{
    class StirrupData5:IStirrupData
    {
        //圆形箍筋的搭接长度应大于等于laE（受拉钢筋抗震锚固长度）
        public StirrupData5()
        {
            this.Stirrup_type = TYPE.TYPE5;
            this.H = 800;
            this.B = 800;
        }
        public StirrupData5(double b, double h)
        {
            this.Stirrup_type = TYPE.TYPE5;
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

        double m;
        public double M
        {
            get { return m; }
            set { m = value; }
        }

        double n;
        public double N
        {
            get { return n; }
            set { n = value; }
        }

    }
}
