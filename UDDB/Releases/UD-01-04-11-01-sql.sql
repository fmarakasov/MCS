/*
    UPDATE TO 1.0.0.15
*/

 ALTER TABLE ContractDoc ADD PrepaymentPrecentType number(1);
 COMMENT ON COLUMN ContractDoc.PrepaymentPrecentType IS '������ ����� ������ �� �������� ������: NULL - ������������� �� �������������, 0 - % �� ����� �������� ��� ���, 1 - % �� ����� �������� � ���';