namespace OnlineCourseD2N.Backend.Handler
{
    public class UploadHandler
    {
        public string Upload(IFormFile file)
        {
            if (file == null || file.Length == 0)
            {
                return "Tidak ada file yang diupload!";
            }

            // valid extension
            List<string> validExtentions = new List<string>() { ".jpg", ".png", ".gif" };
            string extention = Path.GetExtension(file.FileName).Trim();
            if (!validExtentions.Contains(extention))
            {
                return $"Extention file tidak valid ({string.Join(',', validExtentions)})";
            }

            // valid size
            long size = file.Length;
            if (size > (10 * 1024 * 1024))
            {
                return "File yang diupload tidak boleh lebih dari 10mb";
            }

            // nama file
            string fileName = Guid.NewGuid().ToString() + extention;

            // nama path
            string path = Path.Combine(Directory.GetCurrentDirectory(), "Uploads");

            using FileStream stream = new FileStream(Path.Combine(path, fileName), FileMode.Create);
            file.CopyTo(stream);

            return fileName;
        }
    }
}
