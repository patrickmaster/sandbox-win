CREATE TABLE [dbo].[TasksLibraries]
(
	[TaskId] INT     NOT NULL FOREIGN KEY REFERENCES Tasks(Id),
	[LibraryName]     VARCHAR (50) NOT NULL FOREIGN KEY REFERENCES Libraries(Name),
	CONSTRAINT PK_TasksLibraries PRIMARY KEY CLUSTERED ([TaskId], [LibraryName])
)
