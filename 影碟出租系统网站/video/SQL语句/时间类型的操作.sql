--�ѵ�ǰʱ��ת����datetime��
select CONVERT(varchar, getdate(), 120 ) 

--�ѵ�ǰʱ��ת����date��(ȡǰ10���ַ�)
select CONVERT(varchar(10), getdate(), 120 )

--���ڵıȽ�(ǰ10���ַ�)
select * from videoinfo where convert(varchar(10),buydate,120) = convert(varchar(10),'2018-01-01 08:11:11',120)

--ʱ�����͵ıȽ�
select * from videoinfo where buydate = convert(varchar(10),'2018-01-01 08:11:11',120)
select * from videoinfo where buydate = convert(varchar,'2018-01-01 08:11:11',120)