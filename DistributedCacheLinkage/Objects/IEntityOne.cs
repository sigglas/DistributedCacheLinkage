using System;
using System.Collections.Generic;
using System.Text;

namespace DistributedCacheLinkage.Objects
{
    /// <summary>
    /// 需要暫存特定某一份資料時使用
    /// </summary>
    /// <typeparam name="TEntity"></typeparam>
    public interface IEntityOne<TEntity> : IDisposable
    {
        int RetentionSecond { get; }
        /// <summary>
        /// 資料屬於唯一性的，不需要KEY直接取得時使用
        /// </summary>
        /// <returns></returns>
        TEntity GetObject();

        /// <summary>
        /// 資料屬於唯一性的，不需要KEY直接取得時使用
        /// </summary>
        /// <returns></returns>
        TEntity GetObject<TReq>(TReq expression) where TReq : class;

        /// <summary>
        /// 更新資料，當整體資料來源屬於只有一份時使用的
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        bool PutObject(TEntity entity);

    }
}
