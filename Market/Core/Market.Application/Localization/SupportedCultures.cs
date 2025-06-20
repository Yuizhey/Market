using System.Collections.Generic;

namespace Market.Application.Localization;

public static class SupportedCultures
{
    public static readonly List<string> Supported = new() { "en", "ru" };
    public const string DefaultCulture = "ru";
} 