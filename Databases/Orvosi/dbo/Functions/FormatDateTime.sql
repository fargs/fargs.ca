


CREATE FUNCTION [dbo].[FormatDateTime]
(@datetime DATETIME, @formatstr VARCHAR (255))
RETURNS VARCHAR (255)
AS
BEGIN 
-- This Function works similar to the Format function in Visual Basic for creating Custom Formating Strings for Date/Time Variables
-- Valid characters for the @formatstr Are...
-- d Displays the day as a number without a leading zero (1 - 31)
-- dd Displays the day as a number with a leading zero (01 - 31)
-- ddd Displays the day as an abbreviation (Sun - Sat)
-- dddd Displays the day as a full name (Sunday - Saturday)
-- w Displays the day of the week as a number (1 for Sunday through 7 for Saturday)
-- m Displays the month as a number without a leading zero (1 - 12)
-- mm Displays the month as a number with a leading zero (01 - 12)
-- mmm Displays the month as an abbreviation (Jan - Dec)
-- mmmm Displays the month as a full month name (January - December)
-- yy Displays the year as a 2-digit number (00-99)
-- yyyy Displays the year as a 4-digit number (1000 - 9999)
-- q Displays the quarter of the year (1 - 4)
-- h Displays the hour as a number without leading zeros (0 - 23)
-- hh Displays the hour as a number with leading zeros (00 - 23)
-- th Displays the hour as a number without leading zeros (1 - 12)
-- n Displays the minute as a number without leading zeros (0 - 59)
-- nn Displays the minute as a number with leading zeros (00-59)
-- s Displays the second as a number without leading zeros (0 - 60)
-- ss Displays the second as a number with leading zeros (00 - 60)
-- am/pm Displays am before noon; Displays pm after noon through 11:59 P.M.
-- a/p Displays a before noon; Displays p after noon through 11:59 P.M.
-- Examples (assuming a date of March 7th, 2003 at 8:07:05 A.M.)
-- @formatstr Returns
-- m/d/yy 3/7/03
-- mmmm d, yyyy March 7, 2003
-- mm-dd-yyyy h:nnam/pm 03-07-2003 8:07am
DECLARE @outStr varchar(255)
DECLARE @datestr varchar(24)
DECLARE @meridian varchar(1)
DECLARE @temp varchar(2)
SET @outStr = @formatstr
SET @datestr = CONVERT(varchar(24), @datetime, 113)
-- dddd --
SET @outStr = REPLACE(@outStr, 'dddd',
CASE DATEPART(dw, @datetime)
WHEN 1 THEN 'Sunday'
WHEN 2 THEN 'Monday'
WHEN 3 THEN 'Tuesday'
WHEN 4 THEN 'Wednesday'
WHEN 5 THEN 'Thursday'
WHEN 6 THEN 'Friday'
WHEN 7 THEN 'Saturday'
END)
-- ddd --
SET @outStr = REPLACE(@outStr, 'ddd',
CASE DATEPART(dw, @datetime)
WHEN 1 THEN 'Sun'
WHEN 2 THEN 'Mon'
WHEN 3 THEN 'Tue'
WHEN 4 THEN 'Wed'
WHEN 5 THEN 'Thu'
WHEN 6 THEN 'Fri'
WHEN 7 THEN 'Sat'
END)
-- dd --
SET @outStr = REPLACE(@outStr, 'dd', SUBSTRING(@datestr,1,2))
-- d --
SET @outStr = REPLACE(@outStr, 'd', CONVERT(int,SUBSTRING(@datestr,1,2)))
-- w --
SET @outStr = REPLACE(@outStr, 'w', DATEPART(dw,@datetime))
-- yyyy --
SET @outStr = REPLACE(@outStr, 'yyyy', SUBSTRING(@datestr,8,4))
-- yy --
SET @outStr = REPLACE(@outStr, 'yy', SUBSTRING(@datestr,10,2))
-- q --
SET @outStr = REPLACE(@outStr, 'q', DATEPART(q,@datestr))
-- hh --
SET @outStr = REPLACE(@outStr, 'hh', SUBSTRING(@datestr,13,2))
-- th --
IF CONVERT(int,SUBSTRING(@datestr,13,2)) > 12
SET @outStr = REPLACE(@outStr, 'th', CONVERT(int,SUBSTRING(@datestr,13,2)) - 12)
ELSE SET @outStr = REPLACE(@outStr, 'th', SUBSTRING(@datestr,13,2))
-- h --
SET @outStr = REPLACE(@outStr, 'h', CONVERT(int,SUBSTRING(@datestr,13,2)))
-- nn --
SET @outStr = REPLACE(@outStr, 'nn', SUBSTRING(@datestr,16,2))
-- n --
SET @outStr = REPLACE(@outStr, 'n', CONVERT(int,SUBSTRING(@datestr,16,2)))
-- ss --
SET @outStr = REPLACE(@outStr, 'ss', SUBSTRING(@datestr,19,2))
-- s --
SET @outStr = REPLACE(@outStr, 's', CONVERT(int,SUBSTRING(@datestr,19,2)))
-- m, mm, mmm, mmmm (This is last because it put letters back into the @outStr and if done previously, things like the 'h' in 'March' become an hour --
IF CHARINDEX('m',@outStr,0) > 0 BEGIN
IF CHARINDEX('mm',@outStr,0) > 0 BEGIN
IF CHARINDEX('mmm',@outStr,0) > 0 BEGIN
IF CHARINDEX('mmmm',@outStr,0) > 0 BEGIN
SET @outStr = REPLACE(@outStr, 'mmmm',
CASE DATEPART(mm, @datetime)
WHEN 1 THEN 'January'
WHEN 2 THEN 'February'
WHEN 3 THEN 'March'
WHEN 4 THEN 'April'
WHEN 5 THEN 'May'
WHEN 6 THEN 'June'
WHEN 7 THEN 'July'
WHEN 8 THEN 'August'
WHEN 9 THEN 'September'
WHEN 10 THEN 'October'
WHEN 11 THEN 'November'
WHEN 12 THEN 'December'
END)
END
ELSE SET @outStr = REPLACE(@outStr, 'mmm', SUBSTRING(@datestr,4,3))
END
ELSE BEGIN
SET @temp = DATEPART(mm,@datetime)
IF (DATEPART(mm,@datetime)<10) SET @temp = '0' + @temp
SET @outStr = REPLACE(@outStr, 'mm', @temp)
END
END
ELSE BEGIN
SET @outStr = REPLACE(@outStr, 'm', DATEPART(mm,@datetime))
SET @outStr = REPLACE(@outStr, 'a'+CAST(DATEPART(mm,@datetime) AS varchar(1))+'/p'+CAST (DATEPART(mm,@datetime) AS varchar(1)),'am/pm')
END
END
-- Used by am/pm and a/p --
IF CONVERT(int,SUBSTRING(@datestr,13,2)) > 12
SET @meridian = 'p'
ELSE SET @meridian = 'a'
-- am/pm --
SET @outStr = REPLACE(@outStr, 'am/pm', @meridian+'m')
-- a/p --
SET @outStr = REPLACE(@outStr, 'a/p', @meridian)
RETURN @outStr
END