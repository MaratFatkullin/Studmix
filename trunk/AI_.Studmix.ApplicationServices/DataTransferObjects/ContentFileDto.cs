namespace AI_.Studmix.ApplicationServices.DataTransferObjects
{
    public class ContentFileDto
    {
        public string Name { get; set; }
        public int ID { get; set; }
        public bool IsPreview { get; set; }

        public ContentFileDto(int id, string filename, bool isPreview)
        {
            ID = id;
            Name = filename;
            IsPreview = isPreview;
        }
    }
}