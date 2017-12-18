use MYWATCH2;
GO

/* Скалярную функцию (пример 1) */
CREATE FUNCTION MinutesManagerLoadsInDay(@personID int, @DateText VARCHAR(50))
  RETURNS INT
BEGIN
   DECLARE @Data DATETIME;
   DECLARE @count int;
  set @Data=CONVERT(DATE,@DateText)

  SELECT @count=COUNT(Status)
  FROM M_BUSY
  WHERE MID=@personID AND Status=1 AND @Data=CONVERT(DATE, Time)
  RETURN @count*15
END
SELECT  dbo.ManagerLoadInDay(1,'2017-11-01') as Load;



/* Подставляемую табличную функцию (пример 3) */

CREATE FUNCTION dbo.CompanyLoad(@Company int)
  RETURNS TABLE
  AS
  RETURN
  (
     SELECT N.С *15 as 'loadMinutes', Manager.*
     FROM
        (
            SELECT MID , count(M_BUSY.Status) AS С
            FROM M_BUSY
            WHERE STATUS =1
            GROUP BY MID
        ) AS N
      INNER JOIN Manager ON Manager.MID=N.MID
      WHERE @Company=Manager.CompID

  );

SELECT * FROM dbo.CompanyLoad(1)

/* Многооператорную табличную функцию (пример 4) */

CREATE FUNCTION dbo.GetMeetingsByManager(@mid int)
  RETURNS @tbl TABLE (id int PRIMARY KEY, MID INT,  VID INT,TimeStart DATETIME,TimeEnd DATETIME)
  AS
  BEGIN
    INSERT @tbl(id, MID, VID, TimeStart,TimeEnd)
      SELECT Meet_ID, MID, VID,TimeStart, TimeEnd
      FROM Meeting
      WHERE MID=@mid;
    RETURN
  END;

SELECT * FROM dbo.GetMeetingsByManager(1)

/* Рекурсивную функцию(пример 2)*/

CREATE FUNCTION dbo.findnod(@A int,@B int)
RETURNS INT WITH RETURNS NULL ON NULL INPUT
  AS
  BEGIN
    DECLARE @R int;
    IF (@A % @B<>0)
      BEGIN
        set @R =@A % @B
        RETURN dbo.findnod(@B,@R)
      END
    ELSE
        RETURN @B
    return NULL;
  END;

SELECT dbo.findnod(60, 80)

/* функцию с рекурсивным ОТВ (пример  5) */
CREATE FUNCTION dbo.FindNODDD(@A int,@B int)
  RETURNS INT
  WITH RETURNS NULL ON NULL INPUT
  AS
  BEGIN
    DECLARE @result float;
    SET @result = NULL;
      BEGIN
        WITH NOD(num1,num2)
        AS
        (
          SELECT @A, @B
          UNION ALL
          SELECT num2,num1 % num2
          FROM NOD WHERE num1 % num2<>0
        )
        SELECT @result=num2
        FROM NOD
      END
    RETURN @result;
  END

SELECT  dbo.FindNODDD(40,80)

/* Хранимую процедуру без параметров или с параметрами (примеры 6, 7, 8, 9) */

CREATE PROCEDURE dbo.M_BUSY_Status_UPDATE(@MID INT ,@T1 DATETIME  , @T2 DATETIME)
  AS
BEGIN
  UPDATE M_BUSY SET Status=1
      WHERE M_BUSY.MID=@MID AND Time BETWEEN @T1 AND @T2;
END
GO

SELECT *
from M_BUSY
WHERE MID=10 AND Time BETWEEN
  CONVERT(DATETIME,'2017-11-04 09:00:00.000') AND
  CONVERT(DATETIME,'2017-11-04 11:00:00.000')

EXECUTE M_BUSY_Status_UPDATE 10, "2017-11-04 09:00:00.000", "2017-11-04 11:00:00.000"



/* Хранимую процедуру с курсором (пример 11) */

CREATE PROCEDURE CursorMeeting2
  AS
  BEGIN
    DECLARE @counter int = 0
    DECLARE @totalMeeting int = 0
    DECLARE @Conf int
    DECLARE MyCursor CURSOR
    GLOBAL
    FOR
      SELECT M_Confirmation
      FROM Meeting
      WHERE MID = 1
    OPEN MyCursor;
     FETCH NEXT FROM MyCursor;
    WHILE (@@FETCH_STATUS = 0)
    BEGIN
      PRINT 'Price of tour  '
      SELECT @counter = @counter + 1;
      FETCH NEXT FROM MyCursor INTO @Conf;
      SELECT @totalMeeting = @totalMeeting + @Conf;
      PRINT 'Price of tour  ' + CAST(@counter AS VARCHAR(10)) + ': '+CAST(@totalMeeting AS VARCHAR(10));
    END
  END;

EXECUTE CursorMeeting2;

CLOSE MyCursor;
DEALLOCATE MyCursor;

DROP PROCEDURE CursorMeeting2;

