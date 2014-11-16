using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Analysis
{
   public class RegularConst
   {
       /// <summary>
       /// 红球定胆
       /// </summary>
       public static readonly string FixedNumber =
           @"(\((((0[1-9]\s)|(1[0-9]\s)|(2[0-9]\s)|(3[0-3]\s)(?!\1))){1,}((0[1-9])|(1[0-9])|(2[0-9])|(3[0-3]))\))";
   }
}
