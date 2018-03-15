using System;
using YouTrackClientVS.Contracts.Interfaces;

namespace YouTrackClientVS.Contracts.Events
{
    public class NavigationEvent : EventArgs
    {
        public Type Window { get; }
        public IView View { get; }
        public object Parameter { get; set; }

        public NavigationEvent(Type window, IView view)
        {
            Window = window;
            View = view;
        }
    }
}
