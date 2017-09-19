using System;
using System.IO;
using System.Linq;
using System.Net;
using System.Reflection;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Hosting.Server.Features;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using System.Diagnostics;
using FluentAssertions;
using System.Threading;
using Xunit;

namespace RazorPagesFromScratch.Tests.IntegrationTests
{
    [Trait("Category", "Integration")]
    public class SeleniumTestFixture<TStartup> : IDisposable
    {
        public string BaseAddress;
        public IWebDriver webDriver;
        IWebHost builder;
        int MAX_WAIT = 10000;
        public SeleniumTestFixture() : this(Path.Combine(""))
        {

        }

        protected SeleniumTestFixture(string relativeTargetProjectParentDir)
        {
            var startupAssembly = typeof(Startup).GetTypeInfo().Assembly;
            var contentRoot = GetProjectPath(relativeTargetProjectParentDir, startupAssembly);
            var random = new Random();
            var listenPort = random.Next(10000, 20000);
             builder = new WebHostBuilder()
                .UseKestrel(options =>
                {
                    options.Listen(IPAddress.Loopback, listenPort);
                })
               .UseContentRoot(contentRoot)
               // .ConfigureServices(InitializeServices)
               //.UseEnvironment("Development")
               .UseStartup(typeof(TStartup)).Build();
            builder.Start();

           BaseAddress = builder.ServerFeatures.Get<IServerAddressesFeature>().Addresses.Single();
            //Console.WriteLine("baseaddress: {0}",BaseAddress);
            //var o = new ChromeOptions();
            //o.AddArguments("disable-extensions");
            //o.AddArguments("--start-maximized");

            //webDriver = new ChromeDriver(o);
            webDriver = new ChromeDriver();
        }

        /// <summary>
        /// Gets the full path to the target project that we wish to test
        /// </summary>
        /// <param name="projectRelativePath">
        /// The parent directory of the target project.
        /// e.g. src, samples, test, or test/Websites
        /// </param>
        /// <param name="startupAssembly">The target project's assembly.</param>
        /// <returns>The full path to the target project.</returns>
        private static string GetProjectPath(string projectRelativePath, Assembly startupAssembly)
        {
            // Get name of the target project which we want to test
            var projectName = startupAssembly.GetName().Name;

            // Get currently executing test project path
            var applicationBasePath = System.AppContext.BaseDirectory;

            // Find the path to the target project
            var directoryInfo = new DirectoryInfo(applicationBasePath);
            do
            {
                directoryInfo = directoryInfo.Parent;

                var projectDirectoryInfo = new DirectoryInfo(Path.Combine(directoryInfo.FullName, projectRelativePath));
                if (projectDirectoryInfo.Exists)
                {
                    var projectFileInfo = new FileInfo(Path.Combine(projectDirectoryInfo.FullName, projectName, $"{projectName}.csproj"));
                    if (projectFileInfo.Exists)
                    {
                        return Path.Combine(projectDirectoryInfo.FullName, projectName);
                    }
                }
            }
            while (directoryInfo.Parent != null);

            throw new Exception($"Project root could not be located using the application root {applicationBasePath}.");
        }
        protected IWebElement FindItemTextInputBox()
        {

            // Francis starts a new list by entering a new item. He
            // is less interesting than Edith...
            return webDriver.FindElement(By.Name("Item.Text"));
        }
        protected void WaitForRowInListTable(string rowText)
        {
            var startTime = DateTime.Now;
            Stopwatch sw = Stopwatch.StartNew();

            while (true)
            {
                try
                {
                    var table = webDriver.FindElement(By.Id("id_list_table"));
                    var rows = table.FindElements(By.TagName("tr"));
                    //Assert.Contains(rows, row => row.Text == rowText);
                    rows.Should().Contain(row => row.Text == rowText, $"[{rowText}] not found in table");
                    Console.WriteLine("Elapsed miliseconds: {0}", sw.ElapsedMilliseconds);
                    sw.Stop();
                    return;
                }
                catch (WebDriverException)
                {
                    if (sw.ElapsedMilliseconds > MAX_WAIT)
                    {
                        Console.WriteLine("Elapsed miliseconds: {0}", sw.ElapsedMilliseconds);
                        sw.Stop();
                        webDriver.Quit();
                        throw;
                    }
                    Console.WriteLine("Going to sleep for half second");
                    Thread.Sleep(500);

                }
            }
        }
        public void Dispose()
        {
            webDriver.Dispose();
            builder.Dispose();
        }
    }
}
