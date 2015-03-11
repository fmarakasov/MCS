DROP TABLE ApprovalProcess CASCADE CONSTRAINTS;
CREATE SEQUENCE seq_ApprovalGoal START WITH 10000;
CREATE SEQUENCE seq_MissiveType START WITH 10000;
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

INSERT INTO MissiveType
  (ID, Name) 
VALUES 
  (1, 'Исх.');
INSERT INTO MissiveType
  (ID, Name) 
VALUES 
  (2, 'Вх.');
INSERT INTO MissiveType
  (ID, Name) 
VALUES 
  (-1, '(не задано)');
INSERT INTO ApprovalGoal
  (ID, Name) 
VALUES 
  (1, 'Исправление замечаний ФЗ');
INSERT INTO ApprovalGoal
  (ID, Name) 
VALUES 
  (2, 'Исправление замечаний УИР');
INSERT INTO ApprovalGoal
  (ID, Name) 
VALUES 
  (3, 'Исправление устных замечаний');
INSERT INTO ApprovalGoal
  (ID, Name) 
VALUES 
  (4, 'Исправление официально переданных замечаний');
INSERT INTO ApprovalGoal
  (ID, Name) 
VALUES 
  (-1, '(не задано)');

ALTER TABLE ApprovalProcess ADD CONSTRAINT FKApprovalPr530332 FOREIGN KEY (ContractDocID) REFERENCES ContractDoc (ID);
ALTER TABLE ApprovalProcess ADD CONSTRAINT FKApprovalPr495868 FOREIGN KEY (ToLocationID) REFERENCES Location (ID);
ALTER TABLE ApprovalProcess ADD CONSTRAINT FKApprovalPr619985 FOREIGN KEY (ApprovalStateID) REFERENCES ApprovalState (ID);
ALTER TABLE ApprovalProcess ADD CONSTRAINT FKApprovalPr874000 FOREIGN KEY (FromLocationId) REFERENCES Location (ID);
ALTER TABLE ApprovalProcess ADD CONSTRAINT FKApprovalPr803309 FOREIGN KEY (MissiveTypeID) REFERENCES MissiveType (ID);
ALTER TABLE ApprovalProcess ADD CONSTRAINT FKApprovalPr262517 FOREIGN KEY (ApprovalGoalID) REFERENCES ApprovalGoal (ID);

alter table act add IsSigned number(1) DEFAULT '0';