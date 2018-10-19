app.controller('IPInfoCtrl', ['$scope', 'IPInfoService',
    // we inject StudentService  inject becuse we call getAll method for get all student  
function ($scope, IPInfoService) {
    // this is base url   
    var baseUrl = '/api/IPInfo/';
    // get all student from databse  
    $scope.getIPInfo=function()  
    {  
        var apiRoute = baseUrl + 'GetIPInfo/';
        var _student = IPInfoService.getAll(apiRoute);
        console.log("_student: " + _student);
        _student.then(function (response) {  
            $scope.IPInfoData = response.data;  
        },  
        function (error) {  
            console.log("Error: " + error);
        });  
      
    }  
    $scope.getIPInfo();
      
}]);  

