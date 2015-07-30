var Boligf;
(function (Boligf) {
    var States;
    (function (States) {
        var Default = (function () {
            function Default() {
            }
            Default.Home = "home";
            Default.News = "news";
            Default.Documents = "documents";
            Default.Board = "board";
            Default.Residents = "residents";
            return Default;
        })();
        States.Default = Default;
        var Authentication = (function () {
            function Authentication() {
            }
            Authentication.Base = "authentication";
            Authentication.Login = "authentication.login";
            Authentication.Logout = "authentication.logout";
            return Authentication;
        })();
        States.Authentication = Authentication;
        var Association = (function () {
            function Association() {
            }
            Association.Base = "association";
            Association.Register = "association.register";
            Association.RegisterMember = "association.registermember";
            return Association;
        })();
        States.Association = Association;
        var Errors = (function () {
            function Errors() {
            }
            Errors.E404 = "404";
            Errors.E403 = "403";
            return Errors;
        })();
        States.Errors = Errors;
    })(States = Boligf.States || (Boligf.States = {}));
})(Boligf || (Boligf = {}));
var Boligf;
(function (Boligf) {
    Boligf.App;
    var Startup = (function () {
        function Startup() {
            Boligf.App = angular.module('BoligfApp', [
                'ui.router',
                'ngResource',
                'ngCookies',
                'pascalprecht.translate'
            ]);
        }
        Startup.prototype.Run = function () {
            this.setupConfiguration();
        };
        Startup.prototype.setupConfiguration = function () {
            var _this = this;
            var providerInjects = [
                '$stateProvider',
                '$urlRouterProvider',
                '$httpProvider',
                '$translateProvider',
                '$locationProvider'
            ];
            var configFunc = function (stateProvider, urlRouterProvider, httpProvider, translateProvider, locationProvider) {
                _this.setupTranslations(translateProvider);
                _this.setupDefaultRouting(urlRouterProvider, locationProvider);
                _this.setupStates(stateProvider);
                _this.setupHttpInterceptors(httpProvider);
            };
            providerInjects.push(configFunc);
            Boligf.App.config(providerInjects);
        };
        Startup.prototype.setupTranslations = function (translateProvider) {
            translateProvider.useStaticFilesLoader({
                prefix: '/Locale/locale-',
                suffix: '.json'
            });
            translateProvider.preferredLanguage('en');
        };
        Startup.prototype.setupDefaultRouting = function (urlRouterProvider, locationProvider) {
            urlRouterProvider.otherwise("/");
            urlRouterProvider.when("/association/registermember", "/association/registermember/");
            //locationProvider.html5Mode(true);
        };
        Startup.prototype.setupStates = function (stateProvider) {
            stateProvider.state(Boligf.States.Default.Home, {
                url: '/',
                templateUrl: "/Application/Pages/Home/Home.html"
            }).state(Boligf.States.Default.News, {
                url: '/news',
                templateUrl: "/Application/Pages/"
            }).state(Boligf.States.Default.Documents, {
                url: '/documents',
                templateUrl: "/Application/Pages/"
            }).state(Boligf.States.Default.Board, {
                url: '/board',
                templateUrl: "/Application/Pages/"
            }).state(Boligf.States.Default.Residents, {
                url: '/residents',
                templateUrl: "/Application/Pages/"
            }).state(Boligf.States.Authentication.Base, {
                url: '/authentication',
                templateUrl: "/Application/Pages/Authentication/Authentication.html",
                controller: "AuthenticationController",
                controllerAs: "authentication"
            }).state(Boligf.States.Authentication.Login, {
                url: '/login',
                templateUrl: "/Application/Pages/Authentication/Login.html",
                controller: "LoginController",
                controllerAs: "loginCtrl"
            }).state(Boligf.States.Association.Base, {
                url: '/association',
                templateUrl: "/Application/Pages/Association/Association.html",
                controller: "AssociationController",
                controllerAs: "associationCtrl"
            }).state(Boligf.States.Association.Register, {
                url: '/register',
                templateUrl: "/Application/Pages/Association/Register.html",
                controller: "Association_RegisterController",
                controllerAs: "registerCtrl"
            }).state(Boligf.States.Association.RegisterMember, {
                url: '/registermember/:code',
                templateUrl: "/Application/Pages/Association/Member/RegisterMember.html",
                controller: "Association_RegisterMemberController",
                controllerAs: "registerMemberCtrl"
            }).state(Boligf.States.Errors.E404, {
                url: '/404',
                template: "<span>404 page</span>"
            }).state(Boligf.States.Errors.E403, {
                url: '/403',
                template: "<span>403 page</span>"
            });
        };
        Startup.prototype.setupHttpInterceptors = function (httpProvider) {
            httpProvider.interceptors.push("IInterceptHttpProvider");
        };
        Startup.Initialize = function () {
            return new Startup();
        };
        return Startup;
    })();
    Boligf.Startup = Startup;
})(Boligf || (Boligf = {}));
// TODO: Find better way to do this
var app = Boligf.Startup.Initialize();
var Boligf;
(function (Boligf) {
    var BearerTokenStorageService = (function () {
        function BearerTokenStorageService($cookies) {
            this.$cookies = $cookies;
        }
        Object.defineProperty(BearerTokenStorageService.prototype, "token", {
            get: function () {
                return this.$cookies.get(BearerTokenStorageService.tokenKey);
            },
            set: function (value) {
                this.$cookies.put(BearerTokenStorageService.tokenKey, value);
            },
            enumerable: true,
            configurable: true
        });
        BearerTokenStorageService.prototype.anyToken = function () {
            if (this.$cookies.get(BearerTokenStorageService.tokenKey)) {
                return true;
            }
            else {
                return false;
            }
        };
        BearerTokenStorageService.prototype.deleteToken = function () {
            this.$cookies.remove(BearerTokenStorageService.tokenKey);
        };
        BearerTokenStorageService.tokenKey = "auth_token";
        BearerTokenStorageService.$inject = ['$cookieStore'];
        return BearerTokenStorageService;
    })();
    Boligf.BearerTokenStorageService = BearerTokenStorageService;
    Boligf.App.service('IStoreBearerToken', Boligf.BearerTokenStorageService);
})(Boligf || (Boligf = {}));
var Boligf;
(function (Boligf) {
    var BearerTokenInterceptor = (function () {
        function BearerTokenInterceptor($q, bearerTokenStorageService) {
            var _this = this;
            this.$q = $q;
            this.bearerTokenStorageService = bearerTokenStorageService;
            this.request = function (config) {
                config.headers = config.headers || {};
                if (_this.bearerTokenStorageService.anyToken()) {
                    config.headers.Authorization = 'Bearer ' + _this.bearerTokenStorageService.token;
                    config.headers.ContentType = 'application/x-www-form-urlencoded';
                }
                return config;
            };
            this.requestError = function (rejection) {
                return _this.$q.reject(rejection);
            };
            this.responseError = function (rejection) {
                if (rejection != null && rejection.status === 401 && _this.bearerTokenStorageService.anyToken()) {
                    _this.bearerTokenStorageService.deleteToken();
                }
                return _this.$q.reject(rejection);
            };
        }
        BearerTokenInterceptor.$inject = ['$q', 'IStoreBearerToken'];
        return BearerTokenInterceptor;
    })();
    Boligf.BearerTokenInterceptor = BearerTokenInterceptor;
    Boligf.App.service('IInterceptHttpProvider', Boligf.BearerTokenInterceptor);
})(Boligf || (Boligf = {}));
var Boligf;
(function (Boligf) {
    var AuthenticationService = (function () {
        function AuthenticationService($http, $q, bearerTokenStore, userDataStore, associationDataStore) {
            this.$http = $http;
            this.$q = $q;
            this.bearerTokenStore = bearerTokenStore;
            this.userDataStore = userDataStore;
            this.associationDataStore = associationDataStore;
            if (this.bearerTokenStore.anyToken()) {
                this._isAuthenticated = true;
            }
            else {
                this._isAuthenticated = false;
            }
        }
        Object.defineProperty(AuthenticationService.prototype, "isAuthenticated", {
            get: function () {
                return this._isAuthenticated;
            },
            enumerable: true,
            configurable: true
        });
        AuthenticationService.prototype.login = function (email, password) {
            var _this = this;
            var defer = this.$q.defer();
            var data = "grant_type=password&username=" + email + "&password=" + password;
            this.$http.post(Boligf.Config.ApiAccess(true) + '/token', data).success(function (response) {
                _this.bearerTokenStore.token = response.access_token;
                _this.userDataStore.userId = response.userId;
                _this.userDataStore.userName = response.userName;
                _this.associationDataStore.associationId = response.associationId;
                _this._isAuthenticated = true;
                defer.resolve(true);
            }).error(function (err, status) {
                defer.resolve(false);
            });
            return defer.promise;
        };
        AuthenticationService.$inject = ['$http', '$q', 'IStoreBearerToken', 'IStoreUserData', 'IStoreAssociationData'];
        return AuthenticationService;
    })();
    Boligf.AuthenticationService = AuthenticationService;
    Boligf.App.service("IAuthenticationService", Boligf.AuthenticationService);
})(Boligf || (Boligf = {}));
var Boligf;
(function (Boligf) {
    function isAuthorizedDirective(authenticationService) {
        var directive = {
            restrict: 'A',
            scope: {
                isProtected: "=boligfIsAuthorized"
            },
            link: link
        };
        function link(scope, element, attributes) {
            scope.$watch(function () {
                return authenticationService.isAuthenticated;
            }, function (newValue, oldValue) {
                if (newValue) {
                    if (scope.isProtected) {
                        element.show();
                    }
                    else {
                        element.hide();
                    }
                }
                else {
                    if (!scope.isProtected) {
                        element.show();
                    }
                    else {
                        element.hide();
                    }
                }
            });
        }
        return directive;
    }
    Boligf.isAuthorizedDirective = isAuthorizedDirective;
    Boligf.App.directive("boligfIsAuthorized", ['IAuthenticationService', Boligf.isAuthorizedDirective]);
})(Boligf || (Boligf = {}));
// Typings 
/// <reference path="Scripts/typings/angularjs/angular.d.ts"/>
/// <reference path="Scripts/typings/angularjs/angular-route.d.ts"/>
/// <reference path="Scripts/typings/angularjs/angular-resource.d.ts"/>
/// <reference path="Scripts/typings/angularjs/angular-cookies.d.ts"/>
/// <reference path="Scripts/typings/angularjs/angular-mocks.d.ts"/>
/// <reference path="Scripts/typings/angularjs/angular-sanitize.d.ts"/>
/// <reference path="Scripts/typings/angularjs/angular-animate.d.ts"/>
/// <reference path="Scripts/typings/angularjs/angular-translate.d.ts"/>
/// <reference path="Scripts/typings/angularjs/angular-ui-router.d.ts"/>
/// <reference path="Scripts/typings/jquery/jquery.d.ts"/>
// Application statics
/// <reference path="Application/States.ts"/>
// Application uses
/// <reference path="Application/BoligfApp.ts"/>
/// <reference path="Application/BearerTokenStorageService.ts"/>
/// <reference path="Application/BearerTokenInterceptor.ts"/>
/// <reference path="Application/Pages/Authentication/AuthenticationService.ts"/>
/// <reference path="Application/Pages/Authentication/IsAuthorizedDirective.ts"/> 
var Boligf;
(function (Boligf) {
    var ApiService = (function () {
        function ApiService($http, $q) {
            this.$http = $http;
            this.$q = $q;
        }
        ApiService.prototype.get = function (url, config) {
            var defer = this.$q.defer();
            this.$http.get(Boligf.Config.ApiAccess() + url).then(function (response) {
                defer.resolve(response.data);
            }).catch(defer.reject);
            return defer.promise;
        };
        ApiService.prototype.delete = function (url, config) {
            var defer = this.$q.defer();
            this.$http.delete(Boligf.Config.ApiAccess() + url).then(function (response) {
                defer.resolve(response.data);
            }).catch(defer.reject);
            return defer.promise;
        };
        ApiService.prototype.post = function (url, data, config) {
            var defer = this.$q.defer();
            this.$http.post(Boligf.Config.ApiAccess() + url, data).then(function (response) {
                defer.resolve(response.data);
            }).catch(defer.reject);
            return defer.promise;
        };
        ApiService.prototype.put = function (url, data, config) {
            var defer = this.$q.defer();
            this.$http.put(Boligf.Config.ApiAccess() + url, data).then(function (response) {
                defer.resolve(response.data);
            }).catch(defer.reject);
            return defer.promise;
        };
        ApiService.$inject = ['$http', '$q'];
        return ApiService;
    })();
    Boligf.ApiService = ApiService;
    Boligf.App.service("IApiService", Boligf.ApiService);
})(Boligf || (Boligf = {}));
var Boligf;
(function (Boligf) {
    var AssociationAddressService = (function () {
        function AssociationAddressService(apiService) {
            this.apiService = apiService;
        }
        AssociationAddressService.prototype.getCode = function (code) {
            return this.apiService.get("/association/address/code/" + code);
        };
        AssociationAddressService.prototype.post = function (model) {
            return this.apiService.post("/association/" + model.associationId + "/address/" + model.addressId + "/user", model);
        };
        AssociationAddressService.$inject = ['IApiService'];
        return AssociationAddressService;
    })();
    Boligf.AssociationAddressService = AssociationAddressService;
    Boligf.App.service("IAssociationAddressService", Boligf.AssociationAddressService);
})(Boligf || (Boligf = {}));
var Boligf;
(function (Boligf) {
    var AssociationController = (function () {
        function AssociationController(associationMemberService) {
            this.associationMemberService = associationMemberService;
            associationMemberService.setup("456243ef-91eb-4442-8fb9-cf2680582cc8");
            associationMemberService.getAll().then(function (members) {
                console.log(members);
            });
            associationMemberService.getSingle("fd76fe33-e015-420a-92c4-41da4b6a6b9e").then(function (member) {
                console.log(member);
            });
        }
        AssociationController.$inject = ['IAssociationMemberService'];
        return AssociationController;
    })();
    Boligf.AssociationController = AssociationController;
    Boligf.App.controller("AssociationController", Boligf.AssociationController);
})(Boligf || (Boligf = {}));
var Boligf;
(function (Boligf) {
    var AssociationMemberService = (function () {
        function AssociationMemberService(apiService) {
            this.apiService = apiService;
        }
        AssociationMemberService.prototype.setup = function (associationId) {
            this.associationId = associationId;
            return this;
        };
        AssociationMemberService.prototype.getAll = function () {
            this.ensureSetup();
            return this.apiService.get(this.getUrlString(this.associationId));
        };
        AssociationMemberService.prototype.getSingle = function (userId) {
            this.ensureSetup();
            return this.apiService.get(this.getUrlString(this.associationId, userId));
        };
        AssociationMemberService.prototype.ensureSetup = function () {
            if (!this.associationId) {
                throw new Error("setup is not called with 'associationId', before REST calls");
            }
        };
        AssociationMemberService.prototype.getUrlString = function (associationId, userId) {
            return "/association/" + associationId + "/user/" + (userId ? userId : "");
        };
        AssociationMemberService.$inject = ['IApiService'];
        return AssociationMemberService;
    })();
    Boligf.AssociationMemberService = AssociationMemberService;
    Boligf.App.service("IAssociationMemberService", Boligf.AssociationMemberService);
})(Boligf || (Boligf = {}));
var Boligf;
(function (Boligf) {
    var AssociationService = (function () {
        function AssociationService(apiService) {
            this.apiService = apiService;
        }
        AssociationService.prototype.post = function (model) {
            return this.apiService.post("/association", model);
        };
        AssociationService.$inject = ['IApiService'];
        return AssociationService;
    })();
    Boligf.AssociationService = AssociationService;
    Boligf.App.service("IAssociationService", Boligf.AssociationService);
})(Boligf || (Boligf = {}));
var Boligf;
(function (Boligf) {
    var AssociationStorageService = (function () {
        function AssociationStorageService($cookies) {
            this.$cookies = $cookies;
        }
        Object.defineProperty(AssociationStorageService.prototype, "associationId", {
            get: function () {
                return this.$cookies.get("associationId");
            },
            set: function (value) {
                this.$cookies.put("associationId", value);
            },
            enumerable: true,
            configurable: true
        });
        AssociationStorageService.prototype.clear = function () {
            this.$cookies.remove("associationId");
        };
        AssociationStorageService.$inject = ['$cookieStore'];
        return AssociationStorageService;
    })();
    Boligf.AssociationStorageService = AssociationStorageService;
    Boligf.App.service('IStoreAssociationData', Boligf.AssociationStorageService);
})(Boligf || (Boligf = {}));
var Boligf;
(function (Boligf) {
    var MemberLoginInfoComponent = (function () {
        function MemberLoginInfoComponent(associationMemberService, userDataStore, associationDataStore) {
            this.associationMemberService = associationMemberService;
            this.userDataStore = userDataStore;
            this.associationDataStore = associationDataStore;
        }
        Object.defineProperty(MemberLoginInfoComponent.prototype, "isFloorAndDoorAvailable", {
            get: function () {
                if (this.no && this.floor) {
                    return true;
                }
                return false;
            },
            enumerable: true,
            configurable: true
        });
        MemberLoginInfoComponent.prototype.fillMemberInfo = function () {
            var _this = this;
            if (this.associationDataStore.associationId) {
                this.associationMemberService.setup(this.associationDataStore.associationId).getSingle(this.userDataStore.userId).then(function (member) {
                    _this.streetName = member.address.streetAddress;
                    _this.no = member.address.no;
                    _this.floor = member.address.floor;
                    _this.door = member.address.door;
                    _this.ready = true;
                });
            }
        };
        MemberLoginInfoComponent.$inject = ['IAssociationMemberService', 'IStoreUserData', 'IStoreAssociationData'];
        return MemberLoginInfoComponent;
    })();
    Boligf.MemberLoginInfoComponent = MemberLoginInfoComponent;
    function memberLoginInfoDirective(authenticationService) {
        function link(scope, element, attributes, controller) {
            scope.$watch(function () {
                return authenticationService.isAuthenticated;
            }, function (newValue, oldValue) {
                if (newValue) {
                    controller.fillMemberInfo();
                }
            });
        }
        var directive = {
            restrict: 'AE',
            scope: {},
            replace: true,
            templateUrl: '/Application/Pages/Association/Member/MemberLoginInfoComponent.html',
            controller: ['IAssociationMemberService', 'IStoreUserData', 'IStoreAssociationData', Boligf.MemberLoginInfoComponent],
            controllerAs: "memberLoginInfoCtrl",
            link: link
        };
        return directive;
    }
    Boligf.memberLoginInfoDirective = memberLoginInfoDirective;
    Boligf.App.directive("boligfMemberLoginInfo", ['IAuthenticationService', Boligf.memberLoginInfoDirective]);
})(Boligf || (Boligf = {}));
var Boligf;
(function (Boligf) {
    var RegisterMemberController = (function () {
        function RegisterMemberController($state, $stateParams, userService, associationAddressService) {
            this.$state = $state;
            this.$stateParams = $stateParams;
            this.userService = userService;
            this.associationAddressService = associationAddressService;
            this.addressRegistered = {};
            this.registerCode = this.$stateParams["code"];
        }
        RegisterMemberController.prototype.getAddressByCode = function () {
            var _this = this;
            this.associationAddressService.getCode(this.registerCode).then(function (addresWithCode) {
                _this.addressRegistered.addressId = addresWithCode.id;
                _this.addressRegistered.associationId = addresWithCode.associationId;
            }).catch(function () {
            });
        };
        RegisterMemberController.prototype.register = function () {
            var _this = this;
            debugger;
            this.userService.post(this.user).then(function (userId) {
                _this.addressRegistered.userId = userId;
                _this.associationAddressService.post(_this.addressRegistered).then(function () {
                    _this.$state.go(Boligf.States.Default.Home);
                }).catch(function () {
                    _this.userService.delete(userId).then(function (isDeleted) {
                        if (isDeleted) {
                            console.log("user is deleted because association could not be created");
                            _this.$state.go(Boligf.States.Default.Home);
                        }
                    });
                });
            });
        };
        RegisterMemberController.$inject = ['$state', '$stateParams', 'IUserService', 'IAssociationAddressService'];
        return RegisterMemberController;
    })();
    Boligf.RegisterMemberController = RegisterMemberController;
    Boligf.App.controller("Association_RegisterMemberController", Boligf.RegisterMemberController);
})(Boligf || (Boligf = {}));
var Boligf;
(function (Boligf) {
    var RegisterController = (function () {
        function RegisterController($state, userService, associationService) {
            this.$state = $state;
            this.userService = userService;
            this.associationService = associationService;
        }
        RegisterController.prototype.register = function () {
            var _this = this;
            this.userService.post(this.user).then(function (userId) {
                _this.association.userId = userId;
                _this.associationService.post(_this.association).then(function (associationId) {
                    console.log(associationId);
                    _this.$state.go(Boligf.States.Default.Home);
                }).catch(function () {
                    _this.userService.delete(userId).then(function (isDeleted) {
                        if (isDeleted) {
                            console.log("user is deleted because association could not be created");
                            _this.$state.go(Boligf.States.Default.Home);
                        }
                    });
                });
            });
        };
        RegisterController.$inject = ['$state', 'IUserService', 'IAssociationService'];
        return RegisterController;
    })();
    Boligf.RegisterController = RegisterController;
    Boligf.App.controller("Association_RegisterController", Boligf.RegisterController);
})(Boligf || (Boligf = {}));
var Boligf;
(function (Boligf) {
    var AuthenticationController = (function () {
        function AuthenticationController($scope, $http, authenticationService, bearerTokenStore) {
            this.$scope = $scope;
            this.$http = $http;
            this.authenticationService = authenticationService;
            this.bearerTokenStore = bearerTokenStore;
            console.log("Auth ctrl init");
            this.lars = "mikkel";
        }
        AuthenticationController.prototype.testM = function () {
            //var token = this.authenticationService.login("mikkel@damm.dk", "1234");
            console.log("Click event");
            console.log("-- token: " + this.bearerTokenStore.token);
            this.$http.get('/api/authentication', {}).success(function (data) {
                console.log("test call");
                console.log(data);
            });
        };
        AuthenticationController.prototype.testM2 = function () {
            console.log("Click event");
            this.$http.get('/api/authentication/4', {}).success(function (data) {
                console.log("test call 2");
                console.log(data);
            });
        };
        AuthenticationController.$inject = ['$scope', '$http', 'IAuthenticationService', 'IStoreBearerToken'];
        return AuthenticationController;
    })();
    Boligf.AuthenticationController = AuthenticationController;
    Boligf.App.controller("AuthenticationController", Boligf.AuthenticationController);
})(Boligf || (Boligf = {}));
var Boligf;
(function (Boligf) {
    var LoginController = (function () {
        function LoginController($state, authenticationService) {
            this.$state = $state;
            this.authenticationService = authenticationService;
            // Redirect user to home if is logged in
            if (this.authenticationService.isAuthenticated) {
                this.$state.go(Boligf.States.Default.Home);
            }
        }
        LoginController.prototype.login = function () {
            var _this = this;
            this.authenticationService.login(this.email, this.password).then(function (isSuccedded) {
                if (isSuccedded) {
                    _this.$state.go(Boligf.States.Default.Home);
                }
                else {
                    // TODO: Instead of redirect on failure, then just show error for user
                    _this.$state.go(Boligf.States.Errors.E403);
                }
            });
        };
        LoginController.$inject = ['$state', 'IAuthenticationService'];
        return LoginController;
    })();
    Boligf.LoginController = LoginController;
    Boligf.App.controller("LoginController", Boligf.LoginController);
})(Boligf || (Boligf = {}));
var Boligf;
(function (Boligf) {
    var UserProfile = (function () {
        function UserProfile() {
        }
        return UserProfile;
    })();
    Boligf.UserProfile = UserProfile;
    var UserService = (function () {
        function UserService(apiService) {
            this.apiService = apiService;
        }
        UserService.prototype.post = function (model) {
            return this.apiService.post("/user", model);
        };
        UserService.prototype.delete = function (userId) {
            return this.apiService.delete("/user/" + userId);
        };
        UserService.$inject = ['IApiService'];
        return UserService;
    })();
    Boligf.UserService = UserService;
    Boligf.App.service("IUserService", Boligf.UserService);
})(Boligf || (Boligf = {}));
var Boligf;
(function (Boligf) {
    var UserStorageService = (function () {
        function UserStorageService($cookies) {
            this.$cookies = $cookies;
        }
        Object.defineProperty(UserStorageService.prototype, "userId", {
            get: function () {
                return this.$cookies.get("userId");
            },
            set: function (value) {
                this.$cookies.put("userId", value);
            },
            enumerable: true,
            configurable: true
        });
        Object.defineProperty(UserStorageService.prototype, "userName", {
            get: function () {
                return this.$cookies.get("userName");
            },
            set: function (value) {
                this.$cookies.put("userName", value);
            },
            enumerable: true,
            configurable: true
        });
        UserStorageService.prototype.clear = function () {
            this.$cookies.remove("userId");
            this.$cookies.remove("userName");
        };
        UserStorageService.$inject = ['$cookieStore'];
        return UserStorageService;
    })();
    Boligf.UserStorageService = UserStorageService;
    Boligf.App.service('IStoreUserData', Boligf.UserStorageService);
})(Boligf || (Boligf = {}));
var Boligf;
(function (Boligf) {
    var Tests;
    (function (Tests) {
        function describe(str, func) {
        }
        Tests.describe = describe;
        function it(str, func) {
        }
        Tests.it = it;
        function beforeEach(func) {
        }
        Tests.beforeEach = beforeEach;
        describe("BearerTokenStorageService", function () {
            var cookieStoreServiceMock;
            var bearerTokenStorageService;
            beforeEach(function () {
                cookieStoreServiceMock = {
                    get: function (key) {
                    },
                    put: function (key) {
                    },
                    remove: function (key) {
                    }
                };
                spyOn(cookieStoreServiceMock, "get").and.returnValue("mikkel");
                spyOn(cookieStoreServiceMock, "put");
                spyOn(cookieStoreServiceMock, "remove");
                bearerTokenStorageService = new Boligf.BearerTokenStorageService(cookieStoreServiceMock);
            });
            describe("Token is set", function () {
                it("should return token when getting", function () {
                    bearerTokenStorageService.token = "mikkel";
                    expect(bearerTokenStorageService.token).toBe("mikkel");
                    expect(cookieStoreServiceMock.get).toHaveBeenCalled();
                    expect(cookieStoreServiceMock.get).toHaveBeenCalledWith("auth_token");
                });
                it("should return true for containing the token", function () {
                    bearerTokenStorageService.token = "mikkel";
                    expect(bearerTokenStorageService.anyToken()).toBe(true);
                    expect(cookieStoreServiceMock.put).toHaveBeenCalled();
                });
                it("should be able to remove it", function () {
                    bearerTokenStorageService.token = "mikkel";
                    bearerTokenStorageService.deleteToken();
                    expect(cookieStoreServiceMock.remove).toHaveBeenCalled();
                });
            });
            describe("Token not set", function () {
                it("should return false for containing the token", function () {
                    cookieStoreServiceMock.get = function () {
                        return null;
                    };
                    expect(bearerTokenStorageService.anyToken()).toBe(false);
                });
            });
        });
    })(Tests = Boligf.Tests || (Boligf.Tests = {}));
})(Boligf || (Boligf = {}));
//# sourceMappingURL=app.js.map