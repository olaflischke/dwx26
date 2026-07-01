using ChinookDal.Model;

namespace ChinookDal.UnitTests.Model;

public class TrackTests
{
    [Test]
    public void Track_Defaults_UnitPriceIsZeroAndCollectionsInitialized()
    {
        var track = new Track { Name = "N", MediaTypeId = 1 };
        Assert.That(track.InvoiceLines, Is.Not.Null);
        Assert.That(track.Playlists, Is.Not.Null);
        Assert.That(track.UnitPrice, Is.EqualTo(0m));
    }
}
