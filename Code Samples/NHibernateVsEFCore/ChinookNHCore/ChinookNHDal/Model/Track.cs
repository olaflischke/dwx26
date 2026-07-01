namespace ChinookNHDal;

public  class Track
{
    public virtual int TrackId { get; set; }
    public virtual string Name { get; set; }
    public virtual Album Album { get; set; }
    public virtual Genre Genre { get; set; }
    public virtual string Composer { get; set; }
    public virtual int Milliseconds { get; set; }
    public virtual double UnitPrice { get; set; }
    public virtual int MediaTypeId { get; set; }
}
