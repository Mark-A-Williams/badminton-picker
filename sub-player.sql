DECLARE @SessionDate DATETIMEOFFSET = '2021-12-08'
DECLARE @DroppingOutPlayerInitials NVARCHAR(2) = 'JT'
DECLARE @SubbingInPlayerInitials NVARCHAR(2) = 'AU'

DECLARE @SessionId INT
SELECT @SessionId = [Id] FROM [Sessions] WHERE [Date] = @SessionDate

DELETE PlayerSessions WHERE SessionId = @SessionId AND PlayerId = @DroppingOutPlayerInitials
UPDATE PlayerSessions SET Status = 3 WHERE SessionId = @SessionId AND PlayerId = @SubbingInPlayerInitials
