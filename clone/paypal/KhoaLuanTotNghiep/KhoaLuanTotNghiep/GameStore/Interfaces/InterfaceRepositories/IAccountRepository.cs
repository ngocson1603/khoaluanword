using GameStore.Models;
using GameStore.ViewModels;

namespace GameStore.Interfaces.InterfaceRepositories
{
    public interface IAccountRepository : IGameStoreRepository<Account>
    {
        public Account FindByEmail(string email);
        public int SignIn(Account account, string password);
        public int SignUp(Account account);
        public int UpdateAccount(Account account);
    }
}
