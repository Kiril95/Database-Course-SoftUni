CREATE FUNCTION ufn_CalculateFutureValue(@sum DECIMAL(10,4), @yearlyRate FLOAT, @years INT)
RETURNS DECIMAL(10,4)
	BEGIN
		DECLARE @result DECIMAL(10,4)
		SET @result = @sum * POWER((1 + @yearlyRate), @years)

		RETURN @result
	END

SELECT dbo.ufn_CalculateFutureValue(1000, 0.1, 5) as [Result]