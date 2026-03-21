using Auth.Common.Enums;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Auth.Common.Model
{
    public class CustomToken
    {
        [Required(ErrorMessage = "Email is required")]
        [DataType(DataType.EmailAddress)]
        public string Email { get; set; }

        [Required(ErrorMessage = "Roles is required. Ex: Visitor, Common, Manager, Admin")]
        [DataType(DataType.Text)]
        [DefaultValue(nameof(DefaultRoles.Common))]
        public string Roles { get; set; }

        [Required(ErrorMessage = "Channel is required")]
        [DataType(DataType.Text)]
        [DefaultValue("99")]
        public string Channel { get; set; }

        [DefaultValue(180)]
        [DataType(DataType.Duration)]
        [Required(ErrorMessage = "ExpiryTimeInMinutes is required. Default 180 minutes = 3 hours")]
        public double ExpiryTimeInMinutes { get; set; }

        [DefaultValue("DXZN0F5CZD3830")]        
        public string Cnpj { get; set; }
    }
}
