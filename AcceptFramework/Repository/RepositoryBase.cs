using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Linq.Expressions;
using NHibernate.Linq;
using AcceptFramework.DataAccess;
using AcceptFramework.Domain;

namespace AcceptFramework.Repository
{
    [DataObject]
    public class RepositoryBase<TDomain> where TDomain : DomainBase
    {
        internal IQueryable<TDomain> Query(NhibernateContext ctx)
        {
            return ctx.Session.Query<TDomain>();
        }

        internal IQueryable<T> Query<T>(NhibernateContext ctx)
        {
            return ctx.Session.Query<T>();
        }

        /// <summary>
        /// Selects a record by ID.
        /// </summary>
        /// <param name="id">Record ID</param>
        internal virtual TDomain GetById(int id)
        {
            return Get(ent => ent.Id == id);
        }

        /// <summary>
        /// Selects the only record matching a filter. Returns null if a matching record was not found.
        /// Throws and exception if more than one matching record was found.
        /// </summary>
        /// <param name="filter">Filter func</param>
        internal virtual TDomain Get(Expression<Func<TDomain, bool>> filter)
        {
            using (var ctx = new NhibernateContext())
            {
                return ctx.Session.Query<TDomain>().SingleOrDefault(filter);
            }
        }


        /// <summary>
        /// Selects the only record matching a filter. Returns null if a matching record was not found.
        /// Throws and exception if more than one matching record was found.
        /// </summary>
        /// <param name="filter">Filter func</param>
        internal virtual TDomain GetWithLock(Expression<Func<TDomain, bool>> filter, NHibernate.LockMode lockMode)
        {
            using (var ctx = new NhibernateContext())
            {

                TDomain obj = ctx.Session.Query<TDomain>().SingleOrDefault(filter);
                
                try
                {   if(obj != null)
                    ctx.Session.Lock(obj, lockMode);
                }
                catch (Exception e) { throw (e); }

                return obj;
                
            }
        }

        /// <summary>
        /// Selects all records.
        /// </summary>
        [DataObjectMethod(DataObjectMethodType.Select)]
        internal virtual IList<TDomain> Select()
        {
            using (var ctx = new NhibernateContext())
            {
                return ctx.Session.Query<TDomain>().ToList();
            }
        }

        /// <summary>
        /// Selects all records matching a filter.
        /// </summary>
        /// <param name="filter">Filter func</param>
        [DataObjectMethod(DataObjectMethodType.Select)]
        internal virtual IList<TDomain> Select(Expression<Func<TDomain, bool>> filter)
        {
            using (var ctx = new NhibernateContext())
            {
                return ctx.Session.Query<TDomain>().Where(filter).ToList();
            }
        }

        /// <summary>
        /// Creates a new record.
        /// </summary>
        [DataObjectMethod(DataObjectMethodType.Insert)]
        public virtual TDomain Create(TDomain record)
        {
            record.Validate();
            using (var ctx = new NhibernateContext(true))
            {
                ctx.Session.Save(record);
                ctx.Commit();
            }

            return record;
        }

        /// <summary>
        /// Updates a record.
        /// </summary>
        [DataObjectMethod(DataObjectMethodType.Update)]
        internal virtual TDomain Update(TDomain record)
        {
            record.Validate();
            using (var ctx = new NhibernateContext(true))
            {
                //context.Session.SaveOrUpdate(record);
                //context.Session.Update(record);
                ctx.Session.Merge(record);
                ctx.Commit();
            }

            return record;
        }

        /// <summary>
        /// Deletes a record.
        /// </summary>
        [DataObjectMethod(DataObjectMethodType.Delete)]
        internal virtual void Delete(TDomain record)
        {
            using (var ctx = new NhibernateContext(true))
            {
                ctx.Session.Delete(record);
                ctx.Commit();
            }
        }
    
    
    }
}
