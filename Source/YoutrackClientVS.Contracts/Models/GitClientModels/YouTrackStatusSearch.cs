namespace YouTrackClientVS.Contracts.Models.GitClientModels
{
    /// <summary>
    /// https://www.jetbrains.com/help/youtrack/standalone/Search-and-Command-Attributes.html#state
    /// </summary>
    public enum YouTrackStatusSearch
    {
        Resolved,
        Unresolved,
        Submitted,
        Open,
        //In%20Progress,
        InProgress,
        Reopened,
        //To%20be%20discussed,
        ToBeDiscussed,
        Duplicate,
        Incomplete,
        Obsolete
    }
}