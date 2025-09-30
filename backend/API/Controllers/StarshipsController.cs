using Application.Servicies;
using Domain.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class StarshipsController : ControllerBase
    {
        private readonly ISwapiFacadeService _swapiFacade;

        public StarshipsController(ISwapiFacadeService swapiFacade)
        {
            _swapiFacade = swapiFacade ?? throw new ArgumentNullException(nameof(swapiFacade));
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Starship>>> GetStarships([FromQuery] string? search = null, CancellationToken ct = default)
        {

                var starships = await _swapiFacade.GetStarshipsAsync(search, ct);
                return Ok(starships);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<StarshipResponse>> GetStarshipById(int id, CancellationToken ct = default)
        {
                var starship = await _swapiFacade.GetEnrichedStarshipByIdAsync(id, ct);
                return Ok(starship);
        }
    }
} 
