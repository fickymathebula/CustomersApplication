/*
-- This script will create a new database Customer_DB and table Customer
*/

-- 1. Create a database first
IF NOT EXISTS(SELECT 1 FROM sys.databases WHERE name='Customer_DB')
   CREATE DATABASE Customer_DB
GO

-- 2. Navigating to the database Customers_DB
USE Customer_DB
GO

-- 3. Creating Customers Table
CREATE TABLE Customer
             (Id          int not null primary key identity,
              Name        varchar(100),
			  DateOfBirth date)
GO

-- 4. Let's add some testing data, at least 10 customers
INSERT Customer (Name, DateOfBirth) VALUES ('John', '1995-03-25')
INSERT Customer (Name, DateOfBirth) VALUES ('Test', '2015-06-12')
INSERT Customer (Name, DateOfBirth) VALUES ('Marry', '2006-04-11')
INSERT Customer (Name, DateOfBirth) VALUES ('October', '1995-03-25')
INSERT Customer (Name, DateOfBirth) VALUES ('Happy', '2006-04-11')
INSERT Customer (Name, DateOfBirth) VALUES ('Look', '1980-03-21')
INSERT Customer (Name, DateOfBirth) VALUES ('Someone', '2001-11-16')
INSERT Customer (Name, DateOfBirth) VALUES ('Ficky', '2015-06-12')
INSERT Customer (Name, DateOfBirth) VALUES ('Hazel', '1988-01-13')
INSERT Customer (Name, DateOfBirth) VALUES ('November', '2000-09-05')

-- 5. Just so we know the database table is created and data is populated
SELECT * FROM Customer
GO