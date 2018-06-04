using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Diji_CS.Datas.StirrupData
{
    enum TYPE {TYPE1, TYPE2, TYPE3, TYPE4, TYPE5, TYPE6, TYPE7 };
    abstract class IStirrupTypeData
    {
        TYPE stirrup_type;
        public TYPE Stirrup_type
        {
            get { return stirrup_type; }
            set { stirrup_type = value; }
        }
    }
}
