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

    BaseController.prototype.loadFreelanceProjects = function (selector) {
        var _this = this;
        _this._data.freelanceProjects.allActive()
                .then(function (projects) {
                    var allProjectsContainer = $('<ul>');
                    for (var projectKey in projects) {
                        var projectData = {
                            CloseForTakenDatetime: projects[projectKey].CloseForTakenDatetime,
                            Content: projects[projectKey].Content,
                            EnergyCost: projects[projectKey].EnergyCost,
                            ExperienceGain: projects[projectKey].ExperienceGain,
                            Id: projects[projectKey].Id,
                            IsActive: projects[projectKey].IsActive,
                            MoneyGain: projects[projectKey].MoneyGain,
                            OpenForTakenDatetime: projects[projectKey].OpenForTakenDatetime,
                            RequireExperience: projects[projectKey].RequireExperience,
                            SolveDurabationInHours: projects[projectKey].SolveDurabationInHours,
                            Title: projects[projectKey].Title,
                        };


                        //var temlateElem = $(selector).load('./templates/forTake.html');
                        //console.log(temlateElem.toString());
                        //var html = Mustache.to_html(temlateElem.toString(), projectData);

                        //$.get('./templates/freelanceProjects/forTake.html', function (template, textStatus, jqXhr) {
                        //    $(selector).append(Mustache.render($(template).html(), projectData));
                        //});

                        var template = "<li><p hidden id='projectId'>{{Id}}</p><h3>{{Title}}</h3><section><p>Info:{{Content}}</p><p>Requiredments:</br><span>Tatal energy cost: {{EnergyCost}}</span></br><span>Experience: {{RequireExperience}}</span></br><span>Solve Durabation: {{SolveDurabationInHours}} hours</span></br><span>Active from: {{OpenForTakenDatetime}}</span></br><span>Closed fot taken at: {{CloseForTakenDatetime}}</span></p><p>Obtainments:</br><span>Experience Gain: {{ExperienceGain}}</span></br><span>Money Gain: {{MoneyGain}}</span></p></section></li>";
                        var projectElement = $(Mustache.to_html(template, projectData));

                        // console.log($(projectElement));
                        // takeButton
                        $('<input>')
                            .attr('class', 'takeButton')
                            .attr('type', 'button')
                            .attr('value', 'Take Project')
                            .click('click', function () {
                                BaseController.prototype.takeFreelanceProject.call(_this, this.parentElement);
                            })
                            .appendTo(projectElement.last());

                        allProjectsContainer.append(projectElement);
                    }

                    $(selector).append(allProjectsContainer);
                },
                function (error) {
                    console.log(error);
                    // body...
                });
    };

    BaseController.prototype.takeFreelanceProject = function (projectElement) {
        var _this = this;

        //console.log(_this.parentElement.firstChild.innerText);
        var projectId = parseInt(projectElement.firstChild.innerHTML);

        _this._data.freelanceProjects.takeFreelanceProject(projectId)
                   .then(function (project) {
                       projectElement.remove();
                       console.log(project);
                   },
                   function (error) {
                       console.log(error);
                       // body...
                   });
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
				    console.log("Logged in successfully");
				    // redirect
				    app.router.setLocation('#/Freelance-projects');
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