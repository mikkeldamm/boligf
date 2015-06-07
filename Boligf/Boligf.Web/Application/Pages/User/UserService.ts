module Boligf {

	export interface UserProfile {

		id: string;
		email: string;
		firstName: string;
		lastName: string;
	}
	
	export interface IRegisterUser {
		
		email: string;
		password: string;
	}

	export interface IUserService {

		post(model: IRegisterUser): ng.IPromise<string>;
	}

	export class UserService implements IUserService {

		static $inject = ['IApiService'];
		
		constructor(
			private apiService: IApiService
			) {
			
		}

		post(model: IRegisterUser): ng.IPromise<string> {

			return this.apiService.post<string>("/user", model);
		}
	}

	Boligf.App.service("IUserService", Boligf.UserService);
} 