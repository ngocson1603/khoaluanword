#This Python script is used to create a task under a Change Request in ServiceDesk Plus,based on the value that is selected in a specific field.
#Works on python version 3.4.1 and above.

#Below are the list of Packages used in this Script.We have used a Python Library called 'Requests' which is not bundled by default with Python installation.
#More information on this Package and the instructions on installing it on the Application Server are available in the following link.
# http://docs.python-requests.org/en/latest/user/install/#install
import requests
import sys
import json

#Reading the argument passed to the script.The ChangeRequest details are stored as a JSON Object in a file and its path is provided to the Script as input.
file_Path = sys.argv[1]
with open(file_Path) as data_file:
    data = json.load(data_file)

#Assigning value got from the Change Request fields to variables.
inputData = data["INPUT_DATA"]
changeData = inputData["entity_data"]   
changeid = str(changeData["id"])
changedesc = changeData["description"]

#Below is a Sample of the Change Details in Json Format.Please note that there is a Change in the ChangeRequest Json after build 9203 of SDP.
'''
{"INPUT_DATA":{"entity":"change","login_name":"administrator","entity_data":{"id":1,"template":{"id":1,"name":"General Template"},"workflow":{"id":1,"name":"SDGeneral"},"SLAID":"","ISOVERDUE":null,"type":{"id":3,"name":"Significant"},"group":null,"category":{"id":11,"name":"Operating System"},"subcategory":{"id":23,"name":"Windows Server 2003"},"item":null,"impact":{"id":1,"name":"High"},"urgency":{"id":2,"name":"High"},"priority":{"id":4,"name":"High"},"risk":{"id":3,"name":"High"},"scheduled_start_on":{"value":1475697180000,"display_value":"Oct 5, 2016 03:53 PM"},"scheduled_end_on":null,"created_on":{"value":1475610888931,"display_value":"Oct 4, 2016 03:54 PM"},"completed_on":null,"services_affected":[{"id":2,"name":"Communication"}],"assets":[],"title":"Testing Read Json","reason_for_change":{"id":5,"name":"Patch updates"},"description":"Type Desc here","state":{"stage":{"id":1,"name":"Submission"},"status":{"id":3,"name":"Requested"},"comments":""},"roles":[{"id":1,"name":"ChangeManager","users":[{"id":3,"name":"Gopinath"}]},{"id":2,"name":"ChangeOwner","users":[{"id":6,"name":"Balaji"}]},{"id":3,"name":"ChangeRequester","users":[{"id":902,"name":"Prem"}]},{"id":4,"name":"CAB","users":[]},{"id":5,"name":"ChangeApprover","users":[{"id":9,"name":"Pugazh"},{"id":1801,"name":"Bala"}]},{"id":6,"name":"Line Manager","users":[]},{"id":7,"name":"Implementer","users":[]},{"id":8,"name":"Reviewer","users":[]}]},"entity_diff_data":{"type":{"old":{"id":2,"name":"Major"},"new":{"id":3,"name":"Significant"}}}}}
'''

#Create a Session Object that will be used to pass different parameters across the HTTP requests.
#All the parameters like the ServerName , protocol , portnumber and Technician API key that are used in the Script are Input here.Please update this based on your Environment.
with requests.Session() as s:
    url = "http://localhost:8080"#Update this link with the protocol and servername:portnumber values of the ServiceDesk Plus application.

TechnicianKey='D6B4633F-6CBC-4BFF-A907-F5BDFC37D21A' #Replace D6B4633F-6CBC-4BFF-A907-F5BDFC37D21A with a Technician's API key.

#Below is the sample JSON format for adding a Task:
'''{"task": {  "title": "Update SDPLIVEHELP","description": "Content consolidation","status": {"name": "Open"},"change": {"id": "7"}}'''

#Creating the json object for the Task Create operation and storing it in the variable 'jsonData'
jsonData ='''{
    "task": {
        "task_type": {
            "name": "Implementation"
        },
        "change": {"id": "'''+changeid+'''"},
        "title": "Check the Telephony Setting for the lines listed in the Description",
        "description": "'''+changedesc+'''",
        "status": {
            "name": "Open"
        },
    }
}'''
json_data = json.dumps(jsonData)

#Constructing the url for the API call and submitting that to the ServiceDesk Plus server
apprUrl = url + "/api/v3/tasks"
data = {'INPUT_DATA' : jsonData ,'TECHNICIAN_KEY': TechnicianKey}
r = s.post(apprUrl,data)
#print(r.status_code)	

if(r.status_code == 200):#Checking if the API Request was Submitted successfully.The Status Code 200 is returned if the POST operation was successful.
  print("Task Added successfully through Custom Triggers")#This message can be customized based on your requirement.
  print(r.json())
else :
  print("problem submitting request")
  print(r.json())
 
