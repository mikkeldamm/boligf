module Boligf {

	export interface IIsAuthorizedDirectiveScope extends ng.IScope {
		isProtected: boolean;
	}

	export interface IIsAuthorizedDirective extends ng.IDirective {
		restrict: string;
		scope: any;
		link(scope: ng.IScope, element: JQuery, attributes: ng.IAttributes): void;
	}

	export function isAuthorizedDirective(authenticationService: Boligf.IAuthenticationService): ng.IDirective {

		var directive = <IIsAuthorizedDirective> {
			restrict: 'A',
			scope: {
				isProtected: "=boligfIsAuthorized"
			},
			link: link
		}

		function link(scope: IIsAuthorizedDirectiveScope, element: JQuery, attributes: ng.IAttributes): void {

			scope.$watch(
				() => {
					return authenticationService.isAuthenticated;
				},
				(newValue, oldValue) => {
					if (newValue) {
						if (scope.isProtected) {
							element.show();
						} else {
							element.hide();
						}
					} else {
						if (!scope.isProtected) {
							element.show();
						} else {
							element.hide();
						}
					}
				}
			);
		}

		return directive;
	}

	Boligf.App.directive("boligfIsAuthorized", ['IAuthenticationService', Boligf.isAuthorizedDirective]);
} 