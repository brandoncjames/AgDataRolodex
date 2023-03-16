using System;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Paycor.Chat.Api.Controllers
{
    [Authorize]
    [ApiController]
    public class ApiControllerBase : ControllerBase
    {
    }
}
