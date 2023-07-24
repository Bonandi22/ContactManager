using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ContactManager.Database;
using ContactManager.Models;
using System.Net.Http;
using System.Net.Http.Json;



namespace ContactManager.Controllers
{
    public class PersonController : Controller
    {
        private readonly DataContext _context;
   

        public PersonController(DataContext context)
        {
            _context = context;
        }


        // GET: Person
        public async Task<IActionResult> Index()
        {
            var peopleWithContacts = await _context.Persons.Include(p => p.Contacts).ToListAsync();
            return View(peopleWithContacts);
        }


        // GET: Person/Create   
        public IActionResult Create()
        {
            return View();
        }

        // POST: Person/Create   
        [HttpPost]
        public IActionResult Create(Person person)
        {
            if (ModelState.IsValid)
            {
                // Remover o caractere '+' do 'CountryCode' se presente
                if (person.Contacts != null && person.Contacts.Count > 0)
                {
                    foreach (var contact in person.Contacts)
                    {
                        if (!string.IsNullOrEmpty(contact.CountryCode))
                        {
                            contact.CountryCode = contact.CountryCode.TrimStart('+');
                        }
                    }
                }

                // Adicionar a pessoa ao contexto e salvar as mudanças no banco de dados
                _context.Persons.Add(person);
                _context.SaveChanges();

                return RedirectToAction("Index", "Home"); // Ou outra ação de redirecionamento
            }

            return View(person);
        }

        // GET: Person/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Persons == null)
            {
                return NotFound();
            }

            var person = await _context.Persons
                .Include(p => p.Contacts) // Incluir os contatos associados à pessoa
                .FirstOrDefaultAsync(m => m.Id == id);
            if (person == null)
            {
                return NotFound();
            }

            return View(person);
        }


        // GET: Person/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Persons == null)
            {
                return NotFound();
            }

            var person = await _context.Persons
                .Include(p => p.Contacts) // Incluir os contatos associados à pessoa
                .FirstOrDefaultAsync(m => m.Id == id);
            if (person == null)
            {
                return NotFound();
            }
            return View(person);
        }

        // POST: Person/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Name,Email,Contacts")] Person person, string[] CountryCode, string[] Number)
        {
            if (id != person.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    // Recupera a pessoa do banco de dados com seus contatos existentes
                    var existingPerson = await _context.Persons
                        .Include(p => p.Contacts)
                        .FirstOrDefaultAsync(p => p.Id == id);

                    // Atualiza os dados da pessoa (Name e Email)
                    existingPerson.Name = person.Name;
                    existingPerson.Email = person.Email;

                    // Atualiza ou adiciona os contatos associados à pessoa
                    for (int i = 0; i < CountryCode.Length; i++)
                    {
                        if (i < existingPerson.Contacts.Count)
                        {
                            // Atualiza os contatos existentes
                            existingPerson.Contacts[i].CountryCode = CountryCode[i];
                            existingPerson.Contacts[i].Number = Number[i];
                        }
                        else
                        {
                            // Adiciona novos contatos
                            existingPerson.Contacts.Add(new Contact
                            {
                                CountryCode = CountryCode[i],
                                Number = Number[i],
                                PersonId = id
                            });
                        }
                    }

                    // Salva as alterações no banco de dados
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!PersonExists(person.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }

            return View(person);
        }




        // GET: Person/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Persons == null)
            {
                return NotFound();
            }

            var person = await _context.Persons
                .FirstOrDefaultAsync(m => m.Id == id);
            if (person == null)
            {
                return NotFound();
            }

            return View(person);
        }

        // POST: Person/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Persons == null)
            {
                return Problem("Entity set 'DataContext.Persons'  is null.");
            }
            var person = await _context.Persons.FindAsync(id);
            if (person != null)
            {
                _context.Persons.Remove(person);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool PersonExists(int id)
        {
          return (_context.Persons?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
