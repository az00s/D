use Prison
go
ALTER TABLE [dbo].[Detention]  
WITH CHECK ADD  CONSTRAINT [FK_Detention_Employee1] FOREIGN KEY([DetainedByWhomID])
REFERENCES [dbo].[Employee] ([EmployeeID])
GO


ALTER TABLE [dbo].[Detention]  
WITH CHECK ADD  CONSTRAINT [FK_Detention_PlaceOfDetention] FOREIGN KEY([PlaceOfStayID])
REFERENCES [dbo].[PlaceOfStay] ([PlaceID])
GO




