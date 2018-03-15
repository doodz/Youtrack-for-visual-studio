using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

namespace YouTrackClientVS.Contracts.Models.YouTrackClientModels
{


    /// <summary>
    /// Represents an <see cref="Assignee" /> for an <see cref="Issue" />.
    /// </summary>
    public class YouTrackAssignee
    {
        /// <summary>
        /// Username.
        /// </summary>
        public string UserName { get; set; }

        /// <summary>
        /// Full name.
        /// </summary>
        public string FullName { get; set; }
    }


    /// <summary>
    /// Represents a sub value which contains a <see cref="TSubValueType"/> value.
    /// </summary>
    public struct YouTrackSubValue<TSubValueType>
    {
        /// <summary>
        /// Value.
        /// </summary>
        public TSubValueType Value;
    }

    /// <summary>
    /// Represents a YouTrack issue field.
    /// </summary>
    [DebuggerDisplay("{Name}: {Value}")]
    public class YouTrackField
    {
        /// <summary>
        /// Name.
        /// </summary>
        public string Name;

        /// <summary>
        /// Value.
        /// </summary>
        public object Value;

        /// <summary>
        /// Value Id.
        /// </summary>
        public object ValueId;

        /// <summary>
        /// Gets the value as a <see cref="T:System.String"/>.
        /// </summary>
        /// <returns><see cref="Value" /> as <see cref="T:System.String"/>.</returns>
        public string AsString()
        {
            switch (Value)
            {
                case null:
                    return null;
                case ICollection<string> collection:
                    return collection.SingleOrDefault();
            }

            return Value.ToString();
        }

        /// <summary>
        /// Gets the value as a <see cref="T:System.Collections.Generic.ICollection{System.String}"/>.
        /// </summary>
        /// <returns><see cref="Value" /> as <see cref="T:System.Collections.Generic.ICollection{System.String}"/>.</returns>
        public ICollection<string> AsCollection()
        {
            switch (Value)
            {
                case null:
                    return new List<string>();
                case ICollection<string> collection:
                    return collection;
                default:
                    return new List<string>
                    {
                        Value.ToString()
                    };
            }
        }

        /// <summary>
        /// Gets the value as a <see cref="T:System.DateTime"/>.
        /// </summary>
        /// <returns><see cref="Value" /> as <see cref="T:System.DateTime"/>.</returns>
        public DateTime AsDateTime()
        {
            switch (Value)
            {
                case DateTime dateTime:
                    return dateTime;
                case DateTimeOffset dateTimeOffset:
                    return dateTimeOffset.DateTime;
                default:
                    var milliseconds = Convert.ToInt64(AsString());

                    return DateTimeOffset.FromUnixTimeMilliseconds(milliseconds).DateTime;
            }
        }

        /// <summary>
        /// Gets the value as a <see cref="T:System.Int32"/>.
        /// </summary>
        /// <returns><see cref="Value" /> as <see cref="T:System.Int32"/>.</returns>
        public int AsInt32()
        {
            return Convert.ToInt32(AsString());
        }
    }


    /// <summary>
    /// A class that represents YouTrack issue information.
    /// </summary>
    [DebuggerDisplay("{Id}: {Summary}")]
    public class YouTrackIssue
    {
        /// <summary>
        /// Creates an instance of the <see cref="Issue" /> class.
        /// </summary>
        public YouTrackIssue()
        {
            Comments = new List<YouTrackComment>();
            Tags = new List<YouTrackSubValue<string>>();
        }

        /// <summary>
        /// Id of the issue.
        /// </summary>
        public string Id { get; set; }

        /// <summary>
        /// Entity Id internal to YouTrack.
        /// </summary>
        public string EntityId { get; set; }

        /// <summary>
        /// If issue was imported from JIRA, represents the Id it has in JIRA.
        /// </summary>
        public string JiraId { get; set; }

        /// <summary>
        /// Summary of the issue.
        /// </summary>
        public string Summary
        { get; set; }
        /// <summary>
        /// Description of the issue.
        /// </summary>
        public string Description
        { get; set; }

        /// <summary>
        ///  Login of user, who created the issue.
        /// </summary>
        public string ReporterName
        { get; set; }

        /// <summary>
        /// Login of the user, that was the last, who updated the issue
        /// </summary>
        public string UpdaterName
        { get; set; }


        /// <summary>
        /// Time when issue was created.
        /// From the number of milliseconds since January 1, 1970, 00:00:00 GMT represented by this date.
        /// </summary>
        public DateTime Created
        { get; set; }

        /// <summary>
        /// Time when issue was last updated.
        /// From the number of milliseconds since January 1, 1970, 00:00:00 GMT represented by this date.
        /// </summary>
        public DateTime Updated
        { get; set; }

        /// <summary>
        /// Short name of the issue's project
        /// </summary>
        public string ProjectShortName
        { get; set; }


        /// <summary>
        /// Number of comments in issue
        /// </summary>
        public string CommentsCount
        { get; set; }

        /// <summary>
        /// Issue comments.
        /// </summary>
        public ICollection<YouTrackComment> Comments { get; set; }

        /// <summary>
        /// Issue tags.
        /// </summary>
        public ICollection<YouTrackSubValue<string>> Tags { get; set; }


    }
}