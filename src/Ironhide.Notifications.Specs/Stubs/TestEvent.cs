namespace Starscream.Notifications.Specs.Testing
{
    public class TestEvent
    {
        public TestEvent(object command)
        {
            Command = command;
        }

        public object Command { get; private set; }
    }
}