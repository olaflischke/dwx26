using NHibernate.Mapping.ByCode;
using NHibernate.Mapping.ByCode.Conformist;

namespace ChinookNHDal.Mapping;

public class ArtistMap : ClassMapping<Artist>
{
    public ArtistMap()
    {
        Table("Artist");

        Id(x => x.ArtistId, m => m.Generator(Generators.Identity));

        Property(x => x.Name, m => m.Column("Name"));

        Bag(x => x.Albums,
            coll =>
            {
                coll.Key(k => k.Column("ArtistId"));
                coll.Inverse(true);
                coll.Cascade(Cascade.None);
            },
            rel => rel.OneToMany(o => o.Class(typeof(Album))));
    }
}