using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using static System.Net.Mime.MediaTypeNames;

namespace mproject.Models
{
    public class Images
    {
        public int Id { get; set; }
        public string ImageName { get; set; }
        public string FileType { get; set; }
        public string Extension { get; set; }
        public string UploadedBy { get; set; }
        public DateTime? CreatedOn { get; set; }
    }
    public class ImageonDBModel : Images
    {
        public byte[] Data { get; set; }
    }
}
