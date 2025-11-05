package prana.ruleEngineMediator.ruleService;

import java.lang.reflect.Method;
import java.math.BigDecimal;
import java.text.DecimalFormat;
import java.text.NumberFormat;
import java.text.ParseException;
import java.text.SimpleDateFormat;
import java.util.ArrayList;
import java.util.Arrays;
import java.util.LinkedHashSet;
import java.util.List;

import javax.script.ScriptEngine;
import javax.script.ScriptEngineManager;
import org.drools.base.ClassFieldReader;
import org.drools.common.PropagationContextImpl;
import org.drools.event.rule.ObjectInsertedEvent;
import org.drools.event.rule.ObjectRetractedEvent;
import org.drools.event.rule.ObjectUpdatedEvent;
import org.drools.event.rule.WorkingMemoryEventListener;
import org.drools.rule.Pattern;
import org.drools.rule.Rule;
import org.drools.rule.RuleConditionElement;
import org.drools.rule.constraint.EvaluatorConstraint;
import org.drools.rule.constraint.MvelConstraint;
import org.drools.spi.Constraint;

import prana.businessObjects.complianceLevel.Alert;
import prana.ruleEngineMediator.constants.ConfigurationConstants;
import prana.utility.configuration.ConfigurationHelper;
import prana.utility.logging.PranaLogManager;

public class WorkingMemoryEventListnerImp implements WorkingMemoryEventListener {

	// Number formatting for current parameters to international standards.
	NumberFormat fm1 = new DecimalFormat(",##0.00##");
	SimpleDateFormat dateFormat = new SimpleDateFormat("MM/dd/yyyy");
	private boolean validDate = false;
	private boolean isNum = false;

	public WorkingMemoryEventListnerImp() {

	}

	@Override
	public void objectInserted(ObjectInsertedEvent event) {
		try {
			boolean isAllowedMargeInParameter = Boolean.parseBoolean(ConfigurationHelper.getInstance()
					.getValueBySectionAndKey(ConfigurationConstants.SECTION_APP_SETTINGS,
							ConfigurationConstants.KEY_APP_SETTINGS_IS_ALLOWED_MARGE_IN_PARAMETER));
			if (event.getObject().getClass().getName()
					.equals(prana.businessObjects.complianceLevel.Alert.class.getName())) {

				if (((Alert) event.getObject()).getViolated()) {
					PropagationContextImpl propagation = (PropagationContextImpl) event.getPropagationContext();

					Object ComplianceObj = propagation.getLeftTupleOrigin().getHandle().getObject();

					((Alert) event.getObject()).setName(event.getPropagationContext().getRule().getName());
					// ((Alert)
					// event.getObject()).setUUID(event.getPropagationContext().getRule().getId());
					ArrayList<String> expression = new ArrayList<String>();
					StringBuilder ruleSummary = new StringBuilder();
					StringBuilder curParameters = new StringBuilder();
					ArrayList<String> constraintFieldsList = new ArrayList<String>();
					ArrayList<String> thresholdList = new ArrayList<String>();
					ArrayList<String> actualResultList = new ArrayList<String>();
					// formatting string converting scientific number to double
					curParameters.append(prana.utility.misc.Misc
							.formatStringParameters(((Alert) event.getObject()).getParameters().toString()));

					if (isAllowedMargeInParameter && curParameters.length() > 0)
						curParameters.append(", ");
					ruleSummary.append("If ");

					Rule rule = (Rule) propagation.getRule();

					List<RuleConditionElement> ruleConditions = rule.getLhs().getChildren();

					for (RuleConditionElement ruleCondition : ruleConditions) {

						Pattern pattern = (Pattern) ruleCondition;

						List<Constraint> constrains = pattern.getConstraints();

						for (Constraint constrain : constrains) {
							if (constrain instanceof MvelConstraint) {
								MvelConstraint mConstraint = (MvelConstraint) constrain;
								ruleSummary.append(Utilities.formatExpression(mConstraint.getExpression()));

								String field = "";
								String constraintField ="";
								String[] constraint = mConstraint.toString().split("\\|\\|\\s");
								//if rule is creates using Multiple field constraint as any of (or).
								if(constraint.length>1) 
								{
									ArrayList<ArrayList<String>> finalList = MultipleFieldConstraint(constraint, ComplianceObj);
									for (int i = 0; i < finalList.get(0).size(); i++)
                                    {
                                        constraintFieldsList.add(finalList.get(0).get(i));
                                        thresholdList.add(finalList.get(1).get(i));
                                        actualResultList.add(finalList.get(2).get(i));
                                    }
								}
								else 
								{
									constraint = mConstraint.toString().split(" ");
									constraintField = constraint[0];
									constraintField = constraintField.toUpperCase().charAt(0)
											+ constraintField.substring(1, constraintField.length());
									constraintFieldsList.add(constraintField);
									
									//if rule is created normally without any formula
									if (mConstraint.getField() != null) {
										if ((mConstraint.getField().getValue() instanceof Double
												|| mConstraint.getField().getValue() instanceof BigDecimal)) {
											field = fm1.format(mConstraint.getField().getValue());
										} else {
											if (mConstraint.getField().getValue() == null)
												field = (String) mConstraint.getField().getValue();
											else
												field = mConstraint.getField().getValue().toString();
										}
									 }
									else
									{
										// if rule has any field who's threshold is set to formula
										String className = ComplianceObj.getClass().getName();
										List<String> expressionList = new ArrayList<String>(Arrays.asList(constraint));
										field = getThresholdValue(ComplianceObj, expressionList, className);
										if (field.equals("")) {
											String[] f = mConstraint.getExpression().split("==");
											field = f[1].trim();
											String expr = f[0].trim();
											actualResultList.add(getActualResultValue(ComplianceObj, expr, className));
										}
									}
									thresholdList.add(field);
								}

								ruleSummary.append(", ");

								ClassFieldReader fieldsReader = (ClassFieldReader) mConstraint.getFieldExtractor();
								if (fieldsReader != null) {

									String fieldName = fieldsReader.getFieldName();
									if (isAllowedMargeInParameter) {
										curParameters.append(
												getFieldValue(ComplianceObj, fieldName, fieldsReader.getClassName()));
									}

									actualResultList.add(getActualResultValue(ComplianceObj, fieldName,
											fieldsReader.getClassName()));

									if (!expression.contains(fieldsReader.getClassName()))
										expression.add(fieldsReader.getClassName());
									expression.add(mConstraint.getExpression());

								} else {
									String className = ComplianceObj.getClass().getName();
									if (!expression.contains(className)) {
										expression.add(className);
										/*
										 * String tempparameter = updateCurrentParameters( expression, ComplianceObj,
										 * false); curParameters.append(tempparameter); curParameters.delete(0, 2);
										 * curParameters.append(", "); expression.add(tempparameter .toLowerCase());
										 */
									}
									String expr = mConstraint.getExpression();// getting complete expression of rule
									String[] multiExpr = expr.split("\\|\\|\\s");// splitting expression to take out
																					// parameter fields
									for (int i = 0; i < multiExpr.length; i++) {
										expr = multiExpr[i].substring(0, multiExpr[i].indexOf(" "));
										if (isAllowedMargeInParameter && !(curParameters.toString()).toLowerCase()
												.contains(expr.toLowerCase()))
											curParameters.append(getFieldValue(ComplianceObj, expr, className));// adding
																												// fields
																												// and
																												// values
																												// in
																												// parameters
									}
									expression.add(mConstraint.getExpression());
									//actualResultList.add(getActualResultValue(ComplianceObj, expr, className));
								}
							} else if (constrain instanceof EvaluatorConstraint) {
								// works for sound like no solution found
							}

						}

					}

					String constraintFields = "";
					for (int i = 0; i < constraintFieldsList.size(); i++)
						constraintFields += constraintFieldsList.get(i) + ConfigurationConstants.KEY_VALUE_SEPARATOR;
					if (constraintFields.length() > 0)
						constraintFields = constraintFields.substring(0, constraintFields.length() - 1);
					((Alert) event.getObject()).setConstraintFields(constraintFields);

					String threshold = "";
					for (int i = 0; i < thresholdList.size(); i++)
						threshold += thresholdList.get(i) + ConfigurationConstants.KEY_VALUE_SEPARATOR;

					if (threshold.length() > 0)
						threshold = threshold.substring(0, threshold.length() - 1);
					((Alert) event.getObject()).setThreshold(threshold);

					String actualResult = "";
					for (int i = 0; i < actualResultList.size(); i++)
						actualResult += actualResultList.get(i) + ConfigurationConstants.KEY_VALUE_SEPARATOR;
					if (actualResult.length() > 0)
						actualResult = actualResult.substring(0, actualResult.length() - 1);
					((Alert) event.getObject()).setActualResult(actualResult);

					String summary = ruleSummary.substring(0, ruleSummary.length() - 1);

					String vhost = ConfigurationHelper.getInstance().getValueBySectionAndKey(
							ConfigurationConstants.SECTION_APP_SETTINGS,
							ConfigurationConstants.KEY_APP_SETTINGS_AMQP_VHOST);
					if (event.getPropagationContext().getRule().getPackageName()
							.equals(ConfigurationConstants.PRE_TRADE_COMPLIANCE + "_" + vhost))
						summary = summary + " Then block the trade.";
					else
						summary = summary + " Then Limit breached.";

					((Alert) event.getObject()).setDescription(summary);
					if (isAllowedMargeInParameter && curParameters.length() >= 2)
						curParameters.delete(curParameters.length() - 2, curParameters.length());
					if (isAllowedMargeInParameter)
						curParameters.append(updateCurrentParameters(expression, ComplianceObj, false));
					if (curParameters.length() > 0) {
						StringBuilder paraBuilder = new StringBuilder();
						String sub = curParameters.substring(0, curParameters.length());
						LinkedHashSet<String> hashset = new LinkedHashSet<String>(Arrays.asList(sub.split(", ")));
						for (String n : hashset) {
							paraBuilder.append(n).append(", ");
						}

						((Alert) event.getObject()).setParameters(paraBuilder.substring(0, paraBuilder.length() - 2));
					}
					String dimension = updateCurrentParameters(expression, ComplianceObj, true);

					if (dimension != null && !dimension.equals("")) {
						((Alert) event.getObject()).setDimension(dimension.substring(1));
					} else {
						((Alert) event.getObject()).setDimension("Global");
					}

				}
			}
		} catch (Exception ex) {
			PranaLogManager.error(ex);
		}

	}

	/*
	 * Fetch values for repective fields.
	 */
	private String getFieldValue(Object complianceObj, String fieldName, String className) {
		try {
			String field = getActualResultValue(complianceObj, fieldName, className);
			StringBuilder curParameters = new StringBuilder();
			if (curParameters.indexOf(fieldName) == -1) {
				curParameters.append(fieldName + ": " + field);
				curParameters.append(", ");
			}
			return curParameters.toString();
		} catch (Exception ex) {
			PranaLogManager.error(ex);
		}
		return "";
	}

	/*
	 * Fetch values for ActualResult value.
	 */
	@SuppressWarnings({ "rawtypes", "unchecked" })
	private String getActualResultValue(Object complianceObj, String fieldName, String className) {
		try {
			Class cls = Class.forName(className);
			Object obj = cls.newInstance();
			obj = complianceObj;
			Class noparams[] = {};
			fieldName = Character.toString(fieldName.charAt(0)).toUpperCase() + fieldName.substring(1);
			className = className.substring(38);
			Method method = cls.getDeclaredMethod("get" + fieldName, noparams);
			Object fieldValue = method.invoke(obj, new Object[] {});

			String field = "";
			if (fieldValue instanceof Double) {
				field = fm1.format(fieldValue);
			} else if (fieldValue instanceof BigDecimal) {
				field = fm1.format(fieldValue);
			} else {
				field = fieldValue.toString();
			}

			return field.toString();
		} catch (Exception ex) {
			PranaLogManager.error(ex);
		}
		return "";
	}

	// @SuppressWarnings({ "rawtypes", "unchecked" })
	private String updateCurrentParameters(ArrayList<String> expression, Object complianceObj,
			Boolean isOnlyDimensionRequired) {
		try {
			String className = expression.get(0);
			className = className.substring(38);
			Class<?> cls = Class.forName(expression.get(0));
			Class<?> noparams[] = {};
			ArrayList<String> fieldName = new ArrayList<String>();
			// Method method;
			// Object fieldValue;
			StringBuilder currentParameter = new StringBuilder();
			switch (className) {

			case "Account_Symbol":
				fieldName.add("accountShortName");
				fieldName.add("symbol");
				break;
			case "Symbol":
				fieldName.add("symbol");
				break;
			case "MasterFund_Symbol":
				fieldName.add("masterFundName");
				fieldName.add("symbol");
				break;
			case "Asset":
				fieldName.add("asset");
				break;
			case "Account_UnderlyingSymbol":
				fieldName.add("accountShortName");
				fieldName.add("underlyingSymbol");
				break;
			case "Account":
				fieldName.add("accountShortName");
				break;
			case "MasterFund_UnderlyingSymbol":
				fieldName.add("masterFundName");
				fieldName.add("underlyingSymbol");
				break;
			case "MasterFund":
				fieldName.add("masterFundName");
				break;
			case "Sector":
				fieldName.add("sector");
				break;
			case "SubSector":
				fieldName.add("subSector");
				break;
			case "UnderlyingSymbol":
				fieldName.add("underlyingSymbol");
				break;
			case "Trade":
				fieldName.add("symbol");
				break;
			}
			Boolean contains = false;
			for (String fields : fieldName) {

				String field = Character.toUpperCase(fields.charAt(0)) + fields.substring(1);

				if (isOnlyDimensionRequired) {
					Method method = cls.getDeclaredMethod("get" + field, noparams);
					Object fieldValue = method.invoke(complianceObj, new Object[] {});
					currentParameter.append("-" + fieldValue.toString().trim());

				} else {
					for (int i = 0; i < expression.size(); i++)
						if (expression.get(i).contains(fields)) {
							contains = true;
							break;
						}
					if (!contains) {
						Method method = cls.getDeclaredMethod("get" + field, noparams);
						Object fieldValue = method.invoke(complianceObj, new Object[] {});
						currentParameter.append(", " + field + ": " + fieldValue);

					}
					contains = false;
				}
				// String field=Character.toUpperCase(fields.charAt(0))
				// + fields.substring(1);

			}
			return currentParameter.toString();

		} catch (Exception ex) {
			PranaLogManager.error(ex);
			return "";
		}

	}
	
	/* This method calculated the threshold value when the rule is created using a formula */
	@SuppressWarnings({ "rawtypes", "unchecked" })
	private String getThresholdValue(Object complianceObj, List expression, String className) {
		try {
			Class cls = Class.forName(className);
			Object obj = cls.newInstance();
			obj = complianceObj;
			Class noparams[] = {};
			className = className.substring(38);
			
			//to remove staring operator and its left side value and brackets at last
			for(int i=0 ;i<3; i++)
				expression.remove(0);
			if(expression.size()>0)
				expression.remove(expression.size()-1);
	        
	        StringBuilder expr = new StringBuilder(expression.toString());
	        
	        //get the list of all the fields used in formula and the Expression string
	        ArrayList<String> allFields = splitString(expr);
	        String finalString = allFields.get(allFields.size()-1);
	        
	        String fieldName = "";
	        for(int i=0 ; i < allFields.size()-1; i++)
	        {
				try {
					fieldName = Character.toString(allFields.get(i).charAt(0)).toUpperCase()
							+ allFields.get(i).substring(1);
					Method method = cls.getDeclaredMethod("get" + fieldName, noparams);
					Object fieldValue = method.invoke(obj, new Object[] {});
					finalString = finalString.replaceAll(allFields.get(i), fieldValue.toString());
				} catch (Exception ex) {
					return "";
				}
			}

	        //calculation of the final expression received.
			ScriptEngineManager mgr = new ScriptEngineManager();
		    ScriptEngine engine = mgr.getEngineByName("JavaScript");
		    String foo = finalString.toString();
		    Object finalResult = engine.eval(foo);
		    
			String field = "";
			if (finalResult != null) {
				if (finalResult instanceof Double) {
					field = fm1.format(finalResult);
				} else if (finalResult instanceof BigDecimal) {
					field = fm1.format(finalResult);
				} else {
					field = finalResult.toString();
				}
			}

			return field.toString();
		} catch (Exception ex) {
			PranaLogManager.error(ex);
		}
		return "";
	}
	
	// To split the string and get the field names and final expression from string received.
	public static ArrayList<String> splitString(StringBuilder str)
	{
		try {
			
			StringBuffer alpha = new StringBuffer();
			ArrayList<String> listTocheck = new ArrayList<String>();
			ArrayList<String> newListTocheck = new ArrayList<String>();

			for (int i = 0; i < str.length(); i++) {
				if (str.charAt(i) == '.') {
					if (Character.isAlphabetic(str.charAt(i - 1)) && Character.isAlphabetic(str.charAt(i + 1))) {
						str.setCharAt(i - 1, '@');
						str.setCharAt(i, '@');
						alpha.delete(0, alpha.length());
					}
				} else if (Character.isAlphabetic(str.charAt(i)))
					alpha.append(str.charAt(i));
				else {
					if (alpha.length() > 1 && !listTocheck.contains(alpha.toString()))
						listTocheck.add(alpha.toString());
					alpha.delete(0, alpha.length());
				}
			}
			String newExpr = str.toString().replaceAll("@", "");
			newExpr = newExpr.replace(",", "");
			newExpr = newExpr.replace("[", "");
			newExpr = newExpr.replace("]", "");
			newExpr = newExpr.replaceAll("\\s", "");

			// To check if a field is split into two names for avgVolume20Days and
			// beta5YearMonthly.
			if (listTocheck.size() >= 2) {
				String newField = "";
				for (int i = 0; i < listTocheck.size() - 1; i++) {
					if (listTocheck.get(i).equalsIgnoreCase("avgVolume")
							&& listTocheck.get(i + 1).equalsIgnoreCase("Days")) {
						newField = "avgVolume20Days";
						newListTocheck.add(newField);
						i++;
					} else if (listTocheck.get(i).equalsIgnoreCase("beta")
							&& listTocheck.get(i + 1).equalsIgnoreCase("YearMonthly")) {
						newField = "beta5YearMonthly";
						newListTocheck.add(newField);
						i++;
					} else {
						newListTocheck.add(listTocheck.get(i));
					}

				}
			} else if (listTocheck.size() == 1) {
				newListTocheck.add(listTocheck.get(0));
			}
			newListTocheck.add(newExpr);
			return newListTocheck;
		}
		catch (Exception ex) {
			PranaLogManager.error(ex);
		}
		return null;
	}
	
	/*This method is called when the user select [any of (or)]as multiple field constraint in a rule
    The method is gets string with multiple expressions separated by OR operator
	It separates the expression calculates the Threshold and actual and compare both to each other and return 
	actual result , threshold and constraint fields list.*/
	private ArrayList<ArrayList<String>> MultipleFieldConstraint(String[] exprList , Object compObj)
	{
		try {
			ArrayList<String> updatedConstraints = new ArrayList<String>();
			ArrayList<String> updatedActualResult = new ArrayList<String>();
			ArrayList<String> updatedThreshold = new ArrayList<String>();
			ArrayList<ArrayList<String>> finalList = new ArrayList<ArrayList<String>>(3);
			
			String className = compObj.getClass().getName();
			
			String thresStr="";
			String actualStr ="";
			String fieldStr="";
			int k= 0;
			String[] expression;
			List<String> expressionList= new ArrayList<String>();
			
			for(int i =0; i < exprList.length; i++) {
				
				expression = exprList[i].split(" ");
				actualStr = getActualResultValue(compObj, expression[0], className);
				fieldStr = expression[0].substring(0, 1).toUpperCase() + expression[0].substring(1);
				thresStr = expression[2];
				
				// goes into if, if the threshold value is set as formula.
				if(expression[2].contains("("))
				{
						expressionList = new ArrayList<String>(Arrays.asList(exprList[i].split(" ")));
						thresStr = getThresholdValue(compObj, expressionList, className);
						
						updatedConstraints.add(
								RelationalOperation(thresStr, 
										            actualStr ,
										            expression[1] , 
										            fieldStr
										            )
								);
						
				}// goes into else if, if the threshold value is set as String.
				else if(expression[2].startsWith("\""))
				{
					String[] splitStr = exprList[i].split("[!=~=<==><=>=]+");
					splitStr[1] = splitStr[1].substring(2, splitStr[1].length() - 2);
					thresStr = splitStr[1];
					
					updatedConstraints.add(
							RelationalOperation(thresStr, 
									            actualStr,
									            expression[1] , 
									            fieldStr 
									            )
							);
					
				}
				else // goes into else, if the threshold value is set as numeric.
				{
					updatedConstraints.add(
							RelationalOperation(thresStr, 
									            actualStr,
									            expression[1] , 
									            fieldStr
									            )
							);
				}
				
				//update constraint, actual result and threshold list for the expression from which the rule is breached
				if(updatedConstraints.get(k) != null )
				{
					k=k+1;
					updatedActualResult.add(actualStr);
					updatedThreshold.add(thresStr);
				}
				else
				{
					updatedConstraints.remove(k);
				}
			}
			finalList.add(updatedConstraints);
			finalList.add(updatedThreshold);
			finalList.add(updatedActualResult);
			
			return finalList;
			
		}
		catch (Exception ex) {
			PranaLogManager.error(ex);
		}
		return null;
	}
	
	/*Receives the threshold, actual, field and operator as string and the compares the actual and threshold 
	  values to each other with the help of operator */
	private String RelationalOperation(String thresStr, String actStr , String operator, String fieldName)
	{
		try {
			validDate = false;
			isNum = false;
			if(isNumeric(thresStr) && isNumeric(actStr))
			{
				thresStr = thresStr.replaceAll("," , "");
				actStr = actStr.replaceAll("," , "");
				isNum = true;
			}
			else if(isDate(thresStr) && isDate(actStr))
			{
				thresStr = dateFormat.format(dateFormat.parse(thresStr));
				actStr = dateFormat.format(dateFormat.parse(actStr));
				validDate= true;
			}

			switch(operator) {
			
			case "==":
				if(isNum)
				{
					if(Double.parseDouble(actStr) == Double.parseDouble(thresStr))
						return fieldName;
				}
				else
					if(actStr == thresStr)
						return fieldName;
				break;
			
			case "!=":
				if(isNum)
				{
					if(Double.parseDouble(actStr) != Double.parseDouble(thresStr))
						return fieldName;
				}
				else
					if(actStr != thresStr)
						return fieldName;
				break;
				
			case "<":
				if(isNum)
				{
					if(Double.parseDouble(actStr) < Double.parseDouble(thresStr))
						return fieldName;
				}
				else if(validDate)
				{		
					if(actStr.compareTo(thresStr) < 0)
						return fieldName;
				}
				break;
				
			case ">":
				if(isNum)
				{
					if(Double.parseDouble(actStr) > Double.parseDouble(thresStr))
						return fieldName;
				}
				else if(validDate)
				{
					if(actStr.compareTo(thresStr) > 0)
						return fieldName;
				}
				break;
				
			case "<=":
				if(isNum)
				{
					if(Double.parseDouble(actStr) <= Double.parseDouble(thresStr))
						return fieldName;
				}
				else if(validDate)
				{
					if(actStr.compareTo(thresStr) < 0 || actStr.compareTo(thresStr) == 0)
						return fieldName;
				}
				break;
				
			case ">=":
				if(isNum)
				{
					if(Double.parseDouble(actStr) >= Double.parseDouble(thresStr))
						return fieldName;
				}
				else if(validDate)
				{
					if(actStr.compareTo(thresStr) > 0 || actStr.compareTo(thresStr) == 0)
						return fieldName;
				}
				break;
			
			case "~=":
				if(actStr.matches(thresStr))
					return fieldName;
				break;
			}
		}
		catch (Exception ex) {
			PranaLogManager.error(ex);
		}
		return null;
	}
	
	//Check if the string received is numeric.
	public static boolean isNumeric(String strNum) {
	    if (strNum == null) {
	        return false;
	    }
	    try {
	    	strNum = strNum.replaceAll("," , "");
	        Double.parseDouble(strNum);
	    } catch (NumberFormatException nfe) {
	        return false;
	    }
	    return true;
	}
	
	//Check if the string received is convertible to date.
	public boolean isDate(String strDate) {
	    if (strDate == null) {
	        return false;
	    }
        dateFormat.setLenient(false);
        try {
        	dateFormat.parse(strDate.trim()); 
        } catch (ParseException e) {
            return false;
        }
        return true;
	}
	@Override
	public void objectUpdated(ObjectUpdatedEvent event) {
		// TODO Auto-generated method stub

	}

	@Override
	public void objectRetracted(ObjectRetractedEvent event) {
		// TODO Auto-generated method stub

	}

}
