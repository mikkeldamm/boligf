module Boligf {

	export interface IAuthenticationService {
		isAuthenticated: boolean;
		login(email: string, password: string): ng.IPromise<boolean>;
	}

	export class AuthenticationService implements IAuthenticationService {

		static $inject = ['$http', '$q', 'IStoreBearerToken'];

		private _isAuthenticated: boolean;

		public get isAuthenticated() {
			return this._isAuthenticated;
		}

		constructor(
			private $http: ng.IHttpService,
			private $q: ng.IQService,
			private bearerTokenStore: Boligf.IStoreBearerToken
			) {

			if (this.bearerTokenStore.anyToken()) {
				this._isAuthenticated = true;
			} else {
				this._isAuthenticated = false;
			}
		}

		public login(email: string, password: string): ng.IPromise<boolean> {

			var defer = this.$q.defer();
			var data: string = "grant_type=password&username=" + email + "&password=" + password;

			this.$http.post(Boligf.Config.ApiDomainClean + '/token', data).success((response: any) => {

				this.bearerTokenStore.token = response.access_token;
				this._isAuthenticated = true;

				defer.resolve(true);

			}).error((err, status)  => {

				defer.resolve(false);
			});

			return defer.promise;
		}
	}

	Boligf.App.service("IAuthenticationService", Boligf.AuthenticationService);
} 