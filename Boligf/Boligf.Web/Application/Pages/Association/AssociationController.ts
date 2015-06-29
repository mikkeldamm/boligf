module Boligf {
	
	export class AssociationController {

		static $inject = ['IAssociationMemberService'];
		
		constructor(
			private associationMemberService: IAssociationMemberService
			) {

			associationMemberService.setup("456243ef-91eb-4442-8fb9-cf2680582cc8");

			associationMemberService.getAll().then((members: IAssociationMember[]) => {
				console.log(members);
			});

			associationMemberService.getSingle("fd76fe33-e015-420a-92c4-41da4b6a6b9e").then((member: IAssociationMember) => {
				console.log(member);
			});
		}
	}

	Boligf.App.controller("AssociationController", Boligf.AssociationController);
}