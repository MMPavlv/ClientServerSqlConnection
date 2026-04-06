using Microsoft.AspNetCore.Mvc;
using Server.Services;

namespace Server.Controllers;

[ApiController]
[Route("[controller]/[action]")]
public class VersionController : ControllerBase
{
    private readonly BdService _bdService;

    public VersionController(BdService bdService)
    {
        _bdService = bdService;
    }

    [HttpGet]
    public ActionResult<System.Data.ConnectionState> GetConnectionState()
    {
        return _bdService.GetConnectionState();
    }

    [HttpGet]
    public ActionResult<System.Data.ConnectionState> Connect()
    {
        return _bdService.Connect();
    }

    [HttpGet]
    public ActionResult<System.Data.ConnectionState> Close()
    {
        return _bdService.Close();
    }

    [HttpGet]
    public ActionResult<string> GetVersion()
    {
        return _bdService.GetVersion();
    }
}
