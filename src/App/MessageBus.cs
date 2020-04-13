namespace App
{
    public class MessageBus
    {
        private readonly IBus _bus;

        public MessageBus(IBus bus)
        {
            _bus = bus;
        }

        public void SendEmailChangedMessage(long studentId, Email email)
        {
            // Send message
            _bus.Send($"Type: Student_Email_Changed, Id: {studentId}, NewEmail: {email}");
        }
    }
}
