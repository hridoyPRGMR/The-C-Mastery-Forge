namespace P1.Events
{
    /* ✅ Events are a wrapper over delegates, designed for safe publish-subscribe messaging between components.
    They are core to UI programming, game dev, reactive systems, and .NET libraries.

🧠  What Are Events?
    An event is a special kind of delegate that follows the publisher-subscriber pattern.
    Only the class that declares the event can raise it — others can subscribe, but not invoke. */

    public class Events
    {
        public Events()
        {

            var notifier = new Notifier();
            notifier.OnNotify += msg => Console.WriteLine($"Received: {msg}");

            notifier.Trigger();
        }

    }

    // publisher
    public class Notifier
    {
        public delegate void NotificationHandler(string message);
        public event NotificationHandler OnNotify;

        public void Trigger()
        {
            OnNotify?.Invoke("Something Happend");
        }
    }
}