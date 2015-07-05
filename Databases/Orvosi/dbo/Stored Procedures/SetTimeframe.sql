CREATE PROC [dbo].[SetTimeframe]
	@StartDate date
	, @EndDate date
	, @DatePart nvarchar(10) = 'm'
AS

TRUNCATE TABLE dbo.Timeframe

IF (@DatePart = 'd') BEGIN
	INSERT INTO dbo.Timeframe (PK_Date, [Sequence], [Year], [Month], Month_Of_Year, Day_Of_Month, Week_Of_Year)
	SELECT t.PK_Date
	, ROW_NUMBER() OVER(ORDER BY t.PK_Date)
	, t.Sortable_Year
	, t.Sortable_Month
	, t.Month_Of_Year
	, t.Day_Of_Month
	, t.[Week_Of_Year]
	FROM dbo.[Time] t
	WHERE PK_Date BETWEEN @StartDate AND @EndDate
END
ELSE IF (@DatePart = 'm') BEGIN
	INSERT INTO dbo.Timeframe (PK_Date, [Sequence], [Year], [Month], Month_Of_Year, Day_Of_Month, Week_Of_Year)
	SELECT t.PK_Date
	, ROW_NUMBER() OVER(ORDER BY t.PK_Date)
	, t.Sortable_Year
	, t.Sortable_Month
	, t.Month_Of_Year
	, t.Day_Of_Month
	, t.[Week_Of_Year]
	FROM dbo.[Time] t
	WHERE PK_Date BETWEEN @StartDate AND @EndDate AND t.Day_Of_Month = 1
END