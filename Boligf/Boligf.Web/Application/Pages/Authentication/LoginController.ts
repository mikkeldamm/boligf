module Boligf {

	export interface ILoginController extends ng.IScope {
		email: string;
		password: string;
		login(): void;
	}

	export class LoginController {

		static $inject = ['$state', 'IAuthenticationService'];

		email: string;
		password: string;

		constructor(
			private $state: ng.ui.IStateService,
			private authenticationService: IAuthenticationService
			) {

			// Redirect user to home if is logged in
			if (this.authenticationService.isAuthenticated) {
				this.$state.go(Boligf.States.Default.Home);
			}
		}

		login(): void {

			this
				.authenticationService
				.login(this.email, this.password)
				.then((isSuccedded: boolean) => {
					
					if (isSuccedded) {
						this.$state.go(Boligf.States.Default.Home);
					} else {

						// TODO: Instead of redirect on failure, then just show error for user
						this.$state.go(Boligf.States.Errors.E403);
					}
				});
		}
	}

	Boligf.App.controller("LoginController", Boligf.LoginController);
} 