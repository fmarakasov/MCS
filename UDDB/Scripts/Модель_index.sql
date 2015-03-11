/*
/*
Система учёта договоров 1.0
Скрипт сгенерирован 25/11/201206:03:20 PM
1.48
   1) Удалена таблица актов передачи актов к договорам
   2) Таблица  ContractTranActDoc используется и для договоров и для актов
        Удалено ограничение UNIQUE
   3) Переименовано ограничение первичного ключа между ApprovalProcess и ApprovalState
1.47
   1) Задействована таблица версионности схемы БД (1.47 - текущая схема)
   2) Исправлено представление репозитория актов
1.46
    1) Добавлены процедуры для поддержки работы распоряжений
    2) Изменено представление реестра договоров для группировки по основному договору
    3) Черновая (!!!) версия представления реестра актов
    4) Удален Individual. Учёт физ.лиц ведётся с использованием изменённой таблицы контрагентов. 
1.45
    1) Добавлены кэш-поля в contractdoc (disbursed_cache, left_cache)
1.44
    1) Добавлен Individual
    2) Добавлен Education
    3) Введены договоры и этапы с открытыми датами (поля Delta и Comment)
    4) Введено согласование этапов и результатов
    5) Введено назначение ответственного по этапу
	6) Введены группировки типов договоров для отчётов в соответствие с контрагентами (ReportGrouping)
	7) Введено хранение фильтров договоров
	8) Изменена политика удаления акта с cascade на set null
 
  1.43:
	1)  Добавлено поле Images:blob для Contractdoc
	2) Добавлены индексы на внешние ключи для таблиц
	    с интенсивными операциями соединения
	3) Изменены вставляемые строки в WorkType и CurrencyMeasure
*/
*/
CREATE INDEX PrepNDS_Index 
  ON ContractDoc (PrepaymentNDSAlgorithmID);
CREATE INDEX ContractDoc_ContractTypeID 
  ON ContractDoc (ContractTypeID);
CREATE INDEX ContractDoc_CurrencyID 
  ON ContractDoc (CurrencyID);
CREATE INDEX ContractDoc_OriginContractID 
  ON ContractDoc (OriginContractID);
CREATE INDEX ContractDoc_NDSAlgorithmID 
  ON ContractDoc (NDSAlgorithmID);
CREATE INDEX ContractDoc_ContractStateID 
  ON ContractDoc (ContractStateID);
CREATE INDEX ContractDoc_CurrencyMeasureID 
  ON ContractDoc (CurrencyMeasureID);
CREATE INDEX ContractDoc_ContractorID 
  ON ContractDoc (ContractorID);
CREATE INDEX Contractor_ContractorTypeID 
  ON Contractor (ContractorTypeID);
CREATE INDEX Stage_NdsAlgorithmID 
  ON Stage (NdsAlgorithmID);
CREATE INDEX Stage_ParentID 
  ON Stage (ParentID);
CREATE INDEX Stage_ActID 
  ON Stage (ActID);
CREATE INDEX Stage_ScheduleID 
  ON Stage (ScheduleID);
CREATE INDEX Stage_NdsID 
  ON Stage (NdsID);
CREATE INDEX Act_ActTypeID 
  ON Act (ActTypeID);
CREATE INDEX Act_EnterpriceAuthorityID 
  ON Act (EnterpriceAuthorityID);
CREATE INDEX Act_NdsalgorithmID 
  ON Act (NdsalgorithmID);
CREATE INDEX Act_CurrencyID 
  ON Act (CurrencyID);
CREATE INDEX Act_CurrencymeasureID 
  ON Act (CurrencymeasureID);
CREATE INDEX StageResult_EFT_Index 
  ON StageResult (EconomicEfficiencyTypeID);
CREATE INDEX StageResult_NTPSubViewID 
  ON StageResult (NTPSubViewID);
CREATE INDEX NTPSubView_NTPViewID 
  ON NTPSubView (NTPViewID);
CREATE INDEX ReportGrouping_ContractorID 
  ON ReportGrouping (ContractorID);
