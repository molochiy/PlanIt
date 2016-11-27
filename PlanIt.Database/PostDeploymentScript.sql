/*
Post-Deployment Script Template							
--------------------------------------------------------------------------------------
 This file contains SQL statements that will be appended to the build script.		
 Use SQLCMD syntax to include a file in the post-deployment script.			
 Example:      :r .\myfile.sql								
 Use SQLCMD syntax to reference a variable in the post-deployment script.		
 Example:      :setvar TableName MyTable							
               SELECT * FROM [$(TableName)]					
--------------------------------------------------------------------------------------
*/

:r ".\Foreign keys\Comment.sql"
:r ".\Foreign keys\Plan.sql"
:r ".\Foreign keys\PlanItem.sql"
:r ".\Foreign keys\SharedPlanItemUser.sql"
:r ".\Foreign keys\SharedPlanUser.sql"
:r ".\Foreign keys\User.sql"