using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using Process = System.Diagnostics.Process;

namespace ProxyManager
{
    internal static class Program
    {
        // TODO: Use a FileSystemWatcher to monitor changes to the network settings file and reload as needed.

        private static string[] _appArgs;
        private static string _appDataPath;
        private static string _appPath;
        private static Process _childProc;
        private static bool _isChildProc;
        private static NetworkWatcher _netWatcher;
        private static ObservableCollection<NetworkSettings> _networkSettings;
        private static Process _parentProc;
        private static RegistryUtils.RegistryMonitor _regWatcher;
        private static List<DateTime> childExitTimes;
        private static bool debuggerAttached;


        /// <summary>Gets the application data path.</summary>
        public static string AppDataPath
        {
            get { return _appDataPath; }
        }

        /// <summary>Gets the application path.</summary>
        public static string AppPath
        {
            get { return Program._appPath; }
        }

        /// <summary>Gets the start arguments.</summary>
        public static string[] Arguments
        {
            get { return _appArgs; }
        }

        /// <summary>Gets or sets a value indicating whether the program starts automatically.</summary>
        /// <value>
        /// <c>true</c> if the program starts automatically; otherwise, <c>false</c>.
        /// </value>
        public static bool AutoStart
        {
            get
            {
                var hkcu = Microsoft.Win32.Registry.CurrentUser;
                var runPath = hkcu.OpenSubKey(@"Software\Microsoft\Windows\CurrentVersion\Run");
                return runPath.GetValue(Program.Name, "").ToString().ToLower() == AppPath.ToLower();
            }
            set
            {
                var hkcu = Microsoft.Win32.Registry.CurrentUser;
                var runPath = hkcu.OpenSubKey(@"Software\Microsoft\Windows\CurrentVersion\Run", true);
                // delete the key only if the program is set to autorun
                if (!value && Program.AutoStart)
                    runPath.DeleteValue(Program.Name);
                else
                    runPath.SetValue(Program.Name, AppPath);
            }
        }

        public static System.Drawing.Icon Icon
        {
            get { return Properties.Resources.appIcon; }
        }

        public static string Name
        {
            get { return "Auto Proxy Manager"; }
        }

        public static IList<NetworkSettings> NetworkSettings
        {
            get { return Program._networkSettings; }
        }

        public static NetworkWatcher NetworkWatcher
        {
            get { return Program._netWatcher; }
            set { Program._netWatcher = value; }
        }

        public static RegistryUtils.RegistryMonitor RegistryWatcher
        {
            get { return Program._regWatcher; }
        }

        /// <summary>Gets the program version.</summary>
        internal static Version Version
        {
            get
            {
                return System.Reflection.Assembly.GetExecutingAssembly().GetName().Version;
            }
        }

        /// <summary>Gets the child process.</summary>
        /// <value>The child process.</value>
        private static Process ChildProcess
        {
            get { return _childProc; }
        }

        /// <summary>Gets a value indicating whether this instance is a child process.</summary>
        /// <value>
        /// <c>true</c> if this instance is a child process; otherwise, <c>false</c>.
        /// </value>
        private static bool IsChildProcess
        {
            get { return _isChildProc; }
        }

        /// <summary>Gets the parent process.</summary>
        /// <value>The parent process.</value>
        private static Process ParentProcess
        {
            get { return _parentProc; }
        }


        /// <summary>Exits this program instance.</summary>
        public static void Exit()
        {
            System.Windows.Forms.Application.Exit();
        }

        /// <summary>Loads the network settings.</summary>
        public static void LoadNetworkSettings()
        {
            var settingsFile = System.IO.Path.Combine(AppDataPath, "networkSettings.json");
            _networkSettings = new ObservableCollection<NetworkSettings>();

            if (System.IO.File.Exists(settingsFile))
            {
                var jset = new JsonSerializerSettings();
                jset.DefaultValueHandling = DefaultValueHandling.IgnoreAndPopulate;
                jset.Converters.Add(new NetworkConnectionInfo.JsonConverter());

                var fileText = System.IO.File.ReadAllText(settingsFile);
                var savedSettings = JsonConvert.DeserializeObject<List<NetworkSettings>>(fileText);
                _networkSettings = new ObservableCollection<NetworkSettings>(savedSettings);
            }

            if (_networkSettings == null)
                _networkSettings = new ObservableCollection<NetworkSettings>();

            // add event handlers for the network settings collection
            _networkSettings.CollectionChanged += networkSettings_CollectionChanged;
        }

        /// <summary>Saves the network settings.</summary>
        public static void SaveNetworkSettings()
        {
            var settingsFile = System.IO.Path.Combine(AppDataPath, "networkSettings.json");

            var jset = new JsonSerializerSettings();
            jset.DefaultValueHandling = DefaultValueHandling.IgnoreAndPopulate;
            jset.Converters.Add(new NetworkConnectionInfo.JsonConverter());

            var fileText = JsonConvert.SerializeObject(_networkSettings, Formatting.Indented, jset);

            System.IO.File.WriteAllText(settingsFile, fileText);
        }

        private static void Application_ThreadException(object sender,
            System.Threading.ThreadExceptionEventArgs e)
        {
            var settings = new JsonSerializerSettings { DefaultValueHandling = DefaultValueHandling.Include };
            var fileContents = JsonConvert.SerializeObject(e.Exception, Formatting.Indented, settings);
            fileContents = "// AppVersion: " + Program.Version + "\r\n" + fileContents;

            var desktopDir = Environment.GetFolderPath(Environment.SpecialFolder.DesktopDirectory);
            var appVersionDir = Program.Name;
            var crashFile = DateTime.UtcNow.ToString("u").Replace(":", "") + ".json";
            var crashDirPath = Path.Combine(desktopDir, appVersionDir);
            var crashFilePath = Path.Combine(crashDirPath, crashFile);

            try
            {
                Directory.CreateDirectory(crashDirPath);
                File.WriteAllText(crashFilePath, fileContents);

                string format = "An exception of type {0} occured.\n" +
                                "Exception Message: {1}\n\n" +
                                "Email the following file to <falexge@gmail.com>\n\"{2}\"";

                // notify the user of the exception that occured
                MessageBox.Show(string.Format(format, e.Exception.GetType().ToString(),
                                              e.Exception.Message, crashFilePath),
                                "Unhandled Exception Encountered", MessageBoxButtons.OK, MessageBoxIcon.Hand);
            }
            catch
            {
                // exception while creating the crash dir or writing crash file
                var catchMsg = "An exception of type {0} occured but could not be documented.\n" +
                               "Please contact the developer at <falexge@gmail.com> for a solution.";
                MessageBox.Show(string.Format(catchMsg, e.Exception.GetType().ToString()));
            }

            // prompt to restart the application
            if (MessageBox.Show("The exception could have left the program in an unstable state.\n" +
                                "Do you want to restart it?", "Restart Application",
                                MessageBoxButtons.YesNo, MessageBoxIcon.Exclamation
                                ) == DialogResult.Yes)
            {
                Environment.ExitCode = 1;
                Application.Exit();
                Environment.Exit(1); // just in case the Application.Exit() call fails.
            }
            else
            {
                Environment.Exit(0); // user wants program to exit
            }
        }

        /// <summary>Determines if child process can be started.</summary>
        /// <returns><c>true</c> if child process can be started; otherwise <c>false</c>.</returns>
        private static bool CanStartChildProcess()
        {
            /*
             * start a child process if any of these conditions are valid
             *   1. There is no debugger attached to this instance
             *   2. This instance is not a child process
             *   3. Program start arguments does not contain SingleInstance
             *   4. Parent process exited before this child could be started
             */

            if (debuggerAttached || IsChildProcess)
                return false;

            return !Program.Arguments.Contains(ValidArgs.RunInSelf) ||
                   (ParentProcess != null && ParentProcess.HasExited);
        }

        /// <summary>Determines whether this instance this instance can be started.</summary>
        /// <returns><c>true</c> if this instance can be started; otherwise <c>false</c>.</returns>
        private static bool CanStartThisInstance()
        {
            // check for the "newinstance" argument
            var forceNewInstance = false;
            foreach (var arg in Program.Arguments)
            {
                if (arg == ValidArgs.NewInstance)
                    forceNewInstance = true;
                else if (arg.StartsWith(ValidArgs.ParentProcId))
                {
                    var id = int.Parse(arg.Replace(ValidArgs.ParentProcId, "").Trim());
                    _parentProc = Process.GetProcessById(id);
                    _isChildProc = true;
                }
            }

            if (forceNewInstance) return true; // force starting process


            // check for a previous instance
            var appExeFile = new System.IO.FileInfo(AppPath).Name;
            appExeFile = appExeFile.Remove(appExeFile.LastIndexOf('.')); // remove the extension

            var procs = Process.GetProcessesByName(appExeFile);
            var thisProc = Process.GetCurrentProcess();
            debuggerAttached = System.Diagnostics.Debugger.IsAttached;

            /*
             * Conditions for starting this instance:
             *  1. This is the only process
             *  2. More than one process and one of the existing processes is this one's parent
             *  3. A debugger is attached to this process
             */
            return (procs.Length == 1 && procs[0].Id == thisProc.Id) ||     // condition 1
                   (procs.Length > 1 && _parentProc != null &&
                   procs.Select(p => p.Id).Contains(_parentProc.Id)) ||     // condition 2
                   debuggerAttached;                                        // condition 3  
        }

        /// <summary>Gets the a path where the application data can be written to.</summary>
        private static string GetAppDataPath()
        {
            var appDir = new System.IO.FileInfo(_appPath).Directory;
            var appDrive = new System.IO.DriveInfo(appDir.Root.FullName);
            var localAppData = System.Environment.GetEnvironmentVariable("appdata") + "\\" + Program.Name;
            var programFiles = Environment.GetFolderPath(Environment.SpecialFolder.ProgramFiles);
            var windir = Environment.GetEnvironmentVariable("windir");

            try
            {
                // try to create a temporary file in the app path to confirm read/write access
                var tmp = System.IO.Path.Combine(appDir.FullName, System.IO.Path.GetRandomFileName());
                var fs = System.IO.File.Open(tmp, System.IO.FileMode.OpenOrCreate,
                                             System.IO.FileAccess.ReadWrite);
                fs.Close();
                System.IO.File.Delete(tmp);

                // file access confirmed. Ensure we're not in %windir% or %programfiles%
                if (appDir.FullName.Contains(programFiles) || appDir.FullName.Contains(windir))
                    return localAppData;
                //x else if (appDrive.DriveType == System.IO.DriveType.Fixed &&
                //x          System.IO.Directory.GetFiles(appDir.FullName, "portable*").Length > 0)
                //x     return appDir.FullName; // fixed drive, portable specifier
                else
                    return appDir.FullName;
            }
            catch (Exception)
            {
                // we don't have read/write access to application directory.
                return localAppData;
            }
        }

        /// <summary>The main entry point for the application.</summary>
        [STAThread]
        private static void Main(string[] args)
        {
            var thisProc = Process.GetCurrentProcess();
            _appPath = thisProc.MainModule.FileName;
            _appArgs = args.Select(s => s.TrimStart('/', '-').ToLower().Trim()).ToArray();

            Application.ThreadException += Application_ThreadException;
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);


            // ensure we can start this instance
            if (!CanStartThisInstance())
            {
                MessageBox.Show("Another instance of " + Program.Name + " is already running.\n\n" +
                                "Only one instance can run at a time.", "Previous Instance Detected",
                                MessageBoxButtons.OK, MessageBoxIcon.Information);
                return; // terminate program execution
            }


            // prompt to enable auto-start on boot if the noAutorun flag is absent (main process only)
            if (!IsChildProcess && !Program.Arguments.Contains(ValidArgs.NoAutorun))
                SetAutoStart();


            /*  Start child process in the following conditions
             *    1. This is not a child process
             *    2. There is no debugger attached
             *    3. The singleProcess argument was not passed
             *    4. Parent process exited before this one is completely initialized
             */
            if (CanStartChildProcess())
            {
                childExitTimes = new List<DateTime>();
                // recursively start new child processes if exit code is not 0 (successful exit)
                do
                {
                    var childProcArgs = ValidArgs.ParentProcId + thisProc.Id.ToString();
                    _childProc = Process.Start(AppPath.Replace(".vshost.exe", ".exe"), childProcArgs);

                    _childProc.WaitForExit();

                    System.Threading.Thread.Sleep(1000);
                } while (ShouldRestartChildProc(_childProc.ExitCode));

                return; // terminate program execution
            }


            //!+ Only child process can get here.

            // add event handler to exit this instance when the parent process exits
            if (ParentProcess != null && !ParentProcess.HasExited)
            {
                ParentProcess.EnableRaisingEvents = true;
                ParentProcess.Exited += ParentProcess_Exited;
            }


            // initialize data directories and data files
            var netConfigRegPath = @"HKCU\Software\Microsoft\Windows\CurrentVersion\Internet Settings";
            _appDataPath = GetAppDataPath();
            LoadNetworkSettings();

            // initialize the network and registry watchers
            _netWatcher = new NetworkWatcher();
            _regWatcher = new RegistryUtils.RegistryMonitor(netConfigRegPath);
            _regWatcher.RegChangeNotifyFilter = RegistryUtils.RegChangeNotifyFilter.Value;


            Application.Run(new frmMain());


            // stop watchers
            NetworkWatcher.Stop();
            RegistryWatcher.Stop();
            SaveNetworkSettings(); // save settings before exiting
        }

        private static void networkSettings_CollectionChanged(
            object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        {
            // don't perform any action for drastic changes like when the list is cleared.
            if (e.Action == System.Collections.Specialized.NotifyCollectionChangedAction.Reset)
                return;

            // for anything else, just save the new configuration
            SaveNetworkSettings();
        }

        private static void ParentProcess_Exited(object sender, EventArgs e)
        {
            // exit when parent process exits
            Program.Exit();
        }

        private static void SetAutoStart()
        {
            // prompt user to set program to start automatically if it is being run from HDD
            if (new System.IO.DriveInfo(AppPath).DriveType == System.IO.DriveType.Fixed &&
                !AutoStart &&
                MessageBox.Show("Do you want this program to run automatically at startup?",
                                Name, MessageBoxButtons.YesNo, MessageBoxIcon.Question
                                ) == DialogResult.Yes)
            {
                AutoStart = true;
            }
        }

        /// <summary>Determines if the child process should be restarted.</summary>
        /// <param name="exitCode">The exit code of the child process.</param>
        /// <returns>
        ///   <c>true</c> if the child process should be restarted; otherwise <c>false</c>.
        /// </returns>
        private static bool ShouldRestartChildProc(int exitCode)
        {
            if (exitCode == 0) return false; // don't restart on clean exit

            // add exit time to list and clear those older than 2 minutes
            var now = DateTime.Now;
            var maxMins = 2;
            childExitTimes.Add(now);
            childExitTimes.RemoveAll(d => now.Subtract(d).TotalMinutes > maxMins);

            var msgText = "{0} has restarted {1} times in the past {2} minutes.\r\n" +
                          "Do you want to continue running it?";

            // allow restart if less than three exits in the past two minutes or 
            // the user wants the program to be restarted
            if (childExitTimes.Count < 3 ||
                MessageBox.Show(string.Format(msgText, Program.Name, childExitTimes.Count, maxMins),
                                Program.Name, MessageBoxButtons.YesNo, MessageBoxIcon.Exclamation,
                                MessageBoxDefaultButton.Button2) == DialogResult.Yes)
                return true;
            else
                return false;
        }
        internal static class ValidArgs
        {
            public const string NewInstance = "newinstance";
            public const string NoAutorun = "noautorun";
            public const string ParentProcId = "parentid:";
            public const string RunInSelf = "singleprocess";
        }
    }
}