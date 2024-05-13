DELIMITER //
CREATE PROCEDURE CodeGenUserAll()
BEGIN
     SELECT us.Idcontrol AS Code,CONCAT(us.NameSupport,' ',us.Surname) AS Name 
     FROM users us WHERE us.RoleCode = 2 
     AND us.Enabled = true; 
END //
DELIMITER ;
