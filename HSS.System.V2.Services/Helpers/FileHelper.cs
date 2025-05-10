using Microsoft.AspNetCore.Hosting.Internal;
using Microsoft.AspNetCore.Http;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HSS.System.V2.Services.Helpers
{
    internal class FileHelper
    {
        //public static string Upload(IFormFile Photo, string FileName = "Images")
        //{
        //    string uniqueFileName = string.Empty;
        //    if (Photo != null)
        //    {
        //        string uploadsFolder = Path.Combine(HostingEnvironment.WebRootPath, $"images/{folderName}");
        //        if (!Directory.Exists(uploadsFolder))
        //            Directory.CreateDirectory(uploadsFolder);

        //        uniqueFileName = Guid.NewGuid().ToString().Replace("-", "") + Path.GetExtension(Photo.FileName);
        //        string filePath = Path.Combine(uploadsFolder, uniqueFileName);
        //        using (var fileStream = new FileStream(filePath, FileMode.Create))
        //        {
        //            Photo.CopyTo(fileStream);
        //        }
        //    }
        //    return $"images/{folderName}/" + uniqueFileName;
        //}
    }
}
