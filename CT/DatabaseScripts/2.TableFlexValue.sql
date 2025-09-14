   


CREATE SEQUENCE [dbo].[Seq_adm_flex_value_sets] 
 AS [bigint]
 START WITH 1
 INCREMENT BY 1
 MINVALUE -9223372036854775808
 MAXVALUE 9223372036854775807
 CACHE 
GO 
CREATE TABLE [dbo].[adm_flex_value_sets](
	[flex_value_set_id] [bigint] NOT NULL,
	[flex_value_set_name] [nvarchar](500) NULL,
	[enable_flag] [nvarchar](10) NULL,
	[period] [nvarchar](10) NULL,
	[description] [nvarchar](500) NULL, 
	[edit_version] [timestamp] NOT NULL, 
	[created_by] [nvarchar](255) NULL,
	[created_date] [datetime] NULL,
	[modified_by] [nvarchar](255) NULL, 
	[modified_date] [datetime] NULL

) ON [PRIMARY]
GO 
ALTER TABLE [dbo].[adm_flex_value_sets] ADD  CONSTRAINT [DF_adm_flex_value_sets_flex_value_set_id]  DEFAULT (NEXT VALUE FOR [seq_adm_flex_value_sets]) FOR [flex_value_set_id]
GO

 
 
CREATE SEQUENCE [dbo].[seq_adm_flex_values] 
 AS [bigint]
 START WITH 1
 INCREMENT BY 1
 MINVALUE -9223372036854775808
 MAXVALUE 9223372036854775807
 CACHE 
GO  
CREATE TABLE [dbo].[adm_flex_values](
	[flex_value_id] [bigint] NOT NULL,
	[flex_value_set_id] [bigint] NULL,
	[flex_value] [nvarchar](500) NULL,
	[flex_value_name] [nvarchar](500) NULL,
	[enable_flag] [nvarchar](10) NULL,
	[period] [nvarchar](10) NULL,
	[description] [nvarchar](500) NULL,
	[edit_version] [timestamp] NOT NULL, 
	[created_by] [nvarchar](255) NULL,
	[created_date] [datetime] NULL,
	[modified_by] [nvarchar](255) NULL, 
	[modified_date] [datetime] NULL

) ON [PRIMARY]
GO

ALTER TABLE [dbo].[adm_flex_values] ADD  CONSTRAINT [DF_adm_flex_values_flex_value_id]  DEFAULT (NEXT VALUE FOR [seq_adm_flex_values]) FOR [flex_value_id]
GO

 

 
CREATE SEQUENCE [dbo].[seq_adm_flex_hierarchy] 
 AS [bigint]
 START WITH 1
 INCREMENT BY 1
 MINVALUE -9223372036854775808
 MAXVALUE 9223372036854775807
 CACHE 
GO
CREATE TABLE [dbo].[adm_flex_hierarchy](
	[flex_hierarchy_id] [bigint] NOT NULL,
	[parent_flex_value_set_id] [bigint] NOT NULL,
	[child_flex_value_set_id] [bigint] NOT NULL,
	[parent_flex_value_id] [bigint] NOT NULL,
	[child_flex_value_id] [bigint] NOT NULL,
	[child_value] [nvarchar](500) NULL,
	[parent_value] [nvarchar](500) NULL,
	[hierarchy_type] [int] NULL ---0: set ; 1-value
) ON [PRIMARY]
GO

ALTER TABLE [dbo].[adm_flex_hierarchy] ADD  CONSTRAINT [DF_adm_flex_hierarchy_flex_link]  DEFAULT (NEXT VALUE FOR [seq_adm_flex_hierarchy]) FOR [flex_hierarchy_id]
GO

