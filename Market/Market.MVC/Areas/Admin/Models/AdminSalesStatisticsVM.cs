using System;
using System.Collections.Generic;
using Market.Application.Features.Sales.Queries.GetSalesStatistics;

namespace Market.MVC.Areas.Admin.Models;

public class AdminSalesStatisticsVM
{
    public IEnumerable<GetSalesStatisticsDto> Statistics { get; set; }
} 