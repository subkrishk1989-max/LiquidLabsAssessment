CREATE DATABASE ProductDb;
GO

USE ProductDb;
GO

CREATE TABLE Products
(
    Id INT PRIMARY KEY,
    Name NVARCHAR(500) NOT NULL
);
GO

CREATE TABLE ProductAttributes
(
    Id INT IDENTITY(1,1) PRIMARY KEY,

    ProductId INT NOT NULL,

    AttributeName NVARCHAR(200) NOT NULL,

    AttributeValue NVARCHAR(MAX) NULL,

    CONSTRAINT FK_ProductAttributes_Products
        FOREIGN KEY (ProductId)
        REFERENCES Products(Id)
        ON DELETE CASCADE
);
GO