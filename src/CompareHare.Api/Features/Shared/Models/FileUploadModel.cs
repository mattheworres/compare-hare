namespace CompareHare.Api.Features.Shared.Models
{
    public class FileUploadModel
    {
        public FileUploadModel(string fileName, string fileContents)
        {
            FileName = fileName;
            FileContents = fileContents;
        }

        public int? Id { get; set; }
        public string FileName { get; set; }
        public string FileContents { get; set; }
    }
}
