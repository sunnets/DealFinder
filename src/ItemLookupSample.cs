/**********************************************************************************************
 * Copyright 2009 Amazon.com, Inc. or its affiliates. All Rights Reserved.
 *
 * Licensed under the Apache License, Version 2.0 (the "License"). You may not use this file 
 * except in compliance with the License. A copy of the License is located at
 *
 *       http://aws.amazon.com/apache2.0/
 *
 * or in the "LICENSE.txt" file accompanying this file. This file is distributed on an "AS IS"
 * BASIS, WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied. See the
 * License for the specific language governing permissions and limitations under the License. 
 *
 * ********************************************************************************************
 *
 *  Amazon Product Advertising API
 *  Signed Requests Sample Code
 *
 *  API Version: 2009-03-31
 *
 */

using System;
using System.Collections.Generic;
using System.Text;
using System.Net;
using System.IO;
using System.Xml;
using System.Xml.XPath;
using AmazonSESSample;

namespace AmazonProductAdvtApi
{
    class ItemLookupSample
    {
        private const string MY_AWS_ACCESS_KEY_ID = "";
        private const string MY_AWS_SECRET_KEY = "";
        private const string DESTINATION          = "ecs.amazonaws.com";
        private const string ASSOCIATE_TAG = "wwwproductwat-20";

        private const string NAMESPACE = "http://webservices.amazon.com/AWSECommerceService/2011-08-01";
        private const string ITEM_ID = "B004RRC7YY";
        //private const string ITEM_ID = "B0007X74D6";

        public static void Main()
        {
            SignedRequestHelper helper = new SignedRequestHelper(MY_AWS_ACCESS_KEY_ID, MY_AWS_SECRET_KEY, DESTINATION, ASSOCIATE_TAG);

            /*
             * The helper supports two forms of requests - dictionary form and query string form.
             */
            String requestUrl;
            String title;
            String price;

            /*
             * Here is an ItemLookup example where the request is stored as a dictionary.
             */
            IDictionary<string, string> r1 = new Dictionary<string, String>();
            r1["Service"] = "ecs.amazonaws.com";
            r1["Version"] = "2009-03-31";
            r1["Operation"] = "ItemLookup";
            r1["ItemId"] = ITEM_ID;
            r1["ResponseGroup"] = "Small";
            /* Random params for testing */
            r1["AnUrl"] = "http://www.amazon.com/books";
            r1["AnEmailAddress"] = "foobar@nowhere.com";
            r1["AUnicodeString"] = "αβγδεٵٶٷٸٹٺチャーハン叉焼";
            r1["Latin1Chars"] = "ĀāĂăĄąĆćĈĉĊċČčĎďĐđĒēĔĕĖėĘęĚěĜĝĞğĠġĢģĤĥĦħĨĩĪīĬĭĮįİıĲĳ";

            requestUrl = helper.Sign(r1);
            title = FetchTitle(requestUrl);

            String body = "";
            System.Console.WriteLine("Method 1: ItemLookup Dictionary form.");
            body = body + "Title is \"" + title + "\"";
            System.Console.WriteLine("Title is \"" + title + "\"");
            body = body + "Title is \"" + title + "\"";
            System.Console.WriteLine();

            
            r1["ResponseGroup"] = "OfferSummary";
            requestUrl = helper.Sign(r1);
            price = FetchPrice(requestUrl);


            System.Console.WriteLine("Method 1: ItemLookup Dictionary form.");
            System.Console.WriteLine("Price is \"" + price + "\"");
            body = body + "Price is \"" + price + "\"";
            System.Console.WriteLine();

            Mail.sendMail(body);
           
            /*
             * Here is an example where the request is stored as a query-string:
             */

            /*
             * string requestString = "Service=AWSECommerceService&Version=2009-03-31&Operation=ItemLookup&ResponseGroup=Small&ItemId=" + ITEM_ID;
             */
            System.Console.WriteLine("Method 2: Query String form.");

            String[] Keywords = new String[] {
                "Cerave",
                "Cleanser",
                "CeraVe Hydrating Cleanser"
            };

            foreach (String keyword in Keywords)
            {
                String requestString = "Service=AWSECommerceService" 
                    + "&Version=2009-03-31"
                    + "&Operation=ItemSearch"
                    + "&SearchIndex=Beauty"
                    + "&ResponseGroup=Small"
                    + "&Keywords=" + keyword
                    ;
                requestUrl = helper.Sign(requestString);
                title = FetchTitle(requestUrl);



                requestString = "Service=AWSECommerceService"
                    + "&Version=2009-03-31"
                    + "&Operation=ItemSearch"
                    + "&SearchIndex=Beauty"
                    + "&ResponseGroup=OfferSummary"
                    + "&Keywords=" + keyword
                    ;
                requestUrl = helper.Sign(requestString);

                price = FetchPrice(requestUrl);

                System.Console.WriteLine("Keyword=\"" + keyword + "\"; Title=\"" + title + "\"; Price=\"" + price + "\"");
                System.Console.WriteLine();
            }


        //    System.Console.WriteLine("Hit Enter to end");
       //     System.Console.ReadLine();
        }
        

        private static string FetchTitle(string url)
        {
            try
            {
                WebRequest request = HttpWebRequest.Create(url);
                WebResponse response = request.GetResponse();
                XmlDocument doc = new XmlDocument();
                doc.Load(response.GetResponseStream());

                XmlNodeList errorMessageNodes = doc.GetElementsByTagName("Message", NAMESPACE);
                if (errorMessageNodes != null && errorMessageNodes.Count > 0)
                {
                    String message = errorMessageNodes.Item(0).InnerText;
                    return "Error: " + message + " (but signature worked)";
                }

                XmlNode titleNode = doc.GetElementsByTagName("Title", NAMESPACE).Item(0);
                string title = titleNode.InnerText;
                return title;
            }
            catch (Exception e)
            {
                System.Console.WriteLine("Caught Exception: " + e.Message);
                System.Console.WriteLine("Stack Trace: " + e.StackTrace);
            }

            return null;
        }

        private static string FetchPrice(string url)
        {
            try
            {
                WebRequest request = HttpWebRequest.Create(url);
                WebResponse response = request.GetResponse();
                XmlDocument doc = new XmlDocument();
                doc.Load(response.GetResponseStream());

                XmlNodeList errorMessageNodes = doc.GetElementsByTagName("Message", NAMESPACE);
                if (errorMessageNodes != null && errorMessageNodes.Count > 0)
                {
                    String message = errorMessageNodes.Item(0).InnerText;
                    return "Error: " + message + " (but signature worked)";
                }

                XmlNode priceNode = doc.GetElementsByTagName("LowestNewPrice", NAMESPACE).Item(0)["FormattedPrice"];
                string price = priceNode.InnerText;
                return price;
            }
            catch (Exception e)
            {
                System.Console.WriteLine("Caught Exception: " + e.Message);
                System.Console.WriteLine("Stack Trace: " + e.StackTrace);
            }

            return null;
        }
    }
}
