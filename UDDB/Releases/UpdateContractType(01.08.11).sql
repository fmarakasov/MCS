--Update for table ContractType. Set right relations between Name and ReportOrder
--For 01.08.11 release
begin
UPDATE ContractType SET ReportOrder = 4 WHERE ID = 2; --����������� 
UPDATE ContractType SET ReportOrder = 5 WHERE ID = 4; --���������� ������� ������������
UPDATE ContractType SET ReportOrder = 6 WHERE ID = 5;--������������
UPDATE ContractType SET ReportOrder = 7 WHERE ID = 6; --������
UPDATE ContractType SET ReportOrder = 3 WHERE ID = 3; --���
INSERT INTO ContractType  (ID, Name, ReportOrder) VALUES   (7, '�������� �����', 2);

UPDATE ContractorType SET ReportOrder = 1 WHERE ID = 3; --�������
UPDATE ContractorType SET ReportOrder = 2 WHERE ID = 2; --�������� ��������
UPDATE ContractorType SET ReportOrder = 3 WHERE ID = 1; --������ �����������

commit;
end;
