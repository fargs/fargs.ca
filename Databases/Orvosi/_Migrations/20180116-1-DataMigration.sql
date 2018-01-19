/****** Script for SelectTopNRows command from SSMS  ******/

INSERT INTO Feature (Id, Name, Description, IsActive, ModifiedBy, ModifiedDate)
VALUES (307, 'Service Request - Edit Task', NULL, 1, 'lfarago@orvosi.ca', GETDATE())

DECLARE @now DATETIME = GETDATE()
DECLARE @user NVARCHAR(50) = 'lfarago@orvosi.ca'
INSERT INTO dbo.ServiceRequestResource (Id, ServiceRequestId, RoleId, UserId, CreatedUser, CreatedDate, ModifiedUser, ModifiedDate)
SELECT newid(), 
	Id, 
	[Role] = CASE WHEN [Role] = 'CaseCoordinatorId' THEN '9EAB89C0-225C-4027-9F42-CC35E5656B14'
		WHEN [Role] = 'IntakeAssistantId' THEN '9DD582A0-CF86-4FC0-8894-477266068C12'
		WHEN [Role] = 'DocumentReviewerId' THEN '22B5C8AC-2C96-4A74-8057-976914031A7E'
	END, 
	UserId,
	@user,
	@now,
	@user,
	@now
FROM   
   (SELECT Id, [CaseCoordinatorId], [IntakeAssistantId], [DocumentReviewerId]
   FROM ServiceRequest) p  
UNPIVOT  
   (UserId FOR [Role] IN   
      ([CaseCoordinatorId], [IntakeAssistantId], [DocumentReviewerId])  
)AS unpvt; 


INSERT INTO ServiceRequestComment (Id, Comment, PostedDate, UserId, ServiceRequestId, CommentTypeId, CreatedDate, CreatedUser, ModifiedDate, ModifiedUser)
SELECT NEWID(), NOTES, ModifiedDate, PhysicianId, Id, 2, @now, @user, @now, @user
FROM ServiceRequest 
WHERE NOTES IS NOT NULL AND REPLACE(REPLACE(Notes, CHAR(13) + CHAR(10), ''), CHAR(10), '') <> ''


