using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Gallery.Models
{
    public class UploadViewModel
    {
        [Required(ErrorMessage = "Please select a file.")]
        [DataType(DataType.Upload)]
        public IFormFile Image { get; set; }
    }
}
