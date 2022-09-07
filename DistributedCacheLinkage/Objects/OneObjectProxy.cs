using Microsoft.Extensions.Caching.Distributed;
using System;
using System.Collections.Generic;
using System.Text;

namespace DistributedCacheLinkage.Objects
{
    /// <summary>
    /// 需要暫存特定某一份資料時使用
    /// </summary>
    /// <typeparam name="TEntity"></typeparam>
    public class OneObjectProxy<TEntity>: BaseProxy
    {
        private readonly IEntityOne<TEntity> entityOne;
         

        public OneObjectProxy(IDistributedCache distributed, IEntityOne<TEntity> entityOne)
            : base(distributed, $"Memory_One_{entityOne.GetType().FullName}{typeof(TEntity).FullName}", entityOne.RetentionSecond)
        {
            this.entityOne = entityOne;
        }

        private object _Option;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="value"></param>
        public void SetOption(object value) => _Option = value;

        /// <summary>
        /// 資料屬於唯一性的，不需要KEY直接取得時使用
        /// </summary>
        /// <returns></returns>
        public TEntity GetObject()
        {
            TEntity result = GetEntityInMemory<TEntity>();

            if (result == null)
            {
                if (_Option == null)
                    result = entityOne.GetObject();
                else
                    result = entityOne.GetObject(_Option);

            }
            SetEntityInMemory(result);
            return result;
        }
        /// <summary>
        /// 資料屬於只有一份時使用的
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public bool PutObject(TEntity entity)
        {
            var isdone = entityOne.PutObject(entity);

            if (isdone)
            {
                SetEntityInMemory(entity);
            }

            return isdone;
        }
    }
}
