using System.Collections.Generic;
using System.Linq;
using YouTrack.REST.API.Interfaces;

namespace YouTrack.REST.API.QueryBuilders
{
    /// <summary>
    /// https://www.jetbrains.com/help/youtrack/standalone/Search-and-Command-Attributes.html#state
    /// </summary>
    public enum StateSearch
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


    /// <summary>
    /// https://www.jetbrains.com/help/youtrack/standalone/Search-and-Command-Attributes.html
    /// </summary>
    public class tmp
    {
        //filter=State:%20resolved
        //filter=State%3Aresolved
        //filter=%23Resolved
        //filter=%23Unresolved
        //filter=%23open
        //filter=for%3A+me
        //filter=state%3A+submitted
        //filter=sort%20by:%20updated%20desc
    }


    public class IssueQueryBuilder : IIssueQueryBuilder
    {
        private bool _wikifyDescription;
        private int _after;
        private int _max;
        private readonly ICollection<string> _withLst = new List<string>();
        private readonly ICollection<string> _filterLst = new List<string>();

        /// <summary>
        /// Defines whether the issue description is returned as HTML or plain text.
        /// </summary>
        /// <param name="include"></param>
        /// <returns></returns>
        public IIssueQueryBuilder WithWikifyDescription(bool include)
        {
            _wikifyDescription = include;
            return this;
        }

        /// <summary>
        /// A number of issues to skip before getting a list of issues. For example, 
        /// if you specify after=12 in the request,
        /// then in the response you will get all issues matching request but without the first twelve issues found.
        /// </summary>
        /// <param name="after"></param>
        /// <returns></returns>
        public IIssueQueryBuilder After(int after)
        {
            _after = after;
            return this;
        }

        /// <summary>
        /// The maximum number of issues to include in the result. If empty, 10 issues are returned.
        /// </summary>
        /// <param name="after"></param>
        /// <returns></returns>
        public IIssueQueryBuilder Max(int max)
        {
            _max = max;
            return this;
        }

        /// <summary>
        /// List of fields that should be included in the result. 
        /// For example, the request get /issue?with=comment&with=Priority returns a list of issues only with these specific fields in the result. 
        /// Other fields will not be included in the result xml.
        /// </summary>
        /// <param name="with"></param>
        /// <returns></returns>
        public IIssueQueryBuilder AddWith(string with)
        {
            _withLst.Add(with);
            return this;
        }


        public IIssueQueryBuilder AddFilter(string filter)
        {
            _filterLst.Add(filter);
            return this;
        }

        private const string FilterQuery = "filter";
        private const string AfterQuery = "after";
        private const string MaxQuery = "max";
        private const string WithQuery = "with";
        private const string UpdateAfterQuery = "updatedAfter";
        private const string WikifyDescriptionQuery = "wikifyDescription";

        public Dictionary<string, string> GetQueryParameters()
        {
            var _params = new Dictionary<string, string>();
            if (_filterLst.Any()) _params.Add(FilterQuery, string.Join(" ", _filterLst));



            _params.Add(WikifyDescriptionQuery, _wikifyDescription.ToString());
            if (_after > 0)
                _params.Add(AfterQuery, _after.ToString());

            if (_max > 0)
                _params.Add(MaxQuery, _max.ToString());


            foreach (var s in _withLst) _params.Add(WithQuery, s);

            return _params;
        }
    }
}