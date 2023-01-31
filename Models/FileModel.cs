namespace EDUZilla.Models
{
    public class FileModel
    {
        public int Id { get; set; }
        public string FileName { get; set; }
        public byte[] Data { get; set; }
        public string FileDescription { get; set; }
        public virtual Course? Course { get; set; }
    }
}
