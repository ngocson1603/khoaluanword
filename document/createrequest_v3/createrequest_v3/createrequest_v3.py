import sys,requests,urllib
import json,os,time
import datetime
from email.parser import Parser
from functions import read_file
from functions import constructJson_Request,putURL,postURL,postURL_V3
from functions import getURL,getResponseStatus,constructFiles_Attachment
import requests_toolbelt.multipart.encoder
import urllib3
urllib3.disable_warnings(urllib3.exceptions.InsecureRequestWarning)
#time.sleep(60)

config_filepath = os.getcwd()+"/configuration.json"
configuration = read_file(config_filepath,"configuration")

appUrl = configuration['url'] #Update this link with the protocol and servername:portnumber values of the ServiceDesk Plus application.
TechnicianKey = configuration['technicianKey']#Replace 374C0176-A6BF-4CDC-8E05-9731F66FAE71 with a Technician's API key.

resultjson={}
url=""
# File containing request details will be stored as json object and the file path will be passed as argument to the script replacing the $COMPLETE_JSON_FILE argument 
file_Path = sys.argv[1]


def deletereq(requestid,opsName,data):
	if(opsName=="DELETE_REQUEST"):
		
		url = appUrl + "/api/v3/requests/"+workorderid+"/move_to_trash"
	with requests.Session() as s:
		data = {'TECHNICIAN_KEY': TechnicianKey,'format':'json'}
		r = s.delete(url,headers=data,verify = False)		
	return(r)


# Load the json content which contains request details
with open(file_Path,'r', encoding='utf-8') as data_file:
    data = json.load(data_file)
requestObj = data['request']
portalid = read_file(file_Path,"PORTALID")


#Assigning value got from the Request JSON Object to variables which can be used in constructing the JSON for creating the New Request
workorderid = requestObj['id']
requester = requestObj['requester']['name']
subject = requestObj['subject']
#requesttemplate = requestObj['REQUESTTEMPLATE']
#description = requestObj['DESCRIPTION'].replace(u'\xc2\xa0', u' ')
description = requestObj['description']
#description = description.replace("Â", "")
subject = subject.lower()

template=""

if ("request for laptop" in subject):
	template = "Request a Laptop"
elif ("new employee request" in subject):
	template = "New Employee Onboarding"
elif ("request for a mail list" in subject):
	template="Request a new mailing list creation"
elif ("New hire request EMEA" in subject):
	template="Request Enhancement in ServiceDesk Plus"

#print(template)

if(template != ""):
	requestinside={}
	requestinside["request"]={}
	requestinside['request']['requester']={}
	requestinside['request']['requester']['name']=requester
	requestinside['request']['subject']=subject+"--SR"
	requestinside['request']['template']={}
	requestinside["request"]['template']['name']=template
	requestinside['request']['description']=description
	requestinside['request']['status']={}
	requestinside['request']['status']['name']="Open"
	jsonData = json.dumps(requestinside)
	# print(jsonData)

	requesturl = appUrl + "/api/v3/requests"
	r = postURL_V3(requesturl,TechnicianKey,json_data=jsonData,portalid=portalid)
	responseStatus = getResponseStatus(requesturl,r)

	if(r.status_code == 201 or r.status_code == 200):
		responseobj=r.json()
		#print(responseobj)
		status=responseobj['response_status']['status']
		# message=responseobj['operation']['result']['message']
		childID = responseobj['request']['id']
		print("child ticket created with id:::"+ childID)
			  

		if status == 'success':
			#print("inside")#Checking if the status value in the return json is set as "Success".
			resultjson["result"]="success"
			resultjson["message"]="Template set Successfully"
			print(resultjson) #This message will be shown if the Level was updated Successfully.

			result = deletereq(workorderid,"DELETE_REQUEST",jsonData)
					
		else: 
			resultjson["result"]="Failed"
			resultjson["message"]="status " + str(status)
			print(resultjson)
	else:
		resultjson["result"]="Failed"
		resultjson["message"]=r.text
		print(resultjson)
else:
	resultjson["result"]="Success"
	resultjson["message"]="No Changes Done"
	print(resultjson)