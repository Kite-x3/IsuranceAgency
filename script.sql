USE [master]
GO
/****** Object:  Database [InsuranceDB]    Script Date: 26.12.2024 8:26:38 ******/
CREATE DATABASE [InsuranceDB]
 CONTAINMENT = NONE
 ON  PRIMARY 
( NAME = N'InsuranceDB', FILENAME = N'C:\Program Files\Microsoft SQL Server\MSSQL11.SQLEXPRESS\MSSQL\DATA\InsuranceDB.mdf' , SIZE = 5120KB , MAXSIZE = UNLIMITED, FILEGROWTH = 1024KB )
 LOG ON 
( NAME = N'InsuranceDB_log', FILENAME = N'C:\Program Files\Microsoft SQL Server\MSSQL11.SQLEXPRESS\MSSQL\DATA\InsuranceDB_log.ldf' , SIZE = 2048KB , MAXSIZE = 2048GB , FILEGROWTH = 10%)
GO
ALTER DATABASE [InsuranceDB] SET COMPATIBILITY_LEVEL = 110
GO
IF (1 = FULLTEXTSERVICEPROPERTY('IsFullTextInstalled'))
begin
EXEC [InsuranceDB].[dbo].[sp_fulltext_database] @action = 'enable'
end
GO
ALTER DATABASE [InsuranceDB] SET ANSI_NULL_DEFAULT OFF 
GO
ALTER DATABASE [InsuranceDB] SET ANSI_NULLS OFF 
GO
ALTER DATABASE [InsuranceDB] SET ANSI_PADDING OFF 
GO
ALTER DATABASE [InsuranceDB] SET ANSI_WARNINGS OFF 
GO
ALTER DATABASE [InsuranceDB] SET ARITHABORT OFF 
GO
ALTER DATABASE [InsuranceDB] SET AUTO_CLOSE OFF 
GO
ALTER DATABASE [InsuranceDB] SET AUTO_CREATE_STATISTICS ON 
GO
ALTER DATABASE [InsuranceDB] SET AUTO_SHRINK OFF 
GO
ALTER DATABASE [InsuranceDB] SET AUTO_UPDATE_STATISTICS ON 
GO
ALTER DATABASE [InsuranceDB] SET CURSOR_CLOSE_ON_COMMIT OFF 
GO
ALTER DATABASE [InsuranceDB] SET CURSOR_DEFAULT  GLOBAL 
GO
ALTER DATABASE [InsuranceDB] SET CONCAT_NULL_YIELDS_NULL OFF 
GO
ALTER DATABASE [InsuranceDB] SET NUMERIC_ROUNDABORT OFF 
GO
ALTER DATABASE [InsuranceDB] SET QUOTED_IDENTIFIER OFF 
GO
ALTER DATABASE [InsuranceDB] SET RECURSIVE_TRIGGERS OFF 
GO
ALTER DATABASE [InsuranceDB] SET  DISABLE_BROKER 
GO
ALTER DATABASE [InsuranceDB] SET AUTO_UPDATE_STATISTICS_ASYNC OFF 
GO
ALTER DATABASE [InsuranceDB] SET DATE_CORRELATION_OPTIMIZATION OFF 
GO
ALTER DATABASE [InsuranceDB] SET TRUSTWORTHY OFF 
GO
ALTER DATABASE [InsuranceDB] SET ALLOW_SNAPSHOT_ISOLATION OFF 
GO
ALTER DATABASE [InsuranceDB] SET PARAMETERIZATION SIMPLE 
GO
ALTER DATABASE [InsuranceDB] SET READ_COMMITTED_SNAPSHOT OFF 
GO
ALTER DATABASE [InsuranceDB] SET HONOR_BROKER_PRIORITY OFF 
GO
ALTER DATABASE [InsuranceDB] SET RECOVERY SIMPLE 
GO
ALTER DATABASE [InsuranceDB] SET  MULTI_USER 
GO
ALTER DATABASE [InsuranceDB] SET PAGE_VERIFY CHECKSUM  
GO
ALTER DATABASE [InsuranceDB] SET DB_CHAINING OFF 
GO
ALTER DATABASE [InsuranceDB] SET FILESTREAM( NON_TRANSACTED_ACCESS = OFF ) 
GO
ALTER DATABASE [InsuranceDB] SET TARGET_RECOVERY_TIME = 0 SECONDS 
GO
USE [InsuranceDB]
GO
/****** Object:  StoredProcedure [dbo].[GetAnnualReport]    Script Date: 26.12.2024 8:26:38 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[GetAnnualReport]
    @Year INT
AS
BEGIN
    SELECT 
        COUNT(*) AS TotalInsuranceCases,
        SUM(IC.PayoutAmount) AS TotalPayout,
        SUM(C.Cost) AS TotalContractCost
    FROM InsuranceCase IC
    INNER JOIN Contract C ON IC.ContractID = C.ContractID
    WHERE YEAR(IC.Date) = @Year
      AND YEAR(C.StartDate) = @Year;

    SELECT TOP 1 
        IP.Name AS MostProfitableProgramName,
        SUM(C.Cost - IC.PayoutAmount) AS Profit
    FROM InsuranceCase IC
    INNER JOIN Contract C ON IC.ContractID = C.ContractID
    INNER JOIN InsuranceProgram IP ON C.ProgramID = IP.ProgramID
    WHERE YEAR(IC.Date) = @Year
      AND YEAR(C.StartDate) = @Year
    GROUP BY IP.Name
    ORDER BY Profit DESC;
END;

GO
/****** Object:  Table [dbo].[CaseType]    Script Date: 26.12.2024 8:26:38 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[CaseType](
	[CaseTypeID] [int] NOT NULL,
	[Situation] [nvarchar](255) NOT NULL,
	[BaseCost] [int] NOT NULL,
	[Property] [bit] NULL,
 CONSTRAINT [PK__CaseType__E7270816307ADA47] PRIMARY KEY CLUSTERED 
(
	[CaseTypeID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[Client]    Script Date: 26.12.2024 8:26:38 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Client](
	[ClientID] [int] NOT NULL,
	[FullName] [nvarchar](100) NOT NULL,
	[Passport] [nvarchar](20) NOT NULL,
	[BirthDate] [date] NOT NULL,
	[Password] [nchar](20) NOT NULL,
	[Email] [nchar](50) NOT NULL,
 CONSTRAINT [PK__Client__E67E1A04A4893177] PRIMARY KEY CLUSTERED 
(
	[ClientID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[Contract]    Script Date: 26.12.2024 8:26:38 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Contract](
	[ContractID] [int] NOT NULL,
	[ClientID] [int] NOT NULL,
	[InsuranceAgentID] [int] NULL,
	[Cost] [decimal](18, 2) NOT NULL,
	[StartDate] [date] NOT NULL,
	[EndDate] [date] NOT NULL,
	[ObjectID] [int] NULL,
	[ProgramID] [int] NOT NULL,
	[signed] [bit] NULL,
	[ready] [bit] NULL,
	[Comment] [nchar](100) NULL,
	[Number] [int] IDENTITY(1000,1) NOT NULL,
 CONSTRAINT [PK__Contract__C90D3409C1872677] PRIMARY KEY CLUSTERED 
(
	[ContractID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[InsuranceAgent]    Script Date: 26.12.2024 8:26:38 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[InsuranceAgent](
	[InsuranceAgentID] [int] NOT NULL,
	[FullName] [nvarchar](100) NOT NULL,
	[Position] [nvarchar](50) NOT NULL,
	[Password] [nchar](20) NOT NULL,
	[Email] [nchar](50) NOT NULL,
 CONSTRAINT [PK__Insuranc__F63B6D24E90F9168] PRIMARY KEY CLUSTERED 
(
	[InsuranceAgentID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[InsuranceCase]    Script Date: 26.12.2024 8:26:38 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[InsuranceCase](
	[CaseID] [int] NOT NULL,
	[ContractID] [int] NOT NULL,
	[PayoutAmount] [decimal](18, 2) NOT NULL,
	[Date] [date] NOT NULL,
	[CaseTypeID] [int] NOT NULL,
	[signed] [bit] NULL,
	[description] [nchar](100) NULL,
	[PayoutDate] [date] NULL,
	[Comment] [nchar](100) NULL,
 CONSTRAINT [PK__Insuranc__6CAE526C3FA09A93] PRIMARY KEY CLUSTERED 
(
	[CaseID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[InsuranceProgram]    Script Date: 26.12.2024 8:26:38 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[InsuranceProgram](
	[ProgramID] [int] NOT NULL,
	[Name] [nvarchar](100) NOT NULL,
	[CostFormula] [nchar](20) NOT NULL,
	[Property] [bit] NOT NULL,
	[Description] [varchar](max) NOT NULL,
 CONSTRAINT [PK__Insuranc__75256038F7BAAFF2] PRIMARY KEY CLUSTERED 
(
	[ProgramID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[LifestyleOptions]    Script Date: 26.12.2024 8:26:38 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[LifestyleOptions](
	[Lifestyle] [nchar](100) NULL,
	[Scale] [float] NULL,
	[id] [int] IDENTITY(1,1) NOT NULL,
 CONSTRAINT [PK_LifestyleOptions] PRIMARY KEY CLUSTERED 
(
	[id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[Property]    Script Date: 26.12.2024 8:26:38 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Property](
	[PropertyID] [int] NOT NULL,
	[Address] [nvarchar](255) NOT NULL,
	[EstimatedValue] [decimal](18, 2) NOT NULL,
	[TotalArea] [float] NOT NULL,
 CONSTRAINT [PK_Property] PRIMARY KEY CLUSTERED 
(
	[PropertyID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[TimingOptions]    Script Date: 26.12.2024 8:26:38 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[TimingOptions](
	[timing] [nchar](10) NULL,
	[scale] [float] NULL,
	[id] [int] IDENTITY(1,1) NOT NULL,
 CONSTRAINT [PK_TimingOptions] PRIMARY KEY CLUSTERED 
(
	[id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  View [dbo].[InsuranceAnalytics]    Script Date: 26.12.2024 8:26:38 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE VIEW [dbo].[InsuranceAnalytics]
AS
SELECT C.FullName AS ClientFullName, IA.FullName AS InsuranceAgentFullName, Co.Cost AS ContractCost, IC.PayoutAmount AS CasePayoutAmount, IC.Date AS CaseDate, P.Address AS PropertyAddress, 
                  P.EstimatedValue AS PropertyEstimatedValue, P.Risks AS PropertyRisks, L.Age AS LifeAge, L.Gender AS LifeGender, L.LifeRisks
FROM     dbo.Client AS C INNER JOIN
                  dbo.Contract AS Co ON C.ClientID = Co.ClientID INNER JOIN
                  dbo.InsuranceAgent AS IA ON Co.InsuranceAgentID = IA.InsuranceAgentID INNER JOIN
                  dbo.InsuranceCase AS IC ON Co.ContractID = IC.ContractID INNER JOIN
                  dbo.InsuranceObject AS IO ON Co.ObjectID = IO.ObjectID INNER JOIN
                  dbo.Property AS P ON IO.ObjectID = P.PropertyID INNER JOIN
                  dbo.Life AS L ON IO.ObjectID = L.ObjectID

GO
ALTER TABLE [dbo].[Contract]  WITH CHECK ADD  CONSTRAINT [FK__Contract__Client__164452B1] FOREIGN KEY([ClientID])
REFERENCES [dbo].[Client] ([ClientID])
GO
ALTER TABLE [dbo].[Contract] CHECK CONSTRAINT [FK__Contract__Client__164452B1]
GO
ALTER TABLE [dbo].[Contract]  WITH CHECK ADD  CONSTRAINT [FK__Contract__Insura__173876EA] FOREIGN KEY([InsuranceAgentID])
REFERENCES [dbo].[InsuranceAgent] ([InsuranceAgentID])
GO
ALTER TABLE [dbo].[Contract] CHECK CONSTRAINT [FK__Contract__Insura__173876EA]
GO
ALTER TABLE [dbo].[Contract]  WITH CHECK ADD  CONSTRAINT [FK__Contract__Progra__35BCFE0A] FOREIGN KEY([ProgramID])
REFERENCES [dbo].[InsuranceProgram] ([ProgramID])
GO
ALTER TABLE [dbo].[Contract] CHECK CONSTRAINT [FK__Contract__Progra__35BCFE0A]
GO
ALTER TABLE [dbo].[Contract]  WITH CHECK ADD  CONSTRAINT [FK_Contract_Property] FOREIGN KEY([ObjectID])
REFERENCES [dbo].[Property] ([PropertyID])
GO
ALTER TABLE [dbo].[Contract] CHECK CONSTRAINT [FK_Contract_Property]
GO
ALTER TABLE [dbo].[InsuranceCase]  WITH CHECK ADD  CONSTRAINT [FK__Insurance__CaseT__21B6055D] FOREIGN KEY([CaseTypeID])
REFERENCES [dbo].[CaseType] ([CaseTypeID])
GO
ALTER TABLE [dbo].[InsuranceCase] CHECK CONSTRAINT [FK__Insurance__CaseT__21B6055D]
GO
ALTER TABLE [dbo].[InsuranceCase]  WITH CHECK ADD  CONSTRAINT [FK__Insurance__Contr__1DE57479] FOREIGN KEY([ContractID])
REFERENCES [dbo].[Contract] ([ContractID])
GO
ALTER TABLE [dbo].[InsuranceCase] CHECK CONSTRAINT [FK__Insurance__Contr__1DE57479]
GO
EXEC sys.sp_addextendedproperty @name=N'MS_DiagramPane1', @value=N'[0E232FF0-B466-11cf-A24F-00AA00A3EFFF, 1.00]
Begin DesignProperties = 
   Begin PaneConfigurations = 
      Begin PaneConfiguration = 0
         NumPanes = 4
         Configuration = "(H (1[88] 4[5] 2[3] 3) )"
      End
      Begin PaneConfiguration = 1
         NumPanes = 3
         Configuration = "(H (1 [50] 4 [25] 3))"
      End
      Begin PaneConfiguration = 2
         NumPanes = 3
         Configuration = "(H (1 [50] 2 [25] 3))"
      End
      Begin PaneConfiguration = 3
         NumPanes = 3
         Configuration = "(H (4 [30] 2 [40] 3))"
      End
      Begin PaneConfiguration = 4
         NumPanes = 2
         Configuration = "(H (1 [56] 3))"
      End
      Begin PaneConfiguration = 5
         NumPanes = 2
         Configuration = "(H (2 [66] 3))"
      End
      Begin PaneConfiguration = 6
         NumPanes = 2
         Configuration = "(H (4 [50] 3))"
      End
      Begin PaneConfiguration = 7
         NumPanes = 1
         Configuration = "(V (3))"
      End
      Begin PaneConfiguration = 8
         NumPanes = 3
         Configuration = "(H (1[56] 4[18] 2) )"
      End
      Begin PaneConfiguration = 9
         NumPanes = 2
         Configuration = "(H (1 [75] 4))"
      End
      Begin PaneConfiguration = 10
         NumPanes = 2
         Configuration = "(H (1[66] 2) )"
      End
      Begin PaneConfiguration = 11
         NumPanes = 2
         Configuration = "(H (4 [60] 2))"
      End
      Begin PaneConfiguration = 12
         NumPanes = 1
         Configuration = "(H (1) )"
      End
      Begin PaneConfiguration = 13
         NumPanes = 1
         Configuration = "(V (4))"
      End
      Begin PaneConfiguration = 14
         NumPanes = 1
         Configuration = "(V (2))"
      End
      ActivePaneConfig = 0
   End
   Begin DiagramPane = 
      Begin Origin = 
         Top = 0
         Left = 0
      End
      Begin Tables = 
         Begin Table = "C"
            Begin Extent = 
               Top = 9
               Left = 434
               Bottom = 172
               Right = 635
            End
            DisplayFlags = 280
            TopColumn = 0
         End
         Begin Table = "Co"
            Begin Extent = 
               Top = 168
               Left = 787
               Bottom = 456
               Right = 999
            End
            DisplayFlags = 280
            TopColumn = 0
         End
         Begin Table = "IA"
            Begin Extent = 
               Top = 101
               Left = 177
               Bottom = 242
               Right = 389
            End
            DisplayFlags = 280
            TopColumn = 0
         End
         Begin Table = "IC"
            Begin Extent = 
               Top = 52
               Left = 1079
               Bottom = 270
               Right = 1280
            End
            DisplayFlags = 280
            TopColumn = 0
         End
         Begin Table = "IO"
            Begin Extent = 
               Top = 317
               Left = 509
               Bottom = 414
               Right = 710
            End
            DisplayFlags = 280
            TopColumn = 0
         End
         Begin Table = "P"
            Begin Extent = 
               Top = 247
               Left = 116
               Bottom = 436
               Right = 317
            End
            DisplayFlags = 280
            TopColumn = 0
         End
         Begin Table = "L"
            Begin Extent = 
               Top = 441
               Left = 209
               Bottom = 624
               Right = 410
            End
            DisplayFlags = 280
            TopColumn = 0
      ' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'VIEW',@level1name=N'InsuranceAnalytics'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_DiagramPane2', @value=N'   End
      End
   End
   Begin SQLPane = 
   End
   Begin DataPane = 
      Begin ParameterDefaults = ""
      End
   End
   Begin CriteriaPane = 
      Begin ColumnWidths = 11
         Column = 1440
         Alias = 900
         Table = 1170
         Output = 720
         Append = 1400
         NewValue = 1170
         SortType = 1350
         SortOrder = 1410
         GroupBy = 1350
         Filter = 1350
         Or = 1350
         Or = 1350
         Or = 1350
      End
   End
End
' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'VIEW',@level1name=N'InsuranceAnalytics'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_DiagramPaneCount', @value=2 , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'VIEW',@level1name=N'InsuranceAnalytics'
GO
USE [master]
GO
ALTER DATABASE [InsuranceDB] SET  READ_WRITE 
GO
