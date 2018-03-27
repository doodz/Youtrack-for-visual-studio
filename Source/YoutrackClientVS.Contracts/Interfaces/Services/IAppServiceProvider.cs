using System;

namespace YouTrackClientVS.Contracts.Interfaces.Services
{
    public interface IAppServiceProvider : IServiceProvider, IDisposable
    {
        IServiceProvider YouTrackServiceProvider { get; set; }
        TService GetService<TService>() where TService : class;

        //TRet GetService<TService, TRet>() where TService : class
        //    where TRet : class;

        //object TryGetService(Type t);
        //object TryGetService(string typename);
        //TService TryGetService<TService>() where TService : class;

        void AddService<TService>(TService obj, object owner);

        void RemoveService(Type t, object owner);



    }
}