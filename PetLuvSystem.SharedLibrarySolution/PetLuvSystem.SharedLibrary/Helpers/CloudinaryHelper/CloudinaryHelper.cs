using CloudinaryDotNet;
using CloudinaryDotNet.Actions;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;

namespace PetLuvSystem.SharedLibrary.Helpers.CloudinaryHelper
{
    public static class CloudinaryHelper
    {
        private static Cloudinary _cloudinary;

        public static void ConfigureCloudinary(IConfiguration configuration)
        {
            var settings = configuration.GetSection("CloudinarySettings").Get<CloudinarySettings>();

            if (settings == null)
            {
                throw new Exception("Cloudinary chưa được cấu hình");
            }

            var account = new Account(settings.CloudName, settings.ApiKey, settings.ApiSecret);
            _cloudinary = new Cloudinary(account);
        }

        public static async Task<string> UploadImageToCloudinary(IFormFile file, string subFolder)
        {
            if (_cloudinary == null)
            {
                throw new Exception("Cloudinary chưa được cấu hình");
            }

            using (var stream = file.OpenReadStream())
            {
                var uploadParams = new ImageUploadParams
                {
                    File = new FileDescription(file.FileName, stream),
		    Folder = $"PetLuv/{subFolder}"
                };

                var uploadResult = await _cloudinary.UploadAsync(uploadParams);

                if (uploadResult.StatusCode == System.Net.HttpStatusCode.OK)
                {
                    return uploadResult.SecureUrl.ToString();
                }
                else
                {
                    throw new Exception($"Upload ảnh thất bại. Lỗi: {uploadResult.Error.Message}");

                }
            }
        }

        public static async Task DeleteImageFromCloudinaryAsync(string publicId)
        {
            if (_cloudinary == null)
            {
                throw new Exception("Cloudinary chưa được cấu hình.");
            }

            var deletionParams = new DeletionParams(publicId);
            var deletionResult = await _cloudinary.DestroyAsync(deletionParams);
            if (deletionResult.Result != "ok")
            {
                throw new Exception("Xóa ảnh thất bại: " + deletionResult.Error.Message);
            }
        }
    }
}
