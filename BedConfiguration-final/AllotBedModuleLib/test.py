from cgitb import text
from ntpath import join
from typing import List
from flask import Flask, json, jsonify, request
import sys
import requests as req
from pylint.lint import Run

# try:
from io import StringIO
import os
import git
from werkzeug import datastructures

app = Flask(__name__)
x="response"


@app.route("/prnumber", methods=['POST'])
def prNumber():
    import tempfile
    stdout = sys.stdout
    sys.stdout = StringIO()
    temp_dir = tempfile.TemporaryDirectory(dir=os.getcwd())
    print(temp_dir)
    user_data = json.loads(request.data)
    from git import Repo

# Repo cloning
    repo = Repo.clone_from("https://github.com/" +
                            user_data['owner'] + "/" + user_data['repo'] + ".git", temp_dir.name) 
    prinfo = req.get('https://api.github.com/repos/'+
                            user_data['owner'] + "/" + user_data['repo'] + "/pulls/" + user_data["prnumber"])

# checkout to the branch of the PR where pylint needs to be run
    branchName = (prinfo.json())["head"]["ref"]
    repo.git.checkout(branchName)

# Fetches all the required file from PR
    response = req.get('https://api.github.com/repos/' + user_data['owner'] + '/' +
                       user_data['repo'] + '/pulls/' + user_data['prnumber'] + '/files')
    response_dict = json.loads(response.text)
    list_diff = []

    # fetching all py files in the PR
    for x in response_dict:
        data_dict = {}
        filename = x["filename"]
        data_dict["filename"] = filename
        patch = x["patch"].split("@")
        lisy = patch[2].split(",")
        data_dict["startLine"] = (abs(int((lisy[0]))))
        data_dict["endLine"] = data_dict["startLine"] + int((lisy[1].split("+"))[0])
        list_diff.append(data_dict)
    print(list_diff)
    
    # run pylint to all py files
    for x in response_dict:
        path = os.getcwd() + x["filename"]
        file_list = splitall(x["filename"])
        path = os.path.join(temp_dir.name)
        for item in file_list:
            # data_dict = {}
            path = os.path.join(path,item)
            # data_dict["path"] = path
            # data_dict["start-line"] = 
            print((os.path.split(path))[1])
            if((os.path.split(path))[1]).endswith(".py"):
                stdout = sys.stdout
                sys.stdout = StringIO()
                ARGS = [
                "-r", "n", "--msg-template='{path}:{line}:{column}:{msg}'", "--output-format=json"]
                print(type(Run([path]+ARGS, exit=False)))
                test = sys.stdout.getvalue()


#  will parse the response from pylint
    with open("resp_text.txt", "a+") as file:
        file.write(test)
    list = parse_response()


# get latest commit id for post comments
    response = req.get('https://api.github.com/repos/' + user_data['owner'] + '/' +
                user_data['repo'] + '/pulls/' + user_data['prnumber'] + '/commits')
    response = json.loads(response.content)
    commit_id = response[0]["sha"]

    resp = {}
    # fetch required data from pylint output for posting comments api
    for index in range(len(list)):
        for key in list[index]:
            if key == 'line':
                line = list[index][key] # check line number start and end position #'10'
                print("*****"+line)
                if((abs(int(line)) <= data_dict['startLine'])  or ((abs(int(line))) >= data_dict['endLine'])): 
                    a = True
                    break
                #  compare line number when the line number is in diff proceed or break
            if key == 'message':
                message = list[index][key]
            if key == 'path':
                path = (((list[index][key]).replace("\\\\", "/")))# comming as a list
                # if(path)
        if(a):
            a = False
            continue
        
        comments_data = {"commit_id": commit_id,
                    "body": message,
                    "path": path,
                    "position": 0,
                    "side": "LEFT",
                    "line": (abs(int(line)))
                    }
        resp = req.post('https://api.github.com/repos/' + user_data['owner'] + '/' +
                user_data['repo'] + '/pulls/' + user_data['prnumber'] + '/comments', headers={ 'Authorization': 'Bearer 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'},  
                json=comments_data)
    if(resp):
        return resp.text
    else:
        return resp 
 



def parse_response():
    list = []
    with open("resp_text.txt", 'r') as f:
        for line in f:
            line = line.replace('\n', '')
            line = line.replace(" ", "")
            line = line.replace('"', '')
            line = line.replace(',', '')
            if '{' in line:
                data_dict = {}
            if 'path' in line  or 'message' in line or 'line' in line:
                x = line.split(':')
                data_dict[x[0]] = x[1]
            if '}' in line:
                list.append(data_dict)
                data_dict = {}
        print(list)
        with open("resp_text2.txt", "a+") as file:
            string1 = ""
            file.write(str(list))
    return list


import os, sys
def splitall(path):
    allparts = []
    while 1:
        parts = os.path.split(path)
        if parts[0] == path:  # sentinel for absolute paths
            allparts.insert(0, parts[0])
            break
        elif parts[1] == path: # sentinel for relative paths
            allparts.insert(0, parts[1])
            break
        else:
            path = parts[0]
            allparts.insert(0, parts[1])
    return allparts


all =splitall("BedConfiguration-final/BedConfiguration/packages.config")
print(all)


if __name__ == "__main__":
    app.run()
