using System;
using System.IO;
using System.Linq;
using NUnit.Framework;
using Xamarin.UITest;
using Xamarin.UITest.Queries;

namespace AppCenterTest.UITests
{
  [TestFixture(Platform.Android)]
  [TestFixture(Platform.iOS)]
  public class Tests
  {
    IApp app;
    Platform platform;

    public Tests(Platform platform)
    {
      this.platform = platform;
    }

    [SetUp]
    public void BeforeEachTest()
    {
      app = AppInitializer.StartApp(platform);
    }

    [Test]
    public void FABWorks()
    {
      AppResult[] results = app.WaitForElement(c => c.Marked("content"));
      app.Screenshot("Welcome screen.");

      app.Tap(x => x.Marked("fab"));
      app.Screenshot("After floating action button");

      Assert.IsTrue(results.Any());
    }
  }
}
