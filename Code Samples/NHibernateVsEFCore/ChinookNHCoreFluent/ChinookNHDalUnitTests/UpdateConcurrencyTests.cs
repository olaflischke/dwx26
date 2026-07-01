using ChinookNHDal;
using NHibernate;
using NHibernate.Cfg;

namespace ChinookNHDalUnitTests;
public class UpdateConcurrencyTests
{
    [Test]
    public void UpdateTrackTest()
    {
        Configuration configuration = QueryTests.ConfigureNHibernate();
        ISessionFactory factory = configuration.BuildSessionFactory();

        Track track1;
        Track track2;

        // Seesion 1 beginnt
        using (ISession session1 = factory.OpenSession())
        {
            track1 = session1.Get<Track>(1); // Eager Loading
            track1.Name = "Opus 3";

            // Session 2 funkt dazwischen
            using (ISession session2 = factory.OpenSession())
            {
                track2 = session2.Get<Track>(1); // Eager Loading
                track2.Name = "Opus 4";

                session2.Save(track2);

                session2.Flush();
            }

            session1.Save(track1);

            session1.Flush();
        }


        //using (ISession session = factory.OpenSession())
        //{
        //    using (ITransaction transaction = session.BeginTransaction())
        //    {
        //        // Anweisungen
        //
        //
        //        transaction.Commit();
        //    }
        //}
    }

}
