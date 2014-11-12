using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using SSQDao;

namespace Analysis
{
   public abstract class Analysis
   {
       public abstract SSQFrom GetSSQFrom();


       public abstract int GetTotalPageCount();
   }

}
