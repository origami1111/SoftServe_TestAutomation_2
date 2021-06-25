using OpenQA.Selenium;

namespace WHAT_PageObject.Base
{
    public class SignIn
    {
        private const string PATH = @"C:\Users\origami\Desktop\what-tests\WHAT_DP_205_TAQC\WHAT_PageObject\Files\Credentials.json";
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
