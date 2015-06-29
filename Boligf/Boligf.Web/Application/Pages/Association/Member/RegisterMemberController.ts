module Boligf {
	
	export interface IRegisterMemberController {

		user: IRegisterUser;
		addressRegistered: IRegisterUserToAddress;
		registerCode: string;
		getAddressByCode(): void;
		register(): void;
	}

	export class RegisterMemberController implements IRegisterMemberController {

		static $inject = ['$state', '$stateParams', 'IUserService', 'IAssociationAddressService'];

		user: IRegisterUser;
		addressRegistered: IRegisterUserToAddress;
		registerCode: string;
		
		constructor(
			private $state: ng.ui.IStateService,
			private $stateParams: ng.ui.IStateParamsService,
			private userService: IUserService,
			private associationAddressService: IAssociationAddressService
			) {

			this.addressRegistered = <IRegisterUserToAddress> {};
			this.registerCode = this.$stateParams["code"];
		}

		getAddressByCode(): void {

			this.associationAddressService.getCode(this.registerCode).then((addresWithCode: IAssociationAddressCode) => {
				
				this.addressRegistered.addressId = addresWithCode.id;
				this.addressRegistered.associationId = addresWithCode.associationId;
				
			}).catch(() => {
				

			});
		}

		register(): void {
			
			debugger;

			this.userService.post(this.user).then((userId: string) => {

				this.addressRegistered.userId = userId;

				this.associationAddressService.post(this.addressRegistered).then(() => {

					this.$state.go(Boligf.States.Default.Home);

				}).catch(() => {

					this.userService.delete(userId).then((isDeleted: boolean) => {
						if (isDeleted) {

							console.log("user is deleted because association could not be created");
							this.$state.go(Boligf.States.Default.Home);
						}
					});
				});
			});

		}
	}

	Boligf.App.controller("Association_RegisterMemberController", Boligf.RegisterMemberController);
} 