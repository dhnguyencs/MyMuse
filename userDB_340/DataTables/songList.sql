CREATE TABLE [dbo].[songList]
(
	[Id]		INT				NOT NULL	PRIMARY KEY IDENTITY	,

	USR_UUID	varchar(512)	NOT NULL							, 
	p_l_hash	varchar(512)	NOT NULL							, -- references a hash, using this hash as the ID of of the playlist this entry belongs to
	songHash	varchar(512)	NOT NULL							, -- hash points to the song on a user play list.

	title		varchar(512)										,
	artist		varchar(512)										,

)