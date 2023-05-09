using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Xml.Linq;

namespace NewOnlineVotingApplication.Models
{
    public class UserRegistration
    {
        [Key]
        public int UserId { get; set; }
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
        [RegularExpression(@"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[@$!%*?&])[A-Za-z\d@$!%*?&]{8,}$",
        ErrorMessage = "Password must be at least 8 characters long and contain at least one lowercase letter, one uppercase letter, one digit, and one special character (@$!%*?&)")]
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
        public string? IdProof { get; set; }

        [Column("Voting Status")]
        public string? Voting_status { get; set; }


    }
}
