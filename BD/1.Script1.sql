CREATE DATABASE sistematickets;

USE sistematickets;

CREATE TABLE Users(
     Idcontrol INT NOT NULL AUTO_INCREMENT,
     Identification VARCHAR(50) NOT NULL COMMENT 'Numero de Identificacion',
     nameUser VARCHAR(50) NOT NULL COMMENT 'Nombre de Usuario',
     Password VARCHAR(50) NOT NULL COMMENT 'Contraseña',
     RegistrationDate DATETIME DEFAULT CURRENT_TIMESTAMP COMMENT 'Fecha de registro',
     PhotoPerfil VARCHAR(255) NULL COMMENT 'Foto de Perfil',
     themeColor BOOL NOT NULL DEFAULT FALSE COMMENT 'Tema del Aplicativo',
	 Enabled BOOl NOT NULL DEFAULT TRUE,
     RoleCode INT NOT NULL COMMENT 'Código de rol',
     hasConnection VARCHAR(50) NULL,
     Username INT NOT NULL,
     INDEX(Identification),
     PRIMARY KEY(Idcontrol)
);


CREATE TABLE ticketssupport (
    Idcontrol INT NOT NULL AUTO_INCREMENT,
    Consecutive INT NOT NULL UNIQUE,
    Date Date NOT NULL,
    Title VARCHAR(255) NOT NULL,
    Description TEXT NOT NULL,
    Area ENUM('1','2','3','4') NOT NULL,
    Status ENUM('1','2','3','4') NOT NULL COMMENT "Open,InProgress,Result,Close",
    Priority ENUM('1', '2', '3') NOT NULL COMMENT "S,M,L",
    PhotoDescription VARCHAR(255) NULL,
    softwareApplication INT NULL,
    codeConnectionSoftware VARCHAR(30) NULL,
    AssignedTo INT NULL,
	RegistrationDate DATETIME DEFAULT CURRENT_TIMESTAMP,
    Enabled BOOL NOT NULL,
	Username INT NOT NULL,
    PRIMARY KEY(Idcontrol),
    INDEX(AssignedTo),
    FOREIGN KEY (AssignedTo) REFERENCES Users(Idcontrol)
);

-- Añadir índice a la columna Status
ALTER TABLE ticketssupport
ADD INDEX idx_status (Status);

-- Añadir índice a la columna Priority
ALTER TABLE ticketssupport
ADD INDEX idx_priority (Priority);

CREATE TABLE chatOfMapping (
    Idcontrol INT NOT NULL AUTO_INCREMENT,
    Consecutive INT NOT NULL,
    HasUnique VARCHAR(35) NOT NULL,
    Message TEXT NOT NULL,
	RegistrationDate DATETIME DEFAULT CURRENT_TIMESTAMP,
    Enabled BOOL NOT NULL,
	Username INT NOT NULL,
    PRIMARY KEY(Idcontrol),
    INDEX(Username,Consecutive),
    FOREIGN KEY (Username) REFERENCES Users(Idcontrol),
    FOREIGN KEY (Consecutive) REFERENCES ticketssupport(Consecutive)
);

CREATE TABLE informationUser (
    Idcontrol INT NOT NULL AUTO_INCREMENT,
	NameSupport VARCHAR(50) NOT NULL COMMENT 'Nombre',
    Surname VARCHAR(50) NOT NULL COMMENT 'Apellido',
--     FirstName1 VARCHAR(50) NOT NULL,
--     SecondName1 VARCHAR(50) NULL,
--     FirstSurname1 VARCHAR(50) NOT NULL,
--     SecondSurname1 VARCHAR(50) NULL,
    Gender INT NOT NULL COMMENT 'Género',
    TypeIdentification INT NOT NULL COMMENT 'Tipo de Identificacion',
    Identification VARCHAR(50) NOT NULL COMMENT 'Identificacion',
    BloodType VARCHAR(20) NULL COMMENT 'Grupo sanguíneo',
    Country INT NOT NULL COMMENT 'País',
    City INT NOT NULL COMMENT 'Ciudad',
    Address VARCHAR(100) NOT NULL COMMENT 'Dirección',
    Phone INT NOT NULL COMMENT 'Telefono',
    -- EmailAddress VARCHAR(100) NOT NULL COMMENT 'Dirección de correo electrónico',
    BirthDate DATE COMMENT 'Fecha de Nacimiento',
    EmergencyContact INT NOT NULL COMMENT 'Contacto de Emergencia',
    Parentage VARCHAR(50) NOT NULL COMMENT 'Parentesco',
    FirstName VARCHAR(50) NOT NULL COMMENT 'Nombre del Contacto de Emergencia',
   --  SecondName2 VARCHAR(50) NULL COMMENT '',
    FirstSurname VARCHAR(50) NOT NULL COMMENT 'Apellido del Contacto de Emergencia',
--     SecondSurname2 VARCHAR(50) NULL COMMENT '',
    Department INT NOT NULL COMMENT 'Departamento',
    Email VARCHAR(80) NOT NULL COMMENT 'Dirección de correo electrónico',
    Enabled BOOL NOT NULL,
    Username INT NOT NULL ,
    PRIMARY KEY (Idcontrol),
    INDEX(Identification),
    FOREIGN KEY (Identification) REFERENCES Users(Identification) 
);

