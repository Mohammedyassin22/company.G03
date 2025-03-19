using static NuGet.Packaging.PackagingConstants;

namespace company.G03.PL.Helper
{
    public class DocumentSetting
    {
        public static string UploadFile(IFormFile file, string FolderName)
        {
            var folder = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "files", FolderName);

            var filename = $"{Guid.NewGuid()}{file.FileName}";
            var filepath=Path.Combine(folder, filename);
            using var filestream=new FileStream(filepath,FileMode.Create);
            file.CopyTo(filestream);
            return filename;
        } 
        public static void Delete(string filename,string foldername)
        {
            var filepath = Path.Combine(Directory.GetCurrentDirectory(), @"wwwroot\files", foldername, filename);
            if(File.Exists(filepath))
            {
                File.Delete(filepath); 
            }
        }
    }
}
