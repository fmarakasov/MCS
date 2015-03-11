-- This query will display the current date

SELECT SYSDATE FROM DUAL;
ALTER TABLE Responsible DROP CONSTRAINT FKResponsibl163229;
ALTER TABLE Responsible ADD CONSTRAINT FKResponsibl163229 FOREIGN KEY (DisposalID) REFERENCES Disposal (ID) ON DELETE Cascade;

DROP TABLE ResponsibleAssignmentOrder CASCADE CONSTRAINTS;
DROP TABLE ResponsibleForOrder CASCADE CONSTRAINTS;
DROP SEQUENCE seq_ResponsibleAssignmentOrder;
DROP SEQUENCE seq_ResponsibleForOrder;


CREATE TABLE ResponsibleForOrder (
  ID                           number(10) NOT NULL, 
  DepartmentID                 number(10) NOT NULL, 
  EmployeeID                   number(10) NOT NULL, 
  ResponsibleAssignmentOrderID number(10), 
  PRIMARY KEY (ID));
COMMENT ON TABLE ResponsibleForOrder IS 'Ответственный по приказу';
CREATE TABLE ResponsibleAssignmentOrder (
  ID        number(10) NOT NULL, 
  OrderNum  nvarchar2(20), 
  OrderDate date, 
  PRIMARY KEY (ID));

ALTER TABLE ResponsibleForOrder ADD CONSTRAINT ResponsibleForOrder_FK FOREIGN KEY (ResponsibleAssignmentOrderID) REFERENCES ResponsibleAssignmentOrder (ID) ON DELETE Cascade;
ALTER TABLE ResponsibleForOrder ADD CONSTRAINT DepartmentResponsibleOrder_FK FOREIGN KEY (DepartmentID) REFERENCES Department (ID);
ALTER TABLE ResponsibleForOrder ADD CONSTRAINT EmployeeResponsibleOrder_FK FOREIGN KEY (EmployeeID) REFERENCES Employee (ID);

CREATE SEQUENCE seq_ResponsibleAssignmentOrder start with 10000;
CREATE SEQUENCE seq_ResponsibleForOrder START WITH 10000;
