-- sample data
insert into department(dept_name) values ("Customer Service"), ("Public relation"), ("Network Engineering"), 
("System Security"), ("IT Support"), ("IT Operations"), ("Project Management"), ("Business Analysis");

insert into schedule_type(pattern) values ("MTWUFXX"), ("XXWUFAS"), ("MTXXFAS"), ("XTWUFAX");

INSERT INTO weekly_business_need (dept_id, schedule_id, ratio)
VALUES
(1, 1, 0.5),
(1, 3, 0.5),
(6, 1, 0.4),
(6, 2, 0.3),
(6, 3, 0.3),
(6, 4, 0);

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
