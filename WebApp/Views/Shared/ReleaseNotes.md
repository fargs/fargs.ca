
##Release 1.0.0-alpha.3

*2015-07-16*

- Added a basic dashboard with placeholders and a button to Book an Appointment and to View a list of appointments.
- Prototyped the Book an Appointment form. This is all mock data meaning nothings is actually hooked into the database. It is intended to get early feedback on the look and feel of the form.
- Prototyped the appointment list. This is all mock data as well. 
- The status column reflects the workflow: After the booking, if it will either be automatically accepted or have a status of Not Responded. The physician will either accept or reject. If rejected, the company is encouraged to propose another time. In the action menu, they can also re-book with another physician or cancel it completely. Once accepted, there a "Job" will get created with a list of tasks. The job represents the "billable" service from the doctor. A file could have more than one job as seen with ML0003 (MedLegal) and AD0003 (Addendum).
- Other things like document count and the file id is linkable, etc.
- The booking form has a Select Available Time where eventually this will display the physician's availability and they can use it to select the "When" value. Upload documents doesn't work yet but I put it there so you can think about fields we would want.