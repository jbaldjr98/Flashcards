CREATE TABLE [dbo].[Chapters]
(
    [Id]          INT            NOT NULL IDENTITY(1,1) PRIMARY KEY,
    [Name]        NVARCHAR(100)  NOT NULL,
    [Description] NVARCHAR(MAX)  NOT NULL,
    [SubjectId]   INT            NOT NULL,
    CONSTRAINT [FK_Chapters_Subjects] FOREIGN KEY ([SubjectId]) REFERENCES [Subjects]([Id]) ON DELETE CASCADE
)
