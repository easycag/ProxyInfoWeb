ALTER PROCEDURE ProcBidOperation_Delete
(
@vQueryType VARCHAR(50),
@bid INT
)
AS
DECLARE @vID INT
DECLARE @vCount INT
DECLARE @bid_Amount NUMERIC(18,2)
DECLARE @load_id INT
BEGIN
Begin Try 
	
	IF(@vQueryType = 'DELETEBID')
	BEGIN
		BEGIN TRANSACTION 
		  INSERT INTO BidDetail_deleted SELECT * FROM BidDetail WHERE bid_id=@bid
		  DELETE FROM BidDetail WHERE bid_id=@bid
		COMMIT TRANSACTION  
	END
	IF(@vQueryType = 'ACCEPTBID')
	BEGIN
		BEGIN TRANSACTION 
		 UPDATE BidDetail SET bid_Status=1, Bid_Update_DateTime=getDATE() WHERE bid_id=@bid;
	  SELECT @bid_Amount=bid_amount, @load_id=load_id  FROM BidDetail WHERE bid_id=@bid;
	  UPDATE offer_load SET load_total_amount=@bid_Amount,load_pending_amount=@bid_Amount  WHERE load_id=@load_id;
		COMMIT TRANSACTION  
	END
	IF(@vQueryType = 'REJECTBID')
	BEGIN
		BEGIN TRANSACTION 
		 UPDATE BidDetail SET bid_Status=2, Bid_Update_DateTime=getDATE() WHERE bid_id=@bid;
		COMMIT TRANSACTION  
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

