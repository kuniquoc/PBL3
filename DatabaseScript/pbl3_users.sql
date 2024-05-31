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
-- Table structure for table `users`
--

DROP TABLE IF EXISTS `users`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `users` (
  `user_id` int NOT NULL AUTO_INCREMENT,
  `full_name` varchar(255) COLLATE utf8mb4_unicode_ci NOT NULL,
  `birthday` date NOT NULL,
  `email` varchar(255) COLLATE utf8mb4_unicode_ci NOT NULL,
  `password_hash` blob NOT NULL,
  `password_salt` blob NOT NULL,
  `permission` enum('admin','user') COLLATE utf8mb4_unicode_ci NOT NULL DEFAULT 'user',
  `avatar_url` text CHARACTER SET utf8mb4 COLLATE utf8mb4_bin NOT NULL,
  `created_at` date NOT NULL,
  PRIMARY KEY (`user_id`)
) ENGINE=InnoDB AUTO_INCREMENT=23 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_unicode_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `users`
--

LOCK TABLES `users` WRITE;
/*!40000 ALTER TABLE `users` DISABLE KEYS */;
INSERT INTO `users` VALUES (18,'User6035','2000-01-01','ldblckrs',_binary '\��tb�e\�\'�4�9\�#�\�S6����\�\����B5bm�m�H�C(@\�e�\�*4\�?>Ά��l�',_binary '���\�V��\�~\�\�	�sn*�]\��Y]�-\�!{[\�e�����}#���lyU}�\�r��w�%\�\�H�\�\�\�\�)��{\�\�\�\�A\�ս�\�\�F�*H:|�,O;\��J��\�\�\�\�f\�\�FYJ��\��rs3[C��c��c\�','admin','https://cdn3.iconfinder.com/data/icons/web-design-and-development-2-6/512/87-1024.png','2024-05-21'),(19,'string','2000-01-01','string',_binary '\�9G)\\2Vv\�5�lYB�ZP�O/ M}\�$Wv\�\�\�јrn�C�\�+�\r ld�d�\��5�Y�\�',_binary '�+��>\'΀���ٲ��0����B�n� \�8�rt\�	���ި�\�F:\�6�Dӫcww��C(ahs�C4\�8DZ&\�_\�H,�E,�3�n�i\�(��{\rb\�}_\�M\ZL��\n��\�UtN�`\�\�q�\�[4����[,','user','https://cdn3.iconfinder.com/data/icons/web-design-and-development-2-6/512/87-1024.png','2024-05-21'),(20,'admin','2000-01-01','admin',_binary '<ETAZ�/u\�Ly+e]�\�ݵzԀ\���맹\�#�\�w�XIˆ\�S3\�`���Hu{�e\�4\�l#',_binary '�vqW����%\�\�3\�Aؐ�s�D\r\�,�%]\�}�W\�\�O\�#��[,q!R?\Z��O�.�\�%\�1g1E=A\�\�끽~@XO��5\0\�\\�\�,3��5��\0<<3q���\�8{L4\� /\�o��\�\rx\���','admin','https://cdn3.iconfinder.com/data/icons/web-design-and-development-2-6/512/87-1024.png','2024-05-21'),(21,'Quoc','1970-01-01','quoc',_binary '\���^\����C�k\�PgIY>�\�\�<\��\�\"7�\Z�Yd�O���\�\�b\�;:VrpǙB\� ',_binary '6BM�`�.\��Je�*�,,:�T��;�Kuڧi�Y�\�V����q�D\�\��kQrI��\�y�ødn\�D�b1�ŋ?���\��yc$�^K:�qu�A�\�\�稃-�\��*i<~Z�d\��\�=�p\�\�G�','admin','https://cdn3.iconfinder.com/data/icons/web-design-and-development-2-6/512/87-1024.png','2024-05-30'),(22,'User2473','1970-01-01','quoc1',_binary '��%�@㷲�T{�\�N�p�>�Sf܎�\�9���;Y7�\�x2\�:\��`���ĉ\�\�\�j��v',_binary ')�UM7*��h���i\�	�7��1_�u�3ٿ��T\�\�X���&�\�2m�9\��[�Bx-|��\�7S0F�r��\�U��B\���ќ\��mU\��\�]5�\�\�\�N�ν1\�\�pn:ᴉ�=�\�m\�+8\����\�\�\n��(','user','https://cdn3.iconfinder.com/data/icons/web-design-and-development-2-6/512/87-1024.png','2024-05-31');
/*!40000 ALTER TABLE `users` ENABLE KEYS */;
UNLOCK TABLES;
/*!50003 SET @saved_cs_client      = @@character_set_client */ ;
/*!50003 SET @saved_cs_results     = @@character_set_results */ ;
/*!50003 SET @saved_col_connection = @@collation_connection */ ;
/*!50003 SET character_set_client  = utf8mb4 */ ;
/*!50003 SET character_set_results = utf8mb4 */ ;
/*!50003 SET collation_connection  = utf8mb4_0900_ai_ci */ ;
/*!50003 SET @saved_sql_mode       = @@sql_mode */ ;
/*!50003 SET sql_mode              = 'ONLY_FULL_GROUP_BY,STRICT_TRANS_TABLES,NO_ZERO_IN_DATE,NO_ZERO_DATE,ERROR_FOR_DIVISION_BY_ZERO,NO_ENGINE_SUBSTITUTION' */ ;
DELIMITER ;;
/*!50003 CREATE*/ /*!50017 DEFINER=`root`@`localhost`*/ /*!50003 TRIGGER `schedules_AFTER_UPDATE_User` AFTER UPDATE ON `users` FOR EACH ROW BEGIN
	IF OLD.full_name != NEW.full_name THEN 
		UPDATE Schedules
		SET Creator = NEW.full_name
		WHERE UserId = NEW.user_id;
    END IF;
END */;;
DELIMITER ;
/*!50003 SET sql_mode              = @saved_sql_mode */ ;
/*!50003 SET character_set_client  = @saved_cs_client */ ;
/*!50003 SET character_set_results = @saved_cs_results */ ;
/*!50003 SET collation_connection  = @saved_col_connection */ ;
/*!40103 SET TIME_ZONE=@OLD_TIME_ZONE */;

/*!40101 SET SQL_MODE=@OLD_SQL_MODE */;
/*!40014 SET FOREIGN_KEY_CHECKS=@OLD_FOREIGN_KEY_CHECKS */;
/*!40014 SET UNIQUE_CHECKS=@OLD_UNIQUE_CHECKS */;
/*!40101 SET CHARACTER_SET_CLIENT=@OLD_CHARACTER_SET_CLIENT */;
/*!40101 SET CHARACTER_SET_RESULTS=@OLD_CHARACTER_SET_RESULTS */;
/*!40101 SET COLLATION_CONNECTION=@OLD_COLLATION_CONNECTION */;
/*!40111 SET SQL_NOTES=@OLD_SQL_NOTES */;

-- Dump completed on 2024-05-31 13:22:50
