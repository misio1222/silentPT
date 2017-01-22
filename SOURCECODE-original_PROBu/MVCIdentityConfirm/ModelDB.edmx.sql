
-- --------------------------------------------------
-- Entity Designer DDL Script for SQL Server 2005, 2008, 2012 and Azure
-- --------------------------------------------------
-- Date Created: 01/20/2017 12:42:17
-- Generated from EDMX file: C:\GitHub\silencePT\SOURCECODE-original_PROBu\MVCIdentityConfirm\ModelDB.edmx
-- --------------------------------------------------

SET QUOTED_IDENTIFIER OFF;
GO
USE [master];
GO
IF SCHEMA_ID(N'dbo') IS NULL EXECUTE(N'CREATE SCHEMA [dbo]');
GO

-- --------------------------------------------------
-- Dropping existing FOREIGN KEY constraints
-- --------------------------------------------------

IF OBJECT_ID(N'[dbo].[FK_CompanycompanyEmail]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[companyEmailSet] DROP CONSTRAINT [FK_CompanycompanyEmail];
GO

-- --------------------------------------------------
-- Dropping existing tables
-- --------------------------------------------------

IF OBJECT_ID(N'[dbo].[C__MigrationHistory]', 'U') IS NOT NULL
    DROP TABLE [dbo].[C__MigrationHistory];
GO
IF OBJECT_ID(N'[dbo].[Company]', 'U') IS NOT NULL
    DROP TABLE [dbo].[Company];
GO
IF OBJECT_ID(N'[dbo].[companyEmailSet]', 'U') IS NOT NULL
    DROP TABLE [dbo].[companyEmailSet];
GO
IF OBJECT_ID(N'[dbo].[MSreplication_options]', 'U') IS NOT NULL
    DROP TABLE [dbo].[MSreplication_options];
GO
IF OBJECT_ID(N'[dbo].[ocenaCompany]', 'U') IS NOT NULL
    DROP TABLE [dbo].[ocenaCompany];
GO
IF OBJECT_ID(N'[dbo].[ocenaPrzelozony]', 'U') IS NOT NULL
    DROP TABLE [dbo].[ocenaPrzelozony];
GO
IF OBJECT_ID(N'[dbo].[Przelozony]', 'U') IS NOT NULL
    DROP TABLE [dbo].[Przelozony];
GO
IF OBJECT_ID(N'[dbo].[ratingCompany]', 'U') IS NOT NULL
    DROP TABLE [dbo].[ratingCompany];
GO
IF OBJECT_ID(N'[dbo].[spt_fallback_db]', 'U') IS NOT NULL
    DROP TABLE [dbo].[spt_fallback_db];
GO
IF OBJECT_ID(N'[dbo].[spt_fallback_dev]', 'U') IS NOT NULL
    DROP TABLE [dbo].[spt_fallback_dev];
GO
IF OBJECT_ID(N'[dbo].[spt_fallback_usg]', 'U') IS NOT NULL
    DROP TABLE [dbo].[spt_fallback_usg];
GO
IF OBJECT_ID(N'[dbo].[spt_monitor]', 'U') IS NOT NULL
    DROP TABLE [dbo].[spt_monitor];
GO
IF OBJECT_ID(N'[dbo].[UsersCompany]', 'U') IS NOT NULL
    DROP TABLE [dbo].[UsersCompany];
GO
IF OBJECT_ID(N'[dbo].[Wydzial]', 'U') IS NOT NULL
    DROP TABLE [dbo].[Wydzial];
GO
IF OBJECT_ID(N'[dbo].[wypowiedzUser]', 'U') IS NOT NULL
    DROP TABLE [dbo].[wypowiedzUser];
GO
IF OBJECT_ID(N'[dbo].[zgloszenieNar]', 'U') IS NOT NULL
    DROP TABLE [dbo].[zgloszenieNar];
GO

-- --------------------------------------------------
-- Creating all tables
-- --------------------------------------------------

-- Creating table 'C__MigrationHistory'
CREATE TABLE [dbo].[C__MigrationHistory] (
    [MigrationId] nvarchar(150)  NOT NULL,
    [ContextKey] nvarchar(300)  NOT NULL,
    [Model] varbinary(max)  NOT NULL,
    [ProductVersion] nvarchar(32)  NOT NULL
);
GO

-- Creating table 'Company'
CREATE TABLE [dbo].[Company] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [CompanyName] nvarchar(max)  NOT NULL,
    [Wojewodztwo] nvarchar(max)  NOT NULL,
    [Logo] varbinary(max)  NULL,
    [IsSelected] bit  NULL,
    [Miejscowosc] nvarchar(max)  NOT NULL,
    [Ulica] nvarchar(max)  NULL,
    [NIP] int  NULL,
    [Regon] int  NULL
);
GO

-- Creating table 'UsersCompany'
CREATE TABLE [dbo].[UsersCompany] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [UserID] nvarchar(max)  NOT NULL,
    [CompanyID] int  NOT NULL,
    [WydzialID] int  NULL
);
GO

-- Creating table 'Wydzial'
CREATE TABLE [dbo].[Wydzial] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [CompanyID] int  NOT NULL,
    [Wydzial1] nvarchar(max)  NOT NULL,
    [PrzelozonyId] int  NOT NULL,
    [DodalId] nvarchar(max)  NOT NULL
);
GO

-- Creating table 'Przelozony'
CREATE TABLE [dbo].[Przelozony] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [Imie] nvarchar(50)  NOT NULL,
    [Nazwisko] nvarchar(50)  NOT NULL,
    [Stanowisko] nvarchar(50)  NOT NULL,
    [Dodalid] nvarchar(max)  NOT NULL
);
GO

-- Creating table 'wypowiedzUser'
CREATE TABLE [dbo].[wypowiedzUser] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [userId] nvarchar(max)  NOT NULL,
    [wydzialId] int  NOT NULL,
    [content] nvarchar(max)  NOT NULL,
    [companyId] int  NOT NULL,
    [userImie] nvarchar(max)  NOT NULL,
    [userNazwisko] nvarchar(max)  NOT NULL,
    [NameOrLogin] bit  NOT NULL,
    [userLogin] nvarchar(max)  NOT NULL,
    [dateTime] nvarchar(50)  NOT NULL,
    [like] int  NOT NULL,
    [notLike] int  NOT NULL
);
GO

-- Creating table 'zgloszenieNar'
CREATE TABLE [dbo].[zgloszenieNar] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [idAnswear] int  NOT NULL,
    [userId] nvarchar(max)  NOT NULL
);
GO

-- Creating table 'ocenaPrzelozony'
CREATE TABLE [dbo].[ocenaPrzelozony] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [userId] nvarchar(max)  NOT NULL,
    [przelozonyId] int  NOT NULL,
    [kulturaOs] int  NOT NULL,
    [inteligencja] int  NOT NULL,
    [umiSluchania] int  NOT NULL,
    [przyznanieBlad] int  NOT NULL,
    [cwaniastwo] int  NOT NULL,
    [udzielaniePochwal] int  NOT NULL,
    [umieKomunikowania] int  NOT NULL,
    [radzenieKrytyka] int  NOT NULL,
    [rzetenaWiedza] int  NOT NULL
);
GO

-- Creating table 'ocenaCompany'
CREATE TABLE [dbo].[ocenaCompany] (
    [Id] int  NOT NULL,
    [userId] nvarchar(max)  NOT NULL,
    [companyId] int  NOT NULL,
    [awans] int  NOT NULL,
    [uznanie] int  NOT NULL,
    [ksztalcenie] int  NOT NULL,
    [zarobki] int  NOT NULL,
    [atmosfera] int  NOT NULL,
    [uklady_] int  NOT NULL,
    [mobbing] int  NOT NULL,
    [dodatki] int  NOT NULL,
    [socjal] int  NOT NULL,
    [stres] int  NOT NULL
);
GO

-- Creating table 'ratingCompany'
CREATE TABLE [dbo].[ratingCompany] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [companyId] int  NOT NULL,
    [liczbaVoice] int  NOT NULL,
    [awans] int  NOT NULL,
    [uznanie] int  NOT NULL,
    [ksztalcenie] int  NOT NULL,
    [zarobki] int  NOT NULL,
    [atmosfera] int  NOT NULL,
    [uklady] int  NOT NULL,
    [mobbing] int  NOT NULL,
    [dodatki] int  NOT NULL,
    [socjal] int  NOT NULL,
    [stres] int  NOT NULL,
    [ogolnyWynik] int  NULL
);
GO

-- Creating table 'MSreplication_options'
CREATE TABLE [dbo].[MSreplication_options] (
    [optname] nvarchar(128)  NOT NULL,
    [value] bit  NOT NULL,
    [major_version] int  NOT NULL,
    [minor_version] int  NOT NULL,
    [revision] int  NOT NULL,
    [install_failures] int  NOT NULL
);
GO

-- Creating table 'spt_fallback_db'
CREATE TABLE [dbo].[spt_fallback_db] (
    [xserver_name] varchar(30)  NOT NULL,
    [xdttm_ins] datetime  NOT NULL,
    [xdttm_last_ins_upd] datetime  NOT NULL,
    [xfallback_dbid] smallint  NULL,
    [name] varchar(30)  NOT NULL,
    [dbid] smallint  NOT NULL,
    [status] smallint  NOT NULL,
    [version] smallint  NOT NULL
);
GO

-- Creating table 'spt_fallback_dev'
CREATE TABLE [dbo].[spt_fallback_dev] (
    [xserver_name] varchar(30)  NOT NULL,
    [xdttm_ins] datetime  NOT NULL,
    [xdttm_last_ins_upd] datetime  NOT NULL,
    [xfallback_low] int  NULL,
    [xfallback_drive] char(2)  NULL,
    [low] int  NOT NULL,
    [high] int  NOT NULL,
    [status] smallint  NOT NULL,
    [name] varchar(30)  NOT NULL,
    [phyname] varchar(127)  NOT NULL
);
GO

-- Creating table 'spt_fallback_usg'
CREATE TABLE [dbo].[spt_fallback_usg] (
    [xserver_name] varchar(30)  NOT NULL,
    [xdttm_ins] datetime  NOT NULL,
    [xdttm_last_ins_upd] datetime  NOT NULL,
    [xfallback_vstart] int  NULL,
    [dbid] smallint  NOT NULL,
    [segmap] int  NOT NULL,
    [lstart] int  NOT NULL,
    [sizepg] int  NOT NULL,
    [vstart] int  NOT NULL
);
GO

-- Creating table 'spt_monitor'
CREATE TABLE [dbo].[spt_monitor] (
    [lastrun] datetime  NOT NULL,
    [cpu_busy] int  NOT NULL,
    [io_busy] int  NOT NULL,
    [idle] int  NOT NULL,
    [pack_received] int  NOT NULL,
    [pack_sent] int  NOT NULL,
    [connections] int  NOT NULL,
    [pack_errors] int  NOT NULL,
    [total_read] int  NOT NULL,
    [total_write] int  NOT NULL,
    [total_errors] int  NOT NULL
);
GO

-- Creating table 'companyEmailSet'
CREATE TABLE [dbo].[companyEmailSet] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [CompanyId] int  NOT NULL,
    [email] nvarchar(max)  NOT NULL,
    [stanowisko] nvarchar(max)  NOT NULL,
    [addedById] nvarchar(max)  NOT NULL
);
GO

-- --------------------------------------------------
-- Creating all PRIMARY KEY constraints
-- --------------------------------------------------

-- Creating primary key on [MigrationId], [ContextKey] in table 'C__MigrationHistory'
ALTER TABLE [dbo].[C__MigrationHistory]
ADD CONSTRAINT [PK_C__MigrationHistory]
    PRIMARY KEY CLUSTERED ([MigrationId], [ContextKey] ASC);
GO

-- Creating primary key on [Id] in table 'Company'
ALTER TABLE [dbo].[Company]
ADD CONSTRAINT [PK_Company]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'UsersCompany'
ALTER TABLE [dbo].[UsersCompany]
ADD CONSTRAINT [PK_UsersCompany]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'Wydzial'
ALTER TABLE [dbo].[Wydzial]
ADD CONSTRAINT [PK_Wydzial]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'Przelozony'
ALTER TABLE [dbo].[Przelozony]
ADD CONSTRAINT [PK_Przelozony]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'wypowiedzUser'
ALTER TABLE [dbo].[wypowiedzUser]
ADD CONSTRAINT [PK_wypowiedzUser]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'zgloszenieNar'
ALTER TABLE [dbo].[zgloszenieNar]
ADD CONSTRAINT [PK_zgloszenieNar]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'ocenaPrzelozony'
ALTER TABLE [dbo].[ocenaPrzelozony]
ADD CONSTRAINT [PK_ocenaPrzelozony]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'ocenaCompany'
ALTER TABLE [dbo].[ocenaCompany]
ADD CONSTRAINT [PK_ocenaCompany]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'ratingCompany'
ALTER TABLE [dbo].[ratingCompany]
ADD CONSTRAINT [PK_ratingCompany]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [optname], [value], [major_version], [minor_version], [revision], [install_failures] in table 'MSreplication_options'
ALTER TABLE [dbo].[MSreplication_options]
ADD CONSTRAINT [PK_MSreplication_options]
    PRIMARY KEY CLUSTERED ([optname], [value], [major_version], [minor_version], [revision], [install_failures] ASC);
GO

-- Creating primary key on [xserver_name], [xdttm_ins], [xdttm_last_ins_upd], [name], [dbid], [status], [version] in table 'spt_fallback_db'
ALTER TABLE [dbo].[spt_fallback_db]
ADD CONSTRAINT [PK_spt_fallback_db]
    PRIMARY KEY CLUSTERED ([xserver_name], [xdttm_ins], [xdttm_last_ins_upd], [name], [dbid], [status], [version] ASC);
GO

-- Creating primary key on [xserver_name], [xdttm_ins], [xdttm_last_ins_upd], [low], [high], [status], [name], [phyname] in table 'spt_fallback_dev'
ALTER TABLE [dbo].[spt_fallback_dev]
ADD CONSTRAINT [PK_spt_fallback_dev]
    PRIMARY KEY CLUSTERED ([xserver_name], [xdttm_ins], [xdttm_last_ins_upd], [low], [high], [status], [name], [phyname] ASC);
GO

-- Creating primary key on [xserver_name], [xdttm_ins], [xdttm_last_ins_upd], [dbid], [segmap], [lstart], [sizepg], [vstart] in table 'spt_fallback_usg'
ALTER TABLE [dbo].[spt_fallback_usg]
ADD CONSTRAINT [PK_spt_fallback_usg]
    PRIMARY KEY CLUSTERED ([xserver_name], [xdttm_ins], [xdttm_last_ins_upd], [dbid], [segmap], [lstart], [sizepg], [vstart] ASC);
GO

-- Creating primary key on [lastrun], [cpu_busy], [io_busy], [idle], [pack_received], [pack_sent], [connections], [pack_errors], [total_read], [total_write], [total_errors] in table 'spt_monitor'
ALTER TABLE [dbo].[spt_monitor]
ADD CONSTRAINT [PK_spt_monitor]
    PRIMARY KEY CLUSTERED ([lastrun], [cpu_busy], [io_busy], [idle], [pack_received], [pack_sent], [connections], [pack_errors], [total_read], [total_write], [total_errors] ASC);
GO

-- Creating primary key on [Id] in table 'companyEmailSet'
ALTER TABLE [dbo].[companyEmailSet]
ADD CONSTRAINT [PK_companyEmailSet]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- --------------------------------------------------
-- Creating all FOREIGN KEY constraints
-- --------------------------------------------------

-- Creating foreign key on [CompanyId] in table 'companyEmailSet'
ALTER TABLE [dbo].[companyEmailSet]
ADD CONSTRAINT [FK_CompanycompanyEmail]
    FOREIGN KEY ([CompanyId])
    REFERENCES [dbo].[Company]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_CompanycompanyEmail'
CREATE INDEX [IX_FK_CompanycompanyEmail]
ON [dbo].[companyEmailSet]
    ([CompanyId]);
GO

-- --------------------------------------------------
-- Script has ended
-- --------------------------------------------------