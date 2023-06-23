using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Xml.Linq;



namespace AzeShop.Areas.Admin.Models
{
    public class LoginModel
    {
        public string AccountName { get; set; }
        public string Password { get; set; }
    }
}
