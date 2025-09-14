CREATE SCHEMA auth;

go
 
CREATE TABLE auth.[auth_accounts](

    id INT PRIMARY KEY IDENTITY,
	[username] [nvarchar](255) NULL,
	[password_hash] [nvarchar](2000) NULL, 
	[created_by] [nvarchar](255) NULL,
	[created_date] [datetime] NULL,
	[modified_by] [nvarchar](255) NULL, 
	[modified_date] [datetime] NULL
) ON [PRIMARY]
GO


CREATE SEQUENCE [dbo].[adm_employee_seq] 
 AS [bigint]
 START WITH 1
 INCREMENT BY 1
 MINVALUE -9223372036854775808
 MAXVALUE 9223372036854775807
 CACHE 
GO
CREATE TABLE [dbo].[adm_employee](
	[employee_id] [int] NOT NULL,
	[employee_name] [nvarchar](1000) NULL,
	[employee_code] [nvarchar](255) NULL,
	[username] [nvarchar](255) NULL,
	[edit_version] [timestamp] NOT NULL,
	
	[created_by] [nvarchar](255) NULL,
	[created_date] [datetime] NULL,
	[modified_by] [nvarchar](255) NULL, 
	[modified_date] [datetime] NULL,
 CONSTRAINT [PK_employee] PRIMARY KEY CLUSTERED 
(
	[employee_id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO


ALTER TABLE [dbo].[adm_employee] ADD  CONSTRAINT [DF_adm_employee_employee_id]  DEFAULT (NEXT VALUE FOR [adm_employee_seq]) FOR [employee_id]

go
 

 ----
 
CREATE TABLE [dbo].[adm_employee_organization](
	[employee_organization_id] [int] IDENTITY(1,1) NOT NULL,
	[organization_id] [int] NULL,
	[employee_id] [int] NULL,
 CONSTRAINT [PK_adm_employee_organization] PRIMARY KEY CLUSTERED 
(
	[employee_organization_id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO


CREATE TABLE [dbo].[adm_organization](
	[organization_id] [int] IDENTITY(1,1) NOT NULL,
	[organization_name] [nvarchar](1000) NULL,
	[organization_code] [nvarchar](255) NULL,
	[address] [nvarchar](255) NULL,
	[edit_version] [timestamp] NOT NULL,
		
	[created_by] [nvarchar](255) NULL,
	[created_date] [datetime] NULL,
	[modified_by] [nvarchar](255) NULL, 
	[modified_date] [datetime] NULL
) ON [PRIMARY]
GO




CREATE TABLE [dbo].[adm_resource](
	[resource_id] [int] IDENTITY(1,1) NOT NULL,
	[resource_code] [nvarchar](255) NULL,
	[resource_name] [nvarchar](1000) NULL,
	[list_scope] [nvarchar](1000) NULL,
	[url] [nvarchar](255) NULL,
	[edit_version] [timestamp] NOT NULL,
		
	[created_by] [nvarchar](255) NULL,
	[created_date] [datetime] NULL,
	[modified_by] [nvarchar](255) NULL, 
	[modified_date] [datetime] NULL,
 CONSTRAINT [PK_resouce] PRIMARY KEY CLUSTERED 
(
	[resource_id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
 
   CREATE TABLE [dbo].[adm_resource_detail_api](
	[adm_resource_detail_api_id] [int] IDENTITY(1,1) NOT NULL, 
	[resource_id] [int]  NULL, 
	[uri] [nvarchar](255) NULL,
	[edit_version] [timestamp] NOT NULL,
	
	[created_by] [nvarchar](255) NULL,
	[created_date] [datetime] NULL,
	[modified_by] [nvarchar](255) NULL, 
	[modified_date] [datetime] NULL,
 CONSTRAINT [PK_adm_resource_detail_api] PRIMARY KEY CLUSTERED 
(
	[adm_resource_detail_api_id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO


  


CREATE TABLE [dbo].[adm_scope](
	[scope_id] [int] IDENTITY(1,1) NOT NULL,
	[scope_code] [nvarchar](255) NULL,
	[scope_name] [nvarchar](1000) NULL,
	[description] [nvarchar](255) NULL,
	[edit_version] [timestamp] NOT NULL,
	
	[created_by] [nvarchar](255) NULL,
	[created_date] [datetime] NULL,
	[modified_by] [nvarchar](255) NULL, 
	[modified_date] [datetime] NULL,
 CONSTRAINT [pk_scope] PRIMARY KEY CLUSTERED 
(
	[scope_id] ASC
) WITH (
	PAD_INDEX = OFF, 
	STATISTICS_NORECOMPUTE = OFF, 
	IGNORE_DUP_KEY = OFF, 
	ALLOW_ROW_LOCKS = ON, 
	ALLOW_PAGE_LOCKS = ON, 
	OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF
) ON [PRIMARY]
) ON [PRIMARY]

go
 
CREATE TABLE [dbo].[adm_permission](
	[permission_id] [int] IDENTITY(1,1) NOT NULL,
	[resource_id] [int] NOT NULL,
	[scope_id] [int] NOT NULL,
	[role_id] [int] NULL,
	[employee_id] [int] NULL,
	[edit_version] [timestamp] NOT NULL,
	
	[created_by] [nvarchar](255) NULL,
	[created_date] [datetime] NULL,
	[modified_by] [nvarchar](255) NULL, 
	[modified_date] [datetime] NULL,
 CONSTRAINT [pk_permission] PRIMARY KEY CLUSTERED 
(
	[permission_id] ASC
)WITH (
	PAD_INDEX = OFF, 
	STATISTICS_NORECOMPUTE = OFF, 
	IGNORE_DUP_KEY = OFF, 
	ALLOW_ROW_LOCKS = ON, 
	ALLOW_PAGE_LOCKS = ON, 
	OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF
) ON [PRIMARY]
) ON [PRIMARY]
GO



CREATE TABLE [dbo].[adm_role](
	[role_id] [int] IDENTITY(1,1) NOT NULL,
	[role_code] [nvarchar](255) NULL,
	[role_name] [nvarchar](1000) NULL,
	[description] [nvarchar](255) NULL,
	[edit_version] [timestamp] NOT NULL,
	
	[created_by] [nvarchar](255) NULL,
	[created_date] [datetime] NULL,
	[modified_by] [nvarchar](255) NULL, 
	[modified_date] [datetime] NULL,
 CONSTRAINT [PK_Role] PRIMARY KEY CLUSTERED 
(
	[role_id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO




CREATE TABLE [dbo].[adm_employee_role](
	[employee_role_id] [int] IDENTITY(1,1) NOT NULL,
	[employee_id] [int] NOT NULL,
	[role_id] [int] NOT NULL,
 CONSTRAINT [PK_employee_role] PRIMARY KEY CLUSTERED 
(
	[employee_role_id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO



CREATE TYPE dbo.[type_list_string] AS TABLE( 
	[value] [nvarchar](4000) NULL
)
GO

