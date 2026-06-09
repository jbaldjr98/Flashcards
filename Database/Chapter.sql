CREATE TABLE [dbo].[Chapter]
(
	[Id] INT NOT NULL PRIMARY KEY, 
    [Name] NVARCHAR(50) NULL, 
    [Description] NVARCHAR(50) NULL, 
    [SubjectId] INT NULL, 
    CONSTRAINT [FK_Chapter_ToSubject] FOREIGN KEY (SubjectId) REFERENCES [Subject]([Id])
)
