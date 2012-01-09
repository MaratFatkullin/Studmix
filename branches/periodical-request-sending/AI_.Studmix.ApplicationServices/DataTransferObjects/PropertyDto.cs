namespace AI_.Studmix.ApplicationServices.DataTransferObjects
{
    public class PropertyDto
    {
        public int ID { get; set; }

        public string Name { get; set; }

        public PropertyDto(int id, string name)
        {
            ID = id;
            Name = name;
        }
    }
}