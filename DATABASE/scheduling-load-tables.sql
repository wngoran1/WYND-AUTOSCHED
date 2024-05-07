-- sample data
insert into department(dept_name) values ("Customer Service"), ("Public relation"), ("Network Engineering"), 
("System Security"), ("IT Support"), ("IT Operations"), ("Project Management"), ("Business Analysis");

insert into schedule_type(pattern) values ("MTWUFXX"), ("XXWUFAS"), ("MTXXFAS"), ("XTWUFAX");

INSERT INTO timeframe (start_time, end_time) VALUES 
('06:45:00', '15:15:00'),
('08:30:00', '17:00:00'),
('09:45:00', '18:15:00'),
('11:30:00', '20:00:00');

INSERT INTO weekly_business_need (dept_id, schedule_id, ratio)
VALUES
(1, 1, 0.5),(1, 3, 0.5),
(2, 1, 0.5),(2, 3, 0.5),
(3, 1, 0.25),(3, 2, 0.25),(3, 3, 0.25),(3, 4, 0.25),
(4, 1, 0.5),(4, 4, 0.5),
(5, 3, 0.5),(5, 4, 0.5),
(6, 1, 0.25),(6, 2, 0.25),(6, 3, 0.25),(6, 4, 0.25),
(7, 1, 0.5),(7, 3, 0.5),
(8, 1, 1);

INSERT INTO daily_business_need (dept_id, weekday, timeframe_id, ratio)
VALUES
(1, 'mon', 2, 1), (1, 'tue', 2, 1), (1, 'wed', 2, 1), (1, 'thu', 2, 1), (1, 'fri', 2, 1), (1, 'sat', 2, 1), (1, 'sun', 2, 1),
(2, 'mon', 3, 1), (2, 'tue', 3, 1), (2, 'wed', 3, 1), (2, 'thu', 3, 1), (2, 'fri', 3, 1), (2, 'sat', 3, 1), (2, 'sun', 3, 1),
(3, 'mon', 1, 0.5), (3, 'tue', 1, 0.5), (3, 'wed', 1, 0.5), (3, 'thu', 1, 0.5), (3, 'fri', 1, 0.5), (3, 'sat', 1, 0.5), (3, 'sun', 1, 0.5),
(3, 'mon', 4, 0.5), (3, 'tue', 4, 0.5), (3, 'wed', 4, 0.5), (3, 'thu', 4, 0.5), (3, 'fri', 3, 0.5), (3, 'sat', 3, 0.5), (3, 'sun', 3, 0.5),
(4, 'mon', 1, 0.5), (4, 'tue', 1, 0.5), (4, 'wed', 1, 0.5), (4, 'thu', 1, 0.5), (4, 'fri', 1, 0.5), (4, 'sat', 1, 0.5), (4, 'sun', 1, 0.5),
(4, 'mon', 4, 0.5), (4, 'tue', 4, 0.5), (4, 'wed', 4, 0.5), (4, 'thu', 4, 0.5), (4, 'fri', 3, 0.5), (4, 'sat', 3, 0.5), (4, 'sun', 3, 0.5),
(5, 'mon', 2, 1), (5, 'tue', 2, 1), (5, 'wed', 2, 1), (5, 'thu', 2, 1), (5, 'fri', 2, 1), (5, 'sat', 2, 1), (5, 'sun', 2, 1),
(6, 'mon', 2, 0.5), (6, 'tue', 2, 0.5), (6, 'wed', 2, 0.5), (6, 'thu', 2, 0.5), (6, 'fri', 2, 0.5), (6, 'sat', 2, 0.5), (6, 'sun', 2, 0.5),
(6, 'mon', 4, 0.5), (6, 'tue', 4, 0.5), (6, 'wed', 4, 0.5), (6, 'thu', 4, 0.5), (6, 'fri', 4, 0.5), (6, 'sat', 4, 0.5), (6, 'sun', 4, 0.5),
(7, 'mon', 3, 1), (7, 'tue', 3, 1), (7, 'wed', 3, 1), (7, 'thu', 3, 1), (7, 'fri', 3, 1), (7, 'sat', 3, 1), (7, 'sun', 3, 1),
(8, 'mon', 2, 1), (8, 'tue', 2, 1), (8, 'wed', 2, 1), (8, 'thu', 2, 1), (8, 'fri', 2, 1), (8, 'sat', 2, 1), (8, 'sun', 2, 1);


/*MORE PATTERNS FOR LATER
(2, 'mon', 1, 1), (2, 'tue', 1, 1), (2, 'wed', 1, 1), (2, 'thu', 1, 1), (2, 'fri', 1, 1), (2, 'sat', 1, 1), (2, 'sun', 1, 1),
(3, 'mon', 1, 1), (3, 'tue', 1, 1), (3, 'wed', 1, 1), (3, 'thu', 1, 1), (3, 'fri', 1, 1), (3, 'sat', 1, 1), (3, 'sun', 1, 1),
(4, 'mon', 1, 1), (4, 'tue', 1, 1), (4, 'wed', 1, 1), (4, 'thu', 1, 1), (4, 'fri', 1, 1), (4, 'sat', 1, 1), (4, 'sun', 1, 1),
(5, 'mon', 1, 1), (5, 'tue', 1, 1), (5, 'wed', 1, 1), (5, 'thu', 1, 1), (5, 'fri', 1, 1), (5, 'sat', 1, 1), (5, 'sun', 1, 1),
(6, 'mon', 1, 1), (6, 'tue', 1, 1), (6, 'wed', 1, 1), (6, 'thu', 1, 1), (6, 'fri', 1, 1), (6, 'sat', 1, 1), (6, 'sun', 1, 1),
(7, 'mon', 1, 1), (7, 'tue', 1, 1), (7, 'wed', 1, 1), (7, 'thu', 1, 1), (7, 'fri', 1, 1), (7, 'sat', 1, 1), (7, 'sun', 1, 1),
(8, 'mon', 1, 1), (8, 'tue', 1, 1), (8, 'wed', 1, 1), (8, 'thu', 1, 1), (8, 'fri', 1, 1), (8, 'sat', 1, 1), (8, 'sun', 1, 1),

(2, 'mon', 2, 1), (2, 'tue', 2, 1), (2, 'wed', 2, 1), (2, 'thu', 2, 1), (2, 'fri', 2, 1), (2, 'sat', 2, 1), (2, 'sun', 2, 1),
(3, 'mon', 2, 1), (3, 'tue', 2, 1), (3, 'wed', 2, 1), (3, 'thu', 2, 1), (3, 'fri', 2, 1), (3, 'sat', 2, 1), (3, 'sun', 2, 1),
(4, 'mon', 2, 1), (4, 'tue', 2, 1), (4, 'wed', 2, 1), (4, 'thu', 2, 1), (4, 'fri', 2, 1), (4, 'sat', 2, 1), (4, 'sun', 2, 1),
(5, 'mon', 2, 1), (5, 'tue', 2, 1), (5, 'wed', 2, 1), (5, 'thu', 2, 1), (5, 'fri', 2, 1), (5, 'sat', 2, 1), (5, 'sun', 2, 1),
(6, 'mon', 2, 1), (6, 'tue', 2, 1), (6, 'wed', 2, 1), (6, 'thu', 2, 1), (6, 'fri', 2, 1), (6, 'sat', 2, 1), (6, 'sun', 2, 1),
(7, 'mon', 2, 1), (7, 'tue', 2, 1), (7, 'wed', 2, 1), (7, 'thu', 2, 1), (7, 'fri', 2, 1), (7, 'sat', 2, 1), (7, 'sun', 2, 1),
(8, 'mon', 2, 1), (8, 'tue', 2, 1), (8, 'wed', 2, 1), (8, 'thu', 2, 1), (8, 'fri', 2, 1), (8, 'sat', 2, 1), (8, 'sun', 2, 1),

(3, 'mon', 3, 1), (3, 'tue', 3, 1), (3, 'wed', 3, 1), (3, 'thu', 3, 1), (3, 'fri', 3, 1), (3, 'sat', 3, 1), (3, 'sun', 3, 1),
(4, 'mon', 3, 1), (4, 'tue', 3, 1), (4, 'wed', 3, 1), (4, 'thu', 3, 1), (4, 'fri', 3, 1), (4, 'sat', 3, 1), (4, 'sun', 3, 1),
(5, 'mon', 3, 1), (5, 'tue', 3, 1), (5, 'wed', 3, 1), (5, 'thu', 3, 1), (5, 'fri', 3, 1), (5, 'sat', 3, 1), (5, 'sun', 3, 1),
(6, 'mon', 3, 1), (6, 'tue', 3, 1), (6, 'wed', 3, 1), (6, 'thu', 3, 1), (6, 'fri', 3, 1), (6, 'sat', 3, 1), (6, 'sun', 3, 1),
(7, 'mon', 3, 1), (7, 'tue', 3, 1), (7, 'wed', 3, 1), (7, 'thu', 3, 1), (7, 'fri', 3, 1), (7, 'sat', 3, 1), (7, 'sun', 3, 1),
(8, 'mon', 3, 1), (8, 'tue', 3, 1), (8, 'wed', 3, 1), (8, 'thu', 3, 1), (8, 'fri', 3, 1), (8, 'sat', 3, 1), (8, 'sun', 3, 1),

(2, 'mon', 4, 1), (2, 'tue', 4, 1), (2, 'wed', 4, 1), (2, 'thu', 4, 1), (2, 'fri', 4, 1), (2, 'sat', 4, 1), (2, 'sun', 4, 1),
(3, 'mon', 4, 1), (3, 'tue', 4, 1), (3, 'wed', 4, 1), (3, 'thu', 4, 1), (3, 'fri', 4, 1), (3, 'sat', 4, 1), (3, 'sun', 4, 1),
(4, 'mon', 4, 1), (4, 'tue', 4, 1), (4, 'wed', 4, 1), (4, 'thu', 4, 1), (4, 'fri', 4, 1), (4, 'sat', 4, 1), (4, 'sun', 4, 1),
(5, 'mon', 4, 1), (5, 'tue', 4, 1), (5, 'wed', 4, 1), (5, 'thu', 4, 1), (5, 'fri', 4, 1), (5, 'sat', 4, 1), (5, 'sun', 4, 1),
(6, 'mon', 4, 1), (6, 'tue', 4, 1), (6, 'wed', 4, 1), (6, 'thu', 4, 1), (6, 'fri', 4, 1), (6, 'sat', 4, 1), (6, 'sun', 4, 1),
(7, 'mon', 4, 1), (7, 'tue', 4, 1), (7, 'wed', 4, 1), (7, 'thu', 4, 1), (7, 'fri', 4, 1), (7, 'sat', 4, 1), (7, 'sun', 4, 1),
(8, 'mon', 4, 1), (8, 'tue', 4, 1), (8, 'wed', 4, 1), (8, 'thu', 4, 1), (8, 'fri', 4, 1), (8, 'sat', 4, 1), (8, 'sun', 4, 1);*/

INSERT INTO staff (email, ssn, date_of_birth, position, first_name, last_name, street, city, unit, state, zipcode, phone_number, phone_type, gender, dept_id) 
VALUES 
('john.doe@example.com', '123-45-6789', '1985-05-25', 'Software Engineer', 'John', 'Doe', '123 Main St', 'Anytown', 'Apt 101', 'CA', '12345', '123-456-7890', 'Work', 'M', 6),
('jane.smith@example.com', '987-65-4321', '1990-07-15', 'Network Engineer', 'Jane', 'Smith', '456 Elm St', 'Otherville', 'Unit B', 'NY', '54321', '098-765-4321', 'Mobile', 'F', 3),
('mark.johnson@example.com', '456-78-9012', '1982-11-30', 'Database Administrator', 'Mark', 'Johnson', '789 Oak St', 'Sometown', 'Unit C', 'TX', '67890', '456-789-0123', 'Home', 'M', 6),
('alice.wong@example.com', '789-01-2345', '1988-02-10', 'Software Developer', 'Alice', 'Wong', '111 Pine St', 'Cityville', 'Unit 1', 'WA', '11111', '111-222-3333', 'Work', 'F', 6),
('bob.smith@example.com', '234-56-7890', '1995-09-20', 'System Administrator', 'Bob', 'Smith', '222 Cedar St', 'Townville', 'Apt 202', 'OR', '22222', '222-333-4444', 'Mobile', 'M', 5),
('emily.jones@example.com', '567-89-0123', '1987-04-05', 'IT Support Specialist', 'Emily', 'Jones', '333 Maple St', 'Villageville', 'Unit 3', 'FL', '33333', '333-444-5555', 'Work', 'F', 5),
('alexander.brown@example.com', '890-12-3456', '1992-10-15', 'Security Analyst', 'Alexander', 'Brown', '444 Birch St', 'Hamletville', 'Apt 404', 'CA', '44444', '444-555-6666', 'Home', 'M', 4),
('olivia.davis@example.com', '012-34-5678', '1989-12-25', 'Business Analyst', 'Olivia', 'Davis', '555 Walnut St', 'Ruraltown', 'Unit 5', 'NY', '55555', '555-666-7777', 'Work', 'F', 8),
('michael.wilson@example.com', '345-67-8901', '1980-08-12', 'IT Manager', 'Michael', 'Wilson', '666 Oak St', 'Suburbia', 'Unit 6', 'TX', '66666', '666-777-8888', 'Mobile', 'M', 5),
('sarah.thomas@example.com', '678-90-1234', '1993-03-18', 'Software Engineer', 'Sarah', 'Thomas', '777 Elm St', 'Townsville', 'Apt 707', 'WA', '77777', '777-888-9999', 'Work', 'F', 6),
('kevin.moore@example.com', '901-23-4567', '1984-06-22', 'Network Administrator', 'Kevin', 'Moore', '888 Maple St', 'Villagetown', 'Unit 8', 'CA', '88888', '888-999-0000', 'Mobile', 'M', 3),
('linda.harris@example.com', '234-56-7890', '1991-09-14', 'Database Developer', 'Linda', 'Harris', '999 Pine St', 'Citytown', 'Unit 9', 'NY', '99999', '999-000-1111', 'Work', 'F', 7),
('david.thompson@example.com', '567-89-0123', '1978-11-28', 'Sr. Scrum Master', 'David', 'Thompson', '1010 Cedar St', 'Hamletown', 'Apt 101', 'OR', '10101', '101-202-3030', 'Mobile', 'M', 7),
('amy.lewis@example.com', '890-12-3456', '1986-05-07', 'IT Analyst', 'Amy', 'Lewis', '1111 Oak St', 'Suburbville', 'Unit 11', 'TX', '11111', '111-222-3333', 'Work', 'F', 5),
('chris.white@example.com', '012-34-5678', '1994-08-09', 'Front Desk Assistant', 'Chris', 'White', '1212 Elm St', 'Ruralville', 'Apt 12', 'WA', '12121', '121-212-3434', 'Mobile', 'M', 1),
('mary.scott@example.com', '345-67-8901', '1983-01-16', 'Data Entry Clerk', 'Mary', 'Scott', '1313 Maple St', 'Citytown', 'Unit 13', 'CA', '13131', '131-313-4343', 'Work', 'F', 1),
('jason.carter@example.com', '678-90-1234', '1990-04-30', 'Outbound Specialist', 'Jason', 'Carter', '1414 Pine St', 'Hamletville', 'Apt 14', 'NY', '14141', '141-414-5454', 'Mobile', 'M', 2),
('laura.allen@example.com', '901-23-4567', '1987-07-28', 'Cybersecurity Officer', 'Laura', 'Allen', '1515 Oak St', 'Villageland', 'Unit 15', 'TX', '15151', '151-515-6565', 'Work', 'F', 4),
('steven.adams@example.com', '234-56-7890', '1979-09-02', 'Busines Representative', 'Steven', 'Adams', '1616 Cedar St', 'Suburbtown', 'Apt 16', 'WA', '16161', '161-616-7676', 'Mobile', 'M', 2),
('angela.hall@example.com', '567-89-0123', '1985-02-14', 'System Architect', 'Angela', 'Hall', '1717 Elm St', 'Ruralville', 'Unit 17', 'CA', '17171', '171-717-8787', 'Work', 'F', 7),
('matthew.cook@example.com', '890-12-3456', '1982-06-10', 'Financial Officer', 'Matthew', 'Cook', '1818 Maple St', 'Cityville', 'Apt 18', 'NY', '18181', '181-818-9898', 'Mobile', 'M', 8),
('susan.lee@example.com', '012-34-5678', '1996-12-08', 'Financial Auditor', 'Susan', 'Lee', '1919 Pine St', 'Villagetown', 'Unit 19', 'TX', '19191', '191-919-1010', 'Work', 'F', 8),
('andrew.morris@example.com', '345-67-8901', '1984-03-25', 'Inbound Coordinator', 'Andrew', 'Morris', '2020 Oak St', 'Townville', 'Apt 20', 'WA', '20202', '202-020-3030', 'Mobile', 'M', 2),
('lisa.murphy@example.com', '678-90-1234', '1988-05-12', 'Operation Manager', 'Lisa', 'Murphy', '2121 Cedar St', 'Hamletville', 'Unit 21', 'CA', '21212', '212-121-4343', 'Work', 'F', 6),
('ryan.kelly@example.com', '901-23-4567', '1981-08-28', 'Software Developer', 'Ryan', 'Kelly', '2222 Elm St', 'Suburbtown', 'Apt 22', 'NY', '22222', '222-222-5454', 'Mobile', 'M', 6),
('karen.bell@example.com', '234-56-7890', '1993-11-15', 'Sr. Desktop Technician', 'Karen', 'Bell', '2323 Maple St', 'Citytown', 'Unit 23', 'TX', '23232', '232-323-6565', 'Work', 'F', 5),
('daniel.cooper@example.com', '567-89-0123', '1980-02-10', 'Firewall Administrator', 'Daniel', 'Cooper', '2424 Pine St', 'Villageland', 'Apt 24', 'WA', '24242', '242-424-7676', 'Mobile', 'M', 3),
('jennifer.wood@example.com', '890-12-3456', '1986-06-25', 'Assistant Manager', 'Jennifer', 'Wood', '2525 Oak St', 'Ruralville', 'Unit 25', 'CA', '25252', '252-525-8787', 'Work', 'F', 1);

INSERT INTO staff (email, ssn, date_of_birth, position, first_name, last_name, street, city, unit, state, zipcode, phone_number, phone_type, gender, dept_id)
VALUES
('tom.hanks@example.com', '123-45-6789', '1956-07-09', 'Software Engineer', 'Tom', 'Hanks', '123 Main St', 'Anytown', 'Apt 101', 'CA', '12345', '123-456-7890', 'Work', 'M', 6),
('scarlett.johansson@example.com', '987-65-4321', '1984-11-22', 'Network Engineer', 'Scarlett', 'Johansson', '456 Elm St', 'Otherville', 'Unit B', 'NY', '54321', '098-765-4321', 'Mobile', 'F', 3),
('elon.musk@example.com', '456-78-9012', '1971-06-28', 'Database Administrator', 'Elon', 'Musk', '789 Oak St', 'Sometown', 'Unit C', 'TX', '67890', '456-789-0123', 'Home', 'M', 6),
('albert.einstein@example.com', '789-01-2345', '1879-03-14', 'Software Developer', 'Albert', 'Einstein', '111 Pine St', 'Cityville', 'Unit 1', 'WA', '11111', '111-222-3333', 'Work', 'M', 6),
('leonardo.dicaprio@example.com', '234-56-7890', '1974-11-11', 'System Administrator', 'Leonardo', 'DiCaprio', '222 Cedar St', 'Townville', 'Apt 202', 'OR', '22222', '222-333-4444', 'Mobile', 'M', 5),
('nikholas.tesla@example.com', '567-89-0123', '1856-07-10', 'IT Support Specialist', 'Nikholas', 'Tesla jr.', '333 Maple St', 'Villageville', 'Unit 3', 'FL', '33333', '333-444-5555', 'Work', 'M', 5),
('marie.curie@example.com', '890-12-3456', '1867-11-07', 'Security Analyst', 'Marie', 'Curie', '444 Birch St', 'Hamletville', 'Apt 404', 'CA', '44444', '444-555-6666', 'Home', 'F', 4),
('galileo.galilei@example.com', '012-34-5678', '1564-02-15', 'Business Analyst', 'Galileo', 'Galilei', '555 Walnut St', 'Ruraltown', 'Unit 5', 'NY', '55555', '555-666-7777', 'Work', 'M', 8),
('steve.jobs@example.com', '345-67-8901', '1955-02-24', 'IT Manager', 'Steve', 'Jobs', '666 Oak St', 'Suburbia', 'Unit 6', 'TX', '66666', '666-777-8888', 'Mobile', 'M', 5),
('alicia.keys@example.com', '678-90-1234', '1981-01-25', 'Software Engineer', 'Alicia', 'Keys', '777 Elm St', 'Townsville', 'Apt 707', 'WA', '77777', '777-888-9999', 'Work', 'F', 6),
('marilyn.monroe@example.com', '901-23-4567', '1926-06-01', 'Network Administrator', 'Marilyn', 'Monroe', '888 Maple St', 'Villagetown', 'Unit 8', 'CA', '88888', '888-999-0000', 'Mobile', 'F', 3),
('charles.darwin@example.com', '234-56-7890', '1809-02-12', 'Database Developer', 'Charles', 'Darwin', '999 Pine St', 'Citytown', 'Unit 9', 'NY', '99999', '999-000-1111', 'Work', 'M', 7),
('leonardo.da.vinci@example.com', '567-89-0123', '1452-04-15', 'Sr. Scrum Master', 'Leonardo', 'da Vinci', '1010 Cedar St', 'Hamletown', 'Apt 101', 'OR', '10101', '101-202-3030', 'Mobile', 'M', 7),
('adam.lovelace@example.com', '890-12-3456', '1815-12-10', 'IT Analyst', 'Adam', 'Lover', '1111 Oak St', 'Suburbville', 'Unit 11', 'TX', '11111', '111-222-3333', 'Work', 'F', 5),
('mary.curie@example.com', '012-34-5678', '1898-01-01', 'Front Desk Assistant', 'Mary', 'Curie', '1212 Elm St', 'Ruralville', 'Apt 12', 'WA', '12121', '121-212-3434', 'Mobile', 'F', 1),
('stephen.hawking@example.com', '345-67-8901', '1942-01-08', 'Data Entry Clerk', 'Stephen', 'Hawking', '1313 Maple St', 'Citytown', 'Unit 13', 'CA', '13131', '131-313-4343', 'Work', 'M', 1),
('grace.hopper@example.com', '678-90-1234', '1906-12-09', 'Outbound Specialist', 'Grace', 'Hopper', '1414 Pine St', 'Hamletville', 'Apt 14', 'NY', '14141', '141-414-5454', 'Mobile', 'F', 2),
('nikola.jones@example.com', '901-23-4567', '1856-07-10', 'Cybersecurity Officer', 'Nikola', 'Jones', '1515 Oak St', 'Villageland', 'Unit 15', 'TX', '15151', '151-515-6565', 'Work', 'M', 4),
('dwade.howard@example.com', '234-56-7890', '1815-12-10', 'Busines Representative', 'Dwadee', 'Howard', '1616 Cedar St', 'Suburbtown', 'Apt 16', 'WA', '16161', '161-616-7676', 'Mobile', 'F', 2),
('alberto.einsteinson@example.com', '567-89-0123', '1879-03-14', 'System Architect', 'Alberto', 'Einsteinson', '1717 Elm St', 'Ruralville', 'Unit 17', 'CA', '17171', '171-717-8787', 'Work', 'M', 7),
('hedy.lamarr@example.com', '890-12-3456', '1914-11-09', 'Financial Officer', 'Hedy', 'Lamarr', '1818 Maple St', 'Cityville', 'Apt 18', 'NY', '18181', '181-818-9898', 'Mobile', 'F', 8),
('grace.hockerson@example.com', '012-34-5678', '1906-12-09', 'Financial Auditor', 'Grace', 'Hickerson', '1919 Pine St', 'Villagetown', 'Unit 19', 'TX', '19191', '191-919-1010', 'Work', 'F', 8),
('Peter.curie@example.com', '345-67-8901', '1867-11-07', 'Inbound Coordinator', 'Pater', 'Curie', '2020 Oak St', 'Townville', 'Apt 20', 'WA', '20202', '202-020-3030', 'Mobile', 'M', 2),
('stephen.curry@example.com', '678-90-1234', '1942-01-08', 'Operation Manager', 'Stephen', 'Curry', '2121 Cedar St', 'Hamletville', 'Unit 21', 'CA', '21212', '212-121-4343', 'Work', 'M', 6),
('ada.lovelace@example.com', '901-23-4567', '1815-12-10', 'Software Developer', 'Ada', 'Lovelace', '2222 Elm St', 'Suburbtown', 'Apt 22', 'NY', '22222', '222-222-5454', 'Mobile', 'F', 6),
('nikola.tesla@example.com', '234-56-7890', '1856-07-10', 'Sr. Desktop Technician', 'Nikola', 'Tesla', '2323 Maple St', 'Citytown', 'Unit 23', 'TX', '23232', '232-323-6565', 'Work', 'M', 5),
('grace.melody@example.com', '567-89-0123', '1906-12-09', 'Firewall Administrator', 'Grace', 'Hopkins', '2424 Pine St', 'Villageland', 'Apt 24', 'WA', '24242', '242-424-7676', 'Mobile', 'F', 3),
('marie.magdaline@example.com', '890-12-3456', '1867-11-07', 'Assistant Manager', 'Marie', 'Magdaline', '2525 Oak St', 'Ruralville', 'Unit 25', 'CA', '25252', '252-525-8787', 'Work', 'F', 1);


INSERT INTO staff (email, ssn, date_of_birth, position, first_name, last_name, street, city, unit, state, zipcode, phone_number, phone_type, gender, dept_id) 
VALUES 
('admin@umbc.edu', '843-89-0129', '1995-05-18', 'UX Web Designer', 'WILLY', 'NGORAN', '1000 Hiltop cir.', 'Baltimore', 'CMSC447', 'MD', '21250', '(443)985-2774', 'Mobile', 'M', 7);

INSERT INTO credentials (email, pswd, clear) VALUES
('admin@umbc.edu', 'root', 1);

