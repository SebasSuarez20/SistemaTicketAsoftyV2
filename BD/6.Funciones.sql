#Funciones

DELIMITER //
CREATE FUNCTION GetStatusOfTicket(Status VARCHAR(5))
RETURNS VARCHAR(20)
DETERMINISTIC
BEGIN
    SET @StatusResult =(
		CASE Status 
		WHEN 1 THEN "Abierto"
		WHEN 2 THEN "En Proceso"
        WHEN 3 THEN "Cerrado"
        WHEN 4 THEN "Resuelto"
		END);
        
        RETURN @StatusResult;
        
END //
DELIMITER ;

DELIMITER //
CREATE FUNCTION GetPriorityOfTicket(Priority VARCHAR(20))
RETURNS VARCHAR(20)
DETERMINISTIC
BEGIN
    SET @PriorityResult =(
		CASE Priority 
		WHEN 1 THEN "Bajo"
		WHEN 2 THEN "Medio"
        WHEN 3 THEN "Alto"
		END);
        
        RETURN @PriorityResult;
END //
DELIMITER ;


