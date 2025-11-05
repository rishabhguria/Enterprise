package prana.utility.fileIO;

import java.io.File;
import java.io.FileOutputStream;
import java.io.InputStream;
import java.io.StringWriter;

import javax.xml.transform.OutputKeys;
import javax.xml.transform.Transformer;
import javax.xml.transform.TransformerConfigurationException;
import javax.xml.transform.TransformerException;
import javax.xml.transform.TransformerFactory;
import javax.xml.transform.TransformerFactoryConfigurationError;
import javax.xml.transform.dom.DOMSource;
import javax.xml.transform.stream.StreamResult;

import org.w3c.dom.Document;

import prana.utility.logging.PranaLogManager;

public class FileHelper {

	public static String createDirectory(String directoryPath,
			String packageName, String ruleName, String ruleCategory)
			throws Exception {
		File ruleDir;
		try {
			File packageDir = new File(directoryPath + "/" + packageName);
			if (!packageDir.exists()) {
				packageDir.mkdir();
			}
			ruleDir = new File(packageDir.getAbsoluteFile() + "/" + ruleName
					+ "." + ruleCategory);
			if (!ruleDir.exists()) {
				ruleDir.mkdir();
			}
		} catch (Exception ex) {
			PranaLogManager.error(ex.getMessage(), ex);
			throw ex;
		}
		return ruleDir.getCanonicalPath();
	}

	public static void writeStreamToFile(InputStream inputStream,
			String directoryPath, String ruleName, String ruleCategory)
			throws Exception {
		try {
			java.io.OutputStream outputStream = new FileOutputStream(new File(
					directoryPath + "/" + ruleName + ".xml"));

			int read = 0;
			byte[] bytes = new byte[1024];

			while ((read = inputStream.read(bytes)) != -1) {
				outputStream.write(bytes, 0, read);
			}

			if (inputStream != null) {
				try {
					inputStream.close();
				} catch (Exception e) {
					e.printStackTrace();
				}
			}

			if (outputStream != null) {
				try {
					// outputStream.flush();
					outputStream.close();
				} catch (Exception e) {
					e.printStackTrace();
				}

			}
		} catch (Exception ex) {
			PranaLogManager.error(ex.getMessage(), ex);
			throw ex;
		}

	}

	public static void writeXmlDocToFile(Document doc, String path) {
		try {
			TransformerFactory transformerFactory = TransformerFactory
					.newInstance();
			Transformer transformer = transformerFactory.newTransformer();
			DOMSource source = new DOMSource(doc);

			StreamResult result = new StreamResult(new StringWriter());

			// t.setParameter(OutputKeys.INDENT, "yes");
			transformer.setOutputProperty(OutputKeys.INDENT, "yes");
			transformer.setOutputProperty(
					"{http://xml.apache.org/xslt}indent-amount", "5");
			transformer.setOutputProperty(OutputKeys.ENCODING, "ISO-8859-1");
			transformer.transform(source, result);

			// writing to file
			FileOutputStream fop = null;
			File file;
			try {

				file = new File(path);
				fop = new FileOutputStream(file);

				// if file doesnt exists, then create it
				if (!file.exists()) {
					file.createNewFile();
				}

				// get the content in bytes
				String xmlString = result.getWriter().toString();
				byte[] contentInBytes = xmlString.getBytes();

				fop.write(contentInBytes);
				fop.flush();
				fop.close();
			} catch (Exception e) {
				e.printStackTrace();
			} finally {
				try {
					if (fop != null) {
						fop.close();
					}
				} catch (Exception e) {
					e.printStackTrace();
				}
			}
		} catch (TransformerConfigurationException ex) {
			PranaLogManager.error(ex);

		} catch (IllegalArgumentException ex) {
			PranaLogManager.error(ex.getMessage(), ex);
			throw ex;
		} catch (TransformerFactoryConfigurationError ex) {
			PranaLogManager.error(ex.getMessage(), ex);
			throw ex;
		} catch (TransformerException ex) {
			PranaLogManager.error(ex);

		} catch (Exception ex) {
			PranaLogManager.error(ex.getMessage(), ex);
			throw ex;
		}

	}

}
