using NUnit.Framework;
using NHibernate;

namespace ChinookDal.Tests
{
    [TestFixture]
    public class NHibernateHelperTests
    {
        [Test]
        public void OpenSession_ReturnsNonNullSession()
        {
            // Arrange
            ISession expectedSession = null;

            // Act
            ISession actualSession = NHibernateHelper.OpenSession();

            

            // Assert
            Assert.IsNotNull(actualSession);
            Assert.AreNotEqual(expectedSession, actualSession);
        }
    }
}
