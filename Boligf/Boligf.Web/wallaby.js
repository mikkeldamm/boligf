module.exports = function (w) {

	return {
		files: [
			"Scripts/Libraries/jquery.js",
			"Scripts/Libraries/angular.js",
			"Scripts/Libraries/angular.mocks.js",
			"Scripts/Libraries/angular.resource.js",
			"Scripts/Libraries/angular.cookies.js",
			"Scripts/Libraries/angular.ui-router.min.js",
			"Scripts/Libraries/angular.translate.min.js",
			"Scripts/Libraries/angular.translate-loader.min.js",
			'Application/BoligfApp.ts',
			'Application/States.ts',
			'Application/BearerTokenStorageService.ts',
			'Application/BearerTokenInterceptor.ts',
			'Application/ApiService.ts',
			'Application/**/*.ts'
		],

		tests: [
		  'ApplicationTests/*Spec.ts'
		]
	};
};