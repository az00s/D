use Prison
go
ALTER TABLE [dbo].[Employee]  
WITH CHECK ADD  CONSTRAINT [FK_Employee_Position] FOREIGN KEY([PositionID])
REFERENCES [dbo].[Position] ([PositionID])
GO






