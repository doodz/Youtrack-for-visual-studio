using System;
using System.Collections.Generic;
using System.Reactive.Linq;
using YouTrackClientVS.Contracts.Interfaces.AutoCompleteTextBox;

namespace YouTrackClientVS.Infrastructure.AutoCompleteTextBox
{
    public class AutoCompleteQueryResultProvider
    {
        private static readonly Func<IAutoCompleteQuery, IObservable<IEnumerable<IAutoCompleteQueryResult>>>
            EmptyResultSet = q => Observable.Empty<IEnumerable<IAutoCompleteQueryResult>>();

        private static readonly AutoCompleteQueryResultProvider EmptyResultProvider =
            new AutoCompleteQueryResultProvider(EmptyResultSet);

        public static AutoCompleteQueryResultProvider Empty => EmptyResultProvider;

        public AutoCompleteQueryResultProvider()
        {
            GetResults = EmptyResultSet;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="AutoCompleteQueryResultProvider" /> class.
        /// </summary>
        /// <param name="getResults">The get results.</param>
        public AutoCompleteQueryResultProvider(
            Func<IAutoCompleteQuery, IObservable<IEnumerable<IAutoCompleteQueryResult>>> getResults)
        {
            GetResults = getResults;
        }

        public Func<IAutoCompleteQuery, IObservable<IEnumerable<IAutoCompleteQueryResult>>> GetResults
        {
            get;
            private set;
        }
    }
}