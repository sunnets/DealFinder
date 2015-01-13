﻿using System;
using System.Collections.Generic;
using Amazon.SimpleEmail;
using Amazon.SimpleEmail.Model;

namespace AmazonSESSample
{
    class Mail
    {
        public static void sendMail(String BODY)
        {
            const String FROM = "mehrakv@gmail.com";  // Replace with your "From" address. This address must be verified.
            const String TO = "mehrakv@gmail.com"; // Replace with a "To" address. If you have not yet requested
            // production access, this address must be verified.

            const String SUBJECT = "Amazon SES test (AWS SDK for .NET)";
           // const String BODY = "This email was sent through Amazon SES by using the AWS SDK for .NET.";

            // Construct an object to contain the recipient address.
            Destination destination = new Destination();
            destination.ToAddresses = (new List<string>() { TO });

            // Create the subject and body of the message.
            Content subject = new Content(SUBJECT);
            Content textBody = new Content(BODY);
            Body body = new Body(textBody);

            // Create a message with the specified subject and body.
            Message message = new Message(subject, body);

            // Assemble the email.
            SendEmailRequest request = new SendEmailRequest(FROM, destination, message);

            // Choose the AWS region of the Amazon SES endpoint you want to connect to. Note that your production 
            // access status, sending limits, and Amazon SES identity-related settings are specific to a given 
            // AWS region, so be sure to select an AWS region in which you set up Amazon SES. Here, we are using 
            // the US East (N. Virginia) region. Examples of other regions that Amazon SES supports are USWest2 
            // and EUWest1. For a complete list, see http://docs.aws.amazon.com/ses/latest/DeveloperGuide/regions.html 
            Amazon.RegionEndpoint REGION = Amazon.RegionEndpoint.USWest2;

            // Instantiate an Amazon SES client, which will make the service call.
            AmazonSimpleEmailServiceClient client = new AmazonSimpleEmailServiceClient(REGION);

            // Send the email.
            try
            {
                Console.WriteLine("Attempting to send an email through Amazon SES by using the AWS SDK for .NET...");
                client.SendEmail(request);
                Console.WriteLine("Email sent!");
            }
            catch (Exception ex)
            {
                Console.WriteLine("The email was not sent.");
                Console.WriteLine("Error message: " + ex.Message);
            }

          //  Console.Write("Press any key to continue...");
          //  Console.ReadKey();
        }
    }
}
