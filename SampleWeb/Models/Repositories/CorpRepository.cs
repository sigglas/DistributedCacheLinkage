using DistributedCacheLinkage.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;

namespace SampleWeb.Models.Repositories
{
    public class CorpRepository : IEntityRepository<Corp, int>
    {
        public static List<Corp> items = new List<Corp>();
        private static int lastKeyId = 0;
        public CorpRepository()
        {
            if (items.Count == 0)
            {
                //demo default data
                items.Add(new Corp() { CorpId = 0, CorpName = "CorpA" });
                items.Add(new Corp() { CorpId = 1, CorpName = "CorpB" });
                items.Add(new Corp() { CorpId = 2, CorpName = "CorpC" });
                items.Add(new Corp() { CorpId = 3, CorpName = "CorpD" });
                items.Add(new Corp() { CorpId = 4, CorpName = "CorpE" });
                lastKeyId = 4;
            }
        }

        public Corp Add(Corp obj)
        {
            lastKeyId++;
            obj.CorpId = lastKeyId;
            items.Add(obj);

            return obj;
        }

        public void Dispose()
        {
        }

        public IList<Corp> GetEntities(Expression<Func<Corp, bool>> expression)
        {
            var result = new List<Corp>();
            if (expression != null)
                result = items.AsQueryable().Where(expression).ToList();
            else
                result = items.ToList();

            Thread.Sleep(3000);
            return result;
        }

        public bool Remove(int id)
        {
            var item = items.FirstOrDefault(f => f.CorpId == id);
            if (item != null)
            {
                items.Remove(item);
                return true;
            }
            else
                return false;
        }

        public bool Update(Corp obj)
        {
            var item = items.FirstOrDefault(f => f.CorpId == obj.CorpId);
            if (item != null)
            {
                item.CorpName = obj.CorpName;
                return true;
            }
            else
            {
                return false;
            }
        }
    }
}
