CREATE DATABASE combine;
GO

USE combine;
GO

CREATE TABLE Customers
(
id NVARCHAR(20) PRIMARY KEY,
name NVARCHAR(100),
inn NVARCHAR(50),
addres NVARCHAR(100),
phone NVARCHAR(12),
salesman BIT,
buyer BIT
);

CREATE TABLE Users
(
id INT IDENTITY(1,1) PRIMARY KEY,
login NVARCHAR(25),
password NVARCHAR(16),
manufacturer_id NVARCHAR(20),
isActive BIT,
Role NVARCHAR(10),

CONSTRAINT FK_Users_Customers
    FOREIGN KEY (manufacturer_id)
    REFERENCES Customers(id)

);

CREATE TABLE Components
(
id INT IDENTITY(1,1) PRIMARY KEY,
component_name NVARCHAR(50),
code NVARCHAR(50),
price DECIMAL(10,2),
type NVARCHAR(50)
);

CREATE TABLE Products
(
id INT IDENTITY(1,1) PRIMARY KEY,
product_name NVARCHAR(50),
code NVARCHAR(50),
price DECIMAL(10,2)
);

CREATE TABLE ProductsComponents
(
product_id INT,
component_id INT,
components_quentity DECIMAL(10,2),
PRIMARY KEY(product_id, component_id),

CONSTRAINT FK_ProductsComponents_Products
    FOREIGN KEY(product_id)
    REFERENCES Products(id),

CONSTRAINT FK_ProductsComponents_Components
    FOREIGN KEY(component_id)
    REFERENCES Components(id)

);

CREATE TABLE Orders
(
id INT IDENTITY(1,1) PRIMARY KEY,
order_id INT,
product_id INT,
buyer_id NVARCHAR(20),
quentity INT,

CONSTRAINT FK_Orders_Products
    FOREIGN KEY(product_id)
    REFERENCES Products(id),

CONSTRAINT FK_Orders_Customers
    FOREIGN KEY(buyer_id)
    REFERENCES Customers(id)

);

INSERT INTO Customers
VALUES
(N'000000001', N'ООО "Поставка"', N'', N'г.Пятигорск', N'+79198634592', 1, 1),
(N'000000002', N'ООО "Кинотеатр Квант"', N'26320045123', N'г. Железноводск, ул. Мира, 123', N'+79884581555', 1, 0),
(N'000000003', N'ООО "Ромашка"', N'4140784214', N'г. Омск, ул. Строителей, 294', N'+79882584546', 0, 1),
(N'000000008', N'ООО "Новый JDTO"', N'26320045111', N'г. Железноводск', N'+79884581555', 1, 0),
(N'000000009', N'ООО "Ипподром"', N'5874045632', N'г. Уфа, ул. Набережная, 37', N'+79627486389', 1, 1),
(N'000000010', N'ООО "Ассоль"', N'2629011278', N'г. Калуга, ул. Пушкина, 94', N'+79184572398', 0, 1);

INSERT INTO Users
(login, password, manufacturer_id, isActive, Role)
VALUES
('user1', 'user1', '000000003', 1, 'client'),
('admin1', 'admin1', NULL, 1, 'admin');

INSERT INTO Components
(component_name, code, price, type)
VALUES
(N'Молоко нормализованное', N'НФ-00000004', 10.00, N'кг'),
(N'Закваска сметанная', N'НФ-00000005', 40.00, N'кг');

INSERT INTO Products
(product_name, code, price)
VALUES
(N'Кефир 2,5% 900г.', N'НФ-00000007', 80.00),
(N'Кефир 3,2% 900г.', N'НФ-00000008', 82.00),
(N'Молоко 2,5% 900г.', N'НФ-00000009', 79.00),
(N'Сметана классическая 15% 540г.', N'НФ-00000006', 89.00);

INSERT INTO ProductsComponents
VALUES
(4, 1, 0.90),
(4, 2, 0.07);

INSERT INTO Orders
(order_id, product_id, buyer_id, quentity)
VALUES
(1, 1, N'000000002', 12),
(1, 2, N'000000002', 9),
(1, 3, N'000000002', 10);
