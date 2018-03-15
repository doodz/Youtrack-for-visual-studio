using System.Collections.Generic;

namespace YouTrack.REST.API.Interfaces
{
    public interface IIssueQueryBuilder
    {

        Interfaces.IIssueQueryBuilder AddFilter(string filter);
        Interfaces.IIssueQueryBuilder AddWith(string with);
        Interfaces.IIssueQueryBuilder After(int after);
        Dictionary<string, string> GetQueryParameters();
        Interfaces.IIssueQueryBuilder Max(int max);
        Interfaces.IIssueQueryBuilder WithWikifyDescription(bool include);
    }
}