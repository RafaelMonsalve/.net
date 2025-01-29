using System.ServiceProcess;
namespace GetLiquidacion
{

class Program
{
    static void Main()
    {
        ServiceBase[] ServicesToRun;
        ServicesToRun = new ServiceBase[]
        {
            new GetLiquidacionService()
        };
        ServiceBase.Run(ServicesToRun);
    }
}
}