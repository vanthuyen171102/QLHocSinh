using System.ComponentModel.DataAnnotations;
using System.Xml.Linq;
#nullable disable
namespace AzeShop.ModelViews
{
    public class UserLoginModel
    {
        public string UserName { get; set; }
        public string Password { get; set; }
    }
}
