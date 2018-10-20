
CREATE PROCEDURE ProcBidOperation_Delete
(
@vQueryType VARCHAR(50),
@bid INT
)
AS
DECLARE @vID INT
DECLARE @vCount INT
BEGIN
Begin Try 
	
	IF(@vQueryType = 'DELETEBID')
	BEGIN
		BEGIN TRANSACTION 
		  INSERT INTO BidDetail_deleted SELECT * FROM BidDetail WHERE bid_id=@bid
		  DELETE FROM BidDetail WHERE bid_id=@bid
		COMMIT TRANSACTION DELETEBid 
	END
	END TRY
	BEGIN CATCH
	 DECLARE @ErrorMessage NVARCHAR(4000);  
    DECLARE @ErrorSeverity INT;  
    DECLARE @ErrorState INT;  

    SELECT   
        @ErrorMessage = ERROR_MESSAGE(),  
        @ErrorSeverity = ERROR_SEVERITY(),  
        @ErrorState = ERROR_STATE();
        
   
	ROLLBACK TRANSACTION 
	
	RAISERROR (@ErrorMessage, -- Message text.  
               @ErrorSeverity, -- Severity.  
               @ErrorState -- State.  
               );  
	END CATCH
END;
GO

