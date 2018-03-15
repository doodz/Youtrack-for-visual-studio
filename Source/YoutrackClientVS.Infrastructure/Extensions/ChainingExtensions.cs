using System;
using System.Diagnostics;

namespace YouTrackClientVS.Infrastructure.Extensions
{
    public static class ChainingExtensions
    {
        [DebuggerStepThrough]
        public static TInput Then<TInput>(this TInput obj, Action<TInput> action)
        {
            action(obj);
            return obj;
        }
        [DebuggerStepThrough]
        public static TOutput Then<TInput, TOutput>(this TInput obj, Func<TInput, TOutput> action)
        {
            return action(obj);
        }
    }
}