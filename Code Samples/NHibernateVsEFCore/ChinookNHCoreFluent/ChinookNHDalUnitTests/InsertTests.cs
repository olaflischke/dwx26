using ChinookNHDal;
using NHibernate;
using NHibernate.Cfg;
using NHibernate.Linq;

namespace ChinookNHDalUnitTests;

public class InsertTests
{
    [Test]
    public void CanAddNewAlbum()
    {
        Configuration configuration = QueryTests.ConfigureNHibernate();
        ISessionFactory factory = configuration.BuildSessionFactory();

        using ISession session = factory.OpenSession();

        Artist? artist = session.Query<Artist>().Where(at => at.Name == "AC/DC").FirstOrDefault();
        Genre? genre = session.Query<Genre>().FirstOrDefault();

        if (artist != null && genre != null)
        {
            Album album = new Album() { Title = "The new one", Artist = artist };

            for (int i = 0; i < 10; i++)
            {
                Track track = new Track()
                {
                    Name = $"Opus {i}",
                    Album = album,
                    Genre = genre,
                    MediaTypeId = 1
                };

                album.Tracks.Add(track);
            }

            session.Save(album);
            session.Flush();

            Assert.That(true);
        }
        else
        {
            Assert.That(false);
        }
    }
}
