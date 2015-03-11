/*

������� ����� ��������� 1.0
������ ������������ 25/11/201206:03:20 PM
1.48
   1) ������� ������� ����� �������� ����� � ���������
   2) �������  ContractTranActDoc ������������ � ��� ��������� � ��� �����
        ������� ����������� UNIQUE
   3) ������������� ����������� ���������� ����� ����� ApprovalProcess � ApprovalState
1.47
   1) ������������� ������� ������������ ����� �� (1.47 - ������� �����)
   2) ���������� ������������� ����������� �����
1.46
    1) ��������� ��������� ��� ��������� ������ ������������
    2) �������� ������������� ������� ��������� ��� ����������� �� ��������� ��������
    3) �������� (!!!) ������ ������������� ������� �����
    4) ������ Individual. ���� ���.��� ������ � �������������� ��������� ������� ������������. 
1.45
    1) ��������� ���-���� � contractdoc (disbursed_cache, left_cache)
1.44
    1) �������� Individual
    2) �������� Education
    3) ������� �������� � ����� � ��������� ������ (���� Delta � Comment)
    4) ������� ������������ ������ � �����������
    5) ������� ���������� �������������� �� �����
	6) ������� ����������� ����� ��������� ��� ������� � ������������ � ������������� (ReportGrouping)
	7) ������� �������� �������� ���������
	8) �������� �������� �������� ���� � cascade �� set null
 
  1.43:
	1)  ��������� ���� Images:blob ��� Contractdoc
	2) ��������� ������� �� ������� ����� ��� ������
	    � ������������ ���������� ����������
	3) �������� ����������� ������ � WorkType � CurrencyMeasure
*/
CREATE OR REPLACE VIEW CONTRACTREPOSITORYVIEW AS
SELECT c.ID, c.Price, n.Percents as "NdsPercents", cr.Culture, cm.Factor, c.NDSAlgorithmID, alg.Name as "NDSAlgName", n.ID as "NDSID", cr.ID as "CurrencyID", cm.ID as "CurrencyMeasureID", c.CurrencyRate, c.StartAt,
  c.EndsAt, c.AppliedAt, c.ApprovedAt, c.InternalNum, c.Subject, c.BrokeAt, c.OutOfControlAt, c.ReallyFinishedAt, c.ContractStateID, CC.DISBURSED_CACHE, CC.Left_cache, CC.Stages_total_price_cache,  CC.DISBURSED_COWORKERS_CACHE,  CC.LEFT_COWORKERS_CACHE, cs.Name as "StateName", c.Deleted, c.Origincontractid,
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
cst.Parentid = s.Id and (cst.actid is null))) as "ClosedParentCount",
(GetMainContract(c.id)) as "MainContractID",
(select Internalnum from contractdoc where id = (GetMainContract(c.id))) as "MainContractSubject"
   FROM "UD".Contractdoc c
   LEFT JOIN "UD".Contractstate cs ON cs.ID = c.ContractStateID
   LEFT JOIN "UD".Currencymeasure cm ON cm.ID = c.CurrencyMeasureId
   LEFT JOIN "UD".Currency cr ON cr.ID = c.CurrencyID
   LEFT JOIN "UD".NDS n ON n.ID = c.NDSID
   LEFT JOIN "UD".NDSAlgorithm alg ON alg.ID = c.NDSAlgorithmID
   LEFT JOIN "UD".ContractPriceCache CC ON c.ID = CC.CacheID
LEFT JOIN "UD".ContractType ct ON c.ContractTypeID = ct.ID
LEFT JOIN "UD".Person p ON p.ID = c.ContractorPersonID
LEFT OUTER JOIN "UD".Degree d ON d.ID = p.DegreeID
ORDER BY c.ContractTypeID, c.InternalNum;
/
/
create or replace view actrepositoryview as
select
a.id,
a.acttypeid,
atp.typename "ActTypeName",
a.enterpriceauthorityid,
a.currencyid,
c.name "CurrencyName",
c.culture "CurrencyCulture",
a.currencymeasureid,
cm.name "CurrencyMeasureName",
cm.factor "CurrencyMeasureFactor",
a.ndsalgorithmid,
ndsa.name "NdsalgorithmName",
num,
signdate,
a.ndsid,
nds.percents "NdsPercents",
a.regionid,
r.name "RegionName",
totalsum,
sumfortransfer,
status,
currencyrate,
ratedate,
issigned,
(GetActStageNumbers(a.id)) as "ActStageNumbers",
(GetActContractdocId(a.Id)) as "Contractdocid",
(GetMainContract(GetActContractdocid(a.Id))) as "MainContractID",
(select Internalnum from contractdoc where id = (GetMainContract(GetActContractdocid(a.Id)))) as "MainContractSubject",
(select Internalnum from contractdoc where id = (GetActContractdocid(a.Id))) as "ContractSubject"
from act a
left join acttype atp on (a.acttypeid = a.id)
left join currency c on (c.id = a.currencyid)
left join currencymeasure cm on (cm.id = a.currencymeasureid)
left join ndsalgorithm ndsa on (ndsa.id = a.ndsalgorithmid)
left join nds on (nds.id = a.ndsid)
left join region r on (r.id = a.regionid);
/