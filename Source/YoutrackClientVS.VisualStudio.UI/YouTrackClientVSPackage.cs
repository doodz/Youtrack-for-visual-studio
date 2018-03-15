//------------------------------------------------------------------------------
// <copyright file="YouTrackClientVSPackage.cs" company="Company">
//     Copyright (c) Company.  All rights reserved.
// </copyright>
//------------------------------------------------------------------------------

using log4net;
using Microsoft.VisualStudio.ComponentModelHost;
using Microsoft.VisualStudio.Shell;
using Microsoft.VisualStudio.Shell.Interop;
using System;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Threading;
using System.Windows;
using YouTrackClientVS.Contracts.Interfaces.Services;
using YouTrackClientVS.Infrastructure;
using YouTrackClientVS.UI.Helpers;
using YouTrackClientVS.VisualStudio.UI.Settings;
using YouTrackClientVS.VisualStudio.UI.Window;
using Task = System.Threading.Tasks.Task;

namespace YouTrackClientVS.VisualStudio.UI
{
    /// <summary>
    /// This is the class that implements the package exposed by this assembly.
    /// </summary>
    /// <remarks>
    /// <para>
    /// The minimum requirement for a class to be considered a valid package for Visual Studio
    /// is to implement the IVsPackage interface and register itself with the shell.
    /// This package uses the helper classes defined inside the Managed Package Framework (MPF)
    /// to do it: it derives from the Package class that provides the implementation of the
    /// IVsPackage interface and uses the registration attributes defined in the framework to
    /// register itself and its components with the shell. These attributes tell the pkgdef creation
    /// utility what data to put into .pkgdef file.
    /// </para>
    /// <para>
    /// To get loaded into VS, the package must be referred by &lt;Asset Type="Microsoft.VisualStudio.VsPackage" ...&gt; in .vsixmanifest file.
    /// </para>
    /// </remarks>
    /// 
    [PackageRegistration(UseManagedResourcesOnly = true, AllowsBackgroundLoading = true)]
    [InstalledProductRegistration("#110", "#112", "1.0", IconResourceID = 400)] // Info on this package for Help/About
    [Guid(GuidList.guidBitbuketPkgString)]
    [ProvideMenuResource("Menus.ctmenu", 1)]
    [ProvideAutoLoad(UIContextGuids80.SolutionExists)]
    [ProvideAutoLoad(UIContextGuids80.NoSolution)]
    //[ProvideToolWindow(typeof(DiffWindow), Style = VsDockStyle.MDI, Orientation = ToolWindowOrientation.Left,
    //     MultiInstances = true, Transient = true)]
    [ProvideToolWindow(typeof(YouTrackIssuesWindow), Style = VsDockStyle.Tabbed, Orientation = ToolWindowOrientation.Bottom,
         MultiInstances = false, Transient = true)]

    public sealed class YouTrackClientVsPackage : AsyncPackage
    {
        private readonly ILog _logger = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

        public const string ActivityLogName = "YouTrackExtension";
        public const string GitExtensionsId = "11B8E6D7-C08B-4385-B321-321078CDD1F8";

        static YouTrackClientVsPackage()
        {
            AssemblyResolver.InitializeAssemblyResolver();
        }

        public YouTrackClientVsPackage()
        {
            Application.Current.DispatcherUnhandledException += Current_DispatcherUnhandledException;
            AppDomain.CurrentDomain.UnhandledException += CurrentDomain_UnhandledException;
        }

        #region Package Members

        private void CurrentDomain_UnhandledException(object sender, UnhandledExceptionEventArgs e)
        {
            _logger.Error("Unhandled YouTrackClientVsExtensions Error: " + e.ExceptionObject);
        }

        private void Current_DispatcherUnhandledException(object sender,
            System.Windows.Threading.DispatcherUnhandledExceptionEventArgs e)
        {
            _logger.Error("Unhandled Dispatcher YouTrackClientVsExtensions Error", e.Exception);
        }

        protected override async Task InitializeAsync(CancellationToken cancellationToken, IProgress<ServiceProgressData> progress)
        {
            await base.InitializeAsync(cancellationToken, progress);
            var componentModel = (IComponentModel)await GetServiceAsync(typeof(SComponentModel));

            await InitializePackageAsync(componentModel);
        }

        private async Task InitializePackageAsync(IComponentModel componentModel)
        {
            try
            {
                var serviceProvider = componentModel.DefaultExportProvider;

                Application.Current.Resources.Add(Consts.IocResource, serviceProvider);
                var appInitializer = serviceProvider.GetExportedValue<IAppInitializer>();
                var commandsService = serviceProvider.GetExportedValue<ICommandsService>();
                var gitWatcher = serviceProvider.GetExportedValue<IGitWatcher>();
                var userInformationService = serviceProvider.GetExportedValue<IUserInformationService>();
                commandsService.Initialize(this);
                gitWatcher.Initialize();
                await appInitializer.Initialize();
                await userInformationService.Initialize();

                _logger.Info("Initialized YouTrackClientVsPackage Extension");
            }
            catch (Exception ex)
            {
                _logger.Error($"Error Loading YouTrackClientVsPackage: {ex}");
            }
        }

        #endregion
    }
}
