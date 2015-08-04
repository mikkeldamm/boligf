module Boligf {

	export interface IAddressAutocompleteResult {
		id: string;
		streetname: string;
		no: string;
		floor: string;
		door: string;
		zip: string;
		city: string;
	}

	export interface IDawaAddress {
		id: string;
		href: string;
		vejnavn: string;
		husnr: string;
		etage: string;
		dør: string;
		supplerendebynavn: string;
		postnr: string;
		postnrnavn: string;
	}

	export interface IDawaResponseAddress {
		tekst: string;
		adresse: IDawaAddress;
	}

	export interface IAddressAutocompleteComponentScope extends ng.IScope {
		searchQuery: any;
		showList: any;
		hideList: any;
	}

	export interface IAddressAutocompleteDirective extends ng.IDirective {
		restrict: string;
		scope: any;
		controller: any;
		controllerAs: string;
		replace: boolean;
		link(scope: IAddressAutocompleteComponentScope, element: JQuery, attributes: ng.IAttributes, controller: IAddressAutocompleteComponent): void;
	}

	export interface IAddressAutocompleteComponent {
		lookupAddress();
		selectAddress(address: IDawaResponseAddress);
		addressSelected: any;
		addresses: any[];
		searchQuery: any;
		showList;
		hideList;
		getIndexToNextAddress(): number;
		getIndexToPreviousAddress(): number;
		selectCurrentAddresss(): void;
		resetCurrentIndex(): void;
		currentAddressIndex: number;
	}

	export class AddressAutocompleteComponent implements IAddressAutocompleteComponent {

		public searchQuery: any;
		public addresses: any[];
		public shouldShowListBox: boolean;
		public listBoxTop: number;
		public currentAddressIndex: number;
		
		showList: any;
		hideList: any;
		addressSelected: any;

		private timeoutPromise: ng.IPromise<any>;

		static $inject = ['$http', '$timeout'];

		constructor(private $http: ng.IHttpService, private $timeout: ng.ITimeoutService) {

			this.currentAddressIndex = 0;
		}

		public lookupAddress(): void {
			
			if (this.timeoutPromise) {
				this.$timeout.cancel(this.timeoutPromise);
			}

			if (!this.searchQuery || this.searchQuery.length <= 1) {
				this.hideList();
				return;
			}

			this.timeoutPromise = this.$timeout(() => {

				this.getAddressFromQuery();

			}, 250);
		}
		
		public getIndexToNextAddress(): number {

			if (this.currentAddressIndex === (this.addresses.length - 1)) {
				this.currentAddressIndex = 0;
			} else {
				this.currentAddressIndex += 1;
			}

			return this.currentAddressIndex;
		}

		public getIndexToPreviousAddress(): number {

			if (this.currentAddressIndex <= 0) {
				this.currentAddressIndex = (this.addresses.length - 1);
			} else {
				this.currentAddressIndex -= 1;
			}

			return this.currentAddressIndex;
		}

		public selectCurrentAddresss(): void {
			
			this.selectAddress(this.addresses[this.currentAddressIndex]);
		}

		public resetCurrentIndex(): void {

			this.currentAddressIndex = 0;
		}

		public selectAddress(address: IDawaResponseAddress): void {
			
			this.searchQuery = address.tekst;

			var addressResult = <IAddressAutocompleteResult> {
				id: address.adresse.id,
				streetname: address.adresse.vejnavn,
				no: address.adresse.husnr,
				floor: address.adresse.etage,
				door: address.adresse.dør,
				zip: address.adresse.postnr,
				city: address.adresse.postnrnavn
			}

			this.addressSelected({ address: addressResult });
			this.resetCurrentIndex();
			this.hideList();
		}

		private getAddressFromQuery() {
			
			this.$http.get("https://dawa.aws.dk/adresser/autocomplete?per_side=8&q=" + this.searchQuery).then((a:any) => {

				this.addresses = a.data.slice(0, 8);
				this.showList();

			}).catch((b) => {

				// Write here that the address cant be found
				console.log(b);
			});
		}
	}

	export function addressAutocompleteDirective(): ng.IDirective {

		function link(scope: IAddressAutocompleteComponentScope, element: JQuery, attributes: ng.IAttributes, controller: IAddressAutocompleteComponent): void {

			var addressJustSelected: boolean = false;
			var inputElement = element.find('input');
			var listBox = element.find('.autocomplete-box');

			function markCurrentAddress(index) {

				var items = listBox.find('li');

				items.removeClass('selected');

				var item = $(items[index]);
				
				item.addClass('selected');
			}

			function showList() {

				if (listBox.css('top') === "0px") {
					
					listBox.css('top', inputElement.outerHeight());
				}

				listBox.addClass('show');
				
				setTimeout(() => { markCurrentAddress(controller.currentAddressIndex); }, 100);
			}

			function hideList() {
				
				listBox.removeClass('show');
			}

			controller.showList = showList;
			controller.hideList = hideList;

			// Disable default browser autocomplete
			inputElement.attr('autocomplete', 'off');
			
			// Bind focus & blur event
			inputElement.off('focus').on('focus', () => {

				if (addressJustSelected) {
					addressJustSelected = false;
					return false;
				}

				controller.lookupAddress();
			});
			//inputElement.off('blur').on('blur', () => hideList());
			
			// Bind arrow and tab events
			element.off('keyup').on('keyup', (e: JQueryEventObject) => {
				
				if (e.keyCode === 40) { // Down

					markCurrentAddress(controller.getIndexToNextAddress());

				} else if (e.keyCode === 38) { // Up

					markCurrentAddress(controller.getIndexToPreviousAddress());

				} else {

					if (addressJustSelected) {
						addressJustSelected = false;
						return false;
					}

					controller.resetCurrentIndex();
					controller.lookupAddress();
					scope.$apply();
				}
			});

			element.off('keydown').on('keydown', (e: JQueryEventObject) => {
				
				if (e.keyCode === 9 || e.keyCode === 13) { // Tab
					
					controller.selectCurrentAddresss();
					scope.$apply();

					addressJustSelected = true;
				}
			});
		}

		var directive = <IAddressAutocompleteDirective> {
			restrict: 'E',
			scope: {
				searchQuery: "=ngModel",
				placeholder: "@",
				addressSelected: "&"
			},
			replace: true,
			bindToController: true,
			templateUrl: '/Application/AddressAutocompleteComponent.html',
			controller: ['$http', '$timeout', Boligf.AddressAutocompleteComponent],
			controllerAs: "addressAutocompleteCtrl",
			link: link
		}

		return directive;
	}

	Boligf.App.directive("boligfAddressAutocomplete", [Boligf.addressAutocompleteDirective]);
} 