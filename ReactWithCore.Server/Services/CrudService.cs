using Microsoft.Extensions.Hosting;
using Microsoft.VisualBasic;
using ReactWithCore.Server.Models;
using System.Data.SqlClient;
using System.Data;
using System.Net;
using Dapper;

namespace ReactWithCore.Server.Services
{
    public class CrudService : ICrudService

    {
        private readonly IConfiguration _configuration;
        private readonly IWebHostEnvironment _hostEnvironment;
        private readonly string _connectionString;
        public CrudService(IConfiguration configuration, IWebHostEnvironment hostEnvironment)
        {
            _configuration = configuration;
            _connectionString = _configuration.GetConnectionString("ConnectionString").ToString();
            _hostEnvironment = hostEnvironment;
        }

        public Response Create(Product product)
        {
            Response response = new Response();
            using (IDbConnection connection = new SqlConnection(_connectionString))
            {
                var parameters = new DynamicParameters();
                parameters.Add("@Nombre", product.Nombre);
                parameters.Add("@Precio", product.Precio);
                parameters.Add("@Stock", product.Stock);

                // Ejecutar el procedimiento almacenado
                var result = connection.Execute("AddNewProduct", parameters, commandType: CommandType.StoredProcedure);

                if (result == 0)
                {
                    response.statusCode = 100;
                    response.statusMessage = "Error adding the new product";
                }
                else
                {
                    response.statusCode = 200;
                    response.statusMessage = "Product Added Successfully";
                }
            }
            return response;
        }

        public Response Delete(int productId)
        {
            Response response = new Response();
            using (IDbConnection connection = new SqlConnection(_connectionString))
            {
                // Definir los parámetros para el procedimiento almacenado
                var parameters = new DynamicParameters();
                parameters.Add("@Id", productId);

                // Ejecutar el procedimiento almacenado
                var result = connection.Execute("DeleteProduct", parameters, commandType: CommandType.StoredProcedure);

                if (result == 0)
                {
                    response.statusCode = 400;
                    response.statusMessage = "Error deleting the product";
                }
                else
                {
                    response.statusCode = 200;
                    response.statusMessage = "Product Deleted Successfully";
                }
            }
            return response;
        }

        public Response ReadDataFromDb()
        {

            Response response = new Response();
            using (IDbConnection connection = new SqlConnection(_connectionString))
            {
                // Ejecutar el procedimiento almacenado y obtener los resultados directamente en una lista de Product
                var productsList = connection.Query<Product>("GetProducts", commandType: CommandType.StoredProcedure).ToList();

                if (productsList.Any())
                {
                    response.statusCode = 200;
                    response.statusMessage = "Data found";
                    response.listOfProducts = productsList;
                }
                else
                {
                    response.statusCode = 100;
                    response.statusMessage = "No data found";
                    response.listOfProducts = null;
                }
            }
            return response;
        }

        public Response Update(Product product)
        {
            Response response = new Response();
            using (IDbConnection connection = new SqlConnection(_connectionString))
            {
                // Suponiendo que GetProductById ya está implementado para usar Dapper o cualquier otro mecanismo
                Response oldProductResponse = GetProductById(product.Id, true);
                Product oldProduct = oldProductResponse.product;

                if (string.IsNullOrEmpty(product.Nombre))
                {
                    product.Nombre = oldProduct.Nombre;
                }
                if (product.Precio == 0)
                {
                    product.Precio = oldProduct.Precio;
                }
                if (product.Stock == 0)
                {
                    product.Stock = oldProduct.Stock;
                }

                var parameters = new DynamicParameters();
                parameters.Add("@Nombre", product.Nombre);
                parameters.Add("@Precio", product.Precio);
                parameters.Add("@Stock", product.Stock);
                parameters.Add("@Id", product.Id);

                // Ejecutar el procedimiento almacenado
                var result = connection.Execute("UpdateProduct", parameters, commandType: CommandType.StoredProcedure);

                if (result == 0)
                {
                    response.statusCode = 400;
                    response.statusMessage = "Error updating the product";
                }
                else
                {
                    response.statusCode = 200;
                    response.statusMessage = "Product Updated Successfully";
                }
            }
            return response;
        }

        public Response GetProductById(int id, bool isImagePath = false)
        {
            Response response = new Response();
            using (IDbConnection connection = new SqlConnection(_connectionString))
            {
                // Definir los parámetros para el procedimiento almacenado
                var parameters = new DynamicParameters();
                parameters.Add("@Id", id, DbType.Int32);

                // Ejecutar el procedimiento almacenado y obtener el resultado directamente en un objeto Product
                var product = connection.QueryFirstOrDefault<Product>("GetProductById", parameters, commandType: CommandType.StoredProcedure);

                if (product != null)
                {
                    response.statusCode = 200;
                    response.statusMessage = "Data found";
                    response.product = product;
                }
                else
                {
                    response.statusCode = 400;
                    response.statusMessage = $"{id} data not found";
                    response.product = null;
                }
            }
            return response;
        }
        public Response DeleteCartProduct(int Id)
        {
            Response response = new Response();
            using (IDbConnection connection = new SqlConnection(_connectionString))
            {
                // Definir los parámetros para el procedimiento almacenado
                var parameters = new DynamicParameters();
                parameters.Add("@Id", Id, DbType.Int32);

                // Ejecutar el procedimiento almacenado
                var result = connection.Execute("DeleteCartProductById", parameters, commandType: CommandType.StoredProcedure);

                if (result == 0)
                {
                    response.statusCode = 400;
                    response.statusMessage = "Error deleting the product";
                }
                else
                {
                    response.statusCode = 200;
                    response.statusMessage = "Product Deleted Successfully";
                    // Asumiendo que ListOfCartProducts ya está adaptado para usar Dapper
                    response.listOfProducts = ListOfCartProducts().listOfProducts;
                }
            }
            return response;

        }
        public Response ListOfCartProducts()
        {
            Response response = new Response();
            using (IDbConnection connection = new SqlConnection(_connectionString))
            {
                // Ejecutar el procedimiento almacenado y mapear los resultados a una lista de Product
                var productsList = connection.Query<Product>("GetCartItems", commandType: CommandType.StoredProcedure).ToList();

                if (productsList.Any())
                {
                    response.statusCode = 200;
                    response.statusMessage = "Data found";
                    response.listOfProducts = productsList;
                }
                else
                {
                    response.statusCode = 100;
                    response.statusMessage = "No data found";
                    response.listOfProducts = null;
                }
            }
            return response;
        }
                
        public Response AddToCart(Product product)
        {
            Response response = new Response();

            if (product.Id != null)
            {
                using (IDbConnection connection = new SqlConnection(_connectionString))
                {
                    // Obtener la lista de productos en el carrito (asumiendo que ListOfCartProducts ya está adaptado para usar Dapper)
                    var cartProductIds = ListOfCartProducts().listOfProducts?.Select(u => u.Id);

                    if (cartProductIds != null && cartProductIds.Contains(product.Id))
                    {
                        response.statusCode = 100;
                        response.statusMessage = "Product already exists in cart";
                    }
                    else
                    {
                        // Definir los parámetros para la consulta
                        var parameters = new DynamicParameters();
                        parameters.Add("@ProductId", product.Id, DbType.Int32);

                        // Ejecutar la consulta para añadir el producto al carrito
                        var result = connection.Execute("INSERT INTO CART(ProductId) VALUES(@ProductId)", parameters);

                        if (result == 0)
                        {
                            response.statusCode = 100;
                            response.statusMessage = "Error adding the items to cart";
                        }
                        else
                        {
                            response.statusCode = 200;
                            response.statusMessage = "Product Added to cart successfully";
                        }
                    }
                }
            }
            else
            {
                response.statusCode = 100;
                response.statusMessage = "Product ID must be greater than 0";
            }
            return response;

        }

    }
}
