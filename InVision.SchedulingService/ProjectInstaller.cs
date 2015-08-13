using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration.Install;
using System.Linq;
using System.Xml;
using System.ServiceProcess;


namespace InVision.SchedulingService
{
    /// <summary>
    /// Installs the windows service and process.
    /// </summary>
    [RunInstaller(true)]
    public partial class ProjectInstaller : Installer
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ProjectInstaller"/> class.
        /// </summary>
        public ProjectInstaller()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Gets the context parameter.
        /// </summary>
        /// <param name="key">The key.</param>
        /// <returns>The value associated withteh key or null if the key does not exist.</returns>
        private string GetContextParameter(string key)
        {
            return (this.Context.Parameters.ContainsKey(key) ? this.Context.Parameters[key] : null);
        }

        /// <summary>
        /// Handles the BeforeInstall event of the ProjectInstaller control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.Configuration.Install.InstallEventArgs"/> instance containing the event data.</param>
        private void ProjectInstaller_BeforeInstall(object sender, InstallEventArgs e)
        {
            //get the name of the service from the installer context
            //context parameters can be passed in on the InstallUtil command 
            //line or through a user interface
            windowsServiceInstaller.ServiceName = this.GetContextParameter("name") ?? windowsServiceInstaller.ServiceName;
            windowsServiceInstaller.DisplayName = this.GetContextParameter("display") ?? windowsServiceInstaller.DisplayName;

            //set the service name in the app.config file            
            XmlDocument appConfig = new XmlDocument();
            appConfig.Load(this.GetContextParameter("assemblypath") + ".config");
            XmlNode serviceNameNode = appConfig.SelectSingleNode("configuration/applicationSettings/InVision.SchedulingService.Properties.Settings/setting[@name='ServiceName']/value");
            serviceNameNode.InnerText = windowsServiceInstaller.ServiceName;
            appConfig.Save(this.GetContextParameter("assemblypath") + ".config");

            //get the service credentials from the installer context
            string account = this.GetContextParameter("account") ?? serviceProcessInstaller.Account.ToString();
            serviceProcessInstaller.Account = (ServiceAccount)(Enum.Parse(typeof(ServiceAccount), account, true));

            //if using a user account, get credentials from installer context
            //if credentials are left empty, InstallUtil will automatically prompt
            if (serviceProcessInstaller.Account == ServiceAccount.User)
            {
                serviceProcessInstaller.Username = this.GetContextParameter("user") ?? string.Empty;
                serviceProcessInstaller.Password = this.GetContextParameter("password") ?? string.Empty;
            }

            //TODO: insert other installation requirements here
            //NOTE: there is no need to install an Event Log source for the service. 
            //It is installed automatically if AutoLog is set to true on the Service class.
        }

        /// <summary>
        /// Handles the BeforeUninstall event of the ProjectInstaller control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.Configuration.Install.InstallEventArgs"/> instance containing the event data.</param>
        private void ProjectInstaller_BeforeUninstall(object sender, InstallEventArgs e)
        {
            //need to get the name of the service to be uninstalled
            windowsServiceInstaller.ServiceName = this.GetContextParameter("name") ?? windowsServiceInstaller.ServiceName;
        }
    }
}
