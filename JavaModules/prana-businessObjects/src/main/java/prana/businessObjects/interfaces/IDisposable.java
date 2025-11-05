package prana.businessObjects.interfaces;

/**
 * This includes methods to be implemented for disposing resources
 * 
 * @author dewashish
 * 
 */
public interface IDisposable {
	/**
	 * This method should remove all used resources
	 */
	void disposeListener();
}
