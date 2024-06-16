CREATE TABLE [Events].[Event]
(
	[Id] INT NOT NULL PRIMARY KEY,
	[Name] NVARCHAR(100) NOT NULL,
	[OwnerId] INT NOT NULL,
	[ActivityId] INT NOT NULL,
	[Address] NVARCHAR(200) NULL,
	[Time] DATE NULL,
	[CityId] INT NULL,
	[CountryId] INT NULL,
	[IsOpenToEveryone] BIT NOT NULL,
	[MinimumPersons] INT NULL,
	[MaximumPersons] INT NULL,
	[Description] NVARCHAR(MAX) NOT NULL,
	[ImagePath] NVARCHAR(200) NULL,
	[RepetitionType] INT NULL,
	[AmIAttending] BIT NOT NULL,
	CONSTRAINT FK_Event_Activity FOREIGN KEY (ActivityId) REFERENCES [Events].[Activity](Id)


)
