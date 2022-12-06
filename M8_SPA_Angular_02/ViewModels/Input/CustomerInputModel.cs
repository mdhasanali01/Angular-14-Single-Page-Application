using System.ComponentModel.DataAnnotations;

namespace M8_SPA_Angular_02.ViewModels.Input
{
    public class CustomerInputModel
    {
        public int CustomerID { get; set; }
        [Required, StringLength(50), Display(Name = "Customer Name")]
        public string CustomerName { get; set; } = default!;
        [Required, StringLength(150)]
        public string Address { get; set; } = default!;
        [Required, StringLength(50)]
        public string Email { get; set; } = default!;
    }
}
