using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Numerics;

namespace NewOnlineVotingApplication.Models
{
    public class VoteResults
    {
        [Key]
        public int VoterId { get; set; }
        public string? VoterName { get; set; }
        public string? Email { get; set; }

        public string? Vote { get; set; }
      

    }
}
