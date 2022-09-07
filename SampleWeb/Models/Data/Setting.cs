using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SampleWeb.Models.Data
{
    public class Setting
    {
        public string Code { get; set; }
        public string Value { get; set; }
    }

    public class SettingRequest
    {
        public string Code { get; set; }

    }
}
