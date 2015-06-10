module Boligf {
	
	export interface IApiService {
		get<T>(url: string, config?: ng.IRequestShortcutConfig): ng.IPromise<T>;	
		delete<T>(url: string, config?: ng.IRequestShortcutConfig): ng.IPromise<T>;
		post<T>(url: string, data: any, config?: ng.IRequestShortcutConfig): ng.IPromise<T>;
		put<T>(url: string, data: any, config?: ng.IRequestShortcutConfig): ng.IPromise<T>;
	}

	export class ApiService implements IApiService {

		static $inject = ['$http', '$q'];

		constructor(
			private $http: ng.IHttpService,
			private $q: ng.IQService
			) {

		}

		get<T>(url: string, config?: ng.IRequestShortcutConfig): ng.IPromise<T> {

			var defer = this.$q.defer();

			this.$http.get(Boligf.Config.ApiAccess() + url).then((response) => {
				defer.resolve(response.data);
			}).catch(defer.reject);

			return defer.promise;
		}

		delete<T>(url: string, config?: ng.IRequestShortcutConfig): ng.IPromise<T> {

			var defer = this.$q.defer();

			this.$http.delete(Boligf.Config.ApiAccess() + url).then((response) => {
				defer.resolve(response.data);
			}).catch(defer.reject);

			return defer.promise;
		}

		post<T>(url: string, data: any, config?: ng.IRequestShortcutConfig): ng.IPromise<T> {

			var defer = this.$q.defer();

			this.$http.post(Boligf.Config.ApiAccess() + url, data).then((response) => {
				defer.resolve(response.data);
			}).catch(defer.reject);

			return defer.promise;
		}

		put<T>(url: string, data: any, config?: ng.IRequestShortcutConfig): ng.IPromise<T> {

			var defer = this.$q.defer();

			this.$http.put(Boligf.Config.ApiAccess() + url, data).then((response) => {
				defer.resolve(response.data);
			}).catch(defer.reject);

			return defer.promise;
		}
	}

	Boligf.App.service("IApiService", Boligf.ApiService);
} 