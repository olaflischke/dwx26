using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NHibernate.Cfg;
using NHibernate;
using NHibernate.Dialect;
using NHibernate.Driver;
using System.Data;
using System.Linq;
using ChinookDal;
using System.Diagnostics;
using NHibernate.Criterion;
using NHibernate.Linq;


namespace ChinookDalUniTest
{
    [TestClass]
    public class UpdateConcurrencyTest
    {
        [TestMethod]
        public void UpdateTrackTest()
        {
            Configuration configuration = QueryTests.ConfigureNHibernate();
            ISessionFactory factory = configuration.BuildSessionFactory();

            Track track;

            using (ISession session = factory.OpenSession())
            {
                //using (ITransaction transaction = session.BeginTransaction())
                //{
                track = session.Get<Track>(1); // Eager Loading
                track.Name = "Opus 3";
                
                

                session.Save(track);
                //transaction.Commit();
                //}

                session.Flush();
            }


            //using (ISession session = factory.OpenSession())
            //{
            //    using (ITransaction transaction = session.BeginTransaction())
            //    {

            //        session.Update(track);

            //        transaction.Commit();
            //    }
            //}
        }

    }
}
