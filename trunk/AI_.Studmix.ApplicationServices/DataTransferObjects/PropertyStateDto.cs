namespace AI_.Studmix.ApplicationServices.DataTransferObjects
{
    public class PropertyStateDto
    {
        public int Key { get; set; }
        public string Value { get; set; }

        public PropertyStateDto(int key, string value)
        {
            Key = key;
            Value = value;
        }
    }
}