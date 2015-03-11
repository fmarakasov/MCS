/*
Система учёта договоров 1.32
Скрипт сгенерирован 04:40:45 PM 04/11/2011
*/
alter session set nls_date_format='DD.MM.YYYY HH24:MI:SS';
ALTER SESSION SET CONSTRAINTS = DEFERRED;
DROP SEQUENCE seq_ResponsibleAssignmentOrder;
DROP VIEW ContractRepositoryView;
DROP TABLE ContractDoc CASCADE CONSTRAINTS;
DROP TABLE ContractType CASCADE CONSTRAINTS;
DROP TABLE Currency CASCADE CONSTRAINTS;
DROP TABLE NDSAlgorithm CASCADE CONSTRAINTS;
DROP TABLE Prepayment CASCADE CONSTRAINTS;
DROP TABLE Person CASCADE CONSTRAINTS;
DROP TABLE Contractor CASCADE CONSTRAINTS;
DROP TABLE Degree CASCADE CONSTRAINTS;
DROP TABLE NDS CASCADE CONSTRAINTS;
DROP TABLE ContractState CASCADE CONSTRAINTS;
DROP TABLE ContractHierarchy CASCADE CONSTRAINTS;
DROP TABLE CurrencyMeasure CASCADE CONSTRAINTS;
DROP TABLE PaymentDocument CASCADE CONSTRAINTS;
DROP TABLE PrepaymentDocumentType CASCADE CONSTRAINTS;
DROP TABLE ContractPayment CASCADE CONSTRAINTS;
DROP TABLE TroublesRegistry CASCADE CONSTRAINTS;
DROP TABLE Trouble CASCADE CONSTRAINTS;
DROP TABLE ContractTrouble CASCADE CONSTRAINTS;
DROP TABLE Property CASCADE CONSTRAINTS;
DROP TABLE ContractorPropertiy CASCADE CONSTRAINTS;
DROP TABLE FunctionalCustomer CASCADE CONSTRAINTS;
DROP TABLE FunctionalCustomerType CASCADE CONSTRAINTS;
DROP TABLE FunctionalCustomerContract CASCADE CONSTRAINTS;
DROP TABLE ContractorType CASCADE CONSTRAINTS;
DROP TABLE Employee CASCADE CONSTRAINTS;
DROP TABLE EnterpriseAuthority CASCADE CONSTRAINTS;
DROP TABLE Schedule CASCADE CONSTRAINTS;
DROP TABLE ScheduleContract CASCADE CONSTRAINTS;
DROP TABLE Stage CASCADE CONSTRAINTS;
DROP TABLE Act CASCADE CONSTRAINTS;
DROP TABLE ClosedStageRelation CASCADE CONSTRAINTS;
DROP TABLE FuncCustomerPerson CASCADE CONSTRAINTS;
DROP TABLE SightFuncPerson CASCADE CONSTRAINTS;
DROP TABLE Region CASCADE CONSTRAINTS;
DROP TABLE ActType CASCADE CONSTRAINTS;
DROP TABLE WorkType CASCADE CONSTRAINTS;
DROP TABLE Disposal CASCADE CONSTRAINTS;
DROP TABLE Responsible CASCADE CONSTRAINTS;
DROP TABLE Role CASCADE CONSTRAINTS;
DROP TABLE Authority CASCADE CONSTRAINTS;
DROP TABLE SightFuncPersonScheme CASCADE CONSTRAINTS;
DROP TABLE ActPaymentDocument CASCADE CONSTRAINTS;
DROP TABLE TransferAct CASCADE CONSTRAINTS;
DROP TABLE TransferActType CASCADE CONSTRAINTS;
DROP TABLE Document CASCADE CONSTRAINTS;
DROP TABLE TransferActActDocument CASCADE CONSTRAINTS;
DROP TABLE TransferActTypeDocument CASCADE CONSTRAINTS;
DROP TABLE ContractTranActDoc CASCADE CONSTRAINTS;
DROP TABLE StageResult CASCADE CONSTRAINTS;
DROP TABLE NTPSubView CASCADE CONSTRAINTS;
DROP TABLE NTPView CASCADE CONSTRAINTS;
DROP TABLE EconomEfficiencyType CASCADE CONSTRAINTS;
DROP TABLE EconomEfficiencyParameter CASCADE CONSTRAINTS;
DROP TABLE EfficienceParameterType CASCADE CONSTRAINTS;
DROP TABLE EfParameterStageResult CASCADE CONSTRAINTS;
DROP TABLE ContractorPosition CASCADE CONSTRAINTS;
DROP TABLE ContractorAuthority CASCADE CONSTRAINTS;
DROP TABLE Position CASCADE CONSTRAINTS;
DROP TABLE SubGeneralHierarchi CASCADE CONSTRAINTS;
DROP TABLE UDMetadata CASCADE CONSTRAINTS;
DROP TABLE IMPORTINGSCHEME CASCADE CONSTRAINTS;
DROP TABLE IMPORTINGSCHEMEITEM CASCADE CONSTRAINTS;
DROP TABLE Department CASCADE CONSTRAINTS;
DROP TABLE Post CASCADE CONSTRAINTS;
DROP TABLE ResponsibleForOrder CASCADE CONSTRAINTS;
DROP TABLE Location CASCADE CONSTRAINTS;
DROP TABLE ApprovalState CASCADE CONSTRAINTS;
DROP TABLE ApprovalProcess CASCADE CONSTRAINTS;
DROP TABLE Contractdoc_Funds_Fact CASCADE CONSTRAINTS;
DROP TABLE Time_Dim CASCADE CONSTRAINTS;
DROP TABLE MissiveType CASCADE CONSTRAINTS;
DROP TABLE ApprovalGoal CASCADE CONSTRAINTS;
DROP TABLE ResponsibleAssignmentOrder CASCADE CONSTRAINTS;
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
CREATE SEQUENCE seq_ResponsibleAssignmentOrder;
CREATE SEQUENCE seq_ContractDoc START WITH 10000 INCREMENT BY 10;
CREATE SEQUENCE seq_ContractState START WITH 10000;
CREATE SEQUENCE seq_Currency START WITH 10000;
CREATE SEQUENCE seq_ContractType START WITH 10000;
CREATE SEQUENCE seq_Document START WITH 10000;
CREATE SEQUENCE seq_WorkType START WITH 10000;
CREATE SEQUENCE seq_Schedule START WITH 10000;
CREATE SEQUENCE seq_CurrencyMeasure START WITH 10000;
CREATE SEQUENCE seq_PrepaymentDocumentType START WITH 10000;
CREATE SEQUENCE seq_PaymentDocument START WITH 10000;
CREATE SEQUENCE seq_Act START WITH 10000;
CREATE SEQUENCE seq_Authority START WITH 10000;
CREATE SEQUENCE seq_EnterpriseAuthority START WITH 10000;
CREATE SEQUENCE seq_Employee START WITH 10000;
CREATE SEQUENCE seq_Role START WITH 10000;
CREATE SEQUENCE seq_ContractorAuthority START WITH 10000;
CREATE SEQUENCE seq_Disposal START WITH 10000;
CREATE SEQUENCE seq_Trouble START WITH 10000;
CREATE SEQUENCE seq_TroublesRegistry START WITH 10000;
CREATE SEQUENCE seq_TransferActType START WITH 10000;
CREATE SEQUENCE seq_TransferAct START WITH 10000;
CREATE SEQUENCE seq_NDSAlgorithm START WITH 10000;
CREATE SEQUENCE seq_Stage START WITH 10000;
CREATE SEQUENCE seq_Region START WITH 10000;
CREATE SEQUENCE seq_Property START WITH 10000;
CREATE SEQUENCE seq_Contractor START WITH 10000;
CREATE SEQUENCE seq_ContractorType START WITH 10000;
CREATE SEQUENCE seq_Position START WITH 10000;
CREATE SEQUENCE seq_ContractorPosition START WITH 10000;
CREATE SEQUENCE seq_NDS START WITH 10000;
CREATE SEQUENCE seq_Degree START WITH 10000;
CREATE SEQUENCE seq_FuncCustomerPerson START WITH 10000;
CREATE SEQUENCE seq_Person START WITH 10000;
CREATE SEQUENCE seq_FunctionalCustomer START WITH 10000;
CREATE SEQUENCE seq_NTPSubView START WITH 10000;
CREATE SEQUENCE seq_NTPView START WITH 10000;
CREATE SEQUENCE seq_StageResult START WITH 10000;
CREATE SEQUENCE seq_EconomEfficiencyType START WITH 10000;
CREATE SEQUENCE seq_EconomEfficiencyParameter START WITH 10000;
CREATE SEQUENCE seq_SightFuncPersonScheme START WITH 10000;
CREATE SEQUENCE seq_SightFuncPerson START WITH 10000;
CREATE SEQUENCE seq_FunctionalCustomerType START WITH 10000;
CREATE SEQUENCE seq_ActType START WITH 10000;
CREATE SEQUENCE seq_FunctionalCustomerContract START WITH 10000;
CREATE SEQUENCE seq_ClosedStageRelation START WITH 10000;
CREATE SEQUENCE seq_EfficienceParameterType START WITH 10000;
CREATE SEQUENCE seq_ContractTrouble START WITH 10000;
CREATE SEQUENCE seq_Prepayment START WITH 10000;
CREATE SEQUENCE seq_ContractHierarchy START WITH 10000;
CREATE SEQUENCE seq_SubGeneralHierarchi START WITH 10000;
CREATE SEQUENCE seq_ActPaymentDocument START WITH 10000;
CREATE SEQUENCE seq_ContractorPropertiy START WITH 10000;
CREATE SEQUENCE seq_TransferActTypeDocument START WITH 10000;
CREATE SEQUENCE seq_ContractTranActDoc START WITH 10000;
CREATE SEQUENCE seq_ScheduleContract START WITH 10000;
CREATE SEQUENCE seq_ContractPayment START WITH 10000;
CREATE SEQUENCE seq_TransferActActDocument START WITH 10000;
CREATE SEQUENCE seq_Responsible START WITH 10000;
CREATE SEQUENCE seq_EfParameterStageResult START WITH 10000;
CREATE SEQUENCE seq_IMPORTINGSCHEME START WITH 10000;
CREATE SEQUENCE seq_IMPORTINGSCHEMEITEM START WITH 10000;
CREATE SEQUENCE seq_Post START WITH 10000;
CREATE SEQUENCE seq_Location START WITH 10000;
CREATE SEQUENCE seq_ApprovalState START WITH 10000;
CREATE SEQUENCE seq_Department START WITH 10000;
CREATE SEQUENCE seq_Time_Dim START WITH 10000;
CREATE SEQUENCE seq_ApprovalProcess START WITH 10000;
CREATE SEQUENCE seq_ApprovalGoal START WITH 10000;
CREATE SEQUENCE seq_MissiveType START WITH 10000;
CREATE SEQUENCE seq_ResponsibleForOrder START WITH 10000;
CREATE TABLE ContractDoc (
  ID                       number(10) NOT NULL, 
  Price                    number(18, 2), 
  ContractTypeID           number(10), 
  StartAt                  date, 
  EndsAt                   date, 
  AppliedAt                date, 
  ApprovedAt               date, 
  InternalNum              nvarchar2(100), 
  ContractorNum            nvarchar2(100), 
  CurrencyID               number(10), 
  OriginContractID         number(10), 
  NDSAlgorithmID           number(10), 
  PrepaymentSum            number(18, 2), 
  PrepaymentPercent        float(10), 
  PrepaymentNDSAlgorithmID number(10), 
  NDSID                    number(10), 
  ContractStateID          number(10) DEFAULT -1 NOT NULL, 
  CurrencyMeasureID        number(10), 
  ContractorID             number(10), 
  IsProtectability         number(1), 
  Subject                  nvarchar2(2000), 
  CurrencyRate             number(18, 2), 
  RateDate                 date, 
  Description              nvarchar2(2000), 
  ContractorPersonID       number(10), 
  IsActive                 number(1), 
  AuthorityID              number(10), 
  PrepaymentPrecentType    number(1), 
  Deleted                  number(1), 
  DepartmentID             number(10) DEFAULT -1, 
  AgreementNum             number(4) CHECK(AgreementNum > 0), 
  IsGeneral                number(1), 
  IsSubGeneral             number(1), 
  PRIMARY KEY (ID));
COMMENT ON TABLE ContractDoc IS 'Договор (генеральный/субподрядный и доп. соглашения)';
COMMENT ON COLUMN ContractDoc.ID IS 'Идентификатор договора';
COMMENT ON COLUMN ContractDoc.Price IS 'Цена договора в заданной валюте';
COMMENT ON COLUMN ContractDoc.ContractTypeID IS 'Тип договора (напр. НИОКР)';
COMMENT ON COLUMN ContractDoc.StartAt IS 'Дата начала работ по договору';
COMMENT ON COLUMN ContractDoc.EndsAt IS 'Дата окончания работ по договору';
COMMENT ON COLUMN ContractDoc.AppliedAt IS 'Дата принятия договора';
COMMENT ON COLUMN ContractDoc.ApprovedAt IS 'Дата утверждения договора';
COMMENT ON COLUMN ContractDoc.InternalNum IS 'Номер договора, присвоенный заказчиком';
COMMENT ON COLUMN ContractDoc.ContractorNum IS 'Номер договора, присвоенный исполнителем';
COMMENT ON COLUMN ContractDoc.CurrencyID IS 'Ссылка на валюту договора';
COMMENT ON COLUMN ContractDoc.OriginContractID IS 'Ссылка на оригинальный договор (для доп. соглашений)';
COMMENT ON COLUMN ContractDoc.NDSAlgorithmID IS 'Ссылка на алгоритм расчёта НДС';
COMMENT ON COLUMN ContractDoc.PrepaymentSum IS 'Сумма аванса по договору (общая сумма по всем годам, аванс по годам представлен в таблице Prepaiment)';
COMMENT ON COLUMN ContractDoc.PrepaymentPercent IS 'Процент аванса от суммы договора [0..100](общий по всем годам, аванс по годам представлен в таблице Prepaiment';
COMMENT ON COLUMN ContractDoc.PrepaymentNDSAlgorithmID IS 'Ссылка на алгоритм расчёта НДС для аванса';
COMMENT ON COLUMN ContractDoc.NDSID IS 'Ссылка на ставку НДС договора';
COMMENT ON COLUMN ContractDoc.ContractStateID IS 'Ссылка на состояние договора';
COMMENT ON COLUMN ContractDoc.CurrencyMeasureID IS 'Ссылка на единицу измерения цены договора (напр. в тыс. руб.)';
COMMENT ON COLUMN ContractDoc.ContractorID IS 'Ссылка на контрагента исполнителя договора';
COMMENT ON COLUMN ContractDoc.IsProtectability IS 'Признак охраноспособности договора';
COMMENT ON COLUMN ContractDoc.Subject IS 'Тема договора';
COMMENT ON COLUMN ContractDoc.CurrencyRate IS 'Курс валюты по договору на дату RateDate';
COMMENT ON COLUMN ContractDoc.RateDate IS 'Дата для которой берётся курс валюты договора. Как правило совпадает с датой подписания договора';
COMMENT ON COLUMN ContractDoc.Description IS 'Дополнительные сведения о договоре';
COMMENT ON COLUMN ContractDoc.ContractorPersonID IS 'Ссылка на руководителя темы от огранизации исполнителя';
COMMENT ON COLUMN ContractDoc.AuthorityID IS 'Ссылка на распоряжение по организации';
COMMENT ON COLUMN ContractDoc.PrepaymentPrecentType IS 'Формат ввода данных по % аванса: false - от суммы договора без НДС, true - от суммы договора с НДС';
COMMENT ON COLUMN ContractDoc.Deleted IS 'Признак того, что договор удалён пользователем';
COMMENT ON COLUMN ContractDoc.AgreementNum IS 'Номер соглашения к договору. Используется только в ДС в случае, если автоматически вычисленный номер ДС не подходит';
CREATE TABLE ContractType (
  ID          number(10) NOT NULL, 
  Name        nvarchar2(200) NOT NULL UNIQUE, 
  ReportOrder number(3), 
  PRIMARY KEY (ID));
COMMENT ON TABLE ContractType IS 'Тип договора (сп)';
COMMENT ON COLUMN ContractType.ID IS 'Идентификатор';
COMMENT ON COLUMN ContractType.Name IS 'Название (напр. НИОКР)';
COMMENT ON COLUMN ContractType.ReportOrder IS 'Определяет порядок следования группы в отчётах';
CREATE TABLE Currency (
  ID            number(10) NOT NULL, 
  Name          nvarchar2(100) NOT NULL UNIQUE, 
  Culture       nvarchar2(10) NOT NULL, 
  CurrencyI     nvarchar2(20), 
  CurrencyR     nvarchar2(20), 
  CurrencyM     nvarchar2(20), 
  SmallI        nvarchar2(20), 
  SmallR        nvarchar2(20), 
  SmallM        nvarchar2(20), 
  HighSmallName nvarchar2(20), 
  LowSmallName  nvarchar2(20), 
  Code          varchar2(10), 
  PRIMARY KEY (ID));
COMMENT ON TABLE Currency IS 'Валюта (сп)';
COMMENT ON COLUMN Currency.ID IS 'Идентификатор';
COMMENT ON COLUMN Currency.Name IS 'Название (напр. Рубль)';
COMMENT ON COLUMN Currency.Culture IS 'Культура (используется в приложении для отобржения суммы прописью) (напр. ru-ru)';
COMMENT ON COLUMN Currency.CurrencyI IS 'Склонение валюты в именительном падеже единственном числе (напр. рубль)';
COMMENT ON COLUMN Currency.CurrencyR IS 'Склонение валюты в родительном падеже единственном числе (напр. рубля)';
COMMENT ON COLUMN Currency.CurrencyM IS 'Склонение валюты в родительном падеже множественном числе (напр. рублей)';
COMMENT ON COLUMN Currency.SmallI IS 'Склонение минимальной единицы валюты в именительном падеже единственном числе (напр. копейка)';
COMMENT ON COLUMN Currency.SmallR IS 'Склонение минимальной единицы валюты в родительном падеже единственном числе (напр. копейки)';
COMMENT ON COLUMN Currency.SmallM IS 'Склонение минимальной единицы валюты в родительном падеже единственном числе (напр. копеек)';
COMMENT ON COLUMN Currency.HighSmallName IS 'Аббревиатура единицы валюты (напр. рубл.)';
COMMENT ON COLUMN Currency.LowSmallName IS 'Аббревиатура минимальной единицы валюты (напр. коп.)';
COMMENT ON COLUMN Currency.Code IS 'Код единицы валюты (напр. RUB)';
CREATE TABLE NDSAlgorithm (
  ID           number(10) NOT NULL, 
  Name         nvarchar2(100) NOT NULL UNIQUE, 
  PriceTooltip nvarchar2(100), 
  PRIMARY KEY (ID));
COMMENT ON TABLE NDSAlgorithm IS 'Алгоритм НДС (сп)';
COMMENT ON COLUMN NDSAlgorithm.ID IS 'Идентификатор';
COMMENT ON COLUMN NDSAlgorithm.Name IS 'Название (напр. без НДС)';
CREATE TABLE Prepayment (
  ID            number(10) NOT NULL, 
  ContractDocID number(10) NOT NULL, 
  Sum           number(18, 2) NOT NULL, 
  PercentValue  float(10) NOT NULL, 
  Year          number(4) NOT NULL, 
  PRIMARY KEY (ID), 
  CONSTRAINT U_YEAR_CONTRACT 
    UNIQUE (ContractDocID, Year), 
  CONSTRAINT C_YEAR_EXCEEDS_RANGE 
    CHECK (YEAR>=1960), 
  CONSTRAINT C_SUM_EXCEEDS_RANGE 
    CHECK (Sum>0));
COMMENT ON TABLE Prepayment IS 'Предоплата по годам';
COMMENT ON COLUMN Prepayment.ContractDocID IS 'Ссылка на контракт';
COMMENT ON COLUMN Prepayment.Sum IS 'Сумма аванса';
COMMENT ON COLUMN Prepayment.PercentValue IS 'Процент от общей стоимости договора (0..100)';
COMMENT ON COLUMN Prepayment.Year IS 'Год, в который выдан аванс';
CREATE TABLE Person (
  ID                      number(10) NOT NULL, 
  DegreeID                number(10), 
  IsContractHeadAuthority number(1) NOT NULL, 
  IsActSignAuthority      number(1) NOT NULL, 
  IsValid                 number(1) NOT NULL, 
  FamilyName              nvarchar2(30) NOT NULL, 
  FirstName               nvarchar2(30), 
  MiddleName              nvarchar2(30), 
  Sex                     number(1), 
  ContractorPositionID    number(10), 
  PRIMARY KEY (ID));
COMMENT ON TABLE Person IS 'Представитель организации (Гапрома, дочерних организаций, сторонних предсприятий, кроме сотрудников Промгаза)';
COMMENT ON COLUMN Person.ID IS 'Индентификатор';
COMMENT ON COLUMN Person.DegreeID IS 'Ссылка на учёную степень';
COMMENT ON COLUMN Person.IsContractHeadAuthority IS 'Имеет ли человек право подписи от первого лица';
COMMENT ON COLUMN Person.IsActSignAuthority IS 'Имеет ли человек право подписи актов';
COMMENT ON COLUMN Person.IsValid IS 'Являются ли данные по человеку актуальными';
COMMENT ON COLUMN Person.FamilyName IS 'Фамилия';
COMMENT ON COLUMN Person.FirstName IS 'Имя';
COMMENT ON COLUMN Person.MiddleName IS 'Отчество';
COMMENT ON COLUMN Person.Sex IS 'Пол (1-мужщина, 0-женщина)';
COMMENT ON COLUMN Person.ContractorPositionID IS 'Ссылка на должность в организации';
CREATE TABLE Contractor (
  ID               number(10) NOT NULL, 
  Name             nvarchar2(500) NOT NULL UNIQUE, 
  ShortName        nvarchar2(255), 
  Zip              nvarchar2(7), 
  Bank             nvarchar2(200), 
  ContractorTypeID number(10) NOT NULL, 
  INN              nvarchar2(10), 
  Account          nvarchar2(20), 
  BIK              nvarchar2(9), 
  KPP              nvarchar2(9), 
  Address          nvarchar2(1000), 
  CorrespAccount   nvarchar2(20), 
  OKPO             nvarchar2(8), 
  OKONH            nvarchar2(5), 
  OGRN             nvarchar2(13), 
  OKATO            nvarchar2(12), 
  OKVED            nvarchar2(15), 
  PRIMARY KEY (ID));
COMMENT ON TABLE Contractor IS 'Улица';
COMMENT ON COLUMN Contractor.ID IS 'Идентификтор контрагента';
COMMENT ON COLUMN Contractor.Name IS 'Название организации';
COMMENT ON COLUMN Contractor.ShortName IS 'Короткое название организации';
COMMENT ON COLUMN Contractor.Zip IS 'Индекс';
COMMENT ON COLUMN Contractor.Bank IS 'Банк';
COMMENT ON COLUMN Contractor.ContractorTypeID IS 'Ссылка на тип контрагента';
COMMENT ON COLUMN Contractor.INN IS 'ИНН';
COMMENT ON COLUMN Contractor.Account IS 'Расчётный счёт';
COMMENT ON COLUMN Contractor.BIK IS 'БИК';
COMMENT ON COLUMN Contractor.KPP IS 'КПП';
COMMENT ON COLUMN Contractor.Address IS 'Адрес контрагента';
COMMENT ON COLUMN Contractor.CorrespAccount IS 'Корреспондентский счёт';
COMMENT ON COLUMN Contractor.OKPO IS 'ОКПО';
COMMENT ON COLUMN Contractor.OKONH IS 'ОКОНХ';
COMMENT ON COLUMN Contractor.OGRN IS 'ОГРН';
CREATE TABLE Degree (
  ID   number(10) NOT NULL, 
  Name nvarchar2(50) NOT NULL UNIQUE, 
  PRIMARY KEY (ID));
COMMENT ON TABLE Degree IS 'Степень (сп)';
COMMENT ON COLUMN Degree.ID IS 'Идентификатор';
COMMENT ON COLUMN Degree.Name IS 'Название (напр. кондидат наук)';
CREATE TABLE NDS (
  ID       number(10) NOT NULL, 
  Percents number(4, 2) NOT NULL, 
  Year     number(4) NOT NULL, 
  PRIMARY KEY (ID), 
  CONSTRAINT UN_ND_PERCENT_YEAR 
    UNIQUE (Percents, Year), 
  CONSTRAINT C_PERCENTS_EXCEED_RANGE 
    CHECK (PERCENTS >=0 AND PERCENTS<= 100), 
  CONSTRAINT C_YEAR_EXCEED_RANGE 
    CHECK (YEAR >=1960));
COMMENT ON TABLE NDS IS 'НДС (сп)';
COMMENT ON COLUMN NDS.ID IS 'Идентификаьтор';
COMMENT ON COLUMN NDS.Percents IS 'Ставка НДС (0...100) (напр. 18)';
COMMENT ON COLUMN NDS.Year IS 'Год начала дейтсвия';
CREATE TABLE ContractState (
  ID   number(10) NOT NULL, 
  Name nvarchar2(100) NOT NULL UNIQUE, 
  PRIMARY KEY (ID));
COMMENT ON TABLE ContractState IS 'Состояние договора (сп)';
COMMENT ON COLUMN ContractState.ID IS 'Идентификатор';
COMMENT ON COLUMN ContractState.Name IS 'Название (напр. подписан)';
CREATE TABLE ContractHierarchy (
  ID                   number(10) NOT NULL, 
  GeneralContractDocID number(10) NOT NULL, 
  SubContractDocID     number(10) NOT NULL, 
  PRIMARY KEY (ID), 
  CONSTRAINT U_CONTRACT_SUB_CONTRACT 
    UNIQUE (GeneralContractDocID, SubContractDocID));
COMMENT ON TABLE ContractHierarchy IS 'Иерархия договоров (генеральный - субподрядные)';
COMMENT ON COLUMN ContractHierarchy.GeneralContractDocID IS 'Ссылка на генеральный договор';
COMMENT ON COLUMN ContractHierarchy.SubContractDocID IS 'Ссылка на договор субподряда';
CREATE TABLE CurrencyMeasure (
  ID     number(10) NOT NULL, 
  Name   nvarchar2(20) NOT NULL, 
  Factor number(10), 
  PRIMARY KEY (ID));
COMMENT ON TABLE CurrencyMeasure IS 'Единица измерения денег (сп)';
COMMENT ON COLUMN CurrencyMeasure.ID IS 'Идентификатор';
COMMENT ON COLUMN CurrencyMeasure.Name IS 'Название (например тыс.)';
COMMENT ON COLUMN CurrencyMeasure.Factor IS 'Кратность (напр. для тыс. - 1000)';
CREATE TABLE PaymentDocument (
  ID                       number(10) NOT NULL, 
  Num                      nvarchar2(50) NOT NULL, 
  PaymentDate              date, 
  CurrencyMeasureID        number(10) NOT NULL, 
  PrepaymentDocumentTypeID number(10) NOT NULL, 
  PaymentSum               number(18, 2) NOT NULL, 
  PRIMARY KEY (ID));
CREATE TABLE PrepaymentDocumentType (
  ID   number(10) NOT NULL, 
  Name nvarchar2(100) NOT NULL UNIQUE, 
  PRIMARY KEY (ID));
COMMENT ON TABLE PrepaymentDocumentType IS 'Тип платежного документа (сп)';
COMMENT ON COLUMN PrepaymentDocumentType.ID IS 'Идентификатор';
COMMENT ON COLUMN PrepaymentDocumentType.Name IS 'Название (напр. Накладная)';
CREATE TABLE ContractPayment (
  ID                number(10) NOT NULL, 
  PaymentDocumentID number(10) NOT NULL, 
  ContractDocID     number(10) NOT NULL, 
  PRIMARY KEY (ID), 
  CONSTRAINT U_CONTRACT_PAYMENT 
    UNIQUE (PaymentDocumentID, ContractDocID));
CREATE TABLE TroublesRegistry (
  ID         number(10) NOT NULL, 
  Name       nvarchar2(200) NOT NULL UNIQUE, 
  ShortName  nvarchar2(200), 
  ApprovedAt date, 
  OrderNum   nvarchar2(20), 
  ValidFrom  date, 
  ValidTo    date, 
  PRIMARY KEY (ID));
COMMENT ON TABLE TroublesRegistry IS 'Перечень проблем (сп)';
COMMENT ON COLUMN TroublesRegistry.ID IS 'Идентификатор';
COMMENT ON COLUMN TroublesRegistry.Name IS 'Наименование перечня проблемы (напр. Перечень проблем 2006-2010 г.)';
COMMENT ON COLUMN TroublesRegistry.ShortName IS 'Краткое наименование перечня проблем';
COMMENT ON COLUMN TroublesRegistry.ApprovedAt IS 'Дата утверждения';
COMMENT ON COLUMN TroublesRegistry.OrderNum IS 'Номер приказа (напр. 101-202)';
COMMENT ON COLUMN TroublesRegistry.ValidFrom IS 'Дата начала действия';
COMMENT ON COLUMN TroublesRegistry.ValidTo IS 'Дата окончания дейтсвия';
CREATE TABLE Trouble (
  ID                number(10) NOT NULL, 
  Name              nvarchar2(500) NOT NULL UNIQUE, 
  Num               nvarchar2(10), 
  TopTroubleID      number(10), 
  TroubleRegistryID number(10), 
  PRIMARY KEY (ID));
COMMENT ON TABLE Trouble IS 'Проблема (сп)';
COMMENT ON COLUMN Trouble.ID IS 'Идентификатор';
COMMENT ON COLUMN Trouble.Name IS 'Наименование проблемы';
COMMENT ON COLUMN Trouble.Num IS 'Номер (напр. 1.2)';
COMMENT ON COLUMN Trouble.TopTroubleID IS 'ссылка на родительскую проблему (высота дерева 2)';
COMMENT ON COLUMN Trouble.TroubleRegistryID IS 'Ссылка на перечень проблем';
CREATE TABLE ContractTrouble (
  ID            number(10) NOT NULL, 
  TroubleID     number(10) NOT NULL, 
  ContractDocID number(10) NOT NULL, 
  PRIMARY KEY (ID), 
  CONSTRAINT U_CONTRACT_TROUBLE 
    UNIQUE (TroubleID, ContractDocID));
COMMENT ON TABLE ContractTrouble IS 'Проблемы, к которым относен договор (сс)';
COMMENT ON COLUMN ContractTrouble.TroubleID IS 'Ссылка на проблемы';
COMMENT ON COLUMN ContractTrouble.ContractDocID IS 'Ссылка на договор';
CREATE TABLE Property (
  ID   number(10) NOT NULL, 
  Name nvarchar2(50) NOT NULL UNIQUE, 
  PRIMARY KEY (ID));
COMMENT ON TABLE Property IS 'Дополнительные свойства (для контрагентов)(сп)';
COMMENT ON COLUMN Property.ID IS 'Идентификатор';
COMMENT ON COLUMN Property.Name IS 'Название';
CREATE TABLE ContractorPropertiy (
  ID           number(10) NOT NULL, 
  PropertyID   number(10) NOT NULL, 
  ContractorID number(10) NOT NULL, 
  Value        nvarchar2(1000) NOT NULL, 
  PRIMARY KEY (ID), 
  CONSTRAINT U_PROPERTY_CONTRACTOR 
    UNIQUE (PropertyID, ContractorID));
COMMENT ON TABLE ContractorPropertiy IS 'Значения дополнительных атрибутов для контрагента (сс)';
COMMENT ON COLUMN ContractorPropertiy.PropertyID IS 'Ссылка на атрибут';
COMMENT ON COLUMN ContractorPropertiy.ContractorID IS 'Ссылка на контрагента';
COMMENT ON COLUMN ContractorPropertiy.Value IS 'Значение атрибута';
CREATE TABLE FunctionalCustomer (
  ID                         number(10) NOT NULL, 
  Name                       nvarchar2(1000) NOT NULL UNIQUE, 
  ContractorID               number(10) NOT NULL, 
  ParentFunctionalCustomerID number(10), 
  FunctionalCustomerTypeID   number(10) NOT NULL, 
  PRIMARY KEY (ID));
COMMENT ON TABLE FunctionalCustomer IS 'Функциональный заказчик';
COMMENT ON COLUMN FunctionalCustomer.ID IS 'Идентификатор';
COMMENT ON COLUMN FunctionalCustomer.Name IS 'Название (напр. управление инновационного развития)';
COMMENT ON COLUMN FunctionalCustomer.ContractorID IS 'Ссылка на организацию функционального заказчика';
COMMENT ON COLUMN FunctionalCustomer.ParentFunctionalCustomerID IS 'Ссылка на функционального заказчика - предка (связь отдел-подотдел)';
COMMENT ON COLUMN FunctionalCustomer.FunctionalCustomerTypeID IS 'Ссылка на тип функционального заказчика';
CREATE TABLE FunctionalCustomerType (
  ID   number(10) NOT NULL, 
  Name nvarchar2(100) NOT NULL UNIQUE, 
  PRIMARY KEY (ID));
COMMENT ON TABLE FunctionalCustomerType IS 'Тип функционального заказчика (сп)';
COMMENT ON COLUMN FunctionalCustomerType.ID IS 'Идентификатор';
COMMENT ON COLUMN FunctionalCustomerType.Name IS 'Название (напр ?)';
CREATE TABLE FunctionalCustomerContract (
  ID                   number(10) NOT NULL, 
  FunctionalCustomerID number(10) NOT NULL, 
  ContractDocID        number(10) NOT NULL, 
  PRIMARY KEY (ID), 
  CONSTRAINT UN_CONTRACT_FC 
    UNIQUE (FunctionalCustomerID, ContractDocID));
COMMENT ON TABLE FunctionalCustomerContract IS 'Функциональный заказчик договора (сс)';
COMMENT ON COLUMN FunctionalCustomerContract.FunctionalCustomerID IS 'Ссылка на функционального заказчика';
COMMENT ON COLUMN FunctionalCustomerContract.ContractDocID IS 'Ссылка на договор';
CREATE TABLE ContractorType (
  ID          number(10) NOT NULL, 
  Name        nvarchar2(100) NOT NULL UNIQUE, 
  ReportOrder number(3), 
  PRIMARY KEY (ID));
COMMENT ON TABLE ContractorType IS 'Тип контрагента (сп)';
COMMENT ON COLUMN ContractorType.ID IS 'Идентификатор';
COMMENT ON COLUMN ContractorType.Name IS 'Название типа (например. Дочерние организации)';
COMMENT ON COLUMN ContractorType.ReportOrder IS 'Определяет порядок следования группы в отчётах';
CREATE TABLE Employee (
  ID           number(10) NOT NULL, 
  Familyname   nvarchar2(40), 
  FirstName    nvarchar2(40), 
  Middlename   nvarchar2(40), 
  Sex          number(1), 
  PostID       number(10) DEFAULT -1 NOT NULL, 
  DepartmentID number(10) DEFAULT -1 NOT NULL, 
  PRIMARY KEY (ID));
COMMENT ON TABLE Employee IS 'Сотрудник Промгаза';
COMMENT ON COLUMN Employee.ID IS 'Идентификатор';
COMMENT ON COLUMN Employee.Familyname IS 'Фамилия';
COMMENT ON COLUMN Employee.FirstName IS 'Имя';
COMMENT ON COLUMN Employee.Middlename IS 'Отчество';
COMMENT ON COLUMN Employee.Sex IS 'Пол (1-муж, 0-жен.)';
CREATE TABLE EnterpriseAuthority (
  ID          number(10) NOT NULL, 
  Num         nvarchar2(255), 
  ValidFrom   date, 
  ValidTo     date, 
  IsValid     number(1), 
  EmployeeID  number(10) NOT NULL, 
  AuthorityID number(10) NOT NULL, 
  PRIMARY KEY (ID));
COMMENT ON TABLE EnterpriseAuthority IS 'Основание для Промгаза';
COMMENT ON COLUMN EnterpriseAuthority.ID IS 'Идентификатор';
COMMENT ON COLUMN EnterpriseAuthority.Num IS 'Номер основания';
COMMENT ON COLUMN EnterpriseAuthority.ValidFrom IS 'Дата начала действия';
COMMENT ON COLUMN EnterpriseAuthority.ValidTo IS 'Дата окончания действия';
COMMENT ON COLUMN EnterpriseAuthority.IsValid IS 'Признак активности';
COMMENT ON COLUMN EnterpriseAuthority.EmployeeID IS 'Ссылка на ответсвенное лицо (со стороны заказчика)';
COMMENT ON COLUMN EnterpriseAuthority.AuthorityID IS 'Ссылка на основание';
CREATE TABLE Schedule (
  ID                number(10) NOT NULL, 
  CurrencyMeasureID number(10) NOT NULL, 
  WorkTypeID        number(10) NOT NULL, 
  PRIMARY KEY (ID));
COMMENT ON TABLE Schedule IS 'Календарный план';
COMMENT ON COLUMN Schedule.ID IS 'Идентификатор';
COMMENT ON COLUMN Schedule.CurrencyMeasureID IS 'Ссылка на единицу измерения денег (напр. тыс.)';
COMMENT ON COLUMN Schedule.WorkTypeID IS 'Ссылка на тип работ';
CREATE TABLE ScheduleContract (
  ID            number(10) NOT NULL, 
  ContractDocID number(10) NOT NULL, 
  AppNum        number(3), 
  ScheduleID    number(10) NOT NULL, 
  PRIMARY KEY (ID), 
  CONSTRAINT U_SCHEDULE_CONTRACT 
    UNIQUE (ContractDocID, ScheduleID));
COMMENT ON TABLE ScheduleContract IS 'Календарный план договора (сс)';
COMMENT ON COLUMN ScheduleContract.ContractDocID IS 'Ссылка на договор';
COMMENT ON COLUMN ScheduleContract.AppNum IS '№ приложения договора';
CREATE TABLE Stage (
  ID             number(10) NOT NULL, 
  Num            nvarchar2(25), 
  Subject        nvarchar2(2000), 
  StartsAt       date, 
  EndsAt         date, 
  Price          number(18, 2), 
  NdsAlgorithmID number(10) NOT NULL, 
  ParentID       number(10), 
  ActID          number(10), 
  ScheduleID     number(10) NOT NULL, 
  Code           nvarchar2(100), 
  NdsID          number(10) NOT NULL, 
  PRIMARY KEY (ID));
COMMENT ON TABLE Stage IS 'Этап';
COMMENT ON COLUMN Stage.ID IS 'Идентификатор';
COMMENT ON COLUMN Stage.Num IS 'Номер этапа';
COMMENT ON COLUMN Stage.Subject IS 'Название';
COMMENT ON COLUMN Stage.StartsAt IS 'Дата начала';
COMMENT ON COLUMN Stage.EndsAt IS 'Дата окончания';
COMMENT ON COLUMN Stage.Price IS 'Цена этапа';
COMMENT ON COLUMN Stage.NdsAlgorithmID IS 'Ссылка на алгоритм расчета НДС';
COMMENT ON COLUMN Stage.ParentID IS 'Ссылка на родительский этап (организует иерархию этапов: этап-подэтап)';
COMMENT ON COLUMN Stage.ActID IS 'Ссылка на акт сдачи-приемки (если этап закрыт - он не Null)';
COMMENT ON COLUMN Stage.Code IS 'Код стройки';
COMMENT ON COLUMN Stage.NdsID IS 'Ссылка на ставку НДС';
CREATE TABLE Act (
  ID                    number(10) NOT NULL, 
  Num                   nvarchar2(255), 
  SignDate              date, 
  ActTypeID             number(10) NOT NULL, 
  NDSID                 number(10) NOT NULL, 
  RegionID              number(10) NOT NULL, 
  TotalSum              number(18, 2), 
  SumForTransfer        number(18, 2), 
  Status                number(10), 
  EnterpriceAuthorityID number(10) NOT NULL, 
  CurrencyRate          number(18, 2), 
  RateDate              date, 
  NdsalgorithmID        number(10) NOT NULL, 
  CurrencyID            number(10) NOT NULL, 
  CurrencymeasureID     number(10) NOT NULL, 
  IsSigned              number(1) DEFAULT '0', 
  PRIMARY KEY (ID));
COMMENT ON TABLE Act IS 'Акт сдачи-приёмки';
COMMENT ON COLUMN Act.ID IS 'Идентификатор';
COMMENT ON COLUMN Act.Num IS 'Номер акта сдачи-приёмки';
COMMENT ON COLUMN Act.SignDate IS 'Дата подписания акта сдачи-приёмки';
COMMENT ON COLUMN Act.ActTypeID IS 'Ссылка на тип акта сдачи-приёмки';
COMMENT ON COLUMN Act.NDSID IS 'Ссылка на ставку НДС';
COMMENT ON COLUMN Act.RegionID IS 'Ссылка на регион';
COMMENT ON COLUMN Act.TotalSum IS 'Общая сумма по акту сдачи-приёмки';
COMMENT ON COLUMN Act.SumForTransfer IS 'Сумма к перечислению';
COMMENT ON COLUMN Act.Status IS 'Состояние акта сдачи-приёмки';
COMMENT ON COLUMN Act.EnterpriceAuthorityID IS 'Ссылка на основание для Промгаза';
COMMENT ON COLUMN Act.CurrencyRate IS 'Курс валюты на дату RateDate';
COMMENT ON COLUMN Act.RateDate IS 'Дата для курса валюты';
COMMENT ON COLUMN Act.NdsalgorithmID IS 'Алгоритм расчёта НДС';
COMMENT ON COLUMN Act.CurrencyID IS 'Валюта акта';
COMMENT ON COLUMN Act.CurrencymeasureID IS 'Ссылка на единицу измерения денег';
COMMENT ON COLUMN Act.IsSigned IS 'подписан или нет';
CREATE TABLE ClosedStageRelation (
  ID            number(10) NOT NULL, 
  StageID       number(10) NOT NULL, 
  ClosedStageID number(10) NOT NULL, 
  PRIMARY KEY (ID), 
  CONSTRAINT U_STAGE_CLOSED_STAGE 
    UNIQUE (StageID, ClosedStageID));
COMMENT ON TABLE ClosedStageRelation IS 'Связь этапов доп. соглашения с закрытыми этапами (сс)';
COMMENT ON COLUMN ClosedStageRelation.StageID IS 'Cсылка на этап в доп. соглашении';
COMMENT ON COLUMN ClosedStageRelation.ClosedStageID IS 'Ссылка на закрытый этап';
CREATE TABLE FuncCustomerPerson (
  ID             number(10) NOT NULL, 
  FuncCustomerID number(10) NOT NULL UNIQUE, 
  PersonID       number(10), 
  PRIMARY KEY (ID));
COMMENT ON TABLE FuncCustomerPerson IS 'Представители (персоны) функционального заказчика';
COMMENT ON COLUMN FuncCustomerPerson.ID IS 'Идентификатор';
COMMENT ON COLUMN FuncCustomerPerson.FuncCustomerID IS 'Ссылка на функционального заказчика';
COMMENT ON COLUMN FuncCustomerPerson.PersonID IS 'Ссылка на человека - представителя функционального заказчика';
CREATE TABLE SightFuncPerson (
  ID                   number(10) NOT NULL, 
  Name                 nvarchar2(500), 
  SightFuncPersonSchID number(10) NOT NULL, 
  ActID                number(10) NOT NULL, 
  PRIMARY KEY (ID));
COMMENT ON TABLE SightFuncPerson IS 'Визы представителя (персоны) функционального заказчика';
COMMENT ON COLUMN SightFuncPerson.ID IS 'Идентификатор';
COMMENT ON COLUMN SightFuncPerson.Name IS 'Название визы';
COMMENT ON COLUMN SightFuncPerson.SightFuncPersonSchID IS 'Ссылка на схему визирования функционального заказчика';
COMMENT ON COLUMN SightFuncPerson.ActID IS 'Ссылка на акт';
CREATE TABLE Region (
  ID   number(10) NOT NULL, 
  Name nvarchar2(500) NOT NULL UNIQUE, 
  PRIMARY KEY (ID));
COMMENT ON TABLE Region IS 'Регион (сп)';
COMMENT ON COLUMN Region.ID IS 'Идентификатор';
COMMENT ON COLUMN Region.Name IS 'Название региона';
CREATE TABLE ActType (
  ID           number(10) NOT NULL, 
  ContractorID number(10) NOT NULL, 
  TypeName     nvarchar2(200), 
  IsActive     number(1), 
  PRIMARY KEY (ID));
COMMENT ON TABLE ActType IS 'Тип акта сдачи-приёмки';
COMMENT ON COLUMN ActType.ContractorID IS 'Ссылка на контрагента';
COMMENT ON COLUMN ActType.TypeName IS 'Навазние тип акта сдачи-приёмки';
COMMENT ON COLUMN ActType.IsActive IS 'Признак активности';
CREATE TABLE WorkType (
  ID        number(10) NOT NULL, 
  Name      nvarchar2(1000) NOT NULL UNIQUE, 
  ShortName nvarchar2(50) UNIQUE, 
  PRIMARY KEY (ID));
COMMENT ON TABLE WorkType IS 'Тип работ (сп)';
COMMENT ON COLUMN WorkType.ID IS 'Идентификатор';
COMMENT ON COLUMN WorkType.Name IS 'Название типа работ';
COMMENT ON COLUMN WorkType.ShortName IS 'Сокращённое название работ';
CREATE TABLE Disposal (
  ID           number(10) NOT NULL, 
  Num          nvarchar2(10), 
  ApprovedDate date, 
  PRIMARY KEY (ID));
COMMENT ON TABLE Disposal IS 'Распоряжение, устанавливающее ответственных по договору со стороны Промгаза';
COMMENT ON COLUMN Disposal.ID IS 'Идентификатор';
COMMENT ON COLUMN Disposal.Num IS 'Номер распоряжения';
COMMENT ON COLUMN Disposal.ApprovedDate IS 'Дата утверждения';
CREATE TABLE Responsible (
  ID            number(10) NOT NULL, 
  DisposalID    number(10) NOT NULL, 
  EmployeeID    number(10) NOT NULL, 
  RoleID        number(10) DEFAULT -1 NOT NULL, 
  ContractdocID number(10), 
  PRIMARY KEY (ID));
COMMENT ON TABLE Responsible IS 'Ответственные со стороны Промгаза, согласно распоряжению (сс)';
COMMENT ON COLUMN Responsible.DisposalID IS 'Ссылка на распоряжение';
COMMENT ON COLUMN Responsible.EmployeeID IS 'Ссылка на ответсвенного от заказчика согласно распоряжению';
COMMENT ON COLUMN Responsible.RoleID IS 'Ссылка на роль';
COMMENT ON COLUMN Responsible.ContractdocID IS 'Ссылка на договор';
CREATE TABLE Role (
  ID   number(10) NOT NULL, 
  Name nvarchar2(100) NOT NULL UNIQUE, 
  PRIMARY KEY (ID));
COMMENT ON TABLE Role IS 'Роль (сп)';
COMMENT ON COLUMN Role.ID IS 'Идентификатор';
COMMENT ON COLUMN Role.Name IS 'Название (нарп. Руководитель направления?)';
CREATE TABLE Authority (
  ID   number(10) NOT NULL, 
  Name nvarchar2(200) NOT NULL UNIQUE, 
  PRIMARY KEY (ID));
COMMENT ON TABLE Authority IS 'Основание (сп)';
COMMENT ON COLUMN Authority.ID IS 'Идентиыикатор';
COMMENT ON COLUMN Authority.Name IS 'Наименование основания';
CREATE TABLE SightFuncPersonScheme (
  ID           number(10) NOT NULL, 
  Num          number(10), 
  IsActive     number(1), 
  FuncPersonID number(10) NOT NULL, 
  PRIMARY KEY (ID));
COMMENT ON TABLE SightFuncPersonScheme IS 'Схема визирования функционального заказчика';
COMMENT ON COLUMN SightFuncPersonScheme.ID IS 'Идентификатор';
COMMENT ON COLUMN SightFuncPersonScheme.Num IS 'Номер';
COMMENT ON COLUMN SightFuncPersonScheme.IsActive IS 'Признак активности';
COMMENT ON COLUMN SightFuncPersonScheme.FuncPersonID IS 'Ссылка на представителя (персону)функционального заказчика';
CREATE TABLE ActPaymentDocument (
  ID                number(10) NOT NULL, 
  ActID             number(10) NOT NULL, 
  PaymentDocumentID number(10) NOT NULL, 
  PRIMARY KEY (ID), 
  CONSTRAINT U_ACT_PAYMENTDOC 
    UNIQUE (ActID, PaymentDocumentID));
COMMENT ON TABLE ActPaymentDocument IS 'Платежные документы по акту сдачи-приёмки';
COMMENT ON COLUMN ActPaymentDocument.ActID IS 'Ссылка на акт сдачи-приёмки';
COMMENT ON COLUMN ActPaymentDocument.PaymentDocumentID IS 'Ссылка на платежный документ';
CREATE TABLE TransferAct (
  ID                number(10) NOT NULL, 
  Num               number(10), 
  SignDate          date, 
  TransferActTypeID number(10) NOT NULL, 
  PRIMARY KEY (ID));
COMMENT ON TABLE TransferAct IS 'Акт приёмки-передачи - акт регистрирующий, какие документы были переданы заказчику';
COMMENT ON COLUMN TransferAct.ID IS 'Идентификатор';
COMMENT ON COLUMN TransferAct.Num IS 'Номер';
COMMENT ON COLUMN TransferAct.SignDate IS 'Дата подписания акта передачи';
COMMENT ON COLUMN TransferAct.TransferActTypeID IS 'Ссылка на тип акта приёмки-передачи';
CREATE TABLE TransferActType (
  ID   number(10) NOT NULL, 
  Name nvarchar2(255) NOT NULL UNIQUE, 
  PRIMARY KEY (ID));
COMMENT ON TABLE TransferActType IS 'Тип акта приёмки-передачи(сп)';
COMMENT ON COLUMN TransferActType.ID IS 'Идентификатор';
COMMENT ON COLUMN TransferActType.Name IS 'Наименование типа';
CREATE TABLE Document (
  ID   number(10) NOT NULL, 
  Name nvarchar2(255) NOT NULL UNIQUE, 
  PRIMARY KEY (ID));
COMMENT ON TABLE Document IS 'Документ (напр. Техническое задание, календарный план,...)';
COMMENT ON COLUMN Document.ID IS 'Идентификатор';
COMMENT ON COLUMN Document.Name IS 'Наименование документа';
CREATE TABLE TransferActActDocument (
  ID            number(10) NOT NULL, 
  TransferActID number(10) NOT NULL, 
  DocumentID    number(10) NOT NULL, 
  ActID         number(10) NOT NULL, 
  PagesCount    number(10), 
  PRIMARY KEY (ID), 
  CONSTRAINT U_TRANSFERACT_ACT_DOC 
    UNIQUE (TransferActID, DocumentID, ActID));
COMMENT ON TABLE TransferActActDocument IS 'Соответсвие акта сдачи-приемки документу, полученному по акту приемки-передачи (сс)';
COMMENT ON COLUMN TransferActActDocument.TransferActID IS 'Ссылка на акт приёмки-передачи';
COMMENT ON COLUMN TransferActActDocument.DocumentID IS 'Ссылка на документ';
COMMENT ON COLUMN TransferActActDocument.ActID IS 'Ссылка на акт сдачи-приемки';
COMMENT ON COLUMN TransferActActDocument.PagesCount IS 'Количество страниц';
CREATE TABLE TransferActTypeDocument (
  ID                number(10) NOT NULL, 
  TransferActTypeID number(10) NOT NULL, 
  DocumentID        number(10) NOT NULL, 
  PRIMARY KEY (ID), 
  CONSTRAINT U_TRANSFER_ACTTYPE_DOC 
    UNIQUE (TransferActTypeID, DocumentID));
COMMENT ON TABLE TransferActTypeDocument IS 'Тип документа (сс)';
COMMENT ON COLUMN TransferActTypeDocument.TransferActTypeID IS 'Ссылка на тип';
COMMENT ON COLUMN TransferActTypeDocument.DocumentID IS 'Ссылка на документ';
CREATE TABLE ContractTranActDoc (
  ID            number(10) NOT NULL, 
  ContractDocID number(10) NOT NULL, 
  TransferActID number(10) NOT NULL, 
  DocumentID    number(10) NOT NULL, 
  PagesCount    number(10), 
  PRIMARY KEY (ID), 
  CONSTRAINT U_TRANS_ACTDOC_CONTRACT 
    UNIQUE (ContractDocID, TransferActID, DocumentID));
COMMENT ON TABLE ContractTranActDoc IS 'Документы, преданные к договору согласно акту приёмки-передачи';
COMMENT ON COLUMN ContractTranActDoc.ContractDocID IS 'Ссылка на договор';
COMMENT ON COLUMN ContractTranActDoc.TransferActID IS 'Ссылка на акт приемки-передачи';
COMMENT ON COLUMN ContractTranActDoc.DocumentID IS 'ссылка на документ';
COMMENT ON COLUMN ContractTranActDoc.PagesCount IS 'Количество страниц';
CREATE TABLE StageResult (
  ID                       number(10) NOT NULL, 
  Name                     nvarchar2(2000), 
  EconomicEfficiencyTypeID number(10), 
  StageID                  number(10) NOT NULL, 
  NTPSubViewID             number(10) NOT NULL, 
  PRIMARY KEY (ID), 
  CONSTRAINT U_STAGE_RESULT 
    UNIQUE (StageID, Name));
COMMENT ON TABLE StageResult IS 'Результат этапа (сс)';
COMMENT ON COLUMN StageResult.ID IS 'Идентификатор';
COMMENT ON COLUMN StageResult.Name IS 'Наименование результата';
COMMENT ON COLUMN StageResult.EconomicEfficiencyTypeID IS 'Ссылка на тип экономической эффективности';
COMMENT ON COLUMN StageResult.StageID IS 'Ссылка на этап';
COMMENT ON COLUMN StageResult.NTPSubViewID IS 'Ссылка на подвид НТП';
CREATE TABLE NTPSubView (
  ID        number(10) NOT NULL, 
  Name      nvarchar2(255), 
  NTPViewID number(10) NOT NULL, 
  ShortName nvarchar2(50), 
  PRIMARY KEY (ID));
COMMENT ON TABLE NTPSubView IS 'Подвид НТП';
COMMENT ON COLUMN NTPSubView.ID IS 'Идентификатор';
COMMENT ON COLUMN NTPSubView.Name IS 'Название подвида НТП';
COMMENT ON COLUMN NTPSubView.NTPViewID IS 'Ссылка на вид НТП';
COMMENT ON COLUMN NTPSubView.ShortName IS 'Краткое наименование НТП';
CREATE TABLE NTPView (
  ID   number(10) NOT NULL, 
  Name nvarchar2(255) NOT NULL UNIQUE, 
  PRIMARY KEY (ID));
COMMENT ON TABLE NTPView IS 'Вид НТП (сп)';
COMMENT ON COLUMN NTPView.ID IS 'Идентификатор';
COMMENT ON COLUMN NTPView.Name IS 'Название (напр. Концепция или Нормативно-методическая документация)';
CREATE TABLE EconomEfficiencyType (
  ID   number(10) NOT NULL, 
  Name nvarchar2(255) NOT NULL UNIQUE, 
  PRIMARY KEY (ID));
COMMENT ON TABLE EconomEfficiencyType IS 'Тип экономической эффективности (сп)';
COMMENT ON COLUMN EconomEfficiencyType.ID IS 'Идентификатор';
COMMENT ON COLUMN EconomEfficiencyType.Name IS 'Название';
CREATE TABLE EconomEfficiencyParameter (
  ID   number(10) NOT NULL, 
  Name nvarchar2(255) NOT NULL UNIQUE, 
  PRIMARY KEY (ID));
COMMENT ON TABLE EconomEfficiencyParameter IS 'Параметр экономической эффективности (сп)';
COMMENT ON COLUMN EconomEfficiencyParameter.ID IS 'Идентификатор';
COMMENT ON COLUMN EconomEfficiencyParameter.Name IS 'Название параметра';
CREATE TABLE EfficienceParameterType (
  ID                          number(10) NOT NULL, 
  EconomEfficiencyParameterID number(10) NOT NULL, 
  EconomEfficiencyTypeID      number(10) NOT NULL, 
  PRIMARY KEY (ID), 
  CONSTRAINT U_EF_PAR_TYPE 
    UNIQUE (EconomEfficiencyParameterID, EconomEfficiencyTypeID));
COMMENT ON TABLE EfficienceParameterType IS 'Парметры типа экономической эффективности (сс)';
COMMENT ON COLUMN EfficienceParameterType.EconomEfficiencyParameterID IS 'Ссылка на параметр экономической эффективности';
COMMENT ON COLUMN EfficienceParameterType.EconomEfficiencyTypeID IS 'Ссылка на тип экономической эффективности';
CREATE TABLE EfParameterStageResult (
  ID                          number(10) NOT NULL, 
  EconomEfficiencyParameterID number(10) NOT NULL, 
  StageResultID               number(10) NOT NULL, 
  Value                       number(19, 6), 
  PRIMARY KEY (ID), 
  CONSTRAINT U_EF_STAGE_RESULT 
    UNIQUE (EconomEfficiencyParameterID, StageResultID));
COMMENT ON TABLE EfParameterStageResult IS 'Значения параметров экономической эффективности результата этапа (сс)';
COMMENT ON COLUMN EfParameterStageResult.EconomEfficiencyParameterID IS 'Ссылка на параметр экономической эфффективности';
COMMENT ON COLUMN EfParameterStageResult.StageResultID IS 'Ссылка на результат этапа';
COMMENT ON COLUMN EfParameterStageResult.Value IS 'Значение параметра';
CREATE TABLE ContractorPosition (
  ID           number(10) NOT NULL, 
  ContractorID number(10) NOT NULL, 
  PositionID   number(10) NOT NULL, 
  PRIMARY KEY (ID));
COMMENT ON TABLE ContractorPosition IS 'Люди и их должности в организациях (Газпроме, дочерних организациях, сторонних организациях, кроме Промгаза) (сс)';
COMMENT ON COLUMN ContractorPosition.ID IS 'Идентификатор';
COMMENT ON COLUMN ContractorPosition.ContractorID IS 'Ссылка на контрагента (организацию)';
COMMENT ON COLUMN ContractorPosition.PositionID IS 'Должность';
CREATE TABLE ContractorAuthority (
  ID           number(10) NOT NULL, 
  AuthorityID  number(10) NOT NULL, 
  NumDocument  nvarchar2(255), 
  ValidFrom    date, 
  ValidTo      date, 
  IsValid      number(1), 
  ContractorID number(10) NOT NULL, 
  PRIMARY KEY (ID));
COMMENT ON TABLE ContractorAuthority IS 'Основание для предсприятия-исполнителя';
COMMENT ON COLUMN ContractorAuthority.ID IS 'Идентификатор';
COMMENT ON COLUMN ContractorAuthority.AuthorityID IS 'Ссылка на основание';
COMMENT ON COLUMN ContractorAuthority.NumDocument IS 'Номер документа';
COMMENT ON COLUMN ContractorAuthority.ValidFrom IS 'Дата начала действия';
COMMENT ON COLUMN ContractorAuthority.ValidTo IS 'Дата окончания действия';
COMMENT ON COLUMN ContractorAuthority.IsValid IS 'Признак активности';
COMMENT ON COLUMN ContractorAuthority.ContractorID IS 'Ссылка на ответсвенного со стороны исполнителя';
CREATE TABLE Position (
  ID   number(10) NOT NULL, 
  Name nvarchar2(255) NOT NULL UNIQUE, 
  PRIMARY KEY (ID));
COMMENT ON TABLE Position IS 'Должность (сп)';
COMMENT ON COLUMN Position.ID IS 'Идентификатор';
COMMENT ON COLUMN Position.Name IS 'Название';
CREATE TABLE SubGeneralHierarchi (
  ID                        number(10) NOT NULL, 
  GeneralContractDocStageID number(10) NOT NULL, 
  SubContractDocStageID     number(10) NOT NULL, 
  PRIMARY KEY (ID), 
  CONSTRAINT U_STAGE_SUBCONTRACT_STAGE 
    UNIQUE (GeneralContractDocStageID, SubContractDocStageID));
COMMENT ON TABLE SubGeneralHierarchi IS 'Связь этапов генерального договора с субподрядными договорами (сс)';
COMMENT ON COLUMN SubGeneralHierarchi.GeneralContractDocStageID IS 'Ссылка на этап генерального договора';
COMMENT ON COLUMN SubGeneralHierarchi.SubContractDocStageID IS 'Ссылка на этап субподрядного договора';
CREATE TABLE UDMetadata (
  SchemeRelease   number(10) NOT NULL, 
  SchemeBuild     number(10) NOT NULL, 
  SchemeTimestamp date DEFAULT sysdate NOT NULL, 
  PRIMARY KEY (SchemeRelease, 
  SchemeBuild));
COMMENT ON COLUMN UDMetadata.SchemeRelease IS 'Старшая цифра версии схемы';
COMMENT ON COLUMN UDMetadata.SchemeBuild IS 'Младшая цифра версии схемы';
COMMENT ON COLUMN UDMetadata.SchemeTimestamp IS 'Временная отметка создания схемы';
CREATE TABLE IMPORTINGSCHEME (
  ID         number(10) NOT NULL, 
  Schemename nvarchar2(500) NOT NULL UNIQUE, 
  PRIMARY KEY (ID));
COMMENT ON TABLE IMPORTINGSCHEME IS 'Схема импорта';
COMMENT ON COLUMN IMPORTINGSCHEME.ID IS 'Идентификатор схемы импорта';
COMMENT ON COLUMN IMPORTINGSCHEME.Schemename IS 'Имя схемы импорта';
CREATE TABLE IMPORTINGSCHEMEITEM (
  ID          number(10, 0) NOT NULL, 
  Columnname  nvarchar2(200) NOT NULL, 
  Columnsign  nvarchar2(20) NOT NULL, 
  Columnindex number(10) NOT NULL, 
  Isrequired  number(1) NOT NULL, 
  SchemeID    number(10) NOT NULL, 
  PRIMARY KEY (ID));
COMMENT ON TABLE IMPORTINGSCHEMEITEM IS 'Элемент схемы импорта';
COMMENT ON COLUMN IMPORTINGSCHEMEITEM.ID IS 'Идентификатор элемента схемы импорта';
COMMENT ON COLUMN IMPORTINGSCHEMEITEM.Columnname IS 'Имя столбца';
COMMENT ON COLUMN IMPORTINGSCHEMEITEM.Columnsign IS 'Знак';
COMMENT ON COLUMN IMPORTINGSCHEMEITEM.Columnindex IS 'Индекс столбца';
COMMENT ON COLUMN IMPORTINGSCHEMEITEM.Isrequired IS 'Является ли столбец обязательным';
COMMENT ON COLUMN IMPORTINGSCHEMEITEM.SchemeID IS 'Ссылка на схему импорта';
CREATE TABLE Department (
  ID           number(10) NOT NULL, 
  ParentID     number(10), 
  ManagerID    number(10) NOT NULL, 
  DirectedByID number(10) NOT NULL, 
  Name         nvarchar2(200) NOT NULL UNIQUE, 
  PRIMARY KEY (ID));
COMMENT ON COLUMN Department.ManagerID IS 'Руководитель отдела';
COMMENT ON COLUMN Department.DirectedByID IS 'Замдир для отдела';
CREATE TABLE Post (
  ID   number(10) NOT NULL, 
  Name nvarchar2(200) NOT NULL UNIQUE, 
  PRIMARY KEY (ID));
COMMENT ON TABLE Post IS 'Должность';
CREATE TABLE ResponsibleForOrder (
  ID                           number(10) NOT NULL, 
  DepartmentID                 number(10) NOT NULL, 
  EmployeeID                   number(10) NOT NULL, 
  ResponsibleAssignmentOrderID number(10), 
  PRIMARY KEY (ID));
COMMENT ON TABLE ResponsibleForOrder IS 'Ответственный по приказу';
CREATE TABLE Location (
  ID   number(10) NOT NULL, 
  Name nvarchar2(200) NOT NULL UNIQUE, 
  PRIMARY KEY (ID));
COMMENT ON TABLE Location IS 'Место нахождения';
COMMENT ON COLUMN Location.Name IS 'Название места нахождения';
CREATE TABLE ApprovalState (
  ID   number(10) NOT NULL, 
  Name nvarchar2(200) NOT NULL UNIQUE, 
  PRIMARY KEY (ID));
COMMENT ON TABLE ApprovalState IS 'Состояние согласования документа';
COMMENT ON COLUMN ApprovalState.Name IS 'Название состояния согласования';
CREATE TABLE ApprovalProcess (
  ID              number(10) NOT NULL, 
  ContractDocID   number(10) NOT NULL, 
  ToLocationID    number(10) NOT NULL, 
  FromLocationId  number(10) NOT NULL, 
  ApprovalStateID number(10) NOT NULL, 
  ApprovalGoalID  number(10) NOT NULL, 
  TransferStateAt date NOT NULL, 
  EnterStateAt    date NOT NULL, 
  MissiveId       nvarchar2(20), 
  MissiveDate     date, 
  MissiveTypeID   number(10) NOT NULL, 
  Description     nvarchar2(1000), 
  EnteringDate    date DEFAULT current_date NOT NULL, 
  PRIMARY KEY (ID));
COMMENT ON TABLE ApprovalProcess IS 'Журнал согласования договора';
COMMENT ON COLUMN ApprovalProcess.ToLocationID IS 'Кому передано';
COMMENT ON COLUMN ApprovalProcess.FromLocationId IS 'Кто передал';
COMMENT ON COLUMN ApprovalProcess.TransferStateAt IS 'Дата передачи документа в состояние';
COMMENT ON COLUMN ApprovalProcess.EnterStateAt IS 'Дата поступления документа в состояние';
COMMENT ON COLUMN ApprovalProcess.MissiveId IS 'Номер письма';
COMMENT ON COLUMN ApprovalProcess.MissiveDate IS 'Дата письма';
COMMENT ON COLUMN ApprovalProcess.Description IS 'Примечание';
CREATE TABLE Contractdoc_Funds_Fact (
  ContractdocID  number(10) NOT NULL, 
  Time_DimID     number(10) NOT NULL, 
  FundsDisbursed number(18, 2) NOT NULL, 
  FundsLeft      number(18, 2) NOT NULL, 
  FundsTotal     number(18, 2) NOT NULL, 
  PRIMARY KEY (ContractdocID, 
  Time_DimID));
CREATE TABLE Time_Dim (
  ID   number(10) NOT NULL, 
  Year number(5) NOT NULL, 
  PRIMARY KEY (ID));
CREATE TABLE MissiveType (
  ID   number(10) NOT NULL, 
  Name nvarchar2(20) NOT NULL UNIQUE, 
  PRIMARY KEY (ID));
COMMENT ON TABLE MissiveType IS 'Тип письма';
CREATE TABLE ApprovalGoal (
  ID   number(10) NOT NULL, 
  Name nvarchar2(200) NOT NULL UNIQUE, 
  PRIMARY KEY (ID));
COMMENT ON TABLE ApprovalGoal IS 'Цель направления на согласование';
CREATE TABLE ResponsibleAssignmentOrder (
  ID        number(10) NOT NULL, 
  OrderNum  nvarchar2(20), 
  OrderDate date, 
  PRIMARY KEY (ID));

CREATE VIEW "ContractRepositoryView" AS
SELECT c.Price, c.StartAt, c.EndsAt, c.AppliedAt, c.ApprovedAt, c. InternalNum, c.PrepaymentSum, c.PrepaymentPercent, c.IsProtectability, c.Subject, c.IsActive, n.Percents as "NDSPercents", ct.Name as "ContractTypeName", cur.Name as "CurrencyName", ctr.ShortName as "ContractorShortName", ctt.Name as "ContractorType", alg.Name as "NDSAlgorithm" FROM "UD".Contractdoc c 
INNER JOIN "UD".NDS n  ON c.NDSID = n.ID
INNER JOIN "UD".ContractType ct ON c.ContractTypeID = ct.ID
INNER JOIN "UD".Currency cur ON c.CurrencyID = cur.ID
INNER JOIN "UD".NDSAlgorithm alg ON c.NDSAlgorithmID = alg.ID
INNER JOIN "UD".Contractor ctr ON c.ContractorID = ctr.ID
INNER JOIN "UD".ContractorType ctt ON ctr.ContractorTypeID = ctt.ID;
