module Boligf {

	export interface IStoreBearerToken {
		token: string;
		anyToken(): boolean;
		deleteToken();
	}

	export class BearerTokenStorageService implements IStoreBearerToken {

		private static tokenKey: string = "auth_token";

		static $inject = ['$cookieStore'];

		public get token() {
			return this.$cookies.get(BearerTokenStorageService.tokenKey);
		}

		public set token(value) {
			this.$cookies.put(BearerTokenStorageService.tokenKey, value);
		}

		constructor(private $cookies: ng.cookies.ICookieStoreService) {
			
		}

		public anyToken(): boolean {

			if (this.$cookies.get(BearerTokenStorageService.tokenKey)) {
				return true;
			} else {
				return false;
			}
		}

		public deleteToken() {

			this.$cookies.remove(BearerTokenStorageService.tokenKey);
		}
	}

	Boligf.App.service('IStoreBearerToken', Boligf.BearerTokenStorageService);
} 