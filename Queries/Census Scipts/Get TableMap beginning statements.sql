use Geography
go

-- like i said twice now, yeah, they're bursting with happiness from doing the same stupid shit over and over.
-- the only people likely even enjoying this slightly as they rut about their miserable lives are the people who design all of this
-- and even they, one can hope, have a deep black dark hole gnawing away at their center
-- miserable wasteful lazy mother fuckers :)

select script.i, script.TABLE_NAME,script.code from
(

select 1 as i,dt.TABLE_NAME, CONCAT('dr["', dc.COLUMN_NAME,'"]= this.') as code  from INFORMATION_SCHEMA.TABLES dt
inner join INFORMATION_SCHEMA.COLUMNS dc
on dc.TABLE_NAME=dt.TABLE_NAME
where not dc.Column_name in ('Shape','MinLatitude','MaxLatitude','MinLongitude','MaxLongitude')

union

select 0 as i, dt.TABLE_NAME, 'DataRow dr = tgt.NewRow();' as code from INFORMATION_SCHEMA.TABLES dt

union

select 2 as i, dt.TABLE_NAME,'dr["Shape"] = this.Shape?.GetMSSQLInstance();

            var bounding = this.Shape?.GetExtent();

            if (bounding != null)
            {
                dr["MinLatitude"] = bounding.X1;
                dr["MinLongitude"] = bounding.Y1;
                dr["MaxLatitude"] = bounding.X2;
                dr["MaxLongitude"] = bounding.Y2;
            }
			' as code
from INFORMATION_SCHEMA.TABLES dt

union 

select 3 as i, dt.TABLE_NAME, 'tgt.Rows.Add(dr);' as code from INFORMATION_SCHEMA.TABLES dt) script

order by script.TABLE_NAME, script.i


-- so fucking biden alreayd pushed this idea about giving everyone housing and my opinion is these little fucking chomo fucks were stealing resources
-- impersonating john r sohn which is why he, a capable developer, admin, and when he was far younger, capable worker in general, got stuck in the street
-- and they appear to be living on fucking air.
-- isnt that just great. a lifetime spent making things, and before the idea gets very far they steal it. or before the writing gets very far they steal it
-- or before the program is finishe dthey steal it like they have a fucking right along with stealing money, benefits, time, property etc
-- or 'borrowing' it so they pretend to be human, while meanwhile this individual gets shafted 
-- and yeah this is the same rant because theyve been fucking with my memory since i was kid and my good old dad pretended like i was never raped
-- and went right back to being the loving 'hero'.
-- also put me in a jail as a child a few days and then reappeared pretending to be scared after he so calmly walked me in there and left me in a cell
-- led to another blackout.
-- so biden, what are you doing for US ? seems to me you're just helping them pretend to be decent folks that sell their ass MINIMALLY to get free housing
-- and steady jobs. even if they are shit jobs SOMETIMES, or most times, SOMETIMES THEY'RE NOT.
-- maybe biden died of stroke like they hinted at. heh. me i'm surprised i havent. the fucking atom bomb of fucking bullshit i remember while here.

-- oh and HEY you may want to put ACTUAL federal agents in your federal buildings assholes
-- so that if one of us wanders in their looking for help we dont find hell
-- like oh i dunno

-- langley
-- dc
-- philadelphia
-- denver
-- alexandria
-- dallas
-- san antonio

-- and you may also want to remove the fake ones they occasionally open in places like fucking loveland !

-- seriously what are you fucking people even doing alive if you cant keep your own house clean and prevent reckless greed and lust
-- from donating sperm to chomos who cant get their dicks hard for anything but little boys and allowing them to fucking breed
-- and take advantage of men who dont know what they are ???

-- i mean jesus, if you wanted some help trapping them by making them think they were taking advantage, still expect a fucking salary
-- instead i think the jollies are on both sides of the fucked up equation, less enduringly the assholes that tried the hardest to fuck us.

-- personally dont like what seems to have been being a forcible sacrifice that gets ressurrected and thrown back to the fucking canaanites again after a few of them
-- are mutilated, jailed, or put in an unending loop of pain they'll grow tired of before realizings its not worth it!

-- personally when previously writing this found the word canaanite to be fairly well chosen.

-- btw what you tell these people ? that they'd be free some day ? that they should keep their bodies perfect and drink only water and not smoke etc while creating
-- via the fuckin harsh sun and mistreatment of people the appearance of premature aging at points ? while actually causing others to prematurely age by distributing crack and meth ?