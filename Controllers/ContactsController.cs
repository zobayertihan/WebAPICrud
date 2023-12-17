using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebApiCRUD.Controllers.Models;
using WebApiCRUD.Data;

namespace WebApiCRUD.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ContactsController : Controller
    {
        private readonly ContactsAPIDbContext dbContext;
        public ContactsController(ContactsAPIDbContext dbContext) { 
            this.dbContext = dbContext;
        }

        [HttpGet]
        public async Task<IActionResult> GetContacts()
        {
            return Ok(await dbContext.Contacts.ToListAsync());
        }

        [HttpGet]
        [Route("{id:guid}")]
        public async Task<IActionResult> GetContact([FromRoute] Guid id)
        {
            var contact = await dbContext.Contacts.FindAsync(id);

            if (contact == null)
            {
                return NotFound();
            }

            return Ok(contact);

        }

        [HttpPost]
        public async Task<IActionResult> AddContact(AddContactRequest addContactRequest) {
            var contact = new Contact()
            {
                Id = Guid.NewGuid(),
                Address = addContactRequest.Address,
                FullName = addContactRequest.FullName,
                Phone = addContactRequest.Phone,
                Email = addContactRequest.Email,
            };
            await dbContext.Contacts.AddAsync(contact);
            await dbContext.SaveChangesAsync();
            return Ok(contact);
        }

        [HttpPut]
        [Route("{id:guid}")]
        public async Task<IActionResult> UpdateContact([FromRoute] Guid id, UpdateContactRequest updateContactRequest)
        {
            var contact = await dbContext.Contacts.FindAsync(id);


            if (contact != null) { 
                contact.FullName = updateContactRequest.FullName;
                contact.Phone = updateContactRequest.Phone;
                contact.Email = updateContactRequest.Email;
                contact.Address = updateContactRequest.Address;

                await dbContext.SaveChangesAsync();

                return Ok(contact);
            }

            return NotFound();
        }

        [HttpDelete]
        [Route("{id:guid}")]
        public async Task<IActionResult> DelectContact([FromRoute] Guid id)
        {
            var contact = await dbContext.Contacts.FindAsync(id);

            if(contact == null)
            {
                return NotFound();
            }

            dbContext.Remove(contact);
            await dbContext.SaveChangesAsync();
            return Ok();
        }
    }
}
