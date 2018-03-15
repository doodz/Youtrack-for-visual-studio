using System;
using System.ComponentModel.Composition;
using System.Threading.Tasks;
using AutoMapper;
using log4net;
using YouTrackClientVS.Contracts.Interfaces.Services;
using YouTrackClientVS.Contracts.Models;
using YouTrackClientVS.Infrastructure;
using YouTrackClientVS.Infrastructure.Mappings;

namespace YouTrackClientVS.VisualStudio.UI
{
    [Export(typeof(IAppInitializer))]
    [PartCreationPolicy(CreationPolicy.NonShared)]
    public class AppInitializer : IAppInitializer
    {
        private readonly ILog _logger = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private readonly IStorageService _storageService;
        private readonly IYouTrackClientService _youTrackClient;
        private readonly IUserInformationService _userInformationService;

        [ImportingConstructor]
        public AppInitializer(
            IStorageService storageService,
            IYouTrackClientService youTrackClient,
            IUserInformationService userInformationService
            )
        {
            _storageService = storageService;
            _youTrackClient = youTrackClient;
            _userInformationService = userInformationService;
        }

        public async Task Initialize()
        {
            try
            {
                LoggerConfigurator.Setup();
                _userInformationService.StartListening();
                var result = _storageService.LoadUserData();

                Mapper.Initialize(cfg =>
                {
                    cfg.AddProfile<YouTrackMappingsProfile>();
                    BitBucketEnterpriseMappings.AddEnterpriseProfile(cfg);
                });

                await YouTrackClientLogin(result);
            }
            catch (Exception e)
            {
                _logger.Error("Error during App initialization: " + e);
            }
        }

        private async Task YouTrackClientLogin(Result<ConnectionData> result)
        {
            if (result.IsSuccess && result.Data.IsLoggedIn)
            {
                try
                {
                    var cred = new YouTrackCredentials()
                    {
                        Login = result.Data.UserName,
                        Password = result.Data.Password,
                        Host = result.Data.Host
                    };
                    await _youTrackClient.LoginAsync(cred);
                }
                catch (Exception ex)
                {
                    _logger.Warn("Couldn't login user using stored credentials. Credentials must have been changed or there is no internet connection", ex);
                }
            }
        }
    }
}
