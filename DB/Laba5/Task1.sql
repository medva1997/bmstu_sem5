/*Задание 1  
Из таблиц базы данных, созданной в ЛР № 1,
извлечь данные с помощью конструкции FOR XML;
проверить все режимы конструкции FOR XML.
Если написание запросов режима EXPLICIT окажется трудоемким,
то обратиться к разделу MSDN Library «Использование режима EXPLICIT совместно с предложением FOR XML»
по адресу https://msdn.microsoft.com/ru-ru/library/ms189068(v=sql.120).aspx. 
*/
 Use MYWATCH2;

-- AUTO 
  SELECT top 1 MID, LastName, FirstName
  from Manager
  FOR XML AUTO, ELEMENTS 
 /*
  <Manager>
  <MID>1</MID>
  <LastName>Водопьянова</LastName>
  <FirstName>Дарья</FirstName>
</Manager>
  */

  --RAW  
  SELECT top 1 MID, LastName, FirstName
  from Manager  FOR XML RAW('XAXAXA'), ELEMENTS
/*
<XAXAXA>
  <MID>1</MID>
  <LastName>Водопьянова</LastName>
  <FirstName>Дарья</FirstName>
</XAXAXA>
*/

--explicit
SELECT 1 as tag,
    null as parent,
    LastName as 'man!1!lname',
    AdminID as 'man!1!admin!element'
FROM Manager
for xml explicit
