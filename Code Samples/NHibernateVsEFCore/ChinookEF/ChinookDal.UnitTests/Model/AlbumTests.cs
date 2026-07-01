using ChinookDal.Model;

namespace ChinookDal.UnitTests.Model;

public class AlbumTests
{
    [Test]
    public void Album_Defaults_AreInitialized()
    {
        var album = new Album { Title = "T" };
        Assert.That(album.Tracks, Is.Not.Null);
        Assert.That(album.Tracks.Count, Is.EqualTo(0));
    }
}
