use Prison
go
ALTER TABLE [dbo].[PhoneNumber]  
WITH CHECK ADD  CONSTRAINT [FK_PhoneNumber_Detainee] FOREIGN KEY([DetaineeID])
REFERENCES [dbo].[Detainee] ([DetaineeID])
on update cascade
on delete cascade
GO







