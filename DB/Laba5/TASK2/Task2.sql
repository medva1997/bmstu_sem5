/*Задание 2 
	С помощью функции OPENXML и OPENROWSET выполнить загрузку и
 сохранение XML-документа в таблице базы данных, созданной в ЛР № 1. */

 use MYWATCH2;
 GO

 -- Объявляем переменные 
 DECLARE @idoc      int; -- Дескриптор документа внутреннего представления XML-документа 
 DECLARE @xmldoc    nvarchar(4000); -- XML-документ -- Определяем XML-документ 
 SET @xmldoc = '

 <ROOT> 
<Manager>
  <MID>112</MID>
  <CompID>1</CompID>
  <LastName>Gorohova</LastName>
  <FirstName>Irina</FirstName>
  <SecondName>Borisovna</SecondName>
  <Email>ira@medalexey.ru</Email>
  <Password>49D82B501375008EBD0CB9CBD3485E17</Password>
  <PhoneNumber>79262300357</PhoneNumber>
</Manager>
<Manager>
  <MID>113</MID>
  <CompID>1</CompID>
  <LastName>Orekhova</LastName>
  <FirstName>Ekaterina</FirstName>
  <SecondName>Olegovna</SecondName>
  <Email>kate@medalexey.ru</Email>
  <Password>49D72B501375008EBD0CB9CBD3485E17</Password>
  <PhoneNumber>79262300356</PhoneNumber>
</Manager>
</ROOT> ';
-- При помощи вызова процедуры sp_xml_preparedocument создаем внутреннее представление
-- XML-документа @xmldoc, на которое будем ссылаться в дальнейшем с помощью дескриптора @idoc
EXEC sp_xml_preparedocument @idoc OUTPUT, @xmldoc;

select * from Manager where MID>105
INSERT INTO Manager(MID,CompID, LastName,FirstName,SecondName,Email,Password,PhoneNumber)
SELECT * FROM OPENXML (@idoc, N'/ROOT/Manager', 2) WITH 
	(MID    INT ,  CompID INT ,  LastName    NVARCHAR(60), FirstName   NVARCHAR(60),  SecondName  NVARCHAR(60) ,
	  Email  NVARCHAR(60) ,   Password    NVARCHAR(100),  PhoneNumber NVARCHAR(11)); 
select * from Manager where MID>105
-- Удаляем XML-документ из памяти 
EXEC sp_xml_removedocument @idoc; 


--OPENROWSET
 -- Объявляем переменные 
 DECLARE @idoc1      int; -- Дескриптор документа внутреннего представления XML-документа 
 DECLARE @xmldoc1    xml; -- XML-документ -- Определяем XML-документ 
 SELECT @xmldoc1 = c FROM OPENROWSET(BULK 'C:\Users\medva\Documents\REPOS\DB\laba5\TASK2\file.xml', SINGLE_BLOB) AS TEMP(c) 
 
-- При помощи вызова процедуры sp_xml_preparedocument создаем внутреннее представление
-- XML-документа @xmldoc, на которое будем ссылаться в дальнейшем с помощью дескриптора @idoc
EXEC sp_xml_preparedocument @idoc1 OUTPUT, @xmldoc1;
select * from Manager where MID>105
INSERT INTO Manager(MID,CompID, LastName,FirstName,SecondName,Email,Password,PhoneNumber)
SELECT * FROM OPENXML (@idoc1, N'/ROOT/Manager', 2) WITH 
	(MID    INT ,  CompID INT ,  LastName    NVARCHAR(60), FirstName   NVARCHAR(60),  SecondName  NVARCHAR(60) ,
	  Email  NVARCHAR(60) ,   Password    NVARCHAR(100),  PhoneNumber NVARCHAR(11)); 

select * from Manager where MID>105
-- Удаляем XML-документ из памяти 
EXEC sp_xml_removedocument @idoc1; 
