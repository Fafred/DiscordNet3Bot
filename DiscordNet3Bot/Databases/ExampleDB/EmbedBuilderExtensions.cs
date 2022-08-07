using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DiscordNet3Bot.Databases.ExampleDB
{
    public static class EmbedBuilderExtensions
    {
        public static Discord.EmbedBuilder EquipmentInfoListToField(this Discord.EmbedBuilder embedBuilder, List<EquipmentModel>? equipments = null)
        {
            // NOTE:
            //  This will not work for lists of even moderate size, as it will cause an overflow.
            //  Should check for the length - fields can't have more than 1024 characters (iirc)
            StringBuilder stringBuilder = new StringBuilder();

            equipments ??= new List<EquipmentModel>();

            // This uses LINQ to go through and find the longest string in the ItemDescription field.
            //  We take the length of the longest string and add 4 to it to ensure decent looking spacing.
            int descriptionPadLength = equipments
                .Select(x => (x.ItemDescription ?? "").Length)
                .DefaultIfEmpty()
                .Max();

            // We want the descriptionPadLength to at least be length of the word "Description" + 4.
            descriptionPadLength = (descriptionPadLength > 10) ? descriptionPadLength += 4 : descriptionPadLength = 15;

            // Now the boring stuff - user interface.
            // List the equipment in neatly padded columns.
            stringBuilder.Append("`id".PadRight(7, ' '));
            stringBuilder.Append("Description".PadRight(descriptionPadLength, ' '));
            stringBuilder.Append("Cost".PadRight(11, ' '));
            stringBuilder.Append("Weight (in coins) `\n");

            string fieldName = stringBuilder.ToString();
            stringBuilder.Clear();

            stringBuilder.AppendLine("`".PadRight(fieldName.Length - 2, '-'));

            foreach (var equipment in equipments.OrderBy(x => x.ItemDescription))
            {
                stringBuilder.Append($"{equipment.id})".PadRight(6, ' '));
                stringBuilder.Append((equipment.ItemDescription ?? "").PadRight(descriptionPadLength, ' '));
                stringBuilder.Append($"{equipment.CurrencyAmount ?? 0} {(equipment.CurrencyType ?? "")}".PadRight(11, ' '));
                stringBuilder.AppendLine($"{equipment.WeightInCoins ?? 0}".PadRight(18, ' '));
            }

            if (equipments.Count < 1)
            {
                stringBuilder.AppendLine("".PadLeft(fieldName.Length - 2, ' '));
            }

            stringBuilder.AppendLine("`".PadLeft(fieldName.Length - 2, '-'));

            return embedBuilder.AddField(fieldName, stringBuilder.ToString(), inline: false);
        }
    }
}
