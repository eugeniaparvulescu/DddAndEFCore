namespace App
{
    public class Student
    {
        public Student(string name, string email, long favoriteCourseId)
        {
            Name = name;
            Email = email;
            FavoriteCourseId = favoriteCourseId;
        }

        public long Id { get; private set; }
        public string Name { get; private set; }
        public string Email { get; private set; }
        public long FavoriteCourseId { get; private set; }
    }
}
