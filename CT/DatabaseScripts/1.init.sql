CREATE SCHEMA auth;

go
 
CREATE TABLE auth.[auth_accounts](

    id INT PRIMARY KEY IDENTITY,
	[username] [nvarchar](255) NULL,
	[password_hash] [nvarchar](2000) NULL,
	[created_date] [datetime] NULL,
	[created_by] [nvarchar](255) NULL
) ON [PRIMARY]
GO


 