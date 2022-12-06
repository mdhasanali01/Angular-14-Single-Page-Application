using M8_SPA_Angular_02.Models;

namespace M8_SPA_Angular_02.ViewModels
{
    public class OrderViewModel
    {
        public int OrderID { get; set; }

        public DateTime OrderDate { get; set; }

        public DateTime? DeliveryDate { get; set; }

        public Status Status { get; set; }

        public int CustomerID { get; set; }
        public string CustomerName { get; set; } = default!;
        public decimal OrderValue { get; set; }
    }
}
