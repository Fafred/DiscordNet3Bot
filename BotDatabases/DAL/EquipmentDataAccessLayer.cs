using Microsoft.EntityFrameworkCore;
using BotDatabases.Context;
using BotDatabases.Models;

namespace BotDatabases
{
    public class EquipmentDataAccessLayer
    {
        private readonly IDbContextFactory<EquipmentContext> _contextFactory;

        public EquipmentDataAccessLayer(IDbContextFactory<EquipmentContext> contextFactory)
        {
            _contextFactory = contextFactory;
        }

        public async Task CreateEquipment(string itemDescription, int currencyAmount, string currencyType, int weightInCoins)
        {
            using var dbContext = _contextFactory.CreateDbContext();

            dbContext.Add(new EquipmentModel { ItemDescription = itemDescription, CurrencyAmount = currencyAmount, CurrencyType = currencyType, WeightInCoins = weightInCoins });

            await dbContext.SaveChangesAsync();
        }

        public async Task DeleteEquipment(int id)
        {
            using var dbContext = _contextFactory.CreateDbContext();

            var equipment = await dbContext.Equipment
                .FindAsync(id);
            
            if (equipment is not null)
            {
                dbContext.Remove(equipment);

                await dbContext.SaveChangesAsync();
            }
        }

        public EquipmentModel? GetEquipment(int id)
        {
            using var dbContext = _contextFactory.CreateDbContext();

            return dbContext.Equipment
                .FirstOrDefault(x => x.id == id);
        }

        public string? GetItemDescription(int id)
        {
            using var dbContext = _contextFactory.CreateDbContext();

            return dbContext.Equipment
                .FirstOrDefault(x => x.id == id)
                ?.ItemDescription;
        }

        public string? GetCurrencyType(int id)
        {
            using var dbContext = _contextFactory.CreateDbContext();

            return dbContext.Equipment
                .FirstOrDefault(x => x.id == id)
                ?.CurrencyType;
        }

        public int? GetCurrencyAmount(int id)
        {
            using var dbContext = _contextFactory.CreateDbContext();

            return dbContext.Equipment
                .FirstOrDefault(x => x.id == id)
                ?.CurrencyAmount;
        }

        public int? GetWeightInCoins(int id)
        {
            using var dbContext = _contextFactory.CreateDbContext();

            return dbContext.Equipment
                .FirstOrDefault(x => x.id == id)
                ?.WeightInCoins;
        }

        public async Task SetDescription(int id, string itemDescription)
        {
            using var dbContext = _contextFactory.CreateDbContext();

            var equipment = await dbContext.Equipment
                .FindAsync(id);

            if (equipment is null) return;

            equipment.ItemDescription = itemDescription;

            await dbContext.SaveChangesAsync();
        }

        public async Task SetCurrencyType(int id, string currencyType)
        {
            using var dbContext = _contextFactory.CreateDbContext();

            var equipment = await dbContext.Equipment
                .FindAsync(id);

            if (equipment is null) return;

            equipment.CurrencyType = currencyType;

            await dbContext.SaveChangesAsync();
        }

        public async Task SetCurrencyAmount(int id, int currencyAmount)
        {
            using var dbContext = _contextFactory.CreateDbContext();

            var equipment = await dbContext.Equipment
                .FindAsync(id);

            if (equipment is null) return;

            equipment.CurrencyAmount = currencyAmount;

            await dbContext.SaveChangesAsync();
        }

        public async Task SetWeightInCoins(int id, int weightInCoins)
        {
            using var dbContext = _contextFactory.CreateDbContext();

            var equipment = await dbContext.Equipment
                .FindAsync(id);

            if (equipment is null) return;

            equipment.WeightInCoins = weightInCoins;

            await dbContext.SaveChangesAsync();
        }
    }
}