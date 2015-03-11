/*
   UPDATE TO 1.0.0.19
*/

ALTER TABLE Act ADD CurrencymeasureID     number(10);
COMMENT ON COLUMN Act.CurrencymeasureID IS '—сылка на единицу измерени€ денег';
ALTER TABLE Act ADD CONSTRAINT FKAct99460 FOREIGN KEY (CurrencymeasureID) REFERENCES CurrencyMeasure (ID);