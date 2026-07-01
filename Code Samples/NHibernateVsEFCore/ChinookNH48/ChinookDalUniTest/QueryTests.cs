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
    public class QueryTests
    {
        [TestMethod]
        public void GetRockTracksHql()
        {
            Configuration configuration = ConfigureNHibernate();
            ISessionFactory factory = configuration.BuildSessionFactory();

            using (ISession session = factory.OpenSession())
            {
                var qTracks = session.CreateQuery("select tr from Track tr where tr.Genre.Name = 'Rock'");

                foreach (var item in qTracks.Enumerable<Track>())
                {
                    Trace.WriteLine($"{item.Name} - {item.Album.Title}");
                }

                Assert.AreEqual(1297, qTracks.Enumerable<Track>().Count());
            }
        }

        [TestMethod]
        public void GetRockTracksCriteria()
        {
            Configuration configuration = ConfigureNHibernate();
            ISessionFactory factory = configuration.BuildSessionFactory();

            using (ISession session = factory.OpenSession())
            {
                var qTracks = session.CreateCriteria<Track>()
                                        .CreateCriteria("Genre")
                                            .Add(Expression.Like("Name", "Rock"));

                foreach (var item in qTracks.List<Track>())
                {
                    Trace.WriteLine($"{item.Name} - {item.Album.Title}");
                }

                Assert.AreEqual(1297, qTracks.List<Track>().Count());
            }
        }

        [TestMethod]
        public void GetRockTracksQueryOver()
        {
            Configuration configuration = ConfigureNHibernate();
            ISessionFactory factory = configuration.BuildSessionFactory();

            using (ISession session = factory.OpenSession())
            {
                var qTracks = session.QueryOver<Track>()
                                    .JoinQueryOver<Genre>(tr => tr.Genre)
                                    .Where(gr => gr.Name == "Rock");

                foreach (var item in qTracks.List<Track>())
                {
                    Trace.WriteLine($"{item.Name} - {item.Album.Title}");
                }

                Assert.AreEqual(1297, qTracks.List<Track>().Count());
            }
        }

        [TestMethod]
        public void GetRockTracksLinq()
        {
            Configuration configuration = ConfigureNHibernate();
            ISessionFactory factory = configuration.BuildSessionFactory();

            using (ISession session = factory.OpenSession())
            {

                var qTracks = session.Query<Track>()
                                    .Fetch(tr => tr.Album).ThenFetch(al => al.Artist) // Artist
                                    .Where(tr => tr.Genre.Name == "Rock");

                foreach (var item in qTracks.ToList())
                {
                    Trace.WriteLine($"{item.Name} - {item.Album.Title} - {item.Album.Artist.Name}");
                }

                Assert.AreEqual(1297, qTracks.ToList().Count());
            }


        }

        [TestMethod]
        public void GetRockTracksNative()
        {
            Configuration configuration = ConfigureNHibernate();
            ISessionFactory factory = configuration.BuildSessionFactory();

            using (ISession session = factory.OpenSession())
            {

                var qTracks = session.CreateSQLQuery("SELECT * FROM Track AS tr INNER JOIN Genre as gr ON tr.GenreId = gr.GenreId WHERE gr.Name = 'Rock'")
                                        .AddEntity(typeof(Track));

                foreach (var item in qTracks.List<Track>())
                {
                    Trace.WriteLine($"{item.Genre.Name}: {item.Name} - {item.Album.Title}");
                }

                Assert.AreEqual(1297, qTracks.List<Track>().Count());
            }

        }

        [TestMethod]
        public void GetGenres()
        {
            Configuration configuration = ConfigureNHibernate();
            ISessionFactory factory = configuration.BuildSessionFactory();

            using (ISession session=factory.OpenSession())
            {
                var qGenres = session.Query<Genre>();

                foreach (var item in qGenres)
                {
                    Trace.WriteLine($"{item.Name}");
                }
            }

            
        }


        public static Configuration ConfigureNHibernate()
        {
            var cfg = new Configuration();

            cfg.DataBaseIntegration(x =>
            {
                x.ConnectionString = Properties.Settings.Default.ChinookConString;
                x.Driver<SqlClientDriver>();
                x.Dialect<MsSql2012Dialect>(); // TODO: Dynamisch ermittelbar?
                x.IsolationLevel = IsolationLevel.RepeatableRead;
                x.LogSqlInConsole = true;
                x.Timeout = 10;
                x.BatchSize = 10;
            });

            cfg.SessionFactory().GenerateStatistics();

            cfg.AddAssembly("ChinookDal"); // Referenz zur Assembly mit den POCOs und dem Mapping

            return cfg;
        }

    }
}
