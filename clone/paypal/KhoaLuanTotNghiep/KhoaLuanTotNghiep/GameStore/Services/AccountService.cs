using GameStore.Enums;
using GameStore.Extensions;
using GameStore.Helper;
using GameStore.Interfaces.InterfaceRepositories;
using GameStore.Interfaces.InterfaceServices;
using GameStore.Models;
using GameStore.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GameStore.Services
{
    public class AccountService : IAccountService
    {
        private readonly IAccountRepository _accountRepository;

        public AccountService(IAccountRepository accountRepository)
        {
            _accountRepository = accountRepository;
        }

        /// <summary>
        /// Đăng nhập tài khoản
        /// </summary>
        /// <param name="model">Login View Model</param>
        /// <returns>
        /// -1: Không tìm thấy tài khoản được đăng ký với địa chỉ email này
        ///  2: Đăng nhập thất bại
        ///  Các trường hợp còn lại: Đăng nhập thành công
        /// </returns>
        public int SignIn(LoginViewModel model)
        {
            Account account = _accountRepository.FindByEmail(model.Email);

            if (account == null)
                return -1;

            return _accountRepository.SignIn(account, model.Password);
        }

        /// <summary>
        /// Đăng ký tài khoản
        /// </summary>
        /// <param name="model">Register View Model</param>
        /// <returns>
        /// -1: Địa chỉ email này đã được đăng ký tài khoản
        ///  0: Có lỗi trong quá trình đăng ký
        ///  1: Đăng ký thành công
        /// </returns>
        public int SignUp(RegisterViewModel model)
        {
            if (_accountRepository.FindByEmail(model.Email) != null)
                return -1;

            string salt = Utilities.GetRandomKey();

            Account account = new Account()
            {
                Email = model.Email,
                Username = model.Username,
                Password = (model.Password + salt.Trim()).ToMD5(),
                Salt = salt
            };

            return _accountRepository.SignUp(account);
        }

        /// <summary>
        /// Đổi mật khẩu tài khoản
        /// </summary>
        /// <param name="model">Change Password View Model</param>
        /// <param name="accountId"></param>
        /// <returns>
        /// -2: Mật khẩu hiện tại không trùng khớp
        /// -1: Không tìm thấy tài khoản được đăng ký với địa chỉ email này
        ///  0: Có lỗi trong quá trình đổi mật khẩu
        ///  1: Đổi mật khẩu thành công
        /// </returns>
        public int ChangePassword(ChangePasswordViewModel model, int accountId)
        {
            Account account = _accountRepository.GetById(accountId);

            if (account == null)
                return -1;

            string oldPassword = (model.OldPassword.Trim() + account.Salt.Trim()).ToMD5();
            string newPassword = (model.NewPassword.Trim() + account.Salt.Trim()).ToMD5();

            if (!oldPassword.Equals(account.Password))
                return -2;

            account.Password = newPassword;
            return _accountRepository.UpdateAccount(account);
        }

        /// <summary>
        /// Cập nhật số dư ví tài khoản
        /// </summary>
        /// <param name="accountId"></param>
        /// <param name="updateType">Loại cập nhật</param>
        /// <param name="balance">Số dư</param>
        /// <returns>
        /// -1: Không tìm thấy tài khoản
        ///  0: Có lỗi trong quá trình cập nhật số dư ví
        ///  1: Cập nhật số dư ví thành công
        /// </returns>
        public int UpdateWalletBalance(int accountId, int updateType, int balance)
        {
            Account account = _accountRepository.GetById(accountId);

            if (account == null)
                return -1;

            switch (updateType)
            {
                case (int)TransactionTypes.paypal:
                    account.WalletBalance += balance;
                    break;
                default:
                    return 0;
            }

            return _accountRepository.UpdateAccount(account);
        }
    }
}
