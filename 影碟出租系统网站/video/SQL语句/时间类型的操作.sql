--把当前时间转化成datetime型
select CONVERT(varchar, getdate(), 120 ) 

--把当前时间转化成date型(取前10个字符)
select CONVERT(varchar(10), getdate(), 120 )

--日期的比较(前10个字符)
select * from videoinfo where convert(varchar(10),buydate,120) = convert(varchar(10),'2018-01-01 08:11:11',120)

--时间类型的比较
select * from videoinfo where buydate = convert(varchar(10),'2018-01-01 08:11:11',120)
select * from videoinfo where buydate = convert(varchar,'2018-01-01 08:11:11',120)