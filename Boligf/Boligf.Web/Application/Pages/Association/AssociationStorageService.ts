module Boligf {

	export interface IStoreAssociationData {
		associationId: string;
		clear(): void;
	}

	export class AssociationStorageService implements IStoreAssociationData {

		static $inject = ['$cookieStore'];

		public get associationId() {
			return this.$cookies.get("associationId");
		}

		public set associationId(value) {
			this.$cookies.put("associationId", value);
		}
		
		constructor(private $cookies: ng.cookies.ICookieStoreService) {
			
		}
		
		public clear() {

			this.$cookies.remove("associationId");
		}
	}

	Boligf.App.service('IStoreAssociationData', Boligf.AssociationStorageService);
} 