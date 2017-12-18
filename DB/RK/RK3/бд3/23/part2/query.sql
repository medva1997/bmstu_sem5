DECLARE
	@idoc INT DECLARE
		@doc xml SELECT
			@doc = c
		FROM
			OPENROWSET (
				BULK 'C:\Users\User\Desktop\DB\Natan\module3\23\part2\xmlFile1.xml',
				SINGLE_BLOB
			) AS TEMP (c) EXEC sp_xml_preparedocument @idoc OUTPUT,
			@doc SELECT
				@doc.query (
					'
for $b in /Books/Book
where contains(string(data($b/@Topic)), "XML")
return ($b/Title)
'
				);

