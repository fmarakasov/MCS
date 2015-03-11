/*
   UPDATE TO 1.0.0.17
*/

ALTER TABLE Contracttype ADD ReportOrder NUMBER(3);
ALTER TABLE Contractortype ADD ReportOrder NUMBER(3);
ALTER TABLE Act ADD NdsalgorithmID NUMBER(10);
ALTER TABLE Act ADD CurrencyID NUMBER(10);
ALTER TABLE Contractdoc ADD Deleted NUMBER(1);

ALTER TABLE Act ADD CONSTRAINT FKAct481665 FOREIGN KEY (NdsalgorithmID) REFERENCES NDSAlgorithm (ID);
ALTER TABLE Act ADD CONSTRAINT FKAct124626 FOREIGN KEY (CurrencyID) REFERENCES Currency (ID);

COMMENT ON COLUMN ContractType.ReportOrder IS 'Определяет порядок следования группы в отчётах';
COMMENT ON COLUMN ContractorType.ReportOrder IS 'Определяет порядок следования группы в отчётах';
COMMENT ON COLUMN Act.NdsalgorithmID IS 'Алгоритм расчёта НДС';
COMMENT ON COLUMN Act.CurrencyID IS 'Валюта акта';
COMMENT ON COLUMN ContractDoc.Deleted IS 'Признак того, что договор удалён пользователем';