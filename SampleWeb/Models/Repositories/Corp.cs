using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace SampleWeb.Models.Repositories
{
    public class Corp
    {
        [Key]
        public int CorpId { get; set; }
        public string CorpName { get; set; }
    }
}
