CREATE TABLE [dbo].[users]
(
	[Id]			INT					PRIMARY		KEY		identity	NOT NULL,
	UUID			varchar(512)		unique							not null,
	FIRST_NAME		varchar(30)											not null,
	LAST_NAME		varchar(30)											not null,
	EMAIL			varchar(128)		unique							not null, 
    [USER_BACKGROUND] IMAGE NULL,
)