using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using VkGroupManager.Models;
using VkGroupManager.ViewModels.Order;
using VkGroupManager.ViewModels.VK;

namespace VkGroupManager.Components.Order
{
    public class OrderViewComponent : ViewComponent
    {
        const decimal SUM = 40;

        private ApplicationContext _context;
        private readonly SignInManager<User> _signInManager;
        private readonly UserManager<User> _userManager;

        public OrderViewComponent(ApplicationContext context,
            SignInManager<User> signInManager, UserManager<User> userManager)
        {
            _signInManager = signInManager;
            _userManager = userManager;
            _context = context;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            if (_context != null && _signInManager.IsSignedIn(HttpContext.User))
            {
                var user = await _userManager.GetUserAsync(HttpContext.User);
                var order = await _context.Orders
                    .FirstOrDefaultAsync(id => id.OrderId == user.OrderId);

                if (order == null)
                {
                    order = new Models.Order
                    {
                        Sum = SUM
                    };

                    user.Order = order;
                    await _context.AddAsync(order);
                    _context.Update(user);

                    await _context.SaveChangesAsync();
                }

                var data = new VkResponseAndGroupVk
                {
                    Order = new OrderViewModel
                    {
                        OrderId = order.OrderId,
                        Sum = SUM,
                        IsNotOrdered = order.Payed > 0 && order.Payed < SUM
                    }
                };

                if (order.Payed != null)
                {
                    data.Order.LastOrdered = order.Payed.Value;
                }
                else
                {
                    data.Order.LastOrdered = 0;
                }

                data.Order.DateEnd = order.DateEnd;

                return View(data);
            }

            return View();
        }
    }
}
