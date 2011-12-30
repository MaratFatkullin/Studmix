using System.IO;
using System.Web;
using System.Web.Mvc;
using Moq;

namespace AI_.Studmix.WebApplication.Tests
{
    public class TestFixtureBase
    {
        public ControllerContext CreateContext(string username)
        {
            var mock = new Mock<ControllerContext>();
            mock.SetupGet(p => p.HttpContext.User.Identity.Name).Returns(username);
            mock.SetupGet(p => p.HttpContext.Request.IsAuthenticated).Returns(true);
            return mock.Object;
        }

        public Stream CreateStream()
        {
            return new Mock<Stream>().Object;
        }

        public HttpPostedFileBase CreatePostedFile(Stream stream, string filename)
        {
            var mock = new Mock<HttpPostedFileBase>();

            mock.SetupGet(s => s.InputStream).Returns(stream);
            mock.SetupGet(s => s.FileName).Returns(filename);
            return mock.Object;
        }
    }
}