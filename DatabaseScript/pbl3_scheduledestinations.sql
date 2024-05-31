-- MySQL dump 10.13  Distrib 8.0.36, for Win64 (x86_64)
--
-- Host: localhost    Database: pbl3
-- ------------------------------------------------------
-- Server version	8.0.37

/*!40101 SET @OLD_CHARACTER_SET_CLIENT=@@CHARACTER_SET_CLIENT */;
/*!40101 SET @OLD_CHARACTER_SET_RESULTS=@@CHARACTER_SET_RESULTS */;
/*!40101 SET @OLD_COLLATION_CONNECTION=@@COLLATION_CONNECTION */;
/*!50503 SET NAMES utf8 */;
/*!40103 SET @OLD_TIME_ZONE=@@TIME_ZONE */;
/*!40103 SET TIME_ZONE='+00:00' */;
/*!40014 SET @OLD_UNIQUE_CHECKS=@@UNIQUE_CHECKS, UNIQUE_CHECKS=0 */;
/*!40014 SET @OLD_FOREIGN_KEY_CHECKS=@@FOREIGN_KEY_CHECKS, FOREIGN_KEY_CHECKS=0 */;
/*!40101 SET @OLD_SQL_MODE=@@SQL_MODE, SQL_MODE='NO_AUTO_VALUE_ON_ZERO' */;
/*!40111 SET @OLD_SQL_NOTES=@@SQL_NOTES, SQL_NOTES=0 */;

--
-- Table structure for table `scheduledestinations`
--

DROP TABLE IF EXISTS `scheduledestinations`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `scheduledestinations` (
  `ScheduleDestinationId` int NOT NULL AUTO_INCREMENT,
  `ScheduleId` int NOT NULL,
  `DestinationId` int NOT NULL,
  `Date` date NOT NULL,
  `Name` varchar(100) COLLATE utf8mb4_unicode_ci NOT NULL,
  `Address` varchar(100) COLLATE utf8mb4_unicode_ci NOT NULL,
  `ArrivalTime` time DEFAULT NULL,
  `LeaveTime` time DEFAULT NULL,
  `Budget` double NOT NULL,
  `Note` varchar(100) COLLATE utf8mb4_unicode_ci DEFAULT '',
  PRIMARY KEY (`ScheduleDestinationId`),
  KEY `fk_scheDes_sche_idx` (`ScheduleId`),
  KEY `fk_scheDes_des_idx` (`DestinationId`),
  CONSTRAINT `fk_scheDes_des` FOREIGN KEY (`DestinationId`) REFERENCES `destinations` (`DestinationId`) ON DELETE CASCADE,
  CONSTRAINT `fk_scheDes_sche` FOREIGN KEY (`ScheduleId`) REFERENCES `schedules` (`ScheduleId`) ON DELETE CASCADE
) ENGINE=InnoDB AUTO_INCREMENT=13 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_unicode_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `scheduledestinations`
--

LOCK TABLES `scheduledestinations` WRITE;
/*!40000 ALTER TABLE `scheduledestinations` DISABLE KEYS */;
INSERT INTO `scheduledestinations` VALUES (3,3,2,'2021-12-01','Dragon Bridge','Nguyen Van Linh Street','09:00:00','11:00:00',50,'Don\'t forget to take a photo at the Golden Bridge.'),(5,3,1,'2021-12-12','Asia Park','Nguyen Van Linh Street','00:00:00','01:00:00',20.5,'alo alo');
/*!40000 ALTER TABLE `scheduledestinations` ENABLE KEYS */;
UNLOCK TABLES;
/*!40103 SET TIME_ZONE=@OLD_TIME_ZONE */;

/*!40101 SET SQL_MODE=@OLD_SQL_MODE */;
/*!40014 SET FOREIGN_KEY_CHECKS=@OLD_FOREIGN_KEY_CHECKS */;
/*!40014 SET UNIQUE_CHECKS=@OLD_UNIQUE_CHECKS */;
/*!40101 SET CHARACTER_SET_CLIENT=@OLD_CHARACTER_SET_CLIENT */;
/*!40101 SET CHARACTER_SET_RESULTS=@OLD_CHARACTER_SET_RESULTS */;
/*!40101 SET COLLATION_CONNECTION=@OLD_COLLATION_CONNECTION */;
/*!40111 SET SQL_NOTES=@OLD_SQL_NOTES */;

-- Dump completed on 2024-05-31  9:23:22
