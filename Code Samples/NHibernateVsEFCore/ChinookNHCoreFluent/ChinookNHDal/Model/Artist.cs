namespace ChinookNHDal;

public  class Artist
{
    public virtual int ArtistId { get; set; }
    public virtual string Name { get; set; }
    public virtual IList<Album> Albums { get; set; }
}
