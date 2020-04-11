namespace App
{
    public class Course
    {
        public Course(string name)
        {
            Name = name;
        }

        public long Id { get; private set; }
        public string Name { get; private set; }
    }
}
