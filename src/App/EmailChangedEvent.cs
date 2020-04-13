namespace App
{
    public class EmailChangedEvent : IDomainEvent
    {
        public EmailChangedEvent(long studentId, Email email)
        {
            StudentId = studentId;
            Email = email;
        }
        public long StudentId { get; }
        public Email Email { get; }
    }
}
