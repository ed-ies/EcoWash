using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace EcoWash.ValidationRules
{
    public class StringValidationRule : ValidationRule
    {
        // Comprobamos que haya texto en el TextBox asociado
        public string Texto { get; set; }

        public override ValidationResult Validate(object value, CultureInfo cultureInfo)
        {
            if (string.IsNullOrWhiteSpace(value?.ToString()))
            {
                return new ValidationResult(false, $"{Texto} es obligatorio");
            }


            return ValidationResult.ValidResult;
        }
    }
}
