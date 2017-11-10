BULK INSERT MYWATCH2.dbo.Manager
FROM 'C:\Users\medva\source\repos\Labs_DB\DB\outDATA\managers.txt'
WITH (CODEPAGE = '65001', DATAFILETYPE = 'char', FIELDTERMINATOR = ';', ROWTERMINATOR = '\n')

Go

BULK INSERT MYWATCH2.dbo.Visitor
FROM 'C:\Users\medva\source\repos\Labs_DB\DB\outDATA\visitors.txt'
WITH (CODEPAGE = '65001', DATAFILETYPE = 'char', FIELDTERMINATOR = ';', ROWTERMINATOR = '\n')


Go
BULK INSERT MYWATCH2.dbo.Company
FROM 'C:\Users\medva\source\repos\Labs_DB\DB\outDATA\comp.txt'
WITH (CODEPAGE = '65001', DATAFILETYPE = 'char', FIELDTERMINATOR = ';', ROWTERMINATOR = '\n')

Go
BULK INSERT MYWATCH2.dbo.M_BUSY
FROM 'C:\Users\medva\source\repos\Labs_DB\DB\outDATA\busy_managers.txt'
WITH (CODEPAGE = '65001', DATAFILETYPE = 'char', FIELDTERMINATOR = ';', ROWTERMINATOR = '\n')

Go
BULK INSERT MYWATCH2.dbo.V_BUSY
FROM 'C:\Users\medva\source\repos\Labs_DB\DB\outDATA\busy_visitors.txt'
WITH (CODEPAGE = '65001', DATAFILETYPE = 'char', FIELDTERMINATOR = ';', ROWTERMINATOR = '\n')

Go
BULK INSERT MYWATCH2.dbo.Meeting
FROM 'C:\Users\medva\source\repos\Labs_DB\DB\outDATA\meetings.txt'
WITH (CODEPAGE = '65001', DATAFILETYPE = 'char', FIELDTERMINATOR = ';', ROWTERMINATOR = '\n')

Go