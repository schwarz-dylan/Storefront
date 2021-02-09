using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Storefront.UI.MVC.Models;
using System.Net;
using System.Net.Mail;

namespace Storefront.UI.MVC.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Shop()
        {
            

            return View();
        }

        public ActionResult ProductDetail()
        {
            

            return View();
        }

        public ActionResult Cart()
        {


            return View();
        }

        public ActionResult Contact()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Contact(ContactViewModel cvm)
        {
            //When a class has validation attributes, that validation should be chacked BEFORE attempting to process any data
            if (!ModelState.IsValid)
            {
                //send them back to the form, passing their inputs back to form with the HTML form.
                return View(cvm);//the cvm object in this return populates the form with what the user inputted the first time

            }//end if
            //Build the message - what we see when we recieve the email
            string message = $"You have recieved an email from {cvm.Name} with a subject of {cvm.Subject}. Please respond to {cvm.Email} with your response to the following message: <br/>{cvm.Message}";

            //MailMessage object (What send the email from an ASP.NET application - ADD USING STATEMENT FOR SYSTEM.NET.MAIL
            MailMessage mm = new MailMessage(
                //FROM
                "admin@dylanschwarz.com",
                //TO - this assumes forwarding from the host
                "schwarz_dylan@yahoo.com",//hardcoded forward to this email address
                                          //SUBJECT
                cvm.Subject,
                //BODY
                message
                );

            //MialMessage object properties
            //Allow for Html formatting in the email message
            mm.IsBodyHtml = true;
            //if you want to mark these emails with high priority
            mm.Priority = MailPriority.High; //the deafult enum is normal
            //respond to the sender's email instead of our own SmarterAsp email
            mm.ReplyToList.Add(cvm.Email);

            //StmpClient - This is the information from the host(smarterASP.NET) that allows the email to actually be sent.
            SmtpClient client = new SmtpClient("mail.dylanschwarz.com");

            //client Credentials
            client.Credentials = new NetworkCredential("admin@dylanschwarz.com", "Christmas0101!");

            //It is possible that the mailserver is down or we may have configurations issues, so we want to encapsulate our code in a "try/catch"
            try
            {
                //attempt to send the email
                client.Send(mm);
            }//end try
            catch (Exception ex)
            {
                ViewBag.CustomerMessage = $"We're sorry your request could not be sent at this time. Please try again later.<br/>Error Message:<br/>{ex.StackTrace}";
                return View(cvm);//this returns the view with the entire message so that users can copy and paste it for later.
            }//end catch

            //If all goes well, we will return the user to a view that confirms their message has been sent.
            return View("EmailConfirmation", cvm);





        }//end action result



    }//end class
}//end NS