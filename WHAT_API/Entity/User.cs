namespace WHAT_API
{
    public class User
    {
        public int id { get; set; }
        public string email { get; set; }
        public string firstName { get; set; }
        public string lastName { get; set; }
        public object avatarUrl { get; set; }
        public override string ToString()
        {

            return $"{id}, {email}, {firstName}, {lastName}";
        }
    }
    
}
