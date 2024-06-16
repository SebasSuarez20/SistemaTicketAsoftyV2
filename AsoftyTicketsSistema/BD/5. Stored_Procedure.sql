DELIMITER //
CREATE PROCEDURE CodeGenUserAll()
BEGIN
     SELECT us.Idcontrol AS Code,CONCAT(us.NameSupport,' ',us.Surname) AS Name 
     FROM users us WHERE us.RoleCode = 2 
     AND us.Enabled = true; 
END //
DELIMITER ;

DELIMITER //
CREATE PROCEDURE GetChatsForConsecutive(paramsConsecutive INT)
BEGIN
	SELECT 
   tks.Idcontrol AS Idcontrol,
   tks.Consecutive AS Consecutive,
   tks.Title AS Title,
   tks.Description AS Description,
   tks.Status AS Status,
   tks.PhotoDescription AS PhotoTickets,
   CONCAT(u.NameSupport,' ',u.Surname) AS NameSupport,
   CONCAT(us.NameSupport,' ',us.Surname) AS NameCompany,
   thm.HasUnique AS HasUnique,
   thm.Message AS Message,
   thm.RegistrationDate AS DateChat,
   tks.Enabled AS Enabled,
   thm.Username AS Username,
   (CASE usP.RoleCode WHEN 2 THEN "Left"
	ELSE "Rigth" END ) AS Position
   FROM (
   SELECT * FROM ticketssupport tks
   WHERE tks.Enabled = TRUE AND tks.AssignedTo IS NOT NULL AND tks.Consecutive = paramsConsecutive
   ORDER BY tks.Idcontrol ASC
   LIMIT 1
   ) AS tks
   LEFT JOIN tickethasmapping thm ON tks.Consecutive = thm.Consecutive
   LEFT JOIN users usP ON usP.Idcontrol = thm.Username AND usP.Enabled = true
   LEFT JOIN users u ON u.Idcontrol = tks.AssignedTo AND u.Enabled = true
   LEFT JOIN users us ON us.Idcontrol = tks.Username AND us.Enabled = true;
END //
DELIMITER ;


