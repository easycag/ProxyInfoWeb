CREATE TABLE BidDetail
(
bid_id	int	IDENTITY PRIMARY KEY,
user_id	INT,	
load_id	int	,
bid_amount	NUMERIC(18,2)	,
bid_datetime	DATETIME DEFAULT getdate(),	
viewed BIT DEFAULT(0),
bid_status	int,
bind_update_count	int	DEFAULT(0),
bid_update_datetime	DATETIME  ,

CONSTRAINT FK_userbid FOREIGN KEY (user_id) REFERENCES user_login(user_id),
CONSTRAINT FK_loadbid FOREIGN KEY (load_id) REFERENCES offer_load(load_id)
)


