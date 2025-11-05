package prana.esperCalculator.main;

import java.lang.Thread.UncaughtExceptionHandler;
import java.util.HashMap;

import org.apache.commons.mail.DefaultAuthenticator;
import org.apache.commons.mail.Email;
import org.apache.commons.mail.EmailException;
import org.apache.commons.mail.SimpleEmail;

import prana.esperCalculator.constants.ConfigurationConstants;
import prana.utility.configuration.ConfigurationHelper;
import prana.utility.logging.PranaLogManager;

public class DefaultUncaughtExceptionHandler implements UncaughtExceptionHandler {
	private String _hostName;
	private int _smtpPort;
	private String _userName;
	private String _password;
	private String _senderName;
	private String[] _recieverTo;
	private String[] _recieverCC;
	private String[] _recieverBCC;
	private String _bounceAddress;
	private String _vhost;
	private HashMap<String, String> _headers;

	public DefaultUncaughtExceptionHandler() throws Exception {

		try {
			_hostName = ConfigurationHelper.getInstance().getValueBySectionAndKey(
					ConfigurationConstants.SECTION_APP_SETTINGS,
					ConfigurationConstants.KEY_APP_SETTINGS_MAIL_HOST_NAME);
			_smtpPort = Integer.parseInt(ConfigurationHelper.getInstance().getValueBySectionAndKey(
					ConfigurationConstants.SECTION_APP_SETTINGS,
					ConfigurationConstants.KEY_APP_SETTINGS_MAIL_SMTP_PORT));
			_userName = ConfigurationHelper.getInstance().getValueBySectionAndKey(
					ConfigurationConstants.SECTION_APP_SETTINGS,
					ConfigurationConstants.KEY_APP_SETTINGS_MAIL_USER_NAME);
			_password = ConfigurationHelper.getInstance().getValueBySectionAndKey(
					ConfigurationConstants.SECTION_APP_SETTINGS, ConfigurationConstants.KEY_APP_SETTINGS_MAIL_PASSWORD);
			_senderName = ConfigurationHelper.getInstance().getValueBySectionAndKey(
					ConfigurationConstants.SECTION_APP_SETTINGS,
					ConfigurationConstants.KEY_APP_SETTINGS_MAIL_SENDER_NAME);
			_recieverTo = ConfigurationHelper.getInstance()
					.getValueBySectionAndKey(ConfigurationConstants.SECTION_APP_SETTINGS,
							ConfigurationConstants.KEY_APP_SETTINGS_MAIL_RECIEVER_TO)
					.split("/");
			_recieverBCC = ConfigurationHelper.getInstance()
					.getValueBySectionAndKey(ConfigurationConstants.SECTION_APP_SETTINGS,
							ConfigurationConstants.KEY_APP_SETTINGS_MAIL_RECIEVER_BCC)
					.split("/");
			_recieverCC = ConfigurationHelper.getInstance()
					.getValueBySectionAndKey(ConfigurationConstants.SECTION_APP_SETTINGS,
							ConfigurationConstants.KEY_APP_SETTINGS_MAIL_RECIEVER_CC)
					.split("/");
			_bounceAddress = ConfigurationHelper.getInstance().getValueBySectionAndKey(
					ConfigurationConstants.SECTION_APP_SETTINGS,
					ConfigurationConstants.KEY_APP_SETTINGS_MAIL_BOUNCE_ADDRESS);
			_vhost = ConfigurationHelper.getInstance().getValueBySectionAndKey(
					ConfigurationConstants.SECTION_APP_SETTINGS, ConfigurationConstants.KEY_APP_SETTINGS_AMQP_VHOST);
		} catch (Exception ex) {
			PranaLogManager.error(ex.getMessage(), ex);
			throw ex;
		}
	}

	@Override
	public void uncaughtException(Thread thread, Throwable exception) {
		// TODO Auto-generated method stub
		try {
			PranaLogManager.error("Thread stopped. ThreadId: " + thread.getId() + ", ThreadName:" + thread.getName(),
					exception);
			Email mail = new SimpleEmail();
			Email email = new SimpleEmail();
			email.setHostName(_hostName);
			email.setSmtpPort(_smtpPort);
			email.setAuthenticator(new DefaultAuthenticator(_userName, _password));
			email.setFrom(_userName, _senderName);
			email.setHeaders(_headers);
			email.setBounceAddress(_bounceAddress);

			for (String to : _recieverTo) {
				if (!to.equals(""))
					email.addTo(to);
			}

			for (String cc : _recieverCC) {
				if (!cc.equals(""))
					email.addCc(cc);
			}

			for (String bcc : _recieverBCC) {
				if (!bcc.equals(""))
					email.addBcc(bcc);
			}
			mail.setSubject("Unhandled exception on Esper @" + _vhost);
			mail.setMsg("Unhandled exception has been caught and might need to restart the esper.");
			mail.send();
		} catch (EmailException ex) {
			PranaLogManager.error(ex);
		}
	}
}