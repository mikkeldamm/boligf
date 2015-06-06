module Boligf {

	export interface IInterceptHttpProvider {
		request(config: ng.IRequestConfig): ng.IRequestConfig;
		requestError(rejection: ng.IHttpPromiseCallback<any>): ng.IPromise<any>;
		responseError(rejection: ng.IHttpPromiseCallbackArg<any>): ng.IPromise<any>;
	}

	export class BearerTokenInterceptor implements IInterceptHttpProvider {

		static $inject = ['$q', 'IStoreBearerToken'];

		constructor(private $q: ng.IQService, private bearerTokenStorageService: Boligf.IStoreBearerToken) {

		}

		public request = (config: ng.IRequestConfig): ng.IRequestConfig => {

			config.headers = config.headers || {};

			if (this.bearerTokenStorageService.anyToken()) {

				config.headers.Authorization = 'Bearer ' + this.bearerTokenStorageService.token;
				config.headers.ContentType = 'application/x-www-form-urlencoded';
			}

			return config;
		};

		public requestError = (rejection: ng.IHttpPromiseCallback<any>): ng.IPromise<any> => {

			return this.$q.reject(rejection);
		};

		public responseError = (rejection: ng.IHttpPromiseCallbackArg<any>): ng.IPromise<any> => {

			if (rejection != null && rejection.status === 401 && this.bearerTokenStorageService.anyToken()) {

				this.bearerTokenStorageService.deleteToken();

				//this.$state.go(common.States); // TODO: Find a way to redirect here
			}

			return this.$q.reject(rejection);

		};
	}

	Boligf.App.service('IInterceptHttpProvider', Boligf.BearerTokenInterceptor);
} 