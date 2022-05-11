using Cards.API.Data;
using Cards.API.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Cards.API.Controllers
{
    [Route("api/carddto")]
    [ApiController]
    public class CardDTOController : Controller
    {
        private readonly CardContext _context;

        public CardDTOController(CardContext context)
        {
            _context = context;
        }

        // GET: api/carddto
        [HttpGet]
        public async Task<ActionResult<IEnumerable<CardDTO>>> GetAllCardsx() 
        {
            return await _context.Cards
                .Select(x => ItemToDTO(x))
                .ToListAsync();
        }
        
        // GET: api/carddto/{id}
        [HttpGet]
        [Route("{id:guid}")]
        [ActionName("GetCardByIdx")]
        public async Task<ActionResult<CardDTO>> GetCardByIdx([FromRoute] Guid id ) 
        {
            //use the FirstOrDefaultAsync
            //var cardItem = await _context.Cards.FirstOrDefaultAsync(x =>x.Id ==id);
            //or use the FindAsync
            var cardItem = await _context.Cards.FindAsync(id);
            if(cardItem == null)
                return NotFound();

            return ItemToDTO(cardItem);
        }

        // PUT: api/carddto/{id}
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPut]
        [Route("{id:guid}")]
        public async Task<ActionResult<CardDTO>> UpdateCardx([FromRoute] Guid id, CardDTO cardItemDTO)
        {
            if(id != cardItemDTO.Id)
                return BadRequest();


            //_context.Entry(cardIterm).State = EntityState.Modified;
            var cardItem = await _context.Cards.FindAsync(id);
            if(cardItem == null)
                return NotFound("Card not Found");
            
            cardItem.CardholderName = cardItemDTO.CardholderName;
            cardItem.CardNumber = cardItemDTO.CardNumber;
            cardItem.ExpiryMonth = cardItemDTO.ExpiryMonth;
            cardItem.ExpiryYear = cardItemDTO.ExpiryYear;
            cardItem.CVC = cardItemDTO.CVC;

            try
            {
                await _context.SaveChangesAsync();
                //return Ok(cardIterm);
            }
            catch(DbUpdateConcurrencyException) when (!CardExists(id))
            {
                return NotFound("Card not found");
            }

            return NoContent();
        }

        // POST: api/carddto
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPost]
        public async Task<ActionResult<CardDTO>> PostCardx([FromBody] CardDTO cardItemDTO)
        {
            var cardItem = new Card
            {
                    CardholderName = cardItemDTO.CardholderName,
                    CardNumber = cardItemDTO.CardNumber,
                    ExpiryMonth = cardItemDTO.ExpiryMonth,
                    ExpiryYear = cardItemDTO.ExpiryYear,
                    CVC = cardItemDTO.CVC
            };

            cardItem.Id = Guid.NewGuid();
            await _context.Cards.AddAsync(cardItem);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetCardByIdx), new { id = cardItem.Id }, ItemToDTO(cardItem));
        }

        // DELETE: api/carddto/{id}
        [HttpDelete]
        [Route("{id:guid}")]
        public async Task<ActionResult<CardDTO>> DeleteCardx([FromRoute] Guid id)
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

        // If i want to use a DTO class object
        private static CardDTO ItemToDTO(Card cardItem) =>
                new CardDTO
                {
                    Id = cardItem.Id,
                    CardholderName = cardItem.CardholderName,
                    CardNumber = cardItem.CardNumber,
                    ExpiryMonth = cardItem.ExpiryMonth,
                    ExpiryYear = cardItem.ExpiryYear,
                    CVC = cardItem.CVC
                };
    }
}