ALTER PROCEDURE ProcBidOperation
(
@vQueryType VARCHAR(50),
@bid INT,
@cid INT,
@bidamount        VARCHAR (100),
@userid                  INT,
@viewed                  VARCHAR(50),
@status      VARCHAR (1),
@creationdatetime DATETIME2,
@BidUpdateDateTime DATETIME2
)
AS
DECLARE @vID INT
DECLARE @vCount INT
BEGIN
Begin Try 
	IF(@vQueryType = 'PLACEBID')
	BEGIN
	  INSERT INTO BidDetail (load_id, bid_amount, user_id, Viewed, bid_status)
	  VALUES (@cid, @bidamount, @userid, @viewed, '0')
	END
	--ELSE IF(@vQueryType = 'ACCEPTBID')
	--BEGIN
	--  UPDATE BidDetail SET Status=1, BidUpdateDateTime=getDATE() WHERE bid_id=@bid;
	--  UPDATE ConsignmentOffer SET BidID=@bid WHERE CId=@cid;
	--END
	--ELSE IF(@vQueryType = 'REJECTBID')
	--BEGIN
	--  UPDATE BidDetail SET Status='R', BidUpdateDateTime=getDATE() WHERE BidID=@bid 
	--END
	ELSE IF(@vQueryType = 'UPDATEBID')
	BEGIN
	  UPDATE BidDetail SET bid_amount=@bidamount, bid_update_datetime=getDATE(), bind_update_count= bind_update_count +1 WHERE bid_id=@bid 
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
        
	RAISERROR (@ErrorMessage, -- Message text.  
               @ErrorSeverity, -- Severity.  
               @ErrorState -- State.  
               );  
	END CATCH
END;
GO

