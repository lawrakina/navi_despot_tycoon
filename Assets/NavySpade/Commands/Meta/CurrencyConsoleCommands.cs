using System.Linq;
using NavySpade.Meta.Runtime.Economic.Currencies;
using QFSW.QC;
using UnityEngine;

namespace NavySpade.Commands.Meta
{
    [CommandPrefix("meta.currency.")]
    public static class CurrencyConsoleCommands
    {
        [Command("add")]
        public static void AddCoins(Currency currency, int count)
        {
            currency.Count += count;
        }
        
        [Command("set")]
        public static void SetCoins(Currency currency, int value)
        {
            currency.Count = value;
        }
        
        [Command("all.add")]
        public static void AddCoins(int count)
        {
            var currencies = CurrencyConfig.Instance.UsedInGame;
            
            foreach (var currency in currencies)
            {
                AddCoins(currency, count);
            }
        }
        
        [Command("all.set")]
        public static void SetCoins(int value)
        {
            var currencies = CurrencyConfig.Instance.UsedInGame;
            
            foreach (var currency in currencies)
            {
                SetCoins(currency, value);
            }
        }

        [Command("printall")]
        public static void PrintCurrencies()
        {
            var usedInGame = CurrencyConfig.Instance.UsedInGame;
            for (var i = 0; i < usedInGame.Count; i++)
            {
                var currency = usedInGame[i];
                Debug.Log($"{i} : {currency.name}");
            }
        }
    }
    
    public class CurrencyParser : BasicCachedQcParser<Currency>
    {
        public override Currency Parse(string value)
        {
            var config = CurrencyConfig.Instance;

            if (int.TryParse(value, out var intValue))
            {
                return config.UsedInGame[intValue];
            }

            return config.UsedInGame.FirstOrDefault((c) => c.name == value);
        }
    }
}