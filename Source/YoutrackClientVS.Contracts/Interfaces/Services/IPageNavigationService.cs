using System;
using YouTrackClientVS.Contracts.Events;
using YouTrackClientVS.Contracts.Interfaces.Views;

namespace YouTrackClientVS.Contracts.Interfaces.Services
{
    public interface IPageNavigationService<TWindow> : IObservable<NavigationEvent> where TWindow : IWorkflowWindow
    {
        void NavigateBack(bool removeFromHistory = false);
        void Navigate<TView>(object parameter = null) where TView : class, IView;
        IObservable<bool> CanNavigateBackObservable { get; }
        IObservable<bool> CanNavigateForwardObservable { get; }
        void NavigateForward();
        void ClearNavigationHistory();
    }
}