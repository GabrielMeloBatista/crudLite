using LiteDbCRUDLibrary.Controllers;
using LiteDbWebApp.Models;
using Microsoft.AspNetCore.Mvc;

namespace LiteDbWebApp.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ProdutoController : CrudController<Produto>
    {
        // Nenhum código extra necessário, herda tudo do CrudController
    }
}
