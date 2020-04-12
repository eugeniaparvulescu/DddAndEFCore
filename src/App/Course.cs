using System.Linq;

namespace App
{
    public class Course : Entity
    {
        protected Course() { }

        private Course(long id, string name) : base(id)
        {
            Name = name;
        }

        public static readonly Course Calculus = new Course(1, "Calculus");
        public static readonly Course Chemistry = new Course(2, "Chemistry");
        public static readonly Course[] Courses = new[] { Calculus, Chemistry };

        public string Name { get; }

        public static Course Find(long id)
        {
            return Courses.FirstOrDefault(x => x.Id == id);
        }
    }
}
