using OpenQA.Selenium;

namespace WHAT_PageObject.Base
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
