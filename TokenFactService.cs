using System;
using System.Net.Http;
using System.Text;

using System.Threading.Tasks;
using System.ServiceProcess;
using System.Threading;
using System.Data.SqlClient;

using System.Configuration;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.IO;
using System.Diagnostics;

// Asegúrate de agregar la referencia a System.ServiceProcess.ServiceController mediante NuGet
// Ejecuta el siguiente comando en la consola del Administrador de Paquetes:
// Install-Package System.ServiceProcess.ServiceController -Version 4.8.0

public class TokenFactService : ServiceBase
{
    private System.Threading.Timer _timer;
    private readonly HttpClient _httpClient;
    private readonly AppSettings _appSettings;

    public TokenFactService() : base()
    {
        ServiceName = "TokenFactCarport";
        _httpClient = new HttpClient();

        // Cargar la configuración desde appsettings.json
        var json = File.ReadAllText(AppDomain.CurrentDomain.BaseDirectory + "appsettings.json");
        _appSettings = JsonConvert.DeserializeObject<AppSettings>(json) ?? throw new Exception("No se pudo cargar la configuración correctamente desde appsettings.json");
    }

    protected override void OnStart(string[] args)
    {
        // Iniciar el Timer para ejecutarse cada 59 minutos
        _timer = new Timer(ExecuteTask, null, TimeSpan.Zero, TimeSpan.FromMinutes(_appSettings.ServiceSettings.IntervalInMinutes));
    }

    private async void ExecuteTask(object state)
    {
        try
        {
            // Preparar la solicitud HTTP
            var request = new HttpRequestMessage(HttpMethod.Post, _appSettings.ApiSettings.Url);
            request.Headers.Add("X-Who", _appSettings.ApiSettings.WhoHeader);
            request.Content = new StringContent(Newtonsoft.Json.JsonConvert.SerializeObject(new
            {
                u = _appSettings.ApiSettings.Username,
                p = _appSettings.ApiSettings.Password
            }), Encoding.UTF8, "application/json");

            // Hacer la llamada a la API
            HttpResponseMessage response = await _httpClient.SendAsync(request);
            response.EnsureSuccessStatusCode();
            string responseBody = await response.Content.ReadAsStringAsync();

            // Parsear la respuesta
            var document = Newtonsoft.Json.JsonConvert.DeserializeObject<dynamic>(responseBody);
            // Extraer propiedades desde el objeto JSON
if (document == null) throw new Exception("La respuesta de la API no es válida o está vacía");
            var root = document;

            string displayName = root["displayName"];
            string accessToken = root["accessToken"];
            string tenantId = root["tenantId"];
            

            // Insertar en la base de datos
            using (SqlConnection connection = new SqlConnection(_appSettings.DatabaseSettings.ConnectionString))
            {
                await connection.OpenAsync();
                string query = "INSERT INTO [token_fact] (displayName, accessToken, tenantId) VALUES (@displayName, @accessToken, @tenantId)";
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@displayName", displayName);
                    command.Parameters.AddWithValue("@accessToken", accessToken);
                    command.Parameters.AddWithValue("@tenantId", tenantId);
                    await command.ExecuteNonQueryAsync();
                }
            }
        }
       catch (Exception ex)
        {
            // Manejo de errores (puedes escribir en un log)
            if (!EventLog.SourceExists(ServiceName))
            {
                EventLog.CreateEventSource(ServiceName, "Application");
            }
            string errorMessage = $"Error en WorkManager: {ex.Message} " + Environment.NewLine + $"StackTrace: {ex.StackTrace}";
EventLog.WriteEntry(ServiceName, errorMessage, EventLogEntryType.Error);
        }
    }


    protected override void OnStop()
    {
        _timer?.Change(Timeout.Infinite, 0);
        _timer?.Dispose();
    }
}
