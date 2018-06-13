using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Diji_CS.Datas.StirrupData
{
    class StirrupData6 : IStirrupData
    {
        //圆形箍筋的搭接长度应大于等于laE（受拉钢筋抗震锚固长度）
        public StirrupData6()
        {
            this.Stirrup_type = TYPE.TYPE6;
            this.D = 800;
        }
        public StirrupData6(double d)
        {
            this.Stirrup_type = TYPE.TYPE6;
            this.D = d;
        }
        double d;
        public double D
        {
            get { return d; }
            set { d = value; }
        }
    }
}
