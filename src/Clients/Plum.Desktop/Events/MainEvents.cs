using Prism.Events;

namespace Plum.Events
{
    internal class MainWindowLoadingEvent : PubSubEvent<bool> { }

    internal class SignUpSuccessEvent : PubSubEvent<SignUpArgs> { }

    internal class SettingSeccessEvent : PubSubEvent { }
}