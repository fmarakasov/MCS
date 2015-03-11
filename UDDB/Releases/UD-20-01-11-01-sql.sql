/*
Система учёта договоров 1.0
Скрипт сгенерирован 02:33:04 PM 20/01/2011
Версия схемы 1.0
*/

alter session set nls_date_format='DD.MM.YYYY HH24:MI:SS';
ALTER SESSION SET CONSTRAINTS = DEFERRED;
DROP PROCEDURE InsertNextSchemeVersion;
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
DROP TABLE AdditionContractorPropertiy CASCADE CONSTRAINTS;
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
DROP TABLE Act_PaymentDocument CASCADE CONSTRAINTS;
DROP TABLE TransferAct CASCADE CONSTRAINTS;
DROP TABLE TransferActType CASCADE CONSTRAINTS;
DROP TABLE Document CASCADE CONSTRAINTS;
DROP TABLE TransferAct_Act_Document CASCADE CONSTRAINTS;
DROP TABLE TransferActType_Document CASCADE CONSTRAINTS;
DROP TABLE Contract_TransferAct_Document CASCADE CONSTRAINTS;
DROP TABLE StageResult CASCADE CONSTRAINTS;
DROP TABLE NTPSubView CASCADE CONSTRAINTS;
DROP TABLE NTPView CASCADE CONSTRAINTS;
DROP TABLE EconomEfficiencyType CASCADE CONSTRAINTS;
DROP TABLE EconomEfficiencyParameter CASCADE CONSTRAINTS;
DROP TABLE EfficienceParameter_Type CASCADE CONSTRAINTS;
DROP TABLE EfficParameter_StageResult CASCADE CONSTRAINTS;
DROP TABLE ContractorPosition CASCADE CONSTRAINTS;
DROP TABLE ContractorAuthority CASCADE CONSTRAINTS;
DROP TABLE Position CASCADE CONSTRAINTS;
DROP TABLE SubGeneralHierarchi CASCADE CONSTRAINTS;
DROP TABLE UDMetadata CASCADE CONSTRAINTS;
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
CREATE SEQUENCE seq_ContractDoc START WITH 10000 INCREMENT BY 10;
CREATE SEQUENCE seq_ContractState START WITH 10000;
CREATE SEQUENCE seq_Currency START WITH 10000;
CREATE SEQUENCE seq_ContractType START WITH 10000;
CREATE SEQUENCE seq_Document START WITH 10000;
CREATE SEQUENCE seq_WorkType START WITH 10000;
CREATE SEQUENCE seq_Schedule START WITH 1000;
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
  PrepaymentSum            number(10, 2), 
  PrepaymentPercent        float(10), 
  PrepaymentNDSAlgorithmID number(10), 
  NDSID                    number(10), 
  ContractStateID          number(10) NOT NULL, 
  CurrencyMeasureID        number(10), 
  ContractorID             number(10), 
  IsProtectability         number(1), 
  Subject                  nvarchar2(2000) NOT NULL, 
  CurrencyRate             number(18, 2), 
  RateDate                 date, 
  Description              nvarchar2(2000), 
  ContractorPersonID       number(10), 
  IsActive                 number(1), 
  AuthorityID              number(10), 
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
CREATE TABLE ContractType (
  ID   number(10) NOT NULL, 
  Name nvarchar2(200) NOT NULL UNIQUE, 
  PRIMARY KEY (ID));
COMMENT ON TABLE ContractType IS 'Тип договора (сп)';
COMMENT ON COLUMN ContractType.ID IS 'Идентификатор';
COMMENT ON COLUMN ContractType.Name IS 'Название (напр. НИОКР)';
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
  ID   number(10) NOT NULL, 
  Name nvarchar2(100) NOT NULL UNIQUE, 
  PRIMARY KEY (ID));
COMMENT ON TABLE NDSAlgorithm IS 'Алгоритм НДС (сп)';
COMMENT ON COLUMN NDSAlgorithm.ID IS 'Идентификатор';
COMMENT ON COLUMN NDSAlgorithm.Name IS 'Название (напр. без НДС)';
CREATE TABLE Prepayment (
  ContractDocID number(10) NOT NULL, 
  Year          number(4) NOT NULL, 
  Sum           number(10, 2) NOT NULL, 
  "Percent"     float(10) NOT NULL, 
  PRIMARY KEY (ContractDocID, 
  Year));
COMMENT ON TABLE Prepayment IS 'Предоплата по годам';
COMMENT ON COLUMN Prepayment.ContractDocID IS 'Ссылка на контракт';
COMMENT ON COLUMN Prepayment.Year IS 'Год, в который выдан аванс';
COMMENT ON COLUMN Prepayment.Sum IS 'Сумма аванса';
COMMENT ON COLUMN Prepayment."Percent" IS 'Процент от общей стоимости договора (0..100)';
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
  City             nvarchar2(50), 
  Street           nvarchar2(255), 
  Build            nvarchar2(4), 
  Block            nvarchar2(4), 
  Bank             nvarchar2(200), 
  ContractorTypeID number(10) NOT NULL, 
  Appartment       nvarchar2(5), 
  INN              nvarchar2(30), 
  Account          nvarchar2(30), 
  BIK              nvarchar2(30), 
  KPP              nvarchar2(30), 
  PRIMARY KEY (ID));
COMMENT ON TABLE Contractor IS 'Улица';
COMMENT ON COLUMN Contractor.ID IS 'Идентификтор контрагента';
COMMENT ON COLUMN Contractor.Name IS 'Название организации';
COMMENT ON COLUMN Contractor.ShortName IS 'Короткое название организации';
COMMENT ON COLUMN Contractor.Zip IS 'Индекс';
COMMENT ON COLUMN Contractor.City IS 'Город';
COMMENT ON COLUMN Contractor.Build IS 'Номер дома';
COMMENT ON COLUMN Contractor.Block IS 'Номер корпуса';
COMMENT ON COLUMN Contractor.Bank IS 'Банк';
COMMENT ON COLUMN Contractor.ContractorTypeID IS 'Ссылка на тип контрагента';
COMMENT ON COLUMN Contractor.Appartment IS 'Номер квартиры';
COMMENT ON COLUMN Contractor.INN IS 'ИНН';
COMMENT ON COLUMN Contractor.Account IS 'Номер счёта';
COMMENT ON COLUMN Contractor.BIK IS 'БИК';
COMMENT ON COLUMN Contractor.KPP IS 'КПП';
CREATE TABLE Degree (
  ID   number(10) NOT NULL, 
  Name nvarchar2(50) NOT NULL UNIQUE, 
  PRIMARY KEY (ID));
COMMENT ON TABLE Degree IS 'Степень (сп)';
COMMENT ON COLUMN Degree.ID IS 'Идентификатор';
COMMENT ON COLUMN Degree.Name IS 'Название (напр. кондидат наук)';
CREATE TABLE NDS (
  ID        number(10) NOT NULL, 
  "Percent" number(4, 2) NOT NULL UNIQUE, 
  Year      number(4) NOT NULL UNIQUE, 
  PRIMARY KEY (ID));
COMMENT ON TABLE NDS IS 'НДС (сп)';
COMMENT ON COLUMN NDS.ID IS 'Идентификаьтор';
COMMENT ON COLUMN NDS."Percent" IS 'Ставка НДС (0...100) (напр. 18)';
COMMENT ON COLUMN NDS.Year IS 'Год начала дейтсвия';
CREATE TABLE ContractState (
  ID   number(10) NOT NULL, 
  Name nvarchar2(100) NOT NULL UNIQUE, 
  PRIMARY KEY (ID));
COMMENT ON TABLE ContractState IS 'Состояние договора (сп)';
COMMENT ON COLUMN ContractState.ID IS 'Идентификатор';
COMMENT ON COLUMN ContractState.Name IS 'Название (напр. подписан)';
CREATE TABLE ContractHierarchy (
  GeneralContractDocID number(10) NOT NULL, 
  SubContractDocID     number(10) NOT NULL, 
  PRIMARY KEY (GeneralContractDocID, 
  SubContractDocID));
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
  PaymentDocumentID number(10) NOT NULL, 
  ContractDocID     number(10) NOT NULL, 
  PRIMARY KEY (PaymentDocumentID, 
  ContractDocID));
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
  TroubleID     number(10) NOT NULL, 
  ContractDocID number(10) NOT NULL, 
  Description   nvarchar2(200), 
  PRIMARY KEY (TroubleID, 
  ContractDocID));
COMMENT ON TABLE ContractTrouble IS 'Проблемы, к которым относен договор (сс)';
COMMENT ON COLUMN ContractTrouble.TroubleID IS 'Ссылка на проблемы';
COMMENT ON COLUMN ContractTrouble.ContractDocID IS 'Ссылка на договор';
COMMENT ON COLUMN ContractTrouble.Description IS 'Описание проблемы в договоре';
CREATE TABLE Property (
  ID   number(10) NOT NULL, 
  Name nvarchar2(50) NOT NULL UNIQUE, 
  PRIMARY KEY (ID));
COMMENT ON TABLE Property IS 'Дополнительные свойства (для контрагентов)(сп)';
COMMENT ON COLUMN Property.ID IS 'Идентификатор';
COMMENT ON COLUMN Property.Name IS 'Название';
CREATE TABLE AdditionContractorPropertiy (
  PropertyID   number(10) NOT NULL, 
  ContractorID number(10) NOT NULL, 
  Value        nvarchar2(1000) NOT NULL, 
  PRIMARY KEY (PropertyID, 
  ContractorID));
COMMENT ON TABLE AdditionContractorPropertiy IS 'Значения дополнительных атрибутов для контрагента (сс)';
COMMENT ON COLUMN AdditionContractorPropertiy.PropertyID IS 'Ссылка на атрибут';
COMMENT ON COLUMN AdditionContractorPropertiy.ContractorID IS 'Ссылка на контрагента';
COMMENT ON COLUMN AdditionContractorPropertiy.Value IS 'Значение атрибута';
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
  FunctionalCustomerID number(10) NOT NULL, 
  ContractDocID        number(10) NOT NULL, 
  Description          nvarchar2(1000), 
  PRIMARY KEY (FunctionalCustomerID, 
  ContractDocID));
COMMENT ON TABLE FunctionalCustomerContract IS 'Функциональный заказчик договора (сс)';
COMMENT ON COLUMN FunctionalCustomerContract.FunctionalCustomerID IS 'Ссылка на функционального заказчика';
COMMENT ON COLUMN FunctionalCustomerContract.ContractDocID IS 'Ссылка на договор';
COMMENT ON COLUMN FunctionalCustomerContract.Description IS 'Описание функционального заказчика в договоре';
CREATE TABLE ContractorType (
  ID   number(10) NOT NULL, 
  Name nvarchar2(100) NOT NULL UNIQUE, 
  PRIMARY KEY (ID));
COMMENT ON TABLE ContractorType IS 'Тип контрагента (сп)';
COMMENT ON COLUMN ContractorType.ID IS 'Идентификатор';
COMMENT ON COLUMN ContractorType.Name IS 'Название типа (например. Дочерние организации)';
CREATE TABLE Employee (
  ID         number(10) NOT NULL, 
  ManagerID  number(10), 
  RoleID     number(10) NOT NULL, 
  Familyname nvarchar2(40) NOT NULL, 
  FirstName  nvarchar2(40), 
  Middlename nvarchar2(40), 
  Sex        number(1), 
  PRIMARY KEY (ID));
COMMENT ON TABLE Employee IS 'Сотрудник Промгаза';
COMMENT ON COLUMN Employee.ID IS 'Идентификатор';
COMMENT ON COLUMN Employee.ManagerID IS 'Ссылка на руководителя';
COMMENT ON COLUMN Employee.RoleID IS 'Ссылка на роль (напр. руководитель направления?)';
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
  ContractDocID number(10) NOT NULL, 
  ScheduleID    number(10) NOT NULL, 
  AppNum        number(3), 
  PRIMARY KEY (ContractDocID, 
  ScheduleID));
COMMENT ON TABLE ScheduleContract IS 'Календарный план договора (сс)';
COMMENT ON COLUMN ScheduleContract.ContractDocID IS 'Ссылка на договор';
COMMENT ON COLUMN ScheduleContract.AppNum IS '№ приложения договора';
CREATE TABLE Stage (
  ID             number(10) NOT NULL, 
  Num            nvarchar2(25) NOT NULL, 
  Subject        nvarchar2(400) NOT NULL, 
  StartsAt       date NOT NULL, 
  EndsAt         date NOT NULL, 
  Price          number(18, 2) NOT NULL, 
  NdsAlgorithmID number(10) NOT NULL, 
  ParentID       number(10), 
  ActID          number(10), 
  ScheduleID     number(10) NOT NULL, 
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
CREATE TABLE Act (
  ID                    number(10) NOT NULL, 
  Num                   nvarchar2(255), 
  "Date"                date, 
  ActTypeID             number(10) NOT NULL, 
  NDSID                 number(10) NOT NULL, 
  RegionID              number(10) NOT NULL, 
  TotalSum              number(10), 
  SumForTransfer        number(10), 
  Status                number(10), 
  EnterpriceAuthorityID number(10) NOT NULL, 
  CurrencyRate          number(18, 2), 
  RateDate              date, 
  PRIMARY KEY (ID));
COMMENT ON TABLE Act IS 'Акт сдачи-приёмки';
COMMENT ON COLUMN Act.ID IS 'Идентификатор';
COMMENT ON COLUMN Act.Num IS 'Номер акта сдачи-приёмки';
COMMENT ON COLUMN Act."Date" IS 'Дата акта сдачи-приёмки';
COMMENT ON COLUMN Act.ActTypeID IS 'Ссылка на тип акта сдачи-приёмки';
COMMENT ON COLUMN Act.NDSID IS 'Ссылка на ставку НДС';
COMMENT ON COLUMN Act.RegionID IS 'Ссылка на регион';
COMMENT ON COLUMN Act.TotalSum IS 'Общая сумма по акту сдачи-приёмки';
COMMENT ON COLUMN Act.SumForTransfer IS 'Сумма к перечислению';
COMMENT ON COLUMN Act.Status IS 'Состояние акта сдачи-приёмки';
COMMENT ON COLUMN Act.EnterpriceAuthorityID IS 'Ссылка на основание для Промгаза';
COMMENT ON COLUMN Act.CurrencyRate IS 'Курс валюты на дату RateDate';
COMMENT ON COLUMN Act.RateDate IS 'Дата для курса валюты';
CREATE TABLE ClosedStageRelation (
  StageID       number(10) NOT NULL, 
  ClosedStageID number(10) NOT NULL, 
  PRIMARY KEY (StageID, 
  ClosedStageID));
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
  ID   number(10) NOT NULL, 
  Name nvarchar2(255) NOT NULL UNIQUE, 
  PRIMARY KEY (ID));
COMMENT ON TABLE WorkType IS 'Тип работ (сп)';
COMMENT ON COLUMN WorkType.ID IS 'Идентификатор';
COMMENT ON COLUMN WorkType.Name IS 'Название типа работ';
CREATE TABLE Disposal (
  ID            number(10) NOT NULL, 
  Num           nvarchar2(10), 
  ApprovedDate  date, 
  ContractdocID number(10) NOT NULL, 
  PRIMARY KEY (ID));
COMMENT ON TABLE Disposal IS 'Распоряжение, устанавливающее ответственных по договору со стороны Промгаза';
COMMENT ON COLUMN Disposal.ID IS 'Идентификатор';
COMMENT ON COLUMN Disposal.Num IS 'Номер распоряжения';
COMMENT ON COLUMN Disposal.ApprovedDate IS 'Дата утверждения';
COMMENT ON COLUMN Disposal.ContractdocID IS 'Ссылка на договор по которому сделано распоряжение';
CREATE TABLE Responsible (
  DisposalID number(10) NOT NULL, 
  EmployeeID number(10) NOT NULL, 
  PRIMARY KEY (DisposalID, 
  EmployeeID));
COMMENT ON TABLE Responsible IS 'Ответственные со стороны Промгаза, согласно распоряжению (сс)';
COMMENT ON COLUMN Responsible.DisposalID IS 'Ссылка на распоряжение';
COMMENT ON COLUMN Responsible.EmployeeID IS 'Ссылка на ответсвенного от заказчика согласно распоряжению';
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
CREATE TABLE Act_PaymentDocument (
  ActID             number(10) NOT NULL, 
  PaymentDocumentID number(10) NOT NULL, 
  PRIMARY KEY (ActID, 
  PaymentDocumentID));
COMMENT ON TABLE Act_PaymentDocument IS 'Платежные документы по акту сдачи-приёмки';
COMMENT ON COLUMN Act_PaymentDocument.ActID IS 'Ссылка на акт сдачи-приёмки';
COMMENT ON COLUMN Act_PaymentDocument.PaymentDocumentID IS 'Ссылка на платежный документ';
CREATE TABLE TransferAct (
  ID                number(10) NOT NULL, 
  Num               number(10), 
  "Date"            date, 
  TransferActTypeID number(10) NOT NULL, 
  PRIMARY KEY (ID));
COMMENT ON TABLE TransferAct IS 'Акт приёмки-передачи - акт регистрирующий, какие документы были переданы заказчику';
COMMENT ON COLUMN TransferAct.ID IS 'Идентификатор';
COMMENT ON COLUMN TransferAct.Num IS 'Номер';
COMMENT ON COLUMN TransferAct."Date" IS 'Дата акта';
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
CREATE TABLE TransferAct_Act_Document (
  TransferActID number(10) NOT NULL, 
  DocumentID    number(10) NOT NULL, 
  ActID         number(10) NOT NULL, 
  PagesCount    number(10), 
  PRIMARY KEY (TransferActID, 
  DocumentID, 
  ActID));
COMMENT ON TABLE TransferAct_Act_Document IS 'Соответсвие акта сдачи-приемки документу, полученному по акту приемки-передачи (сс)';
COMMENT ON COLUMN TransferAct_Act_Document.TransferActID IS 'Ссылка на акт приёмки-передачи';
COMMENT ON COLUMN TransferAct_Act_Document.DocumentID IS 'Ссылка на документ';
COMMENT ON COLUMN TransferAct_Act_Document.ActID IS 'Ссылка на акт сдачи-приемки';
COMMENT ON COLUMN TransferAct_Act_Document.PagesCount IS 'Количество страниц';
CREATE TABLE TransferActType_Document (
  TransferActTypeID number(10) NOT NULL, 
  DocumentID        number(10) NOT NULL, 
  PRIMARY KEY (TransferActTypeID, 
  DocumentID));
COMMENT ON TABLE TransferActType_Document IS 'Тип документа (сс)';
COMMENT ON COLUMN TransferActType_Document.TransferActTypeID IS 'Ссылка на тип';
COMMENT ON COLUMN TransferActType_Document.DocumentID IS 'Ссылка на документ';
CREATE TABLE Contract_TransferAct_Document (
  ContractDocID number(10) NOT NULL, 
  TransferActID number(10) NOT NULL, 
  DocumentID    number(10) NOT NULL, 
  PagesCount    number(10), 
  PRIMARY KEY (ContractDocID, 
  TransferActID, 
  DocumentID));
COMMENT ON TABLE Contract_TransferAct_Document IS 'Документы, преданные к договору согласно акту приёмки-передачи';
COMMENT ON COLUMN Contract_TransferAct_Document.ContractDocID IS 'Ссылка на договор';
COMMENT ON COLUMN Contract_TransferAct_Document.TransferActID IS 'Ссылка на акт приемки-передачи';
COMMENT ON COLUMN Contract_TransferAct_Document.DocumentID IS 'ссылка на документ';
COMMENT ON COLUMN Contract_TransferAct_Document.PagesCount IS 'Количество страниц';
CREATE TABLE StageResult (
  ID                       number(10) NOT NULL, 
  Name                     nvarchar2(255), 
  EconomicEfficiencyTypeID number(10) NOT NULL, 
  StageID                  number(10) NOT NULL, 
  NTPSubViewID             number(10) NOT NULL, 
  PRIMARY KEY (ID));
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
  PRIMARY KEY (ID));
COMMENT ON TABLE NTPSubView IS 'Подвид НТП';
COMMENT ON COLUMN NTPSubView.ID IS 'Идентификатор';
COMMENT ON COLUMN NTPSubView.Name IS 'Название подвида НТП';
COMMENT ON COLUMN NTPSubView.NTPViewID IS 'Ссылка на вид НТП';
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
CREATE TABLE EfficienceParameter_Type (
  EconomEfficiencyParameterID number(10) NOT NULL, 
  EconomEfficiencyTypeID      number(10) NOT NULL, 
  PRIMARY KEY (EconomEfficiencyParameterID, 
  EconomEfficiencyTypeID));
COMMENT ON TABLE EfficienceParameter_Type IS 'Парметры типа экономической эффективности (сс)';
COMMENT ON COLUMN EfficienceParameter_Type.EconomEfficiencyParameterID IS 'Ссылка на параметр экономической эффективности';
COMMENT ON COLUMN EfficienceParameter_Type.EconomEfficiencyTypeID IS 'Ссылка на тип экономической эффективности';
CREATE TABLE EfficParameter_StageResult (
  EconomEfficiencyParameterID number(10) NOT NULL, 
  StageResultID               number(10) NOT NULL, 
  Value                       number(19, 2), 
  PRIMARY KEY (EconomEfficiencyParameterID, 
  StageResultID));
COMMENT ON TABLE EfficParameter_StageResult IS 'Значения параметров экономической эффективности результата этапа (сс)';
COMMENT ON COLUMN EfficParameter_StageResult.EconomEfficiencyParameterID IS 'Ссылка на параметр экономической эфффективности';
COMMENT ON COLUMN EfficParameter_StageResult.StageResultID IS 'Ссылка на результат этапа';
COMMENT ON COLUMN EfficParameter_StageResult.Value IS 'Значение параметра';
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
  GeneralContractDocStageID number(10) NOT NULL, 
  SubContractDocStageID     number(10) NOT NULL, 
  PRIMARY KEY (GeneralContractDocStageID, 
  SubContractDocStageID));
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
INSERT INTO ContractType
  (ID, Name) 
VALUES 
  (1, 'НИОКР');
INSERT INTO ContractType
  (ID, Name) 
VALUES 
  (2, 'Газификация и газоснабжение регионов');
INSERT INTO ContractType
  (ID, Name) 
VALUES 
  (3, 'ПИР');
INSERT INTO ContractType
  (ID, Name) 
VALUES 
  (4, 'Экспертиза сметной документации');
INSERT INTO ContractType
  (ID, Name) 
VALUES 
  (5, 'Производство');
INSERT INTO ContractType
  (ID, Name) 
VALUES 
  (6, 'Прочие');
INSERT INTO Currency
  (ID, Name, Culture, CurrencyI, CurrencyR, CurrencyM, SmallI, SmallR, SmallM, HighSmallName, LowSmallName, Code) 
VALUES 
  (1, 'Рубль', 'ru-ru', 'рубль', 'рубля', 'рублей', 'копейка', 'копейки', 'копеек', 'руб.', 'коп.', 'RUB');
INSERT INTO Currency
  (ID, Name, Culture, CurrencyI, CurrencyR, CurrencyM, SmallI, SmallR, SmallM, HighSmallName, LowSmallName, Code) 
VALUES 
  (2, 'Доллар', 'en-us', 'доллар', 'доллара', 'долларов', 'цент', 'цента', 'центов', 'дол.', 'цент.', 'USD');
INSERT INTO NDSAlgorithm
  (ID, Name) 
VALUES 
  (1, 'в том числе НДС');
INSERT INTO NDSAlgorithm
  (ID, Name) 
VALUES 
  (2, 'кроме того НДС');
INSERT INTO NDSAlgorithm
  (ID, Name) 
VALUES 
  (3, 'без НДС');
INSERT INTO Contractor
  (ID, Name, ShortName, Zip, City, Street, Build, Block, Bank, ContractorTypeID, Appartment, INN, Account, BIK, KPP) 
VALUES 
  (1, 'НИЦ «Цеосит» ОИК СО РАН ', 'НИЦ «Цеосит» ОИК СО РАН ', null, 'Москва', 'Ленинградский проспект', '14', null, null, 1, null, null, null, null, null);
INSERT INTO Contractor
  (ID, Name, ShortName, Zip, City, Street, Build, Block, Bank, ContractorTypeID, Appartment, INN, Account, BIK, KPP) 
VALUES 
  (2, 'ЗАО фирма "НОЭМИ"', 'ЗАО фирма "НОЭМИ', null, 'Пермь', 'Ленина', '23', null, null, 1, null, null, null, null, null);
INSERT INTO Contractor
  (ID, Name, ShortName, Zip, City, Street, Build, Block, Bank, ContractorTypeID, Appartment, INN, Account, BIK, KPP) 
VALUES 
  (3, 'ООО КЗГО', 'ООО КЗГО', null, 'Ярославль', 'Комсомольская', '34', null, null, 1, null, null, null, null, null);
INSERT INTO Contractor
  (ID, Name, ShortName, Zip, City, Street, Build, Block, Bank, ContractorTypeID, Appartment, INN, Account, BIK, KPP) 
VALUES 
  (4, 'Государственное учреждение НПО «Тайфун»', 'Государственное учреждение НПО «Тайфун»', '116832', 'Москва', 'Губкина', '109а', null, null, 1, null, null, null, null, null);
INSERT INTO Contractor
  (ID, Name, ShortName, Zip, City, Street, Build, Block, Bank, ContractorTypeID, Appartment, INN, Account, BIK, KPP) 
VALUES 
  (5, 'ОАО   «ИнфоТеКС»', 'ОАО   «ИнфоТеКС»', '113623', 'Москва', 'Пушкина', '23/1', null, null, 1, null, null, null, null, null);
INSERT INTO Contractor
  (ID, Name, ShortName, Zip, City, Street, Build, Block, Bank, ContractorTypeID, Appartment, INN, Account, BIK, KPP) 
VALUES 
  (6, 'ОАО «Медицина для Вас»', 'ОАО «Медицина для Вас»', '169900', 'Воркута', 'Юбилейная', '56', null, null, 1, null, null, null, null, null);
INSERT INTO Contractor
  (ID, Name, ShortName, Zip, City, Street, Build, Block, Bank, ContractorTypeID, Appartment, INN, Account, BIK, KPP) 
VALUES 
  (7, 'ОАО "Газпром"', 'ОАО "Газпром"', null, 'Москва', 'Вокзальная', '7', null, null, 3, null, null, null, null, null);
INSERT INTO Contractor
  (ID, Name, ShortName, Zip, City, Street, Build, Block, Bank, ContractorTypeID, Appartment, INN, Account, BIK, KPP) 
VALUES 
  (8, 'ГОУ ВПО Ухтинский Государственный Технический Университет', 'ГОУ ВПО УГТУ', '169300', 'Ухта', 'Первомайская', '13', null, null, 1, null, null, null, null, null);
INSERT INTO Degree
  (ID, Name) 
VALUES 
  (1, 'кандидат наук');
INSERT INTO Degree
  (ID, Name) 
VALUES 
  (2, 'доктор наук');
INSERT INTO Person
  (ID, DegreeID, IsContractHeadAuthority, IsActSignAuthority, IsValid, FamilyName, FirstName, MiddleName, Sex, ContractorPositionID) 
VALUES 
  (2, null, 0, 0, 1, 'Рожков', 'Сергей', 'Андреевич', 1, 1);
INSERT INTO Person
  (ID, DegreeID, IsContractHeadAuthority, IsActSignAuthority, IsValid, FamilyName, FirstName, MiddleName, Sex, ContractorPositionID) 
VALUES 
  (3, 1, 0, 0, 1, 'Воробьева', 'Ирина', 'Львовна', 0, null);
INSERT INTO Person
  (ID, DegreeID, IsContractHeadAuthority, IsActSignAuthority, IsValid, FamilyName, FirstName, MiddleName, Sex, ContractorPositionID) 
VALUES 
  (4, null, 0, 0, 1, 'Козлов', 'Петр', 'Макарович', 1, null);
INSERT INTO Person
  (ID, DegreeID, IsContractHeadAuthority, IsActSignAuthority, IsValid, FamilyName, FirstName, MiddleName, Sex, ContractorPositionID) 
VALUES 
  (5, 2, 0, 0, 1, 'Урумян', 'Николай', 'Валерьевич', 1, null);
INSERT INTO NDS
  (ID, "Percent", Year) 
VALUES 
  (1, 18, 2004);
INSERT INTO ContractState
  (ID, Name) 
VALUES 
  (1, 'Не подписан');
INSERT INTO ContractState
  (ID, Name) 
VALUES 
  (2, 'Подписан');
INSERT INTO CurrencyMeasure
  (ID, Name, Factor) 
VALUES 
  (1, 'ед.', 1);
INSERT INTO CurrencyMeasure
  (ID, Name, Factor) 
VALUES 
  (2, 'тыс.', 1000);
INSERT INTO CurrencyMeasure
  (ID, Name, Factor) 
VALUES 
  (3, 'млн.', 1000000);
INSERT INTO ContractDoc
  (ID, Price, ContractTypeID, StartAt, EndsAt, AppliedAt, ApprovedAt, InternalNum, ContractorNum, CurrencyID, OriginContractID, NDSAlgorithmID, PrepaymentSum, PrepaymentPercent, PrepaymentNDSAlgorithmID, NDSID, ContractStateID, CurrencyMeasureID, ContractorID, IsProtectability, Subject, CurrencyRate, RateDate, Description, ContractorPersonID, IsActive, AuthorityID) 
VALUES 
  (1, 2000, 1, '01.01.2010', '31.12.2011', '27.01.2009', '27.01.2010', '1001-10-1', null, 1, null, 1, 600, 30, 1, 1, 1, 1, 7, null, 'Разработка и внедрение новых технологических регламентов «Газораспределительные системы. Система управления сетями газораспределения»', null, null, 'Договор имеет 3 субподрядных договора ID = 2,3,4 и 1 доп. соглашение ID=5', 5, 1, 1);
INSERT INTO ContractDoc
  (ID, Price, ContractTypeID, StartAt, EndsAt, AppliedAt, ApprovedAt, InternalNum, ContractorNum, CurrencyID, OriginContractID, NDSAlgorithmID, PrepaymentSum, PrepaymentPercent, PrepaymentNDSAlgorithmID, NDSID, ContractStateID, CurrencyMeasureID, ContractorID, IsProtectability, Subject, CurrencyRate, RateDate, Description, ContractorPersonID, IsActive, AuthorityID) 
VALUES 
  (2, 323, 1, '01.10.2010', '18.10.2010', '20.05.2009', '20.05.2009', '124-04-01', null, 1, null, 1, null, null, null, 1, 1, 1, 1, null, 'Разработка технологического регламента', null, null, 'Субподрядное соглашение к ID=1', 2, 1, 1);
INSERT INTO ContractDoc
  (ID, Price, ContractTypeID, StartAt, EndsAt, AppliedAt, ApprovedAt, InternalNum, ContractorNum, CurrencyID, OriginContractID, NDSAlgorithmID, PrepaymentSum, PrepaymentPercent, PrepaymentNDSAlgorithmID, NDSID, ContractStateID, CurrencyMeasureID, ContractorID, IsProtectability, Subject, CurrencyRate, RateDate, Description, ContractorPersonID, IsActive, AuthorityID) 
VALUES 
  (3, 500, 1, '10.01.2010', '27.12.2011', '15.03.2009', '15.03.2009', '300-06-1', null, 1, null, 1, null, null, null, 1, 1, 1, 2, null, 'Разработка нормативной документации для информационного сопровождения разведки и разработки газоконденсатных и нефтегазоконденсатных месторождений в области изучения газоконденсатной характеристики скважин и месторождений, планирования и мониторинга добычи полезных ископаемых', null, null, 'Субподрядное соглашение к ID=1', 3, 1, 1);
INSERT INTO ContractDoc
  (ID, Price, ContractTypeID, StartAt, EndsAt, AppliedAt, ApprovedAt, InternalNum, ContractorNum, CurrencyID, OriginContractID, NDSAlgorithmID, PrepaymentSum, PrepaymentPercent, PrepaymentNDSAlgorithmID, NDSID, ContractStateID, CurrencyMeasureID, ContractorID, IsProtectability, Subject, CurrencyRate, RateDate, Description, ContractorPersonID, IsActive, AuthorityID) 
VALUES 
  (4, 100, 1, '01.04.2011', '15.05.2011', '28.11.2009', '01.01.2010', '53-06-1', null, 1, null, 1, null, null, null, 1, 1, 1, 3, null, 'Проведение подготовительных работ', null, null, 'Субподрядное соглашение к ID=1', 4, 1, 1);
INSERT INTO ContractDoc
  (ID, Price, ContractTypeID, StartAt, EndsAt, AppliedAt, ApprovedAt, InternalNum, ContractorNum, CurrencyID, OriginContractID, NDSAlgorithmID, PrepaymentSum, PrepaymentPercent, PrepaymentNDSAlgorithmID, NDSID, ContractStateID, CurrencyMeasureID, ContractorID, IsProtectability, Subject, CurrencyRate, RateDate, Description, ContractorPersonID, IsActive, AuthorityID) 
VALUES 
  (5, 2000, 1, '01.01.2010', '31.12.2011', '06.05.2009', '20.05.2009', '1001-10-1/1', null, 1, 1, 1, 600, 30, 1, 1, 1, 1, 7, null, 'Разработка и внедрение новых технологических регламентов «Газораспределительные системы. Система управления сетями газораспределения»', null, null, 'Доп. соглашение к ID=1', 5, 1, 1);
INSERT INTO Prepayment
  (ContractDocID, Sum, "Percent", Year) 
VALUES 
  (1, 300, 15, 2009);
INSERT INTO Prepayment
  (ContractDocID, Sum, "Percent", Year) 
VALUES 
  (1, 300, 15, 2010);
INSERT INTO ContractHierarchy
  (GeneralContractDocID, SubContractDocID) 
VALUES 
  (1, 2);
INSERT INTO ContractHierarchy
  (GeneralContractDocID, SubContractDocID) 
VALUES 
  (1, 3);
INSERT INTO ContractHierarchy
  (GeneralContractDocID, SubContractDocID) 
VALUES 
  (1, 4);
INSERT INTO PrepaymentDocumentType
  (ID, Name) 
VALUES 
  (1, 'Счет');
INSERT INTO PrepaymentDocumentType
  (ID, Name) 
VALUES 
  (2, 'Счет-фактура');
INSERT INTO PrepaymentDocumentType
  (ID, Name) 
VALUES 
  (3, 'Накладная');
INSERT INTO TroublesRegistry
  (ID, Name, ShortName, ApprovedAt, OrderNum, ValidFrom, ValidTo) 
VALUES 
  (1, 'Перечень приоритетных научно-технических проблем ОАО "Газпром» на 2006-2010 годы', 'Перечень проблем 2006-2010г', '05.01.2006', '01-106', '01.01.2006', '01.01.2010');
INSERT INTO TroublesRegistry
  (ID, Name, ShortName, ApprovedAt, OrderNum, ValidFrom, ValidTo) 
VALUES 
  (2, 'Перечень приоритетных научно-технических проблем ОАО "Газпром» на 2002-2006 годы', 'Перечень проблем 2002-2006г', '10.01.2002', '01-342', '01.01.2002', '31.12.2005');
INSERT INTO Trouble
  (ID, Name, Num, TopTroubleID, TroubleRegistryID) 
VALUES 
  (1, '1', '1', null, 1);
INSERT INTO Trouble
  (ID, Name, Num, TopTroubleID, TroubleRegistryID) 
VALUES 
  (2, 'Совершенствование методов и моделей формирования перспективных планов и программ газовой промышленности России для устойчивого развития топливно-энергетического комплекса страны.', '1.1', 1, 1);
INSERT INTO Trouble
  (ID, Name, Num, TopTroubleID, TroubleRegistryID) 
VALUES 
  (3, '2', '2', null, 1);
INSERT INTO Trouble
  (ID, Name, Num, TopTroubleID, TroubleRegistryID) 
VALUES 
  (4, '3', '3', null, 1);
INSERT INTO Trouble
  (ID, Name, Num, TopTroubleID, TroubleRegistryID) 
VALUES 
  (5, 'Создание методов и технологий для повышения эффективности разработки и безопасной эксплуатации месторождений.', '3.2', 4, 1);
INSERT INTO Trouble
  (ID, Name, Num, TopTroubleID, TroubleRegistryID) 
VALUES 
  (6, 'Разработка методов, технических средств  и технологий освоения трудноизвлекаемых и нетрадиционных ресурсов газа в низконапорных коллекторах, газогидратных залежах и метана угольных бассейнов.', '3.4', 4, 1);
INSERT INTO Trouble
  (ID, Name, Num, TopTroubleID, TroubleRegistryID) 
VALUES 
  (7, '4', '4', null, 1);
INSERT INTO Trouble
  (ID, Name, Num, TopTroubleID, TroubleRegistryID) 
VALUES 
  (8, 'Создание современных методов и средств диспетчерского управления ЕСГ.', '4.4', 7, 1);
INSERT INTO Trouble
  (ID, Name, Num, TopTroubleID, TroubleRegistryID) 
VALUES 
  (9, 'Создание технологий и технических средств для строительства, реконструкции и эксплуатации трубопроводных систем с оптимальными параметрами транспорта газа и устойчивостью к воздействию естественных факторов и технологических нагрузок', '4.1', 7, 1);
INSERT INTO Trouble
  (ID, Name, Num, TopTroubleID, TroubleRegistryID) 
VALUES 
  (10, ' Развитие технологий и совершествование оборудования для обеспечения надежного функционирования ЕСГ, включая методы и средства диагностики и ремонта.', '4.2', 7, 1);
INSERT INTO Trouble
  (ID, Name, Num, TopTroubleID, TroubleRegistryID) 
VALUES 
  (11, '5', '5', null, 1);
INSERT INTO Trouble
  (ID, Name, Num, TopTroubleID, TroubleRegistryID) 
VALUES 
  (12, 'Совершенствование существующих и создание новых технологических процессов и технических средств глубокой переработки углеводородного сырья с целью производства и вывода на рынок новых видов продукции и услуг.', '5.2', 11, 1);
INSERT INTO Trouble
  (ID, Name, Num, TopTroubleID, TroubleRegistryID) 
VALUES 
  (13, 'Развитие системы обеспечения эффективного использования Обществом топливно-энергетических ресурсов и стимулирования газо- и энергосбережения потребителям ОАО "Газпром"', '5.3', 11, 1);
INSERT INTO Trouble
  (ID, Name, Num, TopTroubleID, TroubleRegistryID) 
VALUES 
  (14, '6', '6', null, 1);
INSERT INTO Trouble
  (ID, Name, Num, TopTroubleID, TroubleRegistryID) 
VALUES 
  (15, '7', '7', null, 1);
INSERT INTO Trouble
  (ID, Name, Num, TopTroubleID, TroubleRegistryID) 
VALUES 
  (16, 'Совершенствование методов и моделей формирования инвестиционных программ и управления проектами.', '7.4', 15, 1);
INSERT INTO Trouble
  (ID, Name, Num, TopTroubleID, TroubleRegistryID) 
VALUES 
  (17, 'Разработка эффективной информационной, ценовой и налоговой политики, а также механизмов ее реализации в целях повышения рыночной капитализации и финансовой устойчивости Общества.', '7.5', 15, 1);
INSERT INTO Trouble
  (ID, Name, Num, TopTroubleID, TroubleRegistryID) 
VALUES 
  (18, '8', '8', null, 1);
INSERT INTO Trouble
  (ID, Name, Num, TopTroubleID, TroubleRegistryID) 
VALUES 
  (19, ' Развитие системы управления здравоохранением ОАО «Газпром»', '8.3', 18, 1);
INSERT INTO ContractorType
  (ID, Name) 
VALUES 
  (1, 'Другие организации');
INSERT INTO ContractorType
  (ID, Name) 
VALUES 
  (2, 'Дочерние организации');
INSERT INTO ContractorType
  (ID, Name) 
VALUES 
  (3, 'ОАО "Газпром"');
INSERT INTO Employee
  (ID, ManagerID, RoleID, Familyname, FirstName, Middlename, Sex) 
VALUES 
  (1, null, 1, 'Лосев', 'Леонид', 'Львович', 1);
INSERT INTO EnterpriseAuthority
  (ID, Num, ValidFrom, ValidTo, IsValid, EmployeeID, AuthorityID) 
VALUES 
  (1, '12-23', '01.01.2010', '01.01.2011', 1, 1, 1);
INSERT INTO ContractTrouble
  (TroubleID, ContractDocID, Description) 
VALUES 
  (6, 1, null);
INSERT INTO ContractTrouble
  (TroubleID, ContractDocID, Description) 
VALUES 
  (16, 1, null);
INSERT INTO FunctionalCustomerType
  (ID, Name) 
VALUES 
  (1, 'Тип функционального заказчика 1');
INSERT INTO FunctionalCustomer
  (ID, Name, ContractorID, ParentFunctionalCustomerID, FunctionalCustomerTypeID) 
VALUES 
  (1, 'Управление инновационного развития', 7, null, 1);
INSERT INTO FunctionalCustomer
  (ID, Name, ContractorID, ParentFunctionalCustomerID, FunctionalCustomerTypeID) 
VALUES 
  (2, 'Департамент по управлению персоналом', 7, null, 1);
INSERT INTO FunctionalCustomer
  (ID, Name, ContractorID, ParentFunctionalCustomerID, FunctionalCustomerTypeID) 
VALUES 
  (3, 'Департамент по добыче газа, газового конденсата, нефти', 7, null, 1);
INSERT INTO FunctionalCustomer
  (ID, Name, ContractorID, ParentFunctionalCustomerID, FunctionalCustomerTypeID) 
VALUES 
  (4, 'Департамент по транспортировке, подземному хранению и использованию газа', 7, null, 1);
INSERT INTO FunctionalCustomer
  (ID, Name, ContractorID, ParentFunctionalCustomerID, FunctionalCustomerTypeID) 
VALUES 
  (5, 'Департамент стратегического развития', 7, null, 1);
INSERT INTO Schedule
  (ID, CurrencyMeasureID, WorkTypeID) 
VALUES 
  (1, 1, 2);
INSERT INTO Schedule
  (ID, CurrencyMeasureID, WorkTypeID) 
VALUES 
  (2, 1, 1);
INSERT INTO ScheduleContract
  (ContractDocID, AppNum, ScheduleID) 
VALUES 
  (1, 3, 1);
INSERT INTO Region
  (ID, Name) 
VALUES 
  (1, 'Москва и Московская облать');
INSERT INTO ActType
  (ID, ContractorID, TypeName, IsActive) 
VALUES 
  (1, 7, 'Акт закрытия', 1);
INSERT INTO Act
  (ID, Num, "Date", ActTypeID, NDSID, RegionID, TotalSum, SumForTransfer, Status, EnterpriceAuthorityID, CurrencyRate, RateDate) 
VALUES 
  (1, '143-23', '01.01.2011', 1, 1, 1, 200, 200, 1, 1, null, null);
INSERT INTO WorkType
  (ID, Name) 
VALUES 
  (1, 'Разработка документации');
INSERT INTO WorkType
  (ID, Name) 
VALUES 
  (2, 'Разработка программного комплекса');
INSERT INTO Role
  (ID, Name) 
VALUES 
  (1, 'Руководитель направления');
INSERT INTO Role
  (ID, Name) 
VALUES 
  (2, 'Ответственный исполнитель');
INSERT INTO Role
  (ID, Name) 
VALUES 
  (3, 'Куратор от договорного отдела');
INSERT INTO Authority
  (ID, Name) 
VALUES 
  (1, 'Основание для Промгаза 1');
INSERT INTO Authority
  (ID, Name) 
VALUES 
  (2, 'Основание для исполнителя 1');
INSERT INTO Stage
  (ID, Num, Subject, StartsAt, EndsAt, Price, NdsAlgorithmID, ParentID, ActID, ScheduleID) 
VALUES 
  (1, '1', 'Анализ CEN/TS 15399:2007 и CEN/TS 15173:2006 и действующих нормативных документов РФ в области газораспределения. Определение структуры и разделов проекта стандарта. Обсуждение итогов работы  рабочей группой ', '05.07.2010 ', '18.10.2010', 200, 1, null, 1, 1);
INSERT INTO Stage
  (ID, Num, Subject, StartsAt, EndsAt, Price, NdsAlgorithmID, ParentID, ActID, ScheduleID) 
VALUES 
  (2, '2', 'Разработка первой редакции проекта стандарта «Газораспределительные системы. Система управления сетями газораспределения» Обсуждение первой редакции проекта стандарта рабочей группой. ', '18.10.2010', '27.12.2010', 600, 1, null, 1, 1);
INSERT INTO Stage
  (ID, Num, Subject, StartsAt, EndsAt, Price, NdsAlgorithmID, ParentID, ActID, ScheduleID) 
VALUES 
  (3, '3', 'Оформление уведомления о разработке стандарта и его публикация. Публикация проекта стандарта в информационной системе общего пользования в электронно-цифровой форме для проведения публичного обсуждения. Сбор замечаний и предложений, полученных при публичном обсуждении проекта стандарта. ', '10.01.2011', '15.05.2011', 200, 1, null, null, 1);
INSERT INTO Stage
  (ID, Num, Subject, StartsAt, EndsAt, Price, NdsAlgorithmID, ParentID, ActID, ScheduleID) 
VALUES 
  (4, '4', 'Внесение изменений и дополнений в проект и подготовка окончательной редакции стандарта. Обсуждение окончательной редакции проекта стандарта рабочей группой. ', '01.04.2011', '30.09.2011', 600, 1, null, null, 1);
INSERT INTO Stage
  (ID, Num, Subject, StartsAt, EndsAt, Price, NdsAlgorithmID, ParentID, ActID, ScheduleID) 
VALUES 
  (5, '5', 'Передача в ПК 4 проекта стандарта «Газораспределительные системы. Система управления сетями газораспределения» для организации экспертизы секретариатом ТК 23.', '01.10.2011', '25.12.2011', 400, 1, null, null, 1);
INSERT INTO Stage
  (ID, Num, Subject, StartsAt, EndsAt, Price, NdsAlgorithmID, ParentID, ActID, ScheduleID) 
VALUES 
  (6, '1.1', 'Анализ CEN/TS 15399:2007 и CEN/TS 15173:2006 и действующих нормативных документов РФ в области газораспределения. ', '05.07.2010', '01.09.2010', 90, 1, 9, null, 2);
INSERT INTO Stage
  (ID, Num, Subject, StartsAt, EndsAt, Price, NdsAlgorithmID, ParentID, ActID, ScheduleID) 
VALUES 
  (7, '1.2', 'Определение структуры и разделов проекта стандарта.', '01.09.2010', '01.10.2010', 10, 1, 9, null, 2);
INSERT INTO Stage
  (ID, Num, Subject, StartsAt, EndsAt, Price, NdsAlgorithmID, ParentID, ActID, ScheduleID) 
VALUES 
  (8, '1.3', 'Обсуждение итогов работы  рабочей группой.', '01.10.2010', '18.10.2010', 10, 1, null, null, 2);
INSERT INTO Stage
  (ID, Num, Subject, StartsAt, EndsAt, Price, NdsAlgorithmID, ParentID, ActID, ScheduleID) 
VALUES 
  (9, '1', 'Анализ CEN/TS 15399:2007 и CEN/TS 15173:2006 и действующих нормативных документов РФ ', '05.07.2010', '18.10.2010', 110, 1, null, null, 2);
INSERT INTO Disposal
  (ID, Num, ApprovedDate, ContractdocID) 
VALUES 
  (1, '103-15', '01.03.2009', 1);
INSERT INTO Disposal
  (ID, Num, ApprovedDate, ContractdocID) 
VALUES 
  (2, '124-16', '15.05.2009', 1);
INSERT INTO Disposal
  (ID, Num, ApprovedDate, ContractdocID) 
VALUES 
  (3, '135-9', '15.03.2009', 2);
INSERT INTO Disposal
  (ID, Num, ApprovedDate, ContractdocID) 
VALUES 
  (4, '165-65-9', '05.01.2010', 3);
INSERT INTO Disposal
  (ID, Num, ApprovedDate, ContractdocID) 
VALUES 
  (5, '32-43', '01.01.2010', 4);
INSERT INTO Responsible
  (DisposalID, EmployeeID) 
VALUES 
  (5, 1);
INSERT INTO Document
  (ID, Name) 
VALUES 
  (1, 'Договор');
INSERT INTO Document
  (ID, Name) 
VALUES 
  (3, 'Календарный план');
INSERT INTO Document
  (ID, Name) 
VALUES 
  (4, 'Протокол цены');
INSERT INTO Document
  (ID, Name) 
VALUES 
  (5, 'Лист согласования');
INSERT INTO Document
  (ID, Name) 
VALUES 
  (6, 'Протокол разногласий');
INSERT INTO Document
  (ID, Name) 
VALUES 
  (7, 'Протокол согласования разногласий');
INSERT INTO Document
  (ID, Name) 
VALUES 
  (8, 'Письма');
INSERT INTO NTPView
  (ID, Name) 
VALUES 
  (1, 'Концепция');
INSERT INTO NTPView
  (ID, Name) 
VALUES 
  (2, 'Нормативно-методическая документация');
INSERT INTO NTPSubView
  (ID, Name, NTPViewID) 
VALUES 
  (1, 'Концепция', 1);
INSERT INTO NTPSubView
  (ID, Name, NTPViewID) 
VALUES 
  (2, 'СТО', 2);
INSERT INTO NTPSubView
  (ID, Name, NTPViewID) 
VALUES 
  (3, 'Методические рекомендации', 2);
INSERT INTO ContractorPosition
  (ID, ContractorID, PositionID) 
VALUES 
  (1, 1, 1);
INSERT INTO ContractorPosition
  (ID, ContractorID, PositionID) 
VALUES 
  (2, 2, 1);
INSERT INTO ContractorPosition
  (ID, ContractorID, PositionID) 
VALUES 
  (3, 3, 2);
INSERT INTO ContractorPosition
  (ID, ContractorID, PositionID) 
VALUES 
  (4, 4, 1);
INSERT INTO ContractorAuthority
  (ID, AuthorityID, NumDocument, ValidFrom, ValidTo, IsValid, ContractorID) 
VALUES 
  (1, 2, '13-65', '01.01.2010', '01.01.2011', 1, 7);
INSERT INTO Position
  (ID, Name) 
VALUES 
  (1, 'Директор');
INSERT INTO Position
  (ID, Name) 
VALUES 
  (2, 'Начальник отдела поставок');
INSERT INTO SubGeneralHierarchi
  (GeneralContractDocStageID, SubContractDocStageID) 
VALUES 
  (1, 6);
INSERT INTO SubGeneralHierarchi
  (GeneralContractDocStageID, SubContractDocStageID) 
VALUES 
  (1, 7);
INSERT INTO SubGeneralHierarchi
  (GeneralContractDocStageID, SubContractDocStageID) 
VALUES 
  (1, 8);
INSERT INTO SubGeneralHierarchi
  (GeneralContractDocStageID, SubContractDocStageID) 
VALUES 
  (1, 9);
INSERT INTO UDMetadata
  (SchemeRelease, SchemeBuild, SchemeTimestamp) 
VALUES 
  (1, 0, '');
ALTER TABLE ContractDoc ADD CONSTRAINT FKContractDo331897 FOREIGN KEY (ContractTypeID) REFERENCES ContractType (ID) ON DELETE Cascade;
ALTER TABLE ContractDoc ADD CONSTRAINT FKContractDo465214 FOREIGN KEY (CurrencyID) REFERENCES Currency (ID) ON DELETE Cascade;
ALTER TABLE ContractDoc ADD CONSTRAINT FKContractDo919377 FOREIGN KEY (OriginContractID) REFERENCES ContractDoc (ID) ON DELETE Cascade;
ALTER TABLE ContractDoc ADD CONSTRAINT FKContractDo12114 FOREIGN KEY (NDSAlgorithmID) REFERENCES NDSAlgorithm (ID) ON DELETE Cascade;
ALTER TABLE Prepayment ADD CONSTRAINT FKPrepayment822569 FOREIGN KEY (ContractDocID) REFERENCES ContractDoc (ID) ON DELETE Cascade;
ALTER TABLE ContractDoc ADD CONSTRAINT FKContractDo459574 FOREIGN KEY (PrepaymentNDSAlgorithmID) REFERENCES NDSAlgorithm (ID);
ALTER TABLE Person ADD CONSTRAINT FKPerson162299 FOREIGN KEY (DegreeID) REFERENCES Degree (ID) ON DELETE Cascade;
ALTER TABLE ContractDoc ADD CONSTRAINT FKContractDo371687 FOREIGN KEY (NDSID) REFERENCES NDS (ID) ON DELETE Cascade;
ALTER TABLE ContractDoc ADD CONSTRAINT FKContractDo917205 FOREIGN KEY (ContractStateID) REFERENCES ContractState (ID) ON DELETE Cascade;
ALTER TABLE ContractHierarchy ADD CONSTRAINT FKContractHi836133 FOREIGN KEY (GeneralContractDocID) REFERENCES ContractDoc (ID) ON DELETE Cascade;
ALTER TABLE ContractHierarchy ADD CONSTRAINT FKContractHi748403 FOREIGN KEY (SubContractDocID) REFERENCES ContractDoc (ID) ON DELETE Cascade;
ALTER TABLE ContractDoc ADD CONSTRAINT FKContractDo561570 FOREIGN KEY (CurrencyMeasureID) REFERENCES CurrencyMeasure (ID);
ALTER TABLE PaymentDocument ADD CONSTRAINT FKPaymentDoc430792 FOREIGN KEY (CurrencyMeasureID) REFERENCES CurrencyMeasure (ID);
ALTER TABLE PaymentDocument ADD CONSTRAINT FKPaymentDoc369604 FOREIGN KEY (PrepaymentDocumentTypeID) REFERENCES PrepaymentDocumentType (ID);
ALTER TABLE ContractPayment ADD CONSTRAINT FKContractPa282609 FOREIGN KEY (PaymentDocumentID) REFERENCES PaymentDocument (ID);
ALTER TABLE ContractPayment ADD CONSTRAINT FKContractPa457777 FOREIGN KEY (ContractDocID) REFERENCES ContractDoc (ID);
ALTER TABLE Trouble ADD CONSTRAINT FKTrouble646380 FOREIGN KEY (TopTroubleID) REFERENCES Trouble (ID);
ALTER TABLE Trouble ADD CONSTRAINT FKTrouble472627 FOREIGN KEY (TroubleRegistryID) REFERENCES TroublesRegistry (ID);
ALTER TABLE ContractTrouble ADD CONSTRAINT FKContractTr217582 FOREIGN KEY (TroubleID) REFERENCES Trouble (ID) ON DELETE Cascade;
ALTER TABLE ContractTrouble ADD CONSTRAINT FKContractTr714891 FOREIGN KEY (ContractDocID) REFERENCES ContractDoc (ID) ON DELETE Cascade;
ALTER TABLE AdditionContractorPropertiy ADD CONSTRAINT FKAdditionCo943431 FOREIGN KEY (PropertyID) REFERENCES Property (ID) ON DELETE Cascade;
ALTER TABLE AdditionContractorPropertiy ADD CONSTRAINT FKAdditionCo625361 FOREIGN KEY (ContractorID) REFERENCES Contractor (ID) ON DELETE Cascade;
ALTER TABLE FunctionalCustomer ADD CONSTRAINT FKFunctional561442 FOREIGN KEY (ContractorID) REFERENCES Contractor (ID) ON DELETE Cascade;
ALTER TABLE FunctionalCustomer ADD CONSTRAINT FKFunctional131682 FOREIGN KEY (ParentFunctionalCustomerID) REFERENCES FunctionalCustomer (ID) ON DELETE Cascade;
ALTER TABLE FunctionalCustomer ADD CONSTRAINT FKFunctional524086 FOREIGN KEY (FunctionalCustomerTypeID) REFERENCES FunctionalCustomerType (ID) ON DELETE Cascade;
ALTER TABLE FunctionalCustomerContract ADD CONSTRAINT FKFunctional451088 FOREIGN KEY (FunctionalCustomerID) REFERENCES FunctionalCustomer (ID) ON DELETE Cascade;
ALTER TABLE FunctionalCustomerContract ADD CONSTRAINT FKFunctional603255 FOREIGN KEY (ContractDocID) REFERENCES ContractDoc (ID) ON DELETE Cascade;
ALTER TABLE Contractor ADD CONSTRAINT FKContractor394676 FOREIGN KEY (ContractorTypeID) REFERENCES ContractorType (ID) ON DELETE Cascade;
ALTER TABLE ContractDoc ADD CONSTRAINT FKContractDo906594 FOREIGN KEY (ContractorID) REFERENCES Contractor (ID) ON DELETE Cascade;
ALTER TABLE ScheduleContract ADD CONSTRAINT FKScheduleCo823189 FOREIGN KEY (ContractDocID) REFERENCES ContractDoc (ID);
ALTER TABLE Stage ADD CONSTRAINT FKStage472417 FOREIGN KEY (NdsAlgorithmID) REFERENCES NDSAlgorithm (ID);
ALTER TABLE Stage ADD CONSTRAINT FKStage219572 FOREIGN KEY (ParentID) REFERENCES Stage (ID);
ALTER TABLE ClosedStageRelation ADD CONSTRAINT FKClosedStag717175 FOREIGN KEY (StageID) REFERENCES Stage (ID);
ALTER TABLE ClosedStageRelation ADD CONSTRAINT FKClosedStag104726 FOREIGN KEY (ClosedStageID) REFERENCES Stage (ID);
ALTER TABLE FuncCustomerPerson ADD CONSTRAINT FKFuncCustom534761 FOREIGN KEY (FuncCustomerID) REFERENCES FunctionalCustomer (ID) ON DELETE Cascade;
ALTER TABLE FuncCustomerPerson ADD CONSTRAINT FKFuncCustom258383 FOREIGN KEY (PersonID) REFERENCES Person (ID) ON DELETE Cascade;
ALTER TABLE Act ADD CONSTRAINT FKAct413802 FOREIGN KEY (RegionID) REFERENCES Region (ID) ON DELETE Cascade;
ALTER TABLE Act ADD CONSTRAINT FKAct287724 FOREIGN KEY (NDSID) REFERENCES NDS (ID) ON DELETE Cascade;
ALTER TABLE ActType ADD CONSTRAINT FKActType734662 FOREIGN KEY (ContractorID) REFERENCES Contractor (ID) ON DELETE Cascade;
ALTER TABLE Schedule ADD CONSTRAINT FKSchedule996822 FOREIGN KEY (WorkTypeID) REFERENCES WorkType (ID);
ALTER TABLE Schedule ADD CONSTRAINT FKSchedule245187 FOREIGN KEY (CurrencyMeasureID) REFERENCES CurrencyMeasure (ID);
ALTER TABLE Responsible ADD CONSTRAINT FKResponsibl163229 FOREIGN KEY (DisposalID) REFERENCES Disposal (ID);
ALTER TABLE Responsible ADD CONSTRAINT FKResponsibl778702 FOREIGN KEY (EmployeeID) REFERENCES Employee (ID) ON DELETE Cascade;
ALTER TABLE Employee ADD CONSTRAINT FKEmployee471769 FOREIGN KEY (ManagerID) REFERENCES Employee (ID);
ALTER TABLE Employee ADD CONSTRAINT FKEmployee880788 FOREIGN KEY (RoleID) REFERENCES Role (ID) ON DELETE Cascade;
ALTER TABLE EnterpriseAuthority ADD CONSTRAINT FKEnterprise947996 FOREIGN KEY (EmployeeID) REFERENCES Employee (ID);
ALTER TABLE EnterpriseAuthority ADD CONSTRAINT FKEnterprise312223 FOREIGN KEY (AuthorityID) REFERENCES Authority (ID) ON DELETE Cascade;
ALTER TABLE SightFuncPersonScheme ADD CONSTRAINT FKSightFuncP349042 FOREIGN KEY (FuncPersonID) REFERENCES FuncCustomerPerson (ID) ON DELETE Cascade;
ALTER TABLE SightFuncPerson ADD CONSTRAINT FKSightFuncP777798 FOREIGN KEY (SightFuncPersonSchID) REFERENCES SightFuncPersonScheme (ID) ON DELETE Cascade;
ALTER TABLE SightFuncPerson ADD CONSTRAINT FKSightFuncP679819 FOREIGN KEY (ActID) REFERENCES Act (ID) ON DELETE Cascade;
ALTER TABLE Act_PaymentDocument ADD CONSTRAINT FKAct_Paymen682899 FOREIGN KEY (ActID) REFERENCES Act (ID) ON DELETE Cascade;
ALTER TABLE Act_PaymentDocument ADD CONSTRAINT FKAct_Paymen800041 FOREIGN KEY (PaymentDocumentID) REFERENCES PaymentDocument (ID);
ALTER TABLE TransferAct ADD CONSTRAINT FKTransferAc108083 FOREIGN KEY (TransferActTypeID) REFERENCES TransferActType (ID) ON DELETE Cascade;
ALTER TABLE TransferAct_Act_Document ADD CONSTRAINT FKTransferAc139783 FOREIGN KEY (TransferActID) REFERENCES TransferAct (ID) ON DELETE Cascade;
ALTER TABLE TransferAct_Act_Document ADD CONSTRAINT FKTransferAc902857 FOREIGN KEY (DocumentID) REFERENCES Document (ID) ON DELETE Cascade;
ALTER TABLE TransferAct_Act_Document ADD CONSTRAINT FKTransferAc831105 FOREIGN KEY (ActID) REFERENCES Act (ID) ON DELETE Cascade;
ALTER TABLE TransferActType_Document ADD CONSTRAINT FKTransferAc807325 FOREIGN KEY (TransferActTypeID) REFERENCES TransferActType (ID) ON DELETE Cascade;
ALTER TABLE TransferActType_Document ADD CONSTRAINT FKTransferAc236332 FOREIGN KEY (DocumentID) REFERENCES Document (ID) ON DELETE Cascade;
ALTER TABLE Contract_TransferAct_Document ADD CONSTRAINT FKContract_T770060 FOREIGN KEY (ContractDocID) REFERENCES ContractDoc (ID) ON DELETE Cascade;
ALTER TABLE Contract_TransferAct_Document ADD CONSTRAINT FKContract_T827000 FOREIGN KEY (TransferActID) REFERENCES TransferAct (ID) ON DELETE Cascade;
ALTER TABLE Contract_TransferAct_Document ADD CONSTRAINT FKContract_T755950 FOREIGN KEY (DocumentID) REFERENCES Document (ID) ON DELETE Cascade;
ALTER TABLE StageResult ADD CONSTRAINT FKStageResul876631 FOREIGN KEY (StageID) REFERENCES Stage (ID);
ALTER TABLE NTPSubView ADD CONSTRAINT FKNTPSubView475996 FOREIGN KEY (NTPViewID) REFERENCES NTPView (ID);
ALTER TABLE StageResult ADD CONSTRAINT FKStageResul865700 FOREIGN KEY (NTPSubViewID) REFERENCES NTPSubView (ID);
ALTER TABLE StageResult ADD CONSTRAINT FKStageResul506505 FOREIGN KEY (EconomicEfficiencyTypeID) REFERENCES EconomEfficiencyType (ID);
ALTER TABLE EfficienceParameter_Type ADD CONSTRAINT FKEfficience633486 FOREIGN KEY (EconomEfficiencyParameterID) REFERENCES EconomEfficiencyParameter (ID);
ALTER TABLE EfficienceParameter_Type ADD CONSTRAINT FKEfficience651243 FOREIGN KEY (EconomEfficiencyTypeID) REFERENCES EconomEfficiencyType (ID);
ALTER TABLE EfficParameter_StageResult ADD CONSTRAINT FKEfficParam916874 FOREIGN KEY (EconomEfficiencyParameterID) REFERENCES EconomEfficiencyParameter (ID);
ALTER TABLE EfficParameter_StageResult ADD CONSTRAINT FKEfficParam575458 FOREIGN KEY (StageResultID) REFERENCES StageResult (ID);
ALTER TABLE ContractorPosition ADD CONSTRAINT FKContractor346407 FOREIGN KEY (ContractorID) REFERENCES Contractor (ID) ON DELETE Cascade;
ALTER TABLE Person ADD CONSTRAINT FKPerson487057 FOREIGN KEY (ContractorPositionID) REFERENCES ContractorPosition (ID) ON DELETE Cascade;
ALTER TABLE Stage ADD CONSTRAINT FKStage364615 FOREIGN KEY (ActID) REFERENCES Act (ID) ON DELETE Cascade;
ALTER TABLE Act ADD CONSTRAINT FKAct228252 FOREIGN KEY (EnterpriceAuthorityID) REFERENCES EnterpriseAuthority (ID) ON DELETE Cascade;
ALTER TABLE ContractorAuthority ADD CONSTRAINT FKContractor597586 FOREIGN KEY (ContractorID) REFERENCES Contractor (ID) ON DELETE Cascade;
ALTER TABLE ContractorAuthority ADD CONSTRAINT FKContractor191604 FOREIGN KEY (AuthorityID) REFERENCES Authority (ID) ON DELETE Cascade;
ALTER TABLE ContractorPosition ADD CONSTRAINT FKContractor858767 FOREIGN KEY (PositionID) REFERENCES Position (ID);
ALTER TABLE ContractDoc ADD CONSTRAINT FKContractDo959699 FOREIGN KEY (ContractorPersonID) REFERENCES Person (ID);
ALTER TABLE SubGeneralHierarchi ADD CONSTRAINT FKSubGeneral189443 FOREIGN KEY (GeneralContractDocStageID) REFERENCES Stage (ID);
ALTER TABLE SubGeneralHierarchi ADD CONSTRAINT FKSubGeneral651218 FOREIGN KEY (SubContractDocStageID) REFERENCES Stage (ID);
ALTER TABLE Act ADD CONSTRAINT FKAct654720 FOREIGN KEY (ActTypeID) REFERENCES ActType (ID);
ALTER TABLE ScheduleContract ADD CONSTRAINT FKScheduleCo47579 FOREIGN KEY (ScheduleID) REFERENCES Schedule (ID);
ALTER TABLE Stage ADD CONSTRAINT FKStage337267 FOREIGN KEY (ScheduleID) REFERENCES Schedule (ID);
ALTER TABLE Disposal ADD CONSTRAINT FKDisposal292972 FOREIGN KEY (ContractdocID) REFERENCES ContractDoc (ID);
ALTER TABLE ContractDoc ADD CONSTRAINT FKContractDo108170 FOREIGN KEY (AuthorityID) REFERENCES EnterpriseAuthority (ID);
CREATE OR REPLACE PROCEDURE InsertNextSchemeVersion
IS
BEGIN
    declare Mj number;
    Mn number;
    c number;
BEGIN
  select count(*) into c from udmetadata;
  if c > 0 then
    select SchemeRelease, SchemeBuild into Mj, Mn from UDMetadata where rownum=1  order by desc SchemeRelease, SchemeBuild;
    Mn:=Mn+1;
  else
    Mj:=1;
    Mn:=0;
  end if;
  INSERT INTO UDMetadata (Schemerelease,  Schemebuild, Schemetimestamp) VALUES (Mj, Mn, sysdate);
END;
END InsertNextSchemeVersion;

