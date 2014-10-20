using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NHibernate;

namespace AcceptFramework.DataAccess
{
    public class NhibernateContext : IDisposable
    {
        #region Fields
        private readonly bool _openTransaction;
        #endregion 

        #region Properties
        public ISession Session { get; private set; }
        #endregion                

        #region Ctors
        public NhibernateContext()
        {
            Initialize(false);
        }

        public NhibernateContext(bool openTransaction)
        {
            _openTransaction = openTransaction;
            Initialize(_openTransaction);
        }

        public void Dispose()
        {
            if (Session == null) return;
            if (Session.Transaction != null) Session.Transaction.Dispose();

            Session.Dispose();
        }
        #endregion

        #region Helpers
        private void Initialize(bool openTransaction)
        {
            Session = NhibernateSessionFactory.Instance.SessionFactory.OpenSession();

            if (openTransaction)
            {
                Session.BeginTransaction();
            }
        }

        public void Commit()
        {
            if (_openTransaction)
            {
                Session.Transaction.Commit();
            }
            else
            {
                throw new ArgumentException("Transaction is not open.");
            }
        }        
        #endregion
        
    }
}
