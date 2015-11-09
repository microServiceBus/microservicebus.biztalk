var edge = require('edge');
var syncrequest = require('sync-request');
var fs = require('fs');
var AdmZip = require('adm-zip');
var readline = require('readline');

var zipUri = "https://blogical.blob.core.windows.net/microservicebus/biztalk/microServiceBus.BizTalk.zip";
var fileName = "_microServiceBus.BizTalk.zip";

var httpResponse = syncrequest('GET', zipUri);
if (httpResponse.statusCode != 200)
    throw 'Unable to download file: ' + fileName;

fs.writeFileSync(fileName, httpResponse.body);

var zip = new AdmZip(fileName);
zip.extractAllTo("biztalk", true);

var install = edge.func({
    assemblyFile: 'microServiceBus.BizTalkReceiveeAdapter.Helper.dll',
    typeName: 'microServiceBus.BizTalkReceiveeAdapter.Helper.BizTalkServiceHelper',
    methodName: 'Install' // This must be Func<object,Task<object>>
});

var unInstall = edge.func({
    assemblyFile: 'microServiceBus.BizTalkReceiveeAdapter.Helper.dll',
    typeName: 'microServiceBus.BizTalkReceiveeAdapter.Helper.BizTalkServiceHelper',
    methodName: 'UnInstall' // This must be Func<object,Task<object>>
});

var installParams = {
    applicationName: 'microservicebus.com',
    receivePort: 'myReceivePort',
    receiveLocation: 'myReceiveLocation',
    submitURI: 'microservicebus://onramp'
};

var unInstallParams = {
    applicationName: 'microservicebus.com'
};

var rl = readline.createInterface({
    input: process.stdin,
    output: process.stdout
});

rl.question("Install or Uninstall ", function (answer) {
    if (answer == 'i') {
        install(installParams, function (error, result) {
            if (error) throw error;
            console.log(result);
        });
    }
    else if (answer == 'u') {
        unInstall(unInstallParams, function (error, result) {
            if (error) throw error;
            console.log(result);
        });
    }
    else { 
        console.log('Unsuported parameter');
    }
});

