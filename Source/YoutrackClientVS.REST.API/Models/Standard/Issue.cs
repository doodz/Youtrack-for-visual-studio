using System;
using System.Collections.Generic;
using System.Diagnostics;
using Newtonsoft.Json;

namespace YouTrack.REST.API.Models.Standard
{
    /// <summary>
    /// Wrapper for <see cref="T:System.Collections.Generic.ICollection`1" /> of <see cref="Issue" />.
    /// </summary>
    internal class IssueCollectionWrapper
    {
        /// <summary>
        /// Wrapped <see cref="T:System.Collections.Generic.ICollection`1" /> of <see cref="Issue" />.
        /// </summary>
        [JsonProperty("issue")]
        public ICollection<Issue> Issues { get; set; }
    }



    /// <summary>
    /// A class that represents YouTrack issue information. Can be casted to a <see cref="DynamicObject"/>.
    /// </summary>
    [DebuggerDisplay("{Id}: {Summary}")]
    public class Issue : ObjectFieldList
    {


        /// <summary>
        /// Creates an instance of the <see cref="Issue" /> class.
        /// </summary>
        public Issue()
        {
            Comments = new List<Comment>();
            Tags = new List<SubValue<string>>();
        }

        /// <summary>
        /// Id of the issue.
        /// </summary>
        [JsonProperty("id")]
        public string Id { get; set; }

        /// <summary>
        /// Entity Id internal to YouTrack.
        /// </summary>
        [JsonProperty("entityId")]
        public string EntityId { get; set; }

        /// <summary>
        /// If issue was imported from JIRA, represents the Id it has in JIRA.
        /// </summary>
        [JsonProperty("jiraId")]
        public string JiraId { get; set; }

        /// <summary>
        /// Summary of the issue.
        /// </summary>
        public string Summary
        {
            get
            {
                var field = GetField();
                return field?.Value.ToString();
            }
            set => SetField(value);
        }

        /// <summary>
        /// Description of the issue.
        /// </summary>
        public string Description
        {
            get
            {
                var field = GetField();
                return field?.Value.ToString();
            }
            set => SetField(value);
        }


        /// <summary>
        /// Login of user, who created the issue.
        /// </summary>
        public string ReporterName
        {
            get
            {
                var field = GetField();
                return field?.Value.ToString();
            }
            set => SetField(value);
        }

        /// <summary>
        /// Short name of the issue's project.
        /// </summary>
        public string ProjectShortName
        {
            get
            {
                var field = GetField();
                return field?.Value.ToString();
            }
            set => SetField(value);
        }

        /// <summary>
        /// Login of the user, that was the last, who updated the issue.
        /// </summary>
        public string UpdaterName
        {
            get
            {
                var field = GetField();
                return field?.Value.ToString();
            }
            set => SetField(value);
        }



        /// <summary>
        /// Number of comments in issue
        /// </summary>
        public string CommentsCount
        {
            get
            {
                var field = GetField();
                return field?.Value.ToString();
            }
            set => SetField(value);
        }

        /// <summary>
        /// Time when issue was created.
        /// From the number of milliseconds since January 1, 1970, 00:00:00 GMT represented by this date.
        /// </summary>
        public DateTime Created
        {
            get
            {
                var field = GetField();
                var createdDatetime = field?.Value.ToString();
                return (new DateTime(1970, 1, 1)).AddMilliseconds(double.Parse(createdDatetime));
            }

        }

        /// <summary>
        /// Time when issue was last updated.
        /// From the number of milliseconds since January 1, 1970, 00:00:00 GMT represented by this date.
        /// </summary>
        public DateTime Updated
        {
            get
            {
                var field = GetField();
                var updatedDatetime = field?.Value.ToString();
                return (new DateTime(1970, 1, 1)).AddMilliseconds(double.Parse(updatedDatetime));
            }

        }

        /// <summary>
        /// Issue comments.
        /// </summary>
        [JsonProperty("comment")]
        public ICollection<Comment> Comments { get; set; }

        /// <summary>
        /// Issue tags.
        /// </summary>
        [JsonProperty("tag")]
        public ICollection<SubValue<string>> Tags { get; set; }



    }
}