/* Pragma options */
-- Enable foreign_keys
PRAGMA foreign_keys = ON;

/* Tables init */
-- Table to store db file version info
CREATE TABLE [BA_DB_UPDATES_HISTORY]
(
	[id]		INTEGER PRIMARY KEY AUTOINCREMENT,            -- update id
	[datetime]	TIMESTAMP NOT NULL DEFAULT CURRENT_TIMESTAMP, -- update timestamp
	[version]	INTEGER UNIQUE NOT NULL                       -- version number
);

-- Table to store todo notes
CREATE TABLE [BA_TODO_NOTES]
(
	[id]		INTEGER PRIMARY KEY AUTOINCREMENT, 	 -- note id
	[datestamp] 	DATE NOT NULL DEFAULT CURRENT_TIMESTAMP, -- added date/time
	[note]  	VARCHAR                                  -- note text
);

-- Cash operations history
CREATE TABLE [BA_CASH_OPERATIONS]
(
	[id]		INTEGER PRIMARY KEY AUTOINCREMENT,       -- operation id
	[datestamp]	DATE NOT NULL DEFAULT CURRENT_TIMESTAMP, -- operation's date
	[summa]		FLOAT NOT NULL,                          -- operation's sum
	[comment]	VARCHAR                                  -- operation's comment
);

-- Employees card-index
-- TODO: Table should be adjusted later
CREATE TABLE [BA_EMPLOYEES_CARDINDEX]
(
	[id]		INTEGER PRIMARY KEY AUTOINCREMENT,	-- employee id
	[hired]		DATE NOT NULL DEFAULT CURRENT_TIMESTAMP,-- when hired
	[fired]		DATE,					-- when fired
	[fullname]	VARCHAR NOT NULL,			-- employee full name
	[photo]		VARCHAR,				-- employee photo, path to file
	[document]	VARCHAR,				-- employee document number e.g. SSN
	[telephone]	VARCHAR,				-- employee contact number
	[address]	VARCHAR,				-- employee address
	[notes]		VARCHAR					-- any additional info
);

-- Employees salary and penalty
CREATE TABLE [BA_EMPLOYEES_CASH]
(
	[id]	INTEGER PRIMARY KEY AUTOINCREMENT,	-- record id
	[emid]	INTEGER NOT NULL					-- employee id
			REFERENCES BA_EMPLOYEES_CARDINDEX(id)
			ON DELETE RESTRICT,
	[opid]	INTEGER NOT NULL					-- operation id
			REFERENCES BA_CASH_OPERATIONS(id)
			ON DELETE CASCADE
);

/* Triggers */
-- TODO: Add trigger to restrict update/delete from BA_DB_UPDATES_HISTORY

/* Queries */
-- Set default database version
INSERT INTO BA_DB_UPDATES_HISTORY (version) VALUES (1);