-- This query will display the current date

SELECT SYSDATE FROM DUAL;

create or replace type ContractManagersTable as table of varchar(4000);

create or replace function getContractManagers(contractid in number)
return ContractManagersTable
PIPELINED
is
   CURSOR MANAGERSCURSOR IS 
   SELECT e.* 
   FROM responsible r, employee e 
   WHERE r.contractdocid = contractid and r.roleid = 1 
   and e.id = r.employeeid
   order by e.familyname, e.firstname, e.middlename; 
   
   managerrec employee%rowtype;
   s varchar(4000);
begin
  OPEN MANAGERSCURSOR; 

  s := '';

  LOOP 
    FETCH MANAGERSCURSOR INTO managerrec; 
    EXIT WHEN MANAGERSCURSOR%NOTFOUND;
    if (not(managerrec.familyname is null)) and (not(managerrec.id < 0)) then 
      s :=  s||managerrec.familyname;
      
      if (not(managerrec.firstname is null)) then 
        s :=  s||' '||SUBSTR(managerrec.firstname, 1, 1)||'.';
        if (not(managerrec.middlename is null)) then 
          s :=  s||' '||SUBSTR(managerrec.middlename, 1, 1)||'.';
        end if;
      end if;   
      s := s || '; ';      
    end if;
  end loop;
  
  s := trim(s);
  s := substr(s, 1, length(s)-1);
  pipe row(s);
  
  CLOSE MANAGERSCURSOR; 
  
  return;
end;
/

CREATE OR REPLACE VIEW CONTRACTREPOSITORYVIEW AS
SELECT c.ID, c.Price, n.Percents as "NdsPercents", cr.Culture, cm.Factor, c.NDSAlgorithmID, alg.Name as "NDSAlgName", n.ID as "NDSID", cr.ID as "CurrencyID", cm.ID as "CurrencyMeasureID", c.CurrencyRate, c.StartAt,
  c.EndsAt, c.AppliedAt, c.ApprovedAt, c.InternalNum, c.Subject, c.BrokeAt, c.OutOfControlAt, c.ReallyFinishedAt, c.ContractStateID, C.DISBURSED_CACHE, C.Left_cache, c.Stages_total_price_cache,  c.DISBURSED_COWORKERS_CACHE,  c.LEFT_COWORKERS_CACHE, cs.Name as "StateName", c.Deleted, c.Origincontractid,
  ct.Name as "ContractTypeName", p.FamilyName as "PersonFamilyName",
  p.FirstName as "PersonFirstName", p.MiddleName as "PersonMiddleName", d.ID as "DegreeId", d.Name as "PersonDegree",
  c.Agreementnum as "Agreementnum",
  (select * from TABLE(cast(getContractManagers(c.id) as ContractManagersTable))) as "ManagerNames",
  (select count(*) from "UD".Contractdoc c2 where c2.origincontractid = c.id) as "AgreementReferenceCount",
   (select count(*) from ContractHierarchy ch where ch.subcontractdocid = c.id) as "GeneralReferenceCount",
   (select count(*) from ScheduleContract sc where sc.contractdocid = c.id) as ScheduleCount,
   (select ctr.ShortName from ContractorContractDoc ccd , "UD".Contractor ctr where (ccd.ContractDocID=c.id AND ctr.Id=ccd.ContractorID and rownum = 1)) as "ContractorShortName",
   (select ctt.Name from ContractorContractDoc ccd , "UD".Contractor ctr, "UD".Contractortype ctt
	where (ccd.ContractDocID=c.id AND ctr.Id=ccd.ContractorID AND ctt.id = ctr.contractortypeid and rownum = 1)) as "ContractorType",
   (select count(*) from ContractorContractDoc ccd where ccd.ContractDocID=c.id) as "ContractorsCount",
   (select count(*) from stage s, schedulecontract schc
where (s.scheduleid = schc.scheduleid) and (schc.contractdocid =
c.Id) and
exists(select * from stage cst  where cst.Parentid = s.id)
and (s.actid is null) and (0=(select count(*) from stage cst where
cst.Parentid = s.Id and (cst.actid is null)))) as "OrphandParentCount",
(select count(*) from stage s, schedulecontract schc
where (s.scheduleid = schc.scheduleid) and (schc.contractdocid = c.Id) and
exists(select * from stage cst where cst.Parentid = s.id)
and (s.actid is not null) and exists(select * from stage cst where
cst.Parentid = s.Id and (cst.actid is null))) as "ClosedParentCount"
   FROM "UD".Contractdoc c
   LEFT JOIN "UD".Contractstate cs ON cs.ID = c.ContractStateID
   LEFT JOIN "UD".Currencymeasure cm ON cm.ID = c.CurrencyMeasureId
   LEFT JOIN "UD".Currency cr ON cr.ID = c.CurrencyID
   LEFT JOIN "UD".NDS n ON n.ID = c.NDSID
   LEFT JOIN "UD".NDSAlgorithm alg ON alg.ID = c.NDSAlgorithmID
LEFT JOIN "UD".ContractType ct ON c.ContractTypeID = ct.ID
LEFT JOIN "UD".Person p ON p.ID = c.ContractorPersonID
LEFT OUTER JOIN "UD".Degree d ON d.ID = p.DegreeID
ORDER BY c.ContractTypeID, c.InternalNum;
/