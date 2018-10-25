CREATE TABLE Message_detail
(
message_id  INT	IDENTITY	PRIMARY KEY,
user_id_from	int	,
user_id_for	int	,
message	TEXT,
parent_message_id	int	,
message_status BIT ,--	(0- Unread, 1 - Read)
message_datetime	datetime	
)
