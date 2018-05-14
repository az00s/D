use Prison
go
CREATE TABLE [dbo].[DetentionsOfDetainees](
	[ID] [int] Primary Key clustered identity(1,1),
	[DetaineeID] [int] NOT NULL,
	[DetentionID] [int] NOT NULL,
 
) 

GO

