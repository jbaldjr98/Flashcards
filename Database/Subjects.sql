CREATE TABLE [dbo].[Subjects]
(
    [Id]          INT            NOT NULL IDENTITY(1,1) PRIMARY KEY,
    [Name]        NVARCHAR(100)  NOT NULL,
    [Description] NVARCHAR(MAX)  NULL
)
