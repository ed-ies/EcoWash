using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace EcoWash.ValidationRules
{
    public class RangoNumericoValidationRule : ValidationRule
    {
        public double Min { get; set; }
        public double Max { get; set; }

        public override ValidationResult Validate(object value, CultureInfo cultureInfo)
        {
            // Intentamos convertir el valor a número
            if (!double.TryParse(value?.ToString(), out double numero))
            {
                return new ValidationResult(false, "El valor introducido no es un número válido");
            }

            // Comprobamos el rango
            if (numero < Min || numero > Max)
            {
                return new ValidationResult(false, $"El valor debe estar entre {Min} y {Max}");
            }

            // Todo correcto
            return ValidationResult.ValidResult;
        }
    }
}
