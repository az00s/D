use Prison
go
create table Detainee
(
	[DetaineeID] [int] Primary Key clustered identity (1,1),
	[FirstName] [nvarchar](35) NOT NULL,
	[LastName] [nvarchar](35) NOT NULL,
	[Patronymic] [nvarchar](40) NULL,
	[BirstDate] [date] NOT NULL,
	[MaritalStatusID] [int] NOT NULL,
	[WorkPlace] [nvarchar](max) NOT NULL,
	[Photo] [nvarchar](max) NOT NULL,
	[ResidenceAddress] [nvarchar](max) NOT NULL,
	[AdditionalData] [nvarchar](max) NULL

)
GO

