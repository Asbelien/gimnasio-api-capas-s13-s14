IF DB_ID('GimnasioDB') IS NULL
    CREATE DATABASE GimnasioDB;
GO

USE GimnasioDB;
GO

CREATE TABLE Socios (
    Id INT IDENTITY(1,1) PRIMARY KEY,
    Dni CHAR(8) NOT NULL UNIQUE,
    Nombre NVARCHAR(100) NOT NULL,
    Telefono NVARCHAR(15) NULL,
    Email NVARCHAR(100) NULL
);

CREATE TABLE Membresias (
    Id INT IDENTITY(1,1) PRIMARY KEY,
    TipoPlan NVARCHAR(30) NOT NULL,
    Precio DECIMAL(8,2) NOT NULL,
    FechaInicio DATE NOT NULL,
    FechaFin DATE NOT NULL,
    SocioId INT NOT NULL,
    CONSTRAINT FK_Membresias_Socios FOREIGN KEY (SocioId) REFERENCES Socios(Id)
);

INSERT INTO Socios (Dni, Nombre, Telefono, Email) VALUES
('70123456', 'Andrea Flores', '987654321', 'andrea@mail.com'),
('71234567', 'Jorge Salazar', '976543210', 'jorge@mail.com'),
('72345678', 'Camila Rojas', '965432109', NULL);

INSERT INTO Membresias (TipoPlan, Precio, FechaInicio, FechaFin, SocioId) VALUES
('Mensual', 90.00, '2026-10-01', '2026-10-31', 1),
('Trimestral', 240.00, '2026-11-01', '2027-01-31', 1),
('Mensual', 90.00, '2026-10-15', '2026-11-14', 2),
('Anual', 800.00, '2026-09-01', '2027-08-31', 3);
GO
