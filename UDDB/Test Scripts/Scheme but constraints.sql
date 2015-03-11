/*
������� ����� ��������� 1.32
������ ������������ 04:40:45 PM 04/11/2011
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
COMMENT ON TABLE ContractDoc IS '������� (�����������/������������ � ���. ����������)';
COMMENT ON COLUMN ContractDoc.ID IS '������������� ��������';
COMMENT ON COLUMN ContractDoc.Price IS '���� �������� � �������� ������';
COMMENT ON COLUMN ContractDoc.ContractTypeID IS '��� �������� (����. �����)';
COMMENT ON COLUMN ContractDoc.StartAt IS '���� ������ ����� �� ��������';
COMMENT ON COLUMN ContractDoc.EndsAt IS '���� ��������� ����� �� ��������';
COMMENT ON COLUMN ContractDoc.AppliedAt IS '���� �������� ��������';
COMMENT ON COLUMN ContractDoc.ApprovedAt IS '���� ����������� ��������';
COMMENT ON COLUMN ContractDoc.InternalNum IS '����� ��������, ����������� ����������';
COMMENT ON COLUMN ContractDoc.ContractorNum IS '����� ��������, ����������� ������������';
COMMENT ON COLUMN ContractDoc.CurrencyID IS '������ �� ������ ��������';
COMMENT ON COLUMN ContractDoc.OriginContractID IS '������ �� ������������ ������� (��� ���. ����������)';
COMMENT ON COLUMN ContractDoc.NDSAlgorithmID IS '������ �� �������� ������� ���';
COMMENT ON COLUMN ContractDoc.PrepaymentSum IS '����� ������ �� �������� (����� ����� �� ���� �����, ����� �� ����� ����������� � ������� Prepaiment)';
COMMENT ON COLUMN ContractDoc.PrepaymentPercent IS '������� ������ �� ����� �������� [0..100](����� �� ���� �����, ����� �� ����� ����������� � ������� Prepaiment';
COMMENT ON COLUMN ContractDoc.PrepaymentNDSAlgorithmID IS '������ �� �������� ������� ��� ��� ������';
COMMENT ON COLUMN ContractDoc.NDSID IS '������ �� ������ ��� ��������';
COMMENT ON COLUMN ContractDoc.ContractStateID IS '������ �� ��������� ��������';
COMMENT ON COLUMN ContractDoc.CurrencyMeasureID IS '������ �� ������� ��������� ���� �������� (����. � ���. ���.)';
COMMENT ON COLUMN ContractDoc.ContractorID IS '������ �� ����������� ����������� ��������';
COMMENT ON COLUMN ContractDoc.IsProtectability IS '������� ����������������� ��������';
COMMENT ON COLUMN ContractDoc.Subject IS '���� ��������';
COMMENT ON COLUMN ContractDoc.CurrencyRate IS '���� ������ �� �������� �� ���� RateDate';
COMMENT ON COLUMN ContractDoc.RateDate IS '���� ��� ������� ������ ���� ������ ��������. ��� ������� ��������� � ����� ���������� ��������';
COMMENT ON COLUMN ContractDoc.Description IS '�������������� �������� � ��������';
COMMENT ON COLUMN ContractDoc.ContractorPersonID IS '������ �� ������������ ���� �� ����������� �����������';
COMMENT ON COLUMN ContractDoc.AuthorityID IS '������ �� ������������ �� �����������';
COMMENT ON COLUMN ContractDoc.PrepaymentPrecentType IS '������ ����� ������ �� % ������: false - �� ����� �������� ��� ���, true - �� ����� �������� � ���';
COMMENT ON COLUMN ContractDoc.Deleted IS '������� ����, ��� ������� ����� �������������';
COMMENT ON COLUMN ContractDoc.AgreementNum IS '����� ���������� � ��������. ������������ ������ � �� � ������, ���� ������������� ����������� ����� �� �� ��������';
CREATE TABLE ContractType (
  ID          number(10) NOT NULL, 
  Name        nvarchar2(200) NOT NULL UNIQUE, 
  ReportOrder number(3), 
  PRIMARY KEY (ID));
COMMENT ON TABLE ContractType IS '��� �������� (��)';
COMMENT ON COLUMN ContractType.ID IS '�������������';
COMMENT ON COLUMN ContractType.Name IS '�������� (����. �����)';
COMMENT ON COLUMN ContractType.ReportOrder IS '���������� ������� ���������� ������ � �������';
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
COMMENT ON TABLE Currency IS '������ (��)';
COMMENT ON COLUMN Currency.ID IS '�������������';
COMMENT ON COLUMN Currency.Name IS '�������� (����. �����)';
COMMENT ON COLUMN Currency.Culture IS '�������� (������������ � ���������� ��� ���������� ����� ��������) (����. ru-ru)';
COMMENT ON COLUMN Currency.CurrencyI IS '��������� ������ � ������������ ������ ������������ ����� (����. �����)';
COMMENT ON COLUMN Currency.CurrencyR IS '��������� ������ � ����������� ������ ������������ ����� (����. �����)';
COMMENT ON COLUMN Currency.CurrencyM IS '��������� ������ � ����������� ������ ������������� ����� (����. ������)';
COMMENT ON COLUMN Currency.SmallI IS '��������� ����������� ������� ������ � ������������ ������ ������������ ����� (����. �������)';
COMMENT ON COLUMN Currency.SmallR IS '��������� ����������� ������� ������ � ����������� ������ ������������ ����� (����. �������)';
COMMENT ON COLUMN Currency.SmallM IS '��������� ����������� ������� ������ � ����������� ������ ������������ ����� (����. ������)';
COMMENT ON COLUMN Currency.HighSmallName IS '������������ ������� ������ (����. ����.)';
COMMENT ON COLUMN Currency.LowSmallName IS '������������ ����������� ������� ������ (����. ���.)';
COMMENT ON COLUMN Currency.Code IS '��� ������� ������ (����. RUB)';
CREATE TABLE NDSAlgorithm (
  ID           number(10) NOT NULL, 
  Name         nvarchar2(100) NOT NULL UNIQUE, 
  PriceTooltip nvarchar2(100), 
  PRIMARY KEY (ID));
COMMENT ON TABLE NDSAlgorithm IS '�������� ��� (��)';
COMMENT ON COLUMN NDSAlgorithm.ID IS '�������������';
COMMENT ON COLUMN NDSAlgorithm.Name IS '�������� (����. ��� ���)';
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
COMMENT ON TABLE Prepayment IS '���������� �� �����';
COMMENT ON COLUMN Prepayment.ContractDocID IS '������ �� ��������';
COMMENT ON COLUMN Prepayment.Sum IS '����� ������';
COMMENT ON COLUMN Prepayment.PercentValue IS '������� �� ����� ��������� �������� (0..100)';
COMMENT ON COLUMN Prepayment.Year IS '���, � ������� ����� �����';
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
COMMENT ON TABLE Person IS '������������� ����������� (�������, �������� �����������, ��������� ������������, ����� ����������� ��������)';
COMMENT ON COLUMN Person.ID IS '��������������';
COMMENT ON COLUMN Person.DegreeID IS '������ �� ������ �������';
COMMENT ON COLUMN Person.IsContractHeadAuthority IS '����� �� ������� ����� ������� �� ������� ����';
COMMENT ON COLUMN Person.IsActSignAuthority IS '����� �� ������� ����� ������� �����';
COMMENT ON COLUMN Person.IsValid IS '�������� �� ������ �� �������� �����������';
COMMENT ON COLUMN Person.FamilyName IS '�������';
COMMENT ON COLUMN Person.FirstName IS '���';
COMMENT ON COLUMN Person.MiddleName IS '��������';
COMMENT ON COLUMN Person.Sex IS '��� (1-�������, 0-�������)';
COMMENT ON COLUMN Person.ContractorPositionID IS '������ �� ��������� � �����������';
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
COMMENT ON TABLE Contractor IS '�����';
COMMENT ON COLUMN Contractor.ID IS '������������ �����������';
COMMENT ON COLUMN Contractor.Name IS '�������� �����������';
COMMENT ON COLUMN Contractor.ShortName IS '�������� �������� �����������';
COMMENT ON COLUMN Contractor.Zip IS '������';
COMMENT ON COLUMN Contractor.Bank IS '����';
COMMENT ON COLUMN Contractor.ContractorTypeID IS '������ �� ��� �����������';
COMMENT ON COLUMN Contractor.INN IS '���';
COMMENT ON COLUMN Contractor.Account IS '��������� ����';
COMMENT ON COLUMN Contractor.BIK IS '���';
COMMENT ON COLUMN Contractor.KPP IS '���';
COMMENT ON COLUMN Contractor.Address IS '����� �����������';
COMMENT ON COLUMN Contractor.CorrespAccount IS '����������������� ����';
COMMENT ON COLUMN Contractor.OKPO IS '����';
COMMENT ON COLUMN Contractor.OKONH IS '�����';
COMMENT ON COLUMN Contractor.OGRN IS '����';
CREATE TABLE Degree (
  ID   number(10) NOT NULL, 
  Name nvarchar2(50) NOT NULL UNIQUE, 
  PRIMARY KEY (ID));
COMMENT ON TABLE Degree IS '������� (��)';
COMMENT ON COLUMN Degree.ID IS '�������������';
COMMENT ON COLUMN Degree.Name IS '�������� (����. �������� ����)';
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
COMMENT ON TABLE NDS IS '��� (��)';
COMMENT ON COLUMN NDS.ID IS '��������������';
COMMENT ON COLUMN NDS.Percents IS '������ ��� (0...100) (����. 18)';
COMMENT ON COLUMN NDS.Year IS '��� ������ ��������';
CREATE TABLE ContractState (
  ID   number(10) NOT NULL, 
  Name nvarchar2(100) NOT NULL UNIQUE, 
  PRIMARY KEY (ID));
COMMENT ON TABLE ContractState IS '��������� �������� (��)';
COMMENT ON COLUMN ContractState.ID IS '�������������';
COMMENT ON COLUMN ContractState.Name IS '�������� (����. ��������)';
CREATE TABLE ContractHierarchy (
  ID                   number(10) NOT NULL, 
  GeneralContractDocID number(10) NOT NULL, 
  SubContractDocID     number(10) NOT NULL, 
  PRIMARY KEY (ID), 
  CONSTRAINT U_CONTRACT_SUB_CONTRACT 
    UNIQUE (GeneralContractDocID, SubContractDocID));
COMMENT ON TABLE ContractHierarchy IS '�������� ��������� (����������� - ������������)';
COMMENT ON COLUMN ContractHierarchy.GeneralContractDocID IS '������ �� ����������� �������';
COMMENT ON COLUMN ContractHierarchy.SubContractDocID IS '������ �� ������� ����������';
CREATE TABLE CurrencyMeasure (
  ID     number(10) NOT NULL, 
  Name   nvarchar2(20) NOT NULL, 
  Factor number(10), 
  PRIMARY KEY (ID));
COMMENT ON TABLE CurrencyMeasure IS '������� ��������� ����� (��)';
COMMENT ON COLUMN CurrencyMeasure.ID IS '�������������';
COMMENT ON COLUMN CurrencyMeasure.Name IS '�������� (�������� ���.)';
COMMENT ON COLUMN CurrencyMeasure.Factor IS '��������� (����. ��� ���. - 1000)';
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
COMMENT ON TABLE PrepaymentDocumentType IS '��� ���������� ��������� (��)';
COMMENT ON COLUMN PrepaymentDocumentType.ID IS '�������������';
COMMENT ON COLUMN PrepaymentDocumentType.Name IS '�������� (����. ���������)';
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
COMMENT ON TABLE TroublesRegistry IS '�������� ������� (��)';
COMMENT ON COLUMN TroublesRegistry.ID IS '�������������';
COMMENT ON COLUMN TroublesRegistry.Name IS '������������ ������� �������� (����. �������� ������� 2006-2010 �.)';
COMMENT ON COLUMN TroublesRegistry.ShortName IS '������� ������������ ������� �������';
COMMENT ON COLUMN TroublesRegistry.ApprovedAt IS '���� �����������';
COMMENT ON COLUMN TroublesRegistry.OrderNum IS '����� ������� (����. 101-202)';
COMMENT ON COLUMN TroublesRegistry.ValidFrom IS '���� ������ ��������';
COMMENT ON COLUMN TroublesRegistry.ValidTo IS '���� ��������� ��������';
CREATE TABLE Trouble (
  ID                number(10) NOT NULL, 
  Name              nvarchar2(500) NOT NULL UNIQUE, 
  Num               nvarchar2(10), 
  TopTroubleID      number(10), 
  TroubleRegistryID number(10), 
  PRIMARY KEY (ID));
COMMENT ON TABLE Trouble IS '�������� (��)';
COMMENT ON COLUMN Trouble.ID IS '�������������';
COMMENT ON COLUMN Trouble.Name IS '������������ ��������';
COMMENT ON COLUMN Trouble.Num IS '����� (����. 1.2)';
COMMENT ON COLUMN Trouble.TopTroubleID IS '������ �� ������������ �������� (������ ������ 2)';
COMMENT ON COLUMN Trouble.TroubleRegistryID IS '������ �� �������� �������';
CREATE TABLE ContractTrouble (
  ID            number(10) NOT NULL, 
  TroubleID     number(10) NOT NULL, 
  ContractDocID number(10) NOT NULL, 
  PRIMARY KEY (ID), 
  CONSTRAINT U_CONTRACT_TROUBLE 
    UNIQUE (TroubleID, ContractDocID));
COMMENT ON TABLE ContractTrouble IS '��������, � ������� ������� ������� (��)';
COMMENT ON COLUMN ContractTrouble.TroubleID IS '������ �� ��������';
COMMENT ON COLUMN ContractTrouble.ContractDocID IS '������ �� �������';
CREATE TABLE Property (
  ID   number(10) NOT NULL, 
  Name nvarchar2(50) NOT NULL UNIQUE, 
  PRIMARY KEY (ID));
COMMENT ON TABLE Property IS '�������������� �������� (��� ������������)(��)';
COMMENT ON COLUMN Property.ID IS '�������������';
COMMENT ON COLUMN Property.Name IS '��������';
CREATE TABLE ContractorPropertiy (
  ID           number(10) NOT NULL, 
  PropertyID   number(10) NOT NULL, 
  ContractorID number(10) NOT NULL, 
  Value        nvarchar2(1000) NOT NULL, 
  PRIMARY KEY (ID), 
  CONSTRAINT U_PROPERTY_CONTRACTOR 
    UNIQUE (PropertyID, ContractorID));
COMMENT ON TABLE ContractorPropertiy IS '�������� �������������� ��������� ��� ����������� (��)';
COMMENT ON COLUMN ContractorPropertiy.PropertyID IS '������ �� �������';
COMMENT ON COLUMN ContractorPropertiy.ContractorID IS '������ �� �����������';
COMMENT ON COLUMN ContractorPropertiy.Value IS '�������� ��������';
CREATE TABLE FunctionalCustomer (
  ID                         number(10) NOT NULL, 
  Name                       nvarchar2(1000) NOT NULL UNIQUE, 
  ContractorID               number(10) NOT NULL, 
  ParentFunctionalCustomerID number(10), 
  FunctionalCustomerTypeID   number(10) NOT NULL, 
  PRIMARY KEY (ID));
COMMENT ON TABLE FunctionalCustomer IS '�������������� ��������';
COMMENT ON COLUMN FunctionalCustomer.ID IS '�������������';
COMMENT ON COLUMN FunctionalCustomer.Name IS '�������� (����. ���������� �������������� ��������)';
COMMENT ON COLUMN FunctionalCustomer.ContractorID IS '������ �� ����������� ��������������� ���������';
COMMENT ON COLUMN FunctionalCustomer.ParentFunctionalCustomerID IS '������ �� ��������������� ��������� - ������ (����� �����-��������)';
COMMENT ON COLUMN FunctionalCustomer.FunctionalCustomerTypeID IS '������ �� ��� ��������������� ���������';
CREATE TABLE FunctionalCustomerType (
  ID   number(10) NOT NULL, 
  Name nvarchar2(100) NOT NULL UNIQUE, 
  PRIMARY KEY (ID));
COMMENT ON TABLE FunctionalCustomerType IS '��� ��������������� ��������� (��)';
COMMENT ON COLUMN FunctionalCustomerType.ID IS '�������������';
COMMENT ON COLUMN FunctionalCustomerType.Name IS '�������� (���� ?)';
CREATE TABLE FunctionalCustomerContract (
  ID                   number(10) NOT NULL, 
  FunctionalCustomerID number(10) NOT NULL, 
  ContractDocID        number(10) NOT NULL, 
  PRIMARY KEY (ID), 
  CONSTRAINT UN_CONTRACT_FC 
    UNIQUE (FunctionalCustomerID, ContractDocID));
COMMENT ON TABLE FunctionalCustomerContract IS '�������������� �������� �������� (��)';
COMMENT ON COLUMN FunctionalCustomerContract.FunctionalCustomerID IS '������ �� ��������������� ���������';
COMMENT ON COLUMN FunctionalCustomerContract.ContractDocID IS '������ �� �������';
CREATE TABLE ContractorType (
  ID          number(10) NOT NULL, 
  Name        nvarchar2(100) NOT NULL UNIQUE, 
  ReportOrder number(3), 
  PRIMARY KEY (ID));
COMMENT ON TABLE ContractorType IS '��� ����������� (��)';
COMMENT ON COLUMN ContractorType.ID IS '�������������';
COMMENT ON COLUMN ContractorType.Name IS '�������� ���� (��������. �������� �����������)';
COMMENT ON COLUMN ContractorType.ReportOrder IS '���������� ������� ���������� ������ � �������';
CREATE TABLE Employee (
  ID           number(10) NOT NULL, 
  Familyname   nvarchar2(40), 
  FirstName    nvarchar2(40), 
  Middlename   nvarchar2(40), 
  Sex          number(1), 
  PostID       number(10) DEFAULT -1 NOT NULL, 
  DepartmentID number(10) DEFAULT -1 NOT NULL, 
  PRIMARY KEY (ID));
COMMENT ON TABLE Employee IS '��������� ��������';
COMMENT ON COLUMN Employee.ID IS '�������������';
COMMENT ON COLUMN Employee.Familyname IS '�������';
COMMENT ON COLUMN Employee.FirstName IS '���';
COMMENT ON COLUMN Employee.Middlename IS '��������';
COMMENT ON COLUMN Employee.Sex IS '��� (1-���, 0-���.)';
CREATE TABLE EnterpriseAuthority (
  ID          number(10) NOT NULL, 
  Num         nvarchar2(255), 
  ValidFrom   date, 
  ValidTo     date, 
  IsValid     number(1), 
  EmployeeID  number(10) NOT NULL, 
  AuthorityID number(10) NOT NULL, 
  PRIMARY KEY (ID));
COMMENT ON TABLE EnterpriseAuthority IS '��������� ��� ��������';
COMMENT ON COLUMN EnterpriseAuthority.ID IS '�������������';
COMMENT ON COLUMN EnterpriseAuthority.Num IS '����� ���������';
COMMENT ON COLUMN EnterpriseAuthority.ValidFrom IS '���� ������ ��������';
COMMENT ON COLUMN EnterpriseAuthority.ValidTo IS '���� ��������� ��������';
COMMENT ON COLUMN EnterpriseAuthority.IsValid IS '������� ����������';
COMMENT ON COLUMN EnterpriseAuthority.EmployeeID IS '������ �� ������������ ���� (�� ������� ���������)';
COMMENT ON COLUMN EnterpriseAuthority.AuthorityID IS '������ �� ���������';
CREATE TABLE Schedule (
  ID                number(10) NOT NULL, 
  CurrencyMeasureID number(10) NOT NULL, 
  WorkTypeID        number(10) NOT NULL, 
  PRIMARY KEY (ID));
COMMENT ON TABLE Schedule IS '����������� ����';
COMMENT ON COLUMN Schedule.ID IS '�������������';
COMMENT ON COLUMN Schedule.CurrencyMeasureID IS '������ �� ������� ��������� ����� (����. ���.)';
COMMENT ON COLUMN Schedule.WorkTypeID IS '������ �� ��� �����';
CREATE TABLE ScheduleContract (
  ID            number(10) NOT NULL, 
  ContractDocID number(10) NOT NULL, 
  AppNum        number(3), 
  ScheduleID    number(10) NOT NULL, 
  PRIMARY KEY (ID), 
  CONSTRAINT U_SCHEDULE_CONTRACT 
    UNIQUE (ContractDocID, ScheduleID));
COMMENT ON TABLE ScheduleContract IS '����������� ���� �������� (��)';
COMMENT ON COLUMN ScheduleContract.ContractDocID IS '������ �� �������';
COMMENT ON COLUMN ScheduleContract.AppNum IS '� ���������� ��������';
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
COMMENT ON TABLE Stage IS '����';
COMMENT ON COLUMN Stage.ID IS '�������������';
COMMENT ON COLUMN Stage.Num IS '����� �����';
COMMENT ON COLUMN Stage.Subject IS '��������';
COMMENT ON COLUMN Stage.StartsAt IS '���� ������';
COMMENT ON COLUMN Stage.EndsAt IS '���� ���������';
COMMENT ON COLUMN Stage.Price IS '���� �����';
COMMENT ON COLUMN Stage.NdsAlgorithmID IS '������ �� �������� ������� ���';
COMMENT ON COLUMN Stage.ParentID IS '������ �� ������������ ���� (���������� �������� ������: ����-�������)';
COMMENT ON COLUMN Stage.ActID IS '������ �� ��� �����-������� (���� ���� ������ - �� �� Null)';
COMMENT ON COLUMN Stage.Code IS '��� �������';
COMMENT ON COLUMN Stage.NdsID IS '������ �� ������ ���';
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
COMMENT ON TABLE Act IS '��� �����-������';
COMMENT ON COLUMN Act.ID IS '�������������';
COMMENT ON COLUMN Act.Num IS '����� ���� �����-������';
COMMENT ON COLUMN Act.SignDate IS '���� ���������� ���� �����-������';
COMMENT ON COLUMN Act.ActTypeID IS '������ �� ��� ���� �����-������';
COMMENT ON COLUMN Act.NDSID IS '������ �� ������ ���';
COMMENT ON COLUMN Act.RegionID IS '������ �� ������';
COMMENT ON COLUMN Act.TotalSum IS '����� ����� �� ���� �����-������';
COMMENT ON COLUMN Act.SumForTransfer IS '����� � ������������';
COMMENT ON COLUMN Act.Status IS '��������� ���� �����-������';
COMMENT ON COLUMN Act.EnterpriceAuthorityID IS '������ �� ��������� ��� ��������';
COMMENT ON COLUMN Act.CurrencyRate IS '���� ������ �� ���� RateDate';
COMMENT ON COLUMN Act.RateDate IS '���� ��� ����� ������';
COMMENT ON COLUMN Act.NdsalgorithmID IS '�������� ������� ���';
COMMENT ON COLUMN Act.CurrencyID IS '������ ����';
COMMENT ON COLUMN Act.CurrencymeasureID IS '������ �� ������� ��������� �����';
COMMENT ON COLUMN Act.IsSigned IS '�������� ��� ���';
CREATE TABLE ClosedStageRelation (
  ID            number(10) NOT NULL, 
  StageID       number(10) NOT NULL, 
  ClosedStageID number(10) NOT NULL, 
  PRIMARY KEY (ID), 
  CONSTRAINT U_STAGE_CLOSED_STAGE 
    UNIQUE (StageID, ClosedStageID));
COMMENT ON TABLE ClosedStageRelation IS '����� ������ ���. ���������� � ��������� ������� (��)';
COMMENT ON COLUMN ClosedStageRelation.StageID IS 'C����� �� ���� � ���. ����������';
COMMENT ON COLUMN ClosedStageRelation.ClosedStageID IS '������ �� �������� ����';
CREATE TABLE FuncCustomerPerson (
  ID             number(10) NOT NULL, 
  FuncCustomerID number(10) NOT NULL UNIQUE, 
  PersonID       number(10), 
  PRIMARY KEY (ID));
COMMENT ON TABLE FuncCustomerPerson IS '������������� (�������) ��������������� ���������';
COMMENT ON COLUMN FuncCustomerPerson.ID IS '�������������';
COMMENT ON COLUMN FuncCustomerPerson.FuncCustomerID IS '������ �� ��������������� ���������';
COMMENT ON COLUMN FuncCustomerPerson.PersonID IS '������ �� �������� - ������������� ��������������� ���������';
CREATE TABLE SightFuncPerson (
  ID                   number(10) NOT NULL, 
  Name                 nvarchar2(500), 
  SightFuncPersonSchID number(10) NOT NULL, 
  ActID                number(10) NOT NULL, 
  PRIMARY KEY (ID));
COMMENT ON TABLE SightFuncPerson IS '���� ������������� (�������) ��������������� ���������';
COMMENT ON COLUMN SightFuncPerson.ID IS '�������������';
COMMENT ON COLUMN SightFuncPerson.Name IS '�������� ����';
COMMENT ON COLUMN SightFuncPerson.SightFuncPersonSchID IS '������ �� ����� ����������� ��������������� ���������';
COMMENT ON COLUMN SightFuncPerson.ActID IS '������ �� ���';
CREATE TABLE Region (
  ID   number(10) NOT NULL, 
  Name nvarchar2(500) NOT NULL UNIQUE, 
  PRIMARY KEY (ID));
COMMENT ON TABLE Region IS '������ (��)';
COMMENT ON COLUMN Region.ID IS '�������������';
COMMENT ON COLUMN Region.Name IS '�������� �������';
CREATE TABLE ActType (
  ID           number(10) NOT NULL, 
  ContractorID number(10) NOT NULL, 
  TypeName     nvarchar2(200), 
  IsActive     number(1), 
  PRIMARY KEY (ID));
COMMENT ON TABLE ActType IS '��� ���� �����-������';
COMMENT ON COLUMN ActType.ContractorID IS '������ �� �����������';
COMMENT ON COLUMN ActType.TypeName IS '�������� ��� ���� �����-������';
COMMENT ON COLUMN ActType.IsActive IS '������� ����������';
CREATE TABLE WorkType (
  ID        number(10) NOT NULL, 
  Name      nvarchar2(1000) NOT NULL UNIQUE, 
  ShortName nvarchar2(50) UNIQUE, 
  PRIMARY KEY (ID));
COMMENT ON TABLE WorkType IS '��� ����� (��)';
COMMENT ON COLUMN WorkType.ID IS '�������������';
COMMENT ON COLUMN WorkType.Name IS '�������� ���� �����';
COMMENT ON COLUMN WorkType.ShortName IS '����������� �������� �����';
CREATE TABLE Disposal (
  ID           number(10) NOT NULL, 
  Num          nvarchar2(10), 
  ApprovedDate date, 
  PRIMARY KEY (ID));
COMMENT ON TABLE Disposal IS '������������, ��������������� ������������� �� �������� �� ������� ��������';
COMMENT ON COLUMN Disposal.ID IS '�������������';
COMMENT ON COLUMN Disposal.Num IS '����� ������������';
COMMENT ON COLUMN Disposal.ApprovedDate IS '���� �����������';
CREATE TABLE Responsible (
  ID            number(10) NOT NULL, 
  DisposalID    number(10) NOT NULL, 
  EmployeeID    number(10) NOT NULL, 
  RoleID        number(10) DEFAULT -1 NOT NULL, 
  ContractdocID number(10), 
  PRIMARY KEY (ID));
COMMENT ON TABLE Responsible IS '������������� �� ������� ��������, �������� ������������ (��)';
COMMENT ON COLUMN Responsible.DisposalID IS '������ �� ������������';
COMMENT ON COLUMN Responsible.EmployeeID IS '������ �� ������������� �� ��������� �������� ������������';
COMMENT ON COLUMN Responsible.RoleID IS '������ �� ����';
COMMENT ON COLUMN Responsible.ContractdocID IS '������ �� �������';
CREATE TABLE Role (
  ID   number(10) NOT NULL, 
  Name nvarchar2(100) NOT NULL UNIQUE, 
  PRIMARY KEY (ID));
COMMENT ON TABLE Role IS '���� (��)';
COMMENT ON COLUMN Role.ID IS '�������������';
COMMENT ON COLUMN Role.Name IS '�������� (����. ������������ �����������?)';
CREATE TABLE Authority (
  ID   number(10) NOT NULL, 
  Name nvarchar2(200) NOT NULL UNIQUE, 
  PRIMARY KEY (ID));
COMMENT ON TABLE Authority IS '��������� (��)';
COMMENT ON COLUMN Authority.ID IS '�������������';
COMMENT ON COLUMN Authority.Name IS '������������ ���������';
CREATE TABLE SightFuncPersonScheme (
  ID           number(10) NOT NULL, 
  Num          number(10), 
  IsActive     number(1), 
  FuncPersonID number(10) NOT NULL, 
  PRIMARY KEY (ID));
COMMENT ON TABLE SightFuncPersonScheme IS '����� ����������� ��������������� ���������';
COMMENT ON COLUMN SightFuncPersonScheme.ID IS '�������������';
COMMENT ON COLUMN SightFuncPersonScheme.Num IS '�����';
COMMENT ON COLUMN SightFuncPersonScheme.IsActive IS '������� ����������';
COMMENT ON COLUMN SightFuncPersonScheme.FuncPersonID IS '������ �� ������������� (�������)��������������� ���������';
CREATE TABLE ActPaymentDocument (
  ID                number(10) NOT NULL, 
  ActID             number(10) NOT NULL, 
  PaymentDocumentID number(10) NOT NULL, 
  PRIMARY KEY (ID), 
  CONSTRAINT U_ACT_PAYMENTDOC 
    UNIQUE (ActID, PaymentDocumentID));
COMMENT ON TABLE ActPaymentDocument IS '��������� ��������� �� ���� �����-������';
COMMENT ON COLUMN ActPaymentDocument.ActID IS '������ �� ��� �����-������';
COMMENT ON COLUMN ActPaymentDocument.PaymentDocumentID IS '������ �� ��������� ��������';
CREATE TABLE TransferAct (
  ID                number(10) NOT NULL, 
  Num               number(10), 
  SignDate          date, 
  TransferActTypeID number(10) NOT NULL, 
  PRIMARY KEY (ID));
COMMENT ON TABLE TransferAct IS '��� ������-�������� - ��� ��������������, ����� ��������� ���� �������� ���������';
COMMENT ON COLUMN TransferAct.ID IS '�������������';
COMMENT ON COLUMN TransferAct.Num IS '�����';
COMMENT ON COLUMN TransferAct.SignDate IS '���� ���������� ���� ��������';
COMMENT ON COLUMN TransferAct.TransferActTypeID IS '������ �� ��� ���� ������-��������';
CREATE TABLE TransferActType (
  ID   number(10) NOT NULL, 
  Name nvarchar2(255) NOT NULL UNIQUE, 
  PRIMARY KEY (ID));
COMMENT ON TABLE TransferActType IS '��� ���� ������-��������(��)';
COMMENT ON COLUMN TransferActType.ID IS '�������������';
COMMENT ON COLUMN TransferActType.Name IS '������������ ����';
CREATE TABLE Document (
  ID   number(10) NOT NULL, 
  Name nvarchar2(255) NOT NULL UNIQUE, 
  PRIMARY KEY (ID));
COMMENT ON TABLE Document IS '�������� (����. ����������� �������, ����������� ����,...)';
COMMENT ON COLUMN Document.ID IS '�������������';
COMMENT ON COLUMN Document.Name IS '������������ ���������';
CREATE TABLE TransferActActDocument (
  ID            number(10) NOT NULL, 
  TransferActID number(10) NOT NULL, 
  DocumentID    number(10) NOT NULL, 
  ActID         number(10) NOT NULL, 
  PagesCount    number(10), 
  PRIMARY KEY (ID), 
  CONSTRAINT U_TRANSFERACT_ACT_DOC 
    UNIQUE (TransferActID, DocumentID, ActID));
COMMENT ON TABLE TransferActActDocument IS '����������� ���� �����-������� ���������, ����������� �� ���� �������-�������� (��)';
COMMENT ON COLUMN TransferActActDocument.TransferActID IS '������ �� ��� ������-��������';
COMMENT ON COLUMN TransferActActDocument.DocumentID IS '������ �� ��������';
COMMENT ON COLUMN TransferActActDocument.ActID IS '������ �� ��� �����-�������';
COMMENT ON COLUMN TransferActActDocument.PagesCount IS '���������� �������';
CREATE TABLE TransferActTypeDocument (
  ID                number(10) NOT NULL, 
  TransferActTypeID number(10) NOT NULL, 
  DocumentID        number(10) NOT NULL, 
  PRIMARY KEY (ID), 
  CONSTRAINT U_TRANSFER_ACTTYPE_DOC 
    UNIQUE (TransferActTypeID, DocumentID));
COMMENT ON TABLE TransferActTypeDocument IS '��� ��������� (��)';
COMMENT ON COLUMN TransferActTypeDocument.TransferActTypeID IS '������ �� ���';
COMMENT ON COLUMN TransferActTypeDocument.DocumentID IS '������ �� ��������';
CREATE TABLE ContractTranActDoc (
  ID            number(10) NOT NULL, 
  ContractDocID number(10) NOT NULL, 
  TransferActID number(10) NOT NULL, 
  DocumentID    number(10) NOT NULL, 
  PagesCount    number(10), 
  PRIMARY KEY (ID), 
  CONSTRAINT U_TRANS_ACTDOC_CONTRACT 
    UNIQUE (ContractDocID, TransferActID, DocumentID));
COMMENT ON TABLE ContractTranActDoc IS '���������, ��������� � �������� �������� ���� ������-��������';
COMMENT ON COLUMN ContractTranActDoc.ContractDocID IS '������ �� �������';
COMMENT ON COLUMN ContractTranActDoc.TransferActID IS '������ �� ��� �������-��������';
COMMENT ON COLUMN ContractTranActDoc.DocumentID IS '������ �� ��������';
COMMENT ON COLUMN ContractTranActDoc.PagesCount IS '���������� �������';
CREATE TABLE StageResult (
  ID                       number(10) NOT NULL, 
  Name                     nvarchar2(2000), 
  EconomicEfficiencyTypeID number(10), 
  StageID                  number(10) NOT NULL, 
  NTPSubViewID             number(10) NOT NULL, 
  PRIMARY KEY (ID), 
  CONSTRAINT U_STAGE_RESULT 
    UNIQUE (StageID, Name));
COMMENT ON TABLE StageResult IS '��������� ����� (��)';
COMMENT ON COLUMN StageResult.ID IS '�������������';
COMMENT ON COLUMN StageResult.Name IS '������������ ����������';
COMMENT ON COLUMN StageResult.EconomicEfficiencyTypeID IS '������ �� ��� ������������� �������������';
COMMENT ON COLUMN StageResult.StageID IS '������ �� ����';
COMMENT ON COLUMN StageResult.NTPSubViewID IS '������ �� ������ ���';
CREATE TABLE NTPSubView (
  ID        number(10) NOT NULL, 
  Name      nvarchar2(255), 
  NTPViewID number(10) NOT NULL, 
  ShortName nvarchar2(50), 
  PRIMARY KEY (ID));
COMMENT ON TABLE NTPSubView IS '������ ���';
COMMENT ON COLUMN NTPSubView.ID IS '�������������';
COMMENT ON COLUMN NTPSubView.Name IS '�������� ������� ���';
COMMENT ON COLUMN NTPSubView.NTPViewID IS '������ �� ��� ���';
COMMENT ON COLUMN NTPSubView.ShortName IS '������� ������������ ���';
CREATE TABLE NTPView (
  ID   number(10) NOT NULL, 
  Name nvarchar2(255) NOT NULL UNIQUE, 
  PRIMARY KEY (ID));
COMMENT ON TABLE NTPView IS '��� ��� (��)';
COMMENT ON COLUMN NTPView.ID IS '�������������';
COMMENT ON COLUMN NTPView.Name IS '�������� (����. ��������� ��� ����������-������������ ������������)';
CREATE TABLE EconomEfficiencyType (
  ID   number(10) NOT NULL, 
  Name nvarchar2(255) NOT NULL UNIQUE, 
  PRIMARY KEY (ID));
COMMENT ON TABLE EconomEfficiencyType IS '��� ������������� ������������� (��)';
COMMENT ON COLUMN EconomEfficiencyType.ID IS '�������������';
COMMENT ON COLUMN EconomEfficiencyType.Name IS '��������';
CREATE TABLE EconomEfficiencyParameter (
  ID   number(10) NOT NULL, 
  Name nvarchar2(255) NOT NULL UNIQUE, 
  PRIMARY KEY (ID));
COMMENT ON TABLE EconomEfficiencyParameter IS '�������� ������������� ������������� (��)';
COMMENT ON COLUMN EconomEfficiencyParameter.ID IS '�������������';
COMMENT ON COLUMN EconomEfficiencyParameter.Name IS '�������� ���������';
CREATE TABLE EfficienceParameterType (
  ID                          number(10) NOT NULL, 
  EconomEfficiencyParameterID number(10) NOT NULL, 
  EconomEfficiencyTypeID      number(10) NOT NULL, 
  PRIMARY KEY (ID), 
  CONSTRAINT U_EF_PAR_TYPE 
    UNIQUE (EconomEfficiencyParameterID, EconomEfficiencyTypeID));
COMMENT ON TABLE EfficienceParameterType IS '�������� ���� ������������� ������������� (��)';
COMMENT ON COLUMN EfficienceParameterType.EconomEfficiencyParameterID IS '������ �� �������� ������������� �������������';
COMMENT ON COLUMN EfficienceParameterType.EconomEfficiencyTypeID IS '������ �� ��� ������������� �������������';
CREATE TABLE EfParameterStageResult (
  ID                          number(10) NOT NULL, 
  EconomEfficiencyParameterID number(10) NOT NULL, 
  StageResultID               number(10) NOT NULL, 
  Value                       number(19, 6), 
  PRIMARY KEY (ID), 
  CONSTRAINT U_EF_STAGE_RESULT 
    UNIQUE (EconomEfficiencyParameterID, StageResultID));
COMMENT ON TABLE EfParameterStageResult IS '�������� ���������� ������������� ������������� ���������� ����� (��)';
COMMENT ON COLUMN EfParameterStageResult.EconomEfficiencyParameterID IS '������ �� �������� ������������� ��������������';
COMMENT ON COLUMN EfParameterStageResult.StageResultID IS '������ �� ��������� �����';
COMMENT ON COLUMN EfParameterStageResult.Value IS '�������� ���������';
CREATE TABLE ContractorPosition (
  ID           number(10) NOT NULL, 
  ContractorID number(10) NOT NULL, 
  PositionID   number(10) NOT NULL, 
  PRIMARY KEY (ID));
COMMENT ON TABLE ContractorPosition IS '���� � �� ��������� � ������������ (��������, �������� ������������, ��������� ������������, ����� ��������) (��)';
COMMENT ON COLUMN ContractorPosition.ID IS '�������������';
COMMENT ON COLUMN ContractorPosition.ContractorID IS '������ �� ����������� (�����������)';
COMMENT ON COLUMN ContractorPosition.PositionID IS '���������';
CREATE TABLE ContractorAuthority (
  ID           number(10) NOT NULL, 
  AuthorityID  number(10) NOT NULL, 
  NumDocument  nvarchar2(255), 
  ValidFrom    date, 
  ValidTo      date, 
  IsValid      number(1), 
  ContractorID number(10) NOT NULL, 
  PRIMARY KEY (ID));
COMMENT ON TABLE ContractorAuthority IS '��������� ��� ������������-�����������';
COMMENT ON COLUMN ContractorAuthority.ID IS '�������������';
COMMENT ON COLUMN ContractorAuthority.AuthorityID IS '������ �� ���������';
COMMENT ON COLUMN ContractorAuthority.NumDocument IS '����� ���������';
COMMENT ON COLUMN ContractorAuthority.ValidFrom IS '���� ������ ��������';
COMMENT ON COLUMN ContractorAuthority.ValidTo IS '���� ��������� ��������';
COMMENT ON COLUMN ContractorAuthority.IsValid IS '������� ����������';
COMMENT ON COLUMN ContractorAuthority.ContractorID IS '������ �� ������������� �� ������� �����������';
CREATE TABLE Position (
  ID   number(10) NOT NULL, 
  Name nvarchar2(255) NOT NULL UNIQUE, 
  PRIMARY KEY (ID));
COMMENT ON TABLE Position IS '��������� (��)';
COMMENT ON COLUMN Position.ID IS '�������������';
COMMENT ON COLUMN Position.Name IS '��������';
CREATE TABLE SubGeneralHierarchi (
  ID                        number(10) NOT NULL, 
  GeneralContractDocStageID number(10) NOT NULL, 
  SubContractDocStageID     number(10) NOT NULL, 
  PRIMARY KEY (ID), 
  CONSTRAINT U_STAGE_SUBCONTRACT_STAGE 
    UNIQUE (GeneralContractDocStageID, SubContractDocStageID));
COMMENT ON TABLE SubGeneralHierarchi IS '����� ������ ������������ �������� � ������������� ���������� (��)';
COMMENT ON COLUMN SubGeneralHierarchi.GeneralContractDocStageID IS '������ �� ���� ������������ ��������';
COMMENT ON COLUMN SubGeneralHierarchi.SubContractDocStageID IS '������ �� ���� ������������� ��������';
CREATE TABLE UDMetadata (
  SchemeRelease   number(10) NOT NULL, 
  SchemeBuild     number(10) NOT NULL, 
  SchemeTimestamp date DEFAULT sysdate NOT NULL, 
  PRIMARY KEY (SchemeRelease, 
  SchemeBuild));
COMMENT ON COLUMN UDMetadata.SchemeRelease IS '������� ����� ������ �����';
COMMENT ON COLUMN UDMetadata.SchemeBuild IS '������� ����� ������ �����';
COMMENT ON COLUMN UDMetadata.SchemeTimestamp IS '��������� ������� �������� �����';
CREATE TABLE IMPORTINGSCHEME (
  ID         number(10) NOT NULL, 
  Schemename nvarchar2(500) NOT NULL UNIQUE, 
  PRIMARY KEY (ID));
COMMENT ON TABLE IMPORTINGSCHEME IS '����� �������';
COMMENT ON COLUMN IMPORTINGSCHEME.ID IS '������������� ����� �������';
COMMENT ON COLUMN IMPORTINGSCHEME.Schemename IS '��� ����� �������';
CREATE TABLE IMPORTINGSCHEMEITEM (
  ID          number(10, 0) NOT NULL, 
  Columnname  nvarchar2(200) NOT NULL, 
  Columnsign  nvarchar2(20) NOT NULL, 
  Columnindex number(10) NOT NULL, 
  Isrequired  number(1) NOT NULL, 
  SchemeID    number(10) NOT NULL, 
  PRIMARY KEY (ID));
COMMENT ON TABLE IMPORTINGSCHEMEITEM IS '������� ����� �������';
COMMENT ON COLUMN IMPORTINGSCHEMEITEM.ID IS '������������� �������� ����� �������';
COMMENT ON COLUMN IMPORTINGSCHEMEITEM.Columnname IS '��� �������';
COMMENT ON COLUMN IMPORTINGSCHEMEITEM.Columnsign IS '����';
COMMENT ON COLUMN IMPORTINGSCHEMEITEM.Columnindex IS '������ �������';
COMMENT ON COLUMN IMPORTINGSCHEMEITEM.Isrequired IS '�������� �� ������� ������������';
COMMENT ON COLUMN IMPORTINGSCHEMEITEM.SchemeID IS '������ �� ����� �������';
CREATE TABLE Department (
  ID           number(10) NOT NULL, 
  ParentID     number(10), 
  ManagerID    number(10) NOT NULL, 
  DirectedByID number(10) NOT NULL, 
  Name         nvarchar2(200) NOT NULL UNIQUE, 
  PRIMARY KEY (ID));
COMMENT ON COLUMN Department.ManagerID IS '������������ ������';
COMMENT ON COLUMN Department.DirectedByID IS '������ ��� ������';
CREATE TABLE Post (
  ID   number(10) NOT NULL, 
  Name nvarchar2(200) NOT NULL UNIQUE, 
  PRIMARY KEY (ID));
COMMENT ON TABLE Post IS '���������';
CREATE TABLE ResponsibleForOrder (
  ID                           number(10) NOT NULL, 
  DepartmentID                 number(10) NOT NULL, 
  EmployeeID                   number(10) NOT NULL, 
  ResponsibleAssignmentOrderID number(10), 
  PRIMARY KEY (ID));
COMMENT ON TABLE ResponsibleForOrder IS '������������� �� �������';
CREATE TABLE Location (
  ID   number(10) NOT NULL, 
  Name nvarchar2(200) NOT NULL UNIQUE, 
  PRIMARY KEY (ID));
COMMENT ON TABLE Location IS '����� ����������';
COMMENT ON COLUMN Location.Name IS '�������� ����� ����������';
CREATE TABLE ApprovalState (
  ID   number(10) NOT NULL, 
  Name nvarchar2(200) NOT NULL UNIQUE, 
  PRIMARY KEY (ID));
COMMENT ON TABLE ApprovalState IS '��������� ������������ ���������';
COMMENT ON COLUMN ApprovalState.Name IS '�������� ��������� ������������';
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
COMMENT ON TABLE ApprovalProcess IS '������ ������������ ��������';
COMMENT ON COLUMN ApprovalProcess.ToLocationID IS '���� ��������';
COMMENT ON COLUMN ApprovalProcess.FromLocationId IS '��� �������';
COMMENT ON COLUMN ApprovalProcess.TransferStateAt IS '���� �������� ��������� � ���������';
COMMENT ON COLUMN ApprovalProcess.EnterStateAt IS '���� ����������� ��������� � ���������';
COMMENT ON COLUMN ApprovalProcess.MissiveId IS '����� ������';
COMMENT ON COLUMN ApprovalProcess.MissiveDate IS '���� ������';
COMMENT ON COLUMN ApprovalProcess.Description IS '����������';
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
COMMENT ON TABLE MissiveType IS '��� ������';
CREATE TABLE ApprovalGoal (
  ID   number(10) NOT NULL, 
  Name nvarchar2(200) NOT NULL UNIQUE, 
  PRIMARY KEY (ID));
COMMENT ON TABLE ApprovalGoal IS '���� ����������� �� ������������';
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
