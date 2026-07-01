using ChinookDal.Model;

namespace ChinookDal.UnitTests.Model;

public class ArtistTests
{
    [Test]
    public void NewArtist_HasEmptyAlbums()
    {
        var artist = new Artist { Name = "X" };
        Assert.That(artist.Albums, Is.Not.Null);
        Assert.That(artist.Albums.Count, Is.EqualTo(0));
    }
}
