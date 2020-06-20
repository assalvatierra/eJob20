
-- --------------------------------------------------
-- Entity Designer DDL Script for SQL Server 2005, 2008, 2012 and Azure
-- --------------------------------------------------
-- Date Created: 06/19/2020 19:56:02
-- Generated from EDMX file: D:\Projects\eJob20\JobsV1\Areas\AutoCare\Data\AppointmentDB.edmx
-- --------------------------------------------------

SET QUOTED_IDENTIFIER OFF;
GO
USE [aspnet-JobsV1-20160528101923];
GO
IF SCHEMA_ID(N'dbo') IS NULL EXECUTE(N'CREATE SCHEMA [dbo]');
GO

-- --------------------------------------------------
-- Dropping existing FOREIGN KEY constraints
-- --------------------------------------------------

IF OBJECT_ID(N'[dbo].[FK_AppointmentStatusAppointment]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[Appointments] DROP CONSTRAINT [FK_AppointmentStatusAppointment];
GO
IF OBJECT_ID(N'[dbo].[FK_AppointmentSlotAppointment]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[Appointments] DROP CONSTRAINT [FK_AppointmentSlotAppointment];
GO

-- --------------------------------------------------
-- Dropping existing tables
-- --------------------------------------------------

IF OBJECT_ID(N'[dbo].[Appointments]', 'U') IS NOT NULL
    DROP TABLE [dbo].[Appointments];
GO
IF OBJECT_ID(N'[dbo].[AppointmentStatus]', 'U') IS NOT NULL
    DROP TABLE [dbo].[AppointmentStatus];
GO
IF OBJECT_ID(N'[dbo].[AppointmentSlots]', 'U') IS NOT NULL
    DROP TABLE [dbo].[AppointmentSlots];
GO

-- --------------------------------------------------
-- Creating all tables
-- --------------------------------------------------

-- Creating table 'Appointments'
CREATE TABLE [dbo].[Appointments] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [DtEntered] datetime  NOT NULL,
    [Customer] nvarchar(60)  NOT NULL,
    [Contact] nvarchar(30)  NOT NULL,
    [CustCode] nvarchar(10)  NULL,
    [Plate] nvarchar(max)  NOT NULL,
    [Conduction] nvarchar(max)  NULL,
    [Request] nvarchar(max)  NOT NULL,
    [Remarks] nvarchar(max)  NULL,
    [AppointmentStatusId] int  NOT NULL,
    [AppointmentSlotId] int  NOT NULL,
    [AppointmentDate] nvarchar(max)  NOT NULL
);
GO

-- Creating table 'AppointmentStatus'
CREATE TABLE [dbo].[AppointmentStatus] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [Status] nvarchar(10)  NOT NULL
);
GO

-- Creating table 'AppointmentSlots'
CREATE TABLE [dbo].[AppointmentSlots] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [Description] nvarchar(max)  NOT NULL
);
GO

-- --------------------------------------------------
-- Creating all PRIMARY KEY constraints
-- --------------------------------------------------

-- Creating primary key on [Id] in table 'Appointments'
ALTER TABLE [dbo].[Appointments]
ADD CONSTRAINT [PK_Appointments]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'AppointmentStatus'
ALTER TABLE [dbo].[AppointmentStatus]
ADD CONSTRAINT [PK_AppointmentStatus]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'AppointmentSlots'
ALTER TABLE [dbo].[AppointmentSlots]
ADD CONSTRAINT [PK_AppointmentSlots]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- --------------------------------------------------
-- Creating all FOREIGN KEY constraints
-- --------------------------------------------------

-- Creating foreign key on [AppointmentStatusId] in table 'Appointments'
ALTER TABLE [dbo].[Appointments]
ADD CONSTRAINT [FK_AppointmentStatusAppointment]
    FOREIGN KEY ([AppointmentStatusId])
    REFERENCES [dbo].[AppointmentStatus]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_AppointmentStatusAppointment'
CREATE INDEX [IX_FK_AppointmentStatusAppointment]
ON [dbo].[Appointments]
    ([AppointmentStatusId]);
GO

-- Creating foreign key on [AppointmentSlotId] in table 'Appointments'
ALTER TABLE [dbo].[Appointments]
ADD CONSTRAINT [FK_AppointmentSlotAppointment]
    FOREIGN KEY ([AppointmentSlotId])
    REFERENCES [dbo].[AppointmentSlots]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_AppointmentSlotAppointment'
CREATE INDEX [IX_FK_AppointmentSlotAppointment]
ON [dbo].[Appointments]
    ([AppointmentSlotId]);
GO

-- --------------------------------------------------
-- Script has ended
-- --------------------------------------------------