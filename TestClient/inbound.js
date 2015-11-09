

var net = require('net');

var server = net.createServer();
server.on("connection", function (socket) {
    socket.on("data", function (data) {
        buf = new Buffer(data, 'base64');
        msg = buf.toString('utf8');

        console.log(data);
        socket.write("Message has been received by the microServiceBus host");
    });
    socket.on("close", function () { 
        console.log('Closed');
    });
    socket.on("error", function (error) { 
        console.log('Error');
    });
});

server.listen(3001, function () {
    console.log('open');

});
