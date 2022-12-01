using Dapper;
using GameStore.Context;
using GameStore.Enums;
using GameStore.Extensions;
using GameStore.Helper;
using GameStore.Interfaces;
using GameStore.Interfaces.InterfaceRepositories;
using GameStore.Models;
using GameStore.ViewModels;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GameStore.Repositories
{
    public class AccountRepository : GameStoreRepository<Account>, IAccountRepository
    {
        public AccountRepository(GameStoreDbContext context) : base(context)
        {

        }

        public Account FindByEmail(string email)
        {
            var query = @"select * from Account where Email = @email";
            var para = new DynamicParameters();
            para.Add("email", email);

            try
            {
                return (Account)Context.Database.GetDbConnection().Query<Account>(query, para).First();
            }
            catch (Exception)
            {
                return null;
            }
        }

        /// <summary>
        /// Đăng nhập tài khoản
        /// </summary>
        /// <param name="account">Tài khoản được đăng nhập</param>
        /// <param name="password">Mật khẩu đăng nhập so sánh với mật khẩu tài khoản</param>
        /// <returns>
        ///  2: Đăng nhập thất bại
        ///  Các trường hợp còn lại: Đăng nhập thành công
        /// </returns>
        public int SignIn(Account account, string password)
        {
            if (account.Password == (password + account.Salt.Trim()).ToMD5())
                return account.Id;

            return 2;
        }

        /// <summary>
        /// Đăng ký tài khoản
        /// </summary>
        /// <param name="account">Tài khoản được đăng ký</param>
        /// <returns>
        ///  0: Có lỗi trong quá trình đăng ký
        ///  1: Đăng ký thành công
        /// </returns>
        public int SignUp(Account account)
        {
            try
            {
                this.Create(account);
                return 1;
            }
            catch (Exception)
            {
                return 0;
            }
        }

        /// <summary>
        /// Cập nhật tài khoản
        /// </summary>
        /// <param name="account">Tài khoản được cập nhật</param>
        /// <returns>
        ///  0: Có lỗi trong quá trình cập nhật
        ///  1: Cập nhật thành côngs
        /// </returns>
        public int UpdateAccount(Account account)
        {
            try
            {
                this.Update(account);
                return 1;
            }
            catch (Exception)
            {
                return 0;
            }
        }
    }
}
