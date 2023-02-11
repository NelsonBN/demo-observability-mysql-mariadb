CREATE USER 'metrics-exporter'@'%' IDENTIFIED BY 'aabbccdd123';
GRANT PROCESS, REPLICATION CLIENT ON *.* TO 'metrics-exporter'@'%';
GRANT SELECT ON `%`.* TO 'metrics-exporter'@'%';


CREATE TABLE `demo`.`Product` (
  `Id` int(11) unsigned NOT NULL AUTO_INCREMENT,
  `Name` varchar(50) NOT NULL,
  `Quantity` int(11) unsigned NOT NULL,
  PRIMARY KEY (`Id`)
) ENGINE=InnoDB ;
