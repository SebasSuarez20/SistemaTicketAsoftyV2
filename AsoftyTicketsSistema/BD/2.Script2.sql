
use sistematickets;

INSERT INTO Users (NameSupport, Surname, Identification, Username, Password, Email, Enabled, RoleCode) 
VALUES ('Miguel', 'Moya', '123456789', 'DevJr4', '12345', 'miguel@example.com',1,1);

INSERT INTO Users (NameSupport, Surname, Identification, Username, Password, Email, Enabled, RoleCode) 
VALUES ('Juan', 'Sebastian', '123456789', 'DevJr2', '12345', 'miguel@example.com',1,2);

INSERT INTO Users (NameSupport, Surname, Identification, Username, Password, Email, Enabled, RoleCode) 
VALUES ('Carolina', 'Cubillos', '44444444', 'Caro2024', '12345', 'carolina@example.com',1,3);

INSERT INTO ticketssupport (Consecutive,Title, Aerea,  Description, Status, Priority, PhotoDescription, AssignedTo, Enabled,Username)
VALUES (1,'Título del ticket','Aerea',  'Descripción del ticket', 'Open', 'S', NULL,NULL,1,1);
