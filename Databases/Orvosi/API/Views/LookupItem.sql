
CREATE VIEW API.LookupItem
AS
SELECT LookupId = l.Id
	, LookupName = l.Name
	, ItemId = li.Id
	, ItemText = li.[Text]
	, ItemValue = li.Value
FROM dbo.[Lookup] l
INNER JOIN dbo.LookupItem li ON l.Id = li.LookupId