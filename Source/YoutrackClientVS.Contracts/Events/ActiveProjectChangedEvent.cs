using YouTrackClientVS.Contracts.Models.YouTrackClientModels;

namespace YouTrackClientVS.Contracts.Events
{
    public class ActiveProjectChangedEvent
    {
        public YouTrackProject ActiveProject { get; private set; }
        public YouTrackProject PreviousProject { get; private set; }

        public ActiveProjectChangedEvent(YouTrackProject activeProject, YouTrackProject previousProject)
        {
            PreviousProject = previousProject;
            ActiveProject = activeProject;
        }

        public bool IsProjectDifferent => ActiveProject?.Name != PreviousProject?.Name;
    }
}