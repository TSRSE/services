using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.ServiceProcess;
using System.Text;
using System.Threading;
using System.IO;

namespace windowow
{
    class Logger
    {
        FileSystemWatcher watcher;
        object obj = new object();
        bool enabled = true;
        public Logger()
        {
            watcher = new FileSystemWatcher("C:\\Windows\\temp");
            watcher.Deleted += Watcher_deleted;
            watcher.Created += Watcher_created;
            watcher.Changed += Watcher_changed;
            watcher.Renamed += Watcher_renamed;
        }
        public void Start()
        {
            watcher.EnableRaisingEvents = true;
            while (enabled)
            {
                Thread.Sleep(1000);
            }
        }

        public void Stop()
        {
            watcher.EnableRaisingEvents = false;
            enabled = false;
        }

        public void Watcher_renamed(object sender, RenamedEventArgs e)
        {
            string fileEvent = "Renamed to " + e.FullPath;
            string filePath = e.OldFullPath;
            RecordEntry(fileEvent, filePath);
        }

        private void RecordEntry(string fileEvents, string filePath)
        {
            lock (obj)
            {
                using (StreamWriter writer = new StreamWriter("C:\\templog.txt", true))
                {
                    writer.WriteLine(String.Format("{0} file {1} was {2} " , 
                        DateTime.Now.ToString("dd//MM//yyyy"),filePath ,fileEvents));
                    writer.Flush();
                }
            }
        }

        private void Watcher_changed(object sender, FileSystemEventArgs e)
        {
            string fileEvent = "Changed";
            string filePath = e.FullPath;
            RecordEntry(fileEvent, filePath);
        }

        private void Watcher_created(object sender, FileSystemEventArgs e)
        {
            string fileEvent = "Created";
            string filePath = e.FullPath;
            RecordEntry(fileEvent, filePath);
        }

        private void Watcher_deleted(object sender, FileSystemEventArgs e)
        {
            string fileEvent = "Deleted";
            string filePath = e.FullPath;
            RecordEntry(fileEvent, filePath);
        }
    }
    public partial class Service1 : ServiceBase
    {
        Logger logger;
        
        public Service1()
        {
            InitializeComponent();
            this.CanStop = true;
            this.CanPauseAndContinue = true;
            this.AutoLog = true;
        }

        protected override void OnStart(string[] args)
        {
            logger = new Logger();
            Thread loggerThread = new Thread(new ThreadStart(logger.Start));
            loggerThread.Start();
        }

        protected override void OnStop()
        {
            logger.Stop();
            Thread.Sleep(1000);
        }
    }
}
