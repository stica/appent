CREATE TABLE [Events].[EventRating]
(
	[Id] INT NOT NULL PRIMARY KEY,
	[UserId] INT NOT NULL,
	[EventId] INT NOT NULL,
	[Rating] DECIMAL(18,2) NOT NULL,
	CONSTRAINT FK_EventRating_User FOREIGN KEY (UserId) REFERENCES [SecurityManagement].[User](Id),
	CONSTRAINT FK_EventRating_Event FOREIGN KEY (EventId) REFERENCES [Events].[Event](Id)
)
