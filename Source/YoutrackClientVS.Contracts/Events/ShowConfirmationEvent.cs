using System.Windows.Input;

namespace YouTrackClientVS.Contracts.Events
{
    public class ShowConfirmationEvent
    {
        public ICommand Command { get; }
        public string Title { get; }
        public string Message { get; }


        public ShowConfirmationEvent(string title, string message, ICommand command)
        {
            Command = command;
            Title = title;
            Message = message;
        }
    }
}
