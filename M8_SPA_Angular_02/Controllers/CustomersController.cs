using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using M8_SPA_Angular_02.Models;
using M8_SPA_Angular_02.Repositories.Interfaces;
using M8_SPA_Angular_02.ViewModels;
using M8_SPA_Angular_02.ViewModels.Input;

namespace M8_SPA_Angular_02.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CustomersController : ControllerBase
    {

        private IWebHostEnvironment env;
        IUnitOfWork unitOfWork;
        IGenericRepository<Customer> repo;
        public CustomersController(IUnitOfWork unitOfWork, IWebHostEnvironment env)
        {
            this.unitOfWork = unitOfWork;
            this.repo = this.unitOfWork.GetRepo<Customer>();
            this.env = env;
        }

        // GET: api/Customers
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Customer>>> GetCustomers()
        {
            var data = await this.repo.GetAllAsync();
            return data.ToList();
        }
        [HttpGet("VM")]
        public async Task<ActionResult<IEnumerable<CustomerViewModel>>> GetCustomerViewModels()
        {
            var data = await this.repo.GetAllAsync(x => x.Include(c => c.Orders));
            return data.Select(c => new CustomerViewModel
            {
                CustomerID = c.CustomerID,
                CustomerName = c.CustomerName,
                Address = c.Address,
                Email = c.Email,
                Picture = c.Picture,
                CanDelete = c.Orders.Count == 0
            }).ToList();
        }
        /// <summary>
        /// to get all customers with order entries
        /////////////////////////////////////////////
        [HttpGet("WithOrders")]
        public async Task<ActionResult<IEnumerable<Customer>>> GetCustomerWithOrders()
        {
            var data = await this.repo.GetAllAsync(x => x.Include(c => c.Orders));
            return data.ToList();
        }
        // GET: api/Customers/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Customer>> GetCustomer(int id)
        {
            var customer = await this.repo.GetAsync(c => c.CustomerID == id);

            if (customer == null)
            {
                return NotFound();
            }

            return customer;
        }
        /// <summary>
        /// to get single customer with order entries
        /////////////////////////////////////////////
        [HttpGet("{id}/WithOrders")]
        public async Task<ActionResult<Customer>> GetCustomerWithOrders(int id)
        {
            var customer = await this.repo.GetAsync(c => c.CustomerID == id, x => x.Include(c => c.Orders));

            if (customer == null)
            {
                return NotFound();
            }

            return customer;
        }
        // PUT: api/Customers/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}/VM")]
        public async Task<IActionResult> PutCustomerViewModel(int id, CustomerInputModel customer)
        {
            if (id != customer.CustomerID)
            {
                return BadRequest();
            }

            var existing = await this.repo.GetAsync(p => p.CustomerID == id);
            if (existing != null)
            {
                existing.CustomerName = customer.CustomerName;
                existing.Address = customer.Address;
                existing.Email = customer.Email;
                await this.repo.UpdateAsync(existing);
            }

            try
            {
                await this.unitOfWork.CompleteAsync();
            }
            catch (DbUpdateConcurrencyException)
            {

                throw;

            }

            return NoContent();
        }

        // POST: api/Customers
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Customer>> PostCustomer(Customer customer)
        {
            await this.repo.AddAsync(customer);
            await unitOfWork.CompleteAsync();

            return customer;
        }
        [HttpPost("VM")]
        public async Task<ActionResult<Customer>> PostCustomerInput(CustomerInputModel customer)
        {
            var newCustomer = new Customer
            {
                CustomerName = customer.CustomerName,
                Address = customer.Address,
                Email = customer.Email,
                Picture = "no-product-image-400x400.png"
            };
            await this.repo.AddAsync(newCustomer);
            await this.unitOfWork.CompleteAsync();

            return newCustomer;
        }
        [HttpPost("Upload/{id}")]
        public async Task<ImagePathResponse> UploadPicture(int id, IFormFile picture)
        {
            var customer = await this.repo.GetAsync(p => p.CustomerID == id);
            var ext = Path.GetExtension(picture.FileName);
            string fileName = Guid.NewGuid() + ext;
            string savePath = Path.Combine(this.env.WebRootPath, "Pictures", fileName);
            FileStream fs = new FileStream(savePath, FileMode.Create);
            picture.CopyTo(fs);
            fs.Close();
            customer.Picture = fileName;
            await this.repo.UpdateAsync(customer);
            await this.unitOfWork.CompleteAsync();
            return new ImagePathResponse { PictureName = fileName };
        }
        // DELETE: api/Customers/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteCustomer(int id)
        {
            var customer = await repo.GetAsync(c => c.CustomerID == id);
            if (customer == null)
            {
                return NotFound();
            }

            await this.repo.DeleteAsync(customer);
            await unitOfWork.CompleteAsync();

            return NoContent();
        }
    }

}
