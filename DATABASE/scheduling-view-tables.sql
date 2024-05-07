select * from assignment;
select * from credentials;
select * from signupcode;
delete from assignment where week_id = 2;
select * from assignment where week_id = 1;
select * from daily_business_need;
select * from daily_business_need order by weekday;

select * from time_off_request;
select * from weekly_business_need;
select * from staff;
select * from staff order by dept_id;
select * from staff natural join staff as everyone;
select * from department;
select dept_name, count(*) from department natural inner join staff group by dept_name;
insert into department (dept_name) values ("Customer Service");
select distinct dept_name from department natural inner join staff;
select * from schedule_type;
select * from timeframe;
select * from weekframe;
select week_id from weekframe where start_day = '2024-04-22';

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

select dept_id, dept_name from department order by dept_name;

SELECT first_name, last_name, position, dept_name FROM staff NATURAL INNER JOIN department WHERE user_id = 7;

select week_id from weekframe where start_day = "2024-04-22";

select start_time, end_time from assignment, timeframe
where week_id = 25 and user_id = 35 and mon_time = timeframe_id;

delete from assignment where week_id = 2;
select user_id from staff natural inner join credentials where email = "admin@umbc.edu" and pswd = "root";

SELECT ratio, start_time, end_time FROM daily_business_need NATURAL INNER JOIN timeframe WHERE dept_id = 8 and weekday="mon";