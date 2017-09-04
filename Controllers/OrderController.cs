using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using VkGroupManager.Models;
using VkGroupManager.Service.Telegram;
using VkGroupManager.ViewModels.Order;

namespace VkGroupManager.Controllers
{
    public class OrderController : Controller
    {
        const decimal SUM = 40;

        private ApplicationContext _context;
        private readonly SignInManager<User> _signInManager;
        private readonly UserManager<User> _userManager;

        private ITelegramService _telegramService;

        public OrderController(ApplicationContext context,
            SignInManager<User> signInManager, UserManager<User> userManager,
            ITelegramService telegramService)
        {
            _signInManager = signInManager;
            _userManager = userManager;
            _context = context;

            _telegramService = telegramService;
        }

        public async Task<IActionResult> Index()
        {
            var user = await _userManager.GetUserAsync(HttpContext.User);
            var order = await _context.Orders
                .FirstOrDefaultAsync(id => id.OrderId == user.OrderId);

            if (order == null)
            {
                order = new Order();
                user.Order = order;
                await _context.AddAsync(order);
                _context.Update(user);

                await _context.SaveChangesAsync();
            }

            return View(new OrderViewModel { OrderId = order.OrderId });
        }

        [HttpPost]
        //[ValidateAntiForgeryToken]
        [ResponseCache(Location = ResponseCacheLocation.None, NoStore = true)]
        public async Task<IActionResult> Paid(string notification_type, string operation_id, string label, string datetime,
            string amount, string withdraw_amount, string sender, string sha1_hash, string currency, bool codepro)
        {
            try
            {
                Decimal.TryParse(amount, out var am);
                Decimal.TryParse(withdraw_amount, out var wa);

                string notification_secret = "q+YzWEJ7ZvV14cc679eqsjhB"; // секретный код
                                                                         // проверяем хэш
                string paramString = String.Format("{0}&{1}&{2}&{3}&{4}&{5}&{6}&{7}&{8}",
                    notification_type, operation_id, amount.Replace(',', '.'), currency, datetime, sender,
                    codepro.ToString().ToLower(), notification_secret, label);

                string paramStringHash1 = GetHash(paramString);

                await _telegramService.SendMessage($"{am} {wa}");
                await _telegramService.SendMessage(sha1_hash);
                await _telegramService.SendMessage(paramStringHash1);
                await _telegramService.SendMessage(paramString);

                // создаем класс для сравнения строк
                StringComparer comparer = StringComparer.OrdinalIgnoreCase;
                // если хэши идентичны, добавляем данные о заказе в бд
                if (0 == comparer.Compare(paramStringHash1, sha1_hash) && am != 0)
                {
                    await _telegramService.SendMessage($"ordered: {label}");

                    Order order = await _context.Orders
                        .FirstOrDefaultAsync(o => o.OrderId == label);

                    if (order != null)
                    {
                        await _telegramService.SendMessage(string.Format("Operation_Id: {0}\namount: {1}\nwithdraw_amount: {2}\nsender: {3}",
                            operation_id,
                            amount,
                            withdraw_amount,
                            sender));

                        order.Operation_Id = operation_id;
                        order.Date = DateTime.Now;
                        order.Amount = am;
                        order.WithdrawAmount = wa;

                        order.Payed = (order.Payed ?? 0) + wa;

                        if (order.Payed >= SUM)
                        {
                            order.DateEnd = (order.DateEnd == null || order.DateEnd.Value < DateTime.Now) ? DateTime.Now.AddDays(31) : order.DateEnd.Value.AddDays(31);
                            order.Payed -= SUM;
                            order.IsOrdered = true;
                        }
                        else
                        {
                            order.IsOrdered = false;
                        }

                        order.Sender = sender;

                        _context.Update(order);
                        await _context.AddAsync(new Order { Date = DateTime.Now, Operation_Id = operation_id, Amount = am, WithdrawAmount = wa, Sum = SUM });
                        await _context.SaveChangesAsync();
                    }
                }
                //else
                //{
                //    await _telegramService.SendMessage($"not ordered: {label}");
                //}
            }
            catch (Exception ex)
            {
                await _telegramService.SendMessageExceptionAsync(ex);
            }

            return Ok("ok");
        }

        private string GetHash(string text)
        {
            byte[] bytes = Encoding.UTF8.GetBytes(text);
            using (var sha1 = SHA1.Create())
            {
                byte[] hashBytes = sha1.ComputeHash(bytes);

                return HexStringFromBytes(hashBytes);
            }
        }

        public static string HexStringFromBytes(byte[] bytes)
        {
            var sb = new StringBuilder();
            foreach (byte b in bytes)
            {
                var hex = b.ToString("x2");
                sb.Append(hex);
            }
            return sb.ToString();
        }
    }
}
