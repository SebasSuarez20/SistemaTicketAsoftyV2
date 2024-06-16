CREATE DATABASE sistematickets;

USE sistematickets;

CREATE TABLE Users(
     Idcontrol INT NOT NULL AUTO_INCREMENT,
     NameSupport VARCHAR(50) NOT NULL,
     Surname VARCHAR(50) NOT NULL,
     Identification VARCHAR(50) NOT NULL,
     nameUser VARCHAR(50) NOT NULL,
     Password VARCHAR(50) NOT NULL,
     Email VARCHAR(80) NOT NULL,
     RegistrationDate DATETIME DEFAULT CURRENT_TIMESTAMP,
     PhotoPerfil VARCHAR(255) NULL,
	 Enabled BOOl NOT NULL DEFAULT TRUE,
     RoleCode INT NOT NULL,
     hasConnection VARCHAR(50) NULL,
     Username INT NOT NULL,
     PRIMARY KEY(Idcontrol)
);


CREATE TABLE ticketssupport (
    Idcontrol INT NOT NULL AUTO_INCREMENT,
    Consecutive INT NOT NULL UNIQUE,
    Title VARCHAR(255) NOT NULL,
    Description TEXT NOT NULL,
    Aerea VARCHAR(20) NOT NULL,
    Status ENUM("Open","InProgres","Result","Close") NOT NULL,
    Priority ENUM('S', 'M', 'L') NOT NULL,
    PhotoDescription VARCHAR(255) NULL,
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



CREATE TABLE ticketHasMapping (
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

  