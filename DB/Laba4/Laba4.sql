/*
Создать, развернуть и протестировать 6 объектов SQL CLR:
1)	Определяемую пользователем скалярную функцию CLR,
2)	Пользовательскую агрегатную функцию CLR,
3)	Определяемую пользователем табличную функцию CLR,
4)	Хранимую процедуру CLR,
5)	Триггер CLR,
6)	Определяемый пользователем тип данных CLR.

*/

USE MYWATCH2
GO

sp_configure 'show advanced options', 1
GO
RECONFIGURE
GO
sp_configure 'clr enabled', 1
GO
RECONFIGURE
GO

/* Скалярная функция CLR */

CREATE ASSEMBLY HandWrittenUDF
FROM
'D:\bmstu_sem5\DB\Laba4\HandWrittenUDF\HandWrittenUDF\bin\Debug\HandWrittenUDF.dll'
GO

drop FUNCTION GetDate;
drop TRIGGER SqlTrigger1;
drop ASSEMBLY HandWrittenUDF;


CREATE FUNCTION GetDate (@timeStart DATETIME,@timeEnd DATETIME)
RETURNS INT
AS
EXTERNAL NAME
HandWrittenUDF.[HandWrittenUDF.UserDefinedFunctions].GetDate
GO

/*
 [Microsoft.SqlServer.Server.SqlFunction]
  public static SqlInt32 GetDate(SqlDateTime timeStart, SqlDateTime timeEnd)
  {
      TimeSpan temp = ((DateTime)timeEnd) - ((DateTime)timeStart);
      return (SqlInt32)(temp.TotalMinutes);
  }
 */


SELECT dbo.GetDate('2017-11-02 11:45:00.000','2017-11-02 11:50:00.000' )

SELECT dbo.GetDate(TimeStart, TimeEnd),TimeStart, TimeEnd from Meeting
GO


/* Агрегратная функция CLR */

SELECT MID, Count(Meet_ID)
from Meeting
GROUP BY MID
ORDER BY MID

SELECT MID, dbo.MeetingCounter2(Meet_ID)
from Meeting
GROUP BY MID
ORDER BY MID




/* Табличная функция CLR */

SELECT Meet_id, vid, timestart, timeend from MYWATCH2.dbo.ManagerMeetings(1)
except
SELECT Meet_id, vid, time1, time2 from temp2

/* Хранимая процедура */

CREATE TABLE  temp2
(
  Meet_id int,
  Vid int,
  Time1 DATETIME,
  Time2 DATETIME
)
  INSERT  INTO  temp2 EXEC GetInfoByManagerMettings 1



/* Триггер CLR */

CREATE TRIGGER SqlTrigger1
  on Visitor
  FOR UPDATE
  AS EXTERNAL NAME HandWrittenUDF.[HandWrittenUDF.Trig].SqlTrigger1

UPDATE Visitor
SET PhoneNumber=N'1111'
WHERE VID=5;

UPDATE Visitor
SET PhoneNumber=N'79262300359'
WHERE VID=5;


/* Определяемый пользователем тип данных CLR */

CREATE TABLE test(
pass passport)

insert into test (pass) values('4321 567890')
SELECT CAST(pass AS varchar(8000))FROM test
SELECT CAST(pass.Seria AS varchar(8000)) from test;


drop TABLE  test;
