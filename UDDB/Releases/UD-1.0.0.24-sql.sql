/* 
	UPDATE TO 1.0.0.23
*/

ALTER TABLE Stage MODIFY Subject NVARCHAR2(2000);


UPDATE Act SET SignDate=CURRENT_DATE WHERE SignDate IS NULL; 
UPDATE Act SET TotalSum=0 WHERE TotalSum IS NULL;
UPDATE Act SET Sumfortransfer=0 WHERE Sumfortransfer IS NULL;
UPDATE Act SET NdsalgorithmID=1 WHERE NdsalgorithmID IS NULL;
UPDATE Act SET CurrencyID=1 WHERE CurrencyID IS NULL;
UPDATE Act SET CurrencymeasureID=1 WHERE CurrencymeasureID IS NULL;


ALTER TABLE Act MODIFY TotalSum NUMBER(18,2) NOT NULL;
ALTER TABLE Act MODIFY SignDate DATE NOT NULL;
ALTER TABLE Act MODIFY RegionID NUMBER(10) NULL;
ALTER TABLE Act MODIFY EnterpriceAuthorityID NUMBER(10);
ALTER TABLE Act MODIFY NdsalgorithmID NUMBER(10) NOT NULL;
ALTER TABLE Act MODIFY CurrencyID NUMBER(10) NOT NULL;
ALTER TABLE Act MODIFY CurrencymeasureID NUMBER(10) NOT NULL;
ALTER TABLE Act MODIFY Sumfortransfer NUMBER(18,2) NOT NULL;

/*
      UPDATE TO 1.0.0.24
*/
ALTER TABLE Contractdoc MODIFY Subject nvarchar2(2000) NULL; 
ALTER TABLE Employee MODIFY Familyname nvarchar2(40) NULL; 

ALTER TABLE Stage MODIFY  Num            nvarchar2(25) NULL;
ALTER TABLE Stage MODIFY Subject        nvarchar2(2000) NULL; 
ALTER TABLE Stage MODIFY StartsAt       date NULL; 
ALTER TABLE Stage MODIFY EndsAt         date NULL; 
ALTER TABLE Stage MODIFY Price          number(18, 2) NULL; 

ALTER TABLE Act MODIFY SignDate  date NULL; 

ALTER TABLE Act MODIFY TotalSum              number(18, 2) NULL; 
ALTER TABLE Act MODIFY SumForTransfer        number(18, 2) NULL; 

INSERT INTO ContractType  (ID, Name, ReportOrder) VALUES   (-1, '(�� ��������)', null);
INSERT INTO Currency  (ID, Name, Culture, CurrencyI, CurrencyR, CurrencyM, SmallI, SmallR, SmallM, HighSmallName, LowSmallName, Code) 
	VALUES (-1, '(�� ��������)', 'ru-ru', null, null, null, null, null, null, null, null, null);
INSERT INTO NDSAlgorithm  (ID, Name, PriceTooltip) VALUES   (-1, '(�� ��������)', '');
INSERT INTO ContractorType  (ID, Name, ReportOrder) VALUES   (-1, '(�� ��������)', 4);
INSERT INTO Contractor  (ID, Name, ShortName, Zip, City, Street, Build, Block, Bank, ContractorTypeID, Appartment, INN, Account, BIK, KPP) 
	VALUES (-1, '�� ��������', null, null, null, null, null, null, null, -1, null, null, null, null, null);
INSERT INTO Degree  (ID, Name) VALUES (-1, '(�� ��������)');
INSERT INTO Person  (ID, DegreeID, IsContractHeadAuthority, IsActSignAuthority, IsValid, FamilyName, FirstName, MiddleName, Sex, ContractorPositionID) 
	VALUES (-1, -1, 0, 0, 0, '(��� ������)', '(��� ������)', '(��� ������)', 1, null);
INSERT INTO ContractState  (ID, Name) VALUES (-1, '(�� ��������)');
INSERT INTO CurrencyMeasure  (ID, Name, Factor) VALUES (-1, '(�� ����������)', 1);
INSERT INTO PrepaymentDocumentType  (ID, Name) VALUES   (-1, '(�� ��������)');
INSERT INTO Role  (ID, Name) VALUES   (-1, '(�� ����������)');
INSERT INTO Employee  (ID, ManagerID, RoleID, Familyname, FirstName, Middlename, Sex) VALUES  (-1, null, -1, '', '', '', 1);
INSERT INTO Authority  (ID, Name) VALUES   (-1, '(�� ��������)');
INSERT INTO EnterpriseAuthority  (ID, Num, ValidFrom, ValidTo, IsValid, EmployeeID, AuthorityID) VALUES (-1, '(�� ��������)', null, null, null, -1, -1);

INSERT INTO Property  (ID, Name) VALUES (-1, '(�� ��������)');
INSERT INTO FunctionalCustomerType  (ID, Name) VALUES  (-1, '(�� ��������)');
INSERT INTO Region  (ID, Name) VALUES (-1, '(�� ��������)');
INSERT INTO ActType  (ID, ContractorID, TypeName, IsActive) VALUES  (-1, -1, '(�� ��������)', null);
INSERT INTO WorkType  (ID, Name, ShortName) VALUES (-1, '(�� ��������)', '');
INSERT INTO ActPaymentDocument  (ActID, PaymentDocumentID, ID) VALUES   (1, 1, 1);
INSERT INTO TransferActType  (ID, Name) VALUES (-1, '(�� ��������)');
INSERT INTO Document  (ID, Name) VALUES (-1, '(�� ��������)');
INSERT INTO NTPView  (ID, Name) VALUES  (-1, '(�� ����������)');
INSERT INTO NTPSubView  (ID, Name, NTPViewID, ShortName) VALUES  (-1, '(�� ����������)', -1, null);
INSERT INTO EconomEfficiencyType  (ID, Name) VALUES   (-1, '(�� ����������)');
INSERT INTO EconomEfficiencyParameter  (ID, Name) VALUES   (-1, '(�� ����������)');
INSERT INTO Position  (ID, Name) VALUES (-1, '(�� ��������)');
INSERT INTO ContractorPosition  (ID, ContractorID, PositionID) VALUES   (-1, -1, -1);



UPDATE Act SET Region=-1 WHERE RegionID IS NULL;
UPDATE Act SET EnterpriceAuthorityID=-1 WHERE EnterpriceAuthorityID IS NULL;

ALTER TABLE Act MODIFY RegionID              number(10) NOT NULL; 
ALTER TABLE Act MODIFY EnterpriceAuthorityID number(10) NOT NULL;

CREATE SEQUENCE seq_IMPORTINGSCHEMEITEM START WITH 10000;
CREATE SEQUENCE seq_IMPORTINGSCHEME START WITH 10000;
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
CREATE TABLE IMPORTINGSCHEME (
  ID         number(10) NOT NULL, 
  Schemename nvarchar2(500) NOT NULL UNIQUE, 
  PRIMARY KEY (ID));
COMMENT ON TABLE IMPORTINGSCHEME IS '����� �������';
COMMENT ON COLUMN IMPORTINGSCHEME.ID IS '������������� ����� �������';
COMMENT ON COLUMN IMPORTINGSCHEME.Schemename IS '��� ����� �������';
ALTER TABLE IMPORTINGSCHEMEITEM ADD CONSTRAINT FKIMPORTINGS434803 FOREIGN KEY (SchemeID) REFERENCES IMPORTINGSCHEME (ID);




