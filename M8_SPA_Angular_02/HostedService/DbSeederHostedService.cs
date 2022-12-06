
using M8_SPA_Angular_02.Models;

 namespace M8_SPA_Angular_02.HostedService
{
    public class DbSeederHostedService : IHostedService
    {
       
        IServiceProvider serviceProvider;
        public DbSeederHostedService(
            IServiceProvider serviceProvider)
        {
            this.serviceProvider = serviceProvider;
           
        }

        public async Task StartAsync(CancellationToken cancellationToken)
        {
            using (IServiceScope scope = serviceProvider.CreateScope())
            {
               
                  var db = scope.ServiceProvider.GetRequiredService<ProductDbContext>();

                  await SeedDbAsync(db);
               
            }
        }
       public async Task SeedDbAsync(ProductDbContext db)
        {
            await db.Database.EnsureCreatedAsync();
            if (!db.Customers.Any())
            {
                var c1 = new Customer { CustomerName = "Nur Rahman Sakib", Email = "sakib@gmail.com", Address = "Dhaka, Bangladesh",Picture="sakib.jpg" };
                await db.Customers.AddAsync(c1);
                var c2 = new Customer { CustomerName = "Mushfiqur Rahim", Email = "mushfiq@gmail.com", Address = "Dhaka, Bangladesh",Picture="8.jpg" };
                await db.Customers.AddAsync(c2);
                var p1 = new Product { ProductName = "Asus VivoBook X543U",Brand="Asus",ReleaseDate= DateTime.Today.AddDays(-1), IsAvailable = true, Price = 45000.00M, Picture = "1.jpg" };
                await db.Products.AddAsync(p1);
                var p2 = new Product { ProductName = "Hp EliteBook 650",Brand="Hp",ReleaseDate= DateTime.Today.AddDays(-1), IsAvailable = true, Price = 60000.00M, Picture = "2.jpg" };
                await db.Products.AddAsync(p2);
                var o1 = new Order { OrderDate = DateTime.Today.AddDays(-8), DeliveryDate = DateTime.Today.AddDays(-1), Customer = c1, Status = Status.Pending };
                o1.OrderItems.Add(new OrderItem { Order = o1, Product = p1, Quantity = 2 });
                var o2 = new Order { OrderDate = DateTime.Today.AddDays(-8), DeliveryDate = DateTime.Today.AddDays(-1), Customer = c2, Status = Status.Pending };
                o2.OrderItems.Add(new OrderItem { Order = o2, Product = p1, Quantity = 1 });
                o2.OrderItems.Add(new OrderItem { Order = o2, Product = p2, Quantity = 1 });
                await db.Orders.AddAsync(o1);
                await db.Orders.AddAsync(o2);
                await db.SaveChangesAsync();
            }

        }
        public Task StopAsync(CancellationToken cancellationToken)
        {
            return Task.CompletedTask;
        }
        
    }
}
