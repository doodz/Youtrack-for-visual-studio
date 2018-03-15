using System;
using System.Collections.Generic;
using System.Linq;
using log4net;
using YouTrackClientVS.Contracts.Interfaces;

namespace YouTrackClientVS.Infrastructure
{
    public abstract class ViewModelBase : ReactiveValidatedObject, IViewModel, IDisposable
    {
        private IEnumerable<IDisposable> _disposables;

        private readonly ILog _logger =
            LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        protected ViewModelBase()
        {
            (this as IViewModelWithCommands)?.InitializeCommands();
        }

        public void InitializeObservables()
        {
            _disposables = SetupObservables().ToList();
        }

        protected virtual IEnumerable<IDisposable> SetupObservables()
        {
            return Enumerable.Empty<IDisposable>();
        }

        public virtual void Dispose()
        {
            foreach (var obs in _disposables ?? Enumerable.Empty<IDisposable>())
                obs?.Dispose();
        }
    }
}
