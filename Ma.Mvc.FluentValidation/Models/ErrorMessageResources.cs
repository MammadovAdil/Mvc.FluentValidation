namespace Ma.Mvc.FluentValidation.Models
{
    public class ErrorMessageResources
    {
        public static string notnull_error
        {
            get
            {
                return "'{PropertyName}' tələb olunan sahədir.";
            }
        }

        public static string notempty_error
        {
            get
            {
                return "'{PropertyName}' tələb olunan sahədir.";
            }
        }

        public static string notequal_error
        {
            get
            {
                return "'{PropertyName}' '{ComparisonValue}' ilə bərabər olmamalıdır.";
            }
        }

        public static string lessthan_error
        {
            get
            {
                return "'{PropertyName}' '{ComparisonValue}'-dən kiçik olmalıdır.";
            }
        }

        public static string lessthanorequal_error
        {
            get
            {
                return "'{PropertyName}' '{ComparisonValue}'-dən böyük olmamalıdır.";
            }
        }

        public static string greaterthan_error
        {
            get
            {
                return "'{PropertyName}' '{ComparisonValue}'-dən böyük olmalıdır.";
            }
        }

        public static string greaterthanorequal_error
        {
            get
            {
                return "'{PropertyName}' '{ComparisonValue}'-dən kiçik olmamalıdır.";
            }
        }

        public static string inclusivebetween_error
        {
            get
            {
                return "'{PropertyName}' {From} və {To} aralığında olmalıdır.";
            }
        }

        public static string length_error
        {
            get
            {
                return "'{PropertyName}' simvol sayı {MinLength} və {MaxLength} aralığında olmalıdır.";
            }
        }

        public static string regex_error
        {
            get
            {
                return "'{PropertyName}' düzgün formatda deyil.";
            }
        }
    }
}
