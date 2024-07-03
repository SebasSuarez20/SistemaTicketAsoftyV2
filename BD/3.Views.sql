
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
	ts.Area AS Area,
    ts.Priority AS Priority,
    (CASE ts.Status 
    WHEN 1 THEN "Abierto"
    WHEN 2 THEN "En Proceso"
    END) AS Status,
	ts.AssignedTo, 
    ch.HasUnique,
    ts.Enabled,
    ts.Username
FROM ticketssupport ts
LEFT JOIN chatOfMapping ch ON
	ch.Consecutive  = ts.Consecutive
    AND ch.Enabled = TRUE
WHERE ts.Enabled = TRUE AND (ts.Status<>"Close" AND ts.Status<>"Result")
GROUP BY ts.Consecutive,ch.HasUnique
ORDER BY ts.Consecutive;