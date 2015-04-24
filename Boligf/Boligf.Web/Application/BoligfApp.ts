/// <reference path="../Scripts/_references.ts"/>

module Boligf {

	export interface IBoligfApp extends ng.IModule { }
	
	export class Startup {

		static BoligfApp: IBoligfApp;

		constructor() {

			Startup.BoligfApp = angular.module('BoligfApp', [
				'ui.router',
				'pascalprecht.translate'
			]);

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

			Startup.BoligfApp.config(providerInjects);
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

			locationProvider.html5Mode(true);
		}

		private setupStates(stateProvider: ng.ui.IStateProvider) {

			stateProvider
				.state(Boligf.States.Home, {
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
		}

		private setupHttpInterceptors(httpProvider: ng.IHttpProvider) {

			//httpProvider.interceptors.push("IInterceptHttpProvider");
		}

		public static Run() {

			return new Startup();
		}
	}
}

Boligf.Startup.Run();
