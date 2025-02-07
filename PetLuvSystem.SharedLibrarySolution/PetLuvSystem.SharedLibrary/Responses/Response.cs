using Microsoft.AspNetCore.Mvc;

namespace PetLuvSystem.SharedLibrary.Responses
{
    public record Response(bool Flag = false, int StatusCode = 200, string Message = null!)
    {
        public object? Data { get; init; }

        public IActionResult ToActionResult(ControllerBase controller)
        {
            return StatusCode switch
            {
                200 => controller.Ok(this),
                201 => controller.Created(string.Empty, this),
                204 => controller.NoContent(),
                400 => controller.BadRequest(this),
                //401 => controller.Unauthorized(this),
                //403 => controller.Forbid(),
                404 => controller.NotFound(this),
                409 => controller.Conflict(this),
                500 => controller.StatusCode(500, this),
                _ => controller.StatusCode(StatusCode, this)
            };
        }
    }
}
