--USE DATABASE ShoppingCartReact

CREATE TABLE Products(Id INT PRIMARY KEY IDENTITY(1,1), Nombre VARCHAR(100), Precio DECIMAL(18,2), Stock INT, FechaRegistro DATETIME);

SELECT * FROM Products

INSERT INTO Products(Nombre, Precio, Stock, FechaRegistro) VALUES('Test1', 300, 12, GETDATE())
INSERT INTO Products(Nombre, Precio, Stock, FechaRegistro) VALUES('Test2', 300, 12, GETDATE())
INSERT INTO Products(Nombre, Precio, Stock, FechaRegistro) VALUES('Test3', 300, 12, GETDATE())
INSERT INTO Products(Nombre, Precio, Stock, FechaRegistro) VALUES('Test4', 300, 12, GETDATE())
INSERT INTO Products(Nombre, Precio, Stock, FechaRegistro) VALUES('Test5', 300, 12, GETDATE())

CREATE TABLE CART(Id int IDENTITY(1,1) PRIMARY KEY, ProductId int)

GO

CREATE PROCEDURE GetProducts
AS
    SELECT * FROM Products;
GO

CREATE PROCEDURE GetProductById @id int
AS
    SELECT * FROM Products WHERE Id =@id;
GO

CREATE PROCEDURE AddNewProduct @Nombre VARCHAR(100), @Precio DECIMAL(18,2), @Stock INT
As
  INSERT INTO Products(Nombre, Precio, Stock, FechaRegistro) VALUES (@Nombre, @Precio, @Stock, GETDATE())
GO


CREATE PROCEDURE UpdateProduct @Id int,@Nombre VARCHAR(100), @Precio DECIMAL(18,2), @Stock INT
As
  UPDATE Products SET Nombre = @Nombre, Precio = @Precio, Stock = @Stock, FechaRegistro = GETDATE() WHERE Id=@Id
GO

CREATE PROCEDURE DeleteProduct @Id int
As
  DELETE FROM Products WHERE Id=@Id
GO

CREATE PROCEDURE GetCartItems
As
	SELECT P.Id, P.Nombre, P.Precio, P.Stock, P.FechaRegistro FROM CART C INNER JOIN Products P ON C.ProductId = P.Id
GO

CREATE PROCEDURE DeleteCartProduct @Id int
As
	DELETE FROM CART WHERE Id=@Id
GO

CREATE PROCEDURE DeleteCartProductById @Id int
As
	DELETE FROM CART WHERE ProductId=@Id
GO

