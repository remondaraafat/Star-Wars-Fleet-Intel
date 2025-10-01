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
        public async Task<ActionResult<IEnumerable<GetStarshipsDto>>> GetStarships([FromQuery] string? search = null, CancellationToken ct = default)
        {

            IEnumerable<GetStarshipsDto> starships = await _swapiFacade.GetStarshipsAsync(search, ct);
                return Ok(starships);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<EnrichedStarshipResponseDto>> GetStarshipById(int id, CancellationToken ct = default)
        {
            EnrichedStarshipResponseDto starshipResponse = await _swapiFacade.GetEnrichedStarshipByIdAsync(id, ct);
                return Ok(starshipResponse);
        }
    }
} 
