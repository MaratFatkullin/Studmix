using System;
using System.Collections.Generic;
using System.ComponentModel;

namespace AI_.Studmix.ApplicationServices.DataTransferObjects
{
    public class ContentPackageDto
    {
        public int ID { get; set; }

        public DateTime CreateDate { get; set; }

        public IEnumerable<PropertyStateDto> PropertyStates { get; set; }

        public IEnumerable<ContentFileDto> Files { get; set; }

        [DisplayName("Название")]
        public string Caption { get; set; }

        [DisplayName("Описание")]
        public string Description { get; set; }

        [DisplayName("Цена")]
        public decimal Price { get; set; }
    }
}