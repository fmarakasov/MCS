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
COMMENT ON TABLE ApprovalProcess IS '������ ������������ ��������';
COMMENT ON COLUMN ApprovalProcess.ToLocationID IS '���� ��������';
COMMENT ON COLUMN ApprovalProcess.FromLocationId IS '��� �������';
COMMENT ON COLUMN ApprovalProcess.TransferStateAt IS '���� �������� ��������� � ���������';
COMMENT ON COLUMN ApprovalProcess.EnterStateAt IS '���� ����������� ��������� � ���������';
COMMENT ON COLUMN ApprovalProcess.MissiveId IS '����� ������';
COMMENT ON COLUMN ApprovalProcess.MissiveDate IS '���� ������';
COMMENT ON COLUMN ApprovalProcess.Description IS '����������';

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

INSERT INTO MissiveType
  (ID, Name) 
VALUES 
  (1, '���.');
INSERT INTO MissiveType
  (ID, Name) 
VALUES 
  (2, '��.');
INSERT INTO MissiveType
  (ID, Name) 
VALUES 
  (-1, '(�� ������)');
INSERT INTO ApprovalGoal
  (ID, Name) 
VALUES 
  (1, '����������� ��������� ��');
INSERT INTO ApprovalGoal
  (ID, Name) 
VALUES 
  (2, '����������� ��������� ���');
INSERT INTO ApprovalGoal
  (ID, Name) 
VALUES 
  (3, '����������� ������ ���������');
INSERT INTO ApprovalGoal
  (ID, Name) 
VALUES 
  (4, '����������� ���������� ���������� ���������');
INSERT INTO ApprovalGoal
  (ID, Name) 
VALUES 
  (-1, '(�� ������)');

ALTER TABLE ApprovalProcess ADD CONSTRAINT FKApprovalPr530332 FOREIGN KEY (ContractDocID) REFERENCES ContractDoc (ID);
ALTER TABLE ApprovalProcess ADD CONSTRAINT FKApprovalPr495868 FOREIGN KEY (ToLocationID) REFERENCES Location (ID);
ALTER TABLE ApprovalProcess ADD CONSTRAINT FKApprovalPr619985 FOREIGN KEY (ApprovalStateID) REFERENCES ApprovalState (ID);
ALTER TABLE ApprovalProcess ADD CONSTRAINT FKApprovalPr874000 FOREIGN KEY (FromLocationId) REFERENCES Location (ID);
ALTER TABLE ApprovalProcess ADD CONSTRAINT FKApprovalPr803309 FOREIGN KEY (MissiveTypeID) REFERENCES MissiveType (ID);
ALTER TABLE ApprovalProcess ADD CONSTRAINT FKApprovalPr262517 FOREIGN KEY (ApprovalGoalID) REFERENCES ApprovalGoal (ID);

alter table act add IsSigned number(1) DEFAULT '0';