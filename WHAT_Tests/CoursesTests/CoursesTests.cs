using NUnit.Framework;
using WHAT_PageObject;

namespace WHAT_Tests
{
    [TestFixture]
    public class CoursesTests : TestBase
    {
        private CoursesPage coursesPage;

        [SetUp]
        public void Precondition()
        {
<<<<<<< HEAD
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
=======
            var credentials = ReaderFileJson.ReadFileJsonCredentials(@"DataFiles\Credentials.json", Role.Admin);
            coursesPage = new SignInPage(driver)
                            .SignInAsAdmin(credentials.Email, credentials.Password)
>>>>>>> a2f8b9a178aa40ed188457277634d9bb53e58ace
                            .SidebarNavigateTo<CoursesPage>();
>>>>>>> 6a0c71d56ccbf1ac0c9b5968b16d1aae587faf68
        }

        [TearDown]
        public void Postcondition()
        {
            coursesPage.Logout();
        }

        [Test]
        public void VerifyCourseDetails()
        {
            int courseNumber = 3;
            string expected =  coursesPage.ReadCourseName(courseNumber);
            
            var courseDetailsPage = coursesPage.ClickCourseName(courseNumber);
            string actual = courseDetailsPage.GetCourseNameDetails();

            Assert.AreEqual(expected, actual);

        }

        [Test]
        public void EditCourse_CLickClearButton()
        {
            int courseNumber = 3;
            string expected = coursesPage.ReadCourseName(courseNumber);

            var actual = coursesPage.ClickPencilLink(courseNumber)
                                  //  .DeleteTextWithBackspaces()
                                  //  .ClickClearButton()
                                    .GetCourseName();

            Assert.AreEqual(expected, actual);
        }

        [Test]
        public void AddCourse_ValidData()
        {
            string courseName = "New course";
            coursesPage.ClickAddCourseButton()
                       .FillCourseNameField(courseName)
                       .ClickCancelButton();
        }

        [TestCase("a", "Too short")]
        [TestCase("Course name with more than fifty characters is too long", "Too long")]
        [TestCase(" Space before course name", "Invalid course name")]
        [TestCase("More than one space   between words", "Invalid course name")]
        [TestCase("Space after course name ", "Invalid course name")]
        [TestCase("Course name with special symbols: C#, .Net", "Invalid course name")]
        [TestCase("Not only Latin letters Кириллица", "Invalid course name")]
        [TestCase("Course name with numbers 12", "Invalid course name")]
        public void AddCourseWithInvalidData(string invalidData, string expected)
        {
            var actual = coursesPage.ClickAddCourseButton()
                                    .FillCourseNameField(invalidData);
            
            Assert.True(expected == actual.GetErrorMessage() && actual.IsSaveButtonDisabled());
        }

        [Test]
        public void AddCourse_EmptyName()
        {
            var expected = "This field is required";
            var anyData = "Test";
            
            var actual = coursesPage.ClickAddCourseButton()
                                    .FillCourseNameField(anyData)
                                    .DeleteTextWithBackspaces(anyData.Length);
            
            Assert.True(expected == actual.GetErrorMessage() && actual.IsSaveButtonDisabled());
        }
    }
}