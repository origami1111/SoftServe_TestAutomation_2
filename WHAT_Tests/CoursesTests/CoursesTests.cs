using NUnit.Framework;
using WHAT_PageObject;

namespace WHAT_Tests
{
    [TestFixture]
    public class CoursesTests : TestBase
    {
        private CoursesPage coursesPage;

        [SetUp]
        public void Setup()
        {
<<<<<<< HEAD
            configuration = new ConfigurationBuilder().AddJsonFile("appsettings.json").Build();
            
            var url = configuration["Url"];
            var credential = configuration.GetSection("Credentials");

            var admin = new Credentials()
            {
                Email = credential["Admin:Email"],
                Password = credential["Admin:Password"]
            };
            
            var mentor = new Credentials()
            {
                Email = credential["Mentor:Email"],
                Password = credential["Mentor:Password"]
            };
            
            driver = new ChromeDriver();
            driver.Navigate().GoToUrl(url);
            driver.Manage().Window.Maximize();
            driver.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(10);

            //coursesPage = new SignInPage(driver)
            //                .SignInAsAdmin(admin.Email, admin.Password)
            //                .SidebarNavigateTo<CoursesPage>();
=======
            coursesPage = new SignIn(driver)
                            .SignInAsAdmin()
                            .SidebarNavigateTo<CoursesPage>();
>>>>>>> 6a0c71d56ccbf1ac0c9b5968b16d1aae587faf68
        }

        [TearDown]
        public void TearDown()
        {
            coursesPage.Logout();
        }

        [Test]
        public void VerifyCourseDetails()
        {
            int courseNumber = 3;
            string expected =  coursesPage.ReadCourseName(courseNumber);
            
            var courseDetailsPage = coursesPage.ClickCourseName(courseNumber);
            string actual = courseDetailsPage.ReadCourseNameDetails();
            driver.Navigate().Back();

            Assert.AreEqual(expected, actual);

        }

        [Test]
        public void EditCourse()
        {
            int courseNumber = 3;
            string courseName = "New course";
            coursesPage.ClickPencilLink(courseNumber);
                     //  .FillCourseName(courseName)
                     //  .ClickCancelButton();

        }


        [Test]
        public void AddCourse()
        {
            string courseName = "New course";
            coursesPage.ClickAddCourseButton()
                       .FillCourseName(courseName)
                       .ClickCancelButton();

        }
    }
}