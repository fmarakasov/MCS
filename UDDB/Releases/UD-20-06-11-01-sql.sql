/*
     UPDATE TO 1.0.0.25
	 ������������� ���� "���������� �����" ������ �������� ����� ����� � �������� ����������
	 ����� ���� �������� ����������  ��������� �� 2000 ��������.
*/
ALTER TABLE StageResult DROP CONSTRAINT U_STAGE_RESULT;
ALTER TABLE StageResult ADD CONSTRAINT U_STAGE_RESULT UNIQUE (StageID, Name);
ALTER TABLE StageResult MODIFY Name NVARCHAR2(2000) NOT NULL;