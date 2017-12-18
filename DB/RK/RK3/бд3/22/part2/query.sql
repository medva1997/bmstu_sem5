DECLARE @xml xml;
SELECT @xml = x FROM OPENROWSET(BULK 'C:/Users/User/Desktop/DB/Natan/module3/part2/XMLFile2.xml', SINGLE_BLOB) AS TEMP(x);
SELECT @xml.query(' let $all := for $course in /courses/course
					return $course/instructor	

					let $dist := for $course in /courses/course
					return distinct-values($course/instructor)

					for $i in $all
					where count(fn:index-of($all, $i)) > 1
					return $i
');
GO