-- Script Date: 10/8/2019 8:16 PM  - ErikEJ.SqlCeScripting version 3.5.2.81
-- Database information:
-- Database: C:\ProgramData\PostmanDeliversData\GameDB.sqlite3
-- ServerVersion: 3.27.2
-- DatabaseSize: 36 KB
-- Created: 10/5/2019 4:02 PM

-- User Table information:
-- Number of tables: 2
-- Games: -1 row(s)
-- PlayerMoves: -1 row(s)

SELECT 1;
PRAGMA foreign_keys=OFF;
BEGIN TRANSACTION;
CREATE TABLE [PlayerMoves] (
  [Id] INTEGER PRIMARY KEY AUTOINCREMENT NOT NULL
, [GameID] bigint NOT NULL
, [Square] bigint NOT NULL
, [GameBoardAfter] text NOT NULL
, [GameBoardBefore] text NOT NULL
, [Timestamp] bigint NOT NULL
, [Player] bigint NOT NULL
);
CREATE TABLE [Games] (
  [Id] INTEGER PRIMARY KEY AUTOINCREMENT NOT NULL
, [PlayerXName] text NOT NULL
, [PlayerOName] text NOT NULL
, [PlayerXID] text NOT NULL
, [PlayerOID] text NOT NULL
, [GameState] bigint NOT NULL
, [Winner] bigint NOT NULL
, [SquareOne] text NULL
, [SquareTwo] text NULL
, [SquareThree] text NULL
, [SquareFour] text NULL
, [SquareFive] text NULL
, [SquareSix] text NULL
, [SquareSeven] text NULL
, [SquareEight] text NULL
, [SquareNine] text NULL
);
COMMIT;

