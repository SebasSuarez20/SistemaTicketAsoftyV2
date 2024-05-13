DELIMITER $$
CREATE TRIGGER AfterHasMapping
BEFORE UPDATE ON ticketssupport
FOR EACH ROW
BEGIN

    SET @ConsHas  = (SELECT COALESCE(Consecutive, '') FROM tickethasmapping tkh 
    WHERE tkh.Consecutive = NEW.Consecutive AND tkh.Enabled = TRUE 
    LIMIT 1);

    IF @ConsHas IS NULL THEN  
        INSERT INTO ticketHasMapping (Consecutive, HasUnique,Message,Enabled,Username) 
        VALUES (NEW.Consecutive, MD5(NEW.Consecutive),'¡Bienvenido a nuestro servicio de soporte en línea! Estamos aquí para ayudarte con cualquier pregunta, problema o consulta que puedas tener. Por favor, descríbenos tu problema o pregunta y te atenderemos lo antes posible. ¡Gracias por confiar en nosotros para resolver tus inquietudes!',1,new.AssignedTo);
    END IF;
END$$
DELIMITER ;

