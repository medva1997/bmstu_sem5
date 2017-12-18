use MYWATCH2
--1.	Инструкция SELECT, использующая предикат сравнения.
      --вывести компании в которых работают первые 20 сотрудники упорядочив по номеру записи о сотруднике
      SELECT Company.CompName
      FROM Manager JOIN Company ON Company.CompID=Manager.CompID
        WHERE MID<20
      ORDER BY MID


--2.	Инструкция SELECT, использующая предикат BETWEEN.
      -- Получить список номеров встреч, начинающихся в указанный интервал
      SELECT Meet_ID, TimeStart
      FROM Meeting
      WHERE TimeStart BETWEEN  '2017-11-02 11:45:00.000' AND '2017-11-02 13:45:00.000';


--3.	Инструкция SELECT, использующая предикат LIKE.
      --Получить список номеров телефонов которые заканчиваются на 3 и начинаются с 79262
      SELECT PhoneNumber
        FROM Manager
      WHERE PhoneNumber LIKE N'79262%3';

--4.	Инструкция SELECT, использующая предикат IN с вложенным подзапросом.
      --Получить id пользователей которые встречаются с администраторами из г Мытищи
      SELECT VID, MID
      FROM Meeting
      WHERE MID IN
            (
              SELECT CompAdminMID
              FROM Company
              WHERE CompCity=N'Мытищи'
            )

--5.	Инструкция SELECT, использующая предикат EXISTS с вложенным подзапросом.
      -- Вывести ID людей у которых нету встреч
      SELECT DISTINCT VID
      FROM Visitor
      WHERE Not EXISTS(
          SELECT Meeting.VID
          FROM Meeting
        WHERE Visitor.VID=Meeting.VID
      )

--TODO 11.	Создание новой временной локальной таблицы из результирующего набора данных инструкции SELECT.
DROP TABLE M_loads

SELECT MID, count(M_BUSY.Status) AS m_loading
INTO M_loads
FROM M_BUSY
WHERE STATUS =1
GROUP BY MID

-- 6.	Инструкция SELECT, использующая предикат сравнения с квантором.
SELECT MID, m_loading
FROM M_loads
WHERE m_loading> ALL
      (
        SELECT m_loading
        FROM M_loads
        JOIN Manager ON Manager.MID=M_loads.MID
        WHERE CompID=3
      )
 SELECT m_loading
        FROM M_loads
        JOIN Manager ON Manager.MID=M_loads.MID
        WHERE CompID=3
--7.	Инструкция SELECT, использующая агрегатные функции в выражениях столбцов.
      --статистика по занятости менеджеров компании
      SELECT CompID, sum(N.С) as 'Sum load', avg(N.С) AS 'AVG M LOAD', min(N.С)AS "MAX M LOAD", MAX(N.С) AS 'MAX M LOAD'
      FROM
        (
            SELECT MID, count(M_BUSY.Status) AS С
            FROM M_BUSY
            WHERE STATUS =1
            GROUP BY MID
        ) AS N
      INNER JOIN Manager ON Manager.MID=N.MID
      GROUP BY CompID
--8.	Инструкция SELECT, использующая скалярные подзапросы в выражениях столбцов.
      SELECT CompID,
        (
          --количество сотруников в компании
          SELECT count(MID)
          FROM Manager
          WHERE CompID=Company.CompID
        ) as NManagers
      FROM Company
      WHERE CompCity=N'Мытищи';
-- 9.	Инструкция SELECT, использующая простое выражение CASE.
      --превращаем метки занятости в человеческий вид
      SELECT
        CASE V_BUSY.Status
          WHEN 1 THEN N'BUSY'
          ELSE N'FREE'
          END AS st
      FROM V_BUSY


--10.	Инструкция SELECT, использующая поисковое выражение CASE.
      --Выводим чье подтверждение мы ждем
      SELECT Meeting.MID,
      CASE
        WHEN V_confirmation=1 AND M_Confirmation=1 THEN N'Состоится'
        WHEN V_confirmation=1 AND M_Confirmation=0 THEN N'Ждем менеджера'
        WHEN V_confirmation=0 AND M_Confirmation=1 THEN N'Ждем посетителя'
        ELSE N'Ждем обоих'
        END as Confirmation
      FROM Meeting;

      SELECT Meeting.MID,
      CASE
        WHEN cast(TimeStart as time) < cast('12:00:00.00' as time) THEN N'Morning'
        WHEN cast(TimeStart as time) < cast('15:00:00.00' as time) THEN N'Day'
        ELSE N'Evening'
        END as Confirmation
      FROM Meeting;


-- 12.	Инструкция SELECT, использующая вложенные коррелированные подзапросы в качестве производных таблиц в предложении FROM.

SELECT Manager.MID, FirstName, t.m_loading
FROM Manager JOIN (
  SELECT MID, count(M_BUSY.Status) AS m_loading
  FROM M_BUSY
  WHERE STATUS =1
  GROUP BY MID) as T ON T.MID=Manager.MID
--13.	Инструкция SELECT, использующая вложенные подзапросы с уровнем вложенности 3.
      --Кто с кем встречается и во сколько
      SELECT
        T2.Meet_ID, T2.TimeStart, T2.TimeEnd, T2.M_FIO, Visitor.LastName+' '+ Visitor.FirstName AS V_FI0
      FROM
        (
          SELECT
            T1.Meet_ID, T1.TimeStart, T1.TimeEnd, Manager.LastName+' '+ Manager.FirstName AS M_FIO,T1.VID
          FROM (
                 SELECT
                   Meet_ID, TimeStart, TimeEnd, MID, VID
                 FROM Meeting
                 WHERE TimeStart BETWEEN '2017-11-02 11:45:00.000' AND '2017-11-02 13:45:00.000'
               ) AS T1
            INNER JOIN Manager ON Manager.MID = T1.MID
        ) AS T2
      INNER JOIN Visitor ON T2.VID=dbo.Visitor.VID
--14.	Инструкция SELECT, консолидирующая данные с помощью предложения GROUP BY, но без предложения HAVING.
      --сколько времени занят каждый из менеджеров
      SELECT MID,count(M_BUSY.Status)
      FROM M_BUSY
      WHERE Status=1
      GROUP BY MID
--15.	Инструкция SELECT, консолидирующая данные с помощью предложения GROUP BY и  предложения HAVING.
      --Вывести mid, которые имеюют занятость больше 15 часов
      SELECT MID,count(M_BUSY.Status)
      FROM M_BUSY
      WHERE Status=1
      GROUP BY MID
      HAVING count(M_BUSY.Status)>15*4

--16.	Однострочная инструкция INSERT, выполняющая вставку в таблицу одной строки значений.
      INSERT Visitor (VID,LastName, FirstName, SecondName, Email, Password, PhoneNumber)
      VALUES (1003,N'Медведев',N'Алексей',N'Вячеславович',N'web@medalexey.ru',N'98765432',N'79262300359');
      SELECT * FROM Visitor WHERE VID=1001;
--17.	Многострочная инструкция INSERT, выполняющая вставку в таблицу результирующего набора данных вложенного подзапроса.
INSERT M_loads (MID, m_loading)
SELECT
  (SELECT max(MID)+1 FROM M_loads),
  (SELECT avg(m_loading)*2  FROM M_loads  WHERE MID>50)

SELECT * from M_loads
--18.	Простая инструкция UPDATE.
    -- Сбрасываем подтверждение встреч у манагера №2
    UPDATE Meeting
    SET V_confirmation=0
    WHERE VID=2
    SELECT * FROM Meeting WHERE VID=2;
--19.	Инструкция UPDATE со скалярным подзапросом в предложении SET.
      SELECT * FROM M_loads WHERE MID=10;

      UPDATE M_loads
          SET M_loads.m_loading =
          (
            SELECT min(m_loading)
            FROM M_loads
            WHERE MID>5
          )
      WHERE MID=10;

--20.	Простая инструкция DELETE
      --Удаляем встечи где нету обоих подтверждений
      DELETE Meeting
      WHERE V_confirmation=0 AND M_Confirmation=0

--21.	Инструкция DELETE с вложенным коррелированным подзапросом в предложении WHERE.

SELECT * FROM M_loads;
      DELETE FROM M_loads
          WHERE M_loads.MID
            IN (
                SELECT M_loads.MID
                FROM M_loads
                WHERE m_loading>150
                )

--22.	Инструкция SELECT, использующая простое обобщенное табличное выражение.
      WITH CTE (CompID, N_busy)
      AS
      (
          SELECT MID,count(M_BUSY.Status) as Total
          FROM M_BUSY
          WHERE Status=1
          GROUP BY MID
      )
      SELECT avg(N_busy)/4 AS N'Среднее время занятости'
      FROM CTE

--23.	Инструкция SELECT, использующая рекурсивное обобщенное табличное выражение.
      --вывод номера в ерархии компании
      UPDATE Manager
      SET AdminID=CompID

      UPDATE Manager
      SET AdminID=NULL
      WHERE CompID=MID;

      UPDATE Manager SET AdminID=54 WHERE MID=99

      WITH TestCTE(UserID, FirstName ,ManagerID , LevelUser)
      AS
       (

        SELECT Manager.MID as UserID, Manager.FirstName , Manager.AdminID as ManagerID, 1 AS LevelUser
          FROM Manager  WHERE AdminID IS NULL
        --Находим якорь рекурсии
        UNION ALL
        --Делаем объединение с TestCTE (хотя мы его еще не дописали)
        SELECT t1.MID, t1.FirstName, t1.AdminID, t2.LevelUser + 1
        FROM Manager AS t1
        JOIN TestCTE as t2 ON t1.AdminID=t2.UserID
       )
       SELECT * FROM TestCTE ORDER BY LevelUser
       OPTION (MAXRECURSION 5)


--24
SELECT CompID,avg(m_loading) AS avg_
FROM M_loads
JOIN Manager on M_loads.MID=Manager.MID
GROUP BY CompID


SELECT [avg_],
 [11],[12],[14],[15]
 FROM (SELECT  'average price' AS 'avg_', CompID, m_loading
       FROM M_loads
       JOIN Manager on M_loads.MID=Manager.MID
       ) x
 PIVOT
 (avg(m_loading)
 FOR CompID
 IN([11],[12],[14],[15])
 ) pvt;


SELECT CompID,avg2 AS avg_
FROM (
  SELECT
    [avg_],
    [11],
    [12],
    [14],
    [15]
  FROM (SELECT
          'average price' AS 'avg_',
          CompID,
          m_loading
        FROM M_loads
          JOIN Manager ON M_loads.MID = Manager.MID
       ) x
  PIVOT
  (avg(m_loading)
  FOR CompID
  IN ([11], [12], [14], [15])
  ) pvt
) pvt

UNPIVOT (avg2
  FOR CompID
  IN ([11], [12], [14], [15])
) unpv;

--25

SELECT FirstName, LastName, Email,PhoneNumber
INTO Users
FROM Visitor

MERGE INTO Users AS p
   USING (   SELECT FirstName, LastName, Email,PhoneNumber FROM Manager) p1
   ON (p.PhoneNumber = p1.PhoneNumber)
   WHEN NOT MATCHED THEN INSERT (FirstName, LastName, Email,PhoneNumber )
    VALUES (p1.FirstName, p1.LastName, p1.Email,p1.PhoneNumber );