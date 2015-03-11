-- This query will display the current date

SELECT SYSDATE FROM DUAL;


ALTER TABLE CONTRACTDOC DROP COLUMN DISBURSED_CACHE;
ALTER TABLE CONTRACTDOC DROP COLUMN LEFT_CACHE;
ALTER TABLE CONTRACTDOC DROP COLUMN STAGES_TOTAL_PRICE_CACHE;
ALTER TABLE CONTRACTDOC DROP COLUMN DISBURSED_COWORKERS_CACHE;
ALTER TABLE CONTRACTDOC DROP COLUMN LEFT_COWORKERS_CACHE;

drop table ContractPriceCache;
CREATE TABLE ContractPriceCache (
  CacheID                   number(10) NOT NULL, 
  Disbursed_cache           number(18, 2), 
  Left_cache                number(18, 2), 
  Stages_total_price_cache  number(18, 2), 
  Disbursed_coworkers_cache number(18, 2), 
  Left_coworkers_cache      number(18, 2), 
  PRIMARY KEY (CacheID));
commit;
COMMENT ON TABLE ContractPriceCache IS 'Хранение вычисленных сумм по договору';
COMMENT ON COLUMN ContractPriceCache.Disbursed_cache IS 'Используется для хранения расчитанного значения суммы выполненных работ по договору для повышения производительности';
COMMENT ON COLUMN ContractPriceCache.Left_cache IS 'Используется для хранения расчитанного значения суммы остатка по работам для повышения производительности';
COMMENT ON COLUMN ContractPriceCache.Stages_total_price_cache IS 'Используется для хранения расчитанного значения суммы по работам по календарному плану для повышения производительности';
COMMENT ON COLUMN ContractPriceCache.Disbursed_coworkers_cache IS 'Используется для хранения расчитанного значения суммы выполненных работ по работам соисполнителей для повышения производительности';
COMMENT ON COLUMN ContractPriceCache.Left_coworkers_cache IS 'Используется для хранения расчитанного значения суммы выполненных остатка по работам соисполнителей для повышения производительности';

commit;
ALTER TABLE ContractPriceCache ADD CONSTRAINT FK_CONTRACTCACHE_CONTRACT FOREIGN KEY (CacheID) REFERENCES ContractDoc (ID) ON DELETE Cascade;

COMMENT ON TABLE ContractPriceCache IS 'Хранение вычисленных сумм по договору';
COMMENT ON COLUMN ContractPriceCache.Disbursed_cache IS 'Используется для хранения расчитанного значения суммы выполненных работ по договору для повышения производительности';
COMMENT ON COLUMN ContractPriceCache.Left_cache IS 'Используется для хранения расчитанного значения суммы остатка по работам для повышения производительности';
COMMENT ON COLUMN ContractPriceCache.Stages_total_price_cache IS 'Используется для хранения расчитанного значения суммы по работам по календарному плану для повышения производительности';
COMMENT ON COLUMN ContractPriceCache.Disbursed_coworkers_cache IS 'Используется для хранения расчитанного значения суммы выполненных работ по работам соисполнителей для повышения производительности';
COMMENT ON COLUMN ContractPriceCache.Left_coworkers_cache IS 'Используется для хранения расчитанного значения суммы выполненных остатка по работам соисполнителей для повышения производительности';


ALTER TABLE ContractTranActDoc ADD Actid NUMBER (10);
ALTER TABLE Document ADD Domain NUMBER (10);

ALTER TABLE ContractTranActDoc MODIFY ContractDocID NUMBER(10);
ALTER TABLE ContractTranActDoc ADD CONSTRAINT FKContractTr647138 FOREIGN KEY (ActID) REFERENCES Act (ID);
DROP TABLE Transferactactdocument;
ALTER TABLE Document add Domain NUMBER(10);

alter table STAGERESULT add actintroductiondate date;
alter table STAGERESULT add efficiencycomment nvarchar2(2000);

-- This query will display the current date

-- Add/modify columns 
alter table CONTRACTDOC add acttypeid number(10);
-- Add comments to the columns 
comment on column CONTRACTDOC.acttypeid
  is 'Тип акта';

alter table CONTRACTDOC
  add constraint CONTRACTACTTYPE_FK foreign key (ACTTYPEID)
  references acttype (ID);