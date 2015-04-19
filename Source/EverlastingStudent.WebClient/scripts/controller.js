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

	BaseController.prototype.loadTimer = function (selector, duration) {
		debugger;
		$(selector).load('./templates/timer.html');
		startTimer(duration, $('#time'));
	};

    BaseController.prototype.loadRegister = function (selector) {
        $(selector).load('./templates/register.html');
    };

    BaseController.prototype.loadHomework = function (selector) {
        this._data.Homework.getHomework()
            .then(function (data) {
                $.get('templates/homework.html', function (template) {
                    var output = Mustache.render(template, data);
                    debugger;
                    $(selector).html(output);
                })
            })
    };

    BaseController.prototype.loadFreelanceProjects = function (selector) {
        //this._data.Homework.getHomework()
        //    .then(function (data) {
        var _this = this;
        $.get('templates/freelanceProejects.html', function (template) {
            var output = Mustache.render(template, null);

            //debugger;
            $(selector).html(output);
            var availableProjects = $('#availableFreelanceProjectsCointainer');
           
            // var availableProjects = $('<div id="availableFreelanceProjectsCointainer" class="panel panel-default"><h3>Freelance Projects Market</h3></div>').appendTo($(selector));
            var allProjectsContainer = $('<ul id="allAvailableProjectUl">');
            availableProjects.append(allProjectsContainer);
            $('<input>')
                .attr('id', 'checkProjectsButtonId')
                .attr('type', 'button')
                .attr('value', 'Check for Projects')
                .click('click', function () {
                    _this._data.freelanceProjects.allActive()
                     .then(function (projects) {
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

                             var template = "<li><p hidden id='projectId'>{{Id}}</p><h3>{{Title}}</h3><section><p>Info:{{Content}}</p><p><h5>Requiredments:</h5><span>Tatal energy cost: {{EnergyCost}}</span><br /><span>Experience: {{RequireExperience}}</span><br /><span>Solve Durabation: {{SolveDurabationInHours}} hours</span><br /><span>Active from: {{OpenForTakenDatetime}}</span><br /><span>Closed fot taken at: {{CloseForTakenDatetime}}</span><br /></p><p><h5>Obtainments:</h5><span>Experience Gain: {{ExperienceGain}}</span><br /><span>Money Gain: {{MoneyGain}}</span></p></section></li>";
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

                         $('#checkProjectsButtonId').remove();
                         //$(selector).append(allProjectsContainer);
                     },
                     function (error) {
                         console.log(error);
                         alert(error.responseJSON['Message']);
                         // body...
                     });
                }).appendTo(availableProjects);


            var myProjectsDiv = $('#myProjectsDiv');

            // var myProjectsDiv = $('<div id="myFreelanceProjectsCointainer" class="panel panel-default"><h3>My Projects</h3></div>').appendTo($(selector));
            var myProjectsContainer = $('<ul id="myProjectUl">');
            myProjectsDiv.append(myProjectsContainer);
            _this._data.freelanceProjects.getMyProjects()
                     .then(function (myProjects) {
                         for (var projectKey in myProjects) {
                             var myProjectData = {
                                 CloseForTakenDatetime: myProjects[projectKey].CloseForTakenDatetime,
                                 Content: myProjects[projectKey].Content,
                                 EnergyCost: myProjects[projectKey].EnergyCost,
                                 ExperienceGain: myProjects[projectKey].ExperienceGain,
                                 Id: myProjects[projectKey].Id,
                                 IsActive: myProjects[projectKey].IsActive,
                                 MoneyGain: myProjects[projectKey].MoneyGain,
                                 OpenForTakenDatetime: myProjects[projectKey].OpenForTakenDatetime,
                                 RequireExperience: myProjects[projectKey].RequireExperience,
                                 SolveDurabationInHours: myProjects[projectKey].SolveDurabationInHours,
                                 Title: myProjects[projectKey].Title,
                                 FreelanceProjectId: myProjects[projectKey].FreelanceProjectId,
                                 BaseFreelanceProjectId: myProjects[projectKey].BaseFreelanceProjectId,
                                 StudentId: myProjects[projectKey].StudentId,
                                 IsSolved: myProjects[projectKey].IsSolved,
                                 StartDateTime: myProjects[projectKey].StartDateTime,
                                 LastWorkingDateTime: myProjects[projectKey].LastWorkingDateTime,
                                 ProgressInPercentage: myProjects[projectKey].ProgressInPercentage,
                                 WorkPercentage: myProjects[projectKey].WorkPercentage,
                             };

                             visualizeMyProject(_this, myProjectData, myProjectsContainer);
                         }
                     },
                     function (error) {
                         console.log(error.responseJSON.message);
                         alert(error.responseJSON['Message']);
                         // body...
                     });
            //});
        });

    };

    var visualizeMyProject = function (currentScope, myProjectData, myProjectsContainer) {
        var templateMine = "<li><p hidden id='projectId'>{{Id}}</p><h3>{{Title}}<div class=\"progress\"><div class=\"progress-bar progress-bar-success progress-bar-striped\" role=\"progressbar\" aria-valuenow=\"{{ProgressInPercentage}}\" aria-valuemin=\"0\" aria-valuemax=\"100\" style=\"width: {{ProgressInPercentage}}%\"><span class=\"sr-only\">{{ProgressInPercentage}}% Complete (success)</span></div></div></h3><section><p>Info:{{Content}}</p><p><h4>Current progress:</h4><span>Finished: {{IsSolved}}</span><br /><span>Started at: {{StartDateTime}}</span><br /><span>Last worked at: {{LastWorkingDateTime}}</span><br /><span>Work progress: {{WorkPercentage}}</span><br /><span>Total progress: {{ProgressInPercentage}}</span><br /></p><p><h5>Requiredments:</h5><span>Tatal energy cost: {{EnergyCost}}</span><br /><span>Experience: {{RequireExperience}}</span><br /><span>Solve Durabation: {{SolveDurabationInHours}} hours</span><br /><span>Active from: {{OpenForTakenDatetime}}</span><br /><span>Closed fot taken at: {{CloseForTakenDatetime}}</span><br /></p><p><h5>Obtainments:</h5> <br /><span>Experience Gain: {{ExperienceGain}}</span><br /><span>Money Gain: {{MoneyGain}}</span></p></section></li>";
        var myProjectElement = $(Mustache.to_html(templateMine, myProjectData));

        // console.log($(projectElement));
        if (!myProjectData.IsSolved) {

            // workButton
            $('<input>')
                .attr('class', 'workButton')
                .attr('type', 'button')
                .attr('value', 'Work on Project')
                .click('click', function () {
                    BaseController.prototype.workOnFreelanceProject.call(currentScope, this.parentElement);//this.parentElement
                }).appendTo(myProjectElement.first());
        }

        $(myProjectsContainer).append(myProjectElement.first());
    };

    BaseController.prototype.takeFreelanceProject = function (projectElement) {
        var _this = this;

        //console.log(_this.parentElement.firstChild.innerText);
        var projectId = parseInt(projectElement.firstChild.innerHTML);

        _this._data.freelanceProjects.takeFreelanceProject(projectId)
                   .then(function (project) {
                       projectElement.remove();
                       visualizeMyProject(_this, project, $("#myProjectUl"));
                       console.log(project);
                   },
                   function (error) {
                       alert(error.responseJSON['Message']);
                       console.log(error);
                       // body...
                   });
    };

    BaseController.prototype.workOnFreelanceProject = function (projectElement) {
        var _this = this;

        //console.log(_this.parentElement.firstChild.innerText);
        var projectId = parseInt(projectElement.firstChild.innerHTML);

        _this._data.freelanceProjects.workOnMineFreelanceProject(projectId)
                   .then(function (project) {
                       var projectsContainer = projectElement.parentElement;
                       projectElement.remove();
                       visualizeMyProject(_this, project, projectsContainer);
                       console.log(project);
                   },
                   function (error) {
                       alert(error.responseJSON['Message']);
                       console.log(error);
                       // body...
                   });
    };

    BaseController.prototype.loadDrinks = function (selector) {
        var _this = this;
        _this._data.drinks.names()
                .then(function (drinks) {
                    var drinksContainer = $('<ul>');
                    for (var drinkKey in drinks) {
                        var drinkData = {
                            Name: drinks[drinkKey].Name,
                            Cost: drinks[drinkKey].Cost
                        };

                        var template = "<li><p hidden id='drinkId'>{{Id}}</p><h3>{{Name}}</h3><section><p>Cost:{{Cost}}</p></section></li>";
                        var drinkElement = $(Mustache.to_html(template, drinkData));

                        $('<input>')
                            .attr('class', 'takeButton')
                            .attr('type', 'button')
                            .attr('value', 'Get Drink')
                            .click('click', function () {
                                BaseController.prototype.getDrink.call(_this, this.parentElement);
                            })
                            .appendTo(drinkElement.last());

                        drinksContainer.append(drinkElement);

                        $('<input>')
                            .attr('class', 'takeButton')
                            .attr('type', 'button')
                            .attr('value', 'Buy Drink')
                            .click('click', function () {
                                BaseController.prototype.buyDrink.call(_this, this.parentElement);
                            })
                            .appendTo(drinkElement.last());

                        drinksContainer.append(drinkElement);
                    }

                    $(selector).append(drinksContainer);
                },
                function (error) {
                    alert(error.responseJSON['Message']);
                    console.log(error);
                    // body...
                });
    };

    BaseController.prototype.loadHardwareParts = function (selector) {
        var _this = this;
        _this._data.hardwareParts.names()
                .then(function (hardwareParts) {
                    var hardwareContainer = $('<ul>');
                    for (var hardwareKey in hardwareParts) {
                        var hardwareData = {
                            Name: hardwareParts[hardwareKey].Name,
                            Cost: hardwareParts[hardwareKey].Cost
                        };

                        var template = "<li><p hidden id='drinkId'>{{Id}}</p><h3>{{Name}}</h3><section><p>Cost:{{Cost}}</p><p>Energy:{{Energy}}</p></section></li>";
                        var hardwareElement = $(Mustache.to_html(template, hardwareData));

                        $('<input>')
                            .attr('class', 'takeButton')
                            .attr('type', 'button')
                            .attr('value', 'Get Hardware')
                            .click('click', function () {
                                BaseController.prototype.getHardware.call(_this, this.parentElement);
                            })
                            .appendTo(hardwareElement.last());

                        hardwareContainer.append(hardwareElement);

                        $('<input>')
                            .attr('class', 'takeButton')
                            .attr('type', 'button')
                            .attr('value', 'Buy Hardware')
                            .click('click', function () {
                                BaseController.prototype.buyHardware.call(_this, this.parentElement);
                            })
                            .appendTo(hardwareElement.last());

                        hardwareContainer.append(hardwareElement);
                    }

                    $(selector).append(hardwareContainer);
                },
                function (error) {
                    alert(error.responseJSON['Message']);
                    console.log(error);
                    // body...
                });
    };

    BaseController.prototype.getDrink = function (drinkElement) {
        var _this = this;

        //console.log(_this.parentElement.firstChild.innerText);
        var drinkId = parseInt(drinkElement.firstChild.innerHTML);

        _this._data.drinks.takeDrink(drinkId)
                   .then(function (drink) {
                       projectElement.remove();
                       console.log(drink);
                   },
                   function (error) {
                       alert(error.responseJSON['Message']);
                       console.log(error);
                       // body...
                   });
    };

    BaseController.prototype.buyDrink = function (drinkElement) {
        var _this = this;

        //console.log(_this.parentElement.firstChild.innerText);
        var drinkId = parseInt(drinkElement.firstChild.innerHTML);

        _this._data.drinks.buyDrink(drinkId)
                   .then(function (drink) {
                       projectElement.remove();
                       console.log(drink);
                   },
                   function (error) {
                       alert(error.responseJSON['Message']);
                       console.log(error);
                       // body...
                   });
    };

    BaseController.prototype.getHardware = function (hardwareElement) {
        var _this = this;

        //console.log(_this.parentElement.firstChild.innerText);
        var hardwareId = parseInt(hardwareElement.firstChild.innerHTML);

        _this._data.hardwareParts.takeHardware(hardwareId)
                   .then(function (hardware) {
                       projectElement.remove();
                       console.log(hardware);
                   },
                   function (error) {
                       alert(error.responseJSON['Message']);
                       console.log(error);
                       // body...
                   });
    };

    BaseController.prototype.buyHardware = function (hardwareElement) {
        var _this = this;

        //console.log(_this.parentElement.firstChild.innerText);
        var drinkId = parseInt(hardwareElement.firstChild.innerHTML);

        _this._data.hardwareParts.buyHardware(hardwareId)
                   .then(function (hardware) {
                       projectElement.remove();
                       console.log(hardware);
                   },
                   function (error) {
                       alert(error.responseJSON['Message']);
                       console.log(error);
                       // body...
                   });
    };

    BaseController.prototype.attachEventHandlers = function () {
        var selector = '#wrapper';
        attachLoginHandler.call(this, selector);
        attachRegisterHandler.call(this, selector);
		attachSolveHomeworkHandler.call(this, selector)
    };

    var attachLoginHandler = function (selector) {
        var _this = this;
        $(selector).on('click', '#login', function () {
            var username = $('#username').val();
            var password = $('#password').val();
            _this._data.users.login(username, password)
				.then(function (data) {
				    console.log("Logged in successfully");
					app.sammy.setLocation('#/homework');
				    // redirect
				    //app.router.setLocation('#/Freelance-projects');
				},
				function (error) {
				    // body...
				    alert(error.responseJSON['Message']);
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
				    alert(error.responseJSON['Message']);
				});
        });
    };

	var attachSolveHomeworkHandler = function (selector) {
		var _this = this;
		debugger;
		$(selector).on('click', '.solve-homework', function (ev) {
			var homeworkId = ev.target.id;
			_this._data.Homework.solve(homeworkId)
				.then(function (data) {
					alert(data);
				},
				function (error) {
					debugger;
					alert(error.responseJSON['Message']);
				});
		});
	};

    return {
        get: function (data) {
            return new BaseController(data);
        }
    }
}());