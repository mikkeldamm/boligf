module Boligf {

	export interface IStoreBearerToken {
		token: string;
		anyToken(): boolean;
		deleteToken();
	}

	export class BearerTokenStorageService implements IStoreBearerToken {

		static $inject = ['$cookieStore'];

		public get token() {
			return this.$cookies.get("auth_token");
		}

		public set token(value) {
			this.$cookies.put("auth_token", value);
		}

		constructor(private $cookies: ng.cookies.ICookieStoreService) {
			
		}

		public anyToken(): boolean {

			if (this.$cookies.get("auth_token")) {
				return true;
			} else {
				return false;
			}
		}

		public deleteToken() {

			this.$cookies.remove("auth_token");
		}
	}

	Boligf.App.service('IStoreBearerToken', Boligf.BearerTokenStorageService);
} 