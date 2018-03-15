using System;
using System.Collections.Generic;
using YouTrack.REST.API.Interfaces;

namespace YouTrack.REST.API.QueryBuilders
{
    public class AttachFileQueryBuilder : IAttachFileQueryBuilder
    {
        private string _attachmentName;
        private string _group;
        private string _author;


        public string GetAttachmentName()
        {
            return _attachmentName;
        }

        public IAttachFileQueryBuilder SetAttachmentName(string attachmentName)
        {
            _attachmentName = attachmentName;
            return this;
        }

        public IAttachFileQueryBuilder SetGroup(string group)
        {
            _group = group;
            return this;
        }

        public IAttachFileQueryBuilder Setauthor(string author)
        {
            _author = author;
            return this;
        }

        public Dictionary<string, string> GetQueryParameters()
        {
            if (string.IsNullOrEmpty(_attachmentName))
            {
                throw new ArgumentNullException(nameof(_attachmentName));
            }
            var _params = new Dictionary<string, string>();



            _params.Add("attachmentName", _attachmentName);

            if (!string.IsNullOrEmpty(_group))
                _params.Add("group", _group);


            if (!string.IsNullOrEmpty(_author))
                _params.Add("author", _author);


            return _params;
        }
    }
}