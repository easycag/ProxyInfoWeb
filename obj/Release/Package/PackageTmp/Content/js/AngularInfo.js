app.service('IPInfoService', function ($http) {
    //**********----Get All Record----***************  
    var urlGet = '';
    this.getAll = function (apiRoute) {
        urlGet = apiRoute;
        return $http.get(urlGet);
    }
});