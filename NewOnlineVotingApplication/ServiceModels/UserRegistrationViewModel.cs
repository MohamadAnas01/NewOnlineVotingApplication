using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Xml.Linq;

namespace NewOnlineVotingApplication.ServiceModels
{
    public class UserRegistrationViewModel
    {
        [Required(ErrorMessage = "First name is required.")]
        [Display(Name = "First Name")]
        [Column("First Name")]
        public string? FirstName { get; set; }


        [Required(ErrorMessage = "Last name is required.")]
        [Display(Name = "Last Name")]
        [Column("Last Name")]
        public string? LastName { get; set; }

        [Required(ErrorMessage = "Select your Date of Birth")]
        [Display(Name = "Date of Birth")]
        [Column("Date Of Birth")]
        public DateTime DateOfBirth { get; set; }
        [Required(ErrorMessage = "Select the Gender")]
        [Display(Name = "Gender")]
        public string? Gender { get; set; }
        [Required(ErrorMessage = "Phone number Required")]
        [Display(Name = "Phone Number")]
        [Column("Phone Number")]
        [DataType(DataType.PhoneNumber)]
        [RegularExpression(@"^\(?([0-9]{3})\)?[-. ]?([0-9]{3})[-. ]?([0-9]{4})$", ErrorMessage = "Not a valid phone number")]
        public string? PhoneNumber { get; set; }



        [Required(ErrorMessage = "Email is Required")]
        [DataType(DataType.EmailAddress)]
        [Display(Name = "Email")]
        public string? Email { get; set; }
        [Required(ErrorMessage = "Password is Required")]
        /*[RegularExpression("^((?=.?[A-Z])(?=.?[a-z])(?=.?[0-9])|(?=.?[A-Z])(?=.?[a-z])(?=.?[^a-zA-Z0-9])|(?=.?[A-Z])(?=.?[0-9])(?=.?[^a-zA-Z0-9])|(?=.?[a-z])(?=.?[0-9])(?=.?[^a-zA-Z0-9])).{8,}$", ErrorMessage = "Passwords must be at least 8 characters and contain at 3 of 4 of the following: upper case (A-Z), lower case (a-z), number (0-9) and special character (e.g. !@#$%^&*)")]*/
        [DataType(DataType.Password)]
        [Display(Name = "Password")]
        public string? Password { get; set; }
        [Required]
        [Compare("Password", ErrorMessage = "Password doesn't match")]
        [DataType(DataType.Password)]
        public string? ConfirmPassword { get; set; }
        [Required(ErrorMessage = "Enter Your Vote Id Number")]
        [Display(Name = "Vote Id Number")]
        [Column("Voter Id")]
        public string? VoterId { get; set; }

        [Required(ErrorMessage = "Attach Your Id Proof")]
        [Display(Name = "Id Proof")]
        [Column("Id Proof")]
        public IFormFile? IdProof { get; set; }

        [Column("Voting Status")]
        public string? Voting_status { get; set; }

    }
}

