using System.Text;

namespace UCABPagaloTodoMS.Infrastructure.Utils
{
    public class ClaveAleatoria
    {
        public static string GenerarClaveAleatoria()
        {
            const string caracteresValidos = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ1234567890";
            var random = new Random();
            var claveAleatoria = new StringBuilder();

            // Generar una clave aleatoria de 8 caracteres
            for (int i = 0; i < 8; i++)
            {
                int indiceCaracter = random.Next(caracteresValidos.Length);
                claveAleatoria.Append(caracteresValidos[indiceCaracter]);
            }

            return claveAleatoria.ToString();
        }
    }
}
