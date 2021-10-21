﻿using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Linq;
using Backups.Archiver;
using Backups.FileSaver;
using Backups.Storage;

namespace Backups.Entities
{
    public class RestorePoint
    {
        private string _archivePath;
        private IArchiver _archiver;
        private IFileSaver _fileSaver;
        private List<IArchive> _archives;

        public RestorePoint(
            IArchiver archiver,
            string archivePath,
            IReadOnlyList<JobObject> jobObjects,
            IFileSaver fileSaver,
            IStorage storage)
        {
            _archiver = archiver;
            _archivePath = archivePath;
            _fileSaver = fileSaver;
            _archives = new List<IArchive>();

            _fileSaver.SaveFiles(archiver, archivePath, storage, jobObjects);
        }

        public string PointPath => _archivePath;
        public string PointName => Path.GetFileName(_archivePath);
    }
}