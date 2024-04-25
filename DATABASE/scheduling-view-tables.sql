select * from assignment;
select * from assignment where week_id = 1;
select * from daily_business_need;
select * from daily_business_need order by weekday;
select * from weekly_business_need;
select * from staff;
select * from staff natural join staff as everyone;
select * from department;
insert into department (dept_name) values ("Maintenace");
select distinct dept_name from department natural inner join staff;
select * from schedule_type;
select * from timeframe;
select * from weekframe;

select * from staff order by email;

select first_name, last_name, position, dept_name
from staff join department using(dept_id)
order by dept_name;

select dept_name, pattern, ratio 
from weekly_business_need NATURAL INNER JOIN department 
NATURAL INNER JOIN schedule_type order by dept_name;

select ratio, pattern
from weekly_business_need NATURAL INNER JOIN department NATURAL INNER JOIN schedule_type;

select week_id, user_id, first_name, Last_name, mon_date, tue_date, wed_date, thu_date, fri_date, sat_date, sun_date from assignment natural inner join staff;

select user_id 
from assignment natural inner join staff natural inner join department
where week_id = 2 and dept_name = "IT Operations" and mon_date is not null;

select ratio, timeframe_id
from daily_business_need natural inner join department
where dept_name = "IT Operations" and weekday = "mon";

select *
from assignment natural inner join staff natural inner join department
natural inner join timeframe
where week_id = 3 and sun_date is not null
order by dept_name, first_name;

select dept_name, start_time, end_time, first_name, Last_name, position 
from assignment natural inner join staff natural inner join department, timeframe
where assignment.sun_time = timeframe.timeframe_id and week_id = 3 and sun_date is not null
order by dept_name, start_time, first_name;

