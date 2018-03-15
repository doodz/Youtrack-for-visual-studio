using System.ComponentModel.Composition;
using System.Windows.Input;
using YouTrackClientVS.Contracts.Events;
using YouTrackClientVS.Contracts.Interfaces;
using YouTrackClientVS.Contracts.Interfaces.Services;

namespace YouTrackClientVS.Services
{
    [Export(typeof(IMessageBoxService))]
    [PartCreationPolicy(CreationPolicy.NonShared)]
    public class MessageBoxService : IMessageBoxService
    {
        private readonly IEventAggregatorService _eventAggregator;

        [ImportingConstructor]
        public MessageBoxService(IEventAggregatorService eventAggregator)
        {
            _eventAggregator = eventAggregator;
        }

        public void ExecuteCommandWithConfirmation(string title, string message, ICommand command)
        {
            var ev = new ShowConfirmationEvent(title, message,command);
            _eventAggregator.Publish(ev);
        }
    }
}
