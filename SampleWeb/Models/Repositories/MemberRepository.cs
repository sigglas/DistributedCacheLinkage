using DistributedCacheLinkage.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;

namespace SampleWeb.Models.Repositories
{
    public class MemberRepository : IEntityRepository<Member, int>
    {
        List<Member> items = new List<Member>();
        private static int lastKeyId = 0;
        public MemberRepository()
        {
            if (items.Count == 0)
            {
                items.Add(new Member() { MemberId = 0, MemberName = "A" });
                items.Add(new Member() { MemberId = 1, MemberName = "B" });
                items.Add(new Member() { MemberId = 2, MemberName = "C" });
                items.Add(new Member() { MemberId = 3, MemberName = "D" });
                items.Add(new Member() { MemberId = 4, MemberName = "E" });
                lastKeyId = 4;
            }
        }

        public Member Add(Member obj)
        {
            lastKeyId++;
            obj.MemberId = lastKeyId;
            items.Add(obj);

            return obj;
        }

        public void Dispose()
        {
        }

        public IList<Member> GetEntities(Expression<Func<Member, bool>> expression)
        {
            var result = new List<Member>();
            if (expression != null)
                result = items.AsQueryable().Where(expression).ToList();
            else
                result = items.ToList();

            Thread.Sleep(3000);
            return result;
        }

        public bool Remove(int id)
        {
            var item = items.FirstOrDefault(f => f.MemberId == id);
            if (item != null)
            {
                items.Remove(item);
                return true;
            }
            else
                return false;
        }

        public bool Update(Member obj)
        {
            var item = items.FirstOrDefault(f => f.MemberId == obj.MemberId);
            if (item != null)
            {
                item.MemberName = obj.MemberName;
                return true;
            }
            else
            {
                return false;
            }
        }
    }
}
