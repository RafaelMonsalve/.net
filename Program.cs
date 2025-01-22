using System.ServiceProcess;
public static class Program
{
    public static void Main()
    {
        ServiceBase[] ServicesToRun;
        ServicesToRun = new ServiceBase[]
        {
            new TokenFactService()  // Inicia el servicio TokenFactService
        };
        ServiceBase.Run(ServicesToRun);
    }
}
