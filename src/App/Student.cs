using System;
using System.Collections.Generic;
using System.Linq;

namespace App
{
    public class Student : Entity
    {
        protected Student() { }

        public Student(Name name, Email email, Course favoriteCourse, Grade favoriteCourseGrade)
        {
            Name = name;
            Email = email;
            FavoriteCourse = favoriteCourse;

            EnrollIn(favoriteCourse, favoriteCourseGrade);
        }

        public virtual Name Name { get; private set; }
        public Email Email { get; private set; }
        public virtual Course FavoriteCourse { get; private set; }

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

        public void EditPersonalInfo(Name name, Email email, Course favoriteCourse)
        {
            if (name == null)
            {
                throw new ArgumentNullException(nameof(name));
            }
            if (email == null)
            {
                throw new ArgumentNullException(nameof(email));
            }
            if (name == null)
            {
                throw new ArgumentNullException(nameof(favoriteCourse));
            }

            Name = name;
            Email = email;
            FavoriteCourse = favoriteCourse;
        }
    }
}
