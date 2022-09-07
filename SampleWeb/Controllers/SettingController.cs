using DistributedCacheLinkage.Objects;
using Microsoft.AspNetCore.Mvc;
using SampleWeb.Models.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SampleWeb.Controllers
{
    [Route("api/[controller]")]
    public class SettingController : ControllerBase
    {
        private readonly OneObjectProxy<List<Setting>> oneObjectProxy;

        public SettingController(OneObjectProxy<List<Setting>> oneObjectProxy)
        {
            this.oneObjectProxy = oneObjectProxy;
        }

        [HttpGet]
        public string Get()
        {
            var settings = oneObjectProxy.GetObject();
             return Newtonsoft.Json.JsonConvert.SerializeObject(settings);
        }


        [HttpPut]
        public bool Put()
        {
            var settings = oneObjectProxy.GetObject();
            settings.Add(new Setting { Code = "aaa", Value = "bbb" });
            var tf = oneObjectProxy.PutObject(settings);
            return tf;
        }
    }
}
