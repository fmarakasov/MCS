/*
/*
Система учёта договоров 1.45
Скрипт сгенерирован 04:25:42 PM 10/07/2012 
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
DROP SEQUENCE seq_ContractDoc;
DROP SEQUENCE seq_ContractState;
DROP SEQUENCE seq_Currency;
DROP SEQUENCE seq_ContractType;
DROP SEQUENCE seq_Document;
DROP SEQUENCE seq_WorkType;
DROP SEQUENCE seq_Schedule;
DROP SEQUENCE seq_CurrencyMeasure;
DROP SEQUENCE seq_PrepaymentDocumentType;
DROP SEQUENCE seq_PaymentDocument;
DROP SEQUENCE seq_Act;
DROP SEQUENCE seq_Authority;
DROP SEQUENCE seq_EnterpriseAuthority;
DROP SEQUENCE seq_Employee;
DROP SEQUENCE seq_Role;
DROP SEQUENCE seq_ContractorAuthority;
DROP SEQUENCE seq_Disposal;
DROP SEQUENCE seq_Trouble;
DROP SEQUENCE seq_TroublesRegistry;
DROP SEQUENCE seq_TransferActType;
DROP SEQUENCE seq_TransferAct;
DROP SEQUENCE seq_NDSAlgorithm;
DROP SEQUENCE seq_Stage;
DROP SEQUENCE seq_Region;
DROP SEQUENCE seq_Property;
DROP SEQUENCE seq_Contractor;
DROP SEQUENCE seq_ContractorType;
DROP SEQUENCE seq_Position;
DROP SEQUENCE seq_ContractorPosition;
DROP SEQUENCE seq_NDS;
DROP SEQUENCE seq_Degree;
DROP SEQUENCE seq_FuncCustomerPerson;
DROP SEQUENCE seq_Person;
DROP SEQUENCE seq_FunctionalCustomer;
DROP SEQUENCE seq_NTPSubView;
DROP SEQUENCE seq_NTPView;
DROP SEQUENCE seq_StageResult;
DROP SEQUENCE seq_EconomEfficiencyType;
DROP SEQUENCE seq_EconomEfficiencyParameter;
DROP SEQUENCE seq_SightFuncPersonScheme;
DROP SEQUENCE seq_SightFuncPerson;
DROP SEQUENCE seq_FunctionalCustomerType;
DROP SEQUENCE seq_ActType;
DROP SEQUENCE seq_FunctionalCustomerContract;
DROP SEQUENCE seq_ClosedStageRelation;
DROP SEQUENCE seq_EfficienceParameterType;
DROP SEQUENCE seq_ContractTrouble;
DROP SEQUENCE seq_Prepayment;
DROP SEQUENCE seq_ContractHierarchy;
DROP SEQUENCE seq_SubGeneralHierarchi;
DROP SEQUENCE seq_ActPaymentDocument;
DROP SEQUENCE seq_ContractorPropertiy;
DROP SEQUENCE seq_TransferActTypeDocument;
DROP SEQUENCE seq_ContractTranActDoc;
DROP SEQUENCE seq_ScheduleContract;
DROP SEQUENCE seq_ContractPayment;
DROP SEQUENCE seq_TransferActActDocument;
DROP SEQUENCE seq_Responsible;
DROP SEQUENCE seq_EfParameterStageResult;
DROP SEQUENCE seq_IMPORTINGSCHEME;
DROP SEQUENCE seq_IMPORTINGSCHEMEITEM;
DROP SEQUENCE seq_Post;
DROP SEQUENCE seq_Location;
DROP SEQUENCE seq_ApprovalState;
DROP SEQUENCE seq_Department;
DROP SEQUENCE seq_Time_Dim;
DROP SEQUENCE seq_ApprovalProcess;
DROP SEQUENCE seq_ApprovalGoal;
DROP SEQUENCE seq_MissiveType;
DROP SEQUENCE seq_ResponsibleForOrder;
DROP SEQUENCE seq_Education;
DROP SEQUENCE seq_ResponsibleAssignmentOrder;
DROP SEQUENCE seq_FilterState;
DROP SEQUENCE seq_Report;
DROP SEQUENCE seq_YearReportColor;
CREATE SEQUENCE seq_ContractDocContractor;
CREATE SEQUENCE seq_ContractDoc START WITH 100000 INCREMENT BY 10;
CREATE SEQUENCE seq_ContractState START WITH 100000;
CREATE SEQUENCE seq_Currency START WITH 100000;
CREATE SEQUENCE seq_ContractType START WITH 100000;
CREATE SEQUENCE seq_Document START WITH 100000;
CREATE SEQUENCE seq_WorkType START WITH 100000;
CREATE SEQUENCE seq_Schedule START WITH 100000;
CREATE SEQUENCE seq_CurrencyMeasure START WITH 100000;
CREATE SEQUENCE seq_PrepaymentDocumentType START WITH 100000;
CREATE SEQUENCE seq_PaymentDocument START WITH 100000;
CREATE SEQUENCE seq_Act START WITH 100000;
CREATE SEQUENCE seq_Authority START WITH 100000;
CREATE SEQUENCE seq_EnterpriseAuthority START WITH 100000;
CREATE SEQUENCE seq_Employee START WITH 100000;
CREATE SEQUENCE seq_Role START WITH 100000;
CREATE SEQUENCE seq_ContractorAuthority START WITH 100000;
CREATE SEQUENCE seq_Disposal START WITH 100000;
CREATE SEQUENCE seq_Trouble START WITH 100000;
CREATE SEQUENCE seq_TroublesRegistry START WITH 100000;
CREATE SEQUENCE seq_TransferActType START WITH 100000;
CREATE SEQUENCE seq_TransferAct START WITH 100000;
CREATE SEQUENCE seq_NDSAlgorithm START WITH 100000;
CREATE SEQUENCE seq_Stage START WITH 100000;
CREATE SEQUENCE seq_Region START WITH 100000;
CREATE SEQUENCE seq_Property START WITH 100000;
CREATE SEQUENCE seq_Contractor START WITH 100000;
CREATE SEQUENCE seq_ContractorType START WITH 100000;
CREATE SEQUENCE seq_Position START WITH 100000;
CREATE SEQUENCE seq_ContractorPosition START WITH 100000;
CREATE SEQUENCE seq_NDS START WITH 100000;
CREATE SEQUENCE seq_Degree START WITH 100000;
CREATE SEQUENCE seq_FuncCustomerPerson START WITH 100000;
CREATE SEQUENCE seq_Person START WITH 100000;
CREATE SEQUENCE seq_FunctionalCustomer START WITH 100000;
CREATE SEQUENCE seq_NTPSubView START WITH 100000;
CREATE SEQUENCE seq_NTPView START WITH 100000;
CREATE SEQUENCE seq_StageResult START WITH 100000;
CREATE SEQUENCE seq_EconomEfficiencyType START WITH 100000;
CREATE SEQUENCE seq_EconomEfficiencyParameter START WITH 100000;
CREATE SEQUENCE seq_SightFuncPersonScheme START WITH 100000;
CREATE SEQUENCE seq_SightFuncPerson START WITH 100000;
CREATE SEQUENCE seq_FunctionalCustomerType START WITH 100000;
CREATE SEQUENCE seq_ActType START WITH 100000;
CREATE SEQUENCE seq_FunctionalCustomerContract START WITH 100000;
CREATE SEQUENCE seq_ClosedStageRelation START WITH 100000;
CREATE SEQUENCE seq_EfficienceParameterType START WITH 100000;
CREATE SEQUENCE seq_ContractTrouble START WITH 100000;
CREATE SEQUENCE seq_Prepayment START WITH 100000;
CREATE SEQUENCE seq_ContractHierarchy START WITH 100000;
CREATE SEQUENCE seq_SubGeneralHierarchi START WITH 100000;
CREATE SEQUENCE seq_ActPaymentDocument START WITH 100000;
CREATE SEQUENCE seq_ContractorPropertiy START WITH 100000;
CREATE SEQUENCE seq_TransferActTypeDocument START WITH 100000;
CREATE SEQUENCE seq_ContractTranActDoc START WITH 100000;
CREATE SEQUENCE seq_ScheduleContract START WITH 100000;
CREATE SEQUENCE seq_ContractPayment START WITH 100000;
CREATE SEQUENCE seq_TransferActActDocument START WITH 100000;
CREATE SEQUENCE seq_Responsible START WITH 100000;
CREATE SEQUENCE seq_EfParameterStageResult START WITH 100000;
CREATE SEQUENCE seq_IMPORTINGSCHEME START WITH 100000;
CREATE SEQUENCE seq_IMPORTINGSCHEMEITEM START WITH 100000;
CREATE SEQUENCE seq_Post START WITH 100000;
CREATE SEQUENCE seq_Location START WITH 100000;
CREATE SEQUENCE seq_ApprovalState START WITH 100000;
CREATE SEQUENCE seq_Department START WITH 100000;
CREATE SEQUENCE seq_Time_Dim START WITH 100000;
CREATE SEQUENCE seq_ApprovalProcess START WITH 100000;
CREATE SEQUENCE seq_ApprovalGoal START WITH 100000;
CREATE SEQUENCE seq_MissiveType START WITH 100000;
CREATE SEQUENCE seq_ResponsibleForOrder START WITH 100000;
CREATE SEQUENCE seq_Education START WITH 100000;
CREATE SEQUENCE seq_ResponsibleAssignmentOrder START WITH 100000;
CREATE SEQUENCE seq_FilterState START WITH 100000;
CREATE SEQUENCE seq_Report START WITH 100000;
CREATE SEQUENCE seq_YearReportColor START WITH 100000;
