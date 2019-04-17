using System.Collections.Generic;
using System.Linq;
using Gravityzero.Console.Utility.Context;
using Gravityzero.Console.Utility.Infrastructure;
using Gravityzero.Console.Utility.Model;
using Gravityzero.Console.Utility.Tools;

namespace Gravityzero.Console.Utility.Commands
{
    public class DeleteRoleCommand : BaseCommand<RoleArgument>
    {
        // W kontrolerze nie może być atrybutu [FromBody] jeżeli parametr identifier przekazuje się w url
        // Brak w kontrolerze metody zwracającej obiekt wg query dlatego należy uzupełnić path przy zmiennej role
        // Do uzyskania obiektu po nazwie użyłem RequestPost, a nie RequestGet
        public DeleteRoleCommand(IList<string> args) : base(args)
        {
        }

        protected override CommandResult Execute(ConsoleContext context, RoleArgument arguments)
        {
            var role = WinApiConnector.RequestPost<string, Response<IEnumerable<Role>>>("http://localhost:5000/Api/Configuration/",arguments.Name);
            if(role.Result.Result.Payload.Count()==1){
                var result = WinApiConnector.RequestDelete<string, Response<string>>("http://localhost:5000/Api/Configuration/DeleteRole"+$"/{role.Result.Result.Payload.FirstOrDefault().Identifier}");
                return new CommandResult(result.Result.IsSuccess ? "OK" : result.Result.Message);
            }
            else if(role.Result.Result.Payload.Count()>1){
                System.Console.WriteLine($"Znaleziono więcej niż jeden obiekt o nazwie {arguments.Name} ({role.Result.Result.Payload.Count()})");
                System.Console.WriteLine($"Dokonaj updatu jednej z nazw ({arguments.Name})");
                return new CommandResult();
            }
            else if(role.Result.Result.Payload.Count()==0){
                System.Console.WriteLine($"Brak obiektu w db ({arguments.Name})");
                return new CommandResult();
            }

            return new CommandResult();
        }
    }
}