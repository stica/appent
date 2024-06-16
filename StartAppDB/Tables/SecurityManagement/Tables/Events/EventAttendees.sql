CREATE TABLE [Events].[EventAttendees]
(
	[Id] INT NOT NULL PRIMARY KEY,
	[EventId] INT NOT NULL,
	[UserId] INT NOT NULL,
	CONSTRAINT FK_EventAttendees_Event FOREIGN KEY (EventId) REFERENCES [Events].[Event](Id),
	CONSTRAINT FK_EventAttendees_User FOREIGN KEY (UserId) REFERENCES [SecurityManagement].[User](Id),

)
