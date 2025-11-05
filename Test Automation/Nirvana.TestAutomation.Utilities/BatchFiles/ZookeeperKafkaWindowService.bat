cd /d E:\PortIssue
REM nssm remove ZookeeperService confirm
sc delete ZookeeperService
nssm install ZookeeperService "E:\Kafka\1. Start Zookeeper Server.bat"
net start ZookeeperService
sc delete KafkaService
REM nssm remove KafkaService confirm
nssm install KafkaService "E:\Kafka\2. Start Kafka.bat"
net start KafkaService
