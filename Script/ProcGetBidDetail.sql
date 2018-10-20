ALTER PROCEDURE ProcGetBidDetail
(
@vQueryType VARCHAR(50),
@vFilterClause VARCHAR(500)
)
AS
DECLARE @vID INT
DECLARE @vCount INT
DECLARE @vQuery VARCHAR(5000)
BEGIN	
	SET @vQuery =  'SELECT * FROM BidDetail ' + @vFilterClause;
	EXEC(@vQuery);
END;
GO

