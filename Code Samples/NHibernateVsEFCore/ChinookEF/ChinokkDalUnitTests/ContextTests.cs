using ChinookDal.Model;
using Microsoft.EntityFrameworkCore;

namespace ChinokkDalUnitTests;

public class ChinookContextTests
{
    string connection = "Data Source=(localdb)\\MSSQLLocalDB;Initial Catalog=Chinook;Integrated Security=True;";

    private ChinookContext GetContext(bool logging)
    {
        DbContextOptionsBuilder<ChinookContext> optionsBuilder = new DbContextOptionsBuilder<ChinookContext>().UseSqlServer(connection);

        if (logging)
        {
            optionsBuilder.LogTo(Console.WriteLine, Microsoft.Extensions.Logging.LogLevel.Information);
        }

        return new ChinookContext(optionsBuilder.Options);
    }

    /// <summary>
    /// Erweiterte GetContext-Methode mit LoggingInterceptor-Unterstützung
    /// </summary>
    /// <param name="logging">Standard EF Core Logging aktivieren</param>
    /// <param name="useInterceptor">LoggingInterceptor aktivieren</param>
    /// <returns>ChinookContext mit gewünschten Logging-Optionen</returns>
    private ChinookContext GetContextWithInterceptor(bool logging = false, bool useInterceptor = false)
    {
        DbContextOptionsBuilder<ChinookContext> optionsBuilder = new DbContextOptionsBuilder<ChinookContext>()
            .UseSqlServer(connection);

        if (logging)
        {
            optionsBuilder.LogTo(Console.WriteLine, Microsoft.Extensions.Logging.LogLevel.Information);
        }

        return new ChinookContext(optionsBuilder.Options, useInterceptor);
    }

    [Test]
    public void CanGetArtists()
    {
        using ChinookContext context = GetContext(logging: true);
        List<Artist> artists = context.Artists.ToList();
        // Replace this line:
        // Assert.AreEqual(275, artists.Count);

        // With this line:
        Assert.That(artists.Count, Is.EqualTo(275));
    }

    [Test]
    public void CanChangeArtist()
    {
        using ChinookContext context = GetContext(logging: true);
        Artist artist = context.Artists.First();
        artist.Name = "The Artist formerly known as AC/DC";
        context.SaveChanges();
    }

    [Test]
    public void CanAddAlbum()
    {
        using ChinookContext context = GetContext(logging: true);

        Artist artist = context.Artists.First();
        Genre genre = context.Genres.First();

        if (artist != null && genre != null)
        {
            Album album = new Album() { Title = "The new one", Artist = artist };

            for (int i = 0; i < 10; i++)
            {
                Track track = new Track()
                {
                    Name = $"Opus {i}",
                    Album = album,
                    Genre = genre,
                    MediaTypeId = 1
                };

                album.Tracks.Add(track);
            }

            context.Albums.Add(album);
            context.SaveChanges();

            Assert.Pass();
        }
        else
            Assert.Fail();
    }

    #region LoggingInterceptor Examples

    [Test]
    public void InterceptorExample_GetArtistsWithLogging()
    {
        Console.WriteLine("=== LoggingInterceptor Example: Artists abfragen ===");

        using ChinookContext context = GetContextWithInterceptor(useInterceptor: true);

        // Diese Abfrage wird vom LoggingInterceptor protokolliert
        List<Artist> artists = context.Artists.Take(5).ToList();

        Console.WriteLine($"Anzahl Künstler: {artists.Count}");
        foreach (var artist in artists)
        {
            Console.WriteLine($"- {artist.Name}");
        }

        Assert.That(artists.Count, Is.EqualTo(5));
    }

    [Test]
    public void InterceptorExample_ComplexQueryWithJoins()
    {
        Console.WriteLine("=== LoggingInterceptor Example: Komplexe Abfrage mit Joins ===");

        using ChinookContext context = GetContextWithInterceptor(useInterceptor: true);

        // Komplexe Abfrage ähnlich den NHibernate-Beispielen
        var rockTracks = context.Tracks
            .Include(t => t.Album)
                .ThenInclude(a => a!.Artist)
            .Include(t => t.Genre)
            .Where(t => t.Genre!.Name == "Rock")
            .Take(10)
            .ToList();

        Console.WriteLine($"Anzahl Rock-Tracks gefunden: {rockTracks.Count}");
        foreach (var track in rockTracks.Take(3))
        {
            Console.WriteLine($"- {track.Name} von {track.Album?.Artist?.Name} ({track.Album?.Title})");
        }

        Assert.That(rockTracks.Count, Is.EqualTo(10));
    }

    [Test]
    public void InterceptorExample_UpdateOperation()
    {
        Console.WriteLine("=== LoggingInterceptor Example: Update-Operation ===");

        using ChinookContext context = GetContextWithInterceptor(useInterceptor: true);

        // Künstler für Update finden
        Artist? artist = context.Artists.FirstOrDefault(a => a.Name!.Contains("AC/DC"));

        if (artist != null)
        {
            string originalName = artist.Name!;
            artist.Name = $"{originalName} (Updated at {DateTime.Now:HH:mm:ss})";

            // Update wird vom Interceptor protokolliert
            context.SaveChanges();

            Console.WriteLine($"Artist updated: {originalName} -> {artist.Name}");

            // Zurücksetzen für weitere Tests
            artist.Name = originalName;
            context.SaveChanges();

            Assert.Pass("Update-Operation erfolgreich protokolliert");
        }
        else
        {
            Assert.Inconclusive("AC/DC nicht in der Datenbank gefunden");
        }
    }

    [Test]
    public void InterceptorExample_AggregateQuery()
    {
        Console.WriteLine("=== LoggingInterceptor Example: Aggregat-Abfrage ===");

        using ChinookContext context = GetContextWithInterceptor(useInterceptor: true);

        // Aggregat-Abfragen ähnlich NHibernate-Beispielen
        var genreStats = context.Genres
            .Select(g => new
            {
                GenreName = g.Name,
                TrackCount = g.Tracks.Count(),
                AvgPrice = g.Tracks.Average(t => t.UnitPrice)
            })
            .Where(g => g.TrackCount > 0)
            .OrderByDescending(g => g.TrackCount)
            .Take(5)
            .ToList();

        Console.WriteLine("Top 5 Genres nach Anzahl Tracks:");
        foreach (var stat in genreStats)
        {
            Console.WriteLine($"- {stat.GenreName}: {stat.TrackCount} Tracks, Ø Preis: {stat.AvgPrice:C}");
        }

        Assert.That(genreStats.Count, Is.EqualTo(5));
    }

    [Test]
    public void InterceptorExample_RawSqlQuery()
    {
        Console.WriteLine("=== LoggingInterceptor Example: Raw SQL Abfrage ===");

        using ChinookContext context = GetContextWithInterceptor(useInterceptor: true);

        // Raw SQL ähnlich dem NHibernate Native SQL Beispiel
        var rockTracks = context.Tracks
            .FromSqlRaw(@"
                SELECT t.* 
                FROM Track t 
                INNER JOIN Genre g ON t.GenreId = g.GenreId 
                WHERE g.Name = 'Rock'")
            .Take(5)
            .ToList();

        Console.WriteLine($"Raw SQL: {rockTracks.Count} Rock-Tracks gefunden");
        foreach (var track in rockTracks)
        {
            Console.WriteLine($"- {track.Name} (TrackId: {track.TrackId})");
        }

        Assert.That(rockTracks.Count, Is.EqualTo(5));
    }

    [Test]
    public void InterceptorExample_CompareSameQueryWithAndWithoutInterceptor()
    {
        Console.WriteLine("=== Vergleich: Mit und ohne LoggingInterceptor ===");

        Console.WriteLine("\n1. Ohne LoggingInterceptor:");
        using (ChinookContext contextWithoutInterceptor = GetContextWithInterceptor(useInterceptor: false))
        {
            var artistsWithout = contextWithoutInterceptor.Artists.Take(3).ToList();
            Console.WriteLine($"   {artistsWithout.Count} Künstler geladen (keine Interceptor-Logs)");
        }

        Console.WriteLine("\n2. Mit LoggingInterceptor:");
        using (ChinookContext contextWithInterceptor = GetContextWithInterceptor(useInterceptor: true))
        {
            var artistsWith = contextWithInterceptor.Artists.Take(3).ToList();
            Console.WriteLine($"   {artistsWith.Count} Künstler geladen (mit Interceptor-Logs oben sichtbar)");
        }

        Assert.Pass("Vergleich erfolgreich durchgeführt");
    }

    #endregion
}