using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using NewOnlineVotingApplication.Models;

namespace NewOnlineVotingApplication.Controllers
{
    public class VoteResultsController : Controller
    {
        private readonly OnlineVotingAppContext _context;

        public VoteResultsController(OnlineVotingAppContext context)
        {
            _context = context;
        }
        [Authorize(Roles = "Admin")]
        // GET: VoteResults
        public async Task<IActionResult> Index()
        {
            return _context.VoteResults != null ?
                        View(await _context.VoteResults.ToListAsync()) :
                        Problem("Entity set 'OnlineVotingAppContext.VoteResults'  is null.");
        }
        [Authorize(Roles = "Admin")]
        public ActionResult DisplayVoteResults()
        {
            var voteCounts = new Dictionary<string, int>();
            var voteResults = _context.VoteResults.ToList();

            foreach (var result in voteResults)
            {
                if (voteCounts.ContainsKey(result.Vote))
                {
                    voteCounts[result.Vote]++;
                }
                else
                {
                    voteCounts[result.Vote] = 1;
                }
            }

            ViewBag.VoteCounts = voteCounts;

            return View();
        }

    }
}
