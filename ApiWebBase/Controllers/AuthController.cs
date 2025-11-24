using Application.Commands;
using Microsoft.AspNetCore.Mvc;

namespace ApiWebBase.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        // O Controller limpo, não injeta Service -> usa o Handler
        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginCommand command, [FromServices] LoginCommandHandler handler)
        {
            try
            {
                var result = await handler.Handle(command);
                return Ok(result);
            }
            catch (UnauthorizedAccessException ex)
            {
                return Unauthorized(new { message = ex.Message });
            }
        }
    }
}