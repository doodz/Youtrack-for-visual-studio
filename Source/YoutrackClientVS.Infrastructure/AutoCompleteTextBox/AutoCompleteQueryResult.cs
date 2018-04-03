using YouTrackClientVS.Contracts.Interfaces.AutoCompleteTextBox;

namespace YouTrackClientVS.Infrastructure.AutoCompleteTextBox
{
    public class AutoCompleteQueryResult : IAutoCompleteQueryResult
    {
        public string Title { get; set; }
        public string Description { get; set; }
        public long Cursor { get; set; }
    }
}
