using ChinookDal.Model;

namespace ChinookDal.UnitTests.Model;

public class PlaylistTests
{
    [Test]
    public void Playlist_Defaults_AreInitialized()
    {
        var playlist = new Playlist { Name = "My" };
        Assert.That(playlist.Tracks, Is.Not.Null);
        Assert.That(playlist.Tracks.Count, Is.EqualTo(0));
    }
}
