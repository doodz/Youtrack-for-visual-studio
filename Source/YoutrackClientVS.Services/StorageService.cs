using System;
using System.ComponentModel.Composition;
using System.IO;
using log4net;
using Newtonsoft.Json;
using YouTrackClientVS.Contracts.Interfaces.Services;
using YouTrackClientVS.Contracts.Models;
using YouTrackClientVS.Infrastructure;
using YouTrackClientVS.Infrastructure.Extensions;

namespace YouTrackClientVS.Services
{
    [Export(typeof(IStorageService))]
    [PartCreationPolicy(CreationPolicy.NonShared)]
    public class StorageService : IStorageService
    {
        private readonly ILog _logger = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private readonly IFileService _fileService;
        private readonly IHashService _hashService;

        [ImportingConstructor]
        public StorageService(IFileService fileService, IHashService hashService)
        {
            _fileService = fileService;
            _hashService = hashService;
        }

        public Result SaveYouTrackClientHistory(YouTrackClientHistory youTrackClientHistory)
        {
            return Save(youTrackClientHistory, Paths.YouTrackClientHistoryPath);

        }

        public Result<YouTrackClientHistory> LoadYouTrackClientHistory()
        {
            return Load<YouTrackClientHistory>(Paths.YouTrackClientHistoryPath);
        }

        public Result SaveUserData(ConnectionData connectionData)
        {
            return Save(connectionData, Paths.YouTrackClientUserDataPath);
        }

        public Result<ConnectionData> LoadUserData()
        {
            return Load<ConnectionData>(Paths.YouTrackClientUserDataPath);
        }


        private Result<T> Load<T>(string path) where T : class
        {
            try
            {
                if (!File.Exists(path))
                    return Result<T>.Fail();

                return _fileService
                    .Read(path)
                    .Then(_hashService.Decrypt)
                    .Then(JsonConvert.DeserializeObject<T>)
                    .Then(Result<T>.Success);
            }
            catch (Exception ex)
            {
                _logger.Error(ex);
                return Result<T>.Fail(ex);
            }
        }

        private Result Save<T>(T data, string path) where T : class
        {
            try
            {
                JsonConvert.SerializeObject(data)
                    .Then(_hashService.Encrypt)
                    .Then(cred => _fileService.Save(path, cred));
            }
            catch (Exception ex)
            {
                _logger.Error(ex);
                return Result.Fail(ex);
            }

            return Result.Success();
        }

    }
}
