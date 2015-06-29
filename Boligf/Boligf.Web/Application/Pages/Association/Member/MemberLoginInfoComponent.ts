module Boligf {

	export interface IMemberLoginInfoComponentScope extends ng.IScope {
		isProtected: boolean;
	}

	export interface IMemberLoginInfoDirective extends ng.IDirective {
		restrict: string;
		scope: any;
		controller: any;
		controllerAs: string;
		replace: boolean;
		link(scope: ng.IScope, element: JQuery, attributes: ng.IAttributes, controller: IMemberLoginInfoComponent): void;
	}

	export interface IMemberLoginInfoComponent {
		ready: boolean;
		streetName: string;
		no: string;
		floor: string;
		door: string;
		isFloorAndDoorAvailable: boolean;
		fillMemberInfo(): void;
	}

	export class MemberLoginInfoComponent implements IMemberLoginInfoComponent {

		public ready: boolean;
		public streetName: string;
		public no: string;
		public floor: string;
		public door: string;

		public get isFloorAndDoorAvailable(): boolean {

			if (this.no && this.floor) {
				return true;
			}

			return false;
		}

		static $inject = ['IAssociationMemberService', 'IStoreUserData', 'IStoreAssociationData'];

		constructor(
			private associationMemberService: IAssociationMemberService,
			private userDataStore: IStoreUserData,
			private associationDataStore: IStoreAssociationData
			) {
			
		}

		public fillMemberInfo(): void {
			
			if (this.associationDataStore.associationId) {

				this.associationMemberService.setup(this.associationDataStore.associationId).getSingle(this.userDataStore.userId).then((member: IAssociationMember) => {

					this.streetName = member.address.streetAddress;
					this.no = member.address.no;
					this.floor = member.address.floor;
					this.door = member.address.door;

					this.ready = true;
				});
			}
		}
	}

	export function memberLoginInfoDirective(authenticationService: IAuthenticationService): ng.IDirective {
		
		function link(scope: IMemberLoginInfoComponentScope, element: JQuery, attributes: ng.IAttributes, controller: IMemberLoginInfoComponent): void {

			scope.$watch(
				() => {
					return authenticationService.isAuthenticated;
				},
				(newValue, oldValue) => {
					if (newValue) {
						controller.fillMemberInfo();
					}
				}
			);
		}

		var directive = <IMemberLoginInfoDirective> {
			restrict: 'AE',
			scope: {},
			replace: true,
			templateUrl: '/Application/Pages/Association/Member/MemberLoginInfoComponent.html',
			controller: ['IAssociationMemberService', 'IStoreUserData', 'IStoreAssociationData', Boligf.MemberLoginInfoComponent],
			controllerAs: "memberLoginInfoCtrl",
			link: link
		}

		return directive;
	}

	Boligf.App.directive("boligfMemberLoginInfo", ['IAuthenticationService', Boligf.memberLoginInfoDirective]);
} 