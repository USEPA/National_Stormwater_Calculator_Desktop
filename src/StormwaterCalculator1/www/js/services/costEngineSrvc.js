
/**
 * @ngdoc overview
 * @name nscwebappApp.costEngineSrvc
 * @description service for computing cost estimates
 */
angular.module('nscwebappApp')
        .factory('costEngineSrvc', function (designComplexity) {

            var dataVersion = '0.0.1';
            var eqnsVersion = '0,0.1';
            var dataReferenceYear = 2014; //used for inflation calcs
            var defaultRoundDP = 2; // default number of decimal places to round to
            var costData = {
                id: {
                    "Capital": { "simpleEquation": "y = 0.2142x + 159.75", "simpleRSquared": "R² = 1", "simple_upperEquation": "y = 1.9321x + 1041.3", "simple_upperRSquared": "R² = 1", "typicalEquation": "y = 3.65x + 1922.8", "typicalRSquared": "R² = 1", "typical_upperEquation": "y = 4.6869x + 2864.7", "typical_upperRSquared": "R² = 1", "complexEquation": "y = 5.7238x + 3806.5", "complexRSquared": "R² = 1", "complex_upperEquation": "y = 6.7608x + 4748.3", "complex_upperRSquared": "R² = 1" },
                    "Maintenance": { "simpleEquation": "y = 0.05x + 1E-13", "simpleRSquared": "R² = 1", "complexEquation": "y = 0.0751x", "complexRSquared": "R² = 1" }
                },
                rh: {
                    "Capital": { "simpleEquation": "y = 0.3844x + 61.8", "simpleRSquared": "R² = 1", "simple_upperEquation": "y = 0.577x + 1812.9", "simple_upperRSquared": "R² = 1", "typicalEquation": "y = 0.7697x + 3564", "typicalRSquared": "R² = 1", "typical_upperEquation": "y = 1.0891x + 3957", "typical_upperRSquared": "R² = 1", "complexEquation": "y = 1.4085x + 4350", "complexRSquared": "R² = 1", "complex_upperEquation": "y = 1.728x + 4743", "complex_upperRSquared": "R² = 1" },
                    "Maintenance": { "simpleEquation": "y = 0.1003x + 0.0002", "simpleRSquared": "R² = 1", "complexEquation": "y = 0.2407x + 0.0006", "complexRSquared": "R² = 1" }
                },
                rg: {
                    "Capital": { "simpleEquation": "y = 0.2717x + 346.08", "simpleRSquared": "R² = 1", "simple_upperEquation": "y = 0.9204x + 2021", "simple_upperRSquared": "R² = 1", "typicalEquation": "y = 1.5691x + 3696", "typicalRSquared": "R² = 1", "typical_upperEquation": "y = 3.29x + 6873.8", "typical_upperRSquared": "R² = 1", "complexEquation": "y = 5.0109x + 10052", "complexRSquared": "R² = 1", "complex_upperEquation": "y = 6.7319x + 13229", "complex_upperRSquared": "R² = 1" },
                    "Maintenance": { "simpleEquation": "y = 0.0675x", "simpleRSquared": "R² = 1", "complexEquation": "y = 1.632x", "complexRSquared": "R² = 1" }
                },
                gr: {
                    "Capital": { "simpleEquation": "y = 0.5421x + 1975.2", "simpleRSquared": "R² = 1", "simple_upperEquation": "y = 1.5215x + 2631.6", "simple_upperRSquared": "R² = 1", "typicalEquation": "y = 2.5009x + 3288", "typicalRSquared": "R² = 1", "typical_upperEquation": "y = 5.0205x + 12056", "typical_upperRSquared": "R² = 1", "complexEquation": "y = 7.5401x + 20824", "complexRSquared": "R² = 1", "complex_upperEquation": "y = 10.06x + 29592", "complex_upperRSquared": "R² = 1" },
                    "Maintenance": { "simpleEquation": "y = 0.0281x + 6E-14", "simpleRSquared": "R² = 1", "complexEquation": "y = 0.2821x + 1E-12", "complexRSquared": "R² = 1" }
                },
                sp: {
                    "Capital": { "simpleEquation": "y = 0.5592x + 1928.2", "simpleRSquared": "R² = 1", "simple_upperEquation": "y = 1.6359x + 2254.4", "simple_upperRSquared": "R² = 1", "typicalEquation": "y = 2.7125x + 2580.6", "typicalRSquared": "R² = 1", "typical_upperEquation": "y = 6.5348x + 8371.9", "typical_upperRSquared": "R² = 1", "complexEquation": "y = 10.357x + 14163", "complexRSquared": "R² = 1", "complex_upperEquation": "y = 14.179x + 19955", "complex_upperRSquared": "R² = 1" },
                    "Maintenance": { "simpleEquation": "y = 0.045x", "simpleRSquared": "R² = 1", "complexEquation": "y = 1.0697x + 4E-12", "complexRSquared": "R² = 1" }
                },
                ib: {
                    "Capital": { "simpleEquation": "y = 0.8205x + 1928.2", "simpleRSquared": "R² = 1", "simple_upperEquation": "y = 0.8339x + 2896.1", "simple_upperRSquared": "R² = 1", "typicalEquation": "y = 0.8473x + 3864", "typicalRSquared": "R² = 1", "typical_upperEquation": "y = 2.3002x + 8457", "typical_upperRSquared": "R² = 1", "complexEquation": "y = 3.7531x + 13050", "complexRSquared": "R² = 1", "complex_upperEquation": "y = 5.2059x + 17643", "complex_upperRSquared": "R² = 1" },
                    "Maintenance": { "simpleEquation": "y = 0.0487x + 2E-13", "simpleRSquared": "R² = 1", "complexEquation": "y = 1.7689x + 8E-12", "complexRSquared": "R² = 1" }
                },
                pp: {
                    "Capital": { "simpleEquation": "y = 2.3502x + 1545", "simpleRSquared": "R² = 1", "simple_upperEquation": "y = 3.5355x + 1672.5", "simple_upperRSquared": "R² = 1", "typicalEquation": "y = 4.7209x + 1800", "typicalRSquared": "R² = 1", "typical_upperEquation": "y = 6.2951x + 2775", "typical_upperRSquared": "R² = 1", "complexEquation": "y = 7.8694x + 3750", "complexRSquared": "R² = 1", "complex_upperEquation": "y = 9.4437x + 4725", "complex_upperRSquared": "R² = 1" },
                    "Maintenance": { "simpleEquation": "y = 0.0563x", "simpleRSquared": "R² = 1", "complexEquation": "y = 0.3075x + 1E-12", "complexRSquared": "R² = 1" }
                },
                vs: {
                    "Capital": { "simpleEquation": "y = 0.2997x + 772.5", "simpleRSquared": "R² = 1", "simple_upperEquation": "y = 0.4329x + 1061.2", "simple_upperRSquared": "R² = 1", "typicalEquation": "y = 0.5662x + 1350", "typicalRSquared": "R² = 1", "typical_upperEquation": "y = 0.7534x + 1612.5", "typical_upperRSquared": "R² = 1", "complexEquation": "y = 0.9406x + 1875", "complexRSquared": "R² = 1", "complex_upperEquation": "y = 1.1278x + 2137.5", "complex_upperRSquared": "R² = 1" },
                    "Maintenance": { "simpleEquation": "y = 0.0626x", "simpleRSquared": "R² = 1", "complexEquation": "y = 0.1215x - 5E-13", "complexRSquared": "R² = 1" }
                }
            };
            //begin psuedo enums
            var bmpTypes = {
                id: 0, //impervious disconnection
                rh: 1, //rain harvesting
                rg: 2, //rain garden
                gr: 3, //green roofs
                sp: 4, //street planter
                ib: 5, //infiltration basin
                pp: 6, //permeable pavement
                vs: 7//vegetated swale
            };
            var defaultBMPProps = {
                bmpType: bmpTypes.RG,
                footprintAreaSqFt: 0,
                capacityVolGals: 0,
                capitalCost: -1,
                maintCost: -1
            };
            // Public API here
            var service = {
                dataVersion: dataVersion,
                eqnsVersion: eqnsVersion,
                dataReferenceYear: dataReferenceYear,
                validateInputs: validateInputs,
                parseLinearEqn: parseLinearEqn,
                computeCost: computeCost,
                computeLIDCtrlCost: computeLIDCtrlCost,
                computeLIDCtrlCosts: computeLIDCtrlCosts,
                defaultRoundDP: defaultRoundDP,
                dpRound: dpRound
            };
            return service;
            ///////////////////////////

            /**
             * @ngdoc function
             * @name dpRound
             * @methodOf nscwebappApp.costEngineSrvc
             * @description utility function to round numbers to various decimal places
             * @param {number} val value to round
             * @param {number} decimalPlaces number of decimal places to round to
             * @returns {Number} resulting rounded number
             */
            function dpRound(val, decimalPlaces) {
                var exp = Math.pow(10, decimalPlaces);
                var rslt = Math.round(val * exp) / exp;
                if (Math.abs(rslt) === 0) {
                    rslt = Math.abs(rslt);
                }
                return rslt;
            }

            /**
             * @ngdoc function
             * @name validateInputs
             * @methodOf nscwebappApp.costEngineSrvc
             * @description validates the inputs used by this service consisting of a BMP property bag and
             * a design complexity property bag. Delegates validation of design complexity to appropriate service
             * @param {string} inputType type of input to valide
             * @param {object} inputs inputs to validate
             * @returns {Boolean} boolean indicating whether validation passed on failed
             */
            function validateInputs(inputType, inputs) {
                switch (inputType) {
                    case 'BMP':
                        return validateBMPInputs(inputs);
                        //break;
                    case 'Complexity':
                        return designComplexity.validateInputs(inputs);
                        //break;
                    default:
                        return false;
                }
            }

            /**
             * @ngdoc function
             * @name validateBMPInputs
             * @methodOf nscwebappApp.costEngineSrvc
             * @description validates BMP property bag passed in as inputs to the cost engine service
             * @param {object} options inputs to validate
             * @returns {Boolean} boolean indicating whether validation passed on failed
             */
            function validateBMPInputs(options) {
                var flag = false;
                options = angular.extend({}, defaultBMPProps, options);
                flag = (typeof (options.footprintAreaSqFt) === 'number' && options.bmpType > -1 && options.bmpType < 8); //check for valid BMP type

                flag = flag && (options.footprintAreaSqFt !== undefined && typeof (options.footprintAreaSqFt) === 'number' &&
                        options.footprintAreaSqFt >= 0);
                flag = flag && (options.capacityVolGals !== undefined && typeof (options.capacityVolGals) === 'number' &&
                        options.capacityVolGals >= 0);
                return flag;
            }

            /**
             * @ngdoc function
             * @name validateNumeric
             * @methodOf nscwebappApp.costEngineSrvc
             * @description utility function that valides arguments are numeric
             * @param {object} options inputs to validate
             * @returns {Boolean} boolean indicating whether validation passed on failed
             */
            function validateNumeric(options) {
                var flag = true;
                angular.forEach(options, function (val, idx) {
                    flag = flag && (typeof (val) === 'number');
                });
                return flag;
            }

            /**
             * @ngdoc function
             * @name parseLinearEqn
             * @methodOf nscwebappApp.costEngineSrvc
             * @description function to parse linear regression equations to isolate slope and intercept
             * @param {type} eqnstr string representing linear equation to be parsed
             * @returns {object} parsed equation components
             */
            function parseLinearEqn(eqnstr) {
                var slope = -1;
                var intercept = -1;
                var rsltFail = { status: false, slope: -1, intercept: -1 };
                var startTokens = 'y=';
                var delim = '-';
                var temparr = [];
                if (eqnstr.indexOf("+") > 1) {
                    delim = '+';
                }
                if (eqnstr.indexOf("x") < 1 || eqnstr.indexOf('y') < 0) {
                    return rsltFail;
                }

                temparr = eqnstr.replace(/ /g, '').replace(startTokens, '').split(delim);
                try {
                    slope = Number(temparr[0].substring(0, temparr[0].length - 1));
                    if (temparr.length > 1) {
                        intercept = Number(temparr[1]);
                    } else {
                        intercept = 0;
                    }
                } catch (error) {
                    console.log(error);
                    return rsltFail;
                }
                return { status: true, slope: slope, intercept: intercept };
            }

            /**
             * @ngdoc function
             * @name parseLinearEqn
             * @methodOf nscwebappApp.costEngineSrvc
             * @description Computes cost given cost curve, intercept, slope, inflation factor and regional adjustment factor 
             * @param {number} bmpSize bmp size
             * @param {number} ccIntercept intercept on cost estimation equation
             * @param {number} ccSlope slope of cost estimation equation
             * @param {number} inflation inflation factor
             * @param {number} regionalFactor regional cost multiplier
             * @returns {Number|Boolean} computed cost or boolean indicating whether validation passed on failed
             */
            function computeCost(bmpSize, ccIntercept, ccSlope, inflation, regionalFactor) {
                var flag = validateNumeric([bmpSize, ccIntercept, ccSlope, inflation, regionalFactor]);
                if (bmpSize < 0.00001) {
                    return 0;
                }
                if (flag) {
                    return dpRound(((bmpSize * ccSlope) + ccIntercept) * inflation * regionalFactor, defaultRoundDP);
                } else {
                    return flag;
                }
            }

            /**
             * @ngdoc function
             * @name computeLIDCtrlCost
             * @methodOf nscwebappApp.costEngineSrvc
             * @description Computes cost give lid control properties, inflation factor and regional adjustment factor  
             * @param {type} lid lid control properties
             * @param {number} inflation inflation factor
             * @param {number} regionalFactor regional cost multiplier
             * @returns {unresolved} lid control properties with computed costs
             */
            function computeLIDCtrlCost(lid, inflation, regionalFactor) {
                var costEqnLow = {};
                var costEqnHigh = {};

                switch (lid.type) {
                    case 0: //DC
                        lid.costQty = lid.footprintAreaSqFt;
                        break;
                    case 1: //RG
                        lid.costQty = lid.footprintAreaSqFt;
                        break;
                    default:
                        lid.costQty = lid.footprintAreaSqFt;
                }
                //compute capital cost
                costEqnLow = parseLinearEqn(costData[lid.id].Capital[lid.complexity + 'Equation']);
                costEqnHigh = parseLinearEqn(costData[lid.id].Capital[lid.complexity + '_upperEquation']);
                lid.capCostLow = computeCost(lid.costQty, costEqnLow.intercept, costEqnLow.slope, inflation, regionalFactor);
                lid.capCostHigh = computeCost(lid.costQty, costEqnHigh.intercept, costEqnHigh.slope, inflation, regionalFactor);

                //compute maintenance cost
                costEqnLow = parseLinearEqn(costData[lid.id].Maintenance['simpleEquation']);
                costEqnHigh = parseLinearEqn(costData[lid.id].Maintenance['complexEquation']);
                lid.maintCostLow = computeCost(lid.costQty, costEqnLow.intercept, costEqnLow.slope, inflation, regionalFactor);
                lid.maintCostHigh = computeCost(lid.costQty, costEqnHigh.intercept, costEqnHigh.slope, inflation, regionalFactor);

                return lid;
            }

            /**
             * @ngdoc function
             * @name computeLIDCtrlCost
             * @methodOf nscwebappApp.costEngineSrvc
             * @description Computes costs given site properties, inflation factor and regional adjustment factor  
             * @param {type} siteData site  properties
             * @param {number} inflation inflation factor
             * @param {number} regionalFactor regional cost multiplier
             * @returns {Array} computed costs for multiple scenarios
             */
            function computeLIDCtrlCosts(siteData, inflation, regionalFactor) {

                var complexities = ['simple', 'typical', 'complex'];
                var siteSuitabilities = ['Poor', 'Moderate', 'Excellent'];
                var topographies = ['Flat (2% Slope)', 'Mod. Flat (5% Slope)', 'Mod. Steep (10% Slope)', 'Steep (> 15% Slope)'];
                var soiltype = ['A', 'B', 'C', 'D'];

                var cmplxty;
                //var prefix = 'current';
                var tblResults = [];
                var tempObj = {};
                var totalsObj = {
                    currentScenarioPercentDrainAreaSum: 0,
                    currentScenarioCapCostLowSum: 0,
                    currentScenarioCapCostHighSum: 0,
                    baseScenarioPercentDrainAreaSum: 0,
                    baseScenarioCapCostLowSum: 0,
                    baseScenarioCapCostHighSum: 0,

                    currentScenarioMaintCostLowSum: 0,
                    currentScenarioMaintCostHighSum: 0,
                    baseScenarioMaintCostLowSum: 0,
                    baseScenarioMaintCostHighSum: 0
                };

                angular.forEach(siteData, function (lidObjs, idy) {
                    var inflation = lidObjs.inflation || 1;
                    var regionalFactor = lidObjs.regionalFactor || 1;
                    angular.forEach(lidObjs.costvars, function (val, idx) {

                        index = _.findIndex(tblResults, function (itm) {
                            return itm.id === val.id;
                        });
                        tempObj = index === -1 ? {} : tblResults[index];

                        //if (val.footprintAreaSqFt !== 0 && val.footprintAreaSqFt !== '0') {
                        cmplxty = designComplexity.computeDesignComplexity(lidObjs.complexity, val.hasPretreatment);
                        console.log("inside computeLIDCtrlCosts complexity=", cmplxty, "val", val);
                        val.complexity = complexities[cmplxty];
                        computeLIDCtrlCost(val, inflation, regionalFactor, idy);//idy is scenario name
                        lidObjs.complexity.devType = lidObjs.complexity['isNewDevelopment'] ? 'New Development' : 'Re-development';
                        lidObjs.complexity.siteSuit = siteSuitabilities[lidObjs.complexity['siteSuitability']];
                        lidObjs.complexity.topo = topographies[lidObjs.complexity['topography']];
                        lidObjs.complexity.soilTypeStr = soiltype[lidObjs.complexity['soilType']];

                        tempObj[idy + 'TotSiteArea'] = val['totSiteArea'];
                        tempObj[idy + 'PercentDrainArea'] = val['percentDrainArea'];
                        tempObj[idy + 'Pretreat'] = val['hasPretreatment'] ? 'Yes' : val['hasPretreatment'] === false ? 'No' : 'NA';
                        tempObj[idy + 'CapCostLow'] = val['capCostLow'];
                        tempObj[idy + 'CapCostHigh'] = val['capCostHigh'];
                        tempObj[idy + 'MaintCostLow'] = val['maintCostLow'];
                        tempObj[idy + 'MaintCostHigh'] = val['maintCostHigh'];

                        //update totals
                        //totalsObj[idy + 'TotSiteArea'] = val['totSiteArea'];
                        totalsObj[idy + 'PercentDrainAreaSum'] = totalsObj[idy + 'PercentDrainAreaSum'] + val['percentDrainArea'];
                        //totalsObj[idy + 'Pretreat'] = val['hasPretreatment'] ? 'Yes' : val['hasPretreatment'] === false ? 'No' : 'NA';
                        totalsObj[idy + 'CapCostLowSum'] = totalsObj[idy + 'CapCostLowSum'] + val['capCostLow'];
                        totalsObj[idy + 'CapCostHighSum'] = totalsObj[idy + 'CapCostHighSum'] + val['capCostHigh'];
                        totalsObj[idy + 'MaintCostLowSum'] = totalsObj[idy + 'MaintCostLowSum'] + val['maintCostLow'];
                        totalsObj[idy + 'MaintCostHighSum'] = totalsObj[idy + 'MaintCostHighSum'] + val['maintCostHigh'];

                        if (index === -1) {
                            tblResults.push(angular.extend({}, tempObj, val));
                        }
                        //}
                    });
                });
                //console.log(tblResults);
                tblResults.push(totalsObj);
                return tblResults;
            }
        });
