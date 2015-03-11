/*
     UPDATE TO 1.0.0.25
	 Потенциальный ключ "Результаты этапа" теперь включает номер этапа и название результата
	 Длина поля названия результата  увеличена до 2000 символов.
*/
ALTER TABLE StageResult DROP CONSTRAINT U_STAGE_RESULT;
ALTER TABLE StageResult ADD CONSTRAINT U_STAGE_RESULT UNIQUE (StageID, Name);
ALTER TABLE StageResult MODIFY Name NVARCHAR2(2000) NOT NULL;