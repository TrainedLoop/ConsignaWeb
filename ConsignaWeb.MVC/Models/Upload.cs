using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;


namespace ConsignaWeb.MVC.Models
{
    public class Upload
    {

        string HTTPHost = "http://" + HttpContext.Current.Request.Url.Host;
        //Directorys
        string BaseProductsDirectory = "/UploadedImages";
        //Sizes
        Size ProductImageSize = new Size { Width = 300, Height = 300 };
        public string ProductImage(HttpPostedFileBase file, int UserId)
        {
            var sourceDirectory = new DirectoryInfo(HttpContext.Current.Server.MapPath("/"));
            var filesDirectory = sourceDirectory + BaseProductsDirectory;
            Random rd = new Random();
            var filename = "/"+UserId + rd.Next(int.MinValue, int.MaxValue).ToString() + Path.GetExtension(file.FileName);

            if (!Directory.Exists(filesDirectory))
                Directory.CreateDirectory(filesDirectory);


            var image = Image.FromStream(file.InputStream);
            if (image.Height <= 300 && image.Width <= 300)
                image.Save(filesDirectory  + filename);

            else
                image.GetThumbnailImage(ProductImageSize.Width, ProductImageSize.Height, null, IntPtr.Zero).Save(filesDirectory + "/" + filename);

            return HTTPHost + BaseProductsDirectory + filename;
        }
    }

}