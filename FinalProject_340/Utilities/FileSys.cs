using System.Drawing.Imaging;
using System.Drawing;

namespace FinalProject_340.Utilities
{
    public static class FileSys
    {
        public static void saveAlbumArt(IFormFile file, string UUID, string HASH)
        {
            if (!Directory.Exists("wwwroot/resources/" + UUID + "/art/"))
                Directory.CreateDirectory("wwwroot/resources/" + UUID + "/art/");
            resizeImage(file, "wwwroot/resources/" + UUID + "/art/" + HASH + "30x30.jpg", 30, 30);
            resizeImage(file, "wwwroot/resources/" + UUID + "/art/" + HASH + "100x100.jpg", 100, 100);
            resizeImage(file, "wwwroot/resources/" + UUID + "/art/" + HASH + "500x500.jpg", 500, 500);
        }
        public static void resizeImage(IFormFile inputFile, string outputFilePath, int newWidth, int newHeight)
        {
            using (var image = new Bitmap(inputFile.OpenReadStream()))
            {
                var resizedImage = new Bitmap(newWidth, newHeight);
                using (var graphics = Graphics.FromImage(resizedImage))
                {
                    graphics.DrawImage(image, 0, 0, newWidth, newHeight);
                }
                resizedImage.Save(outputFilePath, ImageFormat.Jpeg);
            }
        }
        public static async Task<int> SaveFileAsync(IFormFile file, string folderPath, string fileName)
        {
            // If directory does not exist, create it
            if (!Directory.Exists(folderPath))
            {
                Directory.CreateDirectory(folderPath);
            }
            // Combine the folder path and the file name to get the full file path
            var filePath = Path.Combine(folderPath, fileName);

            // Open a FileStream object to write the file to disk
            using (var stream = new FileStream(filePath, FileMode.Create))
            {
                // Copy the contents of the uploaded file to the FileStream
                await file.CopyToAsync(stream);
            }

            // Return the file name
            return 0;
        }
        public static int SaveFile(IFormFile file, string folderPath, string fileName)
        {
            // If directory does not exist, create it
            if (!Directory.Exists(folderPath))
            {
                Directory.CreateDirectory(folderPath);
            }
            // Combine the folder path and the file name to get the full file path
            var filePath = Path.Combine(folderPath, fileName);

            // Log a message to indicate that the SaveFile method is called and provide the file path
            Console.WriteLine("SaveFile method called. Saving file to: " + filePath);

            // Open a FileStream object to write the file to disk
            using (var stream = new FileStream(filePath, FileMode.Create))
            {
                // Copy the contents of the uploaded file to the FileStream
                file.CopyTo(stream);
            }
            return 0;
        }
    }
}
