using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.ServiceProcess;
using System.Configuration.Install;
using System.Linq;
using System.Threading.Tasks;

namespace windowow
{
    [RunInstaller(true)]
    public partial class Installer1 : System.Configuration.Install.Installer
    {
        ServiceInstaller serviceInstaller;
        ServiceProcessInstaller processInstaller;
        //private System.ComponentModel.IContainer components = null;

        public Installer1()
        {
            InitializeComponent();
            //components = new System.ComponentModel.Container();
            serviceInstaller = new ServiceInstaller();
            processInstaller = new ServiceProcessInstaller();
            processInstaller.Account = ServiceAccount.LocalSystem;
            serviceInstaller.StartType = ServiceStartMode.Manual;
            serviceInstaller.ServiceName = "s1lv3r";
            Installers.Add(processInstaller);
            Installers.Add(serviceInstaller);
        }
    }
}
