using Microsoft.Extensions.Caching.Distributed;
using System;
using System.Collections.Generic;
using System.Text;

namespace DistributedCacheLinkage.Objects
{
    /// <summary>
    /// 需要暫存特定資料集合時使用
    /// </summary>
    /// <typeparam name="TEntity"></typeparam>
    /// <typeparam name="TEntityKey"></typeparam>
    public class ManyObjectProxy<TEntity, TEntityKey>: BaseProxy
    {
        private readonly IEntityMany<TEntity, TEntityKey> entityMany;

        public ManyObjectProxy(IDistributedCache distributed, IEntityMany<TEntity, TEntityKey> entityMany)
            : base(distributed, $"Memory_Many_{entityMany.GetType().FullName}{typeof(TEntity).FullName}", entityMany.RetentionSecond)
        {
            this.entityMany = entityMany;
        }

        /// <summary>
        /// 同類型資料個別唯一性的，需要KEY來取得時使用
        /// </summary>
        /// <returns></returns>
        public TEntity GetObject(TEntityKey key)
        {
            TEntity result = GetEntityInMemory<TEntity, TEntityKey>(key);

            if (result == null)
            {
                result = entityMany.GetObject(key);
            }

            SetEntityInMemory(result, key);
            return result;
        }
        /// <summary>
        /// 同類型資料個別唯一性時使用的
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public bool PutObject(TEntity entity, TEntityKey key)
        {
            var isdMany = entityMany.PutObject(entity, key);

            if (isdMany)
            {
                SetEntityInMemory(entity, key);
            }

            return isdMany;
        }
    }
}
