var edge = require('edge');
var syncrequest = require('sync-request');
var readline = require('readline');

var fs = require('fs');
var installAdapter; // Install method
var applyTransmitBindings; // Install method
var isInstalled; // Check if adapter is installed method
var processMessage; // Process incoming message to BizTalk method

var rootFolder = "../../../../../@GIT/microservicebus/microservicebus.host/";
var managementFile = rootFolder + 'microServiceBus.BizTalk/microServiceBus.BizTalkReceiveAdapter.Management.dll';
var runtimeFile = rootFolder + 'microServiceBus.BizTalk/microServiceBus.BizTalkReceiveeAdapter.RunTime.dll';
var helperFile = rootFolder + 'microServiceBus.BizTalk/microServiceBus.BizTalkReceiveeAdapter.Helper.dll';

var application = "microservicebus.com"
var port = "aPort";
var location = "aLocation";

installAdapter = edge.func({
    assemblyFile: helperFile,
    typeName: 'microServiceBus.BizTalkReceiveeAdapter.Helper.BizTalkServiceHelper',
    methodName: 'InstallAdapter'
});

applyReceiveBindings = edge.func({
    assemblyFile: helperFile,
    typeName: 'microServiceBus.BizTalkReceiveeAdapter.Helper.BizTalkServiceHelper',
    methodName: 'ApplyReceiveBindings'
});
applyTransmitBindings = edge.func({
    assemblyFile: helperFile,
    typeName: 'microServiceBus.BizTalkReceiveeAdapter.Helper.BizTalkServiceHelper',
    methodName: 'ApplyTransmitBindings'
});

processMessage = edge.func({
    assemblyFile: helperFile,
    typeName: 'microServiceBus.BizTalkReceiveeAdapter.Helper.BizTalkServiceHelper',
    methodName: 'ProcessMessage'
});

var rl = readline.createInterface({
    input: process.stdin,
    output: process.stdout
});

rl.question("GO? ", function (answer) {
    // Install adapter

    installAdapter(null, function (error, result) {
        if (error || result != null)
            throw 'An error occurd while installing the adapter.' + result;
        else
            console.log('Adapter has been successfully installed');              
    });
    
    // Create receive port
    var applyReceiveBindingsParams = {
        applicationName: application,
        receivePort: "Receive from microServiceBus",
        receiveLocation: "Receive from microServiceBus"
    };

    applyReceiveBindings(applyReceiveBindingsParams, function (error, result) {
        if (error || result != null)
            throw 'An error occurd while installing the adapter.' + result;
        else
            console.log('Adapter bindings has been successfully applied');
    });
    
    // Create send port
    var applyTransmitBindingsParams = {
        applicationName: application,
        sendPort: "Send to microservicebus",
        address: "localhost",
        port: 3001,
        contentType: "application/xml"
    };
    
    applyTransmitBindings(applyTransmitBindingsParams, function (error, result) {
        if (error || result != null)
            throw 'An error occurd while installing the adapter.' + result;
        else
            console.log('Adapter bindings has been successfully applied');
    });

    // Submit message
    var processParams = {
        message: '{"name":"Mikael"}',
        context: context,
        location: location
    };
    // Call Process message
    processMessage(processParams, function (error, result) {
        if (error || !result)
            throw 'An error occurd when transmitting the message to the adapter.' + error;
        me.Done(context, this.Name);
        me.Debug('Process: Sucessbully submitted to BizTalk ' + processParams.submitURI);
    });
});

