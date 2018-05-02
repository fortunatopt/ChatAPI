# ChatAPI
.NET Chat API with an very simple AngularJS frontend

Create a new Empty .NET Web Application with Web API selected.

Install SignalR: Install-Package Microsoft.AspNet.SignalR

Create Startup.cs:
	Add new item - OWIN Startup Class
  
		namespace SampleChat
		{
			public class Startup
			{
				public void Configuration(IAppBuilder app)
				{
					app.MapSignalR();
				}
			}
		}

Install AngularJS Core: Install-Package AngularJS.Core

Add SignalR Hub ChatHub.cs:
	Add new item - SignalR Hub Class
  
		public class ChatHub : Hub
		{
			public void SendMessage(string name, string message)
			{
				Guid id = Guid.NewGuid();
				Clients.All.broadcastMessage(name, message);
			}
		}

Create an AngularJS App - Scripts/ChatApp.js:

	(function () {
		var app = angular.module('chat-app', []);

		app.controller('ChatController', function ($scope) {
			// scope variables
			$scope.name = 'Guest'; // holds the user's name
			$scope.message = ''; // holds the new message
			$scope.messages = []; // collection of messages coming from server
			$scope.chatHub = null; // holds the reference to hub

			$scope.chatHub = $.connection.chatHub; // initializes hub
			$.connection.hub.start(); // starts hub

			// register a client method on hub to be invoked by the server
			$scope.chatHub.client.broadcastMessage = function (id, name, message) {
				var newMessage = name + ' says: ' + message;

				// push the newly coming message to the collection of messages
				$scope.messages.push({ "id": id, "name": name, "message": message });
				$scope.$apply();
			};

			$scope.newMessage = function () {
				// sends a new message to the server
				$scope.chatHub.server.sendMessage($scope.name, $scope.message);

				$scope.message = '';
			};
		});
	}());


Create an index.html:

	<body ng-app="chat-app">

		<div ng-controller="ChatController">
			<div>
				<form name="chatForm">
					Name: <input type="text" ng-model="name" /><br />
					Message: <input type="text" id="message" ng-model="message" ng-keypress="($event.charCode==13)? chatForm.$valid && newMessage() : return" ng-required="true" /><br />
					<input type="button" value="Send" ng-click="chatForm.$valid && newMessage()" ng-required="message" />
				</form>
			</div>
			<div>
				<ul>
					<li ng-repeat="chat in messages">
						<span ng-bind="chat.name"></span> says: <span ng-bind="chat.message"></span>
					</li>
				</ul>
			</div>
		</div>

		<script src="Scripts/jquery-1.6.4.js"></script>
		<script src="Scripts/jquery.signalR-2.2.3.min.js"></script>
		<script src="signalr/hubs"></script>
		<script src="Scripts/angular.js"></script>
		<script src="Scripts/ChatApp.js"></script>
	</body>
