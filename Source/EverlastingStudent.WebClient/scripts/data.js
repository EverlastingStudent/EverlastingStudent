var app = app || {};

app.data = (function () {
	function Data (baseUrl, ajaxRequester) {
		this.users = new Users(baseUrl, ajaxRequester);
	}

	var cradentials = (function () {
		var headers = {
			'Authorization': getSessionToken()
		}

		function getSessionToken() {
			localStorage.getItem('sessionToken');
		}

		function setSessionToken(sessionToken) {
			localStorage.setItem('sessionToken', 'Bearer ' + sessionToken);
		}

		function getUsername(sessionToken) {
			localStorage.getItem('username');		}

		function setUsername(sessionToken) {
			localStorage.setItem('username', sessionToken);
		}

		function getHeaders() {
			return headers;
		}

		return {
			getSessionToken: getSessionToken,
			setSessionToken: setSessionToken,
			getUsername: getUsername,
			setUsername: setUsername,
			getHeaders: getHeaders
		}
	}());

	var Users = (function (argument) {
		function Users(baseUrl, ajaxRequester) {
			this._serviceUrl = baseUrl;
			this._ajaxRequester = ajaxRequester;
			this._headers = cradentials.getHeaders();
		}

		Users.prototype.login = function (username, password) {
			var url = this._serviceUrl + 'token';
			var data = {'userName' : username, 'Password' : password, 'grant_type' : 'password'}
			return this._ajaxRequester.post(url, data, this._headers)
				.then(function (data) {
					cradentials.setSessionToken(data.access_token);
					cradentials.setUsername(data.userName);
					return data;
				});
		};

		Users.prototype.register = function (username, password, confirmPassword, playerType) {
			var user =  {
				Email: username,
				Password: password,
				ConfirmPassword : confirmPassword,
				PlayerType : playerType
			};
			var url = this._serviceUrl + 'api/Account/Register';
			return this._ajaxRequester.post(url, user, this._headers)
				.then(function (data) {
					cradentials.setSessionToken(data.sessionToken);
					return data;
				});
		};

		Users.prototype.validateToken = function (accessToken) {
			// body...
		};

		return Users;
	}());

	return {
		get: function (baseUrl, ajaxRequester) {
			return new Data(baseUrl, ajaxRequester);
		}
	}
}());