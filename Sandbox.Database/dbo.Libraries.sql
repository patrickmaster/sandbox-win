CREATE TABLE [dbo].[Libraries] (
    [Name]     VARCHAR (50) NOT NULL PRIMARY KEY,
    [Path]     TEXT         NOT NULL,
    [TaskId] INT     NOT NULL FOREIGN KEY REFERENCES Tasks(Id)
);

