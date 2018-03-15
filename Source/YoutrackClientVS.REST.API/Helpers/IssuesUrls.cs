namespace YouTrack.REST.API.Helpers
{
    public static class IssuesUrls
    {

        public static string CreateIssue()
        {
            return "issue";
        }

        public static string Exists(string issueId)
        {

            return $"issue/{issueId}/exists";
        }

        public static string DeleteIssue(string issueId)
        {

            return $"issue/{issueId}";
        }

        public static string GetLinksForIssue(string issueId)
        {

            return $"issue/{issueId}/link";
        }


        public static string GetIssue(string issueId, bool wikifyDescription = false)
        {
            return $"issue/{issueId}?wikifyDescription={wikifyDescription}";
        }

        public static string UpdateIssue(string issueId)
        {
            return $"issue/{issueId}";
        }

        public static string ApplyCommand(string issueId)
        {
            return $"issue/{issueId}/execute";
        }

        public static string GetIssuesByProject(string projectId)
        {
            return $"issue/byproject/{projectId}";
        }

        public static string GetIssues()
        {
            return "issue";
        }

        public static string GetCommentsForIssue(string issueId, bool wikifyDescription = false)
        {
            return $"issue/{issueId}/comment?wikifyDescription={wikifyDescription}";

        }

        public static string DeleteCommentForIssue(string issueId, string commentId, bool permanent = false)
        {
            return $"/issue/{issueId}/comment/{commentId}?permanently={permanent.ToString().ToLowerInvariant()}";
        }

        public static string UpdateCommentForIssue(string issueId, string commentId)
        {
            return $"issue/{issueId}/comment/{commentId}";
        }

        public static string AttachFileToIssue(string issueId)
        {
            return $"issue/{issueId}/attachment";
        }


        public static string GetAttachmentsForIssue(string issueId)
        {
            return $"issue/{issueId}/attachment";
        }

        public static string DeleteAttachmentForIssue(string issueId, string attachmentId)
        {
            return $"issue/{issueId}/attachment/{attachmentId}";
        }
    }
}