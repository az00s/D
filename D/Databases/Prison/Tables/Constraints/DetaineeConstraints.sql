use Prison
go
ALTER TABLE [dbo].[Detainee]  
WITH CHECK ADD  CONSTRAINT [FK_Detainee_MaritalStatus] FOREIGN KEY([MaritalStatusID])
REFERENCES [dbo].[MaritalStatus] ([StatusID])
GO




