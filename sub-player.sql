DECLARE @SessionDate DATETIMEOFFSET = '2022-**-** 00:00:00.0000000 +01:00'
DECLARE @DroppingOutPlayerInitials NVARCHAR(2) = '**'
DECLARE @SubbingInPlayerInitials NVARCHAR(2) = '**'

DECLARE @SessionId INT
SELECT @SessionId = [Id] FROM [Sessions] WHERE [Date] = @SessionDate

UPDATE PlayerSessions SET Status = 2 WHERE SessionId = @SessionId AND PlayerId = @DroppingOutPlayerInitials
UPDATE PlayerSessions SET Status = 3 WHERE SessionId = @SessionId AND PlayerId = @SubbingInPlayerInitials
