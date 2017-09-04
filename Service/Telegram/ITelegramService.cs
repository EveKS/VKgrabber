﻿using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using System.Threading;
using System;

namespace VkGroupManager.Service.Telegram
{
    public interface ITelegramService
    {
        Task SendMessage(string message);
        Task SendMessageExceptionAsync(Exception ex);
    }
}