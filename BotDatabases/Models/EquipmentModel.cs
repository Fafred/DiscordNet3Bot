﻿namespace BotDatabases.Models;

public partial class EquipmentModel : DBEntity
{
    public string? ItemDescription { get; set; }

    public int? CurrencyAmount { get; set; }

    public string? CurrencyType { get; set; }

    public int? WeightInCoins { get; set; }
}
