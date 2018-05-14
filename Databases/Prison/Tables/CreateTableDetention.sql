use Prison
go
CREATE TABLE [dbo].[Detention](
	[DetentionID] [int] Primary Key clustered identity(1,1) NOT NULL,
	[DetentionDate] [date] NOT NULL,
	[DetainedByWhomID] [int] NOT NULL,
	[DeliveryDate] [date] NOT NULL,
	[DeliveredByWhomID] [int] NOT NULL,
	[ReleasåDate] [date] NOT NULL,
	[ReleasedByWhomID] [int] NOT NULL,
	[PlaceOfStayID] [int] NOT NULL,
	[AmountForStaying] [decimal](18, 0) NOT NULL,
	[PaidAmount] [decimal](18, 0) NOT NULL,
) 

GO

