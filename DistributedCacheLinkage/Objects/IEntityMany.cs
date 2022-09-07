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
    public interface IEntityMany<TEntity, TEntityKey> : IDisposable
    {
        int RetentionSecond { get; }
        /// <summary>
        /// 同類型資料個別唯一性的，需要KEY來取得時使用
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        TEntity GetObject(TEntityKey key);

        /// <summary>
        /// 同類型資料個別唯一性時使用的
        /// </summary>
        /// <param name="entity"></param>
        /// <param name="key"></param>
        /// <returns></returns>
        bool PutObject(TEntity entity, TEntityKey key);
    }
}
