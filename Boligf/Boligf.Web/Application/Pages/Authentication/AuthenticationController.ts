module Boligf {

	export interface IAuthenticationControllerScope extends ng.IScope {
		testM(): void;
		testM2(): void;
		lars: string;
	}

	export class AuthenticationController {

		static $inject = ['$scope', '$http', 'IAuthenticationService', 'IStoreBearerToken'];

		lars: string;

		constructor(
			private $scope: IAuthenticationControllerScope,
			private $http: ng.IHttpService,
			private authenticationService: IAuthenticationService,
			private bearerTokenStore: Boligf.IStoreBearerToken
			) {

			console.log("Auth ctrl init");
			this.lars = "mikkel";
		}

		testM(): void {

			//var token = this.authenticationService.login("mikkel@damm.dk", "1234");

			console.log("Click event");
			console.log("-- token: " + this.bearerTokenStore.token);

			this.$http.get('/api/authentication', {}).success((data) => {

				console.log("test call");
				console.log(data);
			});
		}

		testM2(): void {

			console.log("Click event");

			this.$http.get('/api/authentication/4', {}).success((data) => {

				console.log("test call 2");
				console.log(data);
			});
		}
	}

	Boligf.App.controller("AuthenticationController", Boligf.AuthenticationController);
}