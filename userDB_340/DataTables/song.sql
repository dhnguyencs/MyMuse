CREATE TABLE [dbo].[song]
(
	[Id]		INT						NOT NULL	PRIMARY KEY IDENTITY	,

	USR_UUID	varchar(512)			NOT NULL							, 
	songHash	varchar(512)			NOT NULL	unique					, -- hash = (1000 byte of file) + byteof(User UUID)

	title		varchar(512)												,
	artist		varchar(512)												,
	plays		int															,
	songLength	int						NOT NULL							,
	fav			int						NOT NULL							, -- 0 = not fav, 1 = fav
	_type		varchar(10)				NOT NULL							, -- mp3, wav, ogg, m4a

)