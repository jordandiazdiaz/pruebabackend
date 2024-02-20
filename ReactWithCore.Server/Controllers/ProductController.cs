using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ReactWithCore.Server.Models;
using System.Buffers.Text;
using System.Data;
using System.Data.SqlClient;
using static System.Runtime.InteropServices.JavaScript.JSType;
using System.Net.NetworkInformation;
using static System.Net.Mime.MediaTypeNames;
using ReactWithCore.Server.Services;
using Microsoft.Extensions.Logging;

namespace ReactWithCore.Server.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class ProductController : ControllerBase
    {
        private readonly ICrudService _iCrudService;
        private readonly ILogger<ProductController> _logger;   
        public ProductController(ICrudService iCrudService, ILogger<ProductController> logger)
        {
            _iCrudService = iCrudService;
            _logger = logger;
            _logger.LogInformation("Application is Starting....");         

        }

        [HttpGet]
        [Route("ListOfProducts")]
        public Response ListOfProducts()
        {
            Response response = new Response();
            response = _iCrudService.ReadDataFromDb();
            return response;
        }

        [HttpPost]
        [Route("AddNewProduct")]
        public Response AddNewProduct(Product product)
        {
            Response response = new Response();
            response = _iCrudService.Create(product);
            return response;
        }

        [HttpPut]
        [Route("UpdateProduct")]
        public Response UpdateProduct(Product product)
        {
            Response response = new Response();
            response = _iCrudService.Update(product);
            return response;
        }

        [HttpDelete]
        [Route("DeleteProduct")]
        public Response DeleteProduct(int id)
        {
            Response response = new Response();
            response = _iCrudService.Delete(id);
            return response;
        }


        [HttpGet]
        [Route("GetProductById")]
        public Response GetProductById(int id, bool isImagePath = false)
        {
            Response response = new Response();
            response = _iCrudService.GetProductById(id, isImagePath);
            return response;
        }

        [HttpPost]
        [Route("AddToCart")]
        public Response AddToCart(Product product)
        {

            Response response = new Response();
            response = _iCrudService.AddToCart(product);
            return response;
        }

        [HttpGet]
        [Route("ListOfCartProducts")]
        public Response ListOfCartProducts()
        {
            Response response = new Response();
            response = _iCrudService.ListOfCartProducts();
            return response;
        }

        [HttpDelete]
        [Route("DeleteCartProduct")]
        public Response DeleteCartProduct(int Id)
        {
            Response response = new Response();
            response = _iCrudService.DeleteCartProduct(Id);
            return response;
        }


    }
}
