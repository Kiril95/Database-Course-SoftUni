CREATE FUNCTION ufn_IsWordComprised(@setOfLetters NVARCHAR(30), @word NVARCHAR(30)) 
RETURNS BIT AS
BEGIN
    DECLARE @result BIT
    DECLARE @counter INT = 1

    WHILE(@counter <= LEN(@word))
		BEGIN
			DECLARE @currLetter CHAR(1) = SUBSTRING(@word, @counter, 1)
			IF (CHARINDEX(@currLetter, @setOfLetters) = 0) 
				SET @result = 0
			ELSE 
				SET @result = 1

			SET @counter += 1
		END
		
    RETURN @result
END

SELECT dbo.ufn_IsWordComprised('bobr', 'rob')
SELECT dbo.ufn_IsWordComprised('pppp', 'Guy')
