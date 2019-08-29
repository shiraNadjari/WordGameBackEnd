using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace COMMON
{
  public  class COMimage
    {
        public int ImageID { get; set; }
        public string URL { get; set; }
        public int CategoryID { get; set; }
        public int BeginIndex { get; set; }
        public int EndIndex { get; set; }
        public int UserId { get; set; }
    }
}
