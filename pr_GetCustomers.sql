/*
-- This contains a stored procedure pr_GetCustomers which will get customers by their respective date of birth
*/

-- 1. First we need to make sure we are creating this stored procedure to the right database
USE Customer_DB
GO

SET ANSI_NULLS ON
GO

-- 2. Create the stored procedure with dateofbirth as parameter 
CREATE PROCEDURE pr_GetCustomers @dateofbirth DATE
AS
BEGIN 
  
  -- 3. This will pull out customers by date of birth
  SELECT * 
    FROM Customer 
   WHERE DateOfBirth = @dateofbirth

   -- 4. Next we will send the notification to the queue for the application to consume

END