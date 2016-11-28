var app = require('express')();
var http = require('http').Server(app);
var io = require('socket.io')(http);
var time = 3000;
var time2 = 1000;
var arDrone = require('ar-drone');
var client = arDrone.createClient();
var pngStream = client.getPngStream();
var lastPng;


app.get('/', function (req, res) {
    res.send('Hello World!')
})


http.listen(3000, function(){
  console.log('listening on *:3000');
});

io.on('connection', function(socket){
  console.log('a user connected');
});

io.on('connection', function(socket){
  console.log('a user connected');
  socket.on('disconnect', function(){
    console.log('user disconnected');
  });
});

io.sockets.on('connection', function (socket) {

    pngStream
  .on('error', console.log)
  .on('data', function (pngBuffer) {
      lastPng = pngBuffer;
      io.sockets.emit('sendImg', lastPng);
  });


  socket.on('test', function (data) {
        console.log("test succesvol");
  });
  socket.on('fly', function (data) {
     
      console.log(data);
        client.takeoff();

     // client
     //   .after(time, function () {
       //     this.clockwise(0.9);
       //     console.log(time);
      //  })

      client
        .after(data, function () {
            this.stop();
            this.land();
            console.log(time2);
        });


  });
});