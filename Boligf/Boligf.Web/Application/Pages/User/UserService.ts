module Boligf {

	export class UserProfile {

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
		delete(userId: string): ng.IPromise<boolean>;
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

		delete(userId: string): ng.IPromise<boolean> {

			return this.apiService.delete<boolean>("/user/" + userId);
		}
	}

	Boligf.App.service("IUserService", Boligf.UserService);
} 