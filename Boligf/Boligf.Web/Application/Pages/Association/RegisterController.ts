module Boligf {
	
	export interface IRegisterController {

		association: IRegisterAssociation;
		register(): void;
		addressSelected(address: any): void;
	}

	export class RegisterController implements IRegisterController {

		static $inject = ['$rootScope', '$state', 'IUserService', 'IAssociationService'];

		association: IRegisterAssociation;
		user: IRegisterUser;

		constructor(
			private $rootScope: IRootScope,
			private $state: ng.ui.IStateService,
			private userService: IUserService,
			private associationService: IAssociationService
			) {

			this.association = <IRegisterAssociation>{};
		}

		register(): void {

			this.$rootScope.isLoading = true;

			this.userService.post(this.user).then((userId: string) => {

				this.association.userId = userId;
				this.associationService.post(this.association).then((associationId: string) => {
					
					this.$rootScope.isLoading = false;
					this.$state.go(Boligf.States.Default.Home);

				}).catch(() => {

					this.userService.delete(userId).then((isDeleted:boolean) => {
						if (isDeleted) {

							console.log("user is deleted because association could not be created");
							this.$rootScope.isLoading = false;
							this.$state.go(Boligf.States.Default.Home);
						}
					});
				});
			});
		}

		addressSelected(address: IAddressAutocompleteResult): void {

			this.association.addressId = address.id;
			this.association.streetAddress = address.streetname;
			this.association.no = address.no;
			this.association.floor = address.floor;
			this.association.door = address.door;
			this.association.zip = address.zip;
			this.association.city = address.city;
		}
	}

	Boligf.App.controller("Association_RegisterController", Boligf.RegisterController);
} 