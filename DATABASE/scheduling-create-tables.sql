create schema if not exists schedule_app;
use schedule_app;

-- this is used to store the week information
CREATE TABLE IF NOT EXISTS weekframe(
	week_id			int auto_increment,
	week_name		varchar(20),
    start_day		date,
    end_day			date as (DATE_ADD(start_day, INTERVAL 6 DAY)),
	primary key(week_id)
);


CREATE TABLE IF NOT EXISTS timeframe(
	timeframe_id	int auto_increment,
    start_time		time,
    end_time		time,
    primary key (timeframe_id)
);

-- patterns look like MTWUFAS using X to indicate days off
-- (M)mon, (T)tue, (W)wed, (U)thu, (F)fri, (A)sat, (S)sun
-- ex: MTXXFAS means off wed and thu
CREATE TABLE IF NOT EXISTS schedule_type(
	schedule_id		int auto_increment,
    pattern			varchar(7),
    primary key (schedule_id)
);

-- we use this to classify employees and regulate their schedule
-- it is used to filter the pool from which employees are selected to fill the schedules
-- the department determine type of employees
-- managers in a department will be put in a special department linked to the department they manage
CREATE TABLE IF NOT EXISTS department(
	dept_id			int auto_increment,
	dept_name		varchar(24),
    primary key (dept_id)
);

-- staff is any one working at the company, employee and managers
-- it is used to determine how many of each type of employee are connected to a department
-- which then help determine how employee a department has in order to create the number of schedules
CREATE TABLE IF NOT EXISTS staff(
	user_id			int auto_increment,
    email			varchar(64) unique,
    -- add unique keyword later
    ssn				varchar(11), 
    date_of_birth	date,
    -- job title
    position		varchar(32),
    -- name
    first_name		varchar(16) ,
    last_name		varchar(16),
    -- address
    street			varchar(20),
    city			varchar(20),
    unit			varchar(10),
    state			char(2),
    zipcode			char(5),
    -- add unique keyword later
    phone_number	char(13),
    phone_type		enum('Mobile', 'Work', 'Home'), 
    gender			enum('M','F','O'),
    dept_id			int,
    foreign key (dept_id) references department(dept_id)
		on delete set null,
    primary key (user_id)
);

-- this relation links each department with a weekly shift pattern
-- and indicated how many shifts are needed
-- multiple shift patterns can be linked to a department
CREATE TABLE IF NOT EXISTS weekly_business_need(
    dept_id			int,
    schedule_id		int,
    ratio			numeric(3,2) check (ratio >= 0 and ratio <=1),
    foreign key (dept_id) references department(dept_id)
		on delete cascade,
    foreign key (schedule_id) references schedule_type(schedule_id)
		on delete cascade,
    primary key (dept_id, schedule_id)
);

-- this relation links each department with a daily timeframe
-- and indicated how many patterns are needed by using a ratio
-- many timeframe can be linked to a department
-- the system should check that sum of ratio is less than 1 before setting up the tuples
CREATE TABLE IF NOT EXISTS daily_business_need(
    dept_id			int,
    weekday			char(3) check (weekday in ('mon','tue', 'wed', 'thu', 'fri', 'sat', 'sun')),
    timeframe_id	int,
    ratio			numeric(3,2) check (ratio >= 0 and ratio <=1),
    -- foreign key (week_id) references weekframe(week_id)
		-- on delete cascade,
    foreign key (dept_id) references department(dept_id)
		on delete cascade,
    foreign key (timeframe_id) references timeframe(timeframe_id)
		on delete cascade,
    primary key (dept_id, weekday, timeframe_id)
);

CREATE TABLE IF NOT EXISTS assignment(
	week_id		int,
    user_id		int, -- unique
    mon_date	date,
    mon_time	int,
    tue_date	date,
    tue_time	int,
    wed_date	date,
    wed_time	int,
    thu_date	date,
    thu_time	int,
    fri_date	date,
    fri_time	int,
    sat_date	date,
    sat_time	int,
    sun_date	date,
    sun_time	int,
    foreign key (week_id) references weekframe(week_id)
		on delete cascade,
    foreign key (user_id) references staff(user_id)
		on delete set null,
	foreign key (mon_time) references timeframe(timeframe_id)
		on delete set null,
	foreign key (tue_time) references timeframe(timeframe_id)
		on delete set null,
	foreign key (wed_time) references timeframe(timeframe_id)
		on delete set null,
	foreign key (thu_time) references timeframe(timeframe_id)
		on delete set null,
	foreign key (fri_time) references timeframe(timeframe_id)
		on delete set null,
	foreign key (sat_time) references timeframe(timeframe_id)
		on delete set null,
	foreign key (sun_time) references timeframe(timeframe_id)
		on delete set null
    -- primary key (week_id)
);