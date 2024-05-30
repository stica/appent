﻿/*
Deployment script for StartAppDB

This code was generated by a tool.
Changes to this file may cause incorrect behavior and will be lost if
the code is regenerated.
*/

GO
SET ANSI_NULLS, ANSI_PADDING, ANSI_WARNINGS, ARITHABORT, CONCAT_NULL_YIELDS_NULL, QUOTED_IDENTIFIER ON;

SET NUMERIC_ROUNDABORT OFF;


GO
:setvar DatabaseName "StartAppDB"
:setvar DefaultFilePrefix "StartAppDB"
:setvar DefaultDataPath "C:\Program Files\Microsoft SQL Server\MSSQL14.MSSQLSERVER\MSSQL\DATA\"
:setvar DefaultLogPath "C:\Program Files\Microsoft SQL Server\MSSQL14.MSSQLSERVER\MSSQL\DATA\"

GO
:on error exit
GO
/*
Detect SQLCMD mode and disable script execution if SQLCMD mode is not supported.
To re-enable the script after enabling SQLCMD mode, execute the following:
SET NOEXEC OFF; 
*/
:setvar __IsSqlCmdEnabled "True"
GO
IF N'$(__IsSqlCmdEnabled)' NOT LIKE N'True'
    BEGIN
        PRINT N'SQLCMD mode must be enabled to successfully execute this script.';
        SET NOEXEC ON;
    END


GO
USE [$(DatabaseName)];


GO
PRINT N'Creating Table [SecurityManagement].[Policy]...';


GO
CREATE TABLE [SecurityManagement].[Policy] (
    [Id]           INT            IDENTITY (1, 1) NOT NULL,
    [Name]         NVARCHAR (50)  NOT NULL,
    [JSONDocument] NVARCHAR (MAX) NOT NULL,
    CONSTRAINT [PK_Policy] PRIMARY KEY CLUSTERED ([Id] ASC)
);


GO
PRINT N'Creating Table [SecurityManagement].[UserGroup]...';


GO
CREATE TABLE [SecurityManagement].[UserGroup] (
    [Id]   INT           IDENTITY (1, 1) NOT NULL,
    [Name] NVARCHAR (50) NOT NULL,
    CONSTRAINT [PK_UserGroup] PRIMARY KEY CLUSTERED ([Id] ASC)
);


GO
PRINT N'Creating Table [SecurityManagement].[UserUserGroup]...';


GO
CREATE TABLE [SecurityManagement].[UserUserGroup] (
    [Id]          INT IDENTITY (1, 1) NOT NULL,
    [UserId]      INT NOT NULL,
    [UserGroupId] INT NOT NULL,
    CONSTRAINT [PK_UserUserGroup] PRIMARY KEY CLUSTERED ([Id] ASC)
);


GO
PRINT N'Creating Table [SecurityManagement].[UserUserPolicy]...';


GO
CREATE TABLE [SecurityManagement].[UserUserPolicy] (
    [Id]          INT IDENTITY (1, 1) NOT NULL,
    [PolicyId]    INT NOT NULL,
    [UserGroupId] INT NOT NULL,
    CONSTRAINT [PK_UserGroupPolicy] PRIMARY KEY CLUSTERED ([Id] ASC)
);


GO
PRINT N'Creating Foreign Key [SecurityManagement].[FK_UserUserGroup_User]...';


GO
ALTER TABLE [SecurityManagement].[UserUserGroup] WITH NOCHECK
    ADD CONSTRAINT [FK_UserUserGroup_User] FOREIGN KEY ([UserId]) REFERENCES [SecurityManagement].[User] ([Id]);


GO
PRINT N'Creating Foreign Key [SecurityManagement].[FK_UserUserGroup_UserGroup]...';


GO
ALTER TABLE [SecurityManagement].[UserUserGroup] WITH NOCHECK
    ADD CONSTRAINT [FK_UserUserGroup_UserGroup] FOREIGN KEY ([UserGroupId]) REFERENCES [SecurityManagement].[UserGroup] ([Id]);


GO
PRINT N'Creating Foreign Key [SecurityManagement].[FK_UserGroupPolicy_Policy]...';


GO
ALTER TABLE [SecurityManagement].[UserUserPolicy] WITH NOCHECK
    ADD CONSTRAINT [FK_UserGroupPolicy_Policy] FOREIGN KEY ([PolicyId]) REFERENCES [SecurityManagement].[Policy] ([Id]);


GO
PRINT N'Creating Foreign Key [SecurityManagement].[FK_UserGroupPolicy_UserGroup]...';


GO
ALTER TABLE [SecurityManagement].[UserUserPolicy] WITH NOCHECK
    ADD CONSTRAINT [FK_UserGroupPolicy_UserGroup] FOREIGN KEY ([UserGroupId]) REFERENCES [SecurityManagement].[UserGroup] ([Id]);


GO
PRINT N'Checking existing data against newly created constraints';


GO
USE [$(DatabaseName)];


GO
ALTER TABLE [SecurityManagement].[UserUserGroup] WITH CHECK CHECK CONSTRAINT [FK_UserUserGroup_User];

ALTER TABLE [SecurityManagement].[UserUserGroup] WITH CHECK CHECK CONSTRAINT [FK_UserUserGroup_UserGroup];

ALTER TABLE [SecurityManagement].[UserUserPolicy] WITH CHECK CHECK CONSTRAINT [FK_UserGroupPolicy_Policy];

ALTER TABLE [SecurityManagement].[UserUserPolicy] WITH CHECK CHECK CONSTRAINT [FK_UserGroupPolicy_UserGroup];


GO
PRINT N'Update complete.';


GO
