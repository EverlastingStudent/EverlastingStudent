var app = app || {};

app.data = (function () {
    function Data(baseUrl, ajaxRequester) {
        this.users = new Users(baseUrl, ajaxRequester);
        this.freelanceProjects = new freelanceProjects(baseUrl, ajaxRequester);

        this.Homework = new Homework(baseUrl, ajaxRequester)
    }

    var credentials = (function () {
        var headers = {
            'Authorization': getSessionToken()
        }

        function getSessionToken() {
            return localStorage.getItem('sessionToken');
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
            this._headers = credentials.getHeaders();
        }

        Users.prototype.login = function (username, password) {
            var url = this._serviceUrl + 'token';
            var data = { 'userName': username, 'Password': password, 'grant_type': 'password' }
            return this._ajaxRequester.post(url, data, this._headers)
				.then(function (data) {
				    credentials.setSessionToken(data.access_token);
				    credentials.setUsername(data.userName);
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
				    credentials.setSessionToken(data.sessionToken);
				    return data;
				});
        };

        Users.prototype.validateToken = function (accessToken) {
            // body...
        };

        return Users;
    }());

    var Homework = (function (argument) {
        function Homework(baseUrl, ajaxRequester) {
            this._serviceUrl = baseUrl + 'api/Homeworks';
            this._ajaxRequester = ajaxRequester;
            this._headers = credentials.getHeaders();
        }

        Homework.prototype.getHomework = function () {
            return this._ajaxRequester.get(this._serviceUrl + '/GetHomeworks', this._headers);
        };

        Homework.prototype.solve = function (homeworkId) {
            return this._ajaxRequester.post(this._serviceUrl + '/solve/' + homeworkId, homeworkId, this._headers);
        };

        return Homework;
    }());

    var freelanceProjects = (function (argument) {
        function freelanceProjects(baseUrl, ajaxRequester) {
            this._serviceUrl = baseUrl;
            this._ajaxRequester = ajaxRequester;
            this._headers = credentials.getHeaders();
        }

        freelanceProjects.prototype.allActive = function () {
            console.log(credentials.getHeaders());
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
                this._headers = credentials.getHeaders();
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
                        this._headers = credentials.getHeaders();
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