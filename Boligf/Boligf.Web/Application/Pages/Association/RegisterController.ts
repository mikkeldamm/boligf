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
					this.$state.go(Boligf.States.Default.Home);

				}).catch(() => {

					this.userService.delete(userId).then((isDeleted:boolean) => {
						if (isDeleted) {

							console.log("user is deleted because association could not be created");
							this.$state.go(Boligf.States.Default.Home);
						}
					});
				});
			});
		}
	}

	Boligf.App.controller("Association_RegisterController", Boligf.RegisterController);
} 