namespace ChinookNHDal;

public class Genre
{
    protected Genre()
    {

    }

    public Genre(string name)
    {
        this.Name = name;
    }

    public virtual int GenreId { get; set; }
    public virtual string Name { get; set; }
    public virtual IList<Track> Tracks { get; set; }
}
