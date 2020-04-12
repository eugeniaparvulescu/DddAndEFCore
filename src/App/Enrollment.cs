using System;
using System.Collections.Generic;
using System.Text;

namespace App
{
    public class Enrollment : Entity
    {
        protected Enrollment() { }
        
        public Enrollment(Course course, Student student, Grade grade)
        {
            Course = course;
            Student = student;
            Grade = grade;
        }

        public Grade Grade { get; }
        public virtual Student Student { get; }
        public virtual Course Course { get; }
    }
}
