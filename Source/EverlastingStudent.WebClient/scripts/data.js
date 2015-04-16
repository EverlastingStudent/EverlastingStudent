var app = app || {};

app.data = (function () {
    function Data(baseUrl, ajaxRequester) {
        this.users = new Users(baseUrl, ajaxRequester);
        this.freelanceProjects = new freelanceProjects(baseUrl, ajaxRequester);
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

        function getUsername(username) {
            localStorage.getItem('username');
        }

        function setUsername(username) {
            localStorage.setItem('username', username);
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
            var data = { 'userName': username, 'Password': password, 'grant_type': 'password' }
            return this._ajaxRequester.post(url, data, this._headers)
				.then(function (data) {
				    cradentials.setSessionToken(data.access_token);
				    cradentials.setUsername(data.userName);
				    return data;
				});
        };

        Users.prototype.register = function (username, password, confirmPassword, playerType) {
            var user = {
                Email: username,
                Password: password,
                ConfirmPassword: confirmPassword,
                PlayerType: playerType
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

    var freelanceProjects = (function (argument) {
        function freelanceProjects(baseUrl, ajaxRequester) {
            this._serviceUrl = baseUrl;
            this._ajaxRequester = ajaxRequester;
            this._headers = cradentials.getHeaders();
        }

        freelanceProjects.prototype.allActive = function () {
            console.log(cradentials.getHeaders());
            var url = this._serviceUrl + "api/FreelanceProjects/allActive";
            return this._ajaxRequester.get(url, { 'Authorization': localStorage.getItem('sessionToken') })
				.then(function (data) {
				    console.log("Freelance projects successfully loaded");
				    return data;
				});
        };


        freelanceProjects.prototype.takeFreelanceProject = function (id) {
            var url = this._serviceUrl + "api/FreelanceProjects/take/" + id;
            return this._ajaxRequester.post(url, null, { 'Authorization': localStorage.getItem('sessionToken') })
				.then(function (data) {
				    console.log("Freelance project successfully taken.");
				    return data;
				});
        };

        freelanceProjects.prototype.getMyProjects = function () {
            var url = this._serviceUrl + "api/FreelanceProjects/myProjects/";
            return this._ajaxRequester.get(url, { 'Authorization': localStorage.getItem('sessionToken') })
				.then(function (data) {
				    console.log("My Freelance Projects successfully loaded.");
				    return data;
				});
        };

        freelanceProjects.prototype.workOnMineFreelanceProject = function (id) {
            var url = this._serviceUrl + "api/FreelanceProjects/work/" + id;
            return this._ajaxRequester.post(url, null, { 'Authorization': localStorage.getItem('sessionToken') })
				.then(function (data) {
				    console.log("Just work on project " + id);
				    return data;
				});
        };

        return freelanceProjects;
    }());

    var Drinks = (function (argument) {
            function Drinks(baseUrl, ajaxRequester) {
                this._serviceUrl = baseUrl;
                this._ajaxRequester = ajaxRequester;
                this._headers = cradentials.getHeaders();
            }

            Drinks.prototype.names = function () {
                var url = this._serviceUrl + "api/Drinks/GetNames";
                return this._ajaxRequester.get(url, { 'Authorization': localStorage.getItem('sessionToken') })
                    .then(function (data) {
                        console.log("Drinks successfully loaded");
                        return data;
                    });
            };


            Drinks.prototype.getDrink = function (id) {
                var url = this._serviceUrl + "api/Drinks/GetADrink/" + id;
                return this._ajaxRequester.post(url, null, { 'Authorization': localStorage.getItem('sessionToken') })
                    .then(function (data) {
                        console.log("Drink successfully taken.");
                        return data;
                    });
            };

            Drinks.prototype.buyDrink = function (id) {
                var url = this._serviceUrl + "api/Drinks/BuyADrink/" + id;
                return this._ajaxRequester.post(url, null, { 'Authorization': localStorage.getItem('sessionToken') })
                    .then(function (data) {
                        console.log("Drink successfully bought.");
                        return data;
                    });
            };

            return Drinks;
        }());

        var hardwareParts = (function (argument) {
                    function hardwareParts(baseUrl, ajaxRequester) {
                        this._serviceUrl = baseUrl;
                        this._ajaxRequester = ajaxRequester;
                        this._headers = cradentials.getHeaders();
                    }

                    hardwareParts.prototype.names = function () {
                        var url = this._serviceUrl + "api/Hardwares/GetNames";
                        return this._ajaxRequester.get(url, { 'Authorization': localStorage.getItem('sessionToken') })
                            .then(function (data) {
                                console.log("Hardwares successfully loaded");
                                return data;
                            });
                    };


                    hardwareParts.prototype.getDrink = function (id) {
                        var url = this._serviceUrl + "api/Hardwares/GetHardware/" + id;
                        return this._ajaxRequester.post(url, null, { 'Authorization': localStorage.getItem('sessionToken') })
                            .then(function (data) {
                                console.log("Hardwares successfully taken.");
                                return data;
                            });
                    };

                    hardwareParts.prototype.buyDrink = function (id) {
                        var url = this._serviceUrl + "api/Hardwares/BuyHardware/" + id;
                        return this._ajaxRequester.post(url, null, { 'Authorization': localStorage.getItem('sessionToken') })
                            .then(function (data) {
                                console.log("Hardwares successfully bought.");
                                return data;
                            });
                    };

                    return hardwareParts;
                }());

    return {
        get: function (baseUrl, ajaxRequester) {
            return new Data(baseUrl, ajaxRequester);
        }
    }
}());