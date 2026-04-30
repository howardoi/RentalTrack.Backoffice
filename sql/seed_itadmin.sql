/* =============================================================================
   Seed: itadmin Backoffice user
   -----------------------------------------------------------------------------
   Email        : itadmin@rentaltrack.local
   Display name : IT Admin
   Password     : 123
   Role         : 2  (SuperAdmin)
   Hash         : BCrypt (BCrypt.Net-Next, work factor 11)

   Idempotent — re-running updates the password / display name / role.
   ============================================================================= */

USE RentalTrack;
GO

DECLARE @Email        NVARCHAR(256) = N'itadmin@rentaltrack.local';
DECLARE @DisplayName  NVARCHAR(200) = N'IT Admin';
DECLARE @PasswordHash NVARCHAR(255) = N'$2a$11$9SGP5Zr/nGPENL1FhxmkCe/zdncVcnhAL7YegINy.GOr49Y3sU7eW';
DECLARE @Role         TINYINT       = 2;  -- 1 = Admin, 2 = SuperAdmin

IF EXISTS (SELECT 1 FROM dbo.AdminUser WHERE Email = @Email)
BEGIN
    UPDATE dbo.AdminUser
    SET DisplayName         = @DisplayName,
        PasswordHash        = @PasswordHash,
        PasswordChangedAt   = SYSUTCDATETIME(),
        FailedLoginAttempts = 0,
        LockoutEndAt        = NULL,
        [Role]              = @Role,
        IsActive            = 1,
        UpdatedAt           = SYSUTCDATETIME()
    WHERE Email = @Email;

    PRINT 'Updated existing AdminUser: ' + @Email;
END
ELSE
BEGIN
    INSERT INTO dbo.AdminUser (Email, DisplayName, PasswordHash, [Role], IsActive)
    VALUES (@Email, @DisplayName, @PasswordHash, @Role, 1);

    PRINT 'Inserted new AdminUser: ' + @Email;
END
GO

SELECT Id, Email, DisplayName, [Role], IsActive, CreatedAt, UpdatedAt
FROM dbo.AdminUser
WHERE Email = N'itadmin@rentaltrack.local';
GO
