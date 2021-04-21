using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using vroom.Extensions;

namespace vroom.Models
{
    public class Bike
    {
        public int Id { get; set; }
        public Make Make { get; set; }
        [RegularExpression("^[1-9]*$", ErrorMessage = "Please Select Make")]
        public int MakeID { get; set; }

        public Model Model { get; set; }

        [RegularExpression("^[1-9]*$", ErrorMessage = "Please Select Model")]
        public int ModelID { get; set; }
        [Required(ErrorMessage = "Please Enter Year")]
        // [Range(2000, 2021, ErrorMessage = "Please ENter Proper Year Value")]
        [YearRangeTillDate(2000, ErrorMessage = "Please Enter Proper Year Value")]
        public int Year { get; set; }
        [Required(ErrorMessage = "Please Provide Mileage Details")]
        [Range(1, int.MaxValue, ErrorMessage = "Please Enter Proper Mileage")]
        public int Mileage { get; set; }
        public string Features { get; set; }
        [Required(ErrorMessage = "Please Provide Seller Name")]
        public string SellerName { get; set; }
        [EmailAddress(ErrorMessage = "Enter Correct Email Address")]
        public string SellerEmail { get; set; }
        [Required(ErrorMessage = "Please Provide Seller Phone Number")]
        public string SellerPhone { get; set; }
        [Required(ErrorMessage = "Please Enter Correct Price")]
        [Range(1, int.MaxValue, ErrorMessage = "Please Enter Proper Price")]
        public int Price { get; set; }
        [Required(ErrorMessage = "Please Select Currency")]
        [RegularExpression("^[a-zA-Z]*$", ErrorMessage = "Please Select Proper Currency")]
        public string Currency { get; set; }
        public string ImagePath { get; set; }
    }
}
