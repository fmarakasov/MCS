--Update for table ContractType. Set right relations between Name and ReportOrder
--For 01.08.11 release
begin
UPDATE ContractType SET ReportOrder = 4 WHERE ID = 2; --Газификация 
UPDATE ContractType SET ReportOrder = 5 WHERE ID = 4; --Экспертиза сметной документации
UPDATE ContractType SET ReportOrder = 6 WHERE ID = 5;--Производство
UPDATE ContractType SET ReportOrder = 7 WHERE ID = 6; --Прочие
UPDATE ContractType SET ReportOrder = 3 WHERE ID = 3; --ПИР
INSERT INTO ContractType  (ID, Name, ReportOrder) VALUES   (7, 'Оказание услуг', 2);

UPDATE ContractorType SET ReportOrder = 1 WHERE ID = 3; --Газпром
UPDATE ContractorType SET ReportOrder = 2 WHERE ID = 2; --Дочерние общества
UPDATE ContractorType SET ReportOrder = 3 WHERE ID = 1; --Другие организации

commit;
end;
