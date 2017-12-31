use MYWATCH2

GO



CREATE XML SCHEMA COLLECTION Visitors_xsd
AS
  '<?xml version="1.0" encoding="UTF-8" ?>
<xs:schema xmlns:xs="http://www.w3.org/2001/XMLSchema">
    <xs:element name="MeetVisitor">
      <xs:complexType>
        <xs:attribute name="FirstName" type="xs:string" use="required" />
        <xs:attribute name="LastName" type="xs:string" use="required" />
        <xs:attribute name="VID" type="xs:int" use="required" />
    </xs:complexType>
  </xs:element>
    <xs:element name="Visitors">
      <xs:complexType>
        <xs:sequence>
        <xs:element ref="MeetVisitor" maxOccurs="unbounded" />
    </xs:sequence>
    </xs:complexType>
  </xs:element>
</xs:schema>'

  GO


CREATE TABLE BigMeeting
(
  BMID          INT IDENTITY (1, 1) PRIMARY KEY,
  MeetName        NVARCHAR(50)          NOT NULL,
  Location         VARCHAR(30)          NOT NULL,
  MeetManager   INT NOT NULL,
  Visitors_xml XML(Visitors_xsd) NOT NULL
)

  SELECT * from BigMeeting

INSERT INTO BigMeeting
VALUES (N'Путешесвие в Рождество','Room1', 1,
        N'<Visitors>
    <MeetVisitor LastName="Титова" FirstName="Инна" VID="80" />
    <MeetVisitor LastName="Ежиков" FirstName="Антон" VID="90" />
</Visitors>'
),
  (N'Iphone','Room1', 1,
        N'<Visitors>
    <MeetVisitor LastName="Рыжкова" FirstName="Инна" VID="80" />
    <MeetVisitor LastName="Зайцева" FirstName="Екатерина" VID="90" />
</Visitors>'
),
  (N'Iphone 10','Room1', 1,
        N'<Visitors>
    <MeetVisitor LastName="Рыжкова" FirstName="Маша" VID="80" />
    <MeetVisitor LastName="Соколова" FirstName="Екатерина" VID="90" />
</Visitors>'
)


/* xml.exist() */

CREATE PROCEDURE HaveMeetVisitorsInna1
  AS
  BEGIN
    SELECT *
    FROM BigMeeting
    WHERE Visitors_xml.exist(N'/Visitors/MeetVisitor[@FirstName="Инна"]')=1
  END;

EXEC HaveMeetVisitorsInna1;



/* xml.value() */

CREATE PROCEDURE VisitorFromMeet @ID INT
  AS
BEGIN
  SELECT MeetName,
    Visitors_xml.value(N'/Visitors[1]/MeetVisitor[1]/@LastName', 'nvarchar(40)') AS 'Visitor LName',
    Visitors_xml.value(N'/Visitors[1]/MeetVisitor[1]/@FirstName', 'nvarchar(40)') AS 'Visitor LName'
  FROM BigMeeting
  WHERE BMID = @ID
END;

exec VisitorFromMeet  2;


/* xml.query() */

CREATE PROCEDURE VisitorFromXML @ID INT
  AS
BEGIN
  SELECT MeetName,
    Visitors_xml.query(N'/Visitors/MeetVisitor') AS Visitor
  FROM BigMeeting
  WHERE BMID = @ID
END;

exec VisitorFromXML  2;

/* xml.nodes() */


DECLARE @xml XML
SET @xml = N'
<Visitors>
    <MeetVisitor VID="1">
        <LastName>Рыжкова</LastName>
        <FirstName>Маша</FirstName>
    </MeetVisitor>
    <MeetVisitor VID="2">
        <LastName>Соколова</LastName>
        <FirstName>Екатерина</FirstName>
    </MeetVisitor>
    <MeetVisitor VID="3">
        <LastName>Зайцева</LastName>
        <FirstName>Екатерина</FirstName>
    </MeetVisitor>
</Visitors>'
SELECT T.ref.value('LastName[1]', 'nvarchar(50)') as MeetVisitors
  FROM @xml.nodes('/Visitors/MeetVisitor ') as T(ref);


---------------------------------------------------------------

CREATE TABLE GlobalVars
(sch xml)

INSERT INTO GlobalVars
VALUES (N'
<Visitors>
    <MeetVisitor VID="1">
        <LastName>Рыжкова</LastName>
        <FirstName>Маша</FirstName>
    </MeetVisitor>
    <MeetVisitor VID="2">
        <LastName>Соколова</LastName>
        <FirstName>Екатерина</FirstName>
    </MeetVisitor>
    <MeetVisitor VID="3">
        <LastName>Зайцева</LastName>
        <FirstName>Екатерина</FirstName>
    </MeetVisitor>
</Visitors>');
GO


/* xml.modify() */

DECLARE @xmll xml
SET @xmll = (
SELECT sch FROM GlobalVars)

/* INSERT */

SET @xmll.modify(N'insert <MeetVisitor VID = "4"> </MeetVisitor> as last into (/Visitors)[1]')
SET @xmll.modify(N'insert <LastName>Павлова</LastName> into (/Visitors/MeetVisitor[4])[1]')
SET @xmll.modify(N'insert <FirstName>Марина</FirstName> after (/Visitors/MeetVisitor[4]/LastName)[1]')
--SELECT @xmll

/* DELETE */
SET @xmll.modify(N'delete /Visitors/MeetVisitor[2]/@FirstName')
SET @xmll.modify(N'delete /Visitors/MeetVisitor[1]')
--SELECT @xmll

/* REPLACE OF */
SET @xmll.modify(N'replace value of (/Visitors/MeetVisitor[2]/FirstName[1]/text())[1] with "Женя"')
SET @xmll.modify(N'replace value of (/Visitors/MeetVisitor[2]/@VID)[1] with "100"')
SELECT @xmll





/* sql.column() */

SELECT BMID , MeetName  , Visitors_xml.query(
N'for $b in /Visitors/MeetVisitor
where $b/@LastName="Рыжкова"
return (<MeetVisitor>{$b}<Meeting id ="{sql:column("BMID")}">
{sql:column("MeetName")}</Meeting></MeetVisitor>)'
) as Visitor
FROM BigMeeting
WHERE Visitors_xml.exist(N'/Visitors/MeetVisitor[@LastName="Рыжкова"]')=1;



/* sql.variable() */

CREATE PROCEDURE UseVariable @var NVARCHAR(30)
  AS
BEGIN
  SELECT BMID , MeetName  , Visitors_xml.query(
N'for $b in /Visitors/MeetVisitor
where $b/@LastName="Рыжкова"
return (<Visitor tag="{sql:variable("@var")}">
{$b}<MeetID no="{sql:column("BMID")}">
{sql:column("MeetName")}</MeetID></Visitor>)'
) as Visitor
FROM BigMeeting
WHERE Visitors_xml.exist(N'//Visitors/MeetVisitor[@LastName="Рыжкова"]')=1
END


DROP PROCEDURE UseVariable;
EXEC UseVariable 'twenty'
