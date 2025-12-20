create database PharmaPro;

use PharmaPro;

create table Products (
    ProductID int primary key identity(1,1),
    ProductName varchar(30) not null,
    Description TEXT,
    Price DECIMAL(8,2) not null,
    Category VARCHAR(20),
    CreatedAt datetime default getdate(),
    UpdatedAt datetime default getdate()
);

create table Inventory (
    InventoryID int primary key identity(1,1),
    ProductID int, foreign key (ProductID) references Products(ProductID),
    Location varchar(25),
    Available int not null default 0,
    LastUpdated datetime default getdate()
);

create table UserAccounts (
    UserID int primary key identity(1,1),
    Username varchar(50) unique not null,
    PasswordHash varchar(100) not null,
    Email varchar(35) unique not null,
    FullName varchar(30),
    Address TEXT,
    PhoneNumber varchar(20),
    UserType varchar(11) check (UserType in ('Customer', 'Admin')),
    CreatedAt datetime default getdate()
);

select * from UserAccounts;


create table Orders (
    OrderID int primary key identity(1,1),
    UserID int, foreign key(UserID) references UserAccounts(UserID),
    OrderDate datetime default getdate(),
    TotalAmount decimal(8,2) not null,
    PaymentStatus varchar(10) check (PaymentStatus in ('Pending', 'Paid', 'Cancelled'))
);

create table OrderDetails (
    OrderDetailID int primary key identity(1,1),
    OrderID int, foreign key (OrderID) references Orders(OrderID),
    ProductID int, foreign key (ProductID) references Products(ProductID),
    Quantity int not null,
    Price decimal(8,2) not null
);

create table Suppliers (
    SupplierID int primary key identity(1,1),
    SupplierName varchar(25) not null,
    ContactPerson varchar(25),
    PhoneNumber varchar(20),
    Email varchar(35) unique not null,
    Address text
);

create table Prescriptions (
    PrescriptionID int primary key identity(1,1),
    UserID int, foreign key (UserID) references UserAccounts(UserID),
    DoctorName varchar(35) not null,
    PrescriptionDate datetime default getdate(),
    Diagnosis text
);

create table PrescriptionDetails (
    PrescriptionDetailID int primary key identity(1,1),
    PrescriptionID int, foreign key (PrescriptionID) references Prescriptions(PrescriptionID),
    ProductID int, foreign key(ProductID) references Products(ProductID),
    Dosage varchar(20),
    Duration varchar(20)
);

create table Billing (
    BillID int primary key identity(1,1),
    OrderID int, foreign key (OrderID) references Orders(OrderID),
    PaymentMethod varchar(12) check (PaymentMethod in ('Cash', 'Credit Card', 'Debit Card', 'Online')),
    PaymentDate datetime default getdate(),
    AmountPaid decimal(8,2) not null
);

create index idx_product_name on Products(ProductName);
create index idx_email on UserAccounts(Email);
create index idx_order_date on Orders(OrderDate);
create index idx_supplier_name on Suppliers(SupplierName);

INSERT INTO UserAccounts (Username, PasswordHash, Email, FullName, Address, PhoneNumber, UserType)
VALUES 
('Hamza', '12345', 'S.Hamza@gmail.com', 'Hamza Afzaal', 'House 23, G-10/4, Karachi', '0300-1234567', 'Admin'),
('Abdullah', '11223344', 'Abdullah@gmail.com', 'Abdullah', 'Flat 9-B, Lahore', '0345-8899001', 'Admin');


INSERT INTO Products (ProductName, Description, Price, Category)
VALUES 
('Paracetamol', 'Effective for reducing fever and relieving pain.', 350, 'Medicine'),
('Vitamin C Tablets', 'Boosts immunity and skin health.', 625, 'Supplements'),
('Cough Syrup', 'Soothes sore throat and relieves cough symptoms.', 790, 'Syrup'),
('Digital Thermometer', 'Measures body temperature accurately and quickly.', 1500, 'Equipment'),
('Bandage Roll', 'Sterile cotton roll for wound dressing and support.', 210, 'First Aid'),
('Hand Sanitizer', 'Kills 99.9% of germs without water.', 450, 'Hygiene'),
('Aspirin', 'Used as a painkiller and blood thinner.', 575, 'Medicine'),
('Glucose Powder', 'Instant energy booster for fatigue and dehydration.', 320, 'Supplements'),
('Face Mask (Pack of 50)', 'Disposable masks for protection against airborne particles.', 1200, 'Hygiene'),
('Insulin Injection', 'Used for diabetes management under prescription.', 2500, 'Medicine');




SELECT ProductID, ProductName, Price FROM Products WHERE ProductID = 1;


select * from UserAccounts;

INSERT INTO UserAccounts (Username, PasswordHash, Email, FullName, Address, PhoneNumber, UserType)
VALUES ('ali', 'password123', 'ali@gmail.com', 'Ali Khan', 'Karachi', '0300-5555555', 'Customer');


INSERT INTO Orders (UserID, OrderDate, TotalAmount)
VALUES (1006, GETDATE(), 500.00);

DECLARE @OrderId INT = SCOPE_IDENTITY();

-- Example: Paracetamol has a unit price of 250.00
INSERT INTO OrderDetails (OrderID, ProductID, Quantity, Price)
VALUES (19, 1, 2, 250.00);

SELECT * FROM Orders ORDER BY OrderDate DESC;

SELECT o.OrderID, u.FullName, u.UserType
FROM Orders o
JOIN UserAccounts u ON o.UserID = u.UserID
ORDER BY o.OrderDate DESC;

