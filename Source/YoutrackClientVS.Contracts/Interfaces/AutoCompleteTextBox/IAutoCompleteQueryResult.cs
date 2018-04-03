namespace YouTrackClientVS.Contracts.Interfaces.AutoCompleteTextBox
{
    public interface IAutoCompleteQueryResult
    {
        string Title { get; }
        string Description { get; }
        long Cursor { get; }
    }
}