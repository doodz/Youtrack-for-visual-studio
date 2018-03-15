// <copyright file="TeamExplorerBase.cs" company="Microsoft Corporation">Copyright Microsoft Corporation. All Rights Reserved. This code released under the terms of the Microsoft Public License (MS-PL, http://opensource.org/licenses/ms-pl.html.) This is sample code only, do not use in production environments.</copyright>

using System;
using Microsoft.TeamFoundation.Client;
using Microsoft.TeamFoundation.Controls;
using YouTrackVSIX.Core;

namespace YouTrackClientVS.TeamFoundation.TeamFoundation
{
    /// <summary>
    /// Team Explorer extension common base class.
    /// </summary>
    public class TeamExplorerBase : ObservableObject, IDisposable
    {
        private bool _contextSubscribed;
        private IServiceProvider _serviceProvider;

        public static readonly Guid TeamExplorerConnectionsSectionId = new Guid("ef6a7a99-f01f-4c91-ad31-183c1354dd97");


        /// <summary>
        /// Get/set the service provider.
        /// </summary>
        public IServiceProvider ServiceProvider
        {
            get => _serviceProvider;

            set
            {
                // Unsubscribe from Team Foundation context changes
                if (_serviceProvider != null) UnsubscribeContextChanges();


                _serviceProvider = value;
                CheckPackage();
                // Subscribe to Team Foundation context changes
                if (_serviceProvider != null) SubscribeContextChanges();
            }
        }

        // Visual doesnt want to load our package automatically when GUID is specified in ProvideAutoLoad
        // this trick is a temporary workaround, we check if package has been loaded, if not we load it manually using VS API
        private void CheckPackage() //todo
        {
            //try
            //{
            //    IVsShell shell = serviceProvider?.GetService(typeof(SVsShell)) as IVsShell;
            //    if (shell == null) return;

            //    // always false, why?
            //    // IVsPackage gitPackage;
            //    // Guid gitExtensionPackage = new Guid(GitClientVSPackage.GitExtensionsId);
            //    // var isGitLoaded = shell.IsPackageLoaded(ref gitExtensionPackage, out gitPackage);

            //    IVsPackage package;
            //    Guid packageToBeLoadedGuid =
            //        new Guid(GuidList.guidBitbuketPkgString);

            //    var isLoaded = shell.IsPackageLoaded(ref packageToBeLoadedGuid, out package);
            //    ActivityLog.LogInformation(GitClientVSPackage.ActivityLogName, "Bitbucket package is loaded: " + (Microsoft.VisualStudio.VSConstants.S_OK == isLoaded));

            //    if (Microsoft.VisualStudio.VSConstants.S_OK != isLoaded)
            //    {
            //        ActivityLog.LogWarning(GitClientVSPackage.ActivityLogName, "Package was not loaded, trying to load it manually...");
            //        shell.LoadPackage(ref packageToBeLoadedGuid, out package);
            //        ActivityLog.LogWarning(GitClientVSPackage.ActivityLogName, "Package has been loaded");
            //    }

            //}
            //catch (Exception ex)
            //{
            //    ActivityLog.LogError(GitClientVSPackage.ActivityLogName, ex.ToString());
            //}
        }

        protected ITeamFoundationContext CurrentContext
        {
            get
            {
                var tfcontextManager = GetService<ITeamFoundationContextManager>();
                return tfcontextManager?.CurrentContext;
            }
        }

        public T GetService<T>()
        {
            if (ServiceProvider != null) return (T)ServiceProvider.GetService(typeof(T));

            return default(T);
        }

        public virtual void Dispose()
        {
            UnsubscribeContextChanges();
        }

        public Guid ShowNotification(string message, NotificationType type)
        {
            var teamExplorer = GetService<ITeamExplorer>();
            if (teamExplorer == null) return Guid.Empty;

            var guid = Guid.NewGuid();
            teamExplorer.ShowNotification(message, type, NotificationFlags.None, null, guid);
            return guid;

        }

        protected void SubscribeContextChanges()
        {
            if (ServiceProvider == null || _contextSubscribed) return;

            var tfcontextManager = GetService<ITeamFoundationContextManager>();
            if (tfcontextManager == null) return;

            tfcontextManager.ContextChanged += ContextChanged;
            _contextSubscribed = true;
        }

        protected void UnsubscribeContextChanges()
        {
            if (ServiceProvider == null || !_contextSubscribed) return;

            var tfcontextManager = GetService<ITeamFoundationContextManager>();
            if (tfcontextManager != null) tfcontextManager.ContextChanged -= ContextChanged;
        }

        protected virtual void ContextChanged(object sender, ContextChangedEventArgs e)
        {
        }
    }
}