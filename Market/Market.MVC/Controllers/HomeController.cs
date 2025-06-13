using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Market.MVC.Models;
using Microsoft.AspNetCore.Authorization;
using Market.Application.Features.Products.Queries.GetLatestByType;
using Market.Domain.Enums;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Market.MVC.Controllers;

public class HomeController : Controller
{
    private readonly ILogger<HomeController> _logger;
    private readonly IMediator _mediator;

    public HomeController(ILogger<HomeController> logger, IMediator mediator)
    {
        _logger = logger;
        _mediator = mediator;
    }

    public async Task<IActionResult> Index()
    {
        _logger.LogInformation("Загрузка главной страницы");
        
        var latestWeb = await _mediator.Send(new GetLatestProductsByTypeQuery(ProductType.WebTemplates));
        var latestFonts = await _mediator.Send(new GetLatestProductsByTypeQuery(ProductType.Fonts));
        var latestGraphics = await _mediator.Send(new GetLatestProductsByTypeQuery(ProductType.Graphics));

        _logger.LogInformation("Загружены последние товары: WebTemplates: {WebCount}, Fonts: {FontsCount}, Graphics: {GraphicsCount}", 
            latestWeb.Count(), latestFonts.Count(), latestGraphics.Count());

        var viewModel = new HomeViewModel
        {
            latestWeb = latestWeb,
            latestFonts = latestFonts,
            latestGraphics = latestGraphics
        };

        return View(viewModel);
    }

    public IActionResult Privacy()
    {
        _logger.LogInformation("Просмотр страницы конфиденциальности");
        return View();
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        _logger.LogError("Ошибка приложения. RequestId: {RequestId}", 
            Activity.Current?.Id ?? HttpContext.TraceIdentifier);
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}