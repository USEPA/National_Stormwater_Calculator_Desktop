/**
 * @ngdoc overview
 * @name nscwebappApp
 * @description
 * # nscwebappApp - EPA Stormwater Calculator Cost Module
 *
 * Main and only controller for the application.
 */
angular.module('nscwebappApp')
        .controller('MainCtrl', function ($scope, $timeout, costEngineSrvc, chartengine) {
            var vm = this;
            var chartSize = { width: 350, height: 250 }; //holds chart width and height passed in from C#
            var siteData = {//holds lid cost estimation variables passed in from C#
                baseScenario: {}, //for baseline scenario
                currentScenario: {}//for current scenario
            };

            siteData.baseScenario.costvars = [
                { id: 'id', name: 'Disconnection', type: 0, footprintAreaSqFt: 0, totSiteArea: 0, percentDrainArea: 0, hasPretreatment: false },
                { id: 'rh', name: 'Rainwater Harvesting', type: 1, footprintAreaSqFt: 0, totSiteArea: 0, percentDrainArea: 0, hasPretreatment: false },
                { id: 'rg', name: 'Rain Gardens', type: 2, footprintAreaSqFt: 0, totSiteArea: 0, percentDrainArea: 0, hasPretreatment: false },
                { id: 'gr', name: 'Green Roofs', type: 3, footprintAreaSqFt: 0, totSiteArea: 0, percentDrainArea: 0, hasPretreatment: false },
                { id: 'sp', name: 'Street Planters', type: 4, footprintAreaSqFt: 0, totSiteArea: 0, percentDrainArea: 0, hasPretreatment: false },
                { id: 'ib', name: 'Infiltration Basins', type: 5, footprintAreaSqFt: 0, totSiteArea: 0, percentDrainArea: 0, hasPretreatment: false },
                { id: 'pp', name: 'Permeable Pavement', type: 6, footprintAreaSqFt: 0, totSiteArea: 0, percentDrainArea: 0, hasPretreatment: false }
            ];

            siteData.currentScenario.chartSize = chartSize;
            siteData.currentScenario.costvars = [
                { id: 'id', name: 'Disconnection', type: 0, footprintAreaSqFt: 0, totSiteArea: 0, percentDrainArea: 0, hasPretreatment: false },
                { id: 'rh', name: 'Rainwater Harvesting', type: 1, footprintAreaSqFt: 0, totSiteArea: 0, percentDrainArea: 0, hasPretreatment: false },
                { id: 'rg', name: 'Rain Gardens', type: 2, footprintAreaSqFt: 0, totSiteArea: 0, percentDrainArea: 0, hasPretreatment: false },
                { id: 'gr', name: 'Green Roofs', type: 3, footprintAreaSqFt: 0, totSiteArea: 0, percentDrainArea: 0, hasPretreatment: false },
                { id: 'sp', name: 'Street Planters', type: 4, footprintAreaSqFt: 0, totSiteArea: 0, percentDrainArea: 0, hasPretreatment: false },
                { id: 'ib', name: 'Infiltration Basins', type: 5, footprintAreaSqFt: 0, totSiteArea: 0, percentDrainArea: 0, hasPretreatment: false },
                { id: 'pp', name: 'Permeable Pavement', type: 6, footprintAreaSqFt: 0, totSiteArea: 0, percentDrainArea: 0, hasPretreatment: false }
            ];

            siteData.baseScenario.complexity = {
                isNewDevelopment: true,
                isReDevelopment: false,
                hasPretreatment: true,
                siteSuitability: 2,
                topography: 2,
                soilType: 1
            };
            siteData.currentScenario.complexity = {
                isNewDevelopment: false,
                isReDevelopment: true,
                hasPretreatment: true,
                siteSuitability: 1,
                topography: 3,
                soilType: 0
            };

            var lidKey = [
                { id: 'id', name: 'Disconnection' },
                { id: 'rh', name: 'Rainwater Harvesting' },
                { id: 'rg', name: 'Rain Gardens' },
                { id: 'gr', name: 'Green Roofs' },
                { id: 'sp', name: 'Street Planters' },
                { id: 'ib', name: 'Infiltration Basins' },
                { id: 'pp', name: 'Permeable Pavement' },
                { id: 'VS', name: 'Vegetated Swales' }
            ];

            var capCostTitle = 'Estimate of Probable Capital Costs';
            var maintCostTitle = 'Estimate of Probable Maintenance Costs';

            vm.hasBaseline = false;
            vm.pageTitle = capCostTitle;
            vm.pageSubTitle = '';
            vm.showTabular = true;
            vm.showGraphical = !vm.showTabular;
            vm.showCap = true;
            vm.showMaint = !vm.showCap;
            vm.showCapitalGraphical = true;
            vm.showCapitalTabular = true;
            vm.showMaintGraphical = true;
            vm.showMaintTabular = true;
            vm.estimatedResults = {};

            vm.chunkedResults = chunk(lidKey, 4);

            vm.toggleGraphicalView = toggleGraphicalView;
            vm.toggleCapMaintViews = toggleCapMaintViews;
            vm.resizeChart = resizeChart;
            //vm.updateSiteData = updateSiteData;
            $scope.updateSiteData = updateSiteData;
            $scope.resizeChart = resizeChart;

            /* Begin function definitions */
            /***************************/
            /**
             * @ngdoc function
             * @name computeLIDCtrlCosts
             * @methodOf nscwebappApp.MainCtrl
             * @param {type} siteData - object containing site data for both current and
             * base scenarios
             * @returns {estimatedResults} - object containing results for both current and base
             * scenarios
             */
            function computeLIDCtrlCosts(siteData) {
                console.log("inside computeLIDCtrlCosts");
                vm.estimatedResults = costEngineSrvc.computeLIDCtrlCosts(siteData);
                vm.totalsResults = vm.estimatedResults[vm.estimatedResults.length - 1];

                //read total site area from any of the lid controls
                if (siteData && siteData.hasOwnProperty('baseScenario') && siteData.baseScenario.hasOwnProperty('costvars') && siteData.baseScenario.costvars.gr !== undefined) {
                    vm.totSiteAreaBaseline = parseFloat(siteData.baseScenario.costvars.gr.totSiteArea);
                }
                if (siteData && siteData.hasOwnProperty('currentScenario') && siteData.currentScenario.hasOwnProperty('costvars') && siteData.currentScenario.costvars.gr !== undefined) {
                    vm.totSiteAreaCurrent = parseFloat(siteData.currentScenario.costvars.gr.totSiteArea);
                }
                vm.siteData = siteData;
                if (siteData && siteData.currentScenario && siteData.currentScenario.dataYear) {
                    vm.pageSubTitle = '(estimates in ' + siteData.currentScenario.dataYear + ' US.$)';
                }
                return vm.estimatedResults;
            }

            /**
             * @ngdoc function
             * @name chartCosts
             * @methodOf nscwebappApp.MainCtrl
             * @description creates a chart of the computed costs
             * @param {type} costResults - computed results
             * @param {type} chartSize - size of chart to create
             * @returns {undefined}
             */
            function chartCosts(costResults, chartSize) {
                chartengine.generateCharts(costResults, chartSize);
            }

            /**
             * @ngdoc function
             * @name toggleGraphicalView
             * @methodOf nscwebappApp.MainCtrl
             * @description toggles from tabular to graphical view and vice versa
             * @param {type} show - boolean, set to true to show tabular view 
             * and to false to show graphical view
             * @returns {undefined}
             */
            function toggleGraphicalView(show) {
                vm.showTabular = show;
                vm.showGraphical = !vm.showTabular;
            }

            /**
             * @ngdoc function
             * @name toggleCapMaintViews
             * @methodOf nscwebappApp.MainCtrl
             * @description toggles from captial to maintenance view and vice versa
             * @param {type} show - boolean, set to true to show capital cost  
             * and to false to show maintenance costs 
             * @returns {undefined}
             */
            function toggleCapMaintViews(show) {
                vm.showCap = show;
                vm.showMaint = !vm.showCap;
                if (show) {
                    vm.pageTitle = capCostTitle;
                } else {
                    vm.pageTitle = maintCostTitle;
                }
            }

            /**
             * @ngdoc function
             * @name chunk
             * @methodOf nscwebappApp.MainCtrl
             * @description breaks an array into chunks
             * @param {type} arr - array to be divided into chunks
             * @param {type} size - size of chunks desired
             * @returns {Array}
             */
            function chunk(arr, size) {
                var newArr = [];
                for (var i = 0; i < arr.length; i += size) {
                    newArr.push(arr.slice(i, i + size));
                }
                return newArr;
            }

            /**
             * @ngdoc function
             * @name updateSiteData
             * @methodOf nscwebappApp.MainCtrl
             * @description receives site data from C# and updates costs and charts
             * @returns {undefined}
             */
            function updateSiteData() {
                $timeout(function () {
                    if (window.browserLiason && window.browserLiason.jsObj) { //then side data passed in from .Net
                        if (window.browserLiason.jsObj !== null && typeof window.browserLiason.jsObj === 'object') {
                            siteData = window.browserLiason.jsObj;
                        } else {
                            console.log(window.browserLiason.jsObj);
                            siteData = JSON.parse(window.browserLiason.jsObj);
                        }
                    }
                    if (siteData !== undefined) {
                        if (siteData.hasOwnProperty('baseScenario') && siteData.baseScenario.hasOwnProperty('costvars') && siteData.baseScenario.costvars !== '') {
                            vm.hasBaseline = true;
                        } else {
                            vm.hasBaseline = false;
                        }
                    }
                    console.log('siteData received from C# inside updateSiteData', siteData);
                    computeLIDCtrlCosts(siteData);
                    chartCosts(siteData, siteData.currentScenario.chartSize);
                    sendResultsToHost(1);
                    return false;
                }, 0);
            }

            /**
             * @ngdoc function
             * @name sendResultsToHost
             * @methodOf nscwebappApp.MainCtrl
             * @description sends messages back to C# host 
             * @param {type} mode - type of message being sent
             * @returns {undefined}
             */
            function sendResultsToHost(mode) {
                console.log("inside sendResultsToHost");
                if (window.browserLiason !== undefined && window.browserLiason.sendToCSharp !== undefined) {
                    switch (mode) {
                        case 0:
                            //ask c# for site data
                            window.browserLiason.sendToCSharp('"want site data"', 2);
                            break;
                        case 1:
                            //send computed results to c#
                            window.browserLiason.sendToCSharp(JSON.stringify(vm.estimatedResults), 1);
                            break;
                        default:
                            //ask c# for site data
                            window.browserLiason.sendToCSharp('"want site data"', 2);
                    }
                }
            }

            /**
             * @ngdoc function
             * @name resizeChart
             * @methodOf nscwebappApp.MainCtrl
             * @description resizes charts when view told to do so
             * @returns {undefined}
             */
            function resizeChart() {
                chartCosts(siteData, window.browserLiason.chartSize);
            }

            /* Begin excution */
            /***************************/
            updateSiteData();
        })
        .filter('offset', function () { //custom filter used to slice array for better display
            return function (input, start) {
                start = parseInt(start, 10);
                return input.slice(start);
            };
        });