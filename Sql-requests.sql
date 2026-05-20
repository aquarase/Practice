--Выборка данных с фильтрацией, сортировкой
select * from Characters where CharacterLevel < 10

select * from Characters
order by CharacterLevel desc

--Удаление, изменение данных
insert into Characters values
	(5, 'XD', 1, 0, null)

update Characters 
set CharacterLevel = 2
where CharacterLevel = 1

delete from Characters where CharacterLevel = 2

--Выборка с группировкой
select count(characterID) as 'Quest count', QuestStatus from CharacterQuests
group by QuestStatus

--Выборка из нескольких связанных таблиц (левое, правое соединение, пересечение)
SELECT Characters.CharacterName, Guilds.GuildName FROM Characters
INNER JOIN Guilds ON Characters.GuildID = Guilds.GuildID;

SELECT Characters.CharacterName, Guilds.GuildName FROM Characters
 LEFT JOIN Guilds ON Characters.GuildID = Guilds.GuildID;

SELECT Characters.CharacterName, Guilds.GuildName FROM Characters
RIGHT JOIN Guilds ON Characters.GuildID = Guilds.GuildID;
