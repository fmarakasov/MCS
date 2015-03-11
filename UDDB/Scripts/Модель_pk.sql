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
ALTER TABLE ContractDoc ADD PRIMARY KEY (ID);
ALTER TABLE ContractType ADD PRIMARY KEY (ID);
ALTER TABLE Currency ADD PRIMARY KEY (ID);
ALTER TABLE NDSAlgorithm ADD PRIMARY KEY (ID);
ALTER TABLE Prepayment ADD PRIMARY KEY (ID);
ALTER TABLE Person ADD PRIMARY KEY (ID);
ALTER TABLE Contractor ADD PRIMARY KEY (ID);
ALTER TABLE Degree ADD PRIMARY KEY (ID);
ALTER TABLE NDS ADD PRIMARY KEY (ID);
ALTER TABLE ContractState ADD PRIMARY KEY (ID);
ALTER TABLE ContractHierarchy ADD PRIMARY KEY (ID);
ALTER TABLE CurrencyMeasure ADD PRIMARY KEY (ID);
ALTER TABLE PaymentDocument ADD PRIMARY KEY (ID);
ALTER TABLE PrepaymentDocumentType ADD PRIMARY KEY (ID);
ALTER TABLE ContractPayment ADD PRIMARY KEY (ID);
ALTER TABLE TroublesRegistry ADD PRIMARY KEY (ID);
ALTER TABLE Trouble ADD PRIMARY KEY (ID);
ALTER TABLE ContractTrouble ADD PRIMARY KEY (ID);
ALTER TABLE Property ADD PRIMARY KEY (ID);
ALTER TABLE ContractorPropertiy ADD PRIMARY KEY (ID);
ALTER TABLE FunctionalCustomer ADD PRIMARY KEY (ID);
ALTER TABLE FunctionalCustomerType ADD PRIMARY KEY (ID);
ALTER TABLE FunctionalCustomerContract ADD PRIMARY KEY (ID);
ALTER TABLE ContractorType ADD PRIMARY KEY (ID);
ALTER TABLE Employee ADD PRIMARY KEY (ID);
ALTER TABLE EnterpriseAuthority ADD PRIMARY KEY (ID);
ALTER TABLE Schedule ADD PRIMARY KEY (ID);
ALTER TABLE ScheduleContract ADD PRIMARY KEY (ID);
ALTER TABLE Stage ADD PRIMARY KEY (ID);
ALTER TABLE Act ADD PRIMARY KEY (ID);
ALTER TABLE ClosedStageRelation ADD PRIMARY KEY (ID);
ALTER TABLE FuncCustomerPerson ADD PRIMARY KEY (ID);
ALTER TABLE SightFuncPerson ADD PRIMARY KEY (ID);
ALTER TABLE Region ADD PRIMARY KEY (ID);
ALTER TABLE ActType ADD PRIMARY KEY (ID);
ALTER TABLE WorkType ADD PRIMARY KEY (ID);
ALTER TABLE Disposal ADD PRIMARY KEY (ID);
ALTER TABLE Responsible ADD PRIMARY KEY (ID);
ALTER TABLE Role ADD PRIMARY KEY (ID);
ALTER TABLE Authority ADD PRIMARY KEY (ID);
ALTER TABLE SightFuncPersonScheme ADD PRIMARY KEY (ID);
ALTER TABLE ActPaymentDocument ADD PRIMARY KEY (ID);
ALTER TABLE TransferAct ADD PRIMARY KEY (ID);
ALTER TABLE TransferActType ADD PRIMARY KEY (ID);
ALTER TABLE Document ADD PRIMARY KEY (ID);
ALTER TABLE TransferActActDocument ADD PRIMARY KEY (ID);
ALTER TABLE TransferActTypeDocument ADD PRIMARY KEY (ID);
ALTER TABLE ContractTranActDoc ADD PRIMARY KEY (ID);
ALTER TABLE StageResult ADD PRIMARY KEY (ID);
ALTER TABLE NTPSubView ADD PRIMARY KEY (ID);
ALTER TABLE NTPView ADD PRIMARY KEY (ID);
ALTER TABLE EconomEfficiencyType ADD PRIMARY KEY (ID);
ALTER TABLE EconomEfficiencyParameter ADD PRIMARY KEY (ID);
ALTER TABLE EfficienceParameterType ADD PRIMARY KEY (ID);
ALTER TABLE EfParameterStageResult ADD PRIMARY KEY (ID);
ALTER TABLE ContractorPosition ADD PRIMARY KEY (ID);
ALTER TABLE ContractorAuthority ADD PRIMARY KEY (ID);
ALTER TABLE Position ADD PRIMARY KEY (ID);
ALTER TABLE SubGeneralHierarchi ADD PRIMARY KEY (ID);
ALTER TABLE UDMetadata ADD PRIMARY KEY (SchemeRelease, SchemeBuild);
ALTER TABLE IMPORTINGSCHEME ADD PRIMARY KEY (ID);
ALTER TABLE IMPORTINGSCHEMEITEM ADD PRIMARY KEY (ID);
ALTER TABLE Department ADD PRIMARY KEY (ID);
ALTER TABLE Post ADD PRIMARY KEY (ID);
ALTER TABLE ResponsibleForOrder ADD PRIMARY KEY (ID);
ALTER TABLE Location ADD PRIMARY KEY (ID);
ALTER TABLE ApprovalState ADD PRIMARY KEY (ID);
ALTER TABLE ApprovalProcess ADD PRIMARY KEY (ID);
ALTER TABLE Contractdoc_Funds_Fact ADD PRIMARY KEY (ContractdocID, Time_DimID);
ALTER TABLE Time_Dim ADD PRIMARY KEY (ID);
ALTER TABLE MissiveType ADD PRIMARY KEY (ID);
ALTER TABLE ApprovalGoal ADD PRIMARY KEY (ID);
ALTER TABLE ResponsibleAssignmentOrder ADD PRIMARY KEY (ID);
ALTER TABLE Education ADD PRIMARY KEY (ID);
ALTER TABLE ReportGrouping ADD PRIMARY KEY (ContractTypeID, ContractTypeSubgroupID);
ALTER TABLE Report ADD PRIMARY KEY (ID);
ALTER TABLE FilterState ADD PRIMARY KEY (ID);
ALTER TABLE Report_FilterState ADD PRIMARY KEY (ReportID, FilterStateID);
ALTER TABLE YearReportColor ADD PRIMARY KEY (Id);
ALTER TABLE ContractDocContractor ADD PRIMARY KEY (ID);
