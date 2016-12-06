/*
 Pre-Deployment Script Template							
--------------------------------------------------------------------------------------
 This file contains SQL statements that will be executed before the build script.	
 Use SQLCMD syntax to include a file in the pre-deployment script.			
 Example:      :r .\myfile.sql								
 Use SQLCMD syntax to reference a variable in the pre-deployment script.		
 Example:      :setvar TableName MyTable							
               SELECT * FROM [$(TableName)]					
--------------------------------------------------------------------------------------
*/

-- Create Tables

:r ".\Tables\Comment.sql"
:r ".\Tables\Image.sql"
:r ".\Tables\Plan.sql"
:r ".\Tables\PlanItem.sql"
:r ".\Tables\Profile.sql"
:r ".\Tables\SharedPlanItemUser.sql"
:r ".\Tables\SharedPlanUser.sql"
:r ".\Tables\SharingStatus.sql"
:r ".\Tables\Status.sql"
:r ".\Tables\User.sql"

-- Inser data

:r ".\Data\Comment.sql"
:r ".\Data\SharingStatus.sql"
:r ".\Data\Status.sql"
:r ".\Data\Profile.sql"
:r ".\Data\User.sql"
:r ".\Data\Plan.sql"
:r ".\Data\PlanItem.sql"