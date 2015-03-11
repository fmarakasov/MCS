-- This query will display the current date

SELECT SYSDATE FROM DUAL;
ALTER TABLE CONTRACTDOC DROP column ISACTIVE;
ALTER TABLE CONTRACTDOC add BROKEAT date;
ALTER TABLE CONTRACTDOC add OUTOFCONTROLAT date;
ALTER TABLE CONTRACTDOC add REALLYFINISHEDAT date;

UPDATE contractdoc c SET C.ContractorID = NULL where not(c.ContractorID in (select Id from contractor));
delete from stageresult where stageid not in (select id from stage);
delete from ScheduleContract where ContractdocId not in (select id from contractdoc);
delete from ContractPayment where ContractdocId not in (select id from contractdoc);
delete from Prepayment where ContractdocId not in (select id from contractdoc);

UPDATE contractdoc SET DepartmentID = null where not (DepartmentID in (select ID from Department));
update Department set ParentID = null where not (ParentID in (select ID from Department));
update employee set departmentid = 10020 where departmentid not in (select id from department);
delete from functionalcustomercontract where not(contractdocid in (select id from contractdoc));

delete from disposal where id not in (select disposalid from responsible);
delete from responsible where contractdocid in (select id from contractdoc where deleted > 0);
delete from contractpayment where contractdocid in (select id from contractdoc where deleted >0);
delete from paymentdocument where (id not in (select paymentdocumentid from contractpayment)) and (id not in (select paymentdocumentid from actpaymentdocument))
delete from contractdoc where deleted > 0;

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

