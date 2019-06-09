DELETE FROM Schedule
ALTER TABLE Schedule ALTER COLUMN DurationStart datetime not null
ALTER TABLE Schedule ALTER COLUMN DurationEnd datetime null