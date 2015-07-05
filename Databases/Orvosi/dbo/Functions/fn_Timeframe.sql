-- =============================================
-- Author:		Les Farago
-- Create date: 
-- Description:	
-- =============================================
CREATE FUNCTION [dbo].[fn_Timeframe] 
(
	@DatePart nvarchar(10) = 'd'
)
RETURNS 
@Timeframe TABLE 
(
	-- Add the column definitions for the TABLE variable here
	PK_Date date
	, [Sequence] int
	, [Year] smallint
	, [Month] nvarchar(50)
	, Month_Of_Year smallint
	, Day_Of_Month smallint
	, Week_Of_Year smallint
	, Day_Of_Week smallint
)
AS
BEGIN

	DECLARE @StartDate date, @EndDate date
	SET @StartDate = '2015-07-01'
	SET @EndDate = '2017-12-31'
	
	IF (@DatePart = 'd') BEGIN
		INSERT INTO @Timeframe (PK_Date, [Sequence], [Year], [Month], Month_Of_Year, Day_Of_Month, Week_Of_Year, Day_Of_Week)
		SELECT t.PK_Date
		, ROW_NUMBER() OVER(ORDER BY t.PK_Date)
		, t.Sortable_Year
		, t.Sortable_Month
		, t.Month_Of_Year
		, t.Day_Of_Month
		, t.[Week_Of_Year]
		, t.[Day_Of_Week]
		FROM dbo.[Time] t
		WHERE PK_Date BETWEEN @StartDate AND @EndDate
	END
	ELSE IF (@DatePart = 'w') BEGIN
		INSERT INTO @Timeframe (PK_Date, [Sequence], [Year], [Month], Month_Of_Year, Day_Of_Month, Week_Of_Year, Day_Of_Week)
		SELECT t.PK_Date
		, ROW_NUMBER() OVER(ORDER BY t.PK_Date)
		, t.Sortable_Year
		, t.Sortable_Month
		, t.Month_Of_Year
		, t.Day_Of_Month
		, t.[Week_Of_Year]
		, t.[Day_Of_Week]
		FROM dbo.[Time] t
		WHERE PK_Date BETWEEN @StartDate AND @EndDate AND t.Day_Of_Week = 1
	END
	ELSE IF (@DatePart = 'm') BEGIN
		INSERT INTO @Timeframe (PK_Date, [Sequence], [Year], [Month], Month_Of_Year, Day_Of_Month, Week_Of_Year, Day_Of_Week)
		SELECT t.PK_Date
		, ROW_NUMBER() OVER(ORDER BY t.PK_Date)
		, t.Sortable_Year
		, t.Sortable_Month
		, t.Month_Of_Year
		, t.Day_Of_Month
		, t.[Week_Of_Year]
		, t.[Day_Of_Week]
		FROM dbo.[Time] t
		WHERE PK_Date BETWEEN @StartDate AND @EndDate AND t.Day_Of_Month = 1
	END
	
	RETURN 
END