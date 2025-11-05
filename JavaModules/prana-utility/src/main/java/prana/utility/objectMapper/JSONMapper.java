package prana.utility.objectMapper;

import java.text.SimpleDateFormat;
import java.util.ArrayList;
import java.util.HashMap;
import java.util.LinkedHashMap;
import java.util.List;

import prana.utility.logging.PranaLogManager;

import com.fasterxml.jackson.core.JsonGenerator;
import com.fasterxml.jackson.core.JsonParser.Feature;
import com.fasterxml.jackson.core.JsonProcessingException;
import com.fasterxml.jackson.core.type.TypeReference;
import com.fasterxml.jackson.databind.DeserializationFeature;
import com.fasterxml.jackson.databind.MapperFeature;
import com.fasterxml.jackson.databind.ObjectMapper;
import com.fasterxml.jackson.databind.SerializationFeature;

public class JSONMapper {

	/**
	 * Jackson object mapper instance
	 */
	private static ObjectMapper _mapper;

	/**
	 * Type reference used to convert json string into LinkedHashMap
	 */
	private static TypeReference<LinkedHashMap<String, Object>> _typeReferenceLinkedHashMap = new TypeReference<LinkedHashMap<String, Object>>() {
	};
	
	/**
	 * Type reference used to convert json string into LinkedHashMap
	 */
	private static TypeReference<List<HashMap<String, Object>>> _typeReferenceListOfLinkedHashMap = new TypeReference<List<HashMap<String, Object>>>() {
	};

	/**
	 * Type reference used to convert json string into HashMap
	 */
	private static TypeReference<HashMap<String, Object>> _typeReferenceHashMap = new TypeReference<HashMap<String, Object>>() {
	};

	/**
	 * Configure the Object mapper instance
	 * 
	 * @throws Exception
	 */
	public static void initializeMapperForEsperCalculator(String dateFormat)
			throws Exception {

		try {
			_mapper = new ObjectMapper();
			_mapper.configure(
					DeserializationFeature.FAIL_ON_UNKNOWN_PROPERTIES, false);
			_mapper.setDateFormat(new SimpleDateFormat(dateFormat));
			_mapper.enable(DeserializationFeature.EAGER_DESERIALIZER_FETCH);
			_mapper.configure(SerializationFeature.INDENT_OUTPUT, true);
			_mapper.configure(SerializationFeature.WRAP_ROOT_VALUE, true);
			_mapper.configure(SerializationFeature.FAIL_ON_EMPTY_BEANS, false);
			_mapper.configure(MapperFeature.DEFAULT_VIEW_INCLUSION, false);
			_mapper.configure(MapperFeature.USE_STATIC_TYPING, false);
			_mapper.configure(Feature.ALLOW_NON_NUMERIC_NUMBERS, true);
			_mapper.configure(JsonGenerator.Feature.WRITE_BIGDECIMAL_AS_PLAIN,
					true);
		} catch (Exception ex) {
			PranaLogManager.error(ex.getMessage(), ex);
			throw ex;
		}

	}

	/**
	 * Configure the Object mapper instance
	 * 
	 * @throws Exception
	 */
	public static void initializeMapperForRuleMediator(String dateFormat)
			throws Exception {

		try {
			_mapper = new ObjectMapper();
			_mapper.configure(
					DeserializationFeature.FAIL_ON_UNKNOWN_PROPERTIES, false);
			_mapper.setDateFormat(new SimpleDateFormat(dateFormat));
			_mapper.enable(DeserializationFeature.EAGER_DESERIALIZER_FETCH);
			_mapper.configure(SerializationFeature.INDENT_OUTPUT, true);
			_mapper.configure(SerializationFeature.WRAP_ROOT_VALUE, true);
			_mapper.configure(DeserializationFeature.UNWRAP_ROOT_VALUE, true);
			_mapper.configure(SerializationFeature.FAIL_ON_EMPTY_BEANS, false);
			_mapper.configure(MapperFeature.DEFAULT_VIEW_INCLUSION, false);
			_mapper.configure(MapperFeature.USE_STATIC_TYPING, false);
			_mapper.configure(JsonGenerator.Feature.WRITE_BIGDECIMAL_AS_PLAIN,
					true);
		} catch (Exception ex) {
			PranaLogManager.error(ex.getMessage(), ex);
			throw ex;
		}

	}

	/**
	 * Returns JSON string for given object
	 * 
	 * @param obj
	 * @return
	 */
	public static String getStringForObject(Object obj) {
		try {
			return _mapper.writeValueAsString(obj);
		} catch (JsonProcessingException ex) {
			PranaLogManager.error("JSON Parsing exception", ex);
			return "";
		} catch (Exception ex) {
			PranaLogManager.error(ex);
			return "";
		}
	}

	
	
	
	/**
	 * Returns LinkedHashMap for given JSON String
	 * 
	 * @param jsonString
	 * @return
	 */
	public static LinkedHashMap<String, Object> getLinkedHashMap(
			String jsonString) {
		try {
			return _mapper.readValue(jsonString, _typeReferenceLinkedHashMap);
		} catch (JsonProcessingException ex) {
			PranaLogManager.error(ex,"JSON Parsing exception");
			return null;
		} catch (Exception ex) {
			PranaLogManager.error(ex);
			return null;
		}
	}
	
	public static List<HashMap<String, Object>> getListOfLinkedHashMap(
			String jsonString) {
		try {
			return _mapper.readValue(jsonString, _typeReferenceListOfLinkedHashMap);
		} catch (JsonProcessingException ex) {
			PranaLogManager.error(ex,"JSON Parsing exception");
			return null;
		} catch (Exception ex) {
			PranaLogManager.error(ex);
			return null;
		}
	}

	/**
	 * Returns HashMap for given JSON String
	 * 
	 * @param jsonString
	 * @return
	 */
	public static HashMap<String, Object> getHashMap(String jsonString) {
		try {
			return _mapper.readValue(jsonString, _typeReferenceHashMap);
		} catch (JsonProcessingException ex) {
			PranaLogManager.error(ex);
			return null;
		} catch (Exception ex) {
			PranaLogManager.error(ex);
			return null;
		}
	}

	/**
	 * Convert given object into LinkedHashMap
	 * 
	 * @param object
	 * @return
	 */
	public static LinkedHashMap<String, Object> convertObject(Object object) {

		try {
			return _mapper.convertValue(object, _typeReferenceLinkedHashMap);
		} catch (IllegalArgumentException ex) {
			PranaLogManager.error(ex);
			return null;
		} catch (Exception ex) {
			PranaLogManager.error(ex);
			return null;
		}
	}

	
	public static <T> ArrayList<T> getJavaTypeArrayFromString(String jsonInput)
	{
		try {
			return (ArrayList<T>) _mapper.readValue(jsonInput, new TypeReference<List<T>>(){});
		} catch (Exception ex) {
			PranaLogManager.error(ex);
		}
		return null;
	}
	
	/**
	 * Returns JavaType converted from HashMap
	 * 
	 * @param map
	 * @param type
	 * @return
	 */
	public static <T> T getJavaTypeFromHashMap(HashMap<String, Object> map,
			Class<T> type) {

		try {
			return _mapper.convertValue(map, type);
		} catch (IllegalArgumentException ex) {
			PranaLogManager.error(ex);
			return null;
		} catch (Exception ex) {
			PranaLogManager.error(ex);
			return null;
		}
	}

}
