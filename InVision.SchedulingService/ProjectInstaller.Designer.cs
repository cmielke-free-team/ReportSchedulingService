namespace InVision.SchedulingService
{
    partial class ProjectInstaller
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary> 
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Component Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.serviceProcessInstaller = new System.ServiceProcess.ServiceProcessInstaller();
            this.windowsServiceInstaller = new System.ServiceProcess.ServiceInstaller();
            // 
            // serviceProcessInstaller
            // 
            this.serviceProcessInstaller.Account = System.ServiceProcess.ServiceAccount.NetworkService;
            this.serviceProcessInstaller.Password = null;
            this.serviceProcessInstaller.Username = null;
            // 
            // windowsServiceInstaller
            // 
            this.windowsServiceInstaller.Description = "Executes and schedules Emdat reports";
            this.windowsServiceInstaller.DisplayName = "Emdat Report Execution Service";
            this.windowsServiceInstaller.ServiceName = "Emdat Report Execution Service";
            this.windowsServiceInstaller.StartType = System.ServiceProcess.ServiceStartMode.Automatic;
            // 
            // ProjectInstaller
            // 
            this.Installers.AddRange(new System.Configuration.Install.Installer[] {
            this.serviceProcessInstaller,
            this.windowsServiceInstaller});
            this.BeforeUninstall += new System.Configuration.Install.InstallEventHandler(this.ProjectInstaller_BeforeUninstall);
            this.BeforeInstall += new System.Configuration.Install.InstallEventHandler(this.ProjectInstaller_BeforeInstall);

        }

        #endregion

        private System.ServiceProcess.ServiceProcessInstaller serviceProcessInstaller;
        private System.ServiceProcess.ServiceInstaller windowsServiceInstaller;
    }
}