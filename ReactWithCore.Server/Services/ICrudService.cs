using ReactWithCore.Server.Models;

namespace ReactWithCore.Server.Services
{
    public interface ICrudService
    {
        Response ReadDataFromDb();
        Response Create(Product product);

        Response GetProductById(int id, bool isImagePath = false);

        Response Update(Product product);

        Response Delete(int productId);

        Response DeleteCartProduct(int Id);

        Response ListOfCartProducts();
    

        Response AddToCart(Product product);
    }

}
