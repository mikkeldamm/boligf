module Boligf {

	export interface IRegisterAssociation {

		name: string;
		addressId: string;
		streetAddress: string;
		no: string;
		floor: string;
		door: string;
		zip: string;
		city: string;
		userId: string;
	}

	export interface IAssociationService {

		post(model: IRegisterAssociation): ng.IPromise<string>;
	}

	export class AssociationService implements IAssociationService {

		static $inject = ['IApiService'];
		
		constructor(
			private apiService: IApiService
			) {
			
		}

		post(model: IRegisterAssociation): ng.IPromise<string> {

			return this.apiService.post<string>("/association", model);
		}
	}

	Boligf.App.service("IAssociationService", Boligf.AssociationService);
} 