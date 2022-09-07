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
    public class IconController : ControllerBase
    {
        private readonly ManyObjectProxy<Icon, int> manyObjectProxy;

        public IconController(ManyObjectProxy<Icon, int> manyObjectProxy)
        {
            this.manyObjectProxy = manyObjectProxy;
        }

        [HttpGet("{id}")]
        public string Get(int id)
        {
            var settings = manyObjectProxy.GetObject(id);
            return Newtonsoft.Json.JsonConvert.SerializeObject(settings);
        }


        [HttpPut]
        public bool Put()
        {
            var obj = new Icon { Id = 3, Content = "bbb" };
            var tf = manyObjectProxy.PutObject(obj, obj.Id);
            return tf;
        }
    }
}
