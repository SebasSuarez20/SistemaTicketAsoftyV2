
use sistematickets;

INSERT INTO Users (NameSupport, Surname, Identification, nameUser, Password, Email, Enabled, RoleCode,Username) 
VALUES ('Miguel', 'Moya', '123456789', 'DevJr4', '12345', 'miguel@example.com',1,1,1);

INSERT INTO Users (NameSupport, Surname, Identification, nameUser, Password, Email, Enabled, RoleCode,Username) 
VALUES ('Juan', 'Sebastian', '123456789', 'DevJr2', '12345', 'miguel@example.com',1,2,1);

INSERT INTO Users (NameSupport, Surname, Identification, nameUser, Password, Email, Enabled, RoleCode,Username) 
VALUES ('Carolina', 'Cubillos', '44444444', 'Caro2024', '12345', 'carolina@example.com',1,3,1);

INSERT INTO ticketssupport (Consecutive,Title, Aerea,  Description, Status, Priority, PhotoDescription, AssignedTo, Enabled,Username)
VALUES (1,'Título del ticket','Aerea',  'Descripción del ticket', '1', '1', NULL,NULL,TRUE,3);
