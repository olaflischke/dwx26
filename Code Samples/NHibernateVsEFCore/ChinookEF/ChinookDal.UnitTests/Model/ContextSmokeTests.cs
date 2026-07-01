using ChinookDal.Model;
using Microsoft.EntityFrameworkCore;

namespace ChinookDal.UnitTests.Model;

public class ContextSmokeTests
{
    [Test]
    public void CanConstructContext_WithOptions()
    {
        var options = new DbContextOptionsBuilder<ChinookContext>().Options;
        using var ctx = new ChinookContext(options);
        Assert.That(ctx, Is.Not.Null);
    }
}
