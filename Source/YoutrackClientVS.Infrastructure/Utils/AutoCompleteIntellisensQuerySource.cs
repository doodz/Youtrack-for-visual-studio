using ReactiveUI;
using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Reactive.Linq;
using System.Threading.Tasks;
using YouTrackClientVS.Contracts.Interfaces.AutoCompleteTextBox;
using YouTrackClientVS.Contracts.Interfaces.Services;
using YouTrackClientVS.Contracts.Interfaces.ViewModels;
using YouTrackClientVS.Contracts.Models.YouTrackClientModels;
using YouTrackClientVS.Infrastructure.AutoCompleteTextBox;
using YouTrackClientVS.Infrastructure.Extensions;

namespace YouTrackClientVS.Infrastructure.Utils
{

    [Export(typeof(IAutoCompleteIntellisenseQuerySource))]
    [PartCreationPolicy(CreationPolicy.NonShared)]
    public class AutoCompleteIntellisenseQuerySource : ViewModelBase, IAutoCompleteIntellisenseQuerySource
    {
        private readonly IYouTrackClientService _youTrackClientService;
        private YouTrackIntellisense _intellisense;
        public string Title => "YouTrack Intellisense";


        public YouTrackIntellisense Intellisense
        {
            get => _intellisense;
            set => this.RaiseAndSetIfChanged(ref _intellisense, value);
        }

        [ImportingConstructor]
        public AutoCompleteIntellisenseQuerySource(IYouTrackClientService youTrackClientService)
        {
            _youTrackClientService = youTrackClientService;
        }


        public Func<IAutoCompleteQuery, IObservable<IEnumerable<IAutoCompleteQueryResult>>>
            QueryResultFunction
        {
            get { return query => Observable.FromAsync(() => RunIntellisenseAsync(query.Term)); }
        }


        private async Task<IEnumerable<IAutoCompleteQueryResult>> RunIntellisenseAsync(string queryTerm)
        {
            Intellisense = (await _youTrackClientService.GetIntellisense(null, queryTerm));
            return Intellisense.Suggest.MapTo<List<AutoCompleteQueryResult>>();
        }
    }
}