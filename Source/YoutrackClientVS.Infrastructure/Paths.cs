using System;
using System.IO;

namespace YouTrackClientVS.Infrastructure
{
    public static class Paths
    {
        public static string YouTrackClientStorageDirectory => Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), @"YouTrackClientVSExtension");
        public static string YouTrackClientLogFilePath => Path.Combine(YouTrackClientStorageDirectory, "Logs", "logs.txt");
        public static string YouTrackClientUserDataPath => Path.Combine(YouTrackClientStorageDirectory, "User", "data.dat");
        public static string YouTrackClientHistoryPath => Path.Combine(YouTrackClientStorageDirectory, "User", "history.dat");

        public static string DefaultRepositoryPath => Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments), "Source", "Repos");
    }
}
