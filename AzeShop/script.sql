USE [master]
GO
/****** Object:  Database [dbAZEShop]    Script Date: 05/12/2023 17:40:34 ******/
CREATE DATABASE [dbAZEShop] ON  PRIMARY 
( NAME = N'dbAZEShop', FILENAME = N'c:\Program Files\Microsoft SQL Server\MSSQL10_50.SQLEXPRESS\MSSQL\DATA\dbAZEShop.mdf' , SIZE = 3072KB , MAXSIZE = UNLIMITED, FILEGROWTH = 1024KB )
 LOG ON 
( NAME = N'dbAZEShop_log', FILENAME = N'c:\Program Files\Microsoft SQL Server\MSSQL10_50.SQLEXPRESS\MSSQL\DATA\dbAZEShop_log.ldf' , SIZE = 1024KB , MAXSIZE = 2048GB , FILEGROWTH = 10%)
GO
ALTER DATABASE [dbAZEShop] SET COMPATIBILITY_LEVEL = 100
GO
IF (1 = FULLTEXTSERVICEPROPERTY('IsFullTextInstalled'))
begin
EXEC [dbAZEShop].[dbo].[sp_fulltext_database] @action = 'enable'
end
GO
ALTER DATABASE [dbAZEShop] SET ANSI_NULL_DEFAULT OFF
GO
ALTER DATABASE [dbAZEShop] SET ANSI_NULLS OFF
GO
ALTER DATABASE [dbAZEShop] SET ANSI_PADDING OFF
GO
ALTER DATABASE [dbAZEShop] SET ANSI_WARNINGS OFF
GO
ALTER DATABASE [dbAZEShop] SET ARITHABORT OFF
GO
ALTER DATABASE [dbAZEShop] SET AUTO_CLOSE OFF
GO
ALTER DATABASE [dbAZEShop] SET AUTO_CREATE_STATISTICS ON
GO
ALTER DATABASE [dbAZEShop] SET AUTO_SHRINK OFF
GO
ALTER DATABASE [dbAZEShop] SET AUTO_UPDATE_STATISTICS ON
GO
ALTER DATABASE [dbAZEShop] SET CURSOR_CLOSE_ON_COMMIT OFF
GO
ALTER DATABASE [dbAZEShop] SET CURSOR_DEFAULT  GLOBAL
GO
ALTER DATABASE [dbAZEShop] SET CONCAT_NULL_YIELDS_NULL OFF
GO
ALTER DATABASE [dbAZEShop] SET NUMERIC_ROUNDABORT OFF
GO
ALTER DATABASE [dbAZEShop] SET QUOTED_IDENTIFIER OFF
GO
ALTER DATABASE [dbAZEShop] SET RECURSIVE_TRIGGERS OFF
GO
ALTER DATABASE [dbAZEShop] SET  DISABLE_BROKER
GO
ALTER DATABASE [dbAZEShop] SET AUTO_UPDATE_STATISTICS_ASYNC OFF
GO
ALTER DATABASE [dbAZEShop] SET DATE_CORRELATION_OPTIMIZATION OFF
GO
ALTER DATABASE [dbAZEShop] SET TRUSTWORTHY OFF
GO
ALTER DATABASE [dbAZEShop] SET ALLOW_SNAPSHOT_ISOLATION OFF
GO
ALTER DATABASE [dbAZEShop] SET PARAMETERIZATION SIMPLE
GO
ALTER DATABASE [dbAZEShop] SET READ_COMMITTED_SNAPSHOT OFF
GO
ALTER DATABASE [dbAZEShop] SET HONOR_BROKER_PRIORITY OFF
GO
ALTER DATABASE [dbAZEShop] SET  READ_WRITE
GO
ALTER DATABASE [dbAZEShop] SET RECOVERY SIMPLE
GO
ALTER DATABASE [dbAZEShop] SET  MULTI_USER
GO
ALTER DATABASE [dbAZEShop] SET PAGE_VERIFY CHECKSUM
GO
ALTER DATABASE [dbAZEShop] SET DB_CHAINING OFF
GO
USE [dbAZEShop]
GO
/****** Object:  Table [dbo].[TransactStatus]    Script Date: 05/12/2023 17:40:34 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[TransactStatus](
	[TransactStatusID] [int] NOT NULL,
	[Status] [nvarchar](100) NOT NULL,
	[Description] [nvarchar](max) NULL,
 CONSTRAINT [PK_TransactStatus] PRIMARY KEY CLUSTERED 
(
	[TransactStatusID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
INSERT [dbo].[TransactStatus] ([TransactStatusID], [Status], [Description]) VALUES (1, N'Chờ xử lý', NULL)
INSERT [dbo].[TransactStatus] ([TransactStatusID], [Status], [Description]) VALUES (2, N'Xác thực', NULL)
INSERT [dbo].[TransactStatus] ([TransactStatusID], [Status], [Description]) VALUES (3, N'Đã xuất kho', NULL)
INSERT [dbo].[TransactStatus] ([TransactStatusID], [Status], [Description]) VALUES (4, N'Đang giao hàng', NULL)
INSERT [dbo].[TransactStatus] ([TransactStatusID], [Status], [Description]) VALUES (5, N'Hoàn thành', NULL)
INSERT [dbo].[TransactStatus] ([TransactStatusID], [Status], [Description]) VALUES (6, N'Bị hủy', NULL)
/****** Object:  Table [dbo].[Roles]    Script Date: 05/12/2023 17:40:34 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Roles](
	[RoleID] [int] IDENTITY(1,1) NOT NULL,
	[RoleName] [nvarchar](30) NULL,
	[Description] [nvarchar](200) NULL,
 CONSTRAINT [PK_Roles_1] PRIMARY KEY CLUSTERED 
(
	[RoleID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
SET IDENTITY_INSERT [dbo].[Roles] ON
INSERT [dbo].[Roles] ([RoleID], [RoleName], [Description]) VALUES (3, N'Admin', NULL)
INSERT [dbo].[Roles] ([RoleID], [RoleName], [Description]) VALUES (4, N'Sale', NULL)
INSERT [dbo].[Roles] ([RoleID], [RoleName], [Description]) VALUES (7, N'SuperAdmin', NULL)
SET IDENTITY_INSERT [dbo].[Roles] OFF
/****** Object:  Table [dbo].[Pages]    Script Date: 05/12/2023 17:40:34 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Pages](
	[PageID] [nchar](30) NOT NULL,
	[PageName] [nvarchar](255) NOT NULL,
	[Contents] [nvarchar](max) NULL,
	[Thumb] [nvarchar](255) NOT NULL,
	[Published] [bit] NOT NULL,
	[Tittle] [nvarchar](255) NULL,
	[Alias] [nvarchar](255) NULL,
	[CreateDate] [datetime] NOT NULL,
	[Ordering] [int] NULL,
	[MetaDesc] [nvarchar](255) NULL,
	[MetaKey] [nvarchar](255) NULL,
 CONSTRAINT [PK_Pages] PRIMARY KEY CLUSTERED 
(
	[PageID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Cities]    Script Date: 05/12/2023 17:40:34 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Cities](
	[CityID] [int] NOT NULL,
	[CityName] [nvarchar](100) NOT NULL,
	[DeliveryPrice] [money] NOT NULL,
 CONSTRAINT [PK_Cities] PRIMARY KEY CLUSTERED 
(
	[CityID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
INSERT [dbo].[Cities] ([CityID], [CityName], [DeliveryPrice]) VALUES (1, N'Hà Nội', 50000.0000)
INSERT [dbo].[Cities] ([CityID], [CityName], [DeliveryPrice]) VALUES (2, N'Hồ Chí Minh', 60000.0000)
/****** Object:  Table [dbo].[Categories]    Script Date: 05/12/2023 17:40:34 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Categories](
	[CatID] [int] IDENTITY(1,1) NOT NULL,
	[CatName] [nvarchar](50) NOT NULL,
	[Description] [nvarchar](300) NULL,
	[Published] [bit] NOT NULL,
	[Thumb] [nvarchar](200) NULL,
	[Title] [nvarchar](250) NULL,
	[Alias] [nvarchar](250) NULL,
 CONSTRAINT [PK_Categories] PRIMARY KEY CLUSTERED 
(
	[CatID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
SET IDENTITY_INSERT [dbo].[Categories] ON
INSERT [dbo].[Categories] ([CatID], [CatName], [Description], [Published], [Thumb], [Title], [Alias]) VALUES (112, N'Laptop', NULL, 1, N'laptop-638061055177758700.jpg', N'Laptop', N'laptop')
INSERT [dbo].[Categories] ([CatID], [CatName], [Description], [Published], [Thumb], [Title], [Alias]) VALUES (113, N'Điện thoại', NULL, 1, N'dien-thoai-638109725763863522.jpg', N'Xe', N'xe')
INSERT [dbo].[Categories] ([CatID], [CatName], [Description], [Published], [Thumb], [Title], [Alias]) VALUES (116, N'Nội thất', NULL, 1, N'noi-that-638106178139149187.jpg', N'Nội thất', N'noi-that')
SET IDENTITY_INSERT [dbo].[Categories] OFF
/****** Object:  Table [dbo].[Districts]    Script Date: 05/12/2023 17:40:34 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Districts](
	[DistrictID] [int] NOT NULL,
	[CityID] [int] NOT NULL,
	[DistrictName] [nvarchar](100) NOT NULL,
 CONSTRAINT [PK_Districts] PRIMARY KEY CLUSTERED 
(
	[DistrictID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
INSERT [dbo].[Districts] ([DistrictID], [CityID], [DistrictName]) VALUES (1, 1, N'Gia Lâm')
INSERT [dbo].[Districts] ([DistrictID], [CityID], [DistrictName]) VALUES (2, 2, N'Quận 1')
INSERT [dbo].[Districts] ([DistrictID], [CityID], [DistrictName]) VALUES (3, 2, N'Quận 2')
INSERT [dbo].[Districts] ([DistrictID], [CityID], [DistrictName]) VALUES (4, 2, N'Quận 3')
INSERT [dbo].[Districts] ([DistrictID], [CityID], [DistrictName]) VALUES (5, 1, N'Long Biên')
INSERT [dbo].[Districts] ([DistrictID], [CityID], [DistrictName]) VALUES (6, 1, N'Thanh Trì')
/****** Object:  Table [dbo].[Customers]    Script Date: 05/12/2023 17:40:34 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Customers](
	[CustomerID] [int] IDENTITY(1,1) NOT NULL,
	[Avatar] [nvarchar](250) NULL,
	[FullName] [nvarchar](50) NOT NULL,
	[Birthday] [datetime] NULL,
	[Address] [nvarchar](250) NULL,
	[CityID] [int] NOT NULL,
	[Email] [nvarchar](150) NOT NULL,
	[Phone] [nvarchar](15) NOT NULL,
	[CreateDate] [datetime] NULL,
	[Password] [nvarchar](50) NOT NULL,
	[LastLogin] [datetime] NULL,
	[Active] [bit] NOT NULL,
 CONSTRAINT [PK_Customers] PRIMARY KEY CLUSTERED 
(
	[CustomerID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
SET IDENTITY_INSERT [dbo].[Customers] ON
INSERT [dbo].[Customers] ([CustomerID], [Avatar], [FullName], [Birthday], [Address], [CityID], [Email], [Phone], [CreateDate], [Password], [LastLogin], [Active]) VALUES (1, NULL, N'Vũ Văn Thuyên', NULL, N'Xóm 5', 2, N'thuyentruong115@gmail.com', N'0989632420   ', NULL, N'1', CAST(0x0000AFA0009B0843 AS DateTime), 1)
INSERT [dbo].[Customers] ([CustomerID], [Avatar], [FullName], [Birthday], [Address], [CityID], [Email], [Phone], [CreateDate], [Password], [LastLogin], [Active]) VALUES (2, N'default.png', N'Vũ Văn Thuyên', NULL, N'Số 32', 1, N'vuvanthuyen@gmail.com', N'0986354108', CAST(0x00008CC500000000 AS DateTime), N'1', CAST(0x0000AD2200000000 AS DateTime), 1)
INSERT [dbo].[Customers] ([CustomerID], [Avatar], [FullName], [Birthday], [Address], [CityID], [Email], [Phone], [CreateDate], [Password], [LastLogin], [Active]) VALUES (44, N'default.png', N'Thuyen Vu', NULL, NULL, 1, N'thuyentruog12912@gmail.com', N'0981218281', CAST(0x0000AF6800A42A64 AS DateTime), N'1', NULL, 1)
INSERT [dbo].[Customers] ([CustomerID], [Avatar], [FullName], [Birthday], [Address], [CityID], [Email], [Phone], [CreateDate], [Password], [LastLogin], [Active]) VALUES (45, N'default.png', N'Vũ Thuyên', NULL, NULL, 1, N'thuyentruong39@gmail.com', N'0986354108', CAST(0x0000AF6800AFAAE5 AS DateTime), N'1', NULL, 1)
INSERT [dbo].[Customers] ([CustomerID], [Avatar], [FullName], [Birthday], [Address], [CityID], [Email], [Phone], [CreateDate], [Password], [LastLogin], [Active]) VALUES (46, N'default.png', N'Thuyên', NULL, NULL, 1, N'thuyentruong112@gmail.com', N'0989632422', CAST(0x0000AF6800CE6CF5 AS DateTime), N'1', NULL, 1)
INSERT [dbo].[Customers] ([CustomerID], [Avatar], [FullName], [Birthday], [Address], [CityID], [Email], [Phone], [CreateDate], [Password], [LastLogin], [Active]) VALUES (47, N'default.png', N'Vũ Bâm', NULL, N'Số 3', 1, N'thuyentruong99@gmail.com', N'0989639931', CAST(0x0000AF6F00C078CA AS DateTime), N'1', NULL, 1)
INSERT [dbo].[Customers] ([CustomerID], [Avatar], [FullName], [Birthday], [Address], [CityID], [Email], [Phone], [CreateDate], [Password], [LastLogin], [Active]) VALUES (48, N'default.png', N'Giang', NULL, N'2', 1, N'thuyentruong1152@gmail.com', N'0982272722', CAST(0x0000AF8E011E640E AS DateTime), N'1', CAST(0x0000AF6F0166A783 AS DateTime), 1)
INSERT [dbo].[Customers] ([CustomerID], [Avatar], [FullName], [Birthday], [Address], [CityID], [Email], [Phone], [CreateDate], [Password], [LastLogin], [Active]) VALUES (49, N'default.png', N'Nguyễn Xuân Ngát', NULL, N'Xóm 5', 2, N'nguyenxuanngat@gmail.com', N'0987212242', CAST(0x0000AF9300DCAA02 AS DateTime), N'1', CAST(0x0000AF9F01561A33 AS DateTime), 1)
INSERT [dbo].[Customers] ([CustomerID], [Avatar], [FullName], [Birthday], [Address], [CityID], [Email], [Phone], [CreateDate], [Password], [LastLogin], [Active]) VALUES (50, N'default.png', N'Tuấn', NULL, N'6525', 1, N'thuyentruong15@gmail.com', N'0982727212', CAST(0x0000AE2A000C70A2 AS DateTime), N'1', CAST(0x0000AF97000C70A2 AS DateTime), 1)
INSERT [dbo].[Customers] ([CustomerID], [Avatar], [FullName], [Birthday], [Address], [CityID], [Email], [Phone], [CreateDate], [Password], [LastLogin], [Active]) VALUES (51, N'default.png', N'Vũ', NULL, N'BN', 2, N'thuyentruong11522@gmail.com', N'0987727722', CAST(0x0000AE2A00DA96F5 AS DateTime), N'1', CAST(0x0000AF9700DA96F5 AS DateTime), 1)
INSERT [dbo].[Customers] ([CustomerID], [Avatar], [FullName], [Birthday], [Address], [CityID], [Email], [Phone], [CreateDate], [Password], [LastLogin], [Active]) VALUES (52, N'default.png', N'Vũ Quốc Tuấn', NULL, N'Xóm 1', 1, N'thuyentruong1125@gmail.com', N'0989632425', CAST(0x0000AFA0009C00F4 AS DateTime), N'17112002', CAST(0x0000AFA0009C00F4 AS DateTime), 1)
SET IDENTITY_INSERT [dbo].[Customers] OFF
/****** Object:  Table [dbo].[Accounts]    Script Date: 05/12/2023 17:40:34 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Accounts](
	[AccountID] [int] IDENTITY(1,1) NOT NULL,
	[AccountName] [nvarchar](50) NOT NULL,
	[Password] [nvarchar](50) NOT NULL,
	[Active] [bit] NOT NULL,
	[RoleID] [int] NOT NULL,
	[LastLogin] [datetime] NULL,
	[CreateDate] [datetime] NULL,
 CONSTRAINT [PK_Accounts] PRIMARY KEY CLUSTERED 
(
	[AccountID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY],
 CONSTRAINT [IX_AccountName] UNIQUE NONCLUSTERED 
(
	[AccountName] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
SET IDENTITY_INSERT [dbo].[Accounts] ON
INSERT [dbo].[Accounts] ([AccountID], [AccountName], [Password], [Active], [RoleID], [LastLogin], [CreateDate]) VALUES (1, N'xuanngat', N'1', 0, 3, CAST(0x0000AF99016E5B3B AS DateTime), CAST(0x000095A200000000 AS DateTime))
INSERT [dbo].[Accounts] ([AccountID], [AccountName], [Password], [Active], [RoleID], [LastLogin], [CreateDate]) VALUES (3, N'vuvanthuyen', N'1', 0, 7, CAST(0x0000AFA000BE20B0 AS DateTime), NULL)
INSERT [dbo].[Accounts] ([AccountID], [AccountName], [Password], [Active], [RoleID], [LastLogin], [CreateDate]) VALUES (97, N'quangthien', N'1', 0, 4, CAST(0x0000AFA00098C175 AS DateTime), NULL)
SET IDENTITY_INSERT [dbo].[Accounts] OFF
/****** Object:  Table [dbo].[Products]    Script Date: 05/12/2023 17:40:34 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Products](
	[ProductID] [int] IDENTITY(1,1) NOT NULL,
	[ProductName] [nvarchar](250) NOT NULL,
	[Description] [nvarchar](max) NULL,
	[CatID] [int] NOT NULL,
	[Price] [money] NOT NULL,
	[Discount] [int] NOT NULL,
	[UnitsInStock] [int] NOT NULL,
	[Thumb] [nvarchar](250) NOT NULL,
	[DateCreated] [datetime] NULL,
	[DateModified] [datetime] NULL,
	[BestSeller] [bit] NOT NULL,
	[HomeFlag] [bit] NOT NULL,
	[Active] [bit] NOT NULL,
	[ShortDesc] [nvarchar](250) NULL,
	[Title] [nvarchar](250) NULL,
	[Alias] [nvarchar](250) NULL,
	[MetaDesc] [nvarchar](250) NULL,
	[MetaKey] [nvarchar](250) NULL,
	[Tags] [nvarchar](max) NULL,
 CONSTRAINT [PK_Products] PRIMARY KEY CLUSTERED 
(
	[ProductID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
SET IDENTITY_INSERT [dbo].[Products] ON
INSERT [dbo].[Products] ([ProductID], [ProductName], [Description], [CatID], [Price], [Discount], [UnitsInStock], [Thumb], [DateCreated], [DateModified], [BestSeller], [HomeFlag], [Active], [ShortDesc], [Title], [Alias], [MetaDesc], [MetaKey], [Tags]) VALUES (28, N'Macbook Pro 2020', NULL, 112, 3355950.0000, 5, 78, N'macbook-pro-2020-638061069681022885.jpg', CAST(0x0000AF6500F0A320 AS DateTime), CAST(0x0000AF9D0174E525 AS DateTime), 1, 1, 1, NULL, NULL, N'macbook-pro-2020', NULL, NULL, NULL)
INSERT [dbo].[Products] ([ProductID], [ProductName], [Description], [CatID], [Price], [Discount], [UnitsInStock], [Thumb], [DateCreated], [DateModified], [BestSeller], [HomeFlag], [Active], [ShortDesc], [Title], [Alias], [MetaDesc], [MetaKey], [Tags]) VALUES (29, N'Laptop Asus Plus', NULL, 112, 16480000.0000, 10, 59, N'laptop-asus-plus-638061070857852771.jpg', CAST(0x0000AF6500F12C3C AS DateTime), CAST(0x0000AF9E01567138 AS DateTime), 1, 1, 1, NULL, NULL, N'laptop-asus-plus', NULL, NULL, NULL)
INSERT [dbo].[Products] ([ProductID], [ProductName], [Description], [CatID], [Price], [Discount], [UnitsInStock], [Thumb], [DateCreated], [DateModified], [BestSeller], [HomeFlag], [Active], [ShortDesc], [Title], [Alias], [MetaDesc], [MetaKey], [Tags]) VALUES (31, N'Bàn Làm Việc', NULL, 116, 1505005.0000, 10, 8, N'ban-lam-viec-638106177679907944.jpg', CAST(0x0000AF7300E4B40C AS DateTime), CAST(0x0000AF99014308E6 AS DateTime), 1, 1, 1, NULL, NULL, N'ban-lam-viec', NULL, NULL, NULL)
INSERT [dbo].[Products] ([ProductID], [ProductName], [Description], [CatID], [Price], [Discount], [UnitsInStock], [Thumb], [DateCreated], [DateModified], [BestSeller], [HomeFlag], [Active], [ShortDesc], [Title], [Alias], [MetaDesc], [MetaKey], [Tags]) VALUES (32, N'Iphone', NULL, 113, 14482014.3885, 0, 133, N'iphone-638073488159086037.png', CAST(0x0000AF73018441B0 AS DateTime), CAST(0x0000AF73018441B0 AS DateTime), 1, 1, 1, NULL, NULL, N'iphone', NULL, NULL, NULL)
INSERT [dbo].[Products] ([ProductID], [ProductName], [Description], [CatID], [Price], [Discount], [UnitsInStock], [Thumb], [DateCreated], [DateModified], [BestSeller], [HomeFlag], [Active], [ShortDesc], [Title], [Alias], [MetaDesc], [MetaKey], [Tags]) VALUES (33, N'Ipad', NULL, 113, 18181818.1818, 30, 11, N'ipad-638106182779407477.jpg', CAST(0x0000AF7301846290 AS DateTime), CAST(0x0000AF9901455E77 AS DateTime), 1, 1, 1, NULL, NULL, N'ipad', NULL, NULL, NULL)
INSERT [dbo].[Products] ([ProductID], [ProductName], [Description], [CatID], [Price], [Discount], [UnitsInStock], [Thumb], [DateCreated], [DateModified], [BestSeller], [HomeFlag], [Active], [ShortDesc], [Title], [Alias], [MetaDesc], [MetaKey], [Tags]) VALUES (35, N'Laptop Lenovo', NULL, 112, 20000000.0000, 25, 14, N'laptop-lenovo-638106183569480768.jpg', CAST(0x0000AF730184DFF4 AS DateTime), CAST(0x0000AF990145BB0F AS DateTime), 1, 1, 1, NULL, NULL, N'laptop-lenovo', NULL, NULL, NULL)
SET IDENTITY_INSERT [dbo].[Products] OFF
/****** Object:  Table [dbo].[Inventory]    Script Date: 05/12/2023 17:40:34 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Inventory](
	[MonthAndYear] [nvarchar](10) NOT NULL,
	[ProductID] [int] NOT NULL,
	[Import_Amount] [int] NOT NULL,
	[Export_Amount] [int] NOT NULL,
	[Import_Money] [money] NOT NULL,
	[Export_Money] [money] NOT NULL,
 CONSTRAINT [PK_Inventory] PRIMARY KEY CLUSTERED 
(
	[MonthAndYear] ASC,
	[ProductID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
INSERT [dbo].[Inventory] ([MonthAndYear], [ProductID], [Import_Amount], [Export_Amount], [Import_Money], [Export_Money]) VALUES (N'2/2022', 33, 10, 4, 150000000.0000, 56000000.0000)
INSERT [dbo].[Inventory] ([MonthAndYear], [ProductID], [Import_Amount], [Export_Amount], [Import_Money], [Export_Money]) VALUES (N'2/2023', 28, 50, 2, 1200400.0000, 6376305.0000)
INSERT [dbo].[Inventory] ([MonthAndYear], [ProductID], [Import_Amount], [Export_Amount], [Import_Money], [Export_Money]) VALUES (N'2/2023', 29, 0, 1, 0.0000, 14832000.0000)
INSERT [dbo].[Inventory] ([MonthAndYear], [ProductID], [Import_Amount], [Export_Amount], [Import_Money], [Export_Money]) VALUES (N'2/2023', 32, 120, 4, 1440000000.0000, 60000000.0000)
/****** Object:  Table [dbo].[Wards]    Script Date: 05/12/2023 17:40:34 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Wards](
	[WardID] [int] NOT NULL,
	[DistrictID] [int] NOT NULL,
	[WardName] [nvarchar](100) NOT NULL,
 CONSTRAINT [PK_Wards] PRIMARY KEY CLUSTERED 
(
	[WardID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
INSERT [dbo].[Wards] ([WardID], [DistrictID], [WardName]) VALUES (1, 1, N'Ninh Hiệp')
INSERT [dbo].[Wards] ([WardID], [DistrictID], [WardName]) VALUES (2, 1, N'Ninh Xá')
INSERT [dbo].[Wards] ([WardID], [DistrictID], [WardName]) VALUES (3, 2, N'Bình Minh')
INSERT [dbo].[Wards] ([WardID], [DistrictID], [WardName]) VALUES (4, 2, N'Bình Sơn')
/****** Object:  Table [dbo].[Receipt]    Script Date: 05/12/2023 17:40:34 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Receipt](
	[ReceiptID] [int] IDENTITY(1,1) NOT NULL,
	[ProductID] [int] NOT NULL,
	[Price] [money] NOT NULL,
	[DateCreated] [datetime] NULL,
	[Amount] [int] NOT NULL,
 CONSTRAINT [PK_Receipt] PRIMARY KEY CLUSTERED 
(
	[ReceiptID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
SET IDENTITY_INSERT [dbo].[Receipt] ON
INSERT [dbo].[Receipt] ([ReceiptID], [ProductID], [Price], [DateCreated], [Amount]) VALUES (26, 32, 12000000.0000, CAST(0x0000AF9E01447F67 AS DateTime), 120)
INSERT [dbo].[Receipt] ([ReceiptID], [ProductID], [Price], [DateCreated], [Amount]) VALUES (27, 33, 15000000.0000, CAST(0x0000AF9E014CB20D AS DateTime), 10)
SET IDENTITY_INSERT [dbo].[Receipt] OFF
/****** Object:  Table [dbo].[Orders]    Script Date: 05/12/2023 17:40:34 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Orders](
	[OrderID] [int] IDENTITY(1,1) NOT NULL,
	[CustomerID] [int] NOT NULL,
	[FullName] [nvarchar](50) NOT NULL,
	[OrderDate] [datetime] NOT NULL,
	[Phone] [nvarchar](15) NOT NULL,
	[TransactStatusID] [int] NOT NULL,
	[DeliveryDate] [datetime] NULL,
	[PaymentDate] [datetime] NULL,
	[Note] [nvarchar](max) NULL,
	[Address] [nvarchar](max) NOT NULL,
	[WardID] [int] NOT NULL,
	[TotalMoney] [money] NOT NULL,
 CONSTRAINT [PK_Orders] PRIMARY KEY CLUSTERED 
(
	[OrderID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
SET IDENTITY_INSERT [dbo].[Orders] ON
INSERT [dbo].[Orders] ([OrderID], [CustomerID], [FullName], [OrderDate], [Phone], [TransactStatusID], [DeliveryDate], [PaymentDate], [Note], [Address], [WardID], [TotalMoney]) VALUES (4, 1, N'Vũ Văn Thuyên', CAST(0x0000AF690163E5D3 AS DateTime), N'0989632420', 5, CAST(0x0000AEB200000000 AS DateTime), CAST(0x0000AFC300000000 AS DateTime), NULL, N'Số 1', 1, 30050000.0000)
INSERT [dbo].[Orders] ([OrderID], [CustomerID], [FullName], [OrderDate], [Phone], [TransactStatusID], [DeliveryDate], [PaymentDate], [Note], [Address], [WardID], [TotalMoney]) VALUES (5, 1, N'Vũ Văn Thuyên', CAST(0x0000AF6901694277 AS DateTime), N'0989632420', 5, CAST(0x0000AF750175F0B7 AS DateTime), CAST(0x0000AFA700000000 AS DateTime), N'jjajsj', N'Số 1', 1, 30050000.0000)
INSERT [dbo].[Orders] ([OrderID], [CustomerID], [FullName], [OrderDate], [Phone], [TransactStatusID], [DeliveryDate], [PaymentDate], [Note], [Address], [WardID], [TotalMoney]) VALUES (7, 1, N'Vũ Văn Thuyên', CAST(0x0000AF9700D88D85 AS DateTime), N'0989632420', 5, CAST(0x0000AF750175F0B7 AS DateTime), CAST(0x0000AF8200000000 AS DateTime), NULL, N'Xóm 5', 2, 15354504.5000)
INSERT [dbo].[Orders] ([OrderID], [CustomerID], [FullName], [OrderDate], [Phone], [TransactStatusID], [DeliveryDate], [PaymentDate], [Note], [Address], [WardID], [TotalMoney]) VALUES (10, 1, N'Vũ Văn Thuyên', CAST(0x0000AF980169B872 AS DateTime), N'0989632420', 5, CAST(0x0000AF9E014C7BC1 AS DateTime), CAST(0x0000AF9E014C7B7E AS DateTime), NULL, N'Xóm 4', 1, 58050000.0000)
INSERT [dbo].[Orders] ([OrderID], [CustomerID], [FullName], [OrderDate], [Phone], [TransactStatusID], [DeliveryDate], [PaymentDate], [Note], [Address], [WardID], [TotalMoney]) VALUES (11, 1, N'Vũ Văn Thuyên', CAST(0x0000AF9901590EA7 AS DateTime), N'0989632420', 5, CAST(0x0000AF99015983EF AS DateTime), CAST(0x0000AE2C015983EF AS DateTime), NULL, N'Xóm 5', 1, 67904504.5000)
INSERT [dbo].[Orders] ([OrderID], [CustomerID], [FullName], [OrderDate], [Phone], [TransactStatusID], [DeliveryDate], [PaymentDate], [Note], [Address], [WardID], [TotalMoney]) VALUES (12, 1, N'Vũ Văn Thuyên', CAST(0x0000AF9901595B96 AS DateTime), N'0989632420', 5, CAST(0x0000AF9901598950 AS DateTime), CAST(0x0000AF7A01598950 AS DateTime), NULL, N'Xóm 5', 2, 89050000.0000)
INSERT [dbo].[Orders] ([OrderID], [CustomerID], [FullName], [OrderDate], [Phone], [TransactStatusID], [DeliveryDate], [PaymentDate], [Note], [Address], [WardID], [TotalMoney]) VALUES (14, 1, N'Vũ Văn Thuyên', CAST(0x0000AF9F01540C14 AS DateTime), N'0989632420', 6, NULL, NULL, NULL, N'Xóm 5', 1, 43496043.1655)
INSERT [dbo].[Orders] ([OrderID], [CustomerID], [FullName], [OrderDate], [Phone], [TransactStatusID], [DeliveryDate], [PaymentDate], [Note], [Address], [WardID], [TotalMoney]) VALUES (15, 1, N'Vũ Văn Thuyên', CAST(0x0000AFA0009B40EA AS DateTime), N'0989632420', 5, CAST(0x0000AFA0009CB19F AS DateTime), CAST(0x0000AFA0009CB17E AS DateTime), NULL, N'Xóm 5', 1, 21258305.0000)
SET IDENTITY_INSERT [dbo].[Orders] OFF
/****** Object:  Table [dbo].[OrderDetails]    Script Date: 05/12/2023 17:40:34 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[OrderDetails](
	[OrderID] [int] NOT NULL,
	[ProductID] [int] NOT NULL,
	[Amount] [int] NOT NULL,
	[Discount] [int] NOT NULL,
	[CreateDate] [datetime] NULL,
	[Price] [money] NOT NULL,
 CONSTRAINT [PK_OrderDetails] PRIMARY KEY CLUSTERED 
(
	[OrderID] ASC,
	[ProductID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
INSERT [dbo].[OrderDetails] ([OrderID], [ProductID], [Amount], [Discount], [CreateDate], [Price]) VALUES (4, 28, 1, 0, CAST(0x0000AF690163E636 AS DateTime), 30000000.0000)
INSERT [dbo].[OrderDetails] ([OrderID], [ProductID], [Amount], [Discount], [CreateDate], [Price]) VALUES (5, 28, 1, 0, CAST(0x0000AF69016942FD AS DateTime), 30000000.0000)
INSERT [dbo].[OrderDetails] ([OrderID], [ProductID], [Amount], [Discount], [CreateDate], [Price]) VALUES (7, 31, 10, 10, CAST(0x0000AF9700D88DAE AS DateTime), 1505005.0000)
INSERT [dbo].[OrderDetails] ([OrderID], [ProductID], [Amount], [Discount], [CreateDate], [Price]) VALUES (7, 33, 6, 30, CAST(0x0000AF9700D88DA7 AS DateTime), 20000000.0000)
INSERT [dbo].[OrderDetails] ([OrderID], [ProductID], [Amount], [Discount], [CreateDate], [Price]) VALUES (10, 32, 2, 0, CAST(0x0000AF980169B89A AS DateTime), 15000000.0000)
INSERT [dbo].[OrderDetails] ([OrderID], [ProductID], [Amount], [Discount], [CreateDate], [Price]) VALUES (10, 33, 2, 30, CAST(0x0000AF980169B893 AS DateTime), 20000000.0000)
INSERT [dbo].[OrderDetails] ([OrderID], [ProductID], [Amount], [Discount], [CreateDate], [Price]) VALUES (11, 29, 1, 10, CAST(0x0000AF9901590ED8 AS DateTime), 25000000.0000)
INSERT [dbo].[OrderDetails] ([OrderID], [ProductID], [Amount], [Discount], [CreateDate], [Price]) VALUES (11, 31, 1, 10, CAST(0x0000AF9901590ED8 AS DateTime), 1505005.0000)
INSERT [dbo].[OrderDetails] ([OrderID], [ProductID], [Amount], [Discount], [CreateDate], [Price]) VALUES (11, 32, 1, 0, CAST(0x0000AF9901590ED8 AS DateTime), 15000000.0000)
INSERT [dbo].[OrderDetails] ([OrderID], [ProductID], [Amount], [Discount], [CreateDate], [Price]) VALUES (11, 33, 1, 30, CAST(0x0000AF9901590ED0 AS DateTime), 20000000.0000)
INSERT [dbo].[OrderDetails] ([OrderID], [ProductID], [Amount], [Discount], [CreateDate], [Price]) VALUES (11, 35, 1, 25, CAST(0x0000AF9901590ED8 AS DateTime), 20000000.0000)
INSERT [dbo].[OrderDetails] ([OrderID], [ProductID], [Amount], [Discount], [CreateDate], [Price]) VALUES (12, 33, 1, 30, CAST(0x0000AF9901595B9A AS DateTime), 20000000.0000)
INSERT [dbo].[OrderDetails] ([OrderID], [ProductID], [Amount], [Discount], [CreateDate], [Price]) VALUES (12, 35, 5, 25, CAST(0x0000AF9901595B9A AS DateTime), 20000000.0000)
INSERT [dbo].[OrderDetails] ([OrderID], [ProductID], [Amount], [Discount], [CreateDate], [Price]) VALUES (14, 32, 3, 0, CAST(0x0000AF9F01540C22 AS DateTime), 14482014.3885)
INSERT [dbo].[OrderDetails] ([OrderID], [ProductID], [Amount], [Discount], [CreateDate], [Price]) VALUES (15, 28, 2, 5, CAST(0x0000AFA0009B410D AS DateTime), 3355950.0000)
INSERT [dbo].[OrderDetails] ([OrderID], [ProductID], [Amount], [Discount], [CreateDate], [Price]) VALUES (15, 29, 1, 10, CAST(0x0000AFA0009B4112 AS DateTime), 16480000.0000)
/****** Object:  ForeignKey [FK_Districts_Cities]    Script Date: 05/12/2023 17:40:34 ******/
ALTER TABLE [dbo].[Districts]  WITH CHECK ADD  CONSTRAINT [FK_Districts_Cities] FOREIGN KEY([CityID])
REFERENCES [dbo].[Cities] ([CityID])
GO
ALTER TABLE [dbo].[Districts] CHECK CONSTRAINT [FK_Districts_Cities]
GO
/****** Object:  ForeignKey [FK_Customers_Cities]    Script Date: 05/12/2023 17:40:34 ******/
ALTER TABLE [dbo].[Customers]  WITH CHECK ADD  CONSTRAINT [FK_Customers_Cities] FOREIGN KEY([CityID])
REFERENCES [dbo].[Cities] ([CityID])
GO
ALTER TABLE [dbo].[Customers] CHECK CONSTRAINT [FK_Customers_Cities]
GO
/****** Object:  ForeignKey [FK_Roles_Accounts]    Script Date: 05/12/2023 17:40:34 ******/
ALTER TABLE [dbo].[Accounts]  WITH CHECK ADD  CONSTRAINT [FK_Roles_Accounts] FOREIGN KEY([RoleID])
REFERENCES [dbo].[Roles] ([RoleID])
GO
ALTER TABLE [dbo].[Accounts] CHECK CONSTRAINT [FK_Roles_Accounts]
GO
/****** Object:  ForeignKey [FK_Products_Categories]    Script Date: 05/12/2023 17:40:34 ******/
ALTER TABLE [dbo].[Products]  WITH CHECK ADD  CONSTRAINT [FK_Products_Categories] FOREIGN KEY([CatID])
REFERENCES [dbo].[Categories] ([CatID])
GO
ALTER TABLE [dbo].[Products] CHECK CONSTRAINT [FK_Products_Categories]
GO
/****** Object:  ForeignKey [FK_Inventory_Products]    Script Date: 05/12/2023 17:40:34 ******/
ALTER TABLE [dbo].[Inventory]  WITH CHECK ADD  CONSTRAINT [FK_Inventory_Products] FOREIGN KEY([ProductID])
REFERENCES [dbo].[Products] ([ProductID])
GO
ALTER TABLE [dbo].[Inventory] CHECK CONSTRAINT [FK_Inventory_Products]
GO
/****** Object:  ForeignKey [FK_Wards_Districts]    Script Date: 05/12/2023 17:40:34 ******/
ALTER TABLE [dbo].[Wards]  WITH CHECK ADD  CONSTRAINT [FK_Wards_Districts] FOREIGN KEY([DistrictID])
REFERENCES [dbo].[Districts] ([DistrictID])
GO
ALTER TABLE [dbo].[Wards] CHECK CONSTRAINT [FK_Wards_Districts]
GO
/****** Object:  ForeignKey [FK_Receipt_Products]    Script Date: 05/12/2023 17:40:34 ******/
ALTER TABLE [dbo].[Receipt]  WITH CHECK ADD  CONSTRAINT [FK_Receipt_Products] FOREIGN KEY([ProductID])
REFERENCES [dbo].[Products] ([ProductID])
GO
ALTER TABLE [dbo].[Receipt] CHECK CONSTRAINT [FK_Receipt_Products]
GO
/****** Object:  ForeignKey [FK_Orders_Customers]    Script Date: 05/12/2023 17:40:34 ******/
ALTER TABLE [dbo].[Orders]  WITH CHECK ADD  CONSTRAINT [FK_Orders_Customers] FOREIGN KEY([CustomerID])
REFERENCES [dbo].[Customers] ([CustomerID])
GO
ALTER TABLE [dbo].[Orders] CHECK CONSTRAINT [FK_Orders_Customers]
GO
/****** Object:  ForeignKey [FK_Orders_TransactStatus]    Script Date: 05/12/2023 17:40:34 ******/
ALTER TABLE [dbo].[Orders]  WITH CHECK ADD  CONSTRAINT [FK_Orders_TransactStatus] FOREIGN KEY([TransactStatusID])
REFERENCES [dbo].[TransactStatus] ([TransactStatusID])
GO
ALTER TABLE [dbo].[Orders] CHECK CONSTRAINT [FK_Orders_TransactStatus]
GO
/****** Object:  ForeignKey [FK_Orders_Wards]    Script Date: 05/12/2023 17:40:34 ******/
ALTER TABLE [dbo].[Orders]  WITH CHECK ADD  CONSTRAINT [FK_Orders_Wards] FOREIGN KEY([WardID])
REFERENCES [dbo].[Wards] ([WardID])
GO
ALTER TABLE [dbo].[Orders] CHECK CONSTRAINT [FK_Orders_Wards]
GO
/****** Object:  ForeignKey [FK_OrderDetails_Orders]    Script Date: 05/12/2023 17:40:34 ******/
ALTER TABLE [dbo].[OrderDetails]  WITH CHECK ADD  CONSTRAINT [FK_OrderDetails_Orders] FOREIGN KEY([OrderID])
REFERENCES [dbo].[Orders] ([OrderID])
GO
ALTER TABLE [dbo].[OrderDetails] CHECK CONSTRAINT [FK_OrderDetails_Orders]
GO
/****** Object:  ForeignKey [FK_OrderDetails_Products]    Script Date: 05/12/2023 17:40:34 ******/
ALTER TABLE [dbo].[OrderDetails]  WITH CHECK ADD  CONSTRAINT [FK_OrderDetails_Products] FOREIGN KEY([ProductID])
REFERENCES [dbo].[Products] ([ProductID])
GO
ALTER TABLE [dbo].[OrderDetails] CHECK CONSTRAINT [FK_OrderDetails_Products]
GO
