
 --=====================================
 --CheckPermision
 --phamha Nov 16, 2024
-- =============================================
CREATE   PROCEDURE [dbo].[proc_CheckPermission]


@uri nvarchar(1000)
,@username nvarchar(256)
,@list_scope dbo.type_list_string READONLY  -- **TVP bắt buộc phải là READONLY**
 
AS
BEGIN
declare @hasPermission bit = 0



if exists (

	select 1
	from dbo.adm_permission a
	inner join adm_employee b on a.employee_id = b.employee_id
	inner join adm_resource c on a.resource_id = c.resource_id
	inner join [adm_resource_detail_api] d on c.resource_id = c.resource_id
	inner join adm_scope e on e.scope_id = a.scope_id
	inner join @list_scope f on f.value = e.scope_code
	where b.username = @username
	and d.uri = @uri 

	union all
	select 1
	from dbo.adm_permission a
	inner join adm_role b1  on a.role_id = b1.role_id
	inner join adm_employee_role b2 on b1.role_id = b2.role_id
	inner join adm_employee b on b2.employee_id = b.employee_id
	inner join adm_resource c on a.resource_id = c.resource_id
	inner join [adm_resource_detail_api] d on c.resource_id = c.resource_id
	inner join adm_scope e on e.scope_id = a.scope_id
	inner join @list_scope f on f.value = e.scope_code
	where b.username = @username
	and d.uri = @uri 

	)
 
	select @hasPermission = cast(1 as bit)


	select @hasPermission
END