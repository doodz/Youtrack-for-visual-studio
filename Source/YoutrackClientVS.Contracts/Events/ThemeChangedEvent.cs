using YouTrackClientVS.Contracts.Models;

namespace YouTrackClientVS.Contracts.Events
{
    public class ThemeChangedEvent
    {
        public Theme Theme { get; set; }

        public ThemeChangedEvent(Theme theme)
        {
            Theme = theme;
        }
    }
}
