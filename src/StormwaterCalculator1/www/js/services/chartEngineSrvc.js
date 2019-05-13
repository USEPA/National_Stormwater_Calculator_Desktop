
/**
 * @ngdoc overview
 * @name nscwebappApp.chartengine
 * @description service for charting computed cost estimates
 */
angular.module('nscwebappApp')
        .service('chartengine', function () {
            var defaultRoundExp = 2; //rounds costs to nearest 10^2 for charting
            // Public API here
            var service = {
                generateCharts: generateCharts
            };
            return service;
            ///////////////////////////

            /**
             * @ngdoc function
             * @name generateCharts
             * @methodOf nscwebappApp.chartengine
             * @description generates bar charts with error bars 
             * @param {object} costResults - computed cost results to generate charts for
             * @param {object} chartWidthHeight - chart dimensions
             * @returns {undefined}
             */
            function generateCharts(costResults, chartWidthHeight) {
                console.log('inside generateCharts chartWidthHeight->', chartWidthHeight);
                var chartSize = chartWidthHeight || {
                    width: 400,
                    height: 300
                };
                var chartTypes = {
                    bar: 'bar',
                    pie: 'pie',
                    donut: 'donut'
                };

                var chartData = formatDataForPlotlyChart(costResults);

                if (chartData.hasOwnProperty('baseScenario')) {
                    generatePlotlyChart('bc-bar', chartTypes.bar, chartData.baseScenario.capitalLables, chartData.baseScenario.capital, chartData.baseScenario.capitalErrors, chartSize);
                    generatePlotlyChart('bm-bar', chartTypes.bar, chartData.baseScenario.maintenanceLables, chartData.baseScenario.maintenance, chartData.baseScenario.maintenanceErrors, chartSize);
                    console.log(chartData.baseScenario.maintenanceErrors);
                }

                //4. generate current bar chart - maintenance cost
                if (chartData.hasOwnProperty('currentScenario')) {
                    generatePlotlyChart('cc-bar', chartTypes.bar, chartData.currentScenario.capitalLables, chartData.currentScenario.capital, chartData.currentScenario.capitalErrors, chartSize);
                    generatePlotlyChart('cm-bar', chartTypes.bar, chartData.currentScenario.maintenanceLables, chartData.currentScenario.maintenance, chartData.currentScenario.maintenanceErrors, chartSize);
                    console.log(chartData.currentScenario.maintenanceErrors);
                }

            }

            /**
             * @ngdoc function
             * @name roundupForCharts
             * @methodOf nscwebappApp.chartengine
             * @description rounds numbers up 
             * @param {number} num - number to be rounded
             * @param {number} exp - number of decimal places to round to
             * @returns {Number} - rounded result
             */
            function roundupForCharts(num, exp) {
                var denom = Math.pow(10, exp);
                return Math.ceil(num / denom) * denom;
            }

            /**
             * @ngdoc function
             * @name formatDataForPlotlyChart
             * @methodOf nscwebappApp.chartengine
             * @description puts computed results to be plotted in format 
             * suitable for the plotly charting library 
             * @param {object} costData - computed cost results to generate charts for
             * @returns {results}
             */
            function formatDataForPlotlyChart(costData) {
                var tempNum = 0;
                var results = {
                    base: {},
                    current: {}
                };
                var capitalLables = [];
                var capitalCostResults = [];
                var capitalErrors = [];

                var maintCostLables = [];
                var maintCostResults = [];
                var maintenanceErrors = [];

                var labelFormatsCap = {};
                var labelFormatsMaint = {};
                var err = 0;
                var i = 0;

                //1. iterate over sitedata.base and sitedata.current scenarios
                angular.forEach(costData, function (scenario, sdx) {
                    console.log('siteData', sdx, scenario);
                    capitalLables.length = 0;
                    capitalCostResults.length = 0;
                    capitalErrors.length = 0;

                    maintCostLables.length = 0;
                    maintCostResults.length = 0;
                    maintenanceErrors.length = 0;
                    labelFormatsCap.length = 0;
                    labelFormatsMaint.length = 0;
                    //2. iterate over each lid control in the scenario
                    angular.forEach(scenario.costvars, function (lid, ldx) {
                        if (lid.footprintAreaSqFt !== 0) {
                            err = parseFloat(lid.capCostHigh) - parseFloat(lid.capCostLow);
                            tempNum = (parseFloat(lid.capCostLow) + parseFloat(lid.capCostHigh)) / 2;
                            tempNum = roundupForCharts(tempNum, defaultRoundExp);
                            capitalLables.push(lid.id.toUpperCase()); //keep capital and maintenance labels seperate in case they diverge in future updates
                            capitalCostResults.push(tempNum);
                            capitalErrors.push(roundupForCharts((err / 2), defaultRoundExp));

                            err = parseFloat(lid.maintCostHigh) - parseFloat(lid.maintCostLow);
                            //console.log('maint err:', sdx, ':', ldx, ' | ', err);
                            tempNum = (parseFloat(lid.maintCostLow) + parseFloat(lid.maintCostHigh)) / 2;
                            tempNum = roundupForCharts(tempNum, defaultRoundExp);
                            maintCostLables.push(lid.id.toUpperCase()); //keep capital and maintenance labels seperate in case they diverge in future updates
                            maintCostResults.push(tempNum);
                            maintenanceErrors.push(roundupForCharts((err / 2), defaultRoundExp));

                            labelFormatsCap[i] = d3.format('$');
                            labelFormatsMaint[i] = d3.format('$');
                            i = i + 1;
                        }
                    });
                    results[sdx] = {
                        capital: angular.copy(capitalCostResults),
                        capitalErrors: angular.copy(capitalErrors),
                        capitalLables: angular.copy(capitalLables),
                        maintenance: angular.copy(maintCostResults),
                        maintenanceErrors: angular.copy(maintenanceErrors),
                        maintenanceLables: angular.copy(maintCostLables),
                        labelFormatsCap: angular.copy(labelFormatsCap),
                        labelFormatsMaint: angular.copy(labelFormatsMaint)
                    };
                });
                return results;
            }

            /**
             * @ngdoc function
             * @name generatePlotlyChart
             * @methodOf nscwebappApp.chartengine
             * @description rounds numbers up 
             * @param {string} container - DOM element idenfier for chart container
             * @param {string} chartType - type of chart to create
             * @param {array} xaxisLabelsArr - x-axis lables
             * @param {array} yaxisDataArr - y-axis data
             * @param {object} chartErrors - data for error bars
             * @param {object} chartsize - chart size
             * @returns {chart}
             */
            function generatePlotlyChart(container, chartType, xaxisLabelsArr, yaxisDataArr, chartErrors, chartsize) {
                var toBePloted = [];
                var colors = ['#fee391', '#edf8b1', '#c7e9b4', '#7fcdbb', '#41b6c4', '#1d91c0', '#225ea8', '#4C6C84', '#0c2c84'];
                var chartContainerEl = container || '#chart';
                var defaultTrace = {
                    name: 'Control',
                    marker: {
                        color: 'rgb(55, 83, 109)',
                        opacity: 0.6,
                        line: {
                            //color: 'rbg(8,48,107)',
                            color: 'rbga(255,255,255,0.5)',
                            width: 1.5
                        }
                    },
                    error_y: {
                        type: 'data',
                        visible: true
                    },
                    type: chartType
                };
                var tempTrace;
                var layout = {
                    //title: 'US Export of Plastic Scrap',
                    xaxis: {
                        tickfont: {
                            size: 10,
                            color: 'rgb(107, 107, 107)'
                        },
                        fixedrange: true //disables zoom
                    },
                    yaxis: {
                        fixedrange: true, //disables zoom
                        title: 'USD',
                        titlefont: {
                            size: 10,
                            color: 'rgb(107, 107, 107)'
                        },
                        tickfont: {
                            size: 12,
                            color: 'rgb(107, 107, 107)'
                        },
                        hoverformat: ",f"
                    },
                    legend: {
                        //x: 0,
                        //y: 1.0,
                        bgcolor: 'rgba(255, 255, 255, 0.2)',
                        bordercolor: 'rgba(255, 255, 255, 0)'
                    },
                    barmode: 'group',
                    bargap: 0.15,
                    bargroupgap: 0.1,
                    paper_bgcolor: 'rgba(255,255,255,0)',
                    plot_bgcolor: 'rgba(255,255,255,0)',
                    autosize: false,
                    width: chartsize.width,
                    height: chartsize.height,
                    margin: {
                        l: 50,
                        r: 50,
                        b: 30,
                        t: 20,
                        pad: 4
                    }
                };
                //names of buttons at https://github.com/plotly/plotly.js/blob/master/src/components/modebar/buttons.js
                var modebar = {
                    displayModeBar: false,
                    displaylogo: false,
                    showTips: true,
                    modeBarButtonsToRemove: ["sendDataToCloud", "resetScale2d",
                        "zoomIn2d", "zoomOut2d", "select2d", "lasso2d"]
                };
                for (var i = 0; i < yaxisDataArr.length; i++) {
                    tempTrace = angular.merge({}, defaultTrace, {
                        x: [xaxisLabelsArr[i]],
                        y: [yaxisDataArr[i]],
                        name: xaxisLabelsArr[i],
                        marker: { color: colors[i] },
                        error_y: {
                            array: [chartErrors[i]]
                        }
                    });
                    toBePloted.push(tempTrace);
                }
                var chart = Plotly.newPlot(chartContainerEl, toBePloted, layout, modebar);
                return chart;
            }
        });