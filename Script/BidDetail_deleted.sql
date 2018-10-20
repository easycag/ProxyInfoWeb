
CREATE TABLE BidDetail_deleted
(
bid_id	int,
user_id	INT,	
load_id	int	,
bid_amount	NUMERIC(18,2)	,
bid_datetime	DATETIME DEFAULT getdate(),	
viewed BIT DEFAULT(0),
bid_status	int,
bind_update_count	int	DEFAULT(0),
bid_update_datetime	DATETIME  ,
)


