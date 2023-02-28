CREATE TABLE [dbo].[song]
(
	[Id]		INT						NOT NULL	PRIMARY KEY IDENTITY	,

	USR_UUID	varchar(512)			NOT NULL							, 
	songHash	varchar(512)			NOT NULL	unique					, -- hash points to the song on a user play list.

	title		varchar(512)												,
	artist		varchar(512)												,
	plays		int													,
)