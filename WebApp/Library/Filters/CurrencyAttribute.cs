namespace WebApp.Library
{
    using System;
    using System.Globalization;
    using System.ComponentModel.DataAnnotations;


    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Field, AllowMultiple = false)]
    sealed public class CurrencyAttribute : ValidationAttribute
    {
        public CurrencyAttribute()
        {
        }


        public override bool IsValid(object value)
        {
            decimal convertedValue;
            bool canBeConverted = Decimal.TryParse(value.ToString(), NumberStyles.Currency, CultureInfo.CurrentCulture.NumberFormat, out convertedValue);
            return canBeConverted;
        }

        public override string FormatErrorMessage(string name)
        {
            return String.Format(CultureInfo.CurrentCulture,
              ErrorMessageString, name, "Value is not a valid currency");
        }

    }
}