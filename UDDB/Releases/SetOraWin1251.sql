/*
	������ ��� ��������� ��������� WIN1251. ��� ������� ������� ����� � ����������� SYSDBA.
	������� ������������ ������ � ������ �������, ������������ � ��������� ������ ?? ������ ����� �� �������
	� ��������� ����� ���� ������ ��� ��������� ������� � ��������������
*/

SHUTDOWN IMMEDIATE;
STARTUP MOUNT;
ALTER SYSTEM ENABLE RESTRICTED SESSION;
ALTER SYSTEM SET JOB_QUEUE_PROCESSES=0;
ALTER SYSTEM SET AQ_TM_PROCESSES=0;
ALTER DATABASE OPEN;
ALTER DATABASE CHARACTER SET INTERNAL_USE CL8MSWIN1251;
ALTER SYSTEM DISABLE RESTRICTED SESSION;
