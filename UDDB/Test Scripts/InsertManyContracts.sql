-- This query will display the current date
declare 
  CurrID number(10);
begin
delete from contractdoc where ID >=100 and ID < 10000;

CurrID:=100;
WHILE CurrID <= 500
LOOP
      INSERT INTO ContractDoc
  (ID, Price, ContractTypeID, StartAt, EndsAt, AppliedAt, ApprovedAt, InternalNum, ContractorNum, CurrencyID, OriginContractID, NDSAlgorithmID, PrepaymentSum, PrepaymentPercent, PrepaymentNDSAlgorithmID, NDSID, ContractStateID, CurrencyMeasureID, ContractorID, IsProtectability, Subject, CurrencyRate, RateDate, Description, ContractorPersonID, IsActive, AuthorityID, PrepaymentPrecentType, Deleted) 
VALUES 
   (CurrID, 2000+CurrID, 1, '01.01.2010', '31.12.2011', '27.01.2009', '27.01.2010', '1000-1000-500', null, 1, null, 1, 600, 30, 1, 1, 1, 1, 7, null, 'Ёто один из многих договоров ', null, null, '“аких договоров много', 5, 1, 1, null, null);
    CurrID := CurrID + 1;
END LOOP;
commit;
end;


