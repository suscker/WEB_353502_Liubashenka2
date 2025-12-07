using Microsoft.AspNetCore.Mvc;

namespace WEB_353502_Liubashenka2.ViewComponents
{
    public class CartViewComponent : ViewComponent
    {
        public IViewComponentResult Invoke()
        {
            var cartSummary = new CartSummaryViewModel
            {
                TotalAmount = 00.0m,
                ItemsCount = 0
            };

            return View(cartSummary);
        }
    }

    public class CartSummaryViewModel
    {
        public decimal TotalAmount { get; set; }
        public int ItemsCount { get; set; }
    }
}