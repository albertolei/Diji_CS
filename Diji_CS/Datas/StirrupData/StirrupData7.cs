using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Diji_CS.Datas.StirrupData
{
    class StirrupData7:IStirrupData
    {
        public StirrupData7()
        {
            this.Stirrup_type = TYPE.TYPE7;
            this.D = 800;
        }
        public StirrupData7(double d)
        {
            this.Stirrup_type = TYPE.TYPE7;
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
