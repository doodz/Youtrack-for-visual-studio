using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ReactiveUI;
using YouTrackClientVS.Contracts.Interfaces;

namespace YouTrackClientVS.Contracts
{
    public class PagedCollection<TData> : ReactiveList<TData>, ISupportIncrementalLoading
    {
        private readonly Func<int, int, Task<IEnumerable<TData>>> _loadTask;
        private readonly int _pageSize;

        public PagedCollection(Func<int, int, Task<IEnumerable<TData>>> loadTask, int pageSize)
        {
            _loadTask = loadTask;
            _pageSize = pageSize;
        }

        public async Task LoadNextPageAsync()
        {
            var pageNumber = (Count / _pageSize);
            var data = (await _loadTask(_pageSize, pageNumber)).ToList();
            var localcount = Count;

            foreach (var item in data.Skip(localcount % _pageSize))
                Add(item);
        }
    }
}