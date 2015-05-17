/// <reference path="../Scripts/_references.ts"/>
var Boligf;
(function (Boligf) {
    var States = (function () {
        function States() {
        }
        States.Home = "home";
        States.Login = "login";
        States.Logout = "logout";
        return States;
    })();
    Boligf.States = States;
})(Boligf || (Boligf = {}));
// Typings
/// <reference path="typings/angularjs/angular.d.ts"/>
/// <reference path="typings/angularjs/angular-route.d.ts"/>
/// <reference path="typings/angularjs/angular-resource.d.ts"/>
/// <reference path="typings/angularjs/angular-cookies.d.ts"/>
/// <reference path="typings/angularjs/angular-mocks.d.ts"/>
/// <reference path="typings/angularjs/angular-sanitize.d.ts"/>
/// <reference path="typings/angularjs/angular-animate.d.ts"/>
/// <reference path="typings/angularjs/angular-translate.d.ts"/>
/// <reference path="typings/angularjs/angular-ui-router.d.ts"/>
/// <reference path="typings/jquery/jquery.d.ts"/>
// Application statics
/// <reference path="../Application/States.ts"/>
// Application uses
/// <reference path="../Application/BoligfApp.ts"/> 
/// <reference path="../Scripts/_references.ts"/>
var Boligf;
(function (Boligf) {
    var Startup = (function () {
        function Startup() {
            Startup.BoligfApp = angular.module('BoligfApp', [
                'ui.router',
                'pascalprecht.translate'
            ]);
            this.setupConfiguration();
        }
        Startup.prototype.setupConfiguration = function () {
            var _this = this;
            var providerInjects = [
                '$stateProvider',
                '$urlRouterProvider',
                '$httpProvider',
                '$translateProvider',
                '$locationProvider'
            ];
            var configFunc = function (stateProvider, urlRouterProvider, httpProvider, translateProvider, locationProvider) {
                _this.setupTranslations(translateProvider);
                _this.setupDefaultRouting(urlRouterProvider, locationProvider);
                _this.setupStates(stateProvider);
                _this.setupHttpInterceptors(httpProvider);
            };
            providerInjects.push(configFunc);
            Startup.BoligfApp.config(providerInjects);
        };
        Startup.prototype.setupTranslations = function (translateProvider) {
            translateProvider.useStaticFilesLoader({
                prefix: '/Locale/locale-',
                suffix: '.json'
            });
            translateProvider.preferredLanguage('en');
        };
        Startup.prototype.setupDefaultRouting = function (urlRouterProvider, locationProvider) {
            urlRouterProvider.otherwise("/");
            locationProvider.html5Mode(true);
        };
        Startup.prototype.setupStates = function (stateProvider) {
            stateProvider.state(Boligf.States.Home, {
                url: '/',
                templateUrl: "/Application/Pages/Authentication/Authentication.html" // TODO: Get template url via template handler
            });
            //.state(Boligf.States.Login, {
            //	url: '/login',
            //	templateUrl: "" // TODO: Get template url via template handler
            //})
            //.state(Boligf.States.Logout, {
            //	url: '/logout',
            //	templateUrl: "" // TODO: Get template url via template handler
            //});
        };
        Startup.prototype.setupHttpInterceptors = function (httpProvider) {
            //httpProvider.interceptors.push("IInterceptHttpProvider");
        };
        Startup.Run = function () {
            return new Startup();
        };
        return Startup;
    })();
    Boligf.Startup = Startup;
})(Boligf || (Boligf = {}));
Boligf.Startup.Run();
