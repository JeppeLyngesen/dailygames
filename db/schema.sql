-- DailyGames – database schema for PHASE 2 (user accounts + sync across devices).
-- Not used yet: phase 1 stores everything in the browser (localStorage).
-- Target: SQL Server / Azure SQL (free offer). Mirrors DailyGames.Core.Models.

CREATE TABLE Games (
    GameId        NVARCHAR(64)   NOT NULL PRIMARY KEY,   -- 'wordle', or 'custom-xxxx'
    Name          NVARCHAR(100)  NOT NULL,
    Url           NVARCHAR(400)  NOT NULL,
    Category      NVARCHAR(50)   NOT NULL DEFAULT 'Other',
    Description   NVARCHAR(300)  NOT NULL DEFAULT '',
    OwnerUserId   INT            NULL,                    -- NULL = catalog game, otherwise a user's custom game
    CreatedAt     DATETIME2      NOT NULL DEFAULT SYSUTCDATETIME()
);

CREATE TABLE Users (
    UserId        INT IDENTITY   PRIMARY KEY,
    Email         NVARCHAR(256)  NOT NULL UNIQUE,
    DisplayName   NVARCHAR(50)   NULL,
    TimeZone      NVARCHAR(64)   NOT NULL DEFAULT 'Europe/Copenhagen', -- streak "days" are local days
    CreatedAt     DATETIME2      NOT NULL DEFAULT SYSUTCDATETIME()
);

ALTER TABLE Games ADD CONSTRAINT FK_Games_Owner FOREIGN KEY (OwnerUserId) REFERENCES Users(UserId) ON DELETE CASCADE;

CREATE TABLE FollowedGames (
    UserId        INT            NOT NULL REFERENCES Users(UserId) ON DELETE CASCADE,
    GameId        NVARCHAR(64)   NOT NULL REFERENCES Games(GameId),
    SortOrder     INT            NOT NULL,
    PRIMARY KEY (UserId, GameId)
);

CREATE TABLE Completions (
    UserId        INT            NOT NULL REFERENCES Users(UserId) ON DELETE CASCADE,
    GameId        NVARCHAR(64)   NOT NULL REFERENCES Games(GameId),
    PlayDate      DATE           NOT NULL,                -- the user's local date
    Solved        BIT            NOT NULL DEFAULT 1,
    Note          NVARCHAR(500)  NULL,
    CreatedAt     DATETIME2      NOT NULL DEFAULT SYSUTCDATETIME(),
    PRIMARY KEY (UserId, GameId, PlayDate)                -- one completion per game per day
);

CREATE INDEX IX_Completions_User_Date ON Completions (UserId, PlayDate DESC);

-- Example: current streak for one user + game with the "gaps and islands" technique.
-- DECLARE @UserId INT = 1, @GameId NVARCHAR(64) = 'wordle', @Today DATE = CAST(GETDATE() AS DATE);
-- WITH Runs AS (
--     SELECT PlayDate, DATEADD(DAY, -ROW_NUMBER() OVER (ORDER BY PlayDate), PlayDate) AS RunKey
--     FROM Completions WHERE UserId = @UserId AND GameId = @GameId AND PlayDate <= @Today
-- )
-- SELECT COUNT(*) AS Length, MIN(PlayDate) AS StartDate, MAX(PlayDate) AS EndDate
-- FROM Runs GROUP BY RunKey ORDER BY EndDate DESC;   -- first row is current if EndDate >= yesterday
