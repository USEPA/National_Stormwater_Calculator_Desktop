'use strict';

/**
 * @ngdoc overview
 * @name nscwebappApp
 * @description
 * # nscwebappApp
 *
 * Main module of the application.
 */
angular.module('nscwebappApp', ['ngRoute'])
        .config(function ($routeProvider) {
            $routeProvider
                    .when('/', {
                        templateUrl: 'main.html',
                        controller: 'MainCtrl',
                        controllerAs: 'main'
                    })
                    .otherwise({
                        redirectTo: '/'
                    });
        });