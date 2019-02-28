using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CodeJam;

namespace GoogleHashCode2019
{
    public class PointCalculator
    {
        public static int GetPoint(List<string> x, List<string> y)
        {
            int counterEqual = x.Count(a => y.Contains(a));
            if (counterEqual == 0)
            {
                return 0;
            }
            int counterXNotEqual = x.Count(a => !y.Contains(a));
            if (counterXNotEqual == 0)
            {
                return 0;
            }
            int counterYNotEqual = y.Count(a => !x.Contains(a));
            if (counterYNotEqual == 0)
            {
                return 0;
            }

            int result = Math.Min(counterEqual, counterXNotEqual);
            result = Math.Min(result, counterYNotEqual);

            return result;
        }

   /*     public GetPoints2()
        {
            CodeJam.        }

        public GetPoints3(){
        
                   
        }*/
        
    }
}
