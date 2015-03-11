-- This query will display the current date

DROP TABLE APPROVALPROCESS;

CREATE SEQUENCE seq_ApprovalProcess START WITH 10000;

CREATE TABLE ApprovalProcess (
  ID              number(10) NOT NULL, 
  ContractDocID   number(10) NOT NULL, 
  LocationID      number(10) NOT NULL, 
  ApprovalStateID number(10) NOT NULL, 
  EnterStateAt    date NOT NULL, 
  MissiveId       nvarchar2(10), 
  MissiveDate     date, 
  Description     nvarchar2(1000), 
  PRIMARY KEY (ID), 
  CONSTRAINT U_APPROVALS 
    UNIQUE (ContractDocID, LocationID, ApprovalStateID, EnterStateAt));

COMMENT ON TABLE ApprovalProcess IS 'Журнал согласования договора';
COMMENT ON COLUMN ApprovalProcess.EnterStateAt IS 'Дата поступления документа в состояние';
COMMENT ON COLUMN ApprovalProcess.MissiveId IS 'Номер письма';
COMMENT ON COLUMN ApprovalProcess.MissiveDate IS 'Дата письма';
COMMENT ON COLUMN ApprovalProcess.Description IS 'Примечание';

ALTER TABLE ApprovalProcess ADD CONSTRAINT FKApprovalPr530332 FOREIGN KEY (ContractDocID) REFERENCES ContractDoc (ID);
ALTER TABLE ApprovalProcess ADD CONSTRAINT FKApprovalPr619985 FOREIGN KEY (ApprovalStateID) REFERENCES ApprovalState (ID);
ALTER TABLE ApprovalProcess ADD CONSTRAINT FKApprovalPr586937 FOREIGN KEY (LocationID) REFERENCES Location (ID);
