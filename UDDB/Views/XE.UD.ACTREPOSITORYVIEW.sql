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
(select Internalnum from contractdoc where id = (GetActContractdocid(a.Id))) as "ContractSubject",
(select count(*) from ContractTranActDoc where actid = a.id) as "TransferedCount"
from act a
left join acttype atp on (a.acttypeid = a.id)
left join currency c on (c.id = a.currencyid)
left join currencymeasure cm on (cm.id = a.currencymeasureid)
left join ndsalgorithm ndsa on (ndsa.id = a.ndsalgorithmid)
left join nds on (nds.id = a.ndsid)
left join region r on (r.id = a.regionid);
