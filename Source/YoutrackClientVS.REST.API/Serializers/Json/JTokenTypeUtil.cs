using Newtonsoft.Json.Linq;

namespace YouTrack.REST.API.Serializers.Json
{
    internal static class JTokenTypeUtil
    {
        public static bool IsSimpleType(JTokenType tokenType)
        {
            // ReSharper disable once SwitchStatementMissingSomeCases
            switch (tokenType)
            {
                case JTokenType.Boolean:
                case JTokenType.Float:
                case JTokenType.Integer:
                case JTokenType.Guid:
                case JTokenType.String:
                case JTokenType.Uri:
                    return true;

                default:
                    return false;
            }
        }
    }
}
