CREATE TABLE [dbo].[Tasks]
(
	[Id] INT NOT NULL PRIMARY KEY, 
    [PackageName] TEXT NOT NULL, 
    [Time] DATETIME NOT NULL, 
    [Platform] SMALLINT NOT NULL, 
    [Code] TEXT NOT NULL, 
    [InputFile] TEXT NULL, 
    [OutputFile] TEXT NULL
)
