using System;
using System.Data;
using System.Data.SqlClient;
using System.Net.Http;
using System.Threading.Tasks;

namespace GetLiquidacion
{
    public class Function
    {
        private readonly ConfigSettings _config;
        private readonly HttpClient _httpClient;

        public Function(ConfigSettings config)
        {
            _config = config ?? throw new ArgumentNullException(nameof(config), "La configuración no puede ser nula.");
            _httpClient = new HttpClient();
        }

        public DataTable ExecuteQuery(string query)
        {
            try
            {
                // Asegúrate de que la cadena de conexión esté configurada en la clase ConfigSettings
                string connectionString = _config.ConnectionString;

                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        using (SqlDataAdapter adapter = new SqlDataAdapter(command))
                        {
                            DataTable result = new DataTable();
                            adapter.Fill(result);
                            return result;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                // Lanza la excepción con información adicional si es necesario
                throw new ApplicationException("Error al ejecutar la consulta SQL.", ex);
            }
        }

        public async Task<string> GetAsync(string url)
        {
            try
            {
                // Enviar solicitud GET
                HttpResponseMessage response = await _httpClient.GetAsync(url);

                // Asegurarse de que la respuesta sea exitosa
                response.EnsureSuccessStatusCode();

                // Leer la respuesta como una cadena
                string responseContent = await response.Content.ReadAsStringAsync();
                return responseContent;
            }
            catch (Exception ex)
            {
                // Manejar errores
                throw new ApplicationException($"Error al consumir la API: {ex.Message}", ex);
            }
        }
    }
}