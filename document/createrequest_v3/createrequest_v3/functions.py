# $Id$
#!/usr/bin/python
# -*- coding: utf-8 -*-
import smtplib
from smtplib import SMTPException
from os.path import basename
from email.mime.application import MIMEApplication
from email.mime.multipart import MIMEMultipart
from email.mime.text import MIMEText
from email.utils import COMMASPACE, formatdate
from zipfile import ZipFile
import json
import urllib
import requests
import datetime
import time
import os
import requests
import json
import operator
import os
import re

import xml.etree.ElementTree as ET


# ------------------ Function to parse input from Request JSON file----------------------


def read_file(file_Path, key=None):
    with open(file_Path) as data_file:
        data = json.load(data_file)
    if key is None:
        return data
    elif key in data:
        dataObj = data[key]
        return dataObj
    else:
        return None


# ----------------------------  Construct Task Input Json  ----------------------------

# send data as jsonstring
# use json.dumps function to convert dict to jsonstring
def constructJson_Task(data, module, id):
    if module is not None:
        json_data = ''' {"task": {"'''+module+'''": { "id": ''' +id + ''' }, ''' + data + '''} }'''
    else:
        json_data = ''' {"task": ''' + data + '''}'''
    return json_data


# ----------------------------  Construct Worklog Input Json  ----------------------------

def constructJson_Worklog(data, module, id):
    json_data = ''' {"worklog": { "'''+module+'''": { "id": ''' +id + ''' }, ''' + data + '''} }'''
    return json_data


# ----------------------------  Construct Request Input Json  ----------------------------

def constructJson_Request(data):
    json_data = ''' {"operation": { "details": ''' + data + '''} }'''
    return json_data


def constructJson_Request_V3(data):
    json_data = ''' {"request": ''' + data + '''}'''
    return json_data

# ----------------------------  Construct commmon Input Json  ----------------------------


def constructJson_input_V3(module,data):
    json_data = ''' {"'''+module+'''": ''' + str(data) + '''}'''
    return json_data


# ----------------------------  Construct Change Input Json  ----------------------------

def constructJson_Change(data):
    json_data = ''' {"operation": { "details": ''' + data + '''} }'''
    return json_data


# ----------------------------  Construct Note Input Json  ----------------------------

# Example:  data = """{ "ispublic":"true","notestext": """+notecontent+"""} """

def constructJson_Note(data):
    json_data = '''{ "operation": { "details": { "notes": { "note": '''+data+'''}}}}'''
    return json_data

#----------- Function to make request share to Users/Department/site/Group/tech -------------------
# Will return old value present in the diff json

def constructJson_shareRequest(groups=None,technicians=None,users=None,department=None,sites=None,parameter=None):
    
    tempDataObj={}

    if parameter is None:
        parameter="name"

    if isinstance(groups, list):
        tempDataObj['groups']=getListAsDictionary(groups,parameter)

    if isinstance(technicians, list):
        tempDataObj['technicians']=getListAsDictionary(technicians,parameter)

    if isinstance(sites, list):
        tempDataObj['sites']=getListAsDictionary(sites,parameter)

    if isinstance(users, list):
        tempDataObj['users']=getListAsDictionary(users,parameter)
   
    if isinstance(department, list):
        tempDataObj['departments']=getListAsDictionary(department,parameter)

    json_data = ''' {"share": ''' + json.dumps(tempDataObj) + '''}'''
    return json_data

# ------------------ Function to get the field details from the json -------------------

def getFieldDetails(responseObj, field):
    value = 'Error'
    status = responseObj['operation']['result']['status']
    if status == 'Success':
        for elem in responseObj['operation']['details']:
            if str(elem['NAME']) == field:
                value = elem['VALUE']
    return value


# ------------------ Function to send mail -------------------

# sent to and ccAdd should be commma seperated email strings
# configuration is a dictionary
def send_mail(configuration,subject,text, toAdd=None,ccAdd=None,files=None):
    server = configuration['server']
    port = configuration['port']
    username = configuration['loginname']
    password = configuration['password']
    fromAdd = configuration['fromAddress']
    
    if toAdd is None:
        send_to=configuration['toAddress']
    else:
        send_to=toAdd

    if isinstance(send_to,list):
        send_to = ",".join(send_to)
    if isinstance(ccAdd,list):
        ccAdd = ",".join(ccAdd)
    send_toList = send_to.split(',')
    if ccAdd is not None:
        ccAddList=ccAdd.split(',')
    else:
        ccAdd=''
        ccAddList=[]
    
    try:
        msg = MIMEMultipart()
        msg['From'] = fromAdd
        msg['To'] = send_to
        msg['CC'] = ccAdd
        msg['Date'] = formatdate(localtime=True)
        msg['Subject'] = subject
        msg.attach(MIMEText(text,'html'))

        send_toList = send_toList + ccAddList

        for f in files or []:
            with open(f, "rb") as fil:
                part = MIMEApplication(
                    fil.read(),
                    Name=basename(f)
                )
            # After the file is closed
            part['Content-Disposition'] = 'attachment; filename="%s"' % basename(f)
            msg.attach(part)
           
        smtp = smtplib.SMTP(server, port)
        #smtp.login(username, password) #uncomment if required
        
        smtp.sendmail(fromAdd, send_toList, msg.as_string())
        q = "Successfully sent email"
    except SMTPException as error:
        q = "Error: unable to send email :  {err}".format(err=error) #This message will appear in the Request History and can be modified based on requirement.
    finally:
        smtp.close()
    return q

# ------------------ Function to add approvers to a stage -------------------

def create_approval_JSON(operationName, operationJson,send_immediately="false"):
    operationJson["send_immediately"]=send_immediately
    resultjson = {}
    resultjson['operation'] = []
    resultjson['result'] = 'success'
    message = 'Sample Python script for ' + operationName
    resultjson['message'] = message
    operationJson['OPERATIONNAME'] = operationName
    resultjson['operation'].append(operationJson)
    return resultjson


def add_approval_stage(operationJson, stage, approversList):
    approvalArray = {stage: []}
    for approver in approversList:
        approvalArray[stage].append(approver)    
    operationJson['INPUT_DATA'].append(approvalArray)
    return operationJson

# ------------------ Function to update request -------------------

def create_update_JSON(operationJson,message = 'Sample Python script for EDIT_REQUEST'):
    resultjson = {}
    resultjson['operation'] = []
    resultjson['result'] = 'success'
    resultjson['message'] = message
    operationJson['OPERATIONNAME'] = "EDIT_REQUEST"
    resultjson['operation'].append(operationJson)
    return resultjson


# format will be below
## '''{"result":"success","operation":[{"OPERATIONNAME":"EDIT_REQUEST","INPUT_DATA":[{"subject":"test subject","description":"test description"}]}],"message":"subject has been updated."}

def update_field(operationJson, field, value):
    obj = {}
    obj[field] = value
    if("INPUT_DATA" in operationJson and len(operationJson["INPUT_DATA"])==0):
        operationJson['INPUT_DATA'].append(obj)
    else:
        operationJson['INPUT_DATA'][0].update(obj)
    return operationJson


# ------------------ Function to update request V3 -------------------
# Sample Format will be
# {"result":"success","message":"Sample Python script","operation":[{"OPERATION_NAME":"UPDATE","FORMAT":"V3",
# "INPUT_DATA":[{"request":{"urgency":{"name":"High"},"group":{"name":"Network"},"priority":{"name":"High"}}}]}]}

def create_update_JSON_V3(operationJson,message = 'Python script for EDIT_REQUEST'):
    resultjson = {}
    resultjson['operation'] = []
    resultjson['result'] = 'success'
    message = message
    resultjson['message'] = message
    operationJson['OPERATIONNAME'] = "UPDATE"
    operationJson['FORMAT'] = "V3"    
    resultjson['operation'].append(operationJson)
    # print(resultjson)
    return resultjson

def update_field_V3(operationJson, field, value,udf_fields=None,resources=None,isDate=False,resolution=None):
    obj= {}
    obj["request"]={}
    if(resources is not None):
        obj["request"]["resources"]={}
        obj["request"]["resources"][resources]={}
        obj["request"]["resources"][resources][field] = value
    elif(udf_fields is not None):
        if(isDate==True):
            obj["request"]["udf_fields"]={}
            obj["request"]["udf_fields"][field] = {}
            obj["request"]["udf_fields"][field]["value"] = value
        else:
            obj["request"]["udf_fields"] = {}
            obj["request"]["udf_fields"][field] = {}
            obj["request"]["udf_fields"][field]["value"]=value
    elif(isDate==True):
        obj["request"][field]={}    
        obj["request"][field]["value"] = value
    elif resolution is not None:
        obj["request"]['resolution']={}    
        obj["request"]['resolution']['content'] = resolution
    else:
        obj["request"][field]={}    
        obj["request"][field]["name"] = value

    if("INPUT_DATA" in operationJson and len(operationJson["INPUT_DATA"])==0):
        operationJson['INPUT_DATA'].append(obj)
    else:
        operationJson['INPUT_DATA'][0]['request'].update(obj['request'])
    # printz
    return operationJson

# ------------------ Function to get current date -------------------

def getcurrentdate(date_format="%Y-%m-%d"):
    now = datetime.datetime.now()
    currentdate = now.strftime(date_format)
    currentdate = datetime.datetime.strptime(currentdate, date_format)
    return currentdate


# ------------------ Function to get date -------------------

def getdate(date_param, date_format="%Y-%m-%d"):
    date = datetime.datetime.fromtimestamp(int(date_param)/1000)
    date = date.strftime(date_format)
    date = datetime.datetime.strptime(date, date_format)
    return date

# ------------------ Function to get date -------------------

def istoday_first_day(dt, d_years=0, d_months=0):
    # d_years, d_months are "deltas" to apply to dt
    y, m = dt.year + d_years, dt.month + d_months
    a, m = divmod(m-1, 12)
    return datetime.date(y+a, m+1, 1)

# ------------------ Function to get date -------------------

def istoday_last_day():
    last_day=False
    dt = datetime.datetime.now().date()    
    dt1=istoday_first_day(dt, 0, 1) + datetime.timedelta(-1)
    if(dt==dt1):
        last_day=True
    return last_day

# ------------------ Function to get date as string -------------------

def getdatestring(unixtime, date_format='%d %b %Y, %H:%M:%S'):
    time = int(unixtime)
    if time > 0:
        date = datetime.datetime.fromtimestamp(time/1000)
        date = date.strftime(date_format)
    else: 
        date = 'N/A'
    return date

# ------------------ Function to unix date as string and replace date and time ------------

def setDateTime(unixtime,hourvalue=None,minutevalue=None):    
    date = datetime.datetime.strptime(getdatestring(unixtime), '%d %b %Y, %H:%M:%S')
    if(hourvalue is not None and minutevalue is not None):
        newdate = date.replace(hour=hourvalue,minute=minutevalue)
    elif(hourvalue is not None):
        newdate = date.replace(hour=hourvalue)
    elif(minutevalue is not None):
        newdate = date.replace(hour=hourvalue)
    return newdate

def convertDate(dateVal):
    return int(datetime.datetime.strptime(dateVal,"%d %b %Y, %I:%M:%S %p").timestamp() * 1000)

def addunixtime(actualtime,addhr=None,addmin=None):
    
    deltatime=0
    if(addhr!=None):
        deltatime=addhr * 60 * 60
    elif(addmin!=None):
        deltatime=addmin * 60
    deltatime=deltatime*1000
    actualtime=actualtime+deltatime
    
    return actualtime


# ------------- Function to add Parameter in CI request -------------

# Need to call like below.
# criteria = addParameter("CI Type","Windows Workstation","START WITH")

def addParameter(name,value,compOperator=None,relOperator=None):
    if compOperator:
        name = " compOperator=\'" + compOperator + "\'>"+ name
    else:
        name = ">" + name
    parameter = ''' <parameter>
                <name''' + name + '''</name>
                <value>''' + value + '''</value>
            </parameter>'''
    if relOperator:
        parameter = parameter + "<reloperator>" + relOperator +"</reloperator>"
    return parameter
# ------------- Function to add return fields in CI request -------------

def addReturnFields(rfArray):
    returnFields = ""
    for rf in rfArray:
        returnFields += '''<name>'''+rf+'''</name>'''
    return returnFields

# ------------------ Function to add CI -------------------

def addCI_inputData(parameter):
    inputdata = '''<API version='1.0' locale='en'> <records> <record>''' + parameter + '''</record> </records> </API>'''
    return inputdata


# ------------------ Function to update CI -------------------

def updateCI_inputData(citype,input_param,update_param):
    inputdata = '''<?xml version='1.0' encoding='UTF-8'?><API version='1.0'><citype><name>'''+citype+'''</name><criterias><criteria>'''+input_param+'''</criteria></criterias><newvalue><record>'''+update_param+'''</record></newvalue></citype></API>'''
    return inputdata

# ------------------ Function to get Requester CI -------------------

def getRequesterCI_inputData(citype,input_param,returnFields):
    inputdata = '''<?xml version='1.0' encoding='UTF-8'?><API version='1.0'><citype><name>'''+citype+'''</name><criterias><criteria>'''+input_param+'''</criteria></criterias><returnFields>'''+returnFields+'''</returnFields></citype></API>'''
    return inputdata

# ------------- Function to add Names in CI request -------------

# Need to call like below..
# returnparameters = addNames_CI(["CI Name", "CI Type"])

def addNames_CI(list):
    names = ""
    for item in list:
        names = names + "<name>" + item + "</name>"
    return names

# ------------------ Function to get CI -------------------

'''

Fetch CI details using Criteria.

# criteria = addParameter("CI Type","Windows Workstation","START WITH")
    # Refer above function syntax
# returnparameters = addNames_CI(["CI Name", "CI Type"])
    # Refer above function syntax
# sortparameters = addNames_CI(["CI Name","Site"])
    # Refer above function syntax
# sortOrder = "desc"
# startindex = "1"
# limit = "50"

'''

def getCI_inputData(criteria, returnparameters, sortparameters, sortOrder, startindex, limit):
    inputdata = ''' <API version='1.0' locale='en'> 
                <criterias> <criteria>''' + criteria + '''</criteria> </criterias> 
                <returnparameters>''' + returnparameters + '''</returnparameters> 
                <sortparameters sortOrder=\'''' + sortOrder + '''\'>''' + sortparameters + '''</sortparameters> 
                <range>
                    <startindex>''' + startindex + '''</startindex>
                    <limit>''' + limit + '''</limit>
                </range>
                </API>'''
    return inputdata

#---------------------------- Get Method -----------------------------------

def getURL(url, TechnicianKey, operationName=None, json_data=None, params_data=None,portalid=None):
    data = {
        'input_data': json_data,
        'TECHNICIAN_KEY': TechnicianKey,
        'format': 'json',
        'OPERATION_NAME': operationName,
        'PORTALID': portalid
        }
    params = {
        'input_data': params_data,
        'TECHNICIAN_KEY': TechnicianKey,
        'format': 'json',
        'PORTALID': portalid
        }
    with requests.Session() as s:
        return s.get(url,params= params, data= data)

#---------------------------- Get Method V3-----------------------------------

def getURL_v3(url, TechnicianKey, operationName=None, json_data=None, params_data=None):

    headers = {
                "technician_key":TechnicianKey
                }

    with requests.Session() as s:
        return s.get(url,params= params_data, data= json_data)

# ----------------------------  Construct Attachment Input Json  ----------------------------

def constructJson_Attachment(module, id):
    json_data = {"attachment": {module: { "id": str(id) } } }
    return json.dumps(json_data)


# ----------------------------  Delete All Attachments ----------------------------
def deleteAttachments(configuration,module,id):
    url = configuration['url'] + "/api/v3/attachments"

    json_data = constructJson_Attachment(module,id)

    r = getURL(url, configuration['technicianKey'], None,None, json_data)
    attachmentresponse = r.json()   

    try:
        if("attachments" in attachmentresponse ):
            attachmentData = attachmentresponse['attachments']
            for attachment in attachmentData:
                url = configuration['url'] + "/api/v3/attachments/" + attachment['id'] 
                deleteURL(url,configuration['technicianKey'])
            print("Successfully Deleted all attachments!")
    except:
        print("Deleting attachment failed!")


# ----------------------------  Approve a Solution ----------------------------
def approveSolution(configuration,solutionID):
    url = configuration['url'] + "/api/v3/solutions/"+solutionID+"/approve"

    r = putURL(url, configuration['technicianKey'])
    response = r.json()   
    status = response['response_status']['status']
    message = response['response_status']['messages'][0]['message']
    return status+" : "+message
        


# ----------------------------  Delete Method  ----------------------------

def deleteURL(url, TechnicianKey):
    data = {
        'TECHNICIAN_KEY': TechnicianKey,
        'format': 'json',
        }
    with requests.Session() as s:
        return s.delete(url,data=data)



#----------------------------- To bundle together attachments ------------------------


# fileNamesList = ['test1.txt', 'test2.txt']
# multiple_files = constructFiles_Attachment(fileNamesList)
# querystring = constructJson_Attachment(module,id)
# return postURL(url,TechnicianKey, params_data = querystring,files=multiple_files)

def constructFiles_Attachment(fileNamesList):
    multiple_files = []
    temp = 0
    for fileName in fileNamesList:
            if(os.path.isfile(fileName)):
                file = open(fileName, "rb")
                fileNameArr = fileName.split("\\")
                fileName = fileNameArr[len(fileNameArr)-1]
                obj =(fileName, file, 'text/plain')
                multiple_files.append(("file"+str(temp), obj))
                temp = temp+1
            else:
                print(fileName + " file not present")
    return multiple_files

# ----------------------------  Post Method  ----------------------------


# Function to send post http connection to the given url

# url  : Api url
# TechnicianKey 
# operationName : Specifies the operation name like ADD_REQUEST. Not mandatory
# json_data : Specifies the data in the request body. Not mandatory
# params_data : Invoke API call with input data in the request url params. Not mandatory
# files  : Related to attachment file data. Not mandatory

# Invoke be like
#response = postURL(baseURL, TechnicianKey, params_data=json_data)

def postURL(url, TechnicianKey, operationName=None, json_data=None, params_data=None, files=None):
    data = {
        'INPUT_DATA': json_data,
        'TECHNICIAN_KEY': TechnicianKey,
        'format': 'json',
        'OPERATION_NAME': operationName
        }
    params = {
        'INPUT_DATA': params_data,
        'TECHNICIAN_KEY': TechnicianKey,
        'format': 'json'
        }
    
    with requests.Session() as s:
        return s.post(url,data=data, params=params, files=files,verify=False)
        

def postURL_V3(url, TechnicianKey, operationName=None, json_data=None, params_data=None, files=None,portalid=None):
    data = {
        'input_data': json_data,
        'TECHNICIAN_KEY': TechnicianKey,
        'format': 'json',
        'OPERATION_NAME': operationName,
        'PORTALID': portalid
        }
    params = {
        'input_data': params_data,
        'TECHNICIAN_KEY': TechnicianKey,
        'format': 'json',
        'PORTALID': portalid
        }
    # print(url)
    # print(data)
    with requests.Session() as s:
        return s.post(url,data=data, params=params, files=files,verify=False)

# ----------------------------  Put Method  ----------------------------


# Function to send post http connection to the given url

# url  : Api url
# TechnicianKey 
# operationName : Specifies the operation name like ADD_REQUEST. Not mandatory
# json_data : Specifies the data in the request body. Not mandatory
# params_data : Invoke API call with input data in the request url params. Not mandatory
# files  : Related to attachment file data. Not mandatory

# Invoke be like
#response = putURL(baseURL, TechnicianKey, params_data=json_data)

def putURL(url, TechnicianKey, operationName=None, json_data=None, params_data=None, files=None):
    data = {
        'input_data': json_data,
        'TECHNICIAN_KEY': TechnicianKey,
        'format': 'json'
       }
    params = {
        'input_data': params_data,
        'TECHNICIAN_KEY': TechnicianKey,
        'format': 'json',
        }
    with requests.Session() as s:
        return s.put(url,data=data, params=params, files=files,verify=False)


 # ----------------------------  checkConditions Method  ----------------------------

# Function to check if all the conditions are satisfied

# Call like below:
#   cond_filepath = os.getcwd()+"/conditions.json"
#   conditions = read_file(cond_filepath,"conditions")

# Sample conditions will be : 
#   {"conditions":[{"name":"PRIORITY","value":"High","connector":"and"},{"name":"SLA","value":"High SLA","connector":"and"},{"name":"DESCRIPTION","value":"how","connector":"and"},{"name":"Approval_Code","value":"ADVC","connector":""}]}

# Invoke condition check will be 
#   result = checkConditions(requestObj, conditions)


def checkConditions(requestObj, conditions):
    # Define bitwise operators
    operators = {
        "and": operator.and_,
        "or": operator.or_
    }

    # Initialize Values
    connector = "and"
    finalResult = True

    # loop through each condition and update finalResult
    for cond in conditions:
        name = cond["name"]
        value = cond["value"]
        conn = cond["connector"]
        if isinstance(requestObj[name], list):
            requestObj[name] = map(str.lower,requestObj[name])
        else:
            requestObj[name] = requestObj[name].lower()
        result = (value.lower() in requestObj[name])
        finalResult = operators.get(connector)(finalResult,result)
        connector = conn
    # return final Result
    return finalResult

# ----------------------------  Handling Responses ----------------------------

# support below url calls and return true/false boolean value.
# sdpapi
# api/v3
# api/cmdb 

def getResponseStatus(url,response):
    responseobj=response.json()
    returnString = ""
    if "api/v3" in url:
        if(isinstance(responseobj['response_status'],dict)):
            status=responseobj['response_status']['status']
        else:
            status=responseobj['response_status'][0]['status']
            
        if status == 'success':
            returnString = 'Success'
        else :
            returnString = 'Failed'
        if 'messages' in responseobj['response_status'] and isinstance(responseobj['response_status']['messages'],list) and 'message' in responseobj['response_status']['messages'][0]:
            return returnString + " : " + responseobj['response_status']['messages'][0]['message']
        elif 'messages' in responseobj['response_status'] and isinstance(responseobj['response_status']['messages'],list) and 'fields' in responseobj['response_status']['messages'][0] and isinstance(responseobj['response_status']['messages'][0]['fields'],list):
            return returnString + " : Following fields not present - " + ','.join(responseobj['response_status']['messages'][0]['fields'])
        else:
            return returnString

    elif "sdpapi" in url:
        status=responseobj['operation']['result']['status']
        if status == 'Success':
            returnString = 'Success : '
        else :
            returnString = 'Failed : '
        return returnString + responseobj['operation']['result']['message']

    elif "api/cmdb" in url:
        status=responseobj['API']['response']['operation']['result']['status']
        if status == 'Success':
            returnString = 'Success : '
        else :
            returnString = 'Failed : '
        return returnString + responseobj['API']['response']['operation']['result']['message']
    else :
        return responseobj


#-------------Method for reading Configurations from xml file ----------------

# Root tag named 'configuration is ignored'

def readXMLFile(xmlFile):
    tree=ET.parse(xmlFile)
    root=tree.getroot()    
    configuration={}
    for element in root.iter():
        if element.tag=='configuration':
            continue
        configuration[element.tag]=element.text           
    return configuration

#------------ Constructing the Json Object for updating the request.---------

# data is Json that will have the Field Name/Value that needs to be updated in the request
# data = {"LEVEL":"TIER 1","PRIORITY":"High","IMPACT":"High"}
# Need to pass the data to actionPlugin_constructReqJSON to vet the full json

def actionPlugin_UpdateRequest(data,OperationName="EDIT_REQUEST",module=None,additionalParams=""):
    if module is not None:
        temp={}
        temp[module]=json.loads(data)
        tempString=json.dumps(temp)
        data=tempString
    json_data = '''{
        "INPUT_DATA": [''' + data + '''],
        "OPERATIONNAME": "''' + OperationName + '''",
        ''' + additionalParams + '''
    },'''
    return json_data

#------------ Constructing the Json Object Add Notes.---------

# notestext in string and ispublic is optional 
# Need to pass the data to actionPlugin_constructReqJSON to vet the full json

def actionPlugin_AddNote(notestext, ispublic="false"):
    json_data = '''{
        "INPUT_DATA": [{
            "notes": {
                "notestext": ''' + notestext + ''',
                "ispublic": ''' + str(ispublic) + '''
            }
        }],
        "OPERATIONNAME": "ADD_NOTE"
    },'''
    return json_data

#------------ Constructing the Json for default return functionality.---------

# Use a combination of above two functions to construct the data parameter
# data = actionPlugin_AddNote("Ticket has been created in JIRA and information populated in SDP")

def actionPlugin_constructReqJSON(data, message="Request Updated Successfully"):
    json_data = '''{
            "message":"''' +message + '''",
            "result":"success",
            "operation":[''' + data + ''']
            }'''  
    return json_data


# -----------Function to replace placeholders from a mapping Dictionary------

# output_data is a string
# mappingDict is a dictionary. the key is replaced by value in output_data
# use convertdates option to convert date fields to string

def handlePlaceHolder (mappingDict, output_data, convertdates=False):
    dateFieldList=['CREATEDTIME', 'DUEBYTIME', 'RESPONDEDTIME', 'RESOLVEDTIME', 'COMPLETEDTIME']
    for elem in mappingDict.keys():
        if(str('$'+elem+'$') in output_data):
            if (convertdates == True) and (elem in dateFieldList):
                output_data=output_data.replace('$'+elem+'$',str(getdatestring(mappingDict[elem])))
            else:
                output_data=output_data.replace('$'+elem+'$',str(mappingDict[elem]))
        if 'resource' in mappingDict:
            for resources in mappingDict['resource'].keys():
                for questions in mappingDict['resource' ][resources].keys():
                    if str('$' + questions + '$') in output_data:
                        if mappingDict['resource'][resources][questions]:
                            answer = mappingDict['resource'][resources][questions]
                        else:                                       
                            answer = 'No Answer'
                        output_data = output_data.replace('$'+ questions + '$', str(answer))
    return output_data

# --------- Function to Get all Tasks ------------------------

# serverURL = 'http://localhost'
# reqID = 40
# technicialKey = 'KEY'

def getAllTasks(serverURL, reqID, technicianKey):
    r = getURL(serverURL + '/api/v3/request/' + reqID
            + '/tasks', technicianKey)
    taskData = r.json()
    if r.status_code == 200:
        
        if taskData['list_info']['has_more_rows'] is True:
            row_count = taskData['list_info']['total_count']
            jsonData = {
                "list_info": {
                    "row_count": row_count,
                    "fields_required": "[get_all]"
                }
            }
            
            lr = getURL(serverURL + '/api/v3/request/' + reqID + '/tasks', technicianKey, params_data=jsonData)

            if lr.status_code == 200:
                taskData = lr.json()
    return taskData


#-------------- Function to get Requester Email ---------------------

def getRequesterMail(workorderid,appurl,apiKey):

    url = appurl
    TechnicianKey= apiKey
    OperationName='GET_REQUEST_FIELDS'
    REQUESTEREMAIL='swetha.parthiban@zohocorp.com'

    #Preparing the API call and submitting it.
    apprUrl = url + "/sdpapi/request/" + str(workorderid)
    r = postURL(apprUrl,TechnicianKey,OperationName)

    if(r.status_code == 200):

        responseObj=r.json()
        status=responseObj['operation']['result']['status']
        message=responseObj['operation']['result']['message']

        if status == 'Success':
            for elem in responseObj['operation']['details']:
                if(str(elem['NAME']) == 'REQUESTEREMAIL'):
                    REQUESTEREMAIL=elem['VALUE']
        else:
            print(status)
            print(message)
    else:
        print('Error: ' + str(r.status_code))
                
    return REQUESTEREMAIL


def getRequesterMail_new(workorderid,appurl,apiKey):

    url = appurl
    TechnicianKey= apiKey
    REQUESTEREMAIL='dummy@dummy.com'

    #Preparing the API call and submitting it.
    apprUrl = url + "/api/v3/requests/" + str(workorderid)
    r = getURL(apprUrl,TechnicianKey)

    responseObj=r.json()
    try:
        REQUESTEREMAIL = responseObj['request']['requester']['email_id']
    except:
        print(r.text)
              
    return REQUESTEREMAIL


#-------------- Function to get Requester ID ---------------------

def getRequesterID(workorderid,appurl,apiKey):

    url = appurl
    TechnicianKey= apiKey
    OperationName='GET_REQUEST_FIELDS'
    REQUESTERID='0'

    #Preparing the API call and submitting it.
    apprUrl = url + "/sdpapi/request/" + str(workorderid)
    r = postURL(apprUrl,TechnicianKey,OperationName)

    if(r.status_code == 200):

        responseObj=r.json()
        status=responseObj['operation']['result']['status']
        message=responseObj['operation']['result']['message']

        if status == 'Success':
            for elem in responseObj['operation']['details']:
                if(str(elem['NAME']) == 'RequesterID'):
                    REQUESTERID=elem['VALUE']
        else:
            print(status)
            print(message)
    else:
        print('Error: ' + str(r.status_code))
                
    return REQUESTERID


#----------- Function to change status using action plugin ---------------
# Will close the status if status s not passed

def updateReqStatus_JSON(status):
    inputJson={}
    inputJson["STATUS"]=status
    updatejson = actionPlugin_UpdateRequest(json.dumps(inputJson))
    returnJson = actionPlugin_constructReqJSON(updatejson,"Successfully Updated Request")
    print(returnJson)
    return returnJson

#----------- Function to change status using API call -------------------
# Will close the status if status s not passed

def updateReqStatus_API(url,technicianKey,status):
    json_data = constructJson_Request('{"status":"'+status+'"}')
    r = postURL(url, technicianKey,"EDIT_REQUEST", json_data)
    return getResponseStatus(url,r)


#----------- Function to get diff json old value -------------------
# Will return old value present in the diff json

def getValueFromDiffJSON(diffJSON,key):
    
    if key in diffJSON:
        return diffJSON[key]['OLD']
    else:
        print("No value present in Diff JSON")


#----------- Function to get diff json old value FOR V3 Format -------------------
# Will return old value present in the diff json

def getValueFromDiffJSON_V3(diffJSON,key):

    try:
        if key in diffJSON['old']:
            if(isinstance(diffJSON['old'][key],dict)):
                return diffJSON['old'][key]['name']
            else:
                return diffJSON['old'][key]
        else:
                return("No value present in Diff JSON")
    except:
        print("Unexpected error in parsing diff json"+diffJSON)


#----------- Function to get diff json old value FOR V3 Format -------------------
# Will return old value present in the diff json

def getIDFromDiffJSON_V3(diffJSON,key):
    
    try:
        if key in diffJSON['old']:
            if(isinstance(diffJSON['old'][key],tuple)):
                return diffJSON['old'][key]['id']
            else:
                return diffJSON['old'][key]
        else:
                print("No value present in Diff JSON")
    except:
        print("Unexpected error in parsing diff json"+diffJSON)

#----------- Function to write the data as a string -------------------
# can pass String/Json as as params

def write_file(output_file_path,dataJSON):

    f = open(output_file_path,'w')
    if(isinstance(dataJSON,dict)):
        f.write(json.dumps(dataJSON))
    else:
        f.write(dataJSON)    
    f.close()

#----------- Function to change the list data as a dictionary with given key-------------------
# can pass List adn key as params..like getListAsDictionary(["high","low"],"name")
# Will return [{"name":"High"},{"name":"low"}]


def getListAsDictionary(list,key):
    tempObj=[]
    for listObj in list:
            temp={}
            temp[key]=listObj
            tempObj.append(temp)
    return tempObj

def setBR_input(operation_name,reason=None,input_data=None,message="Sample Script"):
    resultjson={}
    requestjson={}
    resultjson["operation"] = []
    resultjson["result"]="success"
    operationJson={"INPUT_DATA":[]}
    operationJson["OPERATIONNAME"]=operation_name
    operationJson['REASON']=reason
    operationJson["FORMAT"]="V3"
    if(input_data is not None):
        requestjson=json.loads(input_data)
    resultjson["message"]=message
    operationJson['INPUT_DATA'].append(requestjson)
    resultjson['operation'].append(operationJson)
    return resultjson

def cleanhtml(raw_html):
    cleanr = re.compile('<.*?>')
    cleantext = re.sub(cleanr, '', raw_html)
    finaltext = cleantext.replace("\\n", "")
    finaltext = finaltext.replace("\\,", ",")
    finaltext = finaltext.strip()
    return finaltext

#--------------- Linking Request code starts -------------------##

def createLinkingChildReq(url,workorderid,linked_req):
    url_post = url + "/api/v3/requests/" + workorderid + "/link_requests"
    input_data = linkRequest(linked_req,"Child Requests linked successfully")
    r = postURL_V3(url_post, TechnicianKey,None,input_data)

def linkRequest(workorderlist,comments):
    link_array = {"link_requests":[]}
    for workorder in workorderlist:
        temp={}
        workorderid={}
        workorderid['id'] = workorder
        temp['linked_request'] = workorderid
        temp['comments']=comments
        link_array['link_requests'].append(temp)
    return json.dumps(link_array)


#--------------- Linking Request code Ends -------------------##

def update_request(input_data,operation_name,update_format="V3",message="Sample Python script for update request"):
    resultjson={}
    resultjson["operation"] = []
    resultjson["result"]="success"
    
    resultjson["message"]=message
    operationJson={"INPUT_DATA":[]}
    operationJson["OPERATIONNAME"]=operation_name
    operationJson["FORMAT"]=update_format

    updateReq={}
    updateReq['request']=input_data
    operationJson['INPUT_DATA'].append(updateReq)
    resultjson['operation'].append(operationJson)
    return resultjson

def setAssetState(CIType,CIName,assetState):
    input='''<?xml version="1.0" encoding="UTF-8"?>
<API version="1.0">
   <citype>
      <name>'''+CIType+'''</name>
      <criterias>
         <criteria>
            <parameter>
               <name compOperator="IS">CI Name</name>
               <value>'''+CIName+'''</value>
            </parameter>
         </criteria>
      </criterias>
      <newvalue>
         <record>
            <parameter>
               <name>Asset State</name>
               <value>'''+assetState+'''</value>
            </parameter>
         </record>
      </newvalue>
   </citype>
</API>'''
    return input

def setAssetOwnerShip(CIName,CIType,UserName):
    
    input='''<?xml version="1.0" encoding="UTF-8"?>
<API version="1.0">
   <citype>
      <name>'''+CIType+'''</name>
      <criterias>
         <criteria>
            <parameter>
               <name compOperator="IS">CI Name</name>
               <value>'''+CIName+'''</value>
            </parameter>
         </criteria>
      </criterias>
      <newvalue>
         <record>
            <parameter>
               <name>Asset State</name>
               <value>In use</value>
            </parameter>
            <multi-valued-parameter name="Assign Ownership">
               <record>
                  <parameter>
                     <name>User</name>
                     <value>'''+UserName+'''</value>
                  </parameter>
               </record>
            </multi-valued-parameter>
         </record>
      </newvalue>
   </citype>
</API>'''
    # print(input)
    return input


def getFieldValues(response):
    responseObj = response.json()
    status_code = responseObj['API']['response']['operation']['result']['statuscode']
    status = responseObj['API']['response']['operation']['result']['status']

    if(status_code=='200' and status=="Success"):
        return responseObj['API']['response']['operation']['Details']['field-values']


def getassetdetails(url,fields_required,name):
    input_data='{"list_info": {"fields_required": '+fields_required+',"search_criteria": {"field": "name","condition": "=","values": ["' +name+ '"]}}}'
    tempurl=url+'/api/v3/assets?input_data='+input_data
    # print(tempurl)
    response = getURL(tempurl,configuration['technicianKey'])
    responseObj = response.json()
    return responseObj
    #print(response.text)


#-------------- Function to Create zip file under custom scripts folder-----------------
#
# Example ::  createZipFile(os.getcwd()+'\\resources',os.getcwd()+'\\resources1.zip')
#

def get_all_file_paths(directory):  
    file_paths = [] 
    for root, directories, files in os.walk(directory): 
        for filename in files: 
            # join the two strings in order to form the full filepath. 
            filepath = os.path.join(root, filename) 
            file_paths.append(filepath) 
            # print(file_paths)
    return file_paths         
  
def createZipFile(src, dst): 
    directory = src
    file_paths = get_all_file_paths(directory) 
    # for file_name in file_paths: 
        #print(file_name)   
    with ZipFile(dst,'w') as zip: 
        # writing each file one by one 
        for file in file_paths: 
            zip.write(file) 
    print('Files zipped successfully!')



def writelog(content):
    f = open("log.txt", "a")
    f.write("\n")
    f.write(content)
    f.close()



def getFieldValueFromRequestJson(requestObj,sdpField):

    value = None
    if sdpField in requestObj and requestObj[sdpField] is not None:
        # print(sdpField)
        if isinstance(requestObj[sdpField],str) or isinstance(requestObj[sdpField],unicode):
            value = str(requestObj[sdpField])
        elif isinstance(requestObj[sdpField],dict) and 'name' in requestObj[sdpField]:
            value = str(requestObj[sdpField]['name'])
        elif isinstance(requestObj[sdpField],dict) and 'content' in requestObj[sdpField]:
            value = str(requestObj[sdpField]['content'])
        elif isinstance(requestObj[sdpField],dict) and 'value' in requestObj[sdpField]:
            value = str(requestObj[sdpField]['value'])
    elif 'udf' in sdpField and sdpField in requestObj['udf_fields'] and requestObj['udf_fields'] is not None:
        if isinstance(requestObj['udf_fields'][sdpField],str) or isinstance(requestObj['udf_fields'][sdpField],unicode):
            value = str(requestObj['udf_fields'][sdpField])
        elif isinstance(requestObj['udf_fields'][sdpField],list):
            value = requestObj['udf_fields'][sdpField]
    elif 'closure_code' in sdpField and 'name' in requestObj['closure_code'][sdpField]:
        value = requestObj['closure_code'][sdpField]['name']
    
    return value

def readXLSXFile(fileName):
    wb = xlrd.open_workbook(fileName) 
    sheet = wb.sheet_by_index(0) 
    sheet.cell_value(0, 0)
    return sheet

def getSheetFromWorkBook(wb,sheetName=None):
    if sheetName is not None:
        sheet = wb.sheet_by_name(sheetName) 
    else:
        sheet = wb.sheet_by_index(0) 
    sheet.cell_value(0, 0)
    return sheet




def getportalid(configuration,name):

    portalid=""
    url = configuration['url'] + "/api/v3/portals"
    input_data="""{"list_info":{"search_fields":{"name":\""""+name+"""\"}}}"""

    url=url+"?input_data="+input_data
    r = getURL(url, configuration['technicianKey'])
    #print(r.json())

    if("Success" in getResponseStatus(url,r)):
        response=r.json()
        if(len(response['portals'])>0):
            portalid=response['portals'][0]['id']
        else:
            print("No Portal id is matched")

    return portalid 
    


def getAPIStatus(url,response):
    responseobj=response.json()
    status = ""
    if "api/v3" in url:
        status=responseobj['response_status']['status']   
    elif "sdpapi" in url:
        status=responseobj['operation']['result']['status']
    elif "api/cmdb" in url:
        status=responseobj['API']['response']['operation']['result']['status']
    return status