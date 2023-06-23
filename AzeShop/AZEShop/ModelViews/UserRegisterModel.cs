using AzeShop.Models;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;
#nullable disable
namespace AzeShop.ModelViews
{
    public class UserRegisterModel
    {
        [Key]
        public int CustomerId { get; set; }


        public string FullName { get; set; }

        public string Email { get; set; }

        public string Address { get; set; }

        public int CityId { get; set; }

        public string Phone { get; set; }

       
        public string Password { get; set; }

        public string ConfirmPassword { get; set; }


    }
}
