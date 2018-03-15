using System.Collections.Generic;
using ParseDiff;

namespace YouTrackClientVS.Contracts.Interfaces.Services
{
    public interface IDiffFileParser
    {
        IEnumerable<FileDiff> Parse(string diff);
    }
}