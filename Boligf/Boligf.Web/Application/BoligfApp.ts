/// <reference path="../Scripts/_references.ts"/>

module Boligf {

	export interface IBoligfApp extends ng.IModule { }
	
	export class Startup {

		static BoligfApp: IBoligfApp;

		constructor() {

			Startup.BoligfApp = angular.module('BoligfApp', [
				'ngResource',
				'ui.router',
				'pascalprecht.translate'
			]);

			this.SetupConfiguration();
		}

		private SetupConfiguration() {

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

				this.SetupTranslations(translateProvider);

				this.SetupDefaultRouting(urlRouterProvider, locationProvider);

				this.SetupStates(stateProvider);

				this.SetupHttpInterceptors(httpProvider);

			};

			providerInjects.push(configFunc);

			Startup.BoligfApp.config(providerInjects);
		}

		private SetupTranslations(translateProvider: ng.translate.ITranslateProvider) {

			translateProvider.useStaticFilesLoader({
				prefix: '/Locale/locale-',
				suffix: '.json'
			});

			translateProvider.preferredLanguage('en');
		}

		private SetupDefaultRouting(urlRouterProvider: ng.ui.IUrlRouterProvider, locationProvider: ng.ILocationProvider) {

			urlRouterProvider.otherwise("/");

			locationProvider.html5Mode(true);
		}

		private SetupStates(stateProvider: ng.ui.IStateProvider) {

			stateProvider
				.state(Boligf.States.Home, {
					url: '/',
					templateUrl: "" // TODO: Get template url via template handler
				})
				.state(Boligf.States.Login, {
					url: '/login',
					templateUrl: "" // TODO: Get template url via template handler
				})
				.state(Boligf.States.Logout, {
					url: '/logout',
					templateUrl: "" // TODO: Get template url via template handler
				});
		}

		private SetupHttpInterceptors(httpProvider: ng.IHttpProvider) {

			//httpProvider.interceptors.push("IInterceptHttpProvider");
		}

		static Run() {

			return new Startup();
		}
	}
}

Boligf.Startup.Run();
