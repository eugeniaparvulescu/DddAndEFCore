using System.Collections.Generic;
using System.Linq;

namespace App
{
    public class Student : Entity
    {
        protected Student() { }

        public Student(string name, string email, Course favoriteCourse, Grade favoriteCourseGrade)
        {
            Name = name;
            Email = email;
            FavoriteCourse = favoriteCourse;

            EnrollIn(favoriteCourse, favoriteCourseGrade);
        }

        public string Name { get; set; }
        public string Email { get; set; }
        public virtual Course FavoriteCourse { get; set; }

        private readonly List<Enrollment> _enrollments = new List<Enrollment>();
        public virtual IReadOnlyList<Enrollment> Enrollments => _enrollments.ToList();

        public string EnrollIn(Course course, Grade grade)
        {
            if (_enrollments.Any(x => x.Course == course))
            {
                return $"Already enrolled in course {course}";
            }

            var enrollment = new Enrollment(course, this, grade);
            _enrollments.Add(enrollment);

            return "Ok";
        }

        public void Disenroll(Course course)
        {
            var enrollment = _enrollments.FirstOrDefault(x => x.Course == course);
            if (enrollment == null)
            {
                return;
            }

            _enrollments.Remove(enrollment);
        }
    }
}
