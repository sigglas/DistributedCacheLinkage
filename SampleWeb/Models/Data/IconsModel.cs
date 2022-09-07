using DistributedCacheLinkage.Objects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SampleWeb.Models.Data
{
    public class IconsModel : IEntityMany<Icon, int>
    {
        public static List<Icon> items = new List<Icon>();
        public IconsModel()
        {
            if (items.Count == 0)
            {
                //demo default data
                items.Add(new Icon() { Id = 1, Content = "CorpA" });
                items.Add(new Icon() { Id = 2, Content = "CorpB" });
            }
        }

        public int RetentionSecond => (1 * 60);

        public void Dispose()
        {
        }

        public Icon GetObject(int key)
        {
            return items.Find(f => f.Id == key);
        }

        public bool PutObject(Icon entity, int key)
        {
            if (items.Any(f => f.Id == key))
            {
                return false;
            }
            else
            {
                items.Add(entity);
                return true;
            }
        }
    }
}
