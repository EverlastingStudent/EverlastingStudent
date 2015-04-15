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
            app.view.homeView();
        });

        this.get('#/register', function() {
            controller.loadRegister(selector);
        }); 


        this.get('#/Freelance-projects', function () {
            controller.loadFreelanceProjects(selector);
        });
    });

    app.router.run('#/');
}());