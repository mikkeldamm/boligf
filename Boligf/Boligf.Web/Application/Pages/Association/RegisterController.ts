module Boligf {
	
	export interface IRegisterController {

		association: IRegisterAssociation;
		register(): void;
	}

	export class RegisterController implements IRegisterController {

		static $inject = ['$state', 'IUserService', 'IAssociationService'];

		association: IRegisterAssociation;
		user: IRegisterUser;

		constructor(
			private $state: ng.ui.IStateService,
			private userService: IUserService,
			private associationService: IAssociationService
			) {
			
		}

		register(): void {

			this.userService.post(this.user).then((userId: string) => {
				
				this.association.userId = userId;
				this.associationService.post(this.association).then((associationId: string) => {

					console.log(associationId);
				});
			});
		}
	}

	Boligf.App.controller("Association_RegisterController", Boligf.RegisterController);
} 