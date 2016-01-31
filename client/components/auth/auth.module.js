'use strict';

angular.module('comparehareApp.auth', [
  'comparehareApp.constants',
  'comparehareApp.util',
  'ngCookies',
  'ngRoute'
])
  .config(function($httpProvider) {
    $httpProvider.interceptors.push('authInterceptor');
  });
