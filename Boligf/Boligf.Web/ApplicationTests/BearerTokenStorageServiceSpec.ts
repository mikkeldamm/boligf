describe("BearerTokenStorageService", () => {

	var cookieStoreServiceMock: ng.cookies.ICookieStoreService;
	var bearerTokenStorageService: Boligf.IStoreBearerToken;

	beforeEach(() => {

		cookieStoreServiceMock = <ng.cookies.ICookieStoreService> {
			get: (key:string) => {},
			put: (key: string): void => {},
			remove: (key: string):void => {}
		}

		spyOn(cookieStoreServiceMock, "get").and.returnValue("mikkel");
		spyOn(cookieStoreServiceMock, "put");
		spyOn(cookieStoreServiceMock, "remove");
		
		bearerTokenStorageService = new Boligf.BearerTokenStorageService(cookieStoreServiceMock);

	});

	describe("Token is set",() => {

		it("should return token when getting",() => {

			bearerTokenStorageService.token = "mikkel";
			expect(bearerTokenStorageService.token).toBe("mikkel");

			expect(cookieStoreServiceMock.get).toHaveBeenCalled();
		});

		it("should return true for containing the token", () => {

			bearerTokenStorageService.token = "mikkel";
			expect(bearerTokenStorageService.anyToken()).toBe(true);

			expect(cookieStoreServiceMock.put).toHaveBeenCalled();
		});

		it("should be able to remove it", () => {

			bearerTokenStorageService.token = "mikkel";
			bearerTokenStorageService.deleteToken();

			expect(cookieStoreServiceMock.remove).toHaveBeenCalled();
		});
	});

	describe("Token not set",() => {

		it("should return false for containing the token",() => {

			cookieStoreServiceMock.get = () => { return null; };
			expect(bearerTokenStorageService.anyToken()).toBe(false);
		});
	});
})