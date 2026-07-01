using ChinookNHDal;
using NHibernate;
using NHibernate.Cfg;
using NHibernate.Criterion;
using NHibernate.Linq;
using NHibernate.Dialect;
using NHibernate.Driver;
using System.Diagnostics;
using System.Data;
using Microsoft.Extensions.Configuration;

namespace ChinookNHDalUnitTests;

public class QueryTests
{
    [SetUp]
    public void Setup()
    {

    }

    [Test]
    public void GetRockTracksHql()
    {
        Configuration configuration = ConfigureNHibernate();
        ISessionFactory factory = configuration.BuildSessionFactory();

        using (ISession session = factory.OpenSession())
        {
            IQuery qTracks = session.CreateQuery("select tr from Track tr where tr.Genre.Name = 'Rock'");

            foreach (var item in qTracks.Enumerable<Track>())
            {
                Console.WriteLine($"{item.Name} - {item.Album.Title}");
            }

            Assert.That(qTracks.Enumerable<Track>().Count(), Is.EqualTo(1297));
        }
    }

    [Test]
    public void GetRockTracksCriteria()
    {
        Configuration configuration = ConfigureNHibernate();
        ISessionFactory factory = configuration.BuildSessionFactory();

        using (ISession session = factory.OpenSession())
        {
            ICriteria qTracks = session.CreateCriteria<Track>()
                                    .CreateCriteria("Genre")
                                        .Add(Expression.Like("Name", "Rock"));

            foreach (var item in qTracks.List<Track>())
            {
                Console.WriteLine($"{item.Name} - {item.Album.Title}");
            }

            Assert.That(qTracks.List<Track>().Count(), Is.EqualTo(1297));
        }
    }

    [Test]
    public void GetRockTracksQueryOver()
    {
        Configuration configuration = ConfigureNHibernate();
        ISessionFactory factory = configuration.BuildSessionFactory();

        using (ISession session = factory.OpenSession())
        {
            IQueryOver<Track, Genre> qTracks = session.QueryOver<Track>()
                                .JoinQueryOver<Genre>(tr => tr.Genre)
                                .Where(gr => gr.Name == "Rock");

            foreach (var item in qTracks.List<Track>())
            {
                Console.WriteLine($"{item.Name} - {item.Album.Title}");
            }

            Assert.That(qTracks.List<Track>().Count(), Is.EqualTo(1297));
        }
    }

    [Test]
    public void GetRockTracksLinq()
    {
        Configuration configuration = ConfigureNHibernate();
        ISessionFactory factory = configuration.BuildSessionFactory();

        using (ISession session = factory.OpenSession())
        {

            IQueryable<Track> qTracks = session.Query<Track>()
                                .Fetch(tr => tr.Album).ThenFetch(al => al.Artist) // Artist
                                .Where(tr => tr.Genre.Name == "Rock");

            foreach (var item in qTracks.ToList())
            {
                Console.WriteLine($"{item.Name} - {item.Album.Title} - {item.Album.Artist.Name}");
            }

            Assert.That(qTracks.ToList().Count(), Is.EqualTo(1297));
        }


    }

    [Test]
    public void GetRockTracksNative()
    {
        Configuration configuration = ConfigureNHibernate();
        ISessionFactory factory = configuration.BuildSessionFactory();

        using (ISession session = factory.OpenSession())
        {

            ISQLQuery qTracks = session.CreateSQLQuery("SELECT * FROM Track AS tr INNER JOIN Genre as gr ON tr.GenreId = gr.GenreId WHERE gr.Name = 'Rock'")
                                    .AddEntity(typeof(Track));

            foreach (var item in qTracks.List<Track>())
            {
                Console.WriteLine($"{item.Genre.Name}: {item.Name} - {item.Album.Title}");
            }

            Assert.That(qTracks.List<Track>().Count(), Is.EqualTo(1297));
        }

    }

    [Test]
    public void GetGenres()
    {
        Configuration configuration = ConfigureNHibernate();
        ISessionFactory factory = configuration.BuildSessionFactory();

        using (ISession session = factory.OpenSession())
        {
            var qGenres = session.Query<Genre>();

            foreach (var item in qGenres.ToList())
            {
                Console.WriteLine($"{item.Name}");
            }
        }


    }


    public static Configuration ConfigureNHibernate()
    {
        // Delegation an DAL
        return NHibernateConfigurator.BuildConfiguration(
            "Server=(localdb)\\mssqllocaldb;Database=Chinook;Trusted_Connection=True;");
    }

}
