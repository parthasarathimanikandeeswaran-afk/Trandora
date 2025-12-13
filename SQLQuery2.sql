select * from Users 
CREATE TABLE Categories (
    CategoryId INT IDENTITY(1,1) PRIMARY KEY,
    Name VARCHAR(100) NOT NULL
);



INSERT INTO Categories (Name)
VALUES 
('Watches'),
('Headphones'),
('Sunglasses'),
('Backpacks'),
('Shoes'),
('Jackets'),
('Accessories'),
('Smart Devices');


select * from CartItems



ALTER TABLE Carts
ADD CreatedAt DATETIME DEFAULT GETDATE();

ALTER TABLE Carts
DROP COLUMN CreatedAt;

select * from Carts

ALTER TABLE Carts
ADD CONSTRAINT DF_Carts_CreatedAt DEFAULT GETDATE() FOR CreatedAt;

