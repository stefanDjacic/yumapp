//signalR connection
var connection = new signalR.HubConnectionBuilder()
                            .configureLogging(signalR.LogLevel.Debug)
                            .withUrl('/User')
                            .withAutomaticReconnect()
                            .build();

connection.start().then(function () {
    console.log("SignalR connected!")
}).catch (function (err) {
    console.log(err.toString())
});
//signalR conneciton