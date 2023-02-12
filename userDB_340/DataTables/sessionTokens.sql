CREATE TABLE [dbo].[SessionTokens]
(
	[Id]			INT NOT NULL PRIMARY KEY identity	,
	[SessionID]		varchar(256) unique not null		,
	[accountHash]	varchar(512) not null				,
	[ip]			varchar(512)						,
	[device]		varchar(128)						,
	[created]		datetime							,
	[lastUsed]		datetime							,

	constraint [referenceToRealUser] foreign key (accountHash) references users(UUID)
)