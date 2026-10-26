IF DB_ID(N'FirmaSecuritate') IS NULL
BEGIN
    CREATE DATABASE [FirmaSecuritate];
END;
GO

USE [FirmaSecuritate];
GO

IF OBJECT_ID(N'dbo.Agenti_de_Paza', N'U') IS NULL
BEGIN
    CREATE TABLE dbo.Agenti_de_Paza
    (
        ID_Agent INT IDENTITY(1,1) NOT NULL
            CONSTRAINT PK_Agenti_de_Paza PRIMARY KEY,
        Nume_Complet NVARCHAR(150) NOT NULL,
        Telefon NVARCHAR(30) NOT NULL,
        Salariu DECIMAL(10,2) NOT NULL,
        CONSTRAINT CK_Agenti_de_Paza_Salariu CHECK (Salariu >= 0)
    );
END;
GO
