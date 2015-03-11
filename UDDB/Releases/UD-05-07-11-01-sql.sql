/*
     UPDATE TO 1.0.0.26
*/

/* SEQUENCES */
CREATE SEQUENCE seq_Post START WITH 10000;
CREATE SEQUENCE seq_Location START WITH 10000;
CREATE SEQUENCE seq_ApprovalState START WITH 10000;
CREATE SEQUENCE seq_Department START WITH 10000;
CREATE SEQUENCE seq_Time_Dim START WITH 10000;

/* CONTRACTDOC */
--ALTER TABLE ContractDoc MODIFY  ContractStateID    NUMBER(10) DEFAULT -1 NOT NULL;
ALTER TABLE ContractDoc ADD DepartmentID number(10) DEFAULT -1;
ALTER TABLE ContractDoc ADD AgreementNum number(4);
COMMENT ON COLUMN ContractDoc.AgreementNum IS 'Номер соглашения к договору. Используется только в ДС в случае, если автоматически вычисленный номер ДС не подходит';

/*
    CONTRACTOR  
    В новой схеме город, улица, квартира, корпус контрагента могут быть удалены, но пока остаются для совместимости.
    Вместо этих отдельных атрибутов введён Address
*/
ALTER TABLE Contractor ADD OGRN nvarchar2(13);
ALTER TABLE Contractor ADD OKONH nvarchar2(5);
ALTER TABLE Contractor ADD OKPO nvarchar2(8);
ALTER TABLE Contractor ADD CorrespAccount nvarchar2(20);
ALTER TABLE Contractor ADD Address nvarchar2(1000);
ALTER TABLE Contractor MODIFY KPP nvarchar2(9);
ALTER TABLE Contractor MODIFY BIK nvarchar2(9);
ALTER TABLE Contractor MODIFY Account nvarchar2(20);
ALTER TABLE Contractor MODIFY INN nvarchar2(10);
COMMENT ON COLUMN Contractor.Account IS 'Расчётный счёт';
COMMENT ON COLUMN Contractor.Address IS 'Адрес контрагента';
COMMENT ON COLUMN Contractor.CorrespAccount IS 'Корреспондентский счёт';
COMMENT ON COLUMN Contractor.OKPO IS 'ОКПО';
COMMENT ON COLUMN Contractor.OKONH IS 'ОКОНХ';
COMMENT ON COLUMN Contractor.OGRN IS 'ОГРН';

/* EMPLOYEE
   FKEmployee471769 --FOREIGN KEY (ManagerID) REFERENCES Employee (ID);
   FKEmployee880788 --FOREIGN KEY (RoleID) REFERENCES Role (ID) ON DELETE Cascade; 
*/
ALTER TABLE Employee DROP CONSTRAINT FKEmployee471769; 
ALTER TABLE Employee DROP CONSTRAINT FKEmployee880788; 
ALTER TABLE Employee ADD PostID number(10) DEFAULT -1 NOT NULL;
ALTER TABLE Employee ADD DepartmentID number(10) DEFAULT -1 NOT NULL;
ALTER TABLE Employee DROP COLUMN ManagerID;
ALTER TABLE Employee DROP COLUMN RoleID;

/* DISPOSAL 
  FKDisposal292972 -- FOREIGN KEY (ContractdocID) REFERENCES ContractDoc (ID);
 */
ALTER TABLE Disposal DROP CONSTRAINT FKDisposal292972; 
ALTER TABLE Disposal DROP COLUMN ContractdocID;

/* RESPONSIBLE */
ALTER TABLE Responsible ADD RoleID number(10) DEFAULT -1 NOT NULL;
ALTER TABLE Responsible ADD ContractdocID number(10);
COMMENT ON COLUMN Responsible.RoleID IS 'Ссылка на роль';
COMMENT ON COLUMN Responsible.ContractdocID IS 'Ссылка на договор';

/*EFPARAMETERSTAGERESULT*/
ALTER TABLE EfParameterStageResult MODIFY Value number(19, 6);

/* DEPARTMENT */
CREATE TABLE Department (
  ID           number(10) NOT NULL, 
  ParentID     number(10), 
  ManagerID    number(10) NOT NULL, 
  DirectedByID number(10) NOT NULL, 
  Name         nvarchar2(200) NOT NULL UNIQUE, 
  PRIMARY KEY (ID));

/* POST */
CREATE TABLE Post (
  ID   number(10) NOT NULL, 
  Name nvarchar2(200) NOT NULL UNIQUE, 
  PRIMARY KEY (ID));
COMMENT ON TABLE Post IS 'Должность';

/* RESPONSIBLEFORORDER */
CREATE TABLE ResponsibleForOrder (
  DepartmentID number(10) NOT NULL, 
  EmployeeID   number(10) NOT NULL, 
  PRIMARY KEY (DepartmentID, 
  EmployeeID));
COMMENT ON TABLE ResponsibleForOrder IS 'Ответственный по приказу';

/* LOCATION */
CREATE TABLE Location (
  ID   number(10) NOT NULL, 
  Name nvarchar2(200) NOT NULL UNIQUE, 
  PRIMARY KEY (ID));
COMMENT ON TABLE Location IS 'Место нахождения';
COMMENT ON COLUMN Location.Name IS 'Название места нахождения';

/* APPROVALSTATE */
CREATE TABLE ApprovalState (
  ID   number(10) NOT NULL, 
  Name nvarchar2(200) NOT NULL UNIQUE, 
  PRIMARY KEY (ID));
COMMENT ON TABLE ApprovalState IS 'Состояние согласования документа';
COMMENT ON COLUMN ApprovalState.Name IS 'Название состояния согласования';

/* APPROVALPROCESS */
CREATE TABLE ApprovalProcess (
  ContractDocID   number(10) NOT NULL, 
  LocationID      number(10) NOT NULL, 
  ApprovalStateID number(10) NOT NULL, 
  EnterStateAt    date NOT NULL, 
  MissiveId       nvarchar2(10), 
  MissiveDate     date, 
  Description     nvarchar2(1000), 
  PRIMARY KEY (ContractDocID, 
  LocationID, 
  ApprovalStateID, 
  EnterStateAt));
COMMENT ON TABLE ApprovalProcess IS 'Журнал согласования договора';
COMMENT ON COLUMN ApprovalProcess.EnterStateAt IS 'Дата поступления документа в состояние';
COMMENT ON COLUMN ApprovalProcess.MissiveId IS 'Номер письма';
COMMENT ON COLUMN ApprovalProcess.MissiveDate IS 'Дата письма';
COMMENT ON COLUMN ApprovalProcess.Description IS 'Примечание';

/* 
	CONTRACTDOC_FUNDS_FACT 
	Предполагается, что Contractdoc_Funds_Fact будет содержать данные о сумме средств (всего, остаток, выполнено) по договору на определённый момент времени (сейчас пока на год).
	Данные будут обновляться тригером при модификации данных договора/КП. Требуется, что бы оперативно получать отчётные формы, отображать реестр. Пока не испрользуется.

*/
CREATE TABLE Contractdoc_Funds_Fact (
  ContractdocID  number(10) NOT NULL, 
  Time_DimID     number(10) NOT NULL, 
  FundsDisbursed number(18, 2) NOT NULL, 
  FundsLeft      number(18, 2) NOT NULL, 
  FundsTotal     number(18, 2) NOT NULL, 
  PRIMARY KEY (ContractdocID, 
  Time_DimID));

/* TIME_DIM */
CREATE TABLE Time_Dim (
  ID   number(10) NOT NULL, 
  Year number(5) NOT NULL, 
  PRIMARY KEY (ID));

INSERT INTO PrepaymentDocumentType
  (ID, Name) 
VALUES 
  (4, 'Платёжное поручение');
INSERT INTO Post
  (ID, Name) 
VALUES 
  (-1, '(Не задана)');
INSERT INTO Department
  (ID, ParentID, ManagerID, DirectedByID, Name) 
VALUES 
  (-1, null, -1, -1, '(Не задан)');
INSERT INTO Location
  (ID, Name) 
VALUES 
  (-1, '(Не задано)');
INSERT INTO Location
  (ID, Name) 
VALUES 
  (1, 'УИР');
INSERT INTO Location
  (ID, Name) 
VALUES 
  (2, 'Исполнитель');
INSERT INTO ApprovalState
  (ID, Name) 
VALUES 
  (-1, '(Не задано)');
INSERT INTO ApprovalState
  (ID, Name) 
VALUES 
  (1, 'Согласование');
INSERT INTO ApprovalState
  (ID, Name) 
VALUES 
  (2, 'Подписано');
INSERT INTO ApprovalState
  (ID, Name) 
VALUES 
  (3, 'Исправление');

ALTER TABLE ContractDoc ADD CONSTRAINT ContractdocDepartment_FK FOREIGN KEY (DepartmentID) REFERENCES Department (ID);
ALTER TABLE Department ADD CONSTRAINT ParentDepartment_FK FOREIGN KEY (ParentID) REFERENCES Department (ID);
ALTER TABLE Employee ADD CONSTRAINT WorksAt_FK FOREIGN KEY (DepartmentID) REFERENCES Department (ID);
ALTER TABLE Employee ADD CONSTRAINT EmployeePost_FK FOREIGN KEY (PostID) REFERENCES Post (ID);
ALTER TABLE ResponsibleForOrder ADD CONSTRAINT DepartmentResponsibleOrder_FK FOREIGN KEY (DepartmentID) REFERENCES Department (ID);
ALTER TABLE ResponsibleForOrder ADD CONSTRAINT EmployeeResponsibleOrder_FK FOREIGN KEY (EmployeeID) REFERENCES Employee (ID);
ALTER TABLE Department ADD CONSTRAINT ManagedBy_FK FOREIGN KEY (ManagerID) REFERENCES Employee (ID);
ALTER TABLE Department ADD CONSTRAINT DirectedBy_FK FOREIGN KEY (DirectedByID) REFERENCES Employee (ID);
ALTER TABLE Responsible ADD CONSTRAINT ResponsibleRole_FK FOREIGN KEY (RoleID) REFERENCES Role (ID);
ALTER TABLE Responsible ADD CONSTRAINT ContractdocResponsible_FK FOREIGN KEY (ContractdocID) REFERENCES ContractDoc (ID);
ALTER TABLE ApprovalProcess ADD CONSTRAINT FKApprovalPr530332 FOREIGN KEY (ContractDocID) REFERENCES ContractDoc (ID);
ALTER TABLE ApprovalProcess ADD CONSTRAINT FKApprovalPr586937 FOREIGN KEY (LocationID) REFERENCES Location (ID);
ALTER TABLE ApprovalProcess ADD CONSTRAINT FKApprovalPr619985 FOREIGN KEY (ApprovalStateID) REFERENCES ApprovalState (ID);
ALTER TABLE Contractdoc_Funds_Fact ADD CONSTRAINT FKContractdo229014 FOREIGN KEY (ContractdocID) REFERENCES ContractDoc (ID);
ALTER TABLE Contractdoc_Funds_Fact ADD CONSTRAINT FKContractdo589247 FOREIGN KEY (Time_DimID) REFERENCES Time_Dim (ID);
ALTER TABLE Responsible ADD CONSTRAINT ResponsibleRole_FK FOREIGN KEY (RoleID) REFERENCES Role (ID);

