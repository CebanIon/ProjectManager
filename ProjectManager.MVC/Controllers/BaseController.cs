using MediatR;
using Microsoft.AspNetCore.Mvc;
using ProjectManager.MVC.Models;
using System.Diagnostics;

namespace ProjectManager.MVC.Controllers
{
    public class BaseController : Controller
    {
        private IMediator _mediator = null!;
        protected IMediator Mediator => _mediator ??= HttpContext.RequestServices.GetService<IMediator>();
    }
}