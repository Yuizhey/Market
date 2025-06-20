using System;
using System.Collections.Generic;
using Market.Application.Features.Sales.Queries.GetAllSales;

namespace Market.MVC.Areas.Admin.Models;

public class AdminSalesVM
{
    public IEnumerable<GetAllSalesDto> Sales { get; set; }
} 