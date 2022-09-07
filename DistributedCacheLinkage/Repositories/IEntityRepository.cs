using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;

namespace DistributedCacheLinkage.Repositories
{
    /// <summary>
    /// 
    /// </summary>
    /// <typeparam name="TEntity"></typeparam>
    /// <typeparam name="TEntityKey"></typeparam>
    public interface IEntityRepository<TEntity, TEntityKey> : IDisposable where TEntity : class
    {
        /// <summary>
        /// 從暫存區中取得資料實體，若條件無資料，將會從你的Repository中取得並記錄至暫存區中
        /// </summary>
        /// <param name="expression"></param>
        /// <returns></returns>
        IList<TEntity> GetEntities(Expression<Func<TEntity, bool>> expression);
        /// <summary>
        /// 加入資料實體，並更新記錄至暫存區中
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        TEntity Add(TEntity obj);
        /// <summary>
        /// 更新資料實體，並移除原暫存區的紀錄，加入新的記錄至暫存區中
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        bool Update(TEntity obj);
        /// <summary>
        /// 移除資料實體，並移除原暫存區的紀錄
        /// </summary>
        /// <param name="keyId"></param>
        /// <returns></returns>
        bool Remove(TEntityKey keyId);
    }
}
