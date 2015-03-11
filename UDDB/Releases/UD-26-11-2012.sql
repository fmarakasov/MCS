
/*UPDATE TO 1.48*/
ALTER TABLE ContractTranActDoc ADD Actid NUMBER (10);
ALTER TABLE Document ADD Domain NUMBER (10);

ALTER TABLE ContractTranActDoc MODIFY ContractDocID NUMBER(10);
ALTER TABLE ContractTranActDoc ADD CONSTRAINT FKContractTr647138 FOREIGN KEY (ActID) REFERENCES Act (ID);
DROP TABLE Transferactactdocument;
INSERT INTO TransferActType
  (ID, Name) 
VALUES 
  (-1, '(Не определён)');
INSERT INTO TransferActType
  (ID, Name) 
VALUES 
  (1, 'Акт передачи договоров');
INSERT INTO TransferActType
  (ID, Name) 
VALUES 
  (2, 'Акт передачи актов к договорам');
