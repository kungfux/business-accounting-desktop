-- Fill db with data samples
INSERT INTO BA_TODO_NOTES (note) VALUES ('Try to use this cool app.');
INSERT INTO BA_TODO_NOTES (note) VALUES ('Use this app for my business accounting because it provides features I need.');

INSERT INTO BA_CASH_OPERATIONS (datestamp, summa, comment) VALUES ('2015-01-01 00:00:00', 10.0, 'Доход1');
INSERT INTO BA_CASH_OPERATIONS (datestamp, summa, comment) VALUES ('2015-01-02 00:00:00', 20.0, 'Доход2');
INSERT INTO BA_CASH_OPERATIONS (datestamp, summa, comment) VALUES ('2015-01-03 00:00:00', 30.0, 'Доход3');
INSERT INTO BA_CASH_OPERATIONS (datestamp, summa, comment) VALUES ('2015-01-04 00:00:00', 40.0, 'Доход4');
INSERT INTO BA_CASH_OPERATIONS (datestamp, summa, comment) VALUES ('2015-01-05 00:00:00', 50.0, 'Доход5');
INSERT INTO BA_CASH_OPERATIONS (datestamp, summa, comment) VALUES ('2015-01-01 00:00:00', -1.0, 'Расход1');
INSERT INTO BA_CASH_OPERATIONS (datestamp, summa, comment) VALUES ('2015-01-02 00:00:00', -2.0, 'Расход2');
INSERT INTO BA_CASH_OPERATIONS (datestamp, summa, comment) VALUES ('2015-01-03 00:00:00', -3.0, 'Расход3');
INSERT INTO BA_CASH_OPERATIONS (datestamp, summa, comment) VALUES ('2015-01-04 00:00:00', -4.0, 'Расход4');
INSERT INTO BA_CASH_OPERATIONS (datestamp, summa, comment) VALUES ('2015-01-05 00:00:00', -5.0, 'Расход5');

-- TODO: Add more samples