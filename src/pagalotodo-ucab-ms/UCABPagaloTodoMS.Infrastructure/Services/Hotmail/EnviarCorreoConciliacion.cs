using MailKit.Net.Smtp;
using MailKit.Security;
using Microsoft.EntityFrameworkCore;
using MimeKit;
using UCABPagaloTodoMS.Core.Database;
using UCABPagaloTodoMS.Infrastructure.Services.Hotmail;

namespace UCABPagaloTodoMS.Infrastructure.Services.Hotmail
{
    public class EnviarCorreoConciliacion
    {


        public static bool EnviarCorreo(byte[] datosArchivo, string _correo, string nombre)
        {
            var mensaje = new MimeMessage();
            mensaje.From.Add(new MailboxAddress("Seccion A Grupo E", "seccionagrupoe@hotmail.com"));
            mensaje.To.Add(new MailboxAddress("Destinatario", _correo));
            mensaje.Subject = "Cierre Contable al " + DateTime.Now;

            var streamArchivo = new MemoryStream(datosArchivo);

            // Crear el archivo adjunto
            var archivoAdjunto = new MimePart("application", "octet-stream")
            {
                Content = new MimeContent(streamArchivo),
                ContentDisposition = new ContentDisposition(ContentDisposition.Attachment),
                ContentTransferEncoding = ContentEncoding.Base64,
                FileName = nombre
            };

            // Agregar el archivo adjunto al mensaje
            var multipart = new Multipart("mixed");
            multipart.Add(archivoAdjunto);
            mensaje.Body = multipart;

            using (var clienteSmtp = new SmtpClient())
            {
                clienteSmtp.Connect("smtp.office365.com", 587, SecureSocketOptions.StartTls);
                clienteSmtp.Authenticate("seccionagrupoe@hotmail.com", "Bismarck");

                clienteSmtp.Send(mensaje);
                clienteSmtp.Disconnect(true);
            }
            return true;
        }
    }
}
