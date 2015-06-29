declare module Boligf {
	var Config: Boligf.IConfig;
}

module Boligf {

	export interface IBoligfApp extends ng.IModule { }
	export interface IConfig {
		ApiAccess(hideApi?: boolean): string;
	}
	
	export var App: IBoligfApp;

	export class Startup {

		constructor() {

			App = angular.module('BoligfApp', [
				'ui.router',
				'ngResource',
				'ngCookies',
				'pascalprecht.translate'
			]);
		}

		public Run() {

			this.setupConfiguration();
		}

		private setupConfiguration() {

			var providerInjects: any[] = [
				'$stateProvider',
				'$urlRouterProvider',
				'$httpProvider',
				'$translateProvider',
				'$locationProvider'
			];

			var configFunc = (
				stateProvider: ng.ui.IStateProvider,
				urlRouterProvider: ng.ui.IUrlRouterProvider,
				httpProvider: ng.IHttpProvider,
				translateProvider: ng.translate.ITranslateProvider,
				locationProvider: ng.ILocationProvider
			) => {

				this.setupTranslations(translateProvider);

				this.setupDefaultRouting(urlRouterProvider, locationProvider);

				this.setupStates(stateProvider);

				this.setupHttpInterceptors(httpProvider);

			};

			providerInjects.push(configFunc);

			App.config(providerInjects);
		}

		private setupTranslations(translateProvider: ng.translate.ITranslateProvider) {

			translateProvider.useStaticFilesLoader({
				prefix: '/Locale/locale-',
				suffix: '.json'
			});

			translateProvider.preferredLanguage('en');
		}

		private setupDefaultRouting(urlRouterProvider: ng.ui.IUrlRouterProvider, locationProvider: ng.ILocationProvider) {

			urlRouterProvider.otherwise("/");
			urlRouterProvider.when("/association/registermember", "/association/registermember/");

			locationProvider.html5Mode(true);
		}

		private setupStates(stateProvider: ng.ui.IStateProvider) {

			stateProvider
				.state(Boligf.States.Default.Home, {
					url: '/',
					templateUrl: "/Application/Pages/Home/Home.html"
				})
				.state(Boligf.States.Default.News, {
					url: '/news',
					templateUrl: "/Application/Pages/"
				})
				.state(Boligf.States.Default.Documents, {
					url: '/documents',
					templateUrl: "/Application/Pages/"
				})
				.state(Boligf.States.Default.Board, {
					url: '/board',
					templateUrl: "/Application/Pages/"
				})
				.state(Boligf.States.Default.Residents, {
					url: '/residents',
					templateUrl: "/Application/Pages/"
				})
				.state(Boligf.States.Authentication.Base, {
					url: '/authentication',
					templateUrl: "/Application/Pages/Authentication/Authentication.html",
					controller: "AuthenticationController",
					controllerAs: "authentication"
				})
				.state(Boligf.States.Authentication.Login, {
					url: '/login',
					templateUrl: "/Application/Pages/Authentication/Login.html",
					controller: "LoginController",
					controllerAs: "loginCtrl"
				})
				.state(Boligf.States.Association.Base, {
					url: '/association',
					templateUrl: "/Application/Pages/Association/Association.html",
					controller: "AssociationController",
					controllerAs: "associationCtrl"
				})
				.state(Boligf.States.Association.Register, {
					url: '/register',
					templateUrl: "/Application/Pages/Association/Register.html",
					controller: "Association_RegisterController",
					controllerAs: "registerCtrl"
				})
				.state(Boligf.States.Association.RegisterMember, {
					url: '/registermember/:code',
					templateUrl: "/Application/Pages/Association/Member/RegisterMember.html",
					controller: "Association_RegisterMemberController",
					controllerAs: "registerMemberCtrl"
				})
				.state(Boligf.States.Errors.E404, {
					url: '/404',
					template: "<span>404 page</span>"
				})
				.state(Boligf.States.Errors.E403, {
					url: '/403',
					template: "<span>403 page</span>"
				});
		}

		private setupHttpInterceptors(httpProvider: ng.IHttpProvider) {

			httpProvider.interceptors.push("IInterceptHttpProvider");
		}

		public static Initialize() {

			return new Startup();
		}
	}
}

// TODO: Find better way to do this
var app = Boligf.Startup.Initialize();