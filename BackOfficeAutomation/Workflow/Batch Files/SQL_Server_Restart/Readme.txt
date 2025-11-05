Service Restart Script Readme

This script is designed to restart Windows services based on a list of service names provided in an input file (WindowServiceList.txt). 

Usage :

1. Create an input file (e.g., WindowServiceList.txt) in the same directory as this script.
   - Add the names of the services you want to restart, one service name per line.
   - Example input file content:
     
     Service1
     Service2
     Service3     

2. Run this batch script by double-clicking it or using the command line.

3. The script will check if the input file exists. If not, it will create the input file and instruct you to add service names to it.

4. The script will create a log file in the specified log directory. Each run of the script generates a log with a timestamp under single log file for a day.

5. The script will list the services to be restarted based on the input file.

6. You have two options:
   - Option 1: Restart all services without individual prompts.
     - When prompted, enter 'Y' to restart all services.
   - Option 2: Restart services individually with prompts.
     - When prompted for each service, enter 'Y' to restart it, or 'N' to skip it.

7. For each service, the script will check if the service is running or stopped, stop it if necessary, and then start it. It will log the results in the log file.


Note:
- This script will only work on Windows systems.
- Ensure you have the necessary permissions to stop and start services.
- Need Admin Permission for the User to restart the services.

Warnings:
- Make sure the service names provided in the input file are accurate and match the actual service names on your system.
- Stopping and starting services can impact system stability, so use this script with caution.

Customization:
- You can modify the `input_file` and `log_directory` variables at the beginning of the script to change the default file paths.
