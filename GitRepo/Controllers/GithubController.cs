using GitRepo.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace GitRepo.Controllers
{
    [Route("api/github/braches")]
    [ApiController]
    public class GithubController : ControllerBase
    {

        private readonly GithubServices _service;


        public GithubController(GithubServices service)
        {
            _service = service;
        }


        [HttpGet("{owner}/{repo}")]
        public async Task<IActionResult> ListBranches(string owner, string repo)
        {
            var result = await _service.ListBranches(owner, repo);
            return Ok(result);
        }


        [HttpPost("{owner}/{repo}/create")]
        public async Task<IActionResult> CreateBranch(string owner, string repo, [FromQuery] string newBranch, [FromQuery] string fromBranch)
        {
            var result = await _service.CreateBranch(owner, repo, newBranch, fromBranch);
            return Ok(result);
        }

    }
}
