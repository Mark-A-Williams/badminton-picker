DECLARE @SessionDate DATETIMEOFFSET = '2022-04-06 00:00:00.0000000 +01:00'
DECLARE @DroppingOutPlayerInitials NVARCHAR(2) = 'JS'
DECLARE @SubbingInPlayerInitials NVARCHAR(2) = 'HM'

DECLARE @SessionId INT
SELECT @SessionId = [Id] FROM [Sessions] WHERE [Date] = @SessionDate

DELETE PlayerSessions WHERE SessionId = @SessionId AND PlayerId = @DroppingOutPlayerInitials
UPDATE PlayerSessions SET Status = 3 WHERE SessionId = @SessionId AND PlayerId = @SubbingInPlayerInitials
