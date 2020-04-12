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
    }
}
