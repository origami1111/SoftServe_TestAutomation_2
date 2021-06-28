using OpenQA.Selenium;
using WHAT_PageObject;

namespace WHAT_Tests
{
    public class SignIn
    {
        private const string PATH = @"DataFiles\Credentials.json";
        private IWebDriver driver;
        private SignInPage signInPage;

        public SignIn(IWebDriver driver)
        {
            this.driver = driver;
            signInPage = new SignInPage(driver);
        }

        public LessonsPage SignInAsMentor()
        {
            Credentials credentials = ReaderFileJson.ReadFileJsonCredentials(PATH, Role.Mentor);

            signInPage.
                FillEmail(credentials.Email).
                FillPassword(credentials.Password).
                ClickSignInButton();

            return new LessonsPage(driver);
        }
<<<<<<< HEAD:WHAT_PageObject/Base/SignIn.cs


=======
/*
        public MentorsPage SignInAsSecretar()
        {
            Credentials credentials = ReaderFileJson.ReadFileJsonCredentials(PATH, Role.Secretar);

            signInPage.
                FillEmail(credentials.Email).
                FillPassword(credentials.Password).
                ClickSignInButton();

            return new MentorsPage(driver);
        }

        public SupportPage SignInAsStudent()
        {
            Credentials credentials = ReaderFileJson.ReadFileJsonCredentials(PATH, Role.Student);

            signInPage.
                FillEmail(credentials.Email).
                FillPassword(credentials.Password).
                ClickSignInButton();

            return new SupportPage(driver);
        }
*/
>>>>>>> 6a0c71d56ccbf1ac0c9b5968b16d1aae587faf68:WHAT_Tests/BaseForTests/SignIn.cs
        public StudentsPage SignInAsAdmin()
        {
            Credentials credentials = ReaderFileJson.ReadFileJsonCredentials(PATH, Role.Admin);

            signInPage.
               FillEmail(credentials.Email).
               FillPassword(credentials.Password).
               ClickSignInButton();

            return new StudentsPage(driver);
        }
    }
}
