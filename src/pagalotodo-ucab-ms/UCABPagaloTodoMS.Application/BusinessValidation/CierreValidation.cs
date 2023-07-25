using System.Text;
using UCABPagaloTodoMS.Application.Exceptions;
using UCABPagaloTodoMS.Core.Database;
using UCABPagaloTodoMS.Core.Entities;
using UCABPagaloTodoMS.Infrastructure.Services.Hotmail;

namespace UCABPagaloTodoMS.Application.BusinessValidation
{
    public class CierreValidation
    {


        /// <summary>
        ///     Validates the Business Rules make a cierre
        /// </summary>
        /// <remarks>
        /// <paramref name="_servicio"/> string with the servicios' name
        /// <paramref name="_dbContext"/> Context
        /// </remarks>
        public static async Task MakeCierre(string servicioNombre, IUCABPagaloTodoDbContext dbContext)
        {
            //Get the ID for the services' name
            var servicio = await ServicioValidation.GetServicio(servicioNombre, dbContext);
            //Get list of payments that have not been closed
            using var transaction = dbContext.BeginTransaction();
            //Get the formato
            var Formato = await FormatoValidation.GetFormato(servicio, dbContext);
            //Get the object
            var ListaPagos = await ServicioValidation.GetPagosPendientesCierre(servicio, dbContext);
            var prestador = await PrestadorValidation.GetUserById(servicio.PrestadorEntityId, dbContext);
            //Change the date format
            var formatoFecha = "yyyyMMdd-HHmmss";
            //Set the file name
            string nombreArchivo = $"{servicioNombre} {DateTime.Now.ToString(formatoFecha)}.txt";
            var File = GetFile(ListaPagos, Formato);
            //Send the mail with the file
            if (EnviarCorreoConciliacion.EnviarCorreo(File, prestador.Correo, nombreArchivo))
            {
                //Change the status of the cierre in the pagos
                await PagoValidation.ActualizarCierrePago(ListaPagos, dbContext, prestador, DateTime.Now);
                transaction.Commit();
            }
            else
            {
                throw new CustomException(new() { "Hubo un error al enviar el correo" });
            }
        }

        public static byte[] GetFile(List<PagoEntity> ListaPagos, List<FormatoServicioEntity> Formato)
        {
            byte[] datosArchivo;
            using (MemoryStream stream = new MemoryStream())
            {
                using (StreamWriter writer = new StreamWriter(stream))
                {
                    foreach (var pago in ListaPagos)
                    {
                        writer.WriteLine(GetMessage(pago, Formato));
                    }
                }
                return datosArchivo = stream.ToArray();
            }
        }

        /// <summary>
        ///     Validates the Business Rules make a cierre
        /// </summary>
        /// <remarks>
        /// <paramref name="pago"/> PagoEntitywith the pago
        /// <paramref name="Formato"/> FormatoServicioEntity
        /// </remarks>
        public static string GetMessage(PagoEntity pago, List<FormatoServicioEntity> Formato)
        {
            //Get the message with only the values the prestador want
            var Mensaje = new StringBuilder();
            Mensaje.Append(pago.Id);
            Mensaje.Append(" ");
            Mensaje.Append(pago.Servicio.Nombre);
            foreach (var formato in Formato)
            {
                if(formato.Requerido is true)
                {
                    Mensaje.Append(" ");
                    Mensaje.Append(Campo(pago, formato));
                }
            }
            return Mensaje.ToString();
        }

        public static string Campo(PagoEntity pago, FormatoServicioEntity formato)
        {
            var property = pago.GetType().GetProperty(formato.FormatoCon.NombreCampo);
            var valor = new object();
            if(property == null)
            {
                property = pago.Consumidor.GetType().GetProperty(formato.FormatoCon.NombreCampo);
                if(property == null)
                {
                    property = pago.Servicio.GetType().GetProperty(formato.FormatoCon.NombreCampo);
                    valor = property.GetValue(pago.Servicio);
                }
                else
                    valor = property.GetValue(pago.Consumidor);
            }
            else
            {
                valor = property.GetValue(pago);
            }
            if (formato.FormatoCon.NombreCampo == "Fecha")
            {
                //remove time from date
                valor = property.GetValue(pago);
                var FechaSola = valor.ToString().Split(" ");
                return FechaSola[0].PadRight(formato.Logitud);
            }
            return valor.ToString().PadRight(formato.Logitud);
        }
    }
}
