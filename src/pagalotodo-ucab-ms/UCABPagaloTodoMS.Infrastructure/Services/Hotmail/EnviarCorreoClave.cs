using MailKit.Net.Smtp;
using MailKit.Security;
using Microsoft.EntityFrameworkCore;
using MimeKit;
using UCABPagaloTodoMS.Core.Database;
using UCABPagaloTodoMS.Infrastructure.Services.Hotmail;
using UCABPagaloTodoMS.Infrastructure.Utils;

namespace UCABPagaloTodoMS.Infrastructure.Services.Hotmail
{
    public class EnviarCorreoClave
    {
        public static string EnviarCorreoRecuperacion(string _correo)
        {
            var mensaje = new MimeMessage();
            string nuevaClave = ClaveAleatoria.GenerarClaveAleatoria();
            mensaje.From.Add(new MailboxAddress("Seccion A Grupo E", "seccionagrupoe@hotmail.com"));
            mensaje.To.Add(new MailboxAddress("Destinatario", _correo));
            mensaje.Subject = "Recuperación de clave";
            mensaje.Body = new TextPart("plain")

            {
                Text = "Le enviamos su clave de recuperación. Recuerde que al ingresar por seguridad deberia realizar un cambio de clave, de lo contrario tendrá que seguir ingresando" +
                "con la clave enviada. Clave temporal: " + nuevaClave
            };

            using (var clienteSmtp = new SmtpClient())
            {
                clienteSmtp.Connect("smtp.office365.com", 587, SecureSocketOptions.StartTls);
                clienteSmtp.Authenticate("seccionagrupoe@hotmail.com", "Bismarck");

                clienteSmtp.Send(mensaje);
                clienteSmtp.Disconnect(true);
            }

            return nuevaClave;
        }
    }
}
