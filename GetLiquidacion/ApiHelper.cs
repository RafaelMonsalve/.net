using System;
using System.Net.Http;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;

namespace GetLiquidacion
{
    public static class ApiHelper
    {
        
        private static ConfigSettings _config;
        private static Function _function;

        public static async Task<string> CallApiAsync(string token)
        { 
            try
            {
            
            
            using (var client = new HttpClient())
            {
                var url = $"{_config.ApiUrl}?token={token}";
                var response = await client.GetAsync(url);

                if (response.IsSuccessStatusCode)
                {
                    var content = await response.Content.ReadAsStringAsync();
                    return content;  // 🔹 Devuelve la respuesta en formato string
                }
                else
                {
                    string querylog = "INSERT INTO LOG (SERVICE, STATUS) VALUES ('GetLiquidacionService','No se cargo la configuración')";
                _function.ExecuteQuery(querylog);
                return string.Empty;
                }
            }
        }  catch (Exception ex)
            {
                 string mensajeError = ex.Message.Replace("'", "''");  // Evitar SQL Injection
                string querylog = $"INSERT INTO LOG (SERVICE, STATUS) VALUES ('GetLiquidacionService','❌ Excepción en API: {mensajeError}')";
                _function.ExecuteQuery(querylog);
        
        return string.Empty;
            }   
    }

        /// <summary>
        /// Procesa la respuesta de la API y guarda los datos en la base de datos.
        /// </summary>
       public static async Task ProcessApiResponseAsync(string jsonResponse)
        {
            try
            {
                _config = ConfigLoader.Load();
                _function = new Function(_config);

                var json = JObject.Parse(jsonResponse);
                var liquidaciones = json["GetLiquidacion2Result"]?["Gastos"];

                if (liquidaciones != null)
                {
                    foreach (var liquidacion in liquidaciones)
                    {
                        // **Mapeo de todos los datos de la API**
                        var data = new
                        {
                // Mapear todos los campos de la respuesta de la API
                 AEAT = liquidacion["AEAT"]?.ToObject<bool>() ?? false,
                 BaseImponible1 = liquidacion["BaseImponible1"]?.ToObject<bool>() ?? false,
                 BaseImponible2 = liquidacion["BaseImponible1"]?.ToObject<bool>() ?? false,
                 BaseImponible3 = liquidacion["BaseImponible3"]?.ToString() ?? string.Empty,
                 Cambio = liquidacion["Cambio"]?.ToString() ?? string.Empty,
                 CamposPersonalizados = liquidacion["CamposPersonalizados"]?.ToString() ?? string.Empty,
                 CamposPersonalizadosInforme = liquidacion["CamposPersonalizadosInforme"]?.ToString() ?? string.Empty,
                 Campo = liquidacion["CamposPersonalizadosInformeJSON"]["Campo"]?.ToString() ?? string.Empty,
                 Descripciones = liquidacion["CamposPersonalizadosInformeJSON"]["Descripciones"]?.ToObject<bool>() ?? false,
                 Valor = liquidacion["CamposPersonalizadosInformeJSON"]["Valor"]?.ToString() ?? string.Empty,
                 Canarias = liquidacion["Canarias"]?.ToString() ?? string.Empty,
                 CentroCoste = liquidacion["CentroCoste"]?.ToString() ?? string.Empty,
                 CentroCosteN = liquidacion["CentroCosteN"]?.ToString() ?? string.Empty,
                 CodigoPostal = liquidacion["CodigoPostal"]?.ToString() ?? string.Empty,
                 Descripcion = liquidacion["Descripcion"]?.ToString() ?? string.Empty,
                 DescripcionSubproyecto = liquidacion["DescripcionSubproyecto"]?.ToString() ?? string.Empty,
                 Destino = liquidacion["Destino"]?.ToString() ?? string.Empty,
                 Direccion = liquidacion["Direccion"]?.ToString() ?? string.Empty,
                 Empresa = liquidacion["Empresa"]?.ToString() ?? string.Empty,
                 EmpresaVisitada = liquidacion["EmpresaVisitada"]?.ToString() ?? string.Empty,
                 EntradaManual = liquidacion["EntradaManual"]?.ToString() ?? string.Empty,
                 Establecimiento = liquidacion["Establecimiento"]?.ToString() ?? string.Empty,
                 EstablecimientoN = liquidacion["EstablecimientoN"]?.ToString() ?? string.Empty,
                 Factura = liquidacion["Factura"]?.ToString() ?? string.Empty,
                 Fecha = liquidacion["Fecha"]?.ToObject<bool>() ?? false,
                 Estado_Informe = liquidacion["Estado_Informe"]?.ToString() ?? string.Empty,
                 FotoIlegible = liquidacion["FotoIlegible"]?.ToObject<int>() ?? 0,
                 Fotostamp = liquidacion["Fotostamp"]?.ToString() ?? string.Empty,
                 Hora = liquidacion["Hora"]?.ToString() ?? string.Empty,
                 ID_Estado = liquidacion["ID_Estado"]?.ToString() ?? string.Empty,
                 ID_EstadoConciliado = liquidacion["ID_EstadoConciliado"]?.ToString() ?? string.Empty,
                 ID_Estado_Informe = liquidacion["ID_Estado_Informe"]?.ToString() ?? string.Empty,
                 Imagen = liquidacion["Imagen"]?.ToString() ?? string.Empty,
                 ImporteIva1 = liquidacion["ImporteIva1"]?.ToString() ?? string.Empty,
                 ImporteIva2 = liquidacion["ImporteIva2"]?.ToString() ?? string.Empty,
                 ImporteIva3 = liquidacion["ImporteIva3"]?.ToString() ?? string.Empty,
                 Informe = liquidacion["Informe"]?.ToString() ?? string.Empty,
                 InformeN = liquidacion["InformeN"]?.ToString() ?? string.Empty,
                 Invitados = liquidacion["Invitados"]?.ToString() ?? string.Empty,
                 InvitadosExternos = liquidacion["InvitadosExternos"]?.ToString() ?? string.Empty,
                 InvitadosInternos = liquidacion["InvitadosInternos"]?.ToString() ?? string.Empty,
                 Iva1 = liquidacion["Iva1"]?.ToString() ?? string.Empty,
                 Iva2 = liquidacion["Iva2"]?.ToString() ?? string.Empty,
                 Iva3 = liquidacion["Iva3"]?.ToString() ?? string.Empty,
                 IvaIncuido = liquidacion["IvaIncuido"]?.ToString() ?? string.Empty,
                 Kilometros = liquidacion["Kilometros"]?.ToString() ?? string.Empty,
                 LinkImagen = liquidacion["LinkImagen"]?.ToString() ?? string.Empty,
                 LinkPDF = liquidacion["LinkPDF"]?.ToString() ?? string.Empty,
                 Moneda = liquidacion["Moneda"]?.ToString() ?? string.Empty,
                 MonedaBase = liquidacion["MonedaBase"]?.ToString() ?? string.Empty,
                 Nota = liquidacion["Nota"]?.ToString() ?? string.Empty,
                 NumNoches = liquidacion["NumNoches"]?.ToString() ?? string.Empty,
                 NumeroConciliacion = liquidacion["NumeroConciliacion"]?.ToString() ?? string.Empty,
                 OCR1 = liquidacion["OCR1"]?.ToString() ?? string.Empty,
                 OCR2 = liquidacion["OCR2"]?.ToString() ?? string.Empty,
                 Origen = liquidacion["Origen"]?.ToString() ?? string.Empty,
                 Pais = liquidacion["Pais"]?.ToString() ?? string.Empty,
                 Poblacion = liquidacion["Poblacion"]?.ToString() ?? string.Empty,
                 PrecioKilometro = liquidacion["PrecioKilometro"]?.ToString() ?? string.Empty,
                 Proyecto = liquidacion["Proyecto"]?.ToString() ?? string.Empty,
                 ProyectoN = liquidacion["ProyectoN"]?.ToString() ?? string.Empty,
                 RazonSocial = liquidacion["RazonSocial"]?.ToString() ?? string.Empty,
                 SubTipoViaje = liquidacion["SubTipoViaje"]?.ToString() ?? string.Empty,
                 SubTipoViajeDescripcion = liquidacion["SubTipoViajeDescripcion"]?.ToString() ?? string.Empty,
                 Subproyecto = liquidacion["Subproyecto"]?.ToString() ?? string.Empty,
                 SubtipoGastoGasto = liquidacion["SubtipoGastoGasto"]?.ToString() ?? string.Empty,
                 SubtipoGastoGastoN = liquidacion["SubtipoGastoGastoN"]?.ToString() ?? string.Empty,
                 Banco = liquidacion["Tarjeta"]["Banco"]?.ToString() ?? string.Empty,
                 BancoN = liquidacion["Tarjeta"]["BancoN"]?.ToString() ?? string.Empty,
                 Cta = liquidacion["Tarjeta"]["Cta"]?.ToString() ?? string.Empty,
                 ID = liquidacion["Tarjeta"]["ID"]?.ToString() ?? string.Empty,
                 Numero = liquidacion["Tarjeta"]["Numero"]?.ToString() ?? string.Empty,
                 Reembolsable = liquidacion["Tarjeta"]["Reembolsable"]?.ToString() ?? string.Empty,
                 TipoGasto = liquidacion["TipoGasto"]?.ToString() ?? string.Empty,
                 TipoGastoN = liquidacion["TipoGastoN"]?.ToString() ?? string.Empty,
                 TipoTransporte = liquidacion["TipoTransporte"]?.ToString() ?? string.Empty,
                 TiquetFactura = liquidacion["TiquetFactura"]?.ToString() ?? string.Empty,
                 Total = liquidacion["Total"]?.ToString() ?? string.Empty,
                 TotalMonedaBase = liquidacion["TotalMonedaBase"]?.ToString() ?? string.Empty,
                 TotalValidado = liquidacion["TotalValidado"]?.ToString() ?? string.Empty,
                 CodigoExterno = liquidacion["Usuario"]["CodigoExterno"]?.ToString() ?? string.Empty,
                 CtaAnticipos = liquidacion["Usuario"]["CtaAnticipos"]?.ToString() ?? string.Empty,
                 CtaDietas = liquidacion["Usuario"]["CtaDietas"]?.ToString() ?? string.Empty,
                 CtaNominas = liquidacion["Usuario"]["CtaNominas"]?.ToString() ?? string.Empty,
                 Departamento = liquidacion["Usuario"]["Departamento"]?.ToString() ?? string.Empty,
                 DepartamentoN = liquidacion["Usuario"]["DepartamentoN"]?.ToString() ?? string.Empty,
                 Usuario = liquidacion["Usuario"]["Usuario"]?.ToString() ?? string.Empty,
                 UsuarioN = liquidacion["Usuario"]["UsuarioN"]?.ToString() ?? string.Empty,
                 Viaje = liquidacion["Viaje"]?.ToString() ?? string.Empty,
                 ViajeN = liquidacion["ViajeN"]?.ToString() ?? string.Empty,
                 idSAP = liquidacion["idSAP"]?.ToString() ?? string.Empty

};
                            string queryInsert = $@"
                            INSERT INTO {_config.Liquidacion} 
                            (AEAT,BaseImponible1,BaseImponible2,BaseImponible3,Cambio,CamposPersonalizados,CamposPersonalizadosInforme,
                            Campo,Descripciones,Valor,Canarias,CentroCoste,CentroCosteN,CodigoPostal,CtaGasto,Descripcion,DescripcionSubproyecto,Destino,
                            Direccion,Empresa,EmpresaVisitada,EntradaManual,Establecimiento,EstablecimientoN,Estado,EstadoConciliado,Estado_Informe,Factura,
                            Fecha,FotoIlegible,Fotostamp,Hora,ID_Estado,ID_EstadoConciliado,ID_Estado_Informe,Imagen,ImporteIva1,ImporteIva2,ImporteIva3,
                            Informe,InformeN,Invitados,InvitadosExternos,InvitadosInternos,Iva1,Iva2,Iva3,IvaIncuido,Kilometros,LinkImagen,LinkPDF,Moneda,
                            MonedaBase,Nota,NumNoches,NumeroConciliacion,OCR1,OCR2,Origen,Pais,Poblacion,PrecioKilometro,Proyecto,ProyectoN,RazonSocial,SubTipoViaje,
                            SubTipoViajeDescripcion,Subproyecto,SubtipoGastoGasto,SubtipoGastoGastoN,Banco,BancoN,Cta,ID,Numero,Reembolsable,TipoGasto,
                            TipoGastoN,TipoTransporte,TiquetFactura,Total,TotalMonedaBase,TotalValidado,CodigoExterno,CtaAnticipos,CtaDietas,CtaNominas,
                            Departamento,DepartamentoN,Usuario,UsuarioN,Viaje,ViajeN,idSAP) 
                            VALUES 
                            (@AEAT,@BaseImponible1,@BaseImponible2,@BaseImponible3,@Cambio,@CamposPersonalizados,@CamposPersonalizadosInforme,
                            @Campo,@Descripciones,@Valor,@Canarias,@CentroCoste,@CentroCosteN,@CodigoPostal,@CtaGasto,@Descripcion,@DescripcionSubproyecto,@Destino,
                            @Direccion,@Empresa,@EmpresaVisitada,@EntradaManual,@Establecimiento,@EstablecimientoN,@Estado,@EstadoConciliado,@Estado_Informe,@Factura,
                            @Fecha,@FotoIlegible,@Fotostamp,@Hora,@ID_Estado,@ID_EstadoConciliado,@ID_Estado_Informe,@Imagen,@ImporteIva1,@ImporteIva2,@ImporteIva3,
                            @Informe,@InformeN,@Invitados,@InvitadosExternos,@InvitadosInternos,@Iva1,@Iva2,@Iva3,@IvaIncuido,@Kilometros,@LinkImagen,@LinkPDF,@Moneda,
                            @MonedaBase,@Nota,@NumNoches,@NumeroConciliacion,@OCR1,@OCR2,@Origen,@Pais,@Poblacion,@PrecioKilometro,@Proyecto,@ProyectoN,@RazonSocial,@SubTipoViaje,
                            @SubTipoViajeDescripcion,@Subproyecto,@SubtipoGastoGasto,@SubtipoGastoGastoN,@Banco,@BancoN,@Cta,@ID,@Numero,@Reembolsable,@TipoGasto,
                            @TipoGastoN,@TipoTransporte,@TiquetFactura,@Total,@TotalMonedaBase,@TotalValidado,@CodigoExterno,@CtaAnticipos,@CtaDietas,@CtaNominas,
                            @Departamento,@DepartamentoN,@Usuario,@UsuarioN,@Viaje,@ViajeN,@idSAP) 
                            ";

                        // **Ejecutar la consulta**
                        _function.ExecuteQuery(queryInsert);

                        string querylog = "INSERT INTO LOG (SERVICE, STATUS) VALUES ('GetLiquidacionService','Se realizo el insert con exito')";
                        await Task.Run(()=>_function.ExecuteQuery(querylog));
                    }
                }
                else
                {
                    string querylog = $"INSERT INTO LOG (SERVICE, STATUS) VALUES ('GetLiquidacionService','No fue posible insertar la los registros')";
                await Task.Run(()=>_function.ExecuteQuery(querylog));
                }
            }
            catch (Exception ex)
            {
                string querylog = $"INSERT INTO LOG (SERVICE, STATUS) VALUES ('GetLiquidacionService','{ex.Message}')";
                await Task.Run(()=>_function.ExecuteQuery(querylog));
            }
        }
    }
}