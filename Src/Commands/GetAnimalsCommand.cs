using System;
using System.Collections.Generic;
using System.Linq;
using Gravityzero.Console.Utility.Context;
using Gravityzero.Console.Utility.Infrastructure;
using Gravityzero.Console.Utility.Model;
using Gravityzero.Console.Utility.Tools;

namespace Gravityzero.Console.Utility.Commands
{
    public class GetAnimalsCommand : ICommand
    {
        // komendy animal znajdują sie w katalogu Configuration
        public string Description => "Zwrócenie listy zwierząt";

        public CommandResult Execute(ConsoleContext context)
        {
            var result = WinApiConnector.RequestGet<string, Response<IEnumerable<Animal>>>("https://localhost:44377/Animal/Get","");
            if(result.Result.Result.Payload.Count()>0){
                int i = 1;
                foreach(var animal in result.Result.Result.Payload){
                    System.Console.WriteLine($"{i}. {animal.Name.ToUpper()}");
                    i++;
                }
            }
            return new CommandResult();
        }
    }
}