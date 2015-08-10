module Boligf {

	export interface IAssociationAddressCode extends IAssociationAddress {
		associationId: string;
		codes: string[];
	}

	export interface IRegisterUserToAddress {
		userId: string;
		associationId: string;
		associationName: string;
		addressId: string;
		addressText: string;
	}

	export interface IAssociationAddressService {
		getCode(code: string): ng.IPromise<IAssociationAddressCode>;
		post(model: IRegisterUserToAddress): ng.IPromise<string>;
	}

	export class AssociationAddressService implements IAssociationAddressService {

		static $inject = ['IApiService'];
		
		private associationId: string;

		constructor(
			private apiService: IApiService
			) {
			
		}

		getCode(code: string): ng.IPromise<IAssociationAddressCode> {
			
			return this.apiService.get<IAssociationAddressCode>("/association/address/code/" + code);
		}

		post(model: IRegisterUserToAddress): ng.IPromise<string> {

			return this.apiService.post<string>("/association/" + model.associationId + "/address/" + model.addressId + "/user", model);
		}
	}

	Boligf.App.service("IAssociationAddressService", Boligf.AssociationAddressService);
} 