USE GimnasioDB;
GO

CREATE TABLE Usuarios (
    Id        INT IDENTITY(1,1) PRIMARY KEY,
    Correo    NVARCHAR(150) NOT NULL,
    ClaveHash NVARCHAR(200) NOT NULL,
    Rol       NVARCHAR(30)  NOT NULL,
    CONSTRAINT UQ_Usuarios_Correo UNIQUE (Correo)
);
GO

INSERT INTO Usuarios (Correo, ClaveHash, Rol) VALUES
('recepcion@gimnasio.com', '$2a$11$BSVK5A/a3TABxKqiS//qguvqgYoJ0EyblDg594Y/Cfg4fyREw34mC', 'Recepcionista'),
('entrenador@gimnasio.com', '$2a$11$K.gErKdbNSxRGK.SXOW7a.uknAmw35PjIVTDREEgDDu5xnOH.Ofxm', 'Entrenador');
GO
