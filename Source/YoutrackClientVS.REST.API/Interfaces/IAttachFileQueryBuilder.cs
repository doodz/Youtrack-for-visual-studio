using System.Collections.Generic;

namespace YouTrack.REST.API.Interfaces
{
    public interface IAttachFileQueryBuilder
    {
        Interfaces.IAttachFileQueryBuilder SetAttachmentName(string attachmentName);
        Interfaces.IAttachFileQueryBuilder SetGroup(string group);
        Interfaces.IAttachFileQueryBuilder Setauthor(string author);
        string GetAttachmentName();

        Dictionary<string, string> GetQueryParameters();

    }
}