USE Geography
GO


-- look for any rows that dont have a matching state
SELECT * FROM DBO.Census2018 CE
left JOIN DBO.STATELOOKUP SL
ON CE.StateName=Sl.StateName
where sl.State is null

go

update 
dbo.Census2018
set State = sl.State
from
dbo.Census2018 ce
inner join dbo.StateLookup sl
on sl.StateName=ce.StateName

go

-- look for missed rows.

select * from dbo.Census2018 where State='xx'

go