CREATE TABLE Product
(
    ProductId INT NOT NULL PRIMARY KEY IDENTITY(1,1),
    Code VARCHAR(50) NOT NULL UNIQUE,
    Price DECIMAL NOT NULL,
    Stock INT NOT NULL
);

CREATE TABLE OrderHeader
(
    OrderId INT NOT NULL PRIMARY KEY IDENTITY(1,1),
    [Address] VARCHAR(MAX) NOT NULL,
    Total DECIMAL NOT NULL
);

CREATE TABLE OrderLine
(
    OrderLineId INT NOT NULL PRIMARY KEY IDENTITY(1,1),
    ProductId INT NOT NULL FOREIGN KEY REFERENCES Product(ProductId),
    OrderId INT NOT NULL FOREIGN KEY REFERENCES OrderHeader(OrderId),
    --trebuie sa facem referire si la comanda mare
    Quantity INT NOT NULL,
    Price DECIMAL NOT NULL
);

