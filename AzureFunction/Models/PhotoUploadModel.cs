namespace PhotoStorageIsolated.Models
{
    public class PhotoUploadModel
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public string[] Tags { get; set; }
        public string Photo { get; set; }
    }
}
