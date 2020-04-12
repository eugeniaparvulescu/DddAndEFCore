namespace App.Controllers
{
    public class StudentsController
    {
        private readonly SchoolDbContext _dbContext;
        private readonly StudentRepository _repository;

        public StudentsController(SchoolDbContext dbContext, StudentRepository repository)
        {
            _dbContext = dbContext;
            _repository = repository;
        }

        public string CheckStudentFavoriteCourse(long studentId, long courseId)
        {
            var student = _dbContext.Students.Find(studentId);
            if (student == null)
            {
                return "Student not found";
            }

            var course = Course.Find(courseId);
            if (course == null)
            {
                return "Course not found";
            }

            return student.FavoriteCourse == course ? "Yes" : "No";
        }

        public string EnrollStudent(long studentId, long courseId, Grade grade)
        {
            var student = _repository.GetById(studentId);
            if (student == null)
            {
                return "Student not found";
            }

            var course = Course.Find(courseId);
            if (course == null)
            {
                return "Course not found";
            }

            var result = student.EnrollIn(course, grade);

            _dbContext.SaveChanges();

            return result;
        }

        public string DisenrollStudent(long studentId, long courseId)
        {
            var student = _repository.GetById(studentId);
            if (student == null)
            {
                return "Student not found";
            }

            var course = Course.Find(courseId);
            if (course == null)
            {
                return "Course not found";
            }

            student.Disenroll(course);

            _dbContext.SaveChanges();

            return "Ok";
        }

        public string RegisterStudent(string name, string email, long favoriteCourseId, Grade favoriteCourseGrade)
        {
            var course = Course.Find(favoriteCourseId);
            //var course = _dbContext.Courses.Find(favoriteCourseId);
            if (course == null)
            {
                return "Course not found";
            }

            var student = new Student(name, email, course, favoriteCourseGrade);
            _repository.Save(student);
            
            var studentEntityState = _dbContext.Entry(student).State;
            var courseEntitytate = _dbContext.Entry(student.FavoriteCourse).State;
            var enrollmentEntitytate = _dbContext.Entry(student.Enrollments[0]).State;
            var enrollmentCourseEntitytate = _dbContext.Entry(student.Enrollments[0].Course).State;

            _dbContext.SaveChanges();

            return "Ok";
        }

        public string EditPeronalInformation(long studentId, string name, string email, long favoriteCourseId)
        {
            var student = _repository.GetById(studentId);
            if (student == null)
            {
                return "Student not found";
            }

            var course = Course.Find(favoriteCourseId);
            if (course == null)
            {
                return "Course not found";
            }

            student.Name = name;
            student.Email = email;
            student.FavoriteCourse = course;

            var studentEntityState = _dbContext.Entry(student).State;
            var courseEntitytate = _dbContext.Entry(student.FavoriteCourse).State;

            _dbContext.SaveChanges();

            return "Ok";
        }
    }
}
