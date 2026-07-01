using System.Data;
using NHibernate;
using NHibernate.Cfg;
using NHibernate.Dialect;
using NHibernate.Driver;
using NHibernate.Mapping.ByCode;
using ChinookNHDal.Mapping;

namespace ChinookNHDal;

public static class NHibernateConfigurator
{
    public static Configuration BuildConfiguration(string connectionString)
    {
        var config = new Configuration();

        config.DataBaseIntegration(x =>
        {
            x.ConnectionString = connectionString;
            x.Driver<MicrosoftDataSqlClientDriver>();
            x.Dialect<MsSql2012Dialect>();
            x.IsolationLevel = IsolationLevel.RepeatableRead;
            x.LogSqlInConsole = true;
            x.Timeout = 10;
            x.BatchSize = 10;
        });

        config.SessionFactory().GenerateStatistics();

        var mapper = new ModelMapper();
        mapper.AddMapping<ArtistMap>();
        mapper.AddMapping<AlbumMap>();
        mapper.AddMapping<GenreMap>();
        mapper.AddMapping<TrackMap>();

        var mapping = mapper.CompileMappingForAllExplicitlyAddedEntities();
        config.AddMapping(mapping);

        return config;
    }
}