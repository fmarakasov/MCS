-- This query will display the current date

SELECT SYSDATE FROM DUAL;

ALTER TABLE CONTRACTDOC DROP COLUMN ISACTIVE;
ALTER TABLE ResponsibleForOrder add ContractTypeID number(10);
ALTER TABLE ResponsibleForOrder ADD CONSTRAINT ContractTypeRespForOrder_FK FOREIGN KEY (ContractTypeID) REFERENCES ContractType (ID);

CREATE OR REPLACE NOFORCE VIEW "CONTRACTREPOSITORYVIEW"
  AS SELECT c.ID, c.Price, n.Percents as "NdsPercents", cr.Culture, cm.Factor, c.NDSAlgorithmID, alg.Name as "NDSAlgName", n.ID as "NDSID", cr.ID as "CurrencyID", cm.ID as "CurrencyMeasureID", c.CurrencyRate, c.StartAt, 
  c.EndsAt, c.AppliedAt, c.ApprovedAt, c.InternalNum, c.Subject, c.BrokeAt, c.OutOfControlAt, c.ReallyFinishedAt, c.ContractStateID, cs.Name as "StateName", c.Deleted, c.Origincontractid,
  ct.Name as "ContractTypeName", ctr.ShortName as "ContractorShortName", ctt.Name as "ContractorType", p.FamilyName as "PersonFamilyName",
  p.FirstName as "PersonFirstName", p.MiddleName as "PersonMiddleName", d.ID as "DegreeId", d.Name as "PersonDegree", rol.Name as "RoleName",
  e.FamilyName as "EmployeeFamilyName", e.FirstName as "EmployeeFirstName", e.MiddleName as "EmployeeMiddleName", pt.Name as "EmployeePost", pt.ID as "EmployeepostId",
  (select count(*) from "UD".Contractdoc c2 where c2.origincontractid = c.id) as "AgreementReferenceCount",
   (select count(*) from ContractHierarchy ch where ch.subcontractdocid = c.id) as "GeneralReferenceCount"
   FROM "UD".Contractdoc c 
   LEFT JOIN "UD".Contractstate cs ON cs.ID = c.ContractStateID
   LEFT JOIN "UD".Currencymeasure cm ON cm.ID = c.CurrencyMeasureId
   LEFT JOIN "UD".Currency cr ON cr.ID = c.CurrencyID
   LEFT JOIN "UD".NDS n ON n.ID = c.NDSID
   LEFT JOIN "UD".NDSAlgorithm alg ON alg.ID = c.NDSAlgorithmID
LEFT JOIN "UD".ContractType ct ON c.ContractTypeID = ct.ID
LEFT JOIN "UD".Contractor ctr ON c.ContractorID = ctr.ID
LEFT JOIN "UD".ContractorType ctt ON ctr.ContractorTypeID = ctt.ID
LEFT JOIN "UD".Person p ON p.ID = c.ContractorPersonID
LEFT OUTER JOIN "UD".Degree d ON d.ID = p.DegreeID
LEFT OUTER JOIN "UD".Responsible r ON r.ContractdocID = c.ID AND r.RoleID=1
LEFT JOIN "UD".Role rol ON rol.ID=r.RoleID
LEFT JOIN "UD".Employee e ON e.ID = r.EmployeeID
LEFT JOIN "UD".Post pt ON pt.ID = e.PostID 
ORDER BY c.ContractTypeID, c.InternalNum;


delete from contractdoc where not (origincontractid in (select id from contractdoc));
ALTER TABLE StageResult ADD CONSTRAINT FKStageResul876631 FOREIGN KEY (StageID) REFERENCES Stage (ID) ON DELETE Cascade;
ALTER TABLE ContractDoc ADD CONSTRAINT FKContractDo906594 FOREIGN KEY (ContractorID) REFERENCES Contractor (ID) ON DELETE Cascade;
ALTER TABLE ScheduleContract ADD CONSTRAINT FKScheduleCo823189 FOREIGN KEY (ContractDocID) REFERENCES ContractDoc (ID) ON DELETE Cascade;
ALTER TABLE ContractPayment ADD CONSTRAINT FKContractPa457777 FOREIGN KEY (ContractDocID) REFERENCES ContractDoc (ID);
ALTER TABLE Prepayment ADD CONSTRAINT FKPrepayment822569 FOREIGN KEY (ContractDocID) REFERENCES ContractDoc (ID) ON DELETE Cascade;
ALTER TABLE ContractDoc ADD CONSTRAINT ContractdocDepartment_FK FOREIGN KEY (DepartmentID) REFERENCES Department (ID);
ALTER TABLE Department ADD CONSTRAINT ParentDepartment_FK FOREIGN KEY (ParentID) REFERENCES Department (ID);
ALTER TABLE Employee ADD CONSTRAINT WorksAt_FK FOREIGN KEY (DepartmentID) REFERENCES Department (ID);
ALTER TABLE FunctionalCustomerContract ADD CONSTRAINT FKFunctional603255 FOREIGN KEY (ContractDocID) REFERENCES ContractDoc (ID) ON DELETE Cascade;



