/*
    UPDATE TO 1.0.0.15
*/

 ALTER TABLE ContractDoc ADD PrepaymentPrecentType number(1);
 COMMENT ON COLUMN ContractDoc.PrepaymentPrecentType IS 'Формат ввода данных по договору аванса: NULL - авансирование не пердусмотрено, 0 - % от суммы договора без НДС, 1 - % от суммы договора с НДС';