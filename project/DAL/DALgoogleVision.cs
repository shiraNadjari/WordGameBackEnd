using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using COMMON;

namespace DAL
{
    public class DALgoogleVision
    {
        public static void InsertImage(COMimage img)
        {
            using (DBEntities context = new DBEntities())
            {
                context.Images_tbl.Add(MAPPER.ConvertCOMimageToDALimage(img));
            }
        }

        public static void InsertObjects(COMimageObject obj)
        {

        }
    }
}
