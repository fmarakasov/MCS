-- This query will display the current date

SELECT SYSDATE FROM DUAL;


CREATE TABLE YearReportColor (
  Id			 number(10) not null,
  Quarter        number(2) NOT NULL, 
  Year           number(2) NOT NULL, 
  Color          number(10), 
  CoworkersColor number(10), 
  PRIMARY KEY (Id));

COMMENT ON TABLE YearReportColor IS '��������� � ������� ��������';
COMMENT ON COLUMN YearReportColor.Quarter IS '�������';
COMMENT ON COLUMN YearReportColor.Year IS '���: 0 - �������, -1 - ����������, 1 - ���������';
COMMENT ON COLUMN YearReportColor.Color IS '���� ��������� ����� ��� �����������';
COMMENT ON COLUMN YearReportColor.CoworkersColor IS '���� ��������� ����� ��� �������������';


create sequence SEQ_YEARREPORTCOLOR;
