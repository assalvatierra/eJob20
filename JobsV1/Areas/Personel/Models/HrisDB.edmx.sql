
-- --------------------------------------------------
-- Entity Designer DDL Script for SQL Server 2005, 2008, 2012 and Azure
-- --------------------------------------------------
-- Date Created: 05/03/2019 17:49:14
-- Generated from EDMX file: C:\Users\VILLOSA\Documents\GithubClassic\eJob20\JobsV1\Areas\Personel\Models\HrisDB.edmx
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

IF OBJECT_ID(N'[dbo].[FK_HrPersonelHrPerDoc]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[HrPerDocs] DROP CONSTRAINT [FK_HrPersonelHrPerDoc];
GO
IF OBJECT_ID(N'[dbo].[FK_HrPersonelHrPerPosition]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[HrPerPositions] DROP CONSTRAINT [FK_HrPersonelHrPerPosition];
GO
IF OBJECT_ID(N'[dbo].[FK_HrPositionHrPerPosition]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[HrPerPositions] DROP CONSTRAINT [FK_HrPositionHrPerPosition];
GO
IF OBJECT_ID(N'[dbo].[FK_HrTrainingHrPerTraining]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[HrPerTrainings] DROP CONSTRAINT [FK_HrTrainingHrPerTraining];
GO
IF OBJECT_ID(N'[dbo].[FK_HrPersonelHrPerTraining]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[HrPerTrainings] DROP CONSTRAINT [FK_HrPersonelHrPerTraining];
GO
IF OBJECT_ID(N'[dbo].[FK_HrSkillHrPerSkill]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[HrPerSkills] DROP CONSTRAINT [FK_HrSkillHrPerSkill];
GO
IF OBJECT_ID(N'[dbo].[FK_HrProficiencyHrPerSkill]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[HrPerSkills] DROP CONSTRAINT [FK_HrProficiencyHrPerSkill];
GO
IF OBJECT_ID(N'[dbo].[FK_HrPersonelHrPerSkill]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[HrPerSkills] DROP CONSTRAINT [FK_HrPersonelHrPerSkill];
GO
IF OBJECT_ID(N'[dbo].[FK_HrTrainingHrTrainingSkill]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[HrTrainingSkills] DROP CONSTRAINT [FK_HrTrainingHrTrainingSkill];
GO
IF OBJECT_ID(N'[dbo].[FK_HrSkillHrTrainingSkill]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[HrTrainingSkills] DROP CONSTRAINT [FK_HrSkillHrTrainingSkill];
GO
IF OBJECT_ID(N'[dbo].[FK_HrProficiencyHrTrainingSkill]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[HrTrainingSkills] DROP CONSTRAINT [FK_HrProficiencyHrTrainingSkill];
GO
IF OBJECT_ID(N'[dbo].[FK_HrDtrStatusHrDtr]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[HrDtrs] DROP CONSTRAINT [FK_HrDtrStatusHrDtr];
GO
IF OBJECT_ID(N'[dbo].[FK_HrPersonelHrDtr]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[HrDtrs] DROP CONSTRAINT [FK_HrPersonelHrDtr];
GO
IF OBJECT_ID(N'[dbo].[FK_HrPersonelHrSalary]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[HrSalaries] DROP CONSTRAINT [FK_HrPersonelHrSalary];
GO
IF OBJECT_ID(N'[dbo].[FK_HrSalaryHrDtr]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[HrDtrs] DROP CONSTRAINT [FK_HrSalaryHrDtr];
GO
IF OBJECT_ID(N'[dbo].[FK_HrPersonelHrPayroll]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[HrPayrolls] DROP CONSTRAINT [FK_HrPersonelHrPayroll];
GO
IF OBJECT_ID(N'[dbo].[FK_HrPersonelHrProfile]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[HrProfiles] DROP CONSTRAINT [FK_HrPersonelHrProfile];
GO
IF OBJECT_ID(N'[dbo].[FK_HrPersonelStatusHrPersonel]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[HrPersonels] DROP CONSTRAINT [FK_HrPersonelStatusHrPersonel];
GO
IF OBJECT_ID(N'[dbo].[FK_HrPayrollHrDtr]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[HrDtrs] DROP CONSTRAINT [FK_HrPayrollHrDtr];
GO

-- --------------------------------------------------
-- Dropping existing tables
-- --------------------------------------------------

IF OBJECT_ID(N'[dbo].[HrPersonels]', 'U') IS NOT NULL
    DROP TABLE [dbo].[HrPersonels];
GO
IF OBJECT_ID(N'[dbo].[HrPerPositions]', 'U') IS NOT NULL
    DROP TABLE [dbo].[HrPerPositions];
GO
IF OBJECT_ID(N'[dbo].[HrPerDocs]', 'U') IS NOT NULL
    DROP TABLE [dbo].[HrPerDocs];
GO
IF OBJECT_ID(N'[dbo].[HrPositions]', 'U') IS NOT NULL
    DROP TABLE [dbo].[HrPositions];
GO
IF OBJECT_ID(N'[dbo].[HrTrainings]', 'U') IS NOT NULL
    DROP TABLE [dbo].[HrTrainings];
GO
IF OBJECT_ID(N'[dbo].[HrPerTrainings]', 'U') IS NOT NULL
    DROP TABLE [dbo].[HrPerTrainings];
GO
IF OBJECT_ID(N'[dbo].[HrSkills]', 'U') IS NOT NULL
    DROP TABLE [dbo].[HrSkills];
GO
IF OBJECT_ID(N'[dbo].[HrPerSkills]', 'U') IS NOT NULL
    DROP TABLE [dbo].[HrPerSkills];
GO
IF OBJECT_ID(N'[dbo].[HrProficiencies]', 'U') IS NOT NULL
    DROP TABLE [dbo].[HrProficiencies];
GO
IF OBJECT_ID(N'[dbo].[HrTrainingSkills]', 'U') IS NOT NULL
    DROP TABLE [dbo].[HrTrainingSkills];
GO
IF OBJECT_ID(N'[dbo].[HrDtrs]', 'U') IS NOT NULL
    DROP TABLE [dbo].[HrDtrs];
GO
IF OBJECT_ID(N'[dbo].[HrDtrStatus]', 'U') IS NOT NULL
    DROP TABLE [dbo].[HrDtrStatus];
GO
IF OBJECT_ID(N'[dbo].[HrSalaries]', 'U') IS NOT NULL
    DROP TABLE [dbo].[HrSalaries];
GO
IF OBJECT_ID(N'[dbo].[HrPayrolls]', 'U') IS NOT NULL
    DROP TABLE [dbo].[HrPayrolls];
GO
IF OBJECT_ID(N'[dbo].[HrProfiles]', 'U') IS NOT NULL
    DROP TABLE [dbo].[HrProfiles];
GO
IF OBJECT_ID(N'[dbo].[HrPersonelStatus]', 'U') IS NOT NULL
    DROP TABLE [dbo].[HrPersonelStatus];
GO

-- --------------------------------------------------
-- Creating all tables
-- --------------------------------------------------

-- Creating table 'HrPersonels'
CREATE TABLE [dbo].[HrPersonels] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [Name] nvarchar(80)  NOT NULL,
    [PassportNo] nvarchar(20)  NULL,
    [SSSid] nvarchar(20)  NULL,
    [Tin] nvarchar(20)  NULL,
    [DriverId] nvarchar(20)  NULL,
    [Remarks] nvarchar(80)  NULL,
    [HrPersonelStatusId] int  NOT NULL
);
GO

-- Creating table 'HrPerPositions'
CREATE TABLE [dbo].[HrPerPositions] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [HrPersonelId] int  NOT NULL,
    [HrPositionId] int  NOT NULL,
    [DtStart] datetime  NOT NULL
);
GO

-- Creating table 'HrPerDocs'
CREATE TABLE [dbo].[HrPerDocs] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [HrPersonelId] int  NOT NULL,
    [Description] nvarchar(max)  NOT NULL,
    [FilePath] nvarchar(max)  NOT NULL
);
GO

-- Creating table 'HrPositions'
CREATE TABLE [dbo].[HrPositions] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [Desc] nvarchar(30)  NOT NULL,
    [Remarks] nvarchar(80)  NULL
);
GO

-- Creating table 'HrTrainings'
CREATE TABLE [dbo].[HrTrainings] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [Desc] nvarchar(30)  NOT NULL,
    [Remarks] nvarchar(80)  NULL
);
GO

-- Creating table 'HrPerTrainings'
CREATE TABLE [dbo].[HrPerTrainings] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [HrTrainingId] int  NOT NULL,
    [HrPersonelId] int  NOT NULL,
    [DtCompleted] datetime  NOT NULL
);
GO

-- Creating table 'HrSkills'
CREATE TABLE [dbo].[HrSkills] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [Desc] nvarchar(50)  NOT NULL,
    [Remarks] nvarchar(80)  NULL
);
GO

-- Creating table 'HrPerSkills'
CREATE TABLE [dbo].[HrPerSkills] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [HrSkillId] int  NOT NULL,
    [DtAcquired] datetime  NOT NULL,
    [HrProficiencyId] int  NOT NULL,
    [HrPersonelId] int  NOT NULL
);
GO

-- Creating table 'HrProficiencies'
CREATE TABLE [dbo].[HrProficiencies] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [Level] nvarchar(20)  NOT NULL
);
GO

-- Creating table 'HrTrainingSkills'
CREATE TABLE [dbo].[HrTrainingSkills] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [HrTrainingId] int  NOT NULL,
    [HrSkillId] int  NOT NULL,
    [HrProficiencyId] int  NOT NULL
);
GO

-- Creating table 'HrDtrs'
CREATE TABLE [dbo].[HrDtrs] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [HrSalaryId] int  NOT NULL,
    [DtrDate] datetime  NOT NULL,
    [HrDtrStatusId] int  NOT NULL,
    [HrPersonelId] int  NOT NULL,
    [TimeIn] time  NOT NULL,
    [TimeOut] time  NOT NULL,
    [ActualHrs] decimal(18,0)  NOT NULL,
    [RoundHrs] int  NOT NULL,
    [HrPayrollId] int  NOT NULL
);
GO

-- Creating table 'HrDtrStatus'
CREATE TABLE [dbo].[HrDtrStatus] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [Desc] nvarchar(10)  NOT NULL,
    [Factor] decimal(18,0)  NOT NULL
);
GO

-- Creating table 'HrSalaries'
CREATE TABLE [dbo].[HrSalaries] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [HrPersonelId] int  NOT NULL,
    [DtStart] datetime  NOT NULL,
    [RatePerHr] decimal(18,0)  NOT NULL
);
GO

-- Creating table 'HrPayrolls'
CREATE TABLE [dbo].[HrPayrolls] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [HrPersonelId] int  NOT NULL,
    [DtStart] datetime  NOT NULL,
    [DtEnd] datetime  NOT NULL,
    [Salary] decimal(18,0)  NOT NULL,
    [Allowance] decimal(18,0)  NOT NULL,
    [Deduction] decimal(18,0)  NOT NULL,
    [Yearno] nvarchar(4)  NOT NULL,
    [Monthno] nvarchar(2)  NOT NULL,
    [Status] nvarchar(3)  NOT NULL
);
GO

-- Creating table 'HrProfiles'
CREATE TABLE [dbo].[HrProfiles] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [FirstName] nvarchar(20)  NOT NULL,
    [LastName] nvarchar(30)  NOT NULL,
    [MiddleName] nvarchar(30)  NOT NULL,
    [Mobile1] nvarchar(20)  NULL,
    [Mobile2] nvarchar(20)  NULL,
    [Email] nvarchar(120)  NULL,
    [fbAccount] nvarchar(120)  NULL,
    [PresentAddress] nvarchar(250)  NULL,
    [ProvincialAddress] nvarchar(250)  NULL,
    [Spouse] nvarchar(80)  NULL,
    [HrPersonelId] int  NOT NULL
);
GO

-- Creating table 'HrPersonelStatus'
CREATE TABLE [dbo].[HrPersonelStatus] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [Desc] nvarchar(20)  NOT NULL
);
GO

-- --------------------------------------------------
-- Creating all PRIMARY KEY constraints
-- --------------------------------------------------

-- Creating primary key on [Id] in table 'HrPersonels'
ALTER TABLE [dbo].[HrPersonels]
ADD CONSTRAINT [PK_HrPersonels]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'HrPerPositions'
ALTER TABLE [dbo].[HrPerPositions]
ADD CONSTRAINT [PK_HrPerPositions]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'HrPerDocs'
ALTER TABLE [dbo].[HrPerDocs]
ADD CONSTRAINT [PK_HrPerDocs]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'HrPositions'
ALTER TABLE [dbo].[HrPositions]
ADD CONSTRAINT [PK_HrPositions]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'HrTrainings'
ALTER TABLE [dbo].[HrTrainings]
ADD CONSTRAINT [PK_HrTrainings]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'HrPerTrainings'
ALTER TABLE [dbo].[HrPerTrainings]
ADD CONSTRAINT [PK_HrPerTrainings]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'HrSkills'
ALTER TABLE [dbo].[HrSkills]
ADD CONSTRAINT [PK_HrSkills]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'HrPerSkills'
ALTER TABLE [dbo].[HrPerSkills]
ADD CONSTRAINT [PK_HrPerSkills]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'HrProficiencies'
ALTER TABLE [dbo].[HrProficiencies]
ADD CONSTRAINT [PK_HrProficiencies]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'HrTrainingSkills'
ALTER TABLE [dbo].[HrTrainingSkills]
ADD CONSTRAINT [PK_HrTrainingSkills]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'HrDtrs'
ALTER TABLE [dbo].[HrDtrs]
ADD CONSTRAINT [PK_HrDtrs]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'HrDtrStatus'
ALTER TABLE [dbo].[HrDtrStatus]
ADD CONSTRAINT [PK_HrDtrStatus]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'HrSalaries'
ALTER TABLE [dbo].[HrSalaries]
ADD CONSTRAINT [PK_HrSalaries]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'HrPayrolls'
ALTER TABLE [dbo].[HrPayrolls]
ADD CONSTRAINT [PK_HrPayrolls]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'HrProfiles'
ALTER TABLE [dbo].[HrProfiles]
ADD CONSTRAINT [PK_HrProfiles]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'HrPersonelStatus'
ALTER TABLE [dbo].[HrPersonelStatus]
ADD CONSTRAINT [PK_HrPersonelStatus]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- --------------------------------------------------
-- Creating all FOREIGN KEY constraints
-- --------------------------------------------------

-- Creating foreign key on [HrPersonelId] in table 'HrPerDocs'
ALTER TABLE [dbo].[HrPerDocs]
ADD CONSTRAINT [FK_HrPersonelHrPerDoc]
    FOREIGN KEY ([HrPersonelId])
    REFERENCES [dbo].[HrPersonels]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_HrPersonelHrPerDoc'
CREATE INDEX [IX_FK_HrPersonelHrPerDoc]
ON [dbo].[HrPerDocs]
    ([HrPersonelId]);
GO

-- Creating foreign key on [HrPersonelId] in table 'HrPerPositions'
ALTER TABLE [dbo].[HrPerPositions]
ADD CONSTRAINT [FK_HrPersonelHrPerPosition]
    FOREIGN KEY ([HrPersonelId])
    REFERENCES [dbo].[HrPersonels]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_HrPersonelHrPerPosition'
CREATE INDEX [IX_FK_HrPersonelHrPerPosition]
ON [dbo].[HrPerPositions]
    ([HrPersonelId]);
GO

-- Creating foreign key on [HrPositionId] in table 'HrPerPositions'
ALTER TABLE [dbo].[HrPerPositions]
ADD CONSTRAINT [FK_HrPositionHrPerPosition]
    FOREIGN KEY ([HrPositionId])
    REFERENCES [dbo].[HrPositions]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_HrPositionHrPerPosition'
CREATE INDEX [IX_FK_HrPositionHrPerPosition]
ON [dbo].[HrPerPositions]
    ([HrPositionId]);
GO

-- Creating foreign key on [HrTrainingId] in table 'HrPerTrainings'
ALTER TABLE [dbo].[HrPerTrainings]
ADD CONSTRAINT [FK_HrTrainingHrPerTraining]
    FOREIGN KEY ([HrTrainingId])
    REFERENCES [dbo].[HrTrainings]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_HrTrainingHrPerTraining'
CREATE INDEX [IX_FK_HrTrainingHrPerTraining]
ON [dbo].[HrPerTrainings]
    ([HrTrainingId]);
GO

-- Creating foreign key on [HrPersonelId] in table 'HrPerTrainings'
ALTER TABLE [dbo].[HrPerTrainings]
ADD CONSTRAINT [FK_HrPersonelHrPerTraining]
    FOREIGN KEY ([HrPersonelId])
    REFERENCES [dbo].[HrPersonels]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_HrPersonelHrPerTraining'
CREATE INDEX [IX_FK_HrPersonelHrPerTraining]
ON [dbo].[HrPerTrainings]
    ([HrPersonelId]);
GO

-- Creating foreign key on [HrSkillId] in table 'HrPerSkills'
ALTER TABLE [dbo].[HrPerSkills]
ADD CONSTRAINT [FK_HrSkillHrPerSkill]
    FOREIGN KEY ([HrSkillId])
    REFERENCES [dbo].[HrSkills]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_HrSkillHrPerSkill'
CREATE INDEX [IX_FK_HrSkillHrPerSkill]
ON [dbo].[HrPerSkills]
    ([HrSkillId]);
GO

-- Creating foreign key on [HrProficiencyId] in table 'HrPerSkills'
ALTER TABLE [dbo].[HrPerSkills]
ADD CONSTRAINT [FK_HrProficiencyHrPerSkill]
    FOREIGN KEY ([HrProficiencyId])
    REFERENCES [dbo].[HrProficiencies]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_HrProficiencyHrPerSkill'
CREATE INDEX [IX_FK_HrProficiencyHrPerSkill]
ON [dbo].[HrPerSkills]
    ([HrProficiencyId]);
GO

-- Creating foreign key on [HrPersonelId] in table 'HrPerSkills'
ALTER TABLE [dbo].[HrPerSkills]
ADD CONSTRAINT [FK_HrPersonelHrPerSkill]
    FOREIGN KEY ([HrPersonelId])
    REFERENCES [dbo].[HrPersonels]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_HrPersonelHrPerSkill'
CREATE INDEX [IX_FK_HrPersonelHrPerSkill]
ON [dbo].[HrPerSkills]
    ([HrPersonelId]);
GO

-- Creating foreign key on [HrTrainingId] in table 'HrTrainingSkills'
ALTER TABLE [dbo].[HrTrainingSkills]
ADD CONSTRAINT [FK_HrTrainingHrTrainingSkill]
    FOREIGN KEY ([HrTrainingId])
    REFERENCES [dbo].[HrTrainings]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_HrTrainingHrTrainingSkill'
CREATE INDEX [IX_FK_HrTrainingHrTrainingSkill]
ON [dbo].[HrTrainingSkills]
    ([HrTrainingId]);
GO

-- Creating foreign key on [HrSkillId] in table 'HrTrainingSkills'
ALTER TABLE [dbo].[HrTrainingSkills]
ADD CONSTRAINT [FK_HrSkillHrTrainingSkill]
    FOREIGN KEY ([HrSkillId])
    REFERENCES [dbo].[HrSkills]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_HrSkillHrTrainingSkill'
CREATE INDEX [IX_FK_HrSkillHrTrainingSkill]
ON [dbo].[HrTrainingSkills]
    ([HrSkillId]);
GO

-- Creating foreign key on [HrProficiencyId] in table 'HrTrainingSkills'
ALTER TABLE [dbo].[HrTrainingSkills]
ADD CONSTRAINT [FK_HrProficiencyHrTrainingSkill]
    FOREIGN KEY ([HrProficiencyId])
    REFERENCES [dbo].[HrProficiencies]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_HrProficiencyHrTrainingSkill'
CREATE INDEX [IX_FK_HrProficiencyHrTrainingSkill]
ON [dbo].[HrTrainingSkills]
    ([HrProficiencyId]);
GO

-- Creating foreign key on [HrDtrStatusId] in table 'HrDtrs'
ALTER TABLE [dbo].[HrDtrs]
ADD CONSTRAINT [FK_HrDtrStatusHrDtr]
    FOREIGN KEY ([HrDtrStatusId])
    REFERENCES [dbo].[HrDtrStatus]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_HrDtrStatusHrDtr'
CREATE INDEX [IX_FK_HrDtrStatusHrDtr]
ON [dbo].[HrDtrs]
    ([HrDtrStatusId]);
GO

-- Creating foreign key on [HrPersonelId] in table 'HrDtrs'
ALTER TABLE [dbo].[HrDtrs]
ADD CONSTRAINT [FK_HrPersonelHrDtr]
    FOREIGN KEY ([HrPersonelId])
    REFERENCES [dbo].[HrPersonels]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_HrPersonelHrDtr'
CREATE INDEX [IX_FK_HrPersonelHrDtr]
ON [dbo].[HrDtrs]
    ([HrPersonelId]);
GO

-- Creating foreign key on [HrPersonelId] in table 'HrSalaries'
ALTER TABLE [dbo].[HrSalaries]
ADD CONSTRAINT [FK_HrPersonelHrSalary]
    FOREIGN KEY ([HrPersonelId])
    REFERENCES [dbo].[HrPersonels]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_HrPersonelHrSalary'
CREATE INDEX [IX_FK_HrPersonelHrSalary]
ON [dbo].[HrSalaries]
    ([HrPersonelId]);
GO

-- Creating foreign key on [HrSalaryId] in table 'HrDtrs'
ALTER TABLE [dbo].[HrDtrs]
ADD CONSTRAINT [FK_HrSalaryHrDtr]
    FOREIGN KEY ([HrSalaryId])
    REFERENCES [dbo].[HrSalaries]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_HrSalaryHrDtr'
CREATE INDEX [IX_FK_HrSalaryHrDtr]
ON [dbo].[HrDtrs]
    ([HrSalaryId]);
GO

-- Creating foreign key on [HrPersonelId] in table 'HrPayrolls'
ALTER TABLE [dbo].[HrPayrolls]
ADD CONSTRAINT [FK_HrPersonelHrPayroll]
    FOREIGN KEY ([HrPersonelId])
    REFERENCES [dbo].[HrPersonels]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_HrPersonelHrPayroll'
CREATE INDEX [IX_FK_HrPersonelHrPayroll]
ON [dbo].[HrPayrolls]
    ([HrPersonelId]);
GO

-- Creating foreign key on [HrPersonelId] in table 'HrProfiles'
ALTER TABLE [dbo].[HrProfiles]
ADD CONSTRAINT [FK_HrPersonelHrProfile]
    FOREIGN KEY ([HrPersonelId])
    REFERENCES [dbo].[HrPersonels]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_HrPersonelHrProfile'
CREATE INDEX [IX_FK_HrPersonelHrProfile]
ON [dbo].[HrProfiles]
    ([HrPersonelId]);
GO

-- Creating foreign key on [HrPersonelStatusId] in table 'HrPersonels'
ALTER TABLE [dbo].[HrPersonels]
ADD CONSTRAINT [FK_HrPersonelStatusHrPersonel]
    FOREIGN KEY ([HrPersonelStatusId])
    REFERENCES [dbo].[HrPersonelStatus]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_HrPersonelStatusHrPersonel'
CREATE INDEX [IX_FK_HrPersonelStatusHrPersonel]
ON [dbo].[HrPersonels]
    ([HrPersonelStatusId]);
GO

-- Creating foreign key on [HrPayrollId] in table 'HrDtrs'
ALTER TABLE [dbo].[HrDtrs]
ADD CONSTRAINT [FK_HrPayrollHrDtr]
    FOREIGN KEY ([HrPayrollId])
    REFERENCES [dbo].[HrPayrolls]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_HrPayrollHrDtr'
CREATE INDEX [IX_FK_HrPayrollHrDtr]
ON [dbo].[HrDtrs]
    ([HrPayrollId]);
GO

-- --------------------------------------------------
-- Script has ended
-- --------------------------------------------------