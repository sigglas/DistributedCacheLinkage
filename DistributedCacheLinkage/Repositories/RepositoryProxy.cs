using DistributedCacheLinkage.Package;
using Microsoft.Extensions.Caching.Distributed;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;

namespace DistributedCacheLinkage.Repositories
{
    /// <summary>
    /// 
    /// </summary>
    /// <typeparam name="TEntity"></typeparam>
    /// <typeparam name="TEntityKey"></typeparam>
    public class RepositoryProxy<TEntity, TEntityKey>
        where TEntity : class
    {
        private readonly IDistributedCache distributed;
        private readonly IEntityRepository<TEntity, TEntityKey> repository;
        private readonly string cacheId;
        private List<TEntity> nonCommitEntities;
        private List<object> nonCommitIds;
        /// <summary>
        /// 
        /// </summary>
        /// <param name="distributed"></param>
        /// <param name="repository"></param>
        /// <param name="autoCommit"></param>
        public RepositoryProxy(IDistributedCache distributed, IEntityRepository<TEntity, TEntityKey> repository)
        {
            this.distributed = distributed;
            this.repository = repository;
            this.cacheId = $"Memory_{repository.GetType().FullName}{typeof(TEntity).FullName}";
            this.nonCommitIds = new List<object>();
            this.nonCommitEntities = new List<TEntity>();
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

        /// <summary>
        /// 
        /// </summary>
        public void MemoryCommit()
        {
            if (nonCommitEntities != null && nonCommitEntities.Count > 0)
            {
                bool isChange = false;

                var entities = GetEntitiesInMemory();
                nonCommitEntities.ForEach(entity =>
                {
                    var keyP = entity.GetType().GetProperties()
                         .FirstOrDefault(f => f.GetCustomAttributes(typeof(System.ComponentModel.DataAnnotations.KeyAttribute), true).Length > 0);
                    var key = keyP.GetValue(entity);
                    var cItem = GetEntity(entities, key);

                    if (cItem != null)
                        entities.Remove(cItem);
                    entities.Add(entity);
                    isChange = true;

                });
                if (isChange)
                {
                    SetEntitiesInMemory(entities);
                    nonCommitEntities.Clear();
                }
            }
        }
        /// <summary>
        /// 如果新增或刪除時，實體的repo發生錯誤或拒絕寫入或發生rollback，可以呼叫此處清除此次的暫存
        /// </summary>
        public void Rollback()
        {
            if (nonCommitIds != null)
            {
                var entities = GetEntitiesInMemory();
                nonCommitIds.ForEach(keyId => {

                    var cItem = GetEntity(entities, keyId);

                    if (cItem != null)
                        entities.Remove(cItem);
                });
            }

        }
        /// <summary>
        /// 從暫存區中取得資料實體，若條件無資料，將會從你的Repository中取得並記錄至暫存區中
        /// </summary>
        /// <param name="expression"></param>
        /// <returns></returns>
        public IList<TEntity> GetEntities(Expression<Func<TEntity, bool>> expression = null)
        {
            var result = new List<TEntity>();

            var entities = GetEntitiesInMemory().ToList();

            if (expression != null)
                result = entities.AsQueryable().Where(expression).ToList();
            else
                result.AddRange(entities);

            if (result.Count == 0)
            {
                result = repository.GetEntities(expression).ToList();
                entities.AddRange(result);
            }

            SetEntitiesInMemory(entities);

            entities.Clear();
            return result;
        }
        /// <summary>
        /// 加入資料實體，並更新記錄至暫存區中
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public TEntity Add(TEntity entity)
        {
            TEntity _entity;
            //if (autoCommit)
            //{
            //    var entities = GetEntitiesInMemory();

            //    _entity = repository.Add(entity);

            //    var keyP = entity.GetType().GetProperties()
            //         .FirstOrDefault(f => f.GetCustomAttributes(typeof(System.ComponentModel.DataAnnotations.KeyAttribute), true).Length > 0);
            //    var key = keyP.GetValue(entity);
            //    var cItem = GetEntity(entities, key);
            //    nonCommitIds.Add(key);

            //    if (cItem != null)
            //        entities.Remove(cItem);

            //    entities.Add(_entity);
            //    nonCommitEntities.Add(_entity);

            //    SetEntitiesInMemory(entities);

            //    entities.Clear();
            //}
            //else
            //{
                _entity = repository.Add(entity);
                nonCommitEntities.Add(_entity);
            //}
            return _entity;
        }
        /// <summary>
        /// 更新資料實體，並移除原暫存區的紀錄，加入新的記錄至暫存區中
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public bool Update(TEntity entity)
        {
            var entities = GetEntitiesInMemory();

            var isdone = repository.Update(entity);

            if (isdone)
            {
                var keyP = entity.GetType().GetProperties()
                 .FirstOrDefault(f => f.GetCustomAttributes(typeof(System.ComponentModel.DataAnnotations.KeyAttribute), true).Length > 0);
                var key = keyP.GetValue(entity);
                var cItem = GetEntity(entities, key);
                nonCommitIds.Add(key);

                if (cItem != null)
                    entities.Remove(cItem);

                entities.Add(entity);

                SetEntitiesInMemory(entities);
            }

            entities.Clear();

            return isdone;
        }
        /// <summary>
        /// 移除資料實體，並移除原暫存區的紀錄
        /// </summary>
        /// <param name="keyId"></param>
        /// <returns></returns>
        public bool Remove(TEntityKey keyId)
        {
            var entities = GetEntitiesInMemory();

            var isdone = repository.Remove(keyId);

            if (isdone)
            {
                var cItem = GetEntity(entities, keyId);

                if (cItem != null)
                    entities.Remove(cItem);

                SetEntitiesInMemory(entities);
            }

            entities.Clear();

            return isdone;
        }

        bool _dispose = false;
        /// <summary>
        /// 
        /// </summary>
        public void Dispose()
        {
            if (_dispose == false)
            {
                MemoryCommit();
                nonCommitIds.Clear();
                nonCommitIds = null;
                if (nonCommitEntities != null)
                    nonCommitEntities.Clear();
                nonCommitEntities = null;
                _dispose = true;
            }
        }

        private IList<TEntity> GetEntitiesInMemory()
        {
            IList<TEntity> entities = null;

            var memory = distributed.Get(cacheId);

            if (memory != null)
            {
                entities = Json.FromJsonString<IList<TEntity>>(System.Text.Encoding.UTF8.GetString(memory));
            }
            else
            {
                entities = new System.Collections.ObjectModel.Collection<TEntity>();
            }
            return entities;
        }
        private TEntity GetEntity<TKey>(IList<TEntity> entities, TKey keyId)
        {
            return entities.FirstOrDefault(f =>
            {
                var keyP = f.GetType().GetProperties()
                .FirstOrDefault(f => f.GetCustomAttributes(typeof(System.ComponentModel.DataAnnotations.KeyAttribute), true).Length > 0);
                if (keyP == null)
                    return false;

                var a = (TKey)keyP.GetValue(f);
                if (a.Equals(keyId))
                {
                    return true;
                }
                else
                    return false;
            });
        }
        private void SetEntitiesInMemory(IList<TEntity> entities)
        {
            var bData = System.Text.Encoding.UTF8.GetBytes(Json.AsJsonString(entities));
            distributed.Set(cacheId, bData, new DistributedCacheEntryOptions
            {
                AbsoluteExpiration = DateTime.Now.AddMinutes(15)
            });
        }
 
    }
}
