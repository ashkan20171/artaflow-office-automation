using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
namespace AshkanOfficeAutomation.Web.Controllers;
[Authorize]
public class ArchitectureController:Controller { public IActionResult Index()=>View(); }
