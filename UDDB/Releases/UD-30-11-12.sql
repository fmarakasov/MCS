
/* UPDATE TO 1.49 

   � ���� ����������:
   - ��� ���� �� �������� ���������� � ������� ContractPriceCache   
*/



--DROP TABLE ContractPriceCache;

ALTER TABLE CONTRACTDOC DROP COLUMN DISBURSED_CACHE;
ALTER TABLE CONTRACTDOC DROP COLUMN LEFT_CACHE;
ALTER TABLE CONTRACTDOC DROP COLUMN STAGES_TOTAL_PRICE_CACHE;
ALTER TABLE CONTRACTDOC DROP COLUMN DISBURSED_COWORKERS_CACHE;
ALTER TABLE CONTRACTDOC DROP COLUMN LEFT_COWORKERS_CACHE;

CREATE TABLE ContractPriceCache (
  CacheID                   number(10) NOT NULL, 
  Disbursed_cache           number(18, 2), 
  Left_cache                number(18, 2), 
  Stages_total_price_cache  number(18, 2), 
  Disbursed_coworkers_cache number(18, 2), 
  Left_coworkers_cache      number(18, 2), 
  PRIMARY KEY (CacheID));
commit;
COMMENT ON TABLE ContractPriceCache IS '�������� ����������� ���� �� ��������';
COMMENT ON COLUMN ContractPriceCache.Disbursed_cache IS '������������ ��� �������� ������������ �������� ����� ����������� ����� �� �������� ��� ��������� ������������������';
COMMENT ON COLUMN ContractPriceCache.Left_cache IS '������������ ��� �������� ������������ �������� ����� ������� �� ������� ��� ��������� ������������������';
COMMENT ON COLUMN ContractPriceCache.Stages_total_price_cache IS '������������ ��� �������� ������������ �������� ����� �� ������� �� ������������ ����� ��� ��������� ������������������';
COMMENT ON COLUMN ContractPriceCache.Disbursed_coworkers_cache IS '������������ ��� �������� ������������ �������� ����� ����������� ����� �� ������� �������������� ��� ��������� ������������������';
COMMENT ON COLUMN ContractPriceCache.Left_coworkers_cache IS '������������ ��� �������� ������������ �������� ����� ����������� ������� �� ������� �������������� ��� ��������� ������������������';

commit;
ALTER TABLE ContractPriceCache ADD CONSTRAINT FK_CONTRACTCACHE_CONTRACT FOREIGN KEY (CacheID) REFERENCES ContractDoc (ID) ON DELETE Cascade;

COMMENT ON TABLE ContractPriceCache IS '�������� ����������� ���� �� ��������';
COMMENT ON COLUMN ContractPriceCache.Disbursed_cache IS '������������ ��� �������� ������������ �������� ����� ����������� ����� �� �������� ��� ��������� ������������������';
COMMENT ON COLUMN ContractPriceCache.Left_cache IS '������������ ��� �������� ������������ �������� ����� ������� �� ������� ��� ��������� ������������������';
COMMENT ON COLUMN ContractPriceCache.Stages_total_price_cache IS '������������ ��� �������� ������������ �������� ����� �� ������� �� ������������ ����� ��� ��������� ������������������';
COMMENT ON COLUMN ContractPriceCache.Disbursed_coworkers_cache IS '������������ ��� �������� ������������ �������� ����� ����������� ����� �� ������� �������������� ��� ��������� ������������������';
COMMENT ON COLUMN ContractPriceCache.Left_coworkers_cache IS '������������ ��� �������� ������������ �������� ����� ����������� ������� �� ������� �������������� ��� ��������� ������������������';
/
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