using System;
using System.Threading.Tasks;
using YouTrackVSIX.Core;

namespace YouTrackClientVS.UI.Converters
{
    public class TaskCompletionNotifier<TResult> : ObservableObject
    {
        private TResult _result;

        public TResult Result
        {
            get => _result;
            set
            {
                _result = value;
                SetProperty(ref _result, value);
            }
        }


        public async Task StartAsync(Task<TResult> task, TResult defaultValue = default(TResult))
        {
            try
            {
                Result = await task;
            }
            catch (Exception)
            {
                Result = default(TResult);
            }
        }
    }
}