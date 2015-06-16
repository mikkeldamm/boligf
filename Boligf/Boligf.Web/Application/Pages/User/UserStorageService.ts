module Boligf {

	export interface IStoreUserData {
		userId: string;
		userName: string;
		clear(): void;
	}

	export class UserStorageService implements IStoreUserData {

		static $inject = ['$cookieStore'];

		public get userId() {
			return this.$cookies.get("userId");
		}

		public set userId(value) {
			this.$cookies.put("userId", value);
		}

		public get userName() {
			return this.$cookies.get("userName");
		}

		public set userName(value) {
			this.$cookies.put("userName", value);
		}

		constructor(private $cookies: ng.cookies.ICookieStoreService) {
			
		}
		
		public clear() {

			this.$cookies.remove("userId");
			this.$cookies.remove("userName");
		}
	}

	Boligf.App.service('IStoreUserData', Boligf.UserStorageService);
} 