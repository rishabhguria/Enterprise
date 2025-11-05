package com.nirvana.Email;
import java.io.FileOutputStream;
import java.io.IOException;
import java.nio.file.Files;
import java.nio.file.Paths;

import org.apache.commons.mail.*;



public class email {
	public static void main(String[] args) throws EmailException, IOException {
//	Email email = new SimpleEmail();
//	email.setHostName("smtp.googlemail.com");
//	email.setSmtpPort(465);
//	email.setAuthenticator(new DefaultAuthenticator("reportsvalidator1@nirvanasolutions.com", "UATPassword96"));
//	email.setSSLOnConnect(true);
//	email.setFrom("reportsvalidator1@nirvanasolutions.com");
//	email.setSubject("TestMail");
//	email.setMsg("This is a test mail ... :-)");
//	email.addTo("reportsvalidator1@nirvanasolutions.com");
//	email.send();

		
		String data = "";
	    data = new String(Files.readAllBytes(Paths.get("C:\\Users\\shivam.tanwar\\eclipse-workspace\\BatchRun\\log\\application.html")));
		
		
		
	HtmlEmail email = new HtmlEmail();
	email.setHostName("smtp.googlemail.com");
	email.setSmtpPort(465);
	email.setAuthenticator(new DefaultAuthenticator("reportsvalidator1@nirvanasolutions.com", "UATPassword96"));
	email.setSSLOnConnect(true);
	email.setFrom("reportsvalidator1@nirvanasolutions.com");
	email.setSubject("Chart");
	email.setHtmlMsg(data);

	email.addTo("shivam.tanwar@nirvanasolutions.com");
	  
	  // set the alternative message
	 // email.setTextMsg("Your email client does not support HTML messages");

	  // send the email
	  email.send();
	
	  //com.nirvana.Helper.HelperClass.PrintConsole("=====Email Sent=====");



	  

	}

	  
	  

}
