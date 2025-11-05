CREATE function TimeOnly(@DateTime DateTime)
-- returns only the time portion of a DateTime
returns datetime
as
    begin
    return @DateTime - dbo.getformatteddatepart(@DateTime)
    end
