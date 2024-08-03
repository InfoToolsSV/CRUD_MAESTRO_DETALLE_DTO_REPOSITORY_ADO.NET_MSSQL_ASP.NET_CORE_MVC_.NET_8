CREATE DATABASE ProductSalesDB

USE ProductSalesDB

CREATE TABLE Products
(
    ProductID INT IDENTITY(1,1) PRIMARY KEY,
    Name NVARCHAR(100) NOT NULL,
    Price DECIMAL(18,2) NOT NULL
)

CREATE TABLE Sales
(
    SaleID INT IDENTITY(1,1) PRIMARY KEY,
    Date DATETIME NOT NULL,
    Total DECIMAL(18,2) NOT NULL
)

CREATE TABLE SaleDetails
(
    SaleDetailsID INT IDENTITY(1,1) PRIMARY KEY,
    SaleID INT NOT NULL,
    ProductID INT NOT NULL,
    Quantity INT NOT NULL,
    Price DECIMAL(18,2) NOT NULL,
    FOREIGN KEY(SaleID) REFERENCES Sales(SaleID),
    FOREIGN KEY(ProductID) REFERENCES Products(ProductID)
)

-- PROCEDIMIENTOS ALMACENADOS PARA PRODUCTOS

CREATE PROCEDURE sp_InsertProduct
    @Name NVARCHAR(100),
    @Price DECIMAL(18,2)
AS
BEGIN
    INSERT INTO Products
        (Name, Price)
    VALUES
        (@Name, @Price)
END



CREATE PROCEDURE sp_DeleteProduct
    @ProductID INT
AS
BEGIN
    DELETE FROM Products WHERE ProductID = @ProductID
END


CREATE PROCEDURE sp_GetAllProducts
AS
BEGIN
    SELECT ProductID, Name, Price
    FROM Products
END

CREATE PROCEDURE sp_GetProductByID
    @ProductID INT
AS
BEGIN
    SELECT ProductID, Name, Price
    FROM Products
    WHERE ProductID = @ProductID

END

CREATE PROCEDURE sp_UpdateProduct
    @ProductID INT,
    @Name NVARCHAR(100),
    @Price DECIMAL(18,2)
AS
BEGIN
    UPDATE Products SET Name = @Name, Price = @Price WHERE ProductID = @ProductID
END


-- PROCEDIMIENTOS ALMACENADOS PARA SALES


CREATE TYPE SaleDetailsType AS TABLE
(
    ProductID INT,
    Quantity INT,
    Price Decimal(18,2)
)

CREATE PROCEDURE sp_InsertSale
    @Date DATETIME,
    @Total DECIMAL(18,2),
    @SaleDetails SaleDetailsType READONLY

AS
BEGIN

    INSERT INTO Sales
        (Date, Total)
    VALUES
        (@Date, @Total)

    DECLARE @SaleID INT = SCOPE_IDENTITY();

    INSERT INTO SaleDetails
        (SaleID, ProductID, Quantity, Price)
    SELECT @SaleID, ProductID, Quantity, Price
    FROM @SaleDetails;

    SELECT @SaleID;

END


CREATE PROCEDURE sp_GetProductPrice
    @ProductID INT
AS
BEGIN
    SELECT Price
    FROM Products
    WHERE ProductID = @ProductID

END

CREATE PROCEDURE sp_DeleteSale
    @SaleID INT
AS
BEGIN
    DELETE FROM SaleDetails WHERE SaleID = @SaleID
    DELETE FROM Sales WHERE SaleID = @SaleID

END

CREATE PROCEDURE sp_GetAllSales
AS
BEGIN
    SELECT SaleID, Date, Total
    FROM Sales;
    SELECT SaleDetailsID, SaleID, ProductID, Quantity, Price
    FROM SaleDetails
END


CREATE PROCEDURE sp_GetSaleByID
    @SaleID INT
AS
BEGIN
    SELECT SaleID, Date, Total
    FROM Sales
    WHERE SaleID = @SaleID;
    SELECT SaleDetailsID, SaleID, ProductID, Quantity, Price
    FROM SaleDetails
    WHERE SaleID = @SaleID;
END

CREATE PROCEDURE sp_DeleteSaleDetails
    @SaleID INT
AS
BEGIN
    DELETE FROM SaleDetails WHERE SaleID = @SaleID

END


CREATE PROCEDURE sp_InsertSaleDetail
    @SaleID INT,
    @ProductID INT,
    @Quantity INT,
    @Price DECIMAL(18,2)

AS
BEGIN
    INSERT INTO SaleDetails
        (SaleID, ProductID, Quantity, Price)
    VALUES
        (@SaleID, @ProductID, @Quantity, @Price)
END


CREATE PROCEDURE sp_UpdateSale
    @SaleID INT,
    @Date DATETIME,
    @Total DECIMAL(18,2)
AS
BEGIN

    UPDATE  Sales SET Date = @Date, Total =@Total WHERE SaleID = @SaleID

END