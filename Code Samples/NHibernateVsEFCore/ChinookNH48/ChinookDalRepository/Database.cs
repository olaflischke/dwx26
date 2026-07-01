using NHibernate;
using NHibernate.Cfg;
using NHibernate.Dialect;
using NHibernate.Driver;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChinookDalRepository
{
    public static class Database
    {
        private static ISessionFactory _sessionFactory;

        private static ISessionFactory SessionFactory
        {
            get
            {
                if (_sessionFactory == null)
                {
                    Configuration configuration = ConfigureNHibernate();
                    _sessionFactory = configuration.BuildSessionFactory();
                }

                return _sessionFactory;
            }
        }

        public static ISession OpenSession()
        {
            return SessionFactory.OpenSession();
        }


        private static Configuration ConfigureNHibernate()
        {
            var cfg = new Configuration();

            cfg.DataBaseIntegration(x =>
            {
                x.ConnectionString = Properties.Settings.Default.ChinookConString;
                x.Driver<SqlClientDriver>();
                x.Dialect<MsSql2012Dialect>();
                x.IsolationLevel = IsolationLevel.RepeatableRead;
                x.LogSqlInConsole = true;
                x.Timeout = 10;
                x.BatchSize = 10;
            });

            cfg.SessionFactory().GenerateStatistics();

            cfg.AddAssembly("ChinookDal");

            return cfg;
        }
    }
}
