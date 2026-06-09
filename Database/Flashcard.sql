CREATE TABLE [dbo].[Flashcard]
(
	[Id] INT NOT NULL PRIMARY KEY, 
    [subjectId] INT NULL, 
    [chapterId] INT NULL, 
    [front] NVARCHAR(50) NULL, 
    [back] NVARCHAR(50) NULL, 
    [successRate] FLOAT NULL, 
    [isRevisit] BIT NULL, 
    CONSTRAINT [FK_Flashcard_ToChapter] FOREIGN KEY ([chapterId]) REFERENCES [Chapter]([Id]), 
    CONSTRAINT [FK_Flashcard_ToSubject] FOREIGN KEY ([subjectId]) REFERENCES [Subject]([Id])
)
