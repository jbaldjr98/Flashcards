CREATE TABLE [dbo].[Flashcards]
(
    [Id]          INT            NOT NULL IDENTITY(1,1) PRIMARY KEY,
    [SubjectId]   INT            NOT NULL,
    [ChapterId]   INT            NOT NULL,
    [Front]       NVARCHAR(MAX)  NOT NULL,
    [Back]        NVARCHAR(MAX)  NOT NULL,
    [numSuccess]  INT            NOT NULL DEFAULT 0,
    [numFailure]  INT            NOT NULL DEFAULT 0,
    [IsRevisit]   BIT            NOT NULL DEFAULT 0,
    CONSTRAINT [FK_Flashcards_Chapters] FOREIGN KEY ([ChapterId]) REFERENCES [Chapters]([Id]) ON DELETE CASCADE,
    CONSTRAINT [FK_Flashcards_Subjects] FOREIGN KEY ([SubjectId]) REFERENCES [Subjects]([Id])
)
