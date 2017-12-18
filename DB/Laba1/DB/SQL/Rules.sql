use MYWATCH2;
GO

ALTER TABLE Manager
    ADD
CONSTRAINT ch_Mphone CHECK (PhoneNumber LIKE   N'[0-9][0-9][0-9][0-9][0-9][0-9][0-9][0-9][0-9][0-9][0-9]'),
CONSTRAINT ch_Memail CHECK (Email LIKE N'%_@_%._%')

GO

ALTER TABLE Visitor
    ADD
CONSTRAINT ch_Vphone CHECK (PhoneNumber LIKE   N'[0-9][0-9][0-9][0-9][0-9][0-9][0-9][0-9][0-9][0-9][0-9]'),
CONSTRAINT ch_Vemail CHECK (Email LIKE N'%_@_%._%')

GO

CREATE RULE pass_rule  AS LEN(@password)>= 8;
GO
EXEC  sp_bindrule 'pass_rule', 'Manager.password'
EXEC  sp_bindrule 'pass_rule', 'Visitor.password'
GO
