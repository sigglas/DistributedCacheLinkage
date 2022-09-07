using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace SampleWeb.Models.Data
{
    public class Icon
    {
        [Key]
        public int Id { get; set; }
        public string Content { get; set; }
    }
}
