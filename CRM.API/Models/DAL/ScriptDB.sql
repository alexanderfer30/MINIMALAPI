CREATE DATABASE CRMBD 
GO 

USE CRMBD
GO 

CREATE TABLE Customers (

   Id INT IDENTITY (1,1) PRIMARY KEY, 
   Name Varchar(50) not null, 
   LastName varchar(50) not null,
   Address varchar(255)
)
GO