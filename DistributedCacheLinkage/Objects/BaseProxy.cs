using DistributedCacheLinkage.Package;
using Microsoft.Extensions.Caching.Distributed;
using System;
using System.Collections.Generic;
using System.Text;

namespace DistributedCacheLinkage.Objects
{
    public abstract class BaseProxy
    {
        protected readonly string cacheId;
        protected readonly IDistributedCache distributed;
        private readonly int retentionSecond;

        protected BaseProxy(IDistributedCache distributed, string cacheId, int retentionSecond = 900)
        {
            this.distributed = distributed;
            this.cacheId = cacheId;
            this.retentionSecond = retentionSecond;
        }


        /// <summary>
        /// 清除暫存
        /// </summary>
        public void MemoryClear()
        {
            distributed.Remove(cacheId);
        }
        /// <summary>
        /// 重置暫存時間
        /// </summary>
        public void MemoryRefresh()
        {
            distributed.Refresh(cacheId);
        }

        protected TEntity GetEntityInMemory<TEntity>()
        {
            TEntity entity = default(TEntity);

            var memory = distributed.Get(cacheId);

            if (memory != null)
            {
                entity = Json.FromJsonString<TEntity>(System.Text.Encoding.UTF8.GetString(memory));
            }
            return entity;
        }

        protected TEntity GetEntityInMemory<TEntity, TEntityKey>(TEntityKey key)
        {
            TEntity entity = default(TEntity);

            var memory = distributed.Get($"{cacheId}_by_{key}");

            if (memory != null)
            {
                entity = Json.FromJsonString<TEntity>(System.Text.Encoding.UTF8.GetString(memory));
            }
            return entity;
        }

        protected void SetEntityInMemory(object entity)
        {
            if (entity == null)
                return;
            var bData = System.Text.Encoding.UTF8.GetBytes(Json.AsJsonString(entity));
            distributed.Set(cacheId, bData, new DistributedCacheEntryOptions
            {
                AbsoluteExpiration = DateTime.Now.AddSeconds(retentionSecond)
            });
        }

        protected void SetEntityInMemory<TEntityKey>(object entity, TEntityKey key)
        {
            if (entity == null)
                return;
            var bData = System.Text.Encoding.UTF8.GetBytes(Json.AsJsonString(entity));
            distributed.Set($"{cacheId}_by_{key}", bData, new DistributedCacheEntryOptions
            {
                AbsoluteExpiration = DateTime.Now.AddSeconds(retentionSecond)
            });
        }
    }
}
