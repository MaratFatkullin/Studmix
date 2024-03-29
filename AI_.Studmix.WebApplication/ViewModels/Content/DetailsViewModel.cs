﻿using System.Collections.Generic;
using AI_.Studmix.ApplicationServices.DataTransferObjects;

namespace AI_.Studmix.WebApplication.ViewModels.Content
{
    public class DetailsViewModel
    {
        public ContentPackageDto Package { get; set; }

        public IEnumerable<PropertyDto> Properties { get; set; }

        public bool IsFullAccessGranted { get; set; }
    }
}