﻿/*
 * Copyright 2010-2011 10gen Inc.
 * file : Constants.cs
 * Licensed under the Apache License, Version 2.0 (the "License");
 * you may not use this file except in compliance with the License.
 * You may obtain a copy of the License at
 * 
 * 
 * http://www.apache.org/licenses/LICENSE-2.0
 * 
 * 
 * Unless required by applicable law or agreed to in writing, software
 * distributed under the License is distributed on an "AS IS" BASIS,
 * WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
 * See the License for the specific language governing permissions and
 * limitations under the License.
 */

namespace MongoDB.Azure.ReplicaSets.ReplicaSetRole
{

    using System;

    using Microsoft.WindowsAzure.ServiceRuntime;

    internal static class Settings
    {
        #region DO NOT MODIFY

        // configuration setting names
        internal const string DataDirSetting = "Two10.MongoDB.ConnectionString";
        internal const string LocalCacheDirSetting = "Two10.MongoDB.MongoDBLocalDataDir";
        internal const string DataDirSizeSetting = "Two10.MongoDB.VHDSize";
        internal const string LogDirSetting = "Two10.MongoDB.MongodLogDir";

        internal const string MongodDataBlobContainerName = "mongoddatadrive{0}";
        internal const string MongodDataBlobName = "mongoddblob{0}.vhd";
        internal const string MongoDBBinaryFolder = @"plugins\MongoDB";
        internal const string MongodLogFileName = "mongod.log";
        internal const string MongodCommandLineCloud = "--port {0} --dbpath {1} --logpath {2} --nohttpinterface --logappend --smallfiles --replSet {3} {4}";
        internal const string MongodCommandLineEmulated = "--port {0} --dbpath {1} --logpath {2} --replSet {3} {4} --oplogSize 10 --smallfiles --noprealloc";

        internal const string MongodDataBlobCacheDir = "Two10.MongoDB.MongodDataBlobCacheDir";
        internal const string LogVerbosity = "Verbose";

        // Default values for configurable settings
        private const int DefaultEmulatedDBDriveSize = 1024; // in MB
        private const int DefaultDeployedDBDriveSize = 100 * 1024; // in MB

        #endregion DO NOT MODIFY

        internal static readonly int MaxDBDriveSizeInMB; // in MB
        internal static string MongodLogLevel = "-v";

        static Settings()
        {
            int dbDriveSize;
            if (RoleEnvironment.IsEmulated)
            {
                dbDriveSize = DefaultEmulatedDBDriveSize;
            }
            else
            {
                dbDriveSize = DefaultDeployedDBDriveSize;
            }

            string mongoDataDirSize = null;
            try
            {
                mongoDataDirSize = RoleEnvironment.GetConfigurationSettingValue(DataDirSizeSetting);
            }
            catch (RoleEnvironmentException)
            {
                // setting does not exist use default
            }
            catch (Exception)
            {
                // setting does not exist?
            }

            if (!string.IsNullOrEmpty(mongoDataDirSize))
            {
                int parsedDBDriveSize = 0;
                if (int.TryParse(mongoDataDirSize, out parsedDBDriveSize))
                {
                    MaxDBDriveSizeInMB = parsedDBDriveSize;
                }
                else
                {
                    MaxDBDriveSizeInMB = dbDriveSize;
                }
            }
            else
            {
                MaxDBDriveSizeInMB = dbDriveSize;
            }

            try
            {
                var logLevel = Utilities.GetLogVerbosity(LogVerbosity);
                if (logLevel != null)
                {
                    MongodLogLevel = logLevel;
                }

            }
            catch (RoleEnvironmentException)
            {
                // setting does not exist use default
            }
            catch (Exception)
            {
                // setting does not exist?
            }
        }

    }
}
