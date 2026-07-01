using NHibernate.Mapping.ByCode;
using NHibernate.Mapping.ByCode.Conformist;

namespace ChinookNHDal.Mapping;

public class GenreMap : ClassMapping<Genre>
{
    public GenreMap()
    {
        Table("Genre");

        Id(x => x.GenreId, m => m.Generator(Generators.Identity));

        Property(x => x.Name, m => m.Column("Name"));

        Bag(x => x.Tracks,
            coll =>
            {
                coll.Key(k => k.Column("GenreId"));
                coll.Inverse(true);
                coll.Cascade(Cascade.None);
            },
            rel => rel.OneToMany(o => o.Class(typeof(Track))));
    }
}