using GameStore.ViewModels;

namespace GameStore.Interfaces.InterfaceServices
{
    public interface IAccountService
    {
        public int SignIn(LoginViewModel model);
        public int SignUp(RegisterViewModel model);
        public int ChangePassword(ChangePasswordViewModel model, int accountId);
        public int UpdateWalletBalance(int accountId, int updateType, int balance);
    }
}
