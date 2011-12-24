using System;
using System.Collections.Generic;

namespace AI_.Studmix.ApplicationServices.DataTransferObjects
{
    public class ContentPackageDto
    {
        public int ID { get; set; }

        public DateTime CreateDate { get; set; }

        public IEnumerable<PropertyStateDto> PropertyStates { get; set; }

        public IEnumerable<ContentFileDto> Files { get; set; }

        public string Caption { get; set; }

        public string Description { get; set; }

        public decimal Price { get; set; }
    }
}