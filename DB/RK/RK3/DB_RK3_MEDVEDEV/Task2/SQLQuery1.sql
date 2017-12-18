DECLARE @xml xml;
SELECT @xml = x FROM OPENROWSET(BULK 'D:\DB_RK3_MEDVEDEV\Task1\Medvedev_task1\Medvedev_task1\bin\Debug\XMLFile1.xml', SINGLE_BLOB) AS TEMP(x);
SELECT @xml.query('for $book in /Books/Book
					where count($book/Authors/Author) = 1
					return $book/Title
');
GO
