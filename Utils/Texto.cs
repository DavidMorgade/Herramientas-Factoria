using System;
using System.Globalization;

namespace Herramientas_Factoria.Utils
{
    internal class Texto
    {
        public static string FormatearContratoAGlobal(string input)
        {
            // Eliminar el prefijo "SC"
            string trimmedInput = input.Substring(2);

            // Insertar una barra "/" después del año
            string year = trimmedInput.Substring(0, 4);
            string number = trimmedInput.Substring(4, 4);
            string lastDigit = trimmedInput.Substring(8, 1);

            // Formar la nueva cadena
            string formattedString = $"{year}/{number}-{lastDigit}";

            return formattedString;
        }
        public static string FormatearFechaSinIVA(string inputDate)
        {
            // Parsear la fecha de entrada
            DateTime parsedDate;
            if (DateTime.TryParseExact(inputDate, "dd/MM/yyyy", CultureInfo.InvariantCulture, DateTimeStyles.None, out parsedDate))
            {
                // Formatear la fecha en el nuevo formato
                string formattedDate = parsedDate.ToString("dd 'de' MMMM 'de' yyyy", new CultureInfo("es-ES"));

                return formattedDate;
            }
            else
            {
                throw new ArgumentException("La fecha de entrada no está en el formato correcto.");
            }
        }
    }
}
