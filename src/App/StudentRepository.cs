namespace App
{
    public class StudentRepository
    {
        private readonly SchoolDbContext _dbContext;

        public StudentRepository(SchoolDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public Student GetById(long id)
        {
            // Initialize Enrollments
            // var student = _dbContext.Students.Include(x => x.Enrollments).Single(x => x.Id == studentId); //1
            var student = _dbContext.Students.Find(id); // prefer this because of the identity map cache
            if (student == null)
            {
                return null;
            }

            _dbContext.Entry(student).Collection(x => x.Enrollments).Load(); //2

            return student;
        }

        public void Save(Student student)
        {
            _dbContext.Attach(student);
        }
    }
}
