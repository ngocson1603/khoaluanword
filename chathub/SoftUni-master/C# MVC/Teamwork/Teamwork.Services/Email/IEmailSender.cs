﻿using System.Threading.Tasks;

namespace Teamwork.Services.Email
{
    public interface IEmailSender
    {
        Task SendEmailAsync(string email, string subject, string message);
    }
}
