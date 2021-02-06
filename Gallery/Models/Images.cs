using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Gallery.Models
{
    public class Images
    {
        [Key]
        public int Id { get; set; }
        public string ImageName { get; set; }
        public string Path { get; set; }
        public string CreatedBy { get; set; }
        public DateTime CreatedDate { get; set; }
    }
}
