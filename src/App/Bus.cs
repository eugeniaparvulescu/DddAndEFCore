using System;

namespace App
{
    public class Bus : IBus
    {
        public void Send(string message)
        {
            // Send the message on the bus
            Console.WriteLine($"Message sent: {message}");
        }
    }
}
