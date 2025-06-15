using LiteDB;
using Microsoft.AspNetCore.Mvc;

namespace LiteDbWebApp.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CrudController<T> : ControllerBase where T : class, new()
    {
        private readonly LiteRepository _repo;

        public CrudController()
        {
            _repo = new LiteRepository("dados.db");
        }

        [HttpGet]
        public IActionResult GetAll()
        {
            var items = _repo.Query<T>().ToList();
            return Ok(items);
        }

        [HttpGet("{id}")]
        public IActionResult GetById(int id)
        {
            var item = _repo.SingleById<T>(id);
            if (item == null) return NotFound();
            return Ok(item);
        }

        [HttpPost]
        public IActionResult Create([FromBody] T item)
        {
            _repo.Insert(item);
            // Supondo que o modelo tenha uma propriedade Id
            var idProp = typeof(T).GetProperty("Id");
            var id = idProp?.GetValue(item);
            return CreatedAtAction(nameof(GetById), new { id }, item);
        }

        [HttpPut("{id}")]
        public IActionResult Update(int id, [FromBody] T item)
        {
            var idProp = typeof(T).GetProperty("Id");
            if (idProp == null || (int)idProp.GetValue(item)! != id) return BadRequest();
            _repo.Update(item);
            return NoContent();
        }

        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            _repo.Delete<T>(id);
            return NoContent();
        }
    }
}
