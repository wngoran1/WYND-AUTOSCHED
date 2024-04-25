Login Page:
* cross check inputted info with database to see if that exists. If so, allow the person in and set their acct so we can see schedule.
  
Create Acct Page:
* We assume person has their acct #
* Able to put in acct # and the password they want, they also need to put in their name and DOB to verify to make sure the person is legit.
* after that confirmation, store the new info, including hashed password into database. https://learn.microsoft.com/en-us/dotnet/api/microsoft.aspnetcore.identity.passwordhasher-1?view=aspnetcore-8.0 for more info about hashing passwords.

Main Menu / Index / View Schedule:
* since they logged in, page has the acct now, so now the main page will grab schedule info based on the acct.
* Backend will grab info from database regarding all of the active shifts for a user.
* Top menu buttons allow you to view profile, availability, change availability.
* If manager, more options including altering business needs, adding new employees, and view/edit schedules.
* - same dropdown, just a few more included.

View Profile:
- grab info from database for employee (Name, DOB, other info?)
  
View Availability:
- grab info from database for employee (whatever availability they have, give it to the website so front end can display it nicely)
- 
Request Availability Change:
- grab info from database for employee (whatever availability they have, give it to the website so front end can display it nicely)
-
-  Once user makes edits, the backend will then save that information to the database under a DIFFERENT TABLE. This is becausue the manager has to approve or deny it.
Manager alter business needs:
- grab current bussiness needs from table.
- keep track of edits that the manager makes
- once they click submit, update table to the new one.
  
Manager add new employee:
* Manager adds a new employee, needs to check with database to ensure that user does not already exist.
* If no conflicts, add the new employee into the database (Their acct #, all other info, not the username and pass part though)

Manager view team schedules:
* pull info from database regarding the selection that the user puts in, defaulting to today's date.

Manager schedule editor:
* grab current schedule from database
* keep track of new shifts/edits that the manager inputs
* backend will need to update the database to implement the changes.

Manager approve/deny availability changes.
- manager can view all availability changes. This means that database will pull up all of the ones in the request table and then display it to the manager.
- 1) backend pulls all of the names off the database. Once manager selects one name:
- 2) backend will pull up the availability request. Then the manager can accept or deny.
  3) If accept, update the ACTUAL availability of the user to the one they requested. update to notification table notif to user saying accepted.
  4) If deny, do not update actual availability. Add notification to notif table with the user saying denied for whatever reason they inputted.

Other backend (a major part):

* Algorithm to create a schedule automatically, taking in the business needs and the available people
*   possible scenarios that may make errors and should have a way to handle
*   - too many business needs, not enough employees. (In this case, fill in shifts where we can to have most shifts covered, and alert manager)
    - not enough business needs, too many employees. (In this case, just less people are scheduled)
    -   make sure people dont go over 5 days a week.
    -   make sure people dont go over 40 hours a week.
 
* Notification table in database.
* - When a person clicks notification bell, they will see any notifs that are assigned to the min the table. If they clear notifications, need to delete from notif table.
