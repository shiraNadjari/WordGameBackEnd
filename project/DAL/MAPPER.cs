using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using COMMON;

namespace DAL
{
    class MAPPER
    {
        //image
        public static Images_tbl ConvertCOMimageToDALimage(COMimage image)
        {
            Images_tbl img = new Images_tbl();
            img.CategoryID = image.CategoryID;
            img.ImageID = image.ImageID;
            img.URL = image.URL;
            img.UserId = image.UserId;
            img.BeginIndex = image.BeginIndex;
            img.EndIndex = image.EndIndex;
            return img;
        }

        public static COMimage ConvertDALimageToCOMimage(Images_tbl image)
        {
            COMimage img = new COMimage();
            img.CategoryID = image.CategoryID;
            img.ImageID = image.ImageID;
            img.URL = image.URL;
            img.UserId = image.UserId;
            img.BeginIndex = image.BeginIndex;
            img.EndIndex = image.EndIndex;
            return img;
        }

        public static List<Images_tbl> ConvertListCOMimageToListDALimage(List<COMimage> images)
        {
            List<Images_tbl> imgs = new List<Images_tbl>();
            foreach (COMimage img in images)
            {
                imgs.Add(ConvertCOMimageToDALimage(img));
            }
            return imgs;
        }

        public static List<COMimage> ConvertListDAlimageToListCOMimage(List<Images_tbl> images)
        {
            List<COMimage> imgs = new List<COMimage>();
            foreach (Images_tbl img in images)
            {
                imgs.Add(ConvertDALimageToCOMimage(img));
            }
            return imgs;
        }

        ////////objects
        public static Objects_tbl ConvertCOMobjectToDALobject(COMimageObject obj)
        {
            Objects_tbl o = new Objects_tbl();
            o.ObjectID = obj.ObjectId;
            o.ImageID = obj.ImageID;
            o.Name = obj.Name;
            o.X1 = obj.X1;
            o.X2 = obj.X2;
            o.Y1 = obj.Y1;
            o.Y2 = obj.Y2;
            o.X3 = obj.X3;
            o.Y3 = obj.Y3;
            o.X4 = obj.X4;
            o.Y4 = obj.Y4;
            o.VoiceURL = obj.VoiceURL;
            return o;
        }

        public static COMimageObject ConvertDALobjectToCOMobject(Objects_tbl obj)
        {
            COMimageObject o = new COMimageObject();
            o.ObjectId = obj.ObjectID;
            o.ImageID = Convert.ToInt32(obj.ImageID);
            o.Name = obj.Name;
            o.X1 = obj.X1;
            o.X2 = obj.X2;
            o.Y1 = obj.Y1;
            o.Y2 = obj.Y2;
            o.X3 = obj.X3;
            o.Y3 = obj.Y3;
            o.X4 = obj.X4;
            o.Y4 = obj.Y4;
            o.VoiceURL = obj.VoiceURL;
            return o;
        }

        public static List<Objects_tbl> ConvertListCOMobjectToListDALobject(List<COMimageObject> objects)
        {
            List<Objects_tbl> objs = new List<Objects_tbl>();
            foreach (COMimageObject obj in objects)
            {
                objs.Add(ConvertCOMobjectToDALobject(obj));
            }
            return objs;
        }

        public static List<COMimageObject> ConvertListDALobjectToListCOMobject(List<Objects_tbl> objects)
        {
            List<COMimageObject> objs = new List<COMimageObject>();
            foreach (Objects_tbl obj in objects)
            {
                objs.Add(ConvertDALobjectToCOMobject(obj));
            }
            return objs;
        }

        ////////////
        ///
        public static Categories_tbl ConvertCOMcategoryToDALcategory(COMCategory category)
        {
            Categories_tbl cat = new Categories_tbl();
            cat.CategoryID = category.CategoryId;
            cat.CategoryName = category.CategoryName;
            cat.ImageURL = category.ImageURL;
            return cat;
        }

        public static COMCategory ConvertDALcategoryToCOMcategory(Categories_tbl category)
        {
            COMCategory cat = new COMCategory();
            cat.CategoryId = category.CategoryID;
            cat.CategoryName = category.CategoryName;
            cat.ImageURL = category.ImageURL;
            return cat;
        }

        public static List<Categories_tbl> ConvertListCOMcategoryToListDALcategory(List<COMCategory> categories)
        {
            List<Categories_tbl> cats = new List<Categories_tbl>();
            foreach (COMCategory cat in categories)
            {
                cats.Add(ConvertCOMcategoryToDALcategory(cat));
            }
            return cats;
        }

        public static List<COMCategory> ConvertListDALcategoryToListCOMcategory(List<Categories_tbl> categories)
        {
            List<COMCategory> cats = new List<COMCategory>();
            foreach (Categories_tbl cat in categories)
            {
                cats.Add(ConvertDALcategoryToCOMcategory(cat));
            }
            return cats;
        }


        //user
        public static Users_tbl ConvertCOMuserToDALuser(COMuser user)
        {
            Users_tbl u = new Users_tbl();
            u.UserId = user.UserId;
            u.CategoryName = user.CategoryName;
            u.Email = user.Email;
            u.Password = user.Password;
            return u;
        }

        public static COMuser ConvertDALuserToCOMuser(Users_tbl user)
        {
            COMuser u = new COMuser();
            u.UserId = user.UserId;
            u.CategoryName = user.CategoryName;
            u.Email = user.Email;
            u.Password = user.Password;
            return u;
        }

        public static List<Users_tbl> ConvertListCOMuserToListDALuser(List<COMuser> users)
        {
            List<Users_tbl> us = new List<Users_tbl>();
            foreach (COMuser u in users)
            {
                us.Add(ConvertCOMuserToDALuser(u));
            }
            return us;
        }

        public static List<COMuser> ConvertListDALuserToListCOMuser(List<Users_tbl> users)
        {
            List<COMuser> us = new List<COMuser>();
            foreach (Users_tbl u in users)
            {
                us.Add(ConvertDALuserToCOMuser(u));
            }
            return us;
        }
    }
}
