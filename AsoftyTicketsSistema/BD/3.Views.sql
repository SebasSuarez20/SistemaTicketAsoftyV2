
#Creacion de vista para los vinculados al tickets.
CREATE VIEW ticketSupportView
AS
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
   FROM ticketssupport tks
   LEFT JOIN tickethasmapping thm ON tks.Consecutive = thm.Consecutive
   LEFT JOIN users usP ON usP.Idcontrol = thm.Username AND usP.Enabled = true
   LEFT JOIN users u ON u.Idcontrol = tks.AssignedTo AND u.Enabled = true
   INNER JOIN users us ON us.Idcontrol = tks.Username AND us.Enabled = true
   WHERE tks.Enabled = TRUE AND tks.AssignedTo IS NOT NULL;
   
#Creacion de vista para consecutivo de tickets.
CREATE VIEW ConsecTicketView
AS
SELECT 
tick.Idcontrol as id,
tick.Consecutive as consecutive,
tick.Enabled as enabled
FROM sistematickets.ticketssupport tick
WHERE tick.Enabled = false OR tick.Enabled = true
ORDER BY tick.Consecutive DESC LIMIT 1;

#Creacion de vista para el DashBoard 
CREATE VIEW TicketMapAndSupView AS
SELECT 
    ts.Idcontrol, ts.Consecutive, 
	ts.Aerea AS Area,
    ts.Priority AS Priority,
    (CASE ts.Status 
    WHEN "Open" THEN "Abierto"
    WHEN "InProgress" THEN "En Proceso"
    WHEN "Result" THEN "Resueltos"
    WHEN "Close" THEN "Cancelados"
    END) AS Status,
	ts.AssignedTo, 
    thm.HasUnique,
    ts.Enabled,
    ts.Username
FROM ticketssupport ts
LEFT JOIN tickethasmapping thm ON
	ts.Consecutive = thm.Consecutive 
    AND thm.Enabled = true
GROUP BY ts.Consecutive,thm.HasUnique
ORDER BY ts.Consecutive;