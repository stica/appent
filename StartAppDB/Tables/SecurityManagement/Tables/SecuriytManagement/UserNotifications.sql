CREATE TABLE [SecurityManagement].[UserNotifications]
(
	[Id] INT NOT NULL PRIMARY KEY,
	[UserId] INT NOT NULL,
	[Text] NVARCHAR(100) NOT NULL,
	[Title] NVARCHAR(50) NOT NULL,
	CONSTRAINT [FK_UserNotifications_User] FOREIGN KEY ([UserId]) REFERENCES [SecurityManagement].[User]([Id]),
)
