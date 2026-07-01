using NHibernate.Mapping.ByCode;
using NHibernate.Mapping.ByCode.Conformist;

namespace ChinookNHDal.Mapping;

public class TrackMap : ClassMapping<Track>
{
    public TrackMap()
    {
        Table("Track");

        OptimisticLock(OptimisticLockMode.Dirty);
        DynamicUpdate(true);

        Id(x => x.TrackId, m => m.Generator(Generators.Identity));

        Property(x => x.Name, m => m.Column("Name"));
        Property(x => x.Composer, m => m.Column("Composer"));
        Property(x => x.Milliseconds, m => m.Column("Milliseconds"));
        Property(x => x.UnitPrice, m => m.Column("UnitPrice"));
        Property(x => x.MediaTypeId, m => m.Column("MediaTypeId"));

        ManyToOne(x => x.Album, m =>
        {
            m.Column("AlbumId");
            m.NotNullable(true);
            m.Class(typeof(Album));
        });

        ManyToOne(x => x.Genre, m =>
        {
            m.Column("GenreId");
            m.NotNullable(true);
            m.Class(typeof(Genre));
        });
    }
}