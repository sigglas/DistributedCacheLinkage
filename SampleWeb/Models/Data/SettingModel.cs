using DistributedCacheLinkage.Objects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SampleWeb.Models.Data
{
    public class SettingModel : IEntityOne<List<Setting>>
    {

        public static List<Setting> items = new List<Setting>();
        public SettingModel()
        {
            if (items.Count == 0)
            {
                //demo default data
                items.Add(new Setting() { Code = "Code1", Value = "CorpA" });
                items.Add(new Setting() { Code = "Code2", Value = "CorpB" });
            }
        }

        public int RetentionSecond => (1 * 60);

        public void Dispose()
        {
        }

        public List<Setting> GetObject()
        {
            return items;
        }

        public List<Setting> GetObject<TReq>(TReq expression) where TReq : class
        {
            var condition = expression as SettingRequest;
            return items.Where(w => w.Code == condition.Code).ToList();
        }

        public bool PutObject(List<Setting> entity)
        {
            try
            {
                items.AddRange(entity);
                return true;
            }
            catch
            {
                return false;
            }
        }
    }
}
