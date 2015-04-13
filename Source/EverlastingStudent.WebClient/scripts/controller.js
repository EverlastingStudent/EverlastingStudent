var app = app || {};

app.controller = (function () {
	function BaseController(data) {
		this._data = data;
	}

	BaseController.prototype.loadHome = function (selector) {
		$(selector).load('./templates/home.html');
	};

	BaseController.prototype.loadLogin = function (selector) {
		$(selector).load('./templates/login.html');
	};

	BaseController.prototype.loadRegister = function (selector) {
		$(selector).load('./templates/register.html');
	};

	BaseController.prototype.attachEventHandlers = function () {
		var selector = '#wrapper';
		attachLoginHandler.call(this, selector);
		attachRegisterHandler.call(this, selector);
	};

	var attachLoginHandler = function (selector) {
		var _this = this;
		$(selector).on('click', '#login', function () {
			var username = $('#username').val();
			var password = $('#password').val();
			_this._data.users.login(username, password)
				.then(function (data) {
					// redirect
				},
				function (error) {
					// body...
				});
		});
	};

	var attachRegisterHandler = function (selector) {
		var _this = this;
		$(selector).on('click', '#register', function () {
			var username = $('#username').val();
			var password = $('#password').val();
			var confirmPassword = $('#confirm-password').val();
			var playerType = $('input:radio[name=type]:checked').val();
			_this._data.users.register(username, password, confirmPassword, playerType)
				.then(function (data) {
					alert(data.username);
				},
				function (error) {
					// body...
				});
		});
	};

	return {
		get: function (data) {
			return new BaseController(data);
		}
	}
}());