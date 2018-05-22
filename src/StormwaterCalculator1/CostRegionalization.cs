// ----------------------------------------------------------------------------
// Module:      SiteData.cs
// Project:     EPA Stormwater Calculator
// Description: Code module for the CostRegionalization class that manages computation 
//              of regional cost multiplier based on BLS api. http://www.bls.gov/developers/api_signature_v2.htm
// Copyright:   N/A
// Authors:     D. Pankani, Geosyntec
// Version:     1.2.0.0
// Last Update: 08/17/2016
// ----------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Net;
using System.Data.SqlClient;
using System.Configuration;
using System.Data;
using System.Globalization;
using System.ComponentModel;
using Newtonsoft.Json;



namespace StormwaterCalculator
{
    public class CostRegionalization
    {
        //default National index computed for 2015 used as the basis for normalizing regional indexes
        private double default2015NationalIndex = 201.22;
        //user is given the option to select the nearest 3 bls regions
        private int shortListCount = 3;
        private int dcp = 2; //default number of decimal places to round 2
        //regionalization model coefficients - need to be updated periodically
        private double c0_intercept = -19.4;
        private double c1_readyMix = 0.113;
        private double c2_tractorShovel = 0.325;
        private double c3_energy = 0.096;
        private double c4_fuelUtils = 0.398;

        //minified regionalization json data
        //public string regDefaults = "[{\"BLSSeriesID\":\"0000\",\"State\":\"NA\",\"blsCity\":\"NATIONAL\",\"regionalFactor\":1,\"latitude\":0,\"longitude\":0,\"GEOID\":-1,\"blsReadyMixConcID\":\"PCU327320327320\",\"blsTractorShovelLoadersID\":\"PCU33312033312014\",\"blsEnergyID\":\"CUUS0000SA0E\",\"blsFuelsUtilitiesID\":\"CUUS0000SAH2\"},{\"BLSSeriesID\":\"A427\",\"State\":\"AK\",\"blsCity\":\"Anchorage\",\"regionalFactor\":-1,\"latitude\":61.167916,\"longitude\":-149.847166,\"GEOID\":2305,\"blsReadyMixConcID\":\"PCU327320327320\",\"blsTractorShovelLoadersID\":\"PCU33312033312014\",\"blsEnergyID\":\"CUUSA427SA0E\",\"blsFuelsUtilitiesID\":\"CUUSA427SAH2\"},{\"BLSSeriesID\":\"A319\",\"State\":\"GA\",\"blsCity\":\"Atlanta\",\"regionalFactor\":-1,\"latitude\":33.824102,\"longitude\":-84.331858,\"GEOID\":3817,\"blsReadyMixConcID\":\"PCU327320327320\",\"blsTractorShovelLoadersID\":\"PCU33312033312014\",\"blsEnergyID\":\"CUUSA319SA0E\",\"blsFuelsUtilitiesID\":\"CUUSA319SAH2\"},{\"BLSSeriesID\":\"A103\",\"State\":\"MA\",\"blsCity\":\"Boston\",\"regionalFactor\":-1,\"latitude\":42.373132,\"longitude\":-71.140708,\"GEOID\":9271,\"blsReadyMixConcID\":\"PCU327320327320\",\"blsTractorShovelLoadersID\":\"PCU33312033312014\",\"blsEnergyID\":\"CUUSA103SA0E\",\"blsFuelsUtilitiesID\":\"CUUSA103SAH2\"},{\"BLSSeriesID\":\"A207\",\"State\":\"IL\",\"blsCity\":\"Chicago\",\"regionalFactor\":-1,\"latitude\":41.827126,\"longitude\":-87.895427,\"GEOID\":16264,\"blsReadyMixConcID\":\"PCU327320327320\",\"blsTractorShovelLoadersID\":\"PCU33312033312014\",\"blsEnergyID\":\"CUUSA207SA0E\",\"blsFuelsUtilitiesID\":\"CUUSA207SAH2\"},{\"BLSSeriesID\":\"A213\",\"State\":\"OH\",\"blsCity\":\"Cincinnati\",\"regionalFactor\":-1,\"latitude\":39.185505,\"longitude\":-84.462043,\"GEOID\":16885,\"blsReadyMixConcID\":\"PCU327320327320\",\"blsTractorShovelLoadersID\":\"PCU33312033312014\",\"blsEnergyID\":\"CUUSA213SA0E\",\"blsFuelsUtilitiesID\":\"CUUSA213SAH2\"},{\"BLSSeriesID\":\"A210\",\"State\":\"OH\",\"blsCity\":\"Cleveland\",\"regionalFactor\":-1,\"latitude\":41.443638,\"longitude\":-81.605443,\"GEOID\":17668,\"blsReadyMixConcID\":\"PCU327320327320\",\"blsTractorShovelLoadersID\":\"PCU33312033312014\",\"blsEnergyID\":\"CUUSA210SA0E\",\"blsFuelsUtilitiesID\":\"CUUSA210SAH2\"},{\"BLSSeriesID\":\"A316\",\"State\":\"TX\",\"blsCity\":\"Dallas\",\"regionalFactor\":-1,\"latitude\":36.060538,\"longitude\":-102.515287,\"GEOID\":21988,\"blsReadyMixConcID\":\"PCU327320327320\",\"blsTractorShovelLoadersID\":\"PCU33312033312014\",\"blsEnergyID\":\"CUUSA316SA0E\",\"blsFuelsUtilitiesID\":\"CUUSA316SAH2\"},{\"BLSSeriesID\":\"A433\",\"State\":\"CO\",\"blsCity\":\"Denver\",\"regionalFactor\":-1,\"latitude\":39.710774,\"longitude\":-104.955096,\"GEOID\":23527,\"blsReadyMixConcID\":\"PCU327320327320\",\"blsTractorShovelLoadersID\":\"PCU33312033312014\",\"blsEnergyID\":\"CUUSA433SA0E\",\"blsFuelsUtilitiesID\":\"CUUSA433SAH2\"},{\"BLSSeriesID\":\"A208\",\"State\":\"MI\",\"blsCity\":\"Detroit\",\"regionalFactor\":-1,\"latitude\":42.489752,\"longitude\":-83.227211,\"GEOID\":23824,\"blsReadyMixConcID\":\"PCU327320327320\",\"blsTractorShovelLoadersID\":\"PCU33312033312014\",\"blsEnergyID\":\"CUUSA208SA0E\",\"blsFuelsUtilitiesID\":\"CUUSA208SAH2\"},{\"BLSSeriesID\":\"A426\",\"State\":\"HI\",\"blsCity\":\"Honolulu\",\"regionalFactor\":-1,\"latitude\":21.364556,\"longitude\":-157.939756,\"GEOID\":89770,\"blsReadyMixConcID\":\"PCU327320327320\",\"blsTractorShovelLoadersID\":\"PCU33312033312014\",\"blsEnergyID\":\"CUUSA426SA0E\",\"blsFuelsUtilitiesID\":\"CUUSA426SAH2\"},{\"BLSSeriesID\":\"A318\",\"State\":\"TX\",\"blsCity\":\"Houston\",\"regionalFactor\":-1,\"latitude\":29.784308,\"longitude\":-95.393531,\"GEOID\":40429,\"blsReadyMixConcID\":\"PCU327320327320\",\"blsTractorShovelLoadersID\":\"PCU33312033312014\",\"blsEnergyID\":\"CUUSA318SA0E\",\"blsFuelsUtilitiesID\":\"CUUSA318SAH2\"},{\"BLSSeriesID\":\"A421\",\"State\":\"CA\",\"blsCity\":\"Los Angeles\",\"regionalFactor\":-1,\"latitude\":33.982668,\"longitude\":-118.104332,\"GEOID\":51445,\"blsReadyMixConcID\":\"PCU327320327320\",\"blsTractorShovelLoadersID\":\"PCU33312033312014\",\"blsEnergyID\":\"CUUSA421SA0E\",\"blsFuelsUtilitiesID\":\"CUUSA421SAH2\"},{\"BLSSeriesID\":\"A320\",\"State\":\"FL\",\"blsCity\":\"Miami\",\"regionalFactor\":-1,\"latitude\":26.175616,\"longitude\":-80.231428,\"GEOID\":56602,\"blsReadyMixConcID\":\"PCU327320327320\",\"blsTractorShovelLoadersID\":\"PCU33312033312014\",\"blsEnergyID\":\"CUUSA320SA0E\",\"blsFuelsUtilitiesID\":\"CUUSA320SAH2\"},{\"BLSSeriesID\":\"A212\",\"State\":\"WI\",\"blsCity\":\"Milwaukee\",\"regionalFactor\":-1,\"latitude\":43.055674,\"longitude\":-88.100524,\"GEOID\":57466,\"blsReadyMixConcID\":\"PCU327320327320\",\"blsTractorShovelLoadersID\":\"PCU33312033312014\",\"blsEnergyID\":\"CUUSA212SA0E\",\"blsFuelsUtilitiesID\":\"CUUSA212SAH2\"},{\"BLSSeriesID\":\"A211\",\"State\":\"MN\",\"blsCity\":\"Minneapolis\",\"regionalFactor\":-1,\"latitude\":44.978156,\"longitude\":-93.2798,\"GEOID\":57628,\"blsReadyMixConcID\":\"PCU327320327320\",\"blsTractorShovelLoadersID\":\"PCU33312033312014\",\"blsEnergyID\":\"CUUSA211SA0E\",\"blsFuelsUtilitiesID\":\"CUUSA211SAH2\"},{\"BLSSeriesID\":\"A101\",\"State\":\"NY\",\"blsCity\":\"New York\",\"regionalFactor\":-1,\"latitude\":40.718357,\"longitude\":-73.970221,\"GEOID\":63217,\"blsReadyMixConcID\":\"PCU327320327320\",\"blsTractorShovelLoadersID\":\"PCU33312033312014\",\"blsEnergyID\":\"CUUSA101SA0E\",\"blsFuelsUtilitiesID\":\"CUUSA101SAH2\"},{\"BLSSeriesID\":\"A102\",\"State\":\"PA\",\"blsCity\":\"Philadelphia\",\"regionalFactor\":-1,\"latitude\":39.973331,\"longitude\":-75.298163,\"GEOID\":69076,\"blsReadyMixConcID\":\"PCU327320327320\",\"blsTractorShovelLoadersID\":\"PCU33312033312014\",\"blsEnergyID\":\"CUUSA102SA0E\",\"blsFuelsUtilitiesID\":\"CUUSA102SAH2\"},{\"BLSSeriesID\":\"A104\",\"State\":\"PA\",\"blsCity\":\"Pittsburgh\",\"regionalFactor\":-1,\"latitude\":40.456961,\"longitude\":-79.951005,\"GEOID\":69697,\"blsReadyMixConcID\":\"PCU327320327320\",\"blsTractorShovelLoadersID\":\"PCU33312033312014\",\"blsEnergyID\":\"CUUSA104SA0E\",\"blsFuelsUtilitiesID\":\"CUUSA104SAH2\"},{\"BLSSeriesID\":\"A425\",\"State\":\"OR\",\"blsCity\":\"Portland\",\"regionalFactor\":-1,\"latitude\":45.520404,\"longitude\":-122.651087,\"GEOID\":71317,\"blsReadyMixConcID\":\"PCU327320327320\",\"blsTractorShovelLoadersID\":\"PCU33312033312014\",\"blsEnergyID\":\"CUUSA425SA0E\",\"blsFuelsUtilitiesID\":\"CUUSA425SAH2\"},{\"BLSSeriesID\":\"A424\",\"State\":\"CA\",\"blsCity\":\"San Diego\",\"regionalFactor\":-1,\"latitude\":32.928728,\"longitude\":-117.128799,\"GEOID\":78661,\"blsReadyMixConcID\":\"PCU327320327320\",\"blsTractorShovelLoadersID\":\"PCU33312033312014\",\"blsEnergyID\":\"CUUSA424SA0E\",\"blsFuelsUtilitiesID\":\"CUUSA424SAH2\"},{\"BLSSeriesID\":\"A422\",\"State\":\"CA\",\"blsCity\":\"San Francisco\",\"regionalFactor\":-1,\"latitude\":37.690115,\"longitude\":-122.128498,\"GEOID\":78904,\"blsReadyMixConcID\":\"PCU327320327320\",\"blsTractorShovelLoadersID\":\"PCU33312033312014\",\"blsEnergyID\":\"CUUSA422SA0E\",\"blsFuelsUtilitiesID\":\"CUUSA422SAH2\"},{\"BLSSeriesID\":\"A423\",\"State\":\"WA\",\"blsCity\":\"Seattle\",\"regionalFactor\":-1,\"latitude\":47.468409,\"longitude\":-122.274661,\"GEOID\":80389,\"blsReadyMixConcID\":\"PCU327320327320\",\"blsTractorShovelLoadersID\":\"PCU33312033312014\",\"blsEnergyID\":\"CUUSA423SA0E\",\"blsFuelsUtilitiesID\":\"CUUSA423SAH2\"},{\"BLSSeriesID\":\"A311\",\"State\":\"DC\",\"blsCity\":\"Washington\",\"regionalFactor\":-1,\"latitude\":38.897394,\"longitude\":-77.18974,\"GEOID\":92242,\"blsReadyMixConcID\":\"PCU327320327320\",\"blsTractorShovelLoadersID\":\"PCU33312033312014\",\"blsEnergyID\":\"CUUSA311SA0E\",\"blsFuelsUtilitiesID\":\"CUUSA311SAH2\"}]";
        /* Feb 2018 move to city specific model where each city gets its own coefficients and intercept
        public string regDefaults = "[{\"BLSSeriesID\":\"0000\",\"State\":\"NA\",\"blsCity\":\"NATIONAL\",\"regionalFactor\":1.000,\"latitude\":0,\"longitude\":0,\"GEOID\":-1,\"blsReadyMixConcID\":\"PCU327320327320\",\"blsTractorShovelLoadersID\":\"PCU33312033312014\",\"blsEnergyID\":\"CUUS0000SA0E\",\"blsFuelsUtilitiesID\":\"CUUS0000SAH2\",\"regModel2014Index\":205.3048894},{\"BLSSeriesID\":\"A427\",\"State\":\"AK\",\"blsCity\":\"Anchorage\",\"regionalFactor\":-1.000,\"latitude\":61.167916,\"longitude\":-149.847166,\"GEOID\":2305,\"blsReadyMixConcID\":\"PCU327320327320\",\"blsTractorShovelLoadersID\":\"PCU33312033312014\",\"blsEnergyID\":\"CUUSA427SA0E\",\"blsFuelsUtilitiesID\":\"CUUSA427SAH2\",\"regModel2014Index\":226.7835622},{\"BLSSeriesID\":\"A319\",\"State\":\"GA\",\"blsCity\":\"Atlanta\",\"regionalFactor\":-1.000,\"latitude\":33.824102,\"longitude\":-84.331858,\"GEOID\":3817,\"blsReadyMixConcID\":\"PCU327320327320\",\"blsTractorShovelLoadersID\":\"PCU33312033312014\",\"blsEnergyID\":\"CUUSA319SA0E\",\"blsFuelsUtilitiesID\":\"CUUSA319SAH2\",\"regModel2014Index\":229.417366},{\"BLSSeriesID\":\"A103\",\"State\":\"MA\",\"blsCity\":\"Boston\",\"regionalFactor\":-1.000,\"latitude\":42.373132,\"longitude\":-71.140708,\"GEOID\":9271,\"blsReadyMixConcID\":\"PCU327320327320\",\"blsTractorShovelLoadersID\":\"PCU33312033312014\",\"blsEnergyID\":\"CUUSA103SA0E\",\"blsFuelsUtilitiesID\":\"CUUSA103SAH2\",\"regModel2014Index\":216.5301231},{\"BLSSeriesID\":\"A207\",\"State\":\"IL\",\"blsCity\":\"Chicago\",\"regionalFactor\":-1.000,\"latitude\":41.827126,\"longitude\":-87.895427,\"GEOID\":16264,\"blsReadyMixConcID\":\"PCU327320327320\",\"blsTractorShovelLoadersID\":\"PCU33312033312014\",\"blsEnergyID\":\"CUUSA207SA0E\",\"blsFuelsUtilitiesID\":\"CUUSA207SAH2\",\"regModel2014Index\":193.9052141},{\"BLSSeriesID\":\"A213\",\"State\":\"OH\",\"blsCity\":\"Cincinnati\",\"regionalFactor\":-1.000,\"latitude\":39.185505,\"longitude\":-84.462043,\"GEOID\":16885,\"blsReadyMixConcID\":\"PCU327320327320\",\"blsTractorShovelLoadersID\":\"PCU33312033312014\",\"blsEnergyID\":\"CUUSA213SA0E\",\"blsFuelsUtilitiesID\":\"CUUSA213SAH2\",\"regModel2014Index\":200.9823586},{\"BLSSeriesID\":\"A210\",\"State\":\"OH\",\"blsCity\":\"Cleveland\",\"regionalFactor\":-1.000,\"latitude\":41.443638,\"longitude\":-81.605443,\"GEOID\":17668,\"blsReadyMixConcID\":\"PCU327320327320\",\"blsTractorShovelLoadersID\":\"PCU33312033312014\",\"blsEnergyID\":\"CUUSA210SA0E\",\"blsFuelsUtilitiesID\":\"CUUSA210SAH2\",\"regModel2014Index\":191.0175351},{\"BLSSeriesID\":\"A316\",\"State\":\"TX\",\"blsCity\":\"Dallas\",\"regionalFactor\":-1.000,\"latitude\":36.060538,\"longitude\":-102.515287,\"GEOID\":21988,\"blsReadyMixConcID\":\"PCU327320327320\",\"blsTractorShovelLoadersID\":\"PCU33312033312014\",\"blsEnergyID\":\"CUUSA316SA0E\",\"blsFuelsUtilitiesID\":\"CUUSA316SAH2\",\"regModel2014Index\":205.7769973},{\"BLSSeriesID\":\"A433\",\"State\":\"CO\",\"blsCity\":\"Denver\",\"regionalFactor\":-1.000,\"latitude\":39.710774,\"longitude\":-104.955096,\"GEOID\":23527,\"blsReadyMixConcID\":\"PCU327320327320\",\"blsTractorShovelLoadersID\":\"PCU33312033312014\",\"blsEnergyID\":\"CUUSA433SA0E\",\"blsFuelsUtilitiesID\":\"CUUSA433SAH2\",\"regModel2014Index\":201.125973},{\"BLSSeriesID\":\"A208\",\"State\":\"MI\",\"blsCity\":\"Detroit\",\"regionalFactor\":-1.000,\"latitude\":42.489752,\"longitude\":-83.227211,\"GEOID\":23824,\"blsReadyMixConcID\":\"PCU327320327320\",\"blsTractorShovelLoadersID\":\"PCU33312033312014\",\"blsEnergyID\":\"CUUSA208SA0E\",\"blsFuelsUtilitiesID\":\"CUUSA208SAH2\",\"regModel2014Index\":209.0531332},{\"BLSSeriesID\":\"A426\",\"State\":\"HI\",\"blsCity\":\"Honolulu\",\"regionalFactor\":-1.000,\"latitude\":21.364556,\"longitude\":-157.939756,\"GEOID\":89770,\"blsReadyMixConcID\":\"PCU327320327320\",\"blsTractorShovelLoadersID\":\"PCU33312033312014\",\"blsEnergyID\":\"CUUSA426SA0E\",\"blsFuelsUtilitiesID\":\"CUUSA426SAH2\",\"regModel2014Index\":269.3302901},{\"BLSSeriesID\":\"A318\",\"State\":\"TX\",\"blsCity\":\"Houston\",\"regionalFactor\":-1.000,\"latitude\":29.784308,\"longitude\":-95.393531,\"GEOID\":40429,\"blsReadyMixConcID\":\"PCU327320327320\",\"blsTractorShovelLoadersID\":\"PCU33312033312014\",\"blsEnergyID\":\"CUUSA318SA0E\",\"blsFuelsUtilitiesID\":\"CUUSA318SAH2\",\"regModel2014Index\":181.8025875},{\"BLSSeriesID\":\"A421\",\"State\":\"CA\",\"blsCity\":\"Los Angeles\",\"regionalFactor\":-1.000,\"latitude\":33.982668,\"longitude\":-118.104332,\"GEOID\":51445,\"blsReadyMixConcID\":\"PCU327320327320\",\"blsTractorShovelLoadersID\":\"PCU33312033312014\",\"blsEnergyID\":\"CUUSA421SA0E\",\"blsFuelsUtilitiesID\":\"CUUSA421SAH2\",\"regModel2014Index\":234.2691355},{\"BLSSeriesID\":\"A320\",\"State\":\"FL\",\"blsCity\":\"Miami\",\"regionalFactor\":-1.000,\"latitude\":26.175616,\"longitude\":-80.231428,\"GEOID\":56602,\"blsReadyMixConcID\":\"PCU327320327320\",\"blsTractorShovelLoadersID\":\"PCU33312033312014\",\"blsEnergyID\":\"CUUSA320SA0E\",\"blsFuelsUtilitiesID\":\"CUUSA320SAH2\",\"regModel2014Index\":180.237286},{\"BLSSeriesID\":\"A212\",\"State\":\"WI\",\"blsCity\":\"Milwaukee\",\"regionalFactor\":-1.000,\"latitude\":43.055674,\"longitude\":-88.100524,\"GEOID\":57466,\"blsReadyMixConcID\":\"PCU327320327320\",\"blsTractorShovelLoadersID\":\"PCU33312033312014\",\"blsEnergyID\":\"CUUSA212SA0E\",\"blsFuelsUtilitiesID\":\"CUUSA212SAH2\",\"regModel2014Index\":201.6221956},{\"BLSSeriesID\":\"A211\",\"State\":\"MN\",\"blsCity\":\"Minneapolis\",\"regionalFactor\":-1.000,\"latitude\":44.978156,\"longitude\":-93.2798,\"GEOID\":57628,\"blsReadyMixConcID\":\"PCU327320327320\",\"blsTractorShovelLoadersID\":\"PCU33312033312014\",\"blsEnergyID\":\"CUUSA211SA0E\",\"blsFuelsUtilitiesID\":\"CUUSA211SAH2\",\"regModel2014Index\":193.9197203},{\"BLSSeriesID\":\"A101\",\"State\":\"NY\",\"blsCity\":\"New York\",\"regionalFactor\":-1.000,\"latitude\":40.718357,\"longitude\":-73.970221,\"GEOID\":63217,\"blsReadyMixConcID\":\"PCU327320327320\",\"blsTractorShovelLoadersID\":\"PCU33312033312014\",\"blsEnergyID\":\"CUUSA101SA0E\",\"blsFuelsUtilitiesID\":\"CUUSA101SAH2\",\"regModel2014Index\":192.0815131},{\"BLSSeriesID\":\"A102\",\"State\":\"PA\",\"blsCity\":\"Philadelphia\",\"regionalFactor\":-1.000,\"latitude\":39.973331,\"longitude\":-75.298163,\"GEOID\":69076,\"blsReadyMixConcID\":\"PCU327320327320\",\"blsTractorShovelLoadersID\":\"PCU33312033312014\",\"blsEnergyID\":\"CUUSA102SA0E\",\"blsFuelsUtilitiesID\":\"CUUSA102SAH2\",\"regModel2014Index\":196.349605},{\"BLSSeriesID\":\"A104\",\"State\":\"PA\",\"blsCity\":\"Pittsburgh\",\"regionalFactor\":-1.000,\"latitude\":40.456961,\"longitude\":-79.951005,\"GEOID\":69697,\"blsReadyMixConcID\":\"PCU327320327320\",\"blsTractorShovelLoadersID\":\"PCU33312033312014\",\"blsEnergyID\":\"CUUSA104SA0E\",\"blsFuelsUtilitiesID\":\"CUUSA104SAH2\",\"regModel2014Index\":213.5812894},{\"BLSSeriesID\":\"A425\",\"State\":\"OR\",\"blsCity\":\"Portland\",\"regionalFactor\":-1.000,\"latitude\":45.520404,\"longitude\":-122.651087,\"GEOID\":71317,\"blsReadyMixConcID\":\"PCU327320327320\",\"blsTractorShovelLoadersID\":\"PCU33312033312014\",\"blsEnergyID\":\"CUUSA425SA0E\",\"blsFuelsUtilitiesID\":\"CUUSA425SAH2\",\"regModel2014Index\":216.1105637},{\"BLSSeriesID\":\"A424\",\"State\":\"CA\",\"blsCity\":\"San Diego\",\"regionalFactor\":-1.000,\"latitude\":32.928728,\"longitude\":-117.128799,\"GEOID\":78661,\"blsReadyMixConcID\":\"PCU327320327320\",\"blsTractorShovelLoadersID\":\"PCU33312033312014\",\"blsEnergyID\":\"CUUSA424SA0E\",\"blsFuelsUtilitiesID\":\"CUUSA424SAH2\",\"regModel2014Index\":222.5492038},{\"BLSSeriesID\":\"A422\",\"State\":\"CA\",\"blsCity\":\"San Francisco\",\"regionalFactor\":-1.000,\"latitude\":37.690115,\"longitude\":-122.128498,\"GEOID\":78904,\"blsReadyMixConcID\":\"PCU327320327320\",\"blsTractorShovelLoadersID\":\"PCU33312033312014\",\"blsEnergyID\":\"CUUSA422SA0E\",\"blsFuelsUtilitiesID\":\"CUUSA422SAH2\",\"regModel2014Index\":246.7701289},{\"BLSSeriesID\":\"A423\",\"State\":\"WA\",\"blsCity\":\"Seattle\",\"regionalFactor\":-1.000,\"latitude\":47.468409,\"longitude\":-122.274661,\"GEOID\":80389,\"blsReadyMixConcID\":\"PCU327320327320\",\"blsTractorShovelLoadersID\":\"PCU33312033312014\",\"blsEnergyID\":\"CUUSA423SA0E\",\"blsFuelsUtilitiesID\":\"CUUSA423SAH2\",\"regModel2014Index\":215.314024},{\"BLSSeriesID\":\"A311\",\"State\":\"DC\",\"blsCity\":\"Washington\",\"regionalFactor\":-1.000,\"latitude\":38.897394,\"longitude\":-77.18974,\"GEOID\":92242,\"blsReadyMixConcID\":\"PCU327320327320\",\"blsTractorShovelLoadersID\":\"PCU33312033312014\",\"blsEnergyID\":\"CUUSA311SA0E\",\"blsFuelsUtilitiesID\":\"CUUSA311SAH2\",\"regModel2014Index\":183.9537619}]";
        */
        public string regDefaults = "[{\"BLSSeriesID\":\"0000\",\"State\":\"NA\",\"blsCity\":\"NATIONAL\",\"regionalFactor\":1.000,\"latitude\":0,\"longitude\":0,\"GEOID\":-1,\"blsReadyMixConcID\":\"PCU327320327320\",\"blsTractorShovelLoadersID\":\"PCU33312033312014\",\"blsEnergyID\":\"CUUS0000SA0E\",\"blsFuelsUtilitiesID\":\"CUUS0000SAH2\",\"regModel2014Index\":205.3048894,\"c0_intercept\":-19.4284,\"c1_readyMix\":0.113389,\"c2_tractorShovel\":0.325493,\"c3_energy\":0.096662,\"c4_fuelUtils\":0.398318},{\"BLSSeriesID\":\"A427\",\"State\":\"AK\",\"blsCity\":\"Anchorage\",\"regionalFactor\":1.000,\"latitude\":61.167916,\"longitude\":-149.847166,\"GEOID\":2305,\"blsReadyMixConcID\":\"PCU327320327320\",\"blsTractorShovelLoadersID\":\"PCU33312033312014\",\"blsEnergyID\":\"CUUSA427SA0E\",\"blsFuelsUtilitiesID\":\"CUUSA427SAH2\",\"regModel2014Index\":226.7835622,\"c0_intercept\":-19.4284,\"c1_readyMix\":0.113389,\"c2_tractorShovel\":0.325493,\"c3_energy\":0.096662,\"c4_fuelUtils\":0.398318},{\"BLSSeriesID\":\"A319\",\"State\":\"GA\",\"blsCity\":\"Atlanta\",\"regionalFactor\":1.000,\"latitude\":33.824102,\"longitude\":-84.331858,\"GEOID\":3817,\"blsReadyMixConcID\":\"PCU327320327320\",\"blsTractorShovelLoadersID\":\"PCU33312033312014\",\"blsEnergyID\":\"CUUSA319SA0E\",\"blsFuelsUtilitiesID\":\"CUUSA319SAH2\",\"regModel2014Index\":229.417366,\"c0_intercept\":-19.4284,\"c1_readyMix\":0.113389,\"c2_tractorShovel\":0.304932,\"c3_energy\":0.096662,\"c4_fuelUtils\":0.283199},{\"BLSSeriesID\":\"A103\",\"State\":\"MA\",\"blsCity\":\"Boston\",\"regionalFactor\":1.000,\"latitude\":42.373132,\"longitude\":-71.140708,\"GEOID\":9271,\"blsReadyMixConcID\":\"PCU327320327320\",\"blsTractorShovelLoadersID\":\"PCU33312033312014\",\"blsEnergyID\":\"CUUSA103SA0E\",\"blsFuelsUtilitiesID\":\"CUUSA103SAH2\",\"regModel2014Index\":216.5301231,\"c0_intercept\":-19.4284,\"c1_readyMix\":0.113389,\"c2_tractorShovel\":0.325493,\"c3_energy\":0.096662,\"c4_fuelUtils\":0.4592194},{\"BLSSeriesID\":\"A207\",\"State\":\"IL\",\"blsCity\":\"Chicago\",\"regionalFactor\":1.000,\"latitude\":41.827126,\"longitude\":-87.895427,\"GEOID\":16264,\"blsReadyMixConcID\":\"PCU327320327320\",\"blsTractorShovelLoadersID\":\"PCU33312033312014\",\"blsEnergyID\":\"CUUSA207SA0E\",\"blsFuelsUtilitiesID\":\"CUUSA207SAH2\",\"regModel2014Index\":193.9052141,\"c0_intercept\":-19.4284,\"c1_readyMix\":0.113389,\"c2_tractorShovel\":0.396454,\"c3_energy\":0.096662,\"c4_fuelUtils\":0.44202},{\"BLSSeriesID\":\"A213\",\"State\":\"OH\",\"blsCity\":\"Cincinnati\",\"regionalFactor\":1.000,\"latitude\":39.185505,\"longitude\":-84.462043,\"GEOID\":16885,\"blsReadyMixConcID\":\"PCU327320327320\",\"blsTractorShovelLoadersID\":\"PCU33312033312014\",\"blsEnergyID\":\"CUUSA213SA0E\",\"blsFuelsUtilitiesID\":\"CUUSA213SAH2\",\"regModel2014Index\":200.9823586,\"c0_intercept\":-19.4284,\"c1_readyMix\":0.113389,\"c2_tractorShovel\":0.325493,\"c3_energy\":0.096662,\"c4_fuelUtils\":0.398318},{\"BLSSeriesID\":\"A210\",\"State\":\"OH\",\"blsCity\":\"Cleveland\",\"regionalFactor\":1.000,\"latitude\":41.443638,\"longitude\":-81.605443,\"GEOID\":17668,\"blsReadyMixConcID\":\"PCU327320327320\",\"blsTractorShovelLoadersID\":\"PCU33312033312014\",\"blsEnergyID\":\"CUUSA210SA0E\",\"blsFuelsUtilitiesID\":\"CUUSA210SAH2\",\"regModel2014Index\":191.0175351,\"c0_intercept\":-19.4284,\"c1_readyMix\":0.113389,\"c2_tractorShovel\":0.325493,\"c3_energy\":0.096662,\"c4_fuelUtils\":0.398318},{\"BLSSeriesID\":\"A316\",\"State\":\"TX\",\"blsCity\":\"Dallas\",\"regionalFactor\":1.000,\"latitude\":36.060538,\"longitude\":-102.515287,\"GEOID\":21988,\"blsReadyMixConcID\":\"PCU327320327320\",\"blsTractorShovelLoadersID\":\"PCU33312033312014\",\"blsEnergyID\":\"CUUSA316SA0E\",\"blsFuelsUtilitiesID\":\"CUUSA316SAH2\",\"regModel2014Index\":205.7769973,\"c0_intercept\":-19.4284,\"c1_readyMix\":0.113389,\"c2_tractorShovel\":0.264,\"c3_energy\":0.096662,\"c4_fuelUtils\":0.3392},{\"BLSSeriesID\":\"A433\",\"State\":\"CO\",\"blsCity\":\"Denver\",\"regionalFactor\":1.000,\"latitude\":39.710774,\"longitude\":-104.955096,\"GEOID\":23527,\"blsReadyMixConcID\":\"PCU327320327320\",\"blsTractorShovelLoadersID\":\"PCU33312033312014\",\"blsEnergyID\":\"CUUSA433SA0E\",\"blsFuelsUtilitiesID\":\"CUUSA433SAH2\",\"regModel2014Index\":201.125973,\"c0_intercept\":-19.4284,\"c1_readyMix\":0.113389,\"c2_tractorShovel\":0.325493,\"c3_energy\":0.096662,\"c4_fuelUtils\":0.398318},{\"BLSSeriesID\":\"A208\",\"State\":\"MI\",\"blsCity\":\"Detroit\",\"regionalFactor\":1.000,\"latitude\":42.489752,\"longitude\":-83.227211,\"GEOID\":23824,\"blsReadyMixConcID\":\"PCU327320327320\",\"blsTractorShovelLoadersID\":\"PCU33312033312014\",\"blsEnergyID\":\"CUUSA208SA0E\",\"blsFuelsUtilitiesID\":\"CUUSA208SAH2\",\"regModel2014Index\":209.0531332,\"c0_intercept\":-19.4284,\"c1_readyMix\":0.113389,\"c2_tractorShovel\":0.325493,\"c3_energy\":0.096662,\"c4_fuelUtils\":0.398318},{\"BLSSeriesID\":\"A426\",\"State\":\"HI\",\"blsCity\":\"Honolulu\",\"regionalFactor\":1.000,\"latitude\":21.364556,\"longitude\":-157.939756,\"GEOID\":89770,\"blsReadyMixConcID\":\"PCU327320327320\",\"blsTractorShovelLoadersID\":\"PCU33312033312014\",\"blsEnergyID\":\"CUUSA426SA0E\",\"blsFuelsUtilitiesID\":\"CUUSA426SAH2\",\"regModel2014Index\":269.3302901,\"c0_intercept\":-19.4284,\"c1_readyMix\":0.113389,\"c2_tractorShovel\":0.325493,\"c3_energy\":0.096662,\"c4_fuelUtils\":0.398318},{\"BLSSeriesID\":\"A318\",\"State\":\"TX\",\"blsCity\":\"Houston\",\"regionalFactor\":1.000,\"latitude\":29.784308,\"longitude\":-95.393531,\"GEOID\":40429,\"blsReadyMixConcID\":\"PCU327320327320\",\"blsTractorShovelLoadersID\":\"PCU33312033312014\",\"blsEnergyID\":\"CUUSA318SA0E\",\"blsFuelsUtilitiesID\":\"CUUSA318SAH2\",\"regModel2014Index\":181.8025875,\"c0_intercept\":-19.4284,\"c1_readyMix\":0.113389,\"c2_tractorShovel\":0.325493,\"c3_energy\":0.096662,\"c4_fuelUtils\":0.398318},{\"BLSSeriesID\":\"A421\",\"State\":\"CA\",\"blsCity\":\"Los Angeles\",\"regionalFactor\":1.000,\"latitude\":33.982668,\"longitude\":-118.104332,\"GEOID\":51445,\"blsReadyMixConcID\":\"PCU327320327320\",\"blsTractorShovelLoadersID\":\"PCU33312033312014\",\"blsEnergyID\":\"CUUSA421SA0E\",\"blsFuelsUtilitiesID\":\"CUUSA421SAH2\",\"regModel2014Index\":234.2691355,\"c0_intercept\":-19.4284,\"c1_readyMix\":0.113389,\"c2_tractorShovel\":0.325493,\"c3_energy\":0.096662,\"c4_fuelUtils\":0.398318},{\"BLSSeriesID\":\"A320\",\"State\":\"FL\",\"blsCity\":\"Miami\",\"regionalFactor\":1.000,\"latitude\":26.175616,\"longitude\":-80.231428,\"GEOID\":56602,\"blsReadyMixConcID\":\"PCU327320327320\",\"blsTractorShovelLoadersID\":\"PCU33312033312014\",\"blsEnergyID\":\"CUUSA320SA0E\",\"blsFuelsUtilitiesID\":\"CUUSA320SAH2\",\"regModel2014Index\":180.237286,\"c0_intercept\":-19.4284,\"c1_readyMix\":0.113389,\"c2_tractorShovel\":0.325493,\"c3_energy\":0.096662,\"c4_fuelUtils\":0.398318},{\"BLSSeriesID\":\"A212\",\"State\":\"WI\",\"blsCity\":\"Milwaukee\",\"regionalFactor\":1.000,\"latitude\":43.055674,\"longitude\":-88.100524,\"GEOID\":57466,\"blsReadyMixConcID\":\"PCU327320327320\",\"blsTractorShovelLoadersID\":\"PCU33312033312014\",\"blsEnergyID\":\"CUUSA212SA0E\",\"blsFuelsUtilitiesID\":\"CUUSA212SAH2\",\"regModel2014Index\":201.6221956,\"c0_intercept\":-19.4284,\"c1_readyMix\":0.113389,\"c2_tractorShovel\":0.325493,\"c3_energy\":0.096662,\"c4_fuelUtils\":0.398318},{\"BLSSeriesID\":\"A211\",\"State\":\"MN\",\"blsCity\":\"Minneapolis\",\"regionalFactor\":1.000,\"latitude\":44.978156,\"longitude\":-93.2798,\"GEOID\":57628,\"blsReadyMixConcID\":\"PCU327320327320\",\"blsTractorShovelLoadersID\":\"PCU33312033312014\",\"blsEnergyID\":\"CUUSA211SA0E\",\"blsFuelsUtilitiesID\":\"CUUSA211SAH2\",\"regModel2014Index\":193.9197203,\"c0_intercept\":-19.4284,\"c1_readyMix\":0.113389,\"c2_tractorShovel\":0.357176,\"c3_energy\":0.096662,\"c4_fuelUtils\":0.421136472},{\"BLSSeriesID\":\"A101\",\"State\":\"NY\",\"blsCity\":\"New York\",\"regionalFactor\":1.000,\"latitude\":40.718357,\"longitude\":-73.970221,\"GEOID\":63217,\"blsReadyMixConcID\":\"PCU327320327320\",\"blsTractorShovelLoadersID\":\"PCU33312033312014\",\"blsEnergyID\":\"CUUSA101SA0E\",\"blsFuelsUtilitiesID\":\"CUUSA101SAH2\",\"regModel2014Index\":192.0815131,\"c0_intercept\":-19.4284,\"c1_readyMix\":0.113389,\"c2_tractorShovel\":0.4395572,\"c3_energy\":0.096662,\"c4_fuelUtils\":0.4831199},{\"BLSSeriesID\":\"A102\",\"State\":\"PA\",\"blsCity\":\"Philadelphia\",\"regionalFactor\":1.000,\"latitude\":39.973331,\"longitude\":-75.298163,\"GEOID\":69076,\"blsReadyMixConcID\":\"PCU327320327320\",\"blsTractorShovelLoadersID\":\"PCU33312033312014\",\"blsEnergyID\":\"CUUSA102SA0E\",\"blsFuelsUtilitiesID\":\"CUUSA102SAH2\",\"regModel2014Index\":196.349605,\"c0_intercept\":-19.4284,\"c1_readyMix\":0.113389,\"c2_tractorShovel\":0.40557176,\"c3_energy\":0.096662,\"c4_fuelUtils\":0.462920184},{\"BLSSeriesID\":\"A104\",\"State\":\"PA\",\"blsCity\":\"Pittsburgh\",\"regionalFactor\":1.000,\"latitude\":40.456961,\"longitude\":-79.951005,\"GEOID\":69697,\"blsReadyMixConcID\":\"PCU327320327320\",\"blsTractorShovelLoadersID\":\"PCU33312033312014\",\"blsEnergyID\":\"CUUSA104SA0E\",\"blsFuelsUtilitiesID\":\"CUUSA104SAH2\",\"regModel2014Index\":213.5812894,\"c0_intercept\":-19.4284,\"c1_readyMix\":0.113389,\"c2_tractorShovel\":0.325493,\"c3_energy\":0.096662,\"c4_fuelUtils\":0.398318},{\"BLSSeriesID\":\"A425\",\"State\":\"OR\",\"blsCity\":\"Portland\",\"regionalFactor\":1.000,\"latitude\":45.520404,\"longitude\":-122.651087,\"GEOID\":71317,\"blsReadyMixConcID\":\"PCU327320327320\",\"blsTractorShovelLoadersID\":\"PCU33312033312014\",\"blsEnergyID\":\"CUUSA425SA0E\",\"blsFuelsUtilitiesID\":\"CUUSA425SAH2\",\"regModel2014Index\":216.1105637,\"c0_intercept\":-19.4284,\"c1_readyMix\":0.113389,\"c2_tractorShovel\":0.325493,\"c3_energy\":0.096662,\"c4_fuelUtils\":0.398318},{\"BLSSeriesID\":\"A424\",\"State\":\"CA\",\"blsCity\":\"San Diego\",\"regionalFactor\":1.000,\"latitude\":32.928728,\"longitude\":-117.128799,\"GEOID\":78661,\"blsReadyMixConcID\":\"PCU327320327320\",\"blsTractorShovelLoadersID\":\"PCU33312033312014\",\"blsEnergyID\":\"CUUSA424SA0E\",\"blsFuelsUtilitiesID\":\"CUUSA424SAH2\",\"regModel2014Index\":222.5492038,\"c0_intercept\":-19.4284,\"c1_readyMix\":0.113389,\"c2_tractorShovel\":0.325493,\"c3_energy\":0.096662,\"c4_fuelUtils\":0.398318},{\"BLSSeriesID\":\"A422\",\"State\":\"CA\",\"blsCity\":\"San Francisco\",\"regionalFactor\":1.000,\"latitude\":37.690115,\"longitude\":-122.128498,\"GEOID\":78904,\"blsReadyMixConcID\":\"PCU327320327320\",\"blsTractorShovelLoadersID\":\"PCU33312033312014\",\"blsEnergyID\":\"CUUSA422SA0E\",\"blsFuelsUtilitiesID\":\"CUUSA422SAH2\",\"regModel2014Index\":246.7701289,\"c0_intercept\":-19.4284,\"c1_readyMix\":0.113389,\"c2_tractorShovel\":0.325493,\"c3_energy\":0.096662,\"c4_fuelUtils\":0.398318},{\"BLSSeriesID\":\"A423\",\"State\":\"WA\",\"blsCity\":\"Seattle\",\"regionalFactor\":1.000,\"latitude\":47.468409,\"longitude\":-122.274661,\"GEOID\":80389,\"blsReadyMixConcID\":\"PCU327320327320\",\"blsTractorShovelLoadersID\":\"PCU33312033312014\",\"blsEnergyID\":\"CUUSA423SA0E\",\"blsFuelsUtilitiesID\":\"CUUSA423SAH2\",\"regModel2014Index\":215.314024,\"c0_intercept\":-19.4284,\"c1_readyMix\":0.113389,\"c2_tractorShovel\":0.325493,\"c3_energy\":0.096662,\"c4_fuelUtils\":0.398318},{\"BLSSeriesID\":\"A311\",\"State\":\"DC\",\"blsCity\":\"Washington\",\"regionalFactor\":1.000,\"latitude\":38.897394,\"longitude\":-77.18974,\"GEOID\":92242,\"blsReadyMixConcID\":\"PCU327320327320\",\"blsTractorShovelLoadersID\":\"PCU33312033312014\",\"blsEnergyID\":\"CUUSA311SA0E\",\"blsFuelsUtilitiesID\":\"CUUSA311SAH2\",\"regModel2014Index\":183.9537619,\"c0_intercept\":-19.4284,\"c1_readyMix\":0.113389,\"c2_tractorShovel\":0.36493239,\"c3_energy\":0.096662,\"c4_fuelUtils\":0.45178684}]";
        //Classes needed to deserialze JSON data (JSON to C# website)
        public class DataObject
        {
            public string Name { get; set; }
        }

        public class Footnote
        {
        }

        public class Datum
        {
            public string year { get; set; }
            public string period { get; set; }
            public string periodName { get; set; }
            public string value { get; set; }
            public List<Footnote> footnotes { get; set; }
        }

        public class Series
        {
            public string seriesID { get; set; }
            public List<Datum> data { get; set; }
        }

        public class Results
        {
            public List<Series> series { get; set; }
        }

        public class blsResp
        {
            public string status { get; set; }
            public int responseTime { get; set; }
            public List<object> message { get; set; }
            public Results Results { get; set; }
            public override string ToString()
            {
                return string.Format("Status: {0}", status);
            }
        }
        public class SeriesPost
        {
            public string[] seriesid { get; set; }
            public string startyear { get; set; }
            public string endyear { get; set; }
            public bool catalog { get; set; }
            public bool calculations { get; set; }
            public bool annualaverage { get; set; }
            public string registrationKey { get; set; }
        }
        /**
         * Represents a BLS regional center and holds all its
         * associated data
         */
        public class BlsCenter
        {
            public const double maxValidityDistance = 100.0; // max distance from center for center to be used
            public const int positionOfNationalDefaultInList = 3; // National value is fourth item in the list (0-indexed)
            public int dataYear { get; set; }
            public double inflationFactor { get; set; }
            public double regModel2014Index { get; set; }
            public string BLSSeriesID { get; set; }
            public string State { get; set; }
            public string blsCity { get; set; }
            public double regionalFactor { get; set; }
            public double latitude { get; set; }
            public double longitude { get; set; }
            public double distToCurrentPoint { get; set; }
            public string GEOID { get; set; }
            public string blsReadyMixConcID { get; set; }
            public string blsTractorShovelLoadersID { get; set; }
            public string blsEnergyID { get; set; }
            public string blsFuelsUtilitiesID { get; set; }
            // Feb 2018 addition for city-specific model
            public double c0_intercept { get; set; }
            public double c1_readyMix { get; set; }
            public double c2_tractorShovel { get; set; }
            public double c3_energy { get; set; }
            public double c4_fuelUtils { get; set; }


            public string selectString { get; set; }//formatted string for display in comboxes
        }
        //default constructor
        public CostRegionalization() { }

        //parses minified default regionalization json data
        //and computes distances to bls regional centers
        public List<BlsCenter> parseRegDataAndComputeDist(double lat, double lng)
        {
            List<BlsCenter> BlsData = Newtonsoft.Json.JsonConvert.DeserializeObject<List<BlsCenter>>(regDefaults);
            BlsData = findNearestBLSCenter(BlsData, lng, lat);
            bool status = computeRegionalizationMult(BlsData);
            if (status == false && BlsData.Count < 3)
            {//then was unable to get bls data make only National and other avaialable
                BlsData.RemoveAt(0);
                BlsData.RemoveAt(0);
            }
            return BlsData;
        }

        //locates nearest BLS regional center bases of the coordinates of the user's
        //study area
        public List<BlsCenter> findNearestBLSCenter(List<BlsCenter> BlsCenters, double lng, double lat)
        {
            bool flag = checkIfWithinUSBounds(lng, lat);
            int count = 0;

            foreach (BlsCenter BlsCity in BlsCenters)
            {
                count += 1;

                if (BlsCity.blsCity == "NATIONAL")
                {
                    BlsCity.distToCurrentPoint = 999999;
                    BlsCity.selectString = String.Format("{0} ({1}) {2}", BlsCity.blsCity, "NA", BlsCity.regionalFactor);
                }
                else
                {
                    BlsCity.distToCurrentPoint = distanceTo(lat, lng, BlsCity.latitude, BlsCity.longitude, "M");
                    BlsCity.selectString = String.Format("{0} ({1:F0} miles) {2}", BlsCity.blsCity, BlsCity.distToCurrentPoint, BlsCity.regionalFactor);
                }

                System.Console.WriteLine("Element #{0}: {1}", count, BlsCity.blsCity);
            }
            //sort the list by distance
            var sortedList = BlsCenters.OrderBy(o => o.distToCurrentPoint);
            var shortenedList = sortedList.Take(shortListCount).ToList();
            shortenedList.Add(sortedList.Last());
            shortenedList.Add(new BlsCenter()
            {
                BLSSeriesID = "Other",
                State = "Other",
                blsCity = "Other",
                regionalFactor = 1,
                latitude = 0,
                longitude = 0,
                distToCurrentPoint = 999999,
                GEOID = "Other",
                blsReadyMixConcID = "Other",
                blsTractorShovelLoadersID = "Other",
                blsEnergyID = "Other",
                blsFuelsUtilitiesID = "Other",
                selectString = "Other (NA) 1",
                inflationFactor = 1
            });
            return shortenedList;//BlsCenters.OrderBy(o => o.distToCurrentPoint).ToList();
        }

        // computes reginal multipliers for a list of BLS centers
        public bool computeRegionalizationMult(List<BlsCenter> BlsCenters)
        {
            int count = 0;
            int currentYear = DateTime.Now.Year;
            int currentMonth = DateTime.Now.Month;
            int dataYear = currentYear - 1;

            bool status = true;

            //bls data is updated between the 15th and 20th of the month as of this writting so assume previous years data available in Feb
            if (currentMonth < 2)
            {
                dataYear = dataYear - 1;
            }

            foreach (BlsCenter BlsCity in BlsCenters)
            {
                count += 1;
                BlsCity.dataYear = dataYear;
                if (count > shortListCount + 1) break; //do not compute for National and other cause default multipliers are always 1 however need to compute for National so can calc inflation
                List<string> seriesIDs = new List<string>();
                seriesIDs.Add(BlsCity.blsReadyMixConcID);
                seriesIDs.Add(BlsCity.blsTractorShovelLoadersID);
                seriesIDs.Add(BlsCity.blsEnergyID);
                seriesIDs.Add(BlsCity.blsFuelsUtilitiesID);
                status = status && getBLSData(BlsCity, seriesIDs.ToArray(), dataYear.ToString(), dataYear.ToString());
                Console.WriteLine("Element #{0}: {1} {2} {3} {4} {5}", count, BlsCity.blsCity, BlsCity.blsReadyMixConcID, BlsCity.blsTractorShovelLoadersID, BlsCity.blsEnergyID, BlsCity.blsFuelsUtilitiesID);
            }
            return status;
        }

        //computes the distance between two locations specified by lat / lon coordinates
        public double distanceTo(double lat1, double lon1, double lat2, double lon2, string unit)
        {
            double rlat1 = Math.PI * lat1 / 180;
            double rlat2 = Math.PI * lat2 / 180;
            double theta = lon1 - lon2;
            double rtheta = Math.PI * theta / 180;
            double dist = Math.Sin(rlat1) * Math.Sin(rlat2) + Math.Cos(rlat1) * Math.Cos(rlat2) * Math.Cos(rtheta);
            dist = Math.Acos(dist);
            dist = dist * 180 / Math.PI;
            dist = dist * 60 * 1.1515;
            switch (unit)
            {
                case "K": //Kilometers
                    return dist * 1.609344;
                case "N": //Nautical Miles 
                    return dist * 0.8684;
                case "M": //Miles
                    return dist;
            }
            return dist;
        }

        //checks if supplied coordinates are within the bounds of the United States
        public bool checkIfWithinUSBounds(double lng, double lat)
        {
            //source # http://en.wikipedia.org/wiki/Extreme_points_of_the_United_States#Westernmost
            //[top,left,right,bottom]
            double[] bounds = { 49.3457868, -124.7844079, -66.9513812, 24.7433195 };
            if (lat < bounds[3] || lat > bounds[0] || lng < bounds[1] || lng > bounds[2])
            {
                return false;
            }
            return true;
        }

        //provides BLS series ID for variables used to compute regionalization multipliers
        public string getSeriesIDMapping(string seriesID)
        {
            string result = "";
            var seriesIDsMap = new Dictionary<string, string>
            {
                { "PCU327320327320", "blsReadyMixConcID" },
                { "PCU33312033312014", "blsTractorShovelLoadersID" },
                { "SA0E", "blsEnergyID" },
                { "SAH2", "blsFuelsUtilitiesID" }
            };
            foreach (var pair in seriesIDsMap)
            {
                if (seriesID.Contains(pair.Key))
                {
                    result = pair.Value;
                    return result;
                }
            }
            return result;
        }

        //supervises computation of regional multipliers for a BLS regional center based on a regression
        //equation previously determined by an external model
        public void computeAndSaveRegMult(BlsCenter BlsCity, List<Series> blsSeriesArr)
        {
            string[] validSeriesIDs = { "blsReadyMixConcID", "blsTractorShovelLoadersID", "blsEnergyID", "blsFuelsUtilitiesID" };
            // var seriesLabelIDFrags = new List<string> { "PCU327320327320","PCU33312033312014","SA0E","SAH2" };
            var seriesIDMap = "";
            var blsVarDict = new Dictionary<string, double>();
            if (blsSeriesArr != null)
            {
                foreach (Series serie in blsSeriesArr)
                {
                    seriesIDMap = getSeriesIDMapping(serie.seriesID);
                    foreach (var data in serie.data)
                    {
                        if (data.periodName == "Annual")
                        {
                            blsVarDict.Add(seriesIDMap, Convert.ToDouble(data.value));
                            Console.WriteLine("BLS Series #{0}: year:{1} periodName:{2} value:{3} saved for {4}", serie.seriesID, data.year, data.periodName, data.value, seriesIDMap);
                            break;
                        }
                    }
                }
            }
            //perform regionalization calcs
            /* Feb 2018 replaced with city-specific model
             * BlsCity.regionalFactor = this.c0_intercept +
                (this.c1_readyMix * blsVarDict[validSeriesIDs[0]]) +
                (this.c2_tractorShovel * blsVarDict[validSeriesIDs[1]]) +
                (this.c3_energy * blsVarDict[validSeriesIDs[2]]) +
                (this.c4_fuelUtils * blsVarDict[validSeriesIDs[3]]);*/

            //Feb 2018 city specific model
            BlsCity.regionalFactor = BlsCity.c0_intercept +
            (BlsCity.c1_readyMix * blsVarDict[validSeriesIDs[0]]) +
            (BlsCity.c2_tractorShovel * blsVarDict[validSeriesIDs[1]]) +
            (BlsCity.c3_energy * blsVarDict[validSeriesIDs[2]]) +
            (BlsCity.c4_fuelUtils * blsVarDict[validSeriesIDs[3]]);

            BlsCity.inflationFactor = Math.Round(BlsCity.regionalFactor / BlsCity.regModel2014Index, dcp);
            BlsCity.regionalFactor = Math.Round(BlsCity.regionalFactor / default2015NationalIndex, dcp);
            BlsCity.selectString = String.Format("{0} ({1:F0} miles) {2}", BlsCity.blsCity, BlsCity.distToCurrentPoint, BlsCity.regionalFactor);

        }

        //retrieves data via the BLS api given a valid BLS regional center, a series id and start/end year
        public bool getBLSData(BlsCenter BlsCity, string[] seriesIDs, string startyear, string endyear)
        {
            var httpWebRequest = (HttpWebRequest)WebRequest.Create("https://api.bls.gov/publicAPI/v2/timeseries/data/");
            httpWebRequest.ContentType = "application/json";
            httpWebRequest.Method = "POST";

            using (var streamWriter = new StreamWriter(httpWebRequest.GetRequestStream()))
            {
                string newJson = Newtonsoft.Json.JsonConvert.SerializeObject(new SeriesPost()
                {
                    seriesid = seriesIDs,//(new List<string>() { "CUUR0000SA0" }).ToArray(),
                    startyear = startyear,
                    endyear = endyear,
                    catalog = false,
                    calculations = false,
                    annualaverage = true,
                    registrationKey = "cecffbde84de416dbe4d198437f7c03e"

                });

                //TODO comment out for production So you can see the JSON thats output
                //System.Diagnostics.Debug.WriteLine(newJson);

                streamWriter.Write(newJson);
                streamWriter.Flush();
                streamWriter.Close();
            }
            var httpResponse = (HttpWebResponse)httpWebRequest.GetResponse();
            using (var streamReader = new StreamReader(httpResponse.GetResponseStream()))
            {
                var result = streamReader.ReadToEnd();
                blsResp obj = Newtonsoft.Json.JsonConvert.DeserializeObject<blsResp>(result);
                //check to see if success response received
                if (obj.ToString() == "Status: REQUEST_SUCCEEDED")
                {
                    computeAndSaveRegMult(BlsCity, obj.Results.series);

                }
                //Console.ReadLine();
            }
            return true;
        }
    }
}
