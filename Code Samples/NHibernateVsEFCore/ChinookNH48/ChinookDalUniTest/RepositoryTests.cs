using ChinookDal;
using ChinookDalRepository;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NHibernate.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChinookDalUniTest
{
    [TestClass]
    public class RepositoryTests
    {
        [TestMethod]
        public void GetFirstTrack()
        {
            IRepository repository = new RepositoryBase();

            Track track = (Track)repository.GetById(typeof(Track), 1);

            Console.WriteLine($"{track.Name}");

            Assert.IsNotNull(track);
        }


        [TestMethod]
        public void GetRockTracks()
        {
            IRepository repository = new RepositoryBase();
            
            var qTracks = repository.GetIQueryable<Track>()
                                                .Fetch(tr => tr.Album)
                                                .Where(tr => tr.Genre.Name == "Rock");

            foreach (var item in qTracks.ToList())
            {
                Console.WriteLine($"{item.Name} - {item.Album.Title}");
            }



            Assert.AreEqual(1297, qTracks.Count());
        }

    }
}
