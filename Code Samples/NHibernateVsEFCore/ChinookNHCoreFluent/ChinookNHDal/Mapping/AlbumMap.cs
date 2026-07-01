using NHibernate.Mapping.ByCode;
using NHibernate.Mapping.ByCode.Conformist;

namespace ChinookNHDal.Mapping;

public class AlbumMap : ClassMapping<Album>
{
    public AlbumMap()
    {
        Table("Album");

        Id(x => x.AlbumId, m => m.Generator(Generators.Identity));

        Property(x => x.Title, m => m.Column("Title"));

        ManyToOne(x => x.Artist, m =>
        {
            m.Column("ArtistId");
            m.NotNullable(true);
            m.Cascade(Cascade.All);
            m.Class(typeof(Artist));
        });

        Bag(x => x.Tracks,
            coll =>
            {
                coll.Key(k => k.Column("AlbumId"));
                coll.Inverse(true);
                coll.Cascade(Cascade.All);
            },
            rel => rel.OneToMany(o => o.Class(typeof(Track))));
    }
}