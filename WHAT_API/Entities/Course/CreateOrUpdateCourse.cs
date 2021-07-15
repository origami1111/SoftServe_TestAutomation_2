namespace WHAT_API
{
    public class CreateOrUpdateCourse
    {
        public CreateOrUpdateCourse(string name)
        {
            Name = name;
        }

        public string Name { get; set; }
    }
}
