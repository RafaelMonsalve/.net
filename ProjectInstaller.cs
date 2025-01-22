using System.ComponentModel;
using System.Configuration.Install;
using System.ServiceProcess;

namespace ListarDocumentosServiceInstaller
{
    [RunInstaller(true)]
    public class ProjectInstaller : Installer
    {
        private ServiceProcessInstaller processInstaller;
        private ServiceInstaller serviceInstaller;

        public ProjectInstaller()
        {
            // Configurar el instalador del proceso del servicio
            processInstaller = new ServiceProcessInstaller
            {
                Account = ServiceAccount.LocalSystem
            };

            // Configurar el instalador del servicio
            serviceInstaller = new ServiceInstaller
            {
                ServiceName = "TokenFactCarpor",
                DisplayName = "Token Fact Service",
                StartType = ServiceStartMode.Automatic
            };

            // Añadir ambos instaladores al proyecto
            Installers.Add(processInstaller);
            Installers.Add(serviceInstaller);
        }
    }
}
