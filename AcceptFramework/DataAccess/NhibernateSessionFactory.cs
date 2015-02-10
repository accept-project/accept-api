using FluentNHibernate.Cfg;
using NHibernate;
using NHibernate.Cfg;
using NHibernate.Tool.hbm2ddl;
using AcceptFramework.Mapping.Audit;
using AcceptFramework.Mapping.Session;
using AcceptFramework.Mapping.Common;
using System;



namespace AcceptFramework.DataAccess
{
    public class NhibernateSessionFactory
    {
        #region Fields
        private static NhibernateSessionFactory _instance;
        #endregion 

        #region Properties
        public ISessionFactory SessionFactory { get; private set; }
        #endregion

        #region Constructors
        private NhibernateSessionFactory()
        {
            var baseConfig = new Configuration();
            baseConfig.Configure();

            //having one mapping class path is enough.
            var config = Fluently.Configure(baseConfig).Mappings(m => m.FluentMappings.AddFromAssemblyOf<UserMap>()).Mappings(m => m.FluentMappings.AddFromAssemblyOf<RoleMap>()).ExposeConfiguration(cfg => cfg.Properties.Add("use_outer_join", "true"));
			
            try
            {
                if (System.Configuration.ConfigurationManager.AppSettings["DbSchemaDrop"].CompareTo("1") == 0)
                    config.ExposeConfiguration(cfg => new SchemaExport(cfg).Drop(false, true));
                if (System.Configuration.ConfigurationManager.AppSettings["DbSchemaCreate"].CompareTo("1") == 0)
                    config.ExposeConfiguration(cfg => new SchemaExport(cfg).Create(false, true));
                if (System.Configuration.ConfigurationManager.AppSettings["DbSchemaUpdate"].CompareTo("1") == 0)
                    config.ExposeConfiguration(cfg => new SchemaUpdate(cfg).Execute(false, true));
            }
            catch (Exception e)
            {
                throw (e);
            }

            SessionFactory = config.BuildSessionFactory();                     
        }

        public static NhibernateSessionFactory Instance
        {
            get
            {
                if (_instance != null) return _instance;
                _instance = new NhibernateSessionFactory();
                return _instance;
            }
        }
        #endregion 
            
    }
}
