namespace ChinookNHDal;

public class Album
{
    public virtual int AlbumId { get; set; }
    public virtual string Title { get; set; }
    public virtual Artist Artist { get; set; }
    public virtual IList<Track> Tracks { get; set; } = new List<Track>();
}
