
Availability

Melissa will get the physicians availability and enter it into the system. 

She will first add the days the physician would like to work. She will then collect the time slots they would like to work.

They can add assessment slots (a location is required) and teleconference slots (location is not applicable)

Melissa may reserve a time slot 



Bookings

Melissa has put out availability and has companies reaching out looking to book assessments.

She confirms the booking and finds the time slot and enters the details of the assessment.

She enters the claimant name, the system confirms that a case does not already exist.

	- if exists
		- add a workflow to the existing case 
	- else
		- create a new case and add a workflow


Melissa sometimes will get requests for services where an appointment is not applicable. Examples of these are paper reviews and addendums.

She will click the New button. 

She will first enter the claimant name (Auto complete will show the claimant, company)

	- if existing claimant is selected
		- open the case details page
	- else
		- create a new case

	- display add service form
	- if service has an appointment
		- system will show the available slots
		- she will select an available slot 
		
	- if service has a location
		- she will select the location

	- save the service



DESIGN

- Each company has a list of services, each service has a single workflow.

- A workflow must fall into one of three categories:
	- Appointment at Location
	- Appointment Only
	- No Appointment

Examples:

Service = Accident Benefit
Workflow = Standard Assessment (Appointment at Location)

Service = Teleconference
Workflow = Teleconference (Appointment Only)

Service = Paper Review
Workflow = Addendum (No Appointment)
