DECLARE @SessionDate DATETIMEOFFSET = '2023-07-05'
DECLARE @DroppingOutPlayerInitials NVARCHAR(2) = 'MW'
DECLARE @SubbingInPlayerInitials NVARCHAR(2) = 'GW'

DECLARE @SessionId INT
SELECT @SessionId = [Id] FROM [Sessions] WHERE [Date] = @SessionDate

UPDATE PlayerSessions SET Status = 2 WHERE SessionId = @SessionId AND PlayerId = @DroppingOutPlayerInitials
UPDATE PlayerSessions SET Status = 3 WHERE SessionId = @SessionId AND PlayerId = @SubbingInPlayerInitials

INSERT INTO PlayerSessions (PlayerId, SessionId, Status)
VALUES ('GW', (SELECT TOP 1 Id FROM Sessions ORDER BY Date DESC), 3)