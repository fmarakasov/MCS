/*
������� ����� ��������� 1.0
������ ������������ 02:33:04 PM 20/01/2011
������ ����� 1.0
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
CREATE TABLE ContractType (
  ID   number(10) NOT NULL, 
  Name nvarchar2(200) NOT NULL UNIQUE, 
  PRIMARY KEY (ID));
COMMENT ON TABLE ContractType IS '��� �������� (��)';
COMMENT ON COLUMN ContractType.ID IS '�������������';
COMMENT ON COLUMN ContractType.Name IS '�������� (����. �����)';
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
  ID   number(10) NOT NULL, 
  Name nvarchar2(100) NOT NULL UNIQUE, 
  PRIMARY KEY (ID));
COMMENT ON TABLE NDSAlgorithm IS '�������� ��� (��)';
COMMENT ON COLUMN NDSAlgorithm.ID IS '�������������';
COMMENT ON COLUMN NDSAlgorithm.Name IS '�������� (����. ��� ���)';
CREATE TABLE Prepayment (
  ContractDocID number(10) NOT NULL, 
  Year          number(4) NOT NULL, 
  Sum           number(10, 2) NOT NULL, 
  "Percent"     float(10) NOT NULL, 
  PRIMARY KEY (ContractDocID, 
  Year));
COMMENT ON TABLE Prepayment IS '���������� �� �����';
COMMENT ON COLUMN Prepayment.ContractDocID IS '������ �� ��������';
COMMENT ON COLUMN Prepayment.Year IS '���, � ������� ����� �����';
COMMENT ON COLUMN Prepayment.Sum IS '����� ������';
COMMENT ON COLUMN Prepayment."Percent" IS '������� �� ����� ��������� �������� (0..100)';
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
COMMENT ON TABLE Contractor IS '�����';
COMMENT ON COLUMN Contractor.ID IS '������������ �����������';
COMMENT ON COLUMN Contractor.Name IS '�������� �����������';
COMMENT ON COLUMN Contractor.ShortName IS '�������� �������� �����������';
COMMENT ON COLUMN Contractor.Zip IS '������';
COMMENT ON COLUMN Contractor.City IS '�����';
COMMENT ON COLUMN Contractor.Build IS '����� ����';
COMMENT ON COLUMN Contractor.Block IS '����� �������';
COMMENT ON COLUMN Contractor.Bank IS '����';
COMMENT ON COLUMN Contractor.ContractorTypeID IS '������ �� ��� �����������';
COMMENT ON COLUMN Contractor.Appartment IS '����� ��������';
COMMENT ON COLUMN Contractor.INN IS '���';
COMMENT ON COLUMN Contractor.Account IS '����� �����';
COMMENT ON COLUMN Contractor.BIK IS '���';
COMMENT ON COLUMN Contractor.KPP IS '���';
CREATE TABLE Degree (
  ID   number(10) NOT NULL, 
  Name nvarchar2(50) NOT NULL UNIQUE, 
  PRIMARY KEY (ID));
COMMENT ON TABLE Degree IS '������� (��)';
COMMENT ON COLUMN Degree.ID IS '�������������';
COMMENT ON COLUMN Degree.Name IS '�������� (����. �������� ����)';
CREATE TABLE NDS (
  ID        number(10) NOT NULL, 
  "Percent" number(4, 2) NOT NULL UNIQUE, 
  Year      number(4) NOT NULL UNIQUE, 
  PRIMARY KEY (ID));
COMMENT ON TABLE NDS IS '��� (��)';
COMMENT ON COLUMN NDS.ID IS '��������������';
COMMENT ON COLUMN NDS."Percent" IS '������ ��� (0...100) (����. 18)';
COMMENT ON COLUMN NDS.Year IS '��� ������ ��������';
CREATE TABLE ContractState (
  ID   number(10) NOT NULL, 
  Name nvarchar2(100) NOT NULL UNIQUE, 
  PRIMARY KEY (ID));
COMMENT ON TABLE ContractState IS '��������� �������� (��)';
COMMENT ON COLUMN ContractState.ID IS '�������������';
COMMENT ON COLUMN ContractState.Name IS '�������� (����. ��������)';
CREATE TABLE ContractHierarchy (
  GeneralContractDocID number(10) NOT NULL, 
  SubContractDocID     number(10) NOT NULL, 
  PRIMARY KEY (GeneralContractDocID, 
  SubContractDocID));
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
  TroubleID     number(10) NOT NULL, 
  ContractDocID number(10) NOT NULL, 
  Description   nvarchar2(200), 
  PRIMARY KEY (TroubleID, 
  ContractDocID));
COMMENT ON TABLE ContractTrouble IS '��������, � ������� ������� ������� (��)';
COMMENT ON COLUMN ContractTrouble.TroubleID IS '������ �� ��������';
COMMENT ON COLUMN ContractTrouble.ContractDocID IS '������ �� �������';
COMMENT ON COLUMN ContractTrouble.Description IS '�������� �������� � ��������';
CREATE TABLE Property (
  ID   number(10) NOT NULL, 
  Name nvarchar2(50) NOT NULL UNIQUE, 
  PRIMARY KEY (ID));
COMMENT ON TABLE Property IS '�������������� �������� (��� ������������)(��)';
COMMENT ON COLUMN Property.ID IS '�������������';
COMMENT ON COLUMN Property.Name IS '��������';
CREATE TABLE AdditionContractorPropertiy (
  PropertyID   number(10) NOT NULL, 
  ContractorID number(10) NOT NULL, 
  Value        nvarchar2(1000) NOT NULL, 
  PRIMARY KEY (PropertyID, 
  ContractorID));
COMMENT ON TABLE AdditionContractorPropertiy IS '�������� �������������� ��������� ��� ����������� (��)';
COMMENT ON COLUMN AdditionContractorPropertiy.PropertyID IS '������ �� �������';
COMMENT ON COLUMN AdditionContractorPropertiy.ContractorID IS '������ �� �����������';
COMMENT ON COLUMN AdditionContractorPropertiy.Value IS '�������� ��������';
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
  FunctionalCustomerID number(10) NOT NULL, 
  ContractDocID        number(10) NOT NULL, 
  Description          nvarchar2(1000), 
  PRIMARY KEY (FunctionalCustomerID, 
  ContractDocID));
COMMENT ON TABLE FunctionalCustomerContract IS '�������������� �������� �������� (��)';
COMMENT ON COLUMN FunctionalCustomerContract.FunctionalCustomerID IS '������ �� ��������������� ���������';
COMMENT ON COLUMN FunctionalCustomerContract.ContractDocID IS '������ �� �������';
COMMENT ON COLUMN FunctionalCustomerContract.Description IS '�������� ��������������� ��������� � ��������';
CREATE TABLE ContractorType (
  ID   number(10) NOT NULL, 
  Name nvarchar2(100) NOT NULL UNIQUE, 
  PRIMARY KEY (ID));
COMMENT ON TABLE ContractorType IS '��� ����������� (��)';
COMMENT ON COLUMN ContractorType.ID IS '�������������';
COMMENT ON COLUMN ContractorType.Name IS '�������� ���� (��������. �������� �����������)';
CREATE TABLE Employee (
  ID         number(10) NOT NULL, 
  ManagerID  number(10), 
  RoleID     number(10) NOT NULL, 
  Familyname nvarchar2(40) NOT NULL, 
  FirstName  nvarchar2(40), 
  Middlename nvarchar2(40), 
  Sex        number(1), 
  PRIMARY KEY (ID));
COMMENT ON TABLE Employee IS '��������� ��������';
COMMENT ON COLUMN Employee.ID IS '�������������';
COMMENT ON COLUMN Employee.ManagerID IS '������ �� ������������';
COMMENT ON COLUMN Employee.RoleID IS '������ �� ���� (����. ������������ �����������?)';
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
  ContractDocID number(10) NOT NULL, 
  ScheduleID    number(10) NOT NULL, 
  AppNum        number(3), 
  PRIMARY KEY (ContractDocID, 
  ScheduleID));
COMMENT ON TABLE ScheduleContract IS '����������� ���� �������� (��)';
COMMENT ON COLUMN ScheduleContract.ContractDocID IS '������ �� �������';
COMMENT ON COLUMN ScheduleContract.AppNum IS '� ���������� ��������';
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
COMMENT ON TABLE Act IS '��� �����-������';
COMMENT ON COLUMN Act.ID IS '�������������';
COMMENT ON COLUMN Act.Num IS '����� ���� �����-������';
COMMENT ON COLUMN Act."Date" IS '���� ���� �����-������';
COMMENT ON COLUMN Act.ActTypeID IS '������ �� ��� ���� �����-������';
COMMENT ON COLUMN Act.NDSID IS '������ �� ������ ���';
COMMENT ON COLUMN Act.RegionID IS '������ �� ������';
COMMENT ON COLUMN Act.TotalSum IS '����� ����� �� ���� �����-������';
COMMENT ON COLUMN Act.SumForTransfer IS '����� � ������������';
COMMENT ON COLUMN Act.Status IS '��������� ���� �����-������';
COMMENT ON COLUMN Act.EnterpriceAuthorityID IS '������ �� ��������� ��� ��������';
COMMENT ON COLUMN Act.CurrencyRate IS '���� ������ �� ���� RateDate';
COMMENT ON COLUMN Act.RateDate IS '���� ��� ����� ������';
CREATE TABLE ClosedStageRelation (
  StageID       number(10) NOT NULL, 
  ClosedStageID number(10) NOT NULL, 
  PRIMARY KEY (StageID, 
  ClosedStageID));
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
  ID   number(10) NOT NULL, 
  Name nvarchar2(255) NOT NULL UNIQUE, 
  PRIMARY KEY (ID));
COMMENT ON TABLE WorkType IS '��� ����� (��)';
COMMENT ON COLUMN WorkType.ID IS '�������������';
COMMENT ON COLUMN WorkType.Name IS '�������� ���� �����';
CREATE TABLE Disposal (
  ID            number(10) NOT NULL, 
  Num           nvarchar2(10), 
  ApprovedDate  date, 
  ContractdocID number(10) NOT NULL, 
  PRIMARY KEY (ID));
COMMENT ON TABLE Disposal IS '������������, ��������������� ������������� �� �������� �� ������� ��������';
COMMENT ON COLUMN Disposal.ID IS '�������������';
COMMENT ON COLUMN Disposal.Num IS '����� ������������';
COMMENT ON COLUMN Disposal.ApprovedDate IS '���� �����������';
COMMENT ON COLUMN Disposal.ContractdocID IS '������ �� ������� �� �������� ������� ������������';
CREATE TABLE Responsible (
  DisposalID number(10) NOT NULL, 
  EmployeeID number(10) NOT NULL, 
  PRIMARY KEY (DisposalID, 
  EmployeeID));
COMMENT ON TABLE Responsible IS '������������� �� ������� ��������, �������� ������������ (��)';
COMMENT ON COLUMN Responsible.DisposalID IS '������ �� ������������';
COMMENT ON COLUMN Responsible.EmployeeID IS '������ �� ������������� �� ��������� �������� ������������';
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
CREATE TABLE Act_PaymentDocument (
  ActID             number(10) NOT NULL, 
  PaymentDocumentID number(10) NOT NULL, 
  PRIMARY KEY (ActID, 
  PaymentDocumentID));
COMMENT ON TABLE Act_PaymentDocument IS '��������� ��������� �� ���� �����-������';
COMMENT ON COLUMN Act_PaymentDocument.ActID IS '������ �� ��� �����-������';
COMMENT ON COLUMN Act_PaymentDocument.PaymentDocumentID IS '������ �� ��������� ��������';
CREATE TABLE TransferAct (
  ID                number(10) NOT NULL, 
  Num               number(10), 
  "Date"            date, 
  TransferActTypeID number(10) NOT NULL, 
  PRIMARY KEY (ID));
COMMENT ON TABLE TransferAct IS '��� ������-�������� - ��� ��������������, ����� ��������� ���� �������� ���������';
COMMENT ON COLUMN TransferAct.ID IS '�������������';
COMMENT ON COLUMN TransferAct.Num IS '�����';
COMMENT ON COLUMN TransferAct."Date" IS '���� ����';
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
CREATE TABLE TransferAct_Act_Document (
  TransferActID number(10) NOT NULL, 
  DocumentID    number(10) NOT NULL, 
  ActID         number(10) NOT NULL, 
  PagesCount    number(10), 
  PRIMARY KEY (TransferActID, 
  DocumentID, 
  ActID));
COMMENT ON TABLE TransferAct_Act_Document IS '����������� ���� �����-������� ���������, ����������� �� ���� �������-�������� (��)';
COMMENT ON COLUMN TransferAct_Act_Document.TransferActID IS '������ �� ��� ������-��������';
COMMENT ON COLUMN TransferAct_Act_Document.DocumentID IS '������ �� ��������';
COMMENT ON COLUMN TransferAct_Act_Document.ActID IS '������ �� ��� �����-�������';
COMMENT ON COLUMN TransferAct_Act_Document.PagesCount IS '���������� �������';
CREATE TABLE TransferActType_Document (
  TransferActTypeID number(10) NOT NULL, 
  DocumentID        number(10) NOT NULL, 
  PRIMARY KEY (TransferActTypeID, 
  DocumentID));
COMMENT ON TABLE TransferActType_Document IS '��� ��������� (��)';
COMMENT ON COLUMN TransferActType_Document.TransferActTypeID IS '������ �� ���';
COMMENT ON COLUMN TransferActType_Document.DocumentID IS '������ �� ��������';
CREATE TABLE Contract_TransferAct_Document (
  ContractDocID number(10) NOT NULL, 
  TransferActID number(10) NOT NULL, 
  DocumentID    number(10) NOT NULL, 
  PagesCount    number(10), 
  PRIMARY KEY (ContractDocID, 
  TransferActID, 
  DocumentID));
COMMENT ON TABLE Contract_TransferAct_Document IS '���������, ��������� � �������� �������� ���� ������-��������';
COMMENT ON COLUMN Contract_TransferAct_Document.ContractDocID IS '������ �� �������';
COMMENT ON COLUMN Contract_TransferAct_Document.TransferActID IS '������ �� ��� �������-��������';
COMMENT ON COLUMN Contract_TransferAct_Document.DocumentID IS '������ �� ��������';
COMMENT ON COLUMN Contract_TransferAct_Document.PagesCount IS '���������� �������';
CREATE TABLE StageResult (
  ID                       number(10) NOT NULL, 
  Name                     nvarchar2(255), 
  EconomicEfficiencyTypeID number(10) NOT NULL, 
  StageID                  number(10) NOT NULL, 
  NTPSubViewID             number(10) NOT NULL, 
  PRIMARY KEY (ID));
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
  PRIMARY KEY (ID));
COMMENT ON TABLE NTPSubView IS '������ ���';
COMMENT ON COLUMN NTPSubView.ID IS '�������������';
COMMENT ON COLUMN NTPSubView.Name IS '�������� ������� ���';
COMMENT ON COLUMN NTPSubView.NTPViewID IS '������ �� ��� ���';
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
CREATE TABLE EfficienceParameter_Type (
  EconomEfficiencyParameterID number(10) NOT NULL, 
  EconomEfficiencyTypeID      number(10) NOT NULL, 
  PRIMARY KEY (EconomEfficiencyParameterID, 
  EconomEfficiencyTypeID));
COMMENT ON TABLE EfficienceParameter_Type IS '�������� ���� ������������� ������������� (��)';
COMMENT ON COLUMN EfficienceParameter_Type.EconomEfficiencyParameterID IS '������ �� �������� ������������� �������������';
COMMENT ON COLUMN EfficienceParameter_Type.EconomEfficiencyTypeID IS '������ �� ��� ������������� �������������';
CREATE TABLE EfficParameter_StageResult (
  EconomEfficiencyParameterID number(10) NOT NULL, 
  StageResultID               number(10) NOT NULL, 
  Value                       number(19, 2), 
  PRIMARY KEY (EconomEfficiencyParameterID, 
  StageResultID));
COMMENT ON TABLE EfficParameter_StageResult IS '�������� ���������� ������������� ������������� ���������� ����� (��)';
COMMENT ON COLUMN EfficParameter_StageResult.EconomEfficiencyParameterID IS '������ �� �������� ������������� ��������������';
COMMENT ON COLUMN EfficParameter_StageResult.StageResultID IS '������ �� ��������� �����';
COMMENT ON COLUMN EfficParameter_StageResult.Value IS '�������� ���������';
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
  GeneralContractDocStageID number(10) NOT NULL, 
  SubContractDocStageID     number(10) NOT NULL, 
  PRIMARY KEY (GeneralContractDocStageID, 
  SubContractDocStageID));
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
INSERT INTO ContractType
  (ID, Name) 
VALUES 
  (1, '�����');
INSERT INTO ContractType
  (ID, Name) 
VALUES 
  (2, '����������� � ������������� ��������');
INSERT INTO ContractType
  (ID, Name) 
VALUES 
  (3, '���');
INSERT INTO ContractType
  (ID, Name) 
VALUES 
  (4, '���������� ������� ������������');
INSERT INTO ContractType
  (ID, Name) 
VALUES 
  (5, '������������');
INSERT INTO ContractType
  (ID, Name) 
VALUES 
  (6, '������');
INSERT INTO Currency
  (ID, Name, Culture, CurrencyI, CurrencyR, CurrencyM, SmallI, SmallR, SmallM, HighSmallName, LowSmallName, Code) 
VALUES 
  (1, '�����', 'ru-ru', '�����', '�����', '������', '�������', '�������', '������', '���.', '���.', 'RUB');
INSERT INTO Currency
  (ID, Name, Culture, CurrencyI, CurrencyR, CurrencyM, SmallI, SmallR, SmallM, HighSmallName, LowSmallName, Code) 
VALUES 
  (2, '������', 'en-us', '������', '�������', '��������', '����', '�����', '������', '���.', '����.', 'USD');
INSERT INTO NDSAlgorithm
  (ID, Name) 
VALUES 
  (1, '� ��� ����� ���');
INSERT INTO NDSAlgorithm
  (ID, Name) 
VALUES 
  (2, '����� ���� ���');
INSERT INTO NDSAlgorithm
  (ID, Name) 
VALUES 
  (3, '��� ���');
INSERT INTO Contractor
  (ID, Name, ShortName, Zip, City, Street, Build, Block, Bank, ContractorTypeID, Appartment, INN, Account, BIK, KPP) 
VALUES 
  (1, '��� ������� ��� �� ��� ', '��� ������� ��� �� ��� ', null, '������', '������������� ��������', '14', null, null, 1, null, null, null, null, null);
INSERT INTO Contractor
  (ID, Name, ShortName, Zip, City, Street, Build, Block, Bank, ContractorTypeID, Appartment, INN, Account, BIK, KPP) 
VALUES 
  (2, '��� ����� "�����"', '��� ����� "�����', null, '�����', '������', '23', null, null, 1, null, null, null, null, null);
INSERT INTO Contractor
  (ID, Name, ShortName, Zip, City, Street, Build, Block, Bank, ContractorTypeID, Appartment, INN, Account, BIK, KPP) 
VALUES 
  (3, '��� ����', '��� ����', null, '���������', '�������������', '34', null, null, 1, null, null, null, null, null);
INSERT INTO Contractor
  (ID, Name, ShortName, Zip, City, Street, Build, Block, Bank, ContractorTypeID, Appartment, INN, Account, BIK, KPP) 
VALUES 
  (4, '��������������� ���������� ��� �������', '��������������� ���������� ��� �������', '116832', '������', '�������', '109�', null, null, 1, null, null, null, null, null);
INSERT INTO Contractor
  (ID, Name, ShortName, Zip, City, Street, Build, Block, Bank, ContractorTypeID, Appartment, INN, Account, BIK, KPP) 
VALUES 
  (5, '���   ��������ѻ', '���   ��������ѻ', '113623', '������', '�������', '23/1', null, null, 1, null, null, null, null, null);
INSERT INTO Contractor
  (ID, Name, ShortName, Zip, City, Street, Build, Block, Bank, ContractorTypeID, Appartment, INN, Account, BIK, KPP) 
VALUES 
  (6, '��� ��������� ��� ���', '��� ��������� ��� ���', '169900', '�������', '���������', '56', null, null, 1, null, null, null, null, null);
INSERT INTO Contractor
  (ID, Name, ShortName, Zip, City, Street, Build, Block, Bank, ContractorTypeID, Appartment, INN, Account, BIK, KPP) 
VALUES 
  (7, '��� "�������"', '��� "�������"', null, '������', '����������', '7', null, null, 3, null, null, null, null, null);
INSERT INTO Contractor
  (ID, Name, ShortName, Zip, City, Street, Build, Block, Bank, ContractorTypeID, Appartment, INN, Account, BIK, KPP) 
VALUES 
  (8, '��� ��� ��������� ��������������� ����������� �����������', '��� ��� ����', '169300', '����', '������������', '13', null, null, 1, null, null, null, null, null);
INSERT INTO Degree
  (ID, Name) 
VALUES 
  (1, '�������� ����');
INSERT INTO Degree
  (ID, Name) 
VALUES 
  (2, '������ ����');
INSERT INTO Person
  (ID, DegreeID, IsContractHeadAuthority, IsActSignAuthority, IsValid, FamilyName, FirstName, MiddleName, Sex, ContractorPositionID) 
VALUES 
  (2, null, 0, 0, 1, '������', '������', '���������', 1, 1);
INSERT INTO Person
  (ID, DegreeID, IsContractHeadAuthority, IsActSignAuthority, IsValid, FamilyName, FirstName, MiddleName, Sex, ContractorPositionID) 
VALUES 
  (3, 1, 0, 0, 1, '���������', '�����', '�������', 0, null);
INSERT INTO Person
  (ID, DegreeID, IsContractHeadAuthority, IsActSignAuthority, IsValid, FamilyName, FirstName, MiddleName, Sex, ContractorPositionID) 
VALUES 
  (4, null, 0, 0, 1, '������', '����', '���������', 1, null);
INSERT INTO Person
  (ID, DegreeID, IsContractHeadAuthority, IsActSignAuthority, IsValid, FamilyName, FirstName, MiddleName, Sex, ContractorPositionID) 
VALUES 
  (5, 2, 0, 0, 1, '������', '�������', '����������', 1, null);
INSERT INTO NDS
  (ID, "Percent", Year) 
VALUES 
  (1, 18, 2004);
INSERT INTO ContractState
  (ID, Name) 
VALUES 
  (1, '�� ��������');
INSERT INTO ContractState
  (ID, Name) 
VALUES 
  (2, '��������');
INSERT INTO CurrencyMeasure
  (ID, Name, Factor) 
VALUES 
  (1, '��.', 1);
INSERT INTO CurrencyMeasure
  (ID, Name, Factor) 
VALUES 
  (2, '���.', 1000);
INSERT INTO CurrencyMeasure
  (ID, Name, Factor) 
VALUES 
  (3, '���.', 1000000);
INSERT INTO ContractDoc
  (ID, Price, ContractTypeID, StartAt, EndsAt, AppliedAt, ApprovedAt, InternalNum, ContractorNum, CurrencyID, OriginContractID, NDSAlgorithmID, PrepaymentSum, PrepaymentPercent, PrepaymentNDSAlgorithmID, NDSID, ContractStateID, CurrencyMeasureID, ContractorID, IsProtectability, Subject, CurrencyRate, RateDate, Description, ContractorPersonID, IsActive, AuthorityID) 
VALUES 
  (1, 2000, 1, '01.01.2010', '31.12.2011', '27.01.2009', '27.01.2010', '1001-10-1', null, 1, null, 1, 600, 30, 1, 1, 1, 1, 7, null, '���������� � ��������� ����� ��������������� ����������� ���������������������� �������. ������� ���������� ������ ������������������', null, null, '������� ����� 3 ������������ �������� ID = 2,3,4 � 1 ���. ���������� ID=5', 5, 1, 1);
INSERT INTO ContractDoc
  (ID, Price, ContractTypeID, StartAt, EndsAt, AppliedAt, ApprovedAt, InternalNum, ContractorNum, CurrencyID, OriginContractID, NDSAlgorithmID, PrepaymentSum, PrepaymentPercent, PrepaymentNDSAlgorithmID, NDSID, ContractStateID, CurrencyMeasureID, ContractorID, IsProtectability, Subject, CurrencyRate, RateDate, Description, ContractorPersonID, IsActive, AuthorityID) 
VALUES 
  (2, 323, 1, '01.10.2010', '18.10.2010', '20.05.2009', '20.05.2009', '124-04-01', null, 1, null, 1, null, null, null, 1, 1, 1, 1, null, '���������� ���������������� ����������', null, null, '������������ ���������� � ID=1', 2, 1, 1);
INSERT INTO ContractDoc
  (ID, Price, ContractTypeID, StartAt, EndsAt, AppliedAt, ApprovedAt, InternalNum, ContractorNum, CurrencyID, OriginContractID, NDSAlgorithmID, PrepaymentSum, PrepaymentPercent, PrepaymentNDSAlgorithmID, NDSID, ContractStateID, CurrencyMeasureID, ContractorID, IsProtectability, Subject, CurrencyRate, RateDate, Description, ContractorPersonID, IsActive, AuthorityID) 
VALUES 
  (3, 500, 1, '10.01.2010', '27.12.2011', '15.03.2009', '15.03.2009', '300-06-1', null, 1, null, 1, null, null, null, 1, 1, 1, 2, null, '���������� ����������� ������������ ��� ��������������� ������������� �������� � ���������� ���������������� � ��������������������� ������������� � ������� �������� ���������������� �������������� ������� � �������������, ������������ � ����������� ������ �������� ����������', null, null, '������������ ���������� � ID=1', 3, 1, 1);
INSERT INTO ContractDoc
  (ID, Price, ContractTypeID, StartAt, EndsAt, AppliedAt, ApprovedAt, InternalNum, ContractorNum, CurrencyID, OriginContractID, NDSAlgorithmID, PrepaymentSum, PrepaymentPercent, PrepaymentNDSAlgorithmID, NDSID, ContractStateID, CurrencyMeasureID, ContractorID, IsProtectability, Subject, CurrencyRate, RateDate, Description, ContractorPersonID, IsActive, AuthorityID) 
VALUES 
  (4, 100, 1, '01.04.2011', '15.05.2011', '28.11.2009', '01.01.2010', '53-06-1', null, 1, null, 1, null, null, null, 1, 1, 1, 3, null, '���������� ���������������� �����', null, null, '������������ ���������� � ID=1', 4, 1, 1);
INSERT INTO ContractDoc
  (ID, Price, ContractTypeID, StartAt, EndsAt, AppliedAt, ApprovedAt, InternalNum, ContractorNum, CurrencyID, OriginContractID, NDSAlgorithmID, PrepaymentSum, PrepaymentPercent, PrepaymentNDSAlgorithmID, NDSID, ContractStateID, CurrencyMeasureID, ContractorID, IsProtectability, Subject, CurrencyRate, RateDate, Description, ContractorPersonID, IsActive, AuthorityID) 
VALUES 
  (5, 2000, 1, '01.01.2010', '31.12.2011', '06.05.2009', '20.05.2009', '1001-10-1/1', null, 1, 1, 1, 600, 30, 1, 1, 1, 1, 7, null, '���������� � ��������� ����� ��������������� ����������� ���������������������� �������. ������� ���������� ������ ������������������', null, null, '���. ���������� � ID=1', 5, 1, 1);
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
  (1, '����');
INSERT INTO PrepaymentDocumentType
  (ID, Name) 
VALUES 
  (2, '����-�������');
INSERT INTO PrepaymentDocumentType
  (ID, Name) 
VALUES 
  (3, '���������');
INSERT INTO TroublesRegistry
  (ID, Name, ShortName, ApprovedAt, OrderNum, ValidFrom, ValidTo) 
VALUES 
  (1, '�������� ������������ ������-����������� ������� ��� "������� �� 2006-2010 ����', '�������� ������� 2006-2010�', '05.01.2006', '01-106', '01.01.2006', '01.01.2010');
INSERT INTO TroublesRegistry
  (ID, Name, ShortName, ApprovedAt, OrderNum, ValidFrom, ValidTo) 
VALUES 
  (2, '�������� ������������ ������-����������� ������� ��� "������� �� 2002-2006 ����', '�������� ������� 2002-2006�', '10.01.2002', '01-342', '01.01.2002', '31.12.2005');
INSERT INTO Trouble
  (ID, Name, Num, TopTroubleID, TroubleRegistryID) 
VALUES 
  (1, '1', '1', null, 1);
INSERT INTO Trouble
  (ID, Name, Num, TopTroubleID, TroubleRegistryID) 
VALUES 
  (2, '����������������� ������� � ������� ������������ ������������� ������ � �������� ������� �������������� ������ ��� ����������� �������� ��������-��������������� ��������� ������.', '1.1', 1, 1);
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
  (5, '�������� ������� � ���������� ��� ��������� ������������� ���������� � ���������� ������������ �������������.', '3.2', 4, 1);
INSERT INTO Trouble
  (ID, Name, Num, TopTroubleID, TroubleRegistryID) 
VALUES 
  (6, '���������� �������, ����������� �������  � ���������� �������� ����������������� � �������������� �������� ���� � ������������� �����������, ������������� ������� � ������ �������� ���������.', '3.4', 4, 1);
INSERT INTO Trouble
  (ID, Name, Num, TopTroubleID, TroubleRegistryID) 
VALUES 
  (7, '4', '4', null, 1);
INSERT INTO Trouble
  (ID, Name, Num, TopTroubleID, TroubleRegistryID) 
VALUES 
  (8, '�������� ����������� ������� � ������� �������������� ���������� ���.', '4.4', 7, 1);
INSERT INTO Trouble
  (ID, Name, Num, TopTroubleID, TroubleRegistryID) 
VALUES 
  (9, '�������� ���������� � ����������� ������� ��� �������������, ������������� � ������������ �������������� ������ � ������������ ����������� ���������� ���� � ������������� � ����������� ������������ �������� � ��������������� ��������', '4.1', 7, 1);
INSERT INTO Trouble
  (ID, Name, Num, TopTroubleID, TroubleRegistryID) 
VALUES 
  (10, ' �������� ���������� � ���������������� ������������ ��� ����������� ��������� ���������������� ���, ������� ������ � �������� ����������� � �������.', '4.2', 7, 1);
INSERT INTO Trouble
  (ID, Name, Num, TopTroubleID, TroubleRegistryID) 
VALUES 
  (11, '5', '5', null, 1);
INSERT INTO Trouble
  (ID, Name, Num, TopTroubleID, TroubleRegistryID) 
VALUES 
  (12, '����������������� ������������ � �������� ����� ��������������� ��������� � ����������� ������� �������� ����������� ��������������� ����� � ����� ������������ � ������ �� ����� ����� ����� ��������� � �����.', '5.2', 11, 1);
INSERT INTO Trouble
  (ID, Name, Num, TopTroubleID, TroubleRegistryID) 
VALUES 
  (13, '�������� ������� ����������� ������������ ������������� ��������� ��������-�������������� �������� � �������������� ����- � ���������������� ������������ ��� "�������"', '5.3', 11, 1);
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
  (16, '����������������� ������� � ������� ������������ �������������� �������� � ���������� ���������.', '7.4', 15, 1);
INSERT INTO Trouble
  (ID, Name, Num, TopTroubleID, TroubleRegistryID) 
VALUES 
  (17, '���������� ����������� ��������������, ������� � ��������� ��������, � ����� ���������� �� ���������� � ����� ��������� �������� ������������� � ���������� ������������ ��������.', '7.5', 15, 1);
INSERT INTO Trouble
  (ID, Name, Num, TopTroubleID, TroubleRegistryID) 
VALUES 
  (18, '8', '8', null, 1);
INSERT INTO Trouble
  (ID, Name, Num, TopTroubleID, TroubleRegistryID) 
VALUES 
  (19, ' �������� ������� ���������� ���������������� ��� ��������', '8.3', 18, 1);
INSERT INTO ContractorType
  (ID, Name) 
VALUES 
  (1, '������ �����������');
INSERT INTO ContractorType
  (ID, Name) 
VALUES 
  (2, '�������� �����������');
INSERT INTO ContractorType
  (ID, Name) 
VALUES 
  (3, '��� "�������"');
INSERT INTO Employee
  (ID, ManagerID, RoleID, Familyname, FirstName, Middlename, Sex) 
VALUES 
  (1, null, 1, '�����', '������', '�������', 1);
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
  (1, '��� ��������������� ��������� 1');
INSERT INTO FunctionalCustomer
  (ID, Name, ContractorID, ParentFunctionalCustomerID, FunctionalCustomerTypeID) 
VALUES 
  (1, '���������� �������������� ��������', 7, null, 1);
INSERT INTO FunctionalCustomer
  (ID, Name, ContractorID, ParentFunctionalCustomerID, FunctionalCustomerTypeID) 
VALUES 
  (2, '����������� �� ���������� ����������', 7, null, 1);
INSERT INTO FunctionalCustomer
  (ID, Name, ContractorID, ParentFunctionalCustomerID, FunctionalCustomerTypeID) 
VALUES 
  (3, '����������� �� ������ ����, �������� ����������, �����', 7, null, 1);
INSERT INTO FunctionalCustomer
  (ID, Name, ContractorID, ParentFunctionalCustomerID, FunctionalCustomerTypeID) 
VALUES 
  (4, '����������� �� ���������������, ���������� �������� � ������������� ����', 7, null, 1);
INSERT INTO FunctionalCustomer
  (ID, Name, ContractorID, ParentFunctionalCustomerID, FunctionalCustomerTypeID) 
VALUES 
  (5, '����������� ��������������� ��������', 7, null, 1);
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
  (1, '������ � ���������� ������');
INSERT INTO ActType
  (ID, ContractorID, TypeName, IsActive) 
VALUES 
  (1, 7, '��� ��������', 1);
INSERT INTO Act
  (ID, Num, "Date", ActTypeID, NDSID, RegionID, TotalSum, SumForTransfer, Status, EnterpriceAuthorityID, CurrencyRate, RateDate) 
VALUES 
  (1, '143-23', '01.01.2011', 1, 1, 1, 200, 200, 1, 1, null, null);
INSERT INTO WorkType
  (ID, Name) 
VALUES 
  (1, '���������� ������������');
INSERT INTO WorkType
  (ID, Name) 
VALUES 
  (2, '���������� ������������ ���������');
INSERT INTO Role
  (ID, Name) 
VALUES 
  (1, '������������ �����������');
INSERT INTO Role
  (ID, Name) 
VALUES 
  (2, '������������� �����������');
INSERT INTO Role
  (ID, Name) 
VALUES 
  (3, '������� �� ����������� ������');
INSERT INTO Authority
  (ID, Name) 
VALUES 
  (1, '��������� ��� �������� 1');
INSERT INTO Authority
  (ID, Name) 
VALUES 
  (2, '��������� ��� ����������� 1');
INSERT INTO Stage
  (ID, Num, Subject, StartsAt, EndsAt, Price, NdsAlgorithmID, ParentID, ActID, ScheduleID) 
VALUES 
  (1, '1', '������ CEN/TS 15399:2007 � CEN/TS 15173:2006 � ����������� ����������� ���������� �� � ������� �����������������. ����������� ��������� � �������� ������� ���������. ���������� ������ ������  ������� ������� ', '05.07.2010 ', '18.10.2010', 200, 1, null, 1, 1);
INSERT INTO Stage
  (ID, Num, Subject, StartsAt, EndsAt, Price, NdsAlgorithmID, ParentID, ActID, ScheduleID) 
VALUES 
  (2, '2', '���������� ������ �������� ������� ��������� ���������������������� �������. ������� ���������� ������ ������������������ ���������� ������ �������� ������� ��������� ������� �������. ', '18.10.2010', '27.12.2010', 600, 1, null, 1, 1);
INSERT INTO Stage
  (ID, Num, Subject, StartsAt, EndsAt, Price, NdsAlgorithmID, ParentID, ActID, ScheduleID) 
VALUES 
  (3, '3', '���������� ����������� � ���������� ��������� � ��� ����������. ���������� ������� ��������� � �������������� ������� ������ ����������� � ����������-�������� ����� ��� ���������� ���������� ����������. ���� ��������� � �����������, ���������� ��� ��������� ���������� ������� ���������. ', '10.01.2011', '15.05.2011', 200, 1, null, null, 1);
INSERT INTO Stage
  (ID, Num, Subject, StartsAt, EndsAt, Price, NdsAlgorithmID, ParentID, ActID, ScheduleID) 
VALUES 
  (4, '4', '�������� ��������� � ���������� � ������ � ���������� ������������� �������� ���������. ���������� ������������� �������� ������� ��������� ������� �������. ', '01.04.2011', '30.09.2011', 600, 1, null, null, 1);
INSERT INTO Stage
  (ID, Num, Subject, StartsAt, EndsAt, Price, NdsAlgorithmID, ParentID, ActID, ScheduleID) 
VALUES 
  (5, '5', '�������� � �� 4 ������� ��������� ���������������������� �������. ������� ���������� ������ ������������������ ��� ����������� ���������� ������������� �� 23.', '01.10.2011', '25.12.2011', 400, 1, null, null, 1);
INSERT INTO Stage
  (ID, Num, Subject, StartsAt, EndsAt, Price, NdsAlgorithmID, ParentID, ActID, ScheduleID) 
VALUES 
  (6, '1.1', '������ CEN/TS 15399:2007 � CEN/TS 15173:2006 � ����������� ����������� ���������� �� � ������� �����������������. ', '05.07.2010', '01.09.2010', 90, 1, 9, null, 2);
INSERT INTO Stage
  (ID, Num, Subject, StartsAt, EndsAt, Price, NdsAlgorithmID, ParentID, ActID, ScheduleID) 
VALUES 
  (7, '1.2', '����������� ��������� � �������� ������� ���������.', '01.09.2010', '01.10.2010', 10, 1, 9, null, 2);
INSERT INTO Stage
  (ID, Num, Subject, StartsAt, EndsAt, Price, NdsAlgorithmID, ParentID, ActID, ScheduleID) 
VALUES 
  (8, '1.3', '���������� ������ ������  ������� �������.', '01.10.2010', '18.10.2010', 10, 1, null, null, 2);
INSERT INTO Stage
  (ID, Num, Subject, StartsAt, EndsAt, Price, NdsAlgorithmID, ParentID, ActID, ScheduleID) 
VALUES 
  (9, '1', '������ CEN/TS 15399:2007 � CEN/TS 15173:2006 � ����������� ����������� ���������� �� ', '05.07.2010', '18.10.2010', 110, 1, null, null, 2);
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
  (1, '�������');
INSERT INTO Document
  (ID, Name) 
VALUES 
  (3, '����������� ����');
INSERT INTO Document
  (ID, Name) 
VALUES 
  (4, '�������� ����');
INSERT INTO Document
  (ID, Name) 
VALUES 
  (5, '���� ������������');
INSERT INTO Document
  (ID, Name) 
VALUES 
  (6, '�������� �����������');
INSERT INTO Document
  (ID, Name) 
VALUES 
  (7, '�������� ������������ �����������');
INSERT INTO Document
  (ID, Name) 
VALUES 
  (8, '������');
INSERT INTO NTPView
  (ID, Name) 
VALUES 
  (1, '���������');
INSERT INTO NTPView
  (ID, Name) 
VALUES 
  (2, '����������-������������ ������������');
INSERT INTO NTPSubView
  (ID, Name, NTPViewID) 
VALUES 
  (1, '���������', 1);
INSERT INTO NTPSubView
  (ID, Name, NTPViewID) 
VALUES 
  (2, '���', 2);
INSERT INTO NTPSubView
  (ID, Name, NTPViewID) 
VALUES 
  (3, '������������ ������������', 2);
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
  (1, '��������');
INSERT INTO Position
  (ID, Name) 
VALUES 
  (2, '��������� ������ ��������');
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

