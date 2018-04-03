using System;
using System.Collections.Generic;

namespace YouTrackClientVS.Contracts.Interfaces.AutoCompleteTextBox
{
    public interface IAutoCompleteQuerySource
    {
        string Title { get; }

        Func<IAutoCompleteQuery, IObservable<IEnumerable<IAutoCompleteQueryResult>>> QueryResultFunction { get; }
    }
}
