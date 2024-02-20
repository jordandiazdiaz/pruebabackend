namespace ReactWithCore.Server.Models
{
    public class Product
    {
        public int Id { get; set; }
        public string Nombre { get; set; }
        public int Stock { get; set; }
        public decimal Precio { get; set; }

        public DateTime FechaRegistro { get; set; }

    }
}
