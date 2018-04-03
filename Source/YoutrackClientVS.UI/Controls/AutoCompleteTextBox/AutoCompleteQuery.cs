using YouTrackClientVS.Contracts.Interfaces.AutoCompleteTextBox;

namespace YouTrackClientVS.UI.Controls.AutoCompleteTextBox
{
    public class AutoCompleteQuery : IAutoCompleteQuery
    {
        public AutoCompleteQuery(string term)
        {
            Term = term;
        }

        public string Term { get; private set; }
    }
}