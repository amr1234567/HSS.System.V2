using FluentResults;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Hosting;
using HSS.System.V2.Domain.ResultHelpers.Errors;

namespace HSS.System.V2.Domain.Helpers.Methods
{
    public class UploadFiles
    {
        public static async Task<Result<string>> Upload(IFormFile file, string webRootPath, string folderName)
        {
            try
            {
                if (file == null || file.Length == 0)
                    return Result.Fail("File is Empty or Null");

                var folderPath = Path.Combine(webRootPath, folderName);
                if (!Directory.Exists(folderPath))
                    Directory.CreateDirectory(folderPath);

                var fileName = Guid.NewGuid() + Path.GetExtension(file.FileName);
                var filePath = Path.Combine(folderPath, fileName);

                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    await file.CopyToAsync(stream);
                }

                return Result.Ok(folderName + "/" + fileName);
            }
            catch (Exception ex)
            {
                return new UnKnownError(ex);
            }
        }
    }
}
