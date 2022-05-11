using Cards.API.Data;
using Cards.API.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Cards.API.Controllers
{
    [Route("api/cards")]
    [ApiController]
    public class CardsController : Controller
    {
        private readonly CardContext _context;

        public CardsController(CardContext context)
        {
            _context = context;
        }

        // GET: api/cards
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Card>>> GetAllCards() =>
            await _context.Cards.ToListAsync();
        
        // GET: api/cards/{id}
        [HttpGet]
        [Route("{id:guid}")]
        [ActionName("GetCardById")]
        public async Task<ActionResult<Card>> GetCardById([FromRoute] Guid id ) 
        {
            //use the FirstOrDefaultAsync
            //var cardItem = await _context.Cards.FirstOrDefaultAsync(x =>x.Id ==id);
            //or use the FindAsync
            var cardItem = await _context.Cards.FindAsync(id);
            if(cardItem == null)
                return NotFound();
            return Ok(cardItem);
        }

        // PUT: api/cards/{id}
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPut]
        [Route("{id:guid}")]
        public async Task<ActionResult<Card>> UpdateCard([FromRoute] Guid id, Card cardIterm)
        {
            if(id != cardIterm.Id)
                return BadRequest();

            _context.Entry(cardIterm).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
                //return Ok(cardIterm);
            }
            catch(DbUpdateConcurrencyException)
            {
                if(!CardExists(id))
                    return NotFound("Card not found");
                else
                    throw;
            }

            return NoContent();
        }

        // POST: api/cards
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPost]
        public async Task<ActionResult<Card>> PostCard([FromBody] Card cardItem)
        {
            cardItem.Id = Guid.NewGuid();
            await _context.Cards.AddAsync(cardItem);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetCardById), new { id = cardItem.Id }, cardItem);
        }

        // DELETE: api/cards/{id}
        [HttpDelete]
        [Route("{id:guid}")]
        public async Task<ActionResult<Card>> DeleteCard(Guid id)
        {
            var cardItem = await _context.Cards.FindAsync();
            if(cardItem == null)
                return NotFound("Card not found");
            
            _context.Cards.Remove(cardItem);
            await _context.SaveChangesAsync();

            return Ok(cardItem);
        }

        private bool CardExists(Guid id)=>
             _context.Cards.Any(e =>e.Id == id);
    }
}