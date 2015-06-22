module Boligf {
	
	export interface IRegisterMemberController {
		
		registerCode: string;
		register(): void;
	}

	export class RegisterMemberController implements IRegisterMemberController {

		static $inject = ['$state', 'IUserService'];

		public registerCode: string;
		
		constructor(
			private $state: ng.ui.IStateService,
			private $stateParams: ng.ui.IStateParamsService,
			private userService: IUserService
			) {

			this.registerCode = this.$stateParams["code"];
		}

		register(): void {
			
		}
	}

	Boligf.App.controller("Association_RegisterMemberController", Boligf.RegisterMemberController);
} 