CREATE DATABASE sistematickets;

USE sistematickets;

CREATE TABLE Users(
     Idcontrol INT NOT NULL AUTO_INCREMENT,
     NameSupport VARCHAR(50) NOT NULL,
     Surname VARCHAR(50) NOT NULL,
     Identification VARCHAR(50) NOT NULL,
     Username VARCHAR(50) NOT NULL,
     Password VARCHAR(50) NOT NULL,
     Email VARCHAR(80) NOT NULL,
     RegistrationDate DATETIME DEFAULT CURRENT_TIMESTAMP,
     PhotoPerfil VARCHAR(255) NULL,
     Enabled BOOL NOT NULL,
     RoleCode INT NOT NULL,
     hasConnection VARCHAR(50) NULL,
     PRIMARY KEY(Idcontrol)
);


CREATE TABLE ticketssupport (
    Idcontrol INT NOT NULL AUTO_INCREMENT,
    Consecutive INT NOT NULL UNIQUE,
    Title VARCHAR(255) NOT NULL,
    Description TEXT NOT NULL,
    Aerea VARCHAR(20) NOT NULL,
    Status ENUM('Open', 'InProgress', 'Result', 'Close') NOT NULL,
    Priority ENUM('S', 'M', 'L') NOT NULL,
    PhotoDescription VARCHAR(255) NULL,
    AssignedTo INT NULL,
    Date_Update DATETIME NULL,
	RegistrationDate DATETIME DEFAULT CURRENT_TIMESTAMP,
    Enabled BOOL NOT NULL,
	Username INT NOT NULL,
    PRIMARY KEY(Idcontrol),
    INDEX(AssignedTo)
);


CREATE TABLE ticketHasMapping (
    Idcontrol INT NOT NULL AUTO_INCREMENT,
    Consecutive INT NOT NULL,
    HasUnique VARCHAR(35) NOT NULL,
    Message TEXT NOT NULL,
	RegistrationDate DATETIME DEFAULT CURRENT_TIMESTAMP,
    Enabled BOOL NOT NULL,
	Username INT NOT NULL,
    PRIMARY KEY(Idcontrol),
    INDEX(Username),
    FOREIGN KEY (Username) REFERENCES Users(Idcontrol)
);

  