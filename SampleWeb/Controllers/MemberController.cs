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
    public class MemberController : ControllerBase
    {
        private readonly DistributedCacheLinkage.Repositories.RepositoryProxy<Member, int> repositoryProxy;

        public MemberController(RepositoryProxy<Member, int> repositoryProxy)
        {
            this.repositoryProxy = repositoryProxy;
        }

        [HttpGet]
        public string Get()
        {
            var member = repositoryProxy.GetEntities(item => item.MemberId == 0).FirstOrDefault();
            return member.MemberName;
        }
    }
}
