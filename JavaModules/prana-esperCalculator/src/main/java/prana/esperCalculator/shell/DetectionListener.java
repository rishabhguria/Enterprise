package prana.esperCalculator.shell;

import java.util.Calendar;
import java.util.Date;
import java.util.HashMap;

import org.apache.commons.mail.DefaultAuthenticator;
import org.apache.commons.mail.Email;
import org.apache.commons.mail.EmailException;
import org.apache.commons.mail.SimpleEmail;

import prana.businessObjects.interfaces.IDisposable;
import prana.esperCalculator.communication.DataInitializationRequestProcessor;
import prana.esperCalculator.constants.ConfigurationConstants;
import prana.esperCalculator.commonCode.CEPManager;
import prana.utility.configuration.ConfigurationHelper;
import prana.utility.logging.PranaLogManager;
import prana.utility.objectMapper.JSONMapper;

import com.espertech.esper.common.client.EventBean;
import com.espertech.esper.runtime.client.EPRuntime;
import com.espertech.esper.runtime.client.EPRuntimeDestroyedException;
import com.espertech.esper.runtime.client.EPStatement;
import com.espertech.esper.runtime.client.UpdateListener;

public class DetectionListener implements UpdateListener, IDisposable {

	private static Object _lockerObject = new Object();
	private static Date _lastDumpTime = Calendar.getInstance().getTime();
	private static String _intervalTime = "";
	private static int _dumpIntervalForDetection = 0;

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

	HashMap<String, String> _headers;

	public DetectionListener() {

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

			if (_dumpIntervalForDetection == 0) {
				_dumpIntervalForDetection = Integer.parseInt(ConfigurationHelper.getInstance().getValueBySectionAndKey(
						ConfigurationConstants.SECTION_APP_SETTINGS,
						ConfigurationConstants.KEY_APP_SETTINGS_DUMP_INTERVAL_FOR_DETECTION)) * 1000;
			}
			_headers = new HashMap<>();
			_headers.put("X-Priority", "1");
			_lastDumpTime.setTime(_lastDumpTime.getTime() - 300000);
		} catch (Exception ex) {
			PranaLogManager.error(ex);
		}
	}

	@Override
	public void update(EventBean[] newEvents, EventBean[] oldEvents, EPStatement statement, EPRuntime runtime) {
		try {
			int i = 1;
			if (newEvents != null) {
				for (EventBean eventBean : newEvents) {
					try {
						StringBuilder builder = new StringBuilder();
						builder.append(eventBean.getEventType().getName());
						builder.append("\n" + "TriggerID: " + i++ + "/" + newEvents.length + "\n");
						builder.append(JSONMapper.getStringForObject(eventBean.getUnderlying()));
						PranaLogManager.warn(builder.toString());
						dumpDataForAnalysis();
						String compressionLevel = eventBean.get("compressionLevel").toString();
						boolean result = invokeAutoCorrection();
						sendMail(builder.toString(), result, compressionLevel);
					} catch (EPRuntimeDestroyedException ex) {
						PranaLogManager.error(ex);
					} catch (Exception ex) {
						PranaLogManager.error(ex);
					}
				}
			}
		} catch (Exception ex) {
			PranaLogManager.error(ex);
		}
	}

	private void dumpDataForAnalysis() {
		try {
			synchronized (_lockerObject) {
				if (_lastDumpTime.getTime() + _dumpIntervalForDetection < Calendar.getInstance().getTime().getTime()) {
					_lastDumpTime = Calendar.getInstance().getTime();
					PranaLogManager.info("Dumping data for analysis");
					// Starting a dump that can not be cancelled as dump initiated by detection listener is for analysis purpose
					WindowDumpHandler.dump(ShellManager.getInstance().getTokenizedCommand(new String[] { "dump" }),
							false);
					WindowDumpHandler.zipLastDump();
				} else {
					PranaLogManager.info("Dump already taken in last 5 minutes or already in progress");
				}
			}
		} catch (Exception ex) {
			PranaLogManager.info("Could not dump data");
			PranaLogManager.error(ex);
		}
	}

	private boolean invokeAutoCorrection() {
		try {
			// TODO This method just send the refresh request to trade server to initiate the refresh although if any what if is stuck it needs to be checked manually
			PranaLogManager.info("Initializing refresh as error on esper.");
			return DataInitializationRequestProcessor.getInstance().refreshData();
		} catch (Exception ex) {
			PranaLogManager.info("Could not invoke auto-correction");
			PranaLogManager.error(ex);
			return false;
		}
	}

	private void setDetectionIntervalToString() {
		int detectionIntervalinSec = Integer.parseInt(CEPManager.getVariableValue("DetectionFrequency").toString())
				* 3;
		if (detectionIntervalinSec < 60)
			_intervalTime = detectionIntervalinSec + " sec";
		else {
			if (detectionIntervalinSec % 60 == 0) {
				_intervalTime = detectionIntervalinSec / 60 + " minutes";
			} else
				_intervalTime = detectionIntervalinSec / 60 + " minutes " + detectionIntervalinSec % 60 + " sec";
		}
	}

	private void sendMail(String string, boolean autoCorrectionResult, String compressionLevel) {
		try {
			if (_recieverTo.length == 0 || (_recieverTo.length == 1 && _recieverTo[0].equals("")))
				return;

			if (_intervalTime == null || _intervalTime.equals(""))
				setDetectionIntervalToString();

			String autoCorrectionResultMessage;
			if (autoCorrectionResult)
				autoCorrectionResultMessage = "Auto-correction initiated by invoking refresh on esper.\n"
						+ "If you have received this mail more than once in last " + _intervalTime
						+ ", please restart esper.";
			else
				autoCorrectionResultMessage = "Auto-correction initiated by invoking refresh on esper "
						+ "\nbut FAILED to initiate. \nPlease restart esper.";
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

			if (autoCorrectionResult)
				email.setSubject("Problem on esper - calculation error @" + compressionLevel + " on " + _vhost
						+ " - Autocorrection initiated");
			else
				email.setSubject("Problem on esper - calculation error @ " + _vhost + " Autocorrection FAILED");
			email.setMsg("This detection has been run only on some fields. "
					+ "There could be problem on some other fields as well. " + "Data has been dump for analysis.\n\n"
					+ string + "\n\n" + autoCorrectionResultMessage + "\n\n(Only applicable for post trade)");

			email.send();
		} catch (EmailException ex) {
			PranaLogManager.info("Could not send mail");
			PranaLogManager.error(ex);
		} catch (Exception ex) {
			PranaLogManager.info("Could not send mail");
			PranaLogManager.error(ex);
		}
	}

	@Override
	public void disposeListener() {
		// TODO Auto-generated method stub

	}
}
