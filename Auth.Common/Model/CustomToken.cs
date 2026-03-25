using Auth.Common.Lib.Enums;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Auth.Common.Lib.Model
{
    public class CustomToken
    {
        [DataType(DataType.Text)]
        [DefaultValue(null)]
        public string Name { get; set; }

        [DataType(DataType.EmailAddress)]
        public string Email { get; set; }
                
        [DataType(DataType.Text)]
        [DisplayName("Ex: Visitor, Common, Manager, Admin")]
        [DefaultValue(nameof(DefaultRoles.Common))]
        public string Roles { get; set; }        

        [DefaultValue(60)]
        [DataType(DataType.Duration)]
        public double ExpiryTimeInMinutes { get; set; }

        [DataType(DataType.Text)]
        [DefaultValue(null)]
        public string AccountStatus { get; set; }

        [DisplayName("You can provide the customer ID as a numeric, string, or object, whichever you need.")]
        [DefaultValue(null)]
        public object CustomerId { get; set; }

        [DisplayName("You can provide the user ID as a numeric, string, or object, whichever you need.")]
        [DefaultValue(null)]
        public object UserId { get; set; }

        [DefaultValue(null)]        
        public string Cnpj { get; set; }
    }
}
