﻿using System;
using System.Text;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Newtonsoft.Json.Linq;
using oloapiarc.HelperFiles;
using oloapiarc.DataModels;

namespace oloapiarc.TestCases
{
    [TestClass]
    public class APITests
    {
        private ApiHelper apiHandler;
        private BaseAPIModel baseModel;

        [TestInitialize]
        public void testInit()
        {
            baseModel = new BaseAPIModel();

            baseModel.baseUrl = "https://jsonplaceholder.typicode.com/";
            apiHandler = new ApiHelper(baseModel.baseUrl);
        }

        [TestCleanup]
        public void testCleanUp()
        {
            apiHandler.Disconnect();
        }

        [TestMethod]
        [TestCategory("GET")]
        public async Task VerifyGenericGETPostsEndpointContract()
        {
            //Set the endpoint
            baseModel.endpoint = "posts";

            //Run the endpoint, redirecting the call to use a GET
            var response = await apiHandler.RunEndpoint(baseModel, RequestType.GET);

            try
            {
                //Verify an object is returned
                Assert.IsTrue(response != null);

                //Parse the object into an array to allow checking data
                JArray parsedRes = JArray.Parse(response);

                //Contract validation
                foreach (var c in baseModel.Contract)
                    Assert.IsTrue(parsedRes[0].ToString().Contains(c));
            }
            catch (System.NullReferenceException err)
            {
                throw err;
            }
            catch (System.ArgumentOutOfRangeException)
            {
                Assert.IsTrue(true);
            }
            finally
            {
                apiHandler.Disconnect();
            }
        }

        [TestMethod]
        [TestCategory("POST")]
        public async Task VerifyGenericPOSTEndpoint()
        {
            //Set the endpoint
            baseModel.endpoint = "posts";
            //baseModel.body = new JObject();

            //Run the endpoint, redirecting the call to use a GET
            var response = await apiHandler.RunEndpoint(baseModel, RequestType.POST);

            try
            {
                //Verify an object is returned
                Assert.IsTrue(response != null);

                //Parse the object into an array to allow checking data
                JArray parsedRes = JArray.Parse(response);

                //Contract validation
                foreach (var c in baseModel.Contract)
                    Assert.IsTrue(parsedRes[0].ToString().Contains(c));
            }
            catch (System.NullReferenceException err)
            {
                throw err;
            }
            catch (System.ArgumentOutOfRangeException)
            {
                Assert.IsTrue(true);
            }
            finally
            {
                apiHandler.Disconnect();
            }
        }
    }
}
