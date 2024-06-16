CREATE TABLE [Events].[FollowedActivity]
(
	[Id] INT NOT NULL PRIMARY KEY,
	[UserId] INT NOT NULL,
	[ActivityId] INT NOT NULL,
	CONSTRAINT FK_FollowedActivity_Activity FOREIGN KEY (ActivityId) REFERENCES [Events].[Activity](Id),
	CONSTRAINT FK_FollowedActivity_User FOREIGN KEY (UserId) REFERENCES [SecurityManagement].[User](Id)
)
