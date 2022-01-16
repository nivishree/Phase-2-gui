import requests as req
import json

def test()
    user_data = {"commit_id": "4f0b4591bac65a9c773cea6f8b12e3027a866c59",
"body": "Test comment on november ",
"path":"BedConfiguration-final/AllotBedModuleLib/packages.config",
"position": 0,
"side": "LEFT",
"line": 18;
};
            
    # resp_data = json.loads(user_data)
    
            
    response = req.post('https://api.github.com/repos/nivishree/Phase-2-gui/pulls/4/comments', headers={ 'Authorization': 'Bearer  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'},  json=user_data)

