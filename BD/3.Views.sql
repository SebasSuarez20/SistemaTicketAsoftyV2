
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
    GetPriorityOfTicket(ts.Priority) AS Priority,
    GetStatusOfTicket(ts.Status) AS Status,
	ts.AssignedTo, 
    ch.HasUnique,
    ts.Enabled,
    ts.Username
FROM ticketssupport ts
LEFT JOIN chatOfMapping ch ON
	ch.Consecutive  = ts.Consecutive
    AND ch.Enabled = TRUE
WHERE ts.Enabled = TRUE AND (ts.Status<>3 AND ts.Status<>4)
GROUP BY ts.Consecutive,ch.HasUnique
ORDER BY ts.Consecutive;