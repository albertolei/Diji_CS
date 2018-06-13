using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Diji_CS.Datas.StirrupData
{
    //圆形箍筋的搭接长度应大于等于laE（受拉钢筋抗震锚固长度）
    class StirrupData4 : IStirrupData
    {
        public StirrupData4() 
        {
            this.Stirrup_type = TYPE.TYPE4;
            this.B = 800;
            this.H = 800;
        }
        public StirrupData4(double b, double h)
        {
            this.Stirrup_type = TYPE.TYPE4;
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
