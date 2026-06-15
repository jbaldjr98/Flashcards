CREATE TABLE [dbo].[Flashcard]
(
	[Id] INT NOT NULL PRIMARY KEY, 
    [subjectId] INT NULL, 
    [chapterId] INT NULL, 
    [front] NVARCHAR(50) NULL, 
    [back] NVARCHAR(50) NULL, 
    [numSuccess] INT NULL, 
    [isRevisit] BIT NULL, 
    [numFailure] INT NULL, 
    CONSTRAINT [FK_Flashcard_ToChapter] FOREIGN KEY ([chapterId]) REFERENCES [Chapter]([Id]), 
    CONSTRAINT [FK_Flashcard_ToSubject] FOREIGN KEY ([subjectId]) REFERENCES [Subject]([Id])
)
