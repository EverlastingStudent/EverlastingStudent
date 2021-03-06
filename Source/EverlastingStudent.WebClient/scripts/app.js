var app = app || {};

(function () {
    var baseUrl = 'http://localhost:41831/';
    var ajaxRequester = app.ajaxRequester.get();
    var data = app.data.get(baseUrl, ajaxRequester);
    var controller = app.controller.get(data);
    controller.attachEventHandlers();

    app.router = Sammy(function() {
        var selector = '#wrapper';
        this.get('#/', function() {
            controller.loadHome(selector);
        });

        this.get('#/login', function() {
            controller.loadLogin(selector);
        });

        this.get('#/login/', function() {
            app.sammy.setLocation('#/home/');
        });

        this.get('#/register', function() {
            controller.loadRegister(selector);
        });

        this.get('#/homework', function() {
            controller.loadHomework(selector);
        });

        this.get('#/freelance-projects', function () {
            controller.loadFreelanceProjects(selector);
        });

        this.get('#/drinks', function () {
            controller.loadDrinks(selector);
        });

        this.get('#/hardware', function () {
            controller.loadHardwareParts(selector);
        });

		this.get('#/timer/:duration', function() {
			var duration = this.params['duration']
			controller.loadTimer(selector, duration);
		});
    });

    app.router.run('#/');
}());