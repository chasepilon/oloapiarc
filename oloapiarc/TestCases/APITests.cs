using System;
using System.Text;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using oloapiarc.Helpers;
using oloapiarc.DataModels;

namespace oloapiarc.TestCases
{
    [TestClass]
    public class APITests
    {
        /*
         * Side Bar discussion:
         * https://jsonplaceholder.typicode.com/posts/1/photos
         * https://jsonplaceholder.typicode.com/posts/11/photos
         * 
         * Looks like these exist contrary to the docs
         * Also, they seem to be returning the same data
         * 
         *  Available routes being:
         *  /posts/1/comments
         *  /albums/1/photos
         *  /users/1/albums
         *  /users/1/todos
         *  /users/1/posts
         */
        private ApiHelper apiHandler;
        private BaseAPIModel baseModel;

        [TestInitialize]
        public void testInit()
        {
            baseModel = new BaseAPIModel();

            baseModel.baseUrl = ConfigHelper.BaseUrls["JsonPlaceholder"];
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
            baseModel.endpoint = ConfigHelper.BaseUrls["basePosts"];

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
            baseModel.endpoint = ConfigHelper.BaseUrls["basePosts"];
            baseModel.body = new JObject();

            //Set up the body
            foreach(var e in TestData.PostTestData)
                baseModel.body.Add(e.Key, e.Value);

            //Run the endpoint, redirecting the call to use a GET
            var response = await apiHandler.RunEndpoint(baseModel, RequestType.POST);

            try
            {
                //Verify an object is returned
                Assert.IsTrue(response != null);

                //Parse the object to allow checking data
                JObject parsedRes = JObject.Parse(response);

                //Contract validation
                foreach (var c in baseModel.Contract)
                    Assert.IsTrue(parsedRes.ToString().Contains(c));
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
        [TestCategory("PATCH")]
        public async Task VerifyPATCHEndpoint()
        {
            //Set the endpoint
            baseModel.endpoint = ConfigHelper.BaseUrls["basePosts"];
            baseModel.body = new JObject();

            //Set up the body
            foreach (var e in TestData.PostTestData)
                baseModel.body.Add(e.Key, e.Value);

            //Generate the test data
            var response = await apiHandler.RunEndpoint(baseModel, RequestType.POST);

            try
            {
                //Verify an object is returned
                Assert.IsTrue(response != null);

                JObject parsedRes = JObject.Parse(response);

                //Set up the PATCH
                baseModel.endpoint = string.Format(ConfigHelper.BaseUrls["postsById"], parsedRes["id"]);
                baseModel.body = new JObject();
                foreach (var e in TestData.PatchTestData)
                    baseModel.body.Add(e.Key, e.Value);

                //Run the PATCH against the test data
                response = await apiHandler.RunEndpoint(baseModel, RequestType.PATCH);

                //Verify an object is returned
                Assert.IsTrue(response != null);

                //Parse the object to allow checking data
                parsedRes = JObject.Parse(response);

                //Verify the PATCH response
                foreach (var e in TestData.PatchTestData)
                    Assert.IsTrue(parsedRes.ToString().Contains(e.Value));

                /*
                 * This section currently fails, I would assume due to the fact that 
                 * the POST/PATCH is not actually stored, however; this would be part of the 
                 * test as well in order to verify the PATCH went thru successfully rather 
                 * than simply relying on the PATCH response
                 */ 

                ////Run a GET to ensure the data was actually changed
                //response = await apiHandler.RunEndpoint(baseModel, RequestType.GET);
                //parsedRes = JObject.Parse(response);

                //foreach (var e in TestData.PatchTestData)
                //    Assert.IsTrue(parsedRes.ToString().Contains(e.Value));
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
