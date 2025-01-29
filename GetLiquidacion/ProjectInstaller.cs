using System.ComponentModel;
using System.Configuration.Install;
using System.ServiceProcess;


namespace GetLiquidacion
{
[RunInstaller(true)]
public class ProjectInstaller : Installer
{
    private ServiceProcessInstaller processInstaller;
    private ServiceInstaller serviceInstaller;

    public ProjectInstaller()
    {
        processInstaller = new ServiceProcessInstaller();
        serviceInstaller = new ServiceInstaller();

        processInstaller.Account = ServiceAccount.LocalSystem;

        serviceInstaller.ServiceName = "LiquidacionService";
        serviceInstaller.DisplayName = "Liquidacion Service";
        serviceInstaller.StartType = ServiceStartMode.Automatic;

        Installers.Add(processInstaller);
        Installers.Add(serviceInstaller);
    }
}

}