CREATE PROC dbo.TruncateHarvest
AS
TRUNCATE TABLE Harvest.TaskAssignment
TRUNCATE TABLE Harvest.UserAssignment
TRUNCATE TABLE Harvest.Client
TRUNCATE TABLE Harvest.DayEntry
TRUNCATE TABLE Harvest.Task
TRUNCATE TABLE Harvest.Project
TRUNCATE TABLE Harvest.[User]