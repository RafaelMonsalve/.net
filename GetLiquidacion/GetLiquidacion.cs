using System;
using System.Data;
using System.ServiceProcess;
using System.Timers;
using System.Threading.Tasks;

namespace GetLiquidacion
{
    public partial class GetLiquidacionService : ServiceBase
    {
        private Timer _timer;
        private Function _function;
        private ConfigSettings _config;

        public GetLiquidacionService()
        {
            this.ServiceName = "GetLiquidacionService";

            try
            {
                // 🔹 Cargar configuración
                _config = ConfigLoader.Load();

                // 🔹 Inicializar función con configuración
                _function = new Function(_config);

                // 🔹 Inicializar el Timer
                _timer = new Timer
                {
                    Interval = _config.Interval, // Intervalo en milisegundos
                    AutoReset = true
                };
                _timer.Elapsed += TimerElapsed;
            }
            catch (Exception ex)
            {
                LogError($"❌ Error cargando configuración: {ex.Message}");
                throw;
            }
        }

        protected override void OnStart(string[] args)
        {
            try
            {
                _timer.Start();

                string querylog = "INSERT INTO LOG (SERVICE, STATUS) VALUES ('GetLiquidacionService','✅ Servicio iniciado')";
                _function.ExecuteQuery(querylog);
            }
            catch (Exception ex)
            {
                string querylog = "INSERT INTO LOG (SERVICE, STATUS) VALUES ('GetLiquidacionService','❌ Error al iniciar el servicio')";
                _function.ExecuteQuery(querylog);
                LogError($"❌ Error al iniciar el servicio: {ex.Message}");
                throw;
            }
        }

        protected override void OnStop()
        {
            try
            {
                _timer?.Stop();
                _timer?.Dispose();

                string querylog = "INSERT INTO LOG (SERVICE, STATUS) VALUES ('GetLiquidacionService','🛑 Servicio detenido')";
                _function.ExecuteQuery(querylog);
            }
            catch (Exception ex)
            {
                string querylog = $"INSERT INTO LOG (SERVICE, STATUS) VALUES ('GetLiquidacionService','❌ Error al detener el servicio: {ex.Message}')";
                _function.ExecuteQuery(querylog);
                LogError($"❌ Error al detener el servicio: {ex.Message}");
            }
        }

        private async void TimerElapsed(object sender, ElapsedEventArgs e)
        {
            try
            {
                // 🔹 1️⃣ Obtener el token más reciente
                string tokenQuery = $"SELECT TOP 1 token FROM {_config.TokenTable} ORDER BY FechaCreacion DESC";
                DataTable tokenTable = _function.ExecuteQuery(tokenQuery);

                if (tokenTable.Rows.Count == 0)
                {
                    LogError("⚠️ No se encontró ningún token en la base de datos.");
                    return;
                }

                string token = tokenTable.Rows[0]["token"].ToString();

                // 🔹 2️⃣ Obtener anticipos en estado 1
                string anticipoQuery = $"SELECT idliquidacion FROM {_config.GetAnticipoTable} WHERE estado = 1";
                DataTable anticipos = _function.ExecuteQuery(anticipoQuery);

                if (anticipos.Rows.Count == 0)
                {
                    string querylog = $"INSERT INTO LOG (SERVICE, STATUS) VALUES ('GetLiquidacionService','⚠️ No se encontraron anticipos en estado 1.')";
                    _function.ExecuteQuery(querylog);
                    LogError("⚠️ No se encontraron anticipos en estado 1.");
                    return;
                }

                // 🔹 3️⃣ Procesar anticipos
                foreach (DataRow anticipoRow in anticipos.Rows)
                {
                    string id = anticipoRow["idliquidacion"].ToString();
                    string apiUrl = $"{_config.ApiUrl}?token={token}&numero={id}";

                    // 🔹 Consumir API
                    string apiResponse = await ApiHelper.CallApiAsync(apiUrl);

                    if (!string.IsNullOrEmpty(apiResponse))
                    {
                        string querylog = $"INSERT INTO LOG (SERVICE, STATUS) VALUES ('GetLiquidacionService','✅ Procesado el anticipo con ID {id}')";
                        _function.ExecuteQuery(querylog);
                        LogInfo($"✅ Procesado el anticipo con ID {id}");
                    }
                    else
                    {
                        string querylog = $"INSERT INTO LOG (SERVICE, STATUS) VALUES ('GetLiquidacionService','⚠️ No se obtuvo respuesta válida para el anticipo con ID {id}')";
                        _function.ExecuteQuery(querylog);
                        LogError($"⚠️ No se obtuvo respuesta válida para el anticipo con ID {id}");
                    }
                }
            }
            catch (Exception ex)
            {
                LogError($"❌ Error en el TimerElapsed: {ex.Message}");
            }
        }

        private void LogError(string message)
        {
            Console.WriteLine($"[ERROR] {DateTime.Now}: {message}");
        }

        private void LogInfo(string message)
        {
            Console.WriteLine($"[INFO] {DateTime.Now}: {message}");
        }
    }
}
