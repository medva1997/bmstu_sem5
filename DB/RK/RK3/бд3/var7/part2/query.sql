DECLARE @xml xml;
SELECT @xml = x FROM OPENROWSET(BULK 'C:/Users/User/Desktop/DB/Misha/module3/part2/XMLFile1.xml', SINGLE_BLOB) AS TEMP(x);
SELECT @xml.query('for $book in /Books/Book
					where $book/Price = min(Books/Book/Price[1])
					return $book/Title
');
GO
