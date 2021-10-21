﻿using System.Collections.Generic;
using System.IO;
using Backups.Archiver;
using Backups.Entities;
using Backups.Storage;

namespace Backups.FileSaver
{
    public class SplitFileSaver : IFileSaver
    {
        public void SaveFiles(
            IArchiver archiver,
            string archivePath,
            IStorage storage,
            IReadOnlyList<JobObject> jobObjects)
        {
            uint counter = 1;
            foreach (JobObject jobObject in jobObjects)
            {
                using Stream stream = archiver.Archive(jobObject);
                storage.SaveFromStream(
                    archiver.GetArchiveNameFromFileName(archivePath + counter),
                    stream);
                ++counter;
            }
        }
    }
}