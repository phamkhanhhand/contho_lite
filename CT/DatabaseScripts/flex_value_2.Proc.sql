 
 --=====================================
 --save link flex value set
 --phamkhanhhand Sep 14, 2025
-- =============================================
CREATE or alter PROCEDURE [dbo].[proc_save_hierarchy_set]

 @id bigint
,@list_parent dbo.[type_list_bigint] READONLY  -- **TVP bắt buộc phải là READONLY**
,@list_child dbo.[type_list_bigint] READONLY  -- **TVP bắt buộc phải là READONLY**
 
AS
BEGIN 


declare @hierarchy_type int  = 0 --set=0; flexvalue = 1

--remind: validate in BE before: delete not has value child-parent

--delete not exists parent/child

delete a
from [adm_flex_hierarchy] a
left join @list_parent b on b.value = a.[parent_flex_value_set_id]
where a.child_flex_value_set_id = @id
and a.hierarchy_type =@hierarchy_type 
and b.value is null

delete a
from [adm_flex_hierarchy] a
left join @list_child b on b.value = a.child_flex_value_set_id 
where a.[parent_flex_value_set_id]= @id
and a.hierarchy_type =@hierarchy_type 
and b.value is null

--insert if notexists	

--parent
INSERT INTO [dbo].[adm_flex_hierarchy]
           ([flex_hierarchy_id]
           ,[parent_flex_value_set_id]
           ,[child_flex_value_set_id]
           ,[parent_flex_value_id]
           ,[child_flex_value_id]
           ,[child_value]
           ,[parent_value]
           ,[hierarchy_type])

		   select 
		   NEXT VALUE FOR dbo.seq_adm_flex_hierarchy [flex_hierarchy_id]
           , a.value [parent_flex_value_set_id]
           ,@id [child_flex_value_set_id]
           ,NULL [parent_flex_value_id]
           ,NULL [child_flex_value_id]
           ,NULL [child_value]
           ,NULL [parent_value]
           ,@hierarchy_type [hierarchy_type]
		   from @list_parent a
		   left join [adm_flex_hierarchy] b 
				on a.value =b.parent_flex_value_set_id 
				and b.hierarchy_type =@hierarchy_type 
				and b.child_flex_value_set_id = @id
		   where b.flex_hierarchy_id is null
     
	 --child
	 
INSERT INTO [dbo].[adm_flex_hierarchy]
           ([flex_hierarchy_id]
           ,[parent_flex_value_set_id]
           ,[child_flex_value_set_id]
           ,[parent_flex_value_id]
           ,[child_flex_value_id]
           ,[child_value]
           ,[parent_value]
           ,[hierarchy_type])

		   select 
		   NEXT VALUE FOR dbo.seq_adm_flex_hierarchy [flex_hierarchy_id]
           ,@id [parent_flex_value_set_id]
           ,a.value [child_flex_value_set_id]
           ,NULL [parent_flex_value_id]
           ,NULL [child_flex_value_id]
           ,NULL [child_value]
           ,NULL [parent_value]
           ,@hierarchy_type [hierarchy_type] 
		   from @list_child a
		   left join [adm_flex_hierarchy] b 
				on a.value =b.[child_flex_value_set_id] 
				and b.hierarchy_type =@hierarchy_type 
				and b.[parent_flex_value_set_id] = @id

		   where b.flex_hierarchy_id is null
     
	 select 1

END



 
 --=====================================
 --save link flex value
 --note that: flex value set rule flex value link
 --phamkhanhhand Sep 14, 2025
-- =============================================
CREATE or alter PROCEDURE [dbo].[proc_save_hierarchy_value]
 @id bigint 
,@ref_set_ID bigint
,@list_ref dbo.[type_list_bigint] READONLY  -- **TVP bắt buộc phải là READONLY**
 ,@for int --0: child; 1-parent 
AS
BEGIN 


declare @hierarchy_type int  = 1 --set=0; flexvalue = 1
declare @set_id bigint
declare @flex_value nvarchar(500)

select @set_id = a.flex_value_set_id
	   ,@flex_value = a.flex_value
from adm_flex_values a
where a.flex_value_id = @id

--remind: validate in BE before: delete not has value child-parent

--delete not exists parent/child


--add child link
if (@for=0)
begin


	--delete not ref new child
	delete a
	from [adm_flex_hierarchy] a
	left join @list_ref b on b.value = a.child_flex_value_id 
	where a.parent_flex_value_id = @id
	and a.[parent_flex_value_set_id]= @set_id
	and a.child_flex_value_set_id = @ref_set_ID
	and a.hierarchy_type =@hierarchy_type 
	and b.value is null


	
	INSERT INTO [dbo].[adm_flex_hierarchy]
           ([flex_hierarchy_id]
           ,[parent_flex_value_set_id]
           ,[child_flex_value_set_id]
           ,[parent_flex_value_id]
           ,[child_flex_value_id]
           ,[child_value]
           ,[parent_value]
           ,[hierarchy_type])

		   select 
		    NEXT VALUE FOR dbo.seq_adm_flex_hierarchy [flex_hierarchy_id] 
           ,@set_id [parent_flex_value_set_id]
           ,@ref_set_id  [child_flex_value_set_id]
           ,@id [parent_flex_value_id]
           ,a.value [child_flex_value_id]
           ,c.flex_value [child_value]
           ,@flex_value [parent_value]
           ,@hierarchy_type [hierarchy_type]

		   from @list_ref a
		   left join [adm_flex_hierarchy] b
				on b.hierarchy_type = @hierarchy_type
				  and b.parent_flex_value_set_id = @set_id
				  and b.parent_flex_value_id = @id
				  and b.child_flex_value_set_id	= @ref_set_ID
				  and b.child_flex_value_id = a.value
			inner join adm_flex_values c on a.value = c.flex_value_id
			where b.flex_hierarchy_id is null
			 

end
 

 ---else add for parent
 
else if (@for=1)
begin


	--delete not ref new child
	delete a
	from [adm_flex_hierarchy] a
	left join @list_ref b on b.value = a.parent_flex_value_id 
	where a.child_flex_value_id = @id
	and a.[child_flex_value_set_id]= @set_id
	and a.parent_flex_value_set_id = @ref_set_ID
	and a.hierarchy_type =@hierarchy_type 
	and b.value is null


	
	INSERT INTO [dbo].[adm_flex_hierarchy]
           ([flex_hierarchy_id]
           ,[parent_flex_value_set_id]
           ,[child_flex_value_set_id]
           ,[parent_flex_value_id]
           ,[child_flex_value_id]
           ,[child_value]
           ,[parent_value]
           ,[hierarchy_type])

		   select 
		    NEXT VALUE FOR dbo.seq_adm_flex_hierarchy [flex_hierarchy_id] 
           ,@ref_set_id [parent_flex_value_set_id]
           ,@set_id   [child_flex_value_set_id]
           ,a.value [parent_flex_value_id]
           ,@id [child_flex_value_id]
           ,@flex_value [child_value]
           ,c.flex_value [parent_value]
           ,@hierarchy_type [hierarchy_type]

		   from @list_ref a
		   left join [adm_flex_hierarchy] b
				on b.hierarchy_type = @hierarchy_type
				  and b.child_flex_value_set_id = @set_id
				  and b.child_flex_value_id = @id
				  and b.parent_flex_value_set_id	= @ref_set_ID
				  and b.parent_flex_value_id = a.value
			inner join adm_flex_values c on a.value = c.flex_value_id
			where b.flex_hierarchy_id is null
			 

end
 
     
	 select 1

END

go

 
 --=====================================
 --save link flex value
 --note that: flex value set rule flex value link
 --phamkhanhhand Sep 14, 2025
-- =============================================
CREATE or alter PROCEDURE [dbo].[proc_save_hierarchy_value_all]
 @id bigint 

,@child_set_ID bigint
,@list_child dbo.[type_list_bigint] READONLY  -- **TVP bắt buộc phải là READONLY** 



,@parent_set_ID bigint
,@list_parent dbo.[type_list_bigint] READONLY  -- **TVP bắt buộc phải là READONLY** 


AS
BEGIN 


declare @hierarchy_type int  = 1 --set=0; flexvalue = 1
declare @set_id bigint
declare @flex_value nvarchar(500)

select @set_id = a.flex_value_set_id
	   ,@flex_value = a.flex_value
from adm_flex_values a
where a.flex_value_id = @id

--remind: validate in BE before: delete not has value child-parent

--delete not exists parent/child


--add child link 
begin


	--delete not ref new child
	delete a
	from [adm_flex_hierarchy] a
	left join @list_child b on b.value = a.child_flex_value_id 
	where a.parent_flex_value_id = @id
	and a.[parent_flex_value_set_id]= @set_id
	and a.child_flex_value_set_id = @child_set_ID
	and a.hierarchy_type =@hierarchy_type 
	and b.value is null


	
	INSERT INTO [dbo].[adm_flex_hierarchy]
           ([flex_hierarchy_id]
           ,[parent_flex_value_set_id]
           ,[child_flex_value_set_id]
           ,[parent_flex_value_id]
           ,[child_flex_value_id]
           ,[child_value]
           ,[parent_value]
           ,[hierarchy_type])

		   select 
		    NEXT VALUE FOR dbo.seq_adm_flex_hierarchy [flex_hierarchy_id] 
           ,@set_id [parent_flex_value_set_id]
           ,@child_set_id  [child_flex_value_set_id]
           ,@id [parent_flex_value_id]
           ,a.value [child_flex_value_id]
           ,c.flex_value [child_value]
           ,@flex_value [parent_value]
           ,@hierarchy_type [hierarchy_type]

		   from @list_child a
		   left join [adm_flex_hierarchy] b
				on b.hierarchy_type = @hierarchy_type
				  and b.parent_flex_value_set_id = @set_id
				  and b.parent_flex_value_id = @id
				  and b.child_flex_value_set_id	= @child_set_ID
				  and b.child_flex_value_id = a.value
			inner join adm_flex_values c on a.value = c.flex_value_id
			where b.flex_hierarchy_id is null
			 

end
 

 ---else add for parent 
begin


	--delete not ref new child
	delete a
	from [adm_flex_hierarchy] a
	left join @list_parent b on b.value = a.parent_flex_value_id 
	where a.child_flex_value_id = @id
	and a.[child_flex_value_set_id]= @set_id
	and a.parent_flex_value_set_id = @parent_set_ID
	and a.hierarchy_type =@hierarchy_type 
	and b.value is null


	
	INSERT INTO [dbo].[adm_flex_hierarchy]
           ([flex_hierarchy_id]
           ,[parent_flex_value_set_id]
           ,[child_flex_value_set_id]
           ,[parent_flex_value_id]
           ,[child_flex_value_id]
           ,[child_value]
           ,[parent_value]
           ,[hierarchy_type])

		   select 
		    NEXT VALUE FOR dbo.seq_adm_flex_hierarchy [flex_hierarchy_id] 
           ,@parent_set_id [parent_flex_value_set_id]
           ,@set_id   [child_flex_value_set_id]
           ,a.value [parent_flex_value_id]
           ,@id [child_flex_value_id]
           ,@flex_value [child_value]
           ,c.flex_value [parent_value]
           ,@hierarchy_type [hierarchy_type]

		   from @list_parent a
		   left join [adm_flex_hierarchy] b
				on b.hierarchy_type = @hierarchy_type
				  and b.child_flex_value_set_id = @set_id
				  and b.child_flex_value_id = @id
				  and b.parent_flex_value_set_id	= @parent_set_ID
				  and b.parent_flex_value_id = a.value
			inner join adm_flex_values c on a.value = c.flex_value_id
			where b.flex_hierarchy_id is null
			 

end
 
     
	 select 1

END

go



 --=====================================
 --save link flex value
 --note that: flex value set rule flex value link
 --phamkhanhhand Sep 14, 2025
-- =============================================
CREATE or alter PROCEDURE [dbo].[proc_save_hierarchy_value_all_set]
 @id bigint  
,@list_child dbo.[type_list_bigint] READONLY  -- **TVP bắt buộc phải là READONLY** 
,@list_parent dbo.[type_list_bigint] READONLY  -- **TVP bắt buộc phải là READONLY** 
AS
BEGIN 


declare @hierarchy_type int  = 1 --set=0; flexvalue = 1
declare @set_id bigint
declare @flex_value nvarchar(500)

select @set_id = a.flex_value_set_id
	   ,@flex_value = a.flex_value
from adm_flex_values a
where a.flex_value_id = @id

--remind: validate in BE before: delete not has value child-parent

--delete not exists parent/child


--add child link 
begin


	--delete not ref new child, for all set
	delete a
	from [adm_flex_hierarchy] a
			--inner join adm_flex_values c on a.child_flex_value_id = c.flex_value_id
	left join @list_child b on b.value = a.child_flex_value_id 
	where a.parent_flex_value_id = @id
	and a.[parent_flex_value_set_id]= @set_id
	--and a.child_flex_value_set_id = c.flex_value_set_id
	and a.hierarchy_type =@hierarchy_type 
	and b.value is null


	
	INSERT INTO [dbo].[adm_flex_hierarchy]
           ([flex_hierarchy_id]
           ,[parent_flex_value_set_id]
           ,[child_flex_value_set_id]
           ,[parent_flex_value_id]
           ,[child_flex_value_id]
           ,[child_value]
           ,[parent_value]
           ,[hierarchy_type])

		   select 
		    NEXT VALUE FOR dbo.seq_adm_flex_hierarchy [flex_hierarchy_id] 
           ,@set_id [parent_flex_value_set_id]
           ,c.flex_value_set_id  [child_flex_value_set_id]
           ,@id [parent_flex_value_id]
           ,a.value [child_flex_value_id]
           ,c.flex_value [child_value]
           ,@flex_value [parent_value]
           ,@hierarchy_type [hierarchy_type]

		   from @list_child a
			inner join adm_flex_values c on a.value = c.flex_value_id
		   left join [adm_flex_hierarchy] b
				on b.hierarchy_type = @hierarchy_type
				  and b.parent_flex_value_set_id = @set_id
				  and b.parent_flex_value_id = @id
				  and b.child_flex_value_set_id	= c.flex_value_set_id
				  and b.child_flex_value_id = a.value
			where b.flex_hierarchy_id is null
			 

end
 

 ---else add for parent 
begin


	--delete not ref new child
	delete a
	from [adm_flex_hierarchy] a
			--inner join adm_flex_values c on a.child_flex_value_id = c.flex_value_id
	left join @list_parent b on b.value = a.parent_flex_value_id 
	where a.child_flex_value_id = @id
	and a.[child_flex_value_set_id]= @set_id
	--and a.parent_flex_value_set_id = c.flex_value_set_id
	and a.hierarchy_type =@hierarchy_type 
	and b.value is null


	
	INSERT INTO [dbo].[adm_flex_hierarchy]
           ([flex_hierarchy_id]
           ,[parent_flex_value_set_id]
           ,[child_flex_value_set_id]
           ,[parent_flex_value_id]
           ,[child_flex_value_id]
           ,[child_value]
           ,[parent_value]
           ,[hierarchy_type])

		   select 
		    NEXT VALUE FOR dbo.seq_adm_flex_hierarchy [flex_hierarchy_id] 
           ,c.flex_value_set_id [parent_flex_value_set_id]
           ,@set_id   [child_flex_value_set_id]
           ,a.value [parent_flex_value_id]
           ,@id [child_flex_value_id]
           ,@flex_value [child_value]
           ,c.flex_value [parent_value]
           ,@hierarchy_type [hierarchy_type]

		   from @list_parent a
			inner join adm_flex_values c on a.value = c.flex_value_id
		   left join [adm_flex_hierarchy] b
				on b.hierarchy_type = @hierarchy_type
				  and b.child_flex_value_set_id = @set_id
				  and b.child_flex_value_id = @id
				  and b.parent_flex_value_set_id	= c.flex_value_set_id
				  and b.parent_flex_value_id = a.value
			where b.flex_hierarchy_id is null
			 

end
 
     
	 select 1

END

go