using DistributedCacheLinkage.Repositories;
using Microsoft.AspNetCore.Mvc;
using SampleWeb.Models.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SampleWeb.Controllers
{
    [Route("api/[controller]")]
    public class CorpController : ControllerBase
    {
        private readonly DistributedCacheLinkage.Repositories.RepositoryProxy<Corp, int> repositoryProxy;

        public CorpController(RepositoryProxy<Corp, int> repositoryProxy)
        {
            this.repositoryProxy = repositoryProxy;
        }

        [HttpGet]
        public JsonResult Get()
        {
            var corps = repositoryProxy.GetEntities();
            return new JsonResult(corps);
        }


        [HttpGet("{id}")]
        public JsonResult Get(int id)
        {
            var corp = repositoryProxy.GetEntities(item => item.CorpId == id).FirstOrDefault();
            return new JsonResult(corp);
        }


        [HttpPost]
        public JsonResult Post([FromBody] string value)
        {
            var obj = repositoryProxy.Add(new Corp { CorpName = value });
            return new JsonResult(obj);
        }

        [HttpPut("{id}")]
        public bool Put(int id, [FromBody] string value)
        {
            return repositoryProxy.Update(new Corp { CorpId = id, CorpName = value });
        }

        [HttpDelete("{id}")]
        public bool Delete(int id)
        {
            return repositoryProxy.Remove(id);
        }
    }
}
