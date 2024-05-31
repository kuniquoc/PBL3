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
INSERT INTO `users` VALUES (18,'User6035','2000-01-01','ldblckrs',_binary '\Âó´tbòe\é‚\'¨4¿9\È#œ\îS6†¼§¨\ê\Ïõµ³B5bm¾m·HúC(@\Òe†\ãˆ*4\Í?>Î†½Àl¯',_binary '¡›ú\ÃV·¼\Ç~\Õ\é	°sn*°]\ÆÀY]Ž-\Æ!{[\ÙeÁ¡ó«¹}#ó÷ÀlyU}ÿ\ËrüšwŠ%\Ë\ÑHõ\Ì\Ó\á\Ä)ü™{\èª\í\Þ\ÌA\ÃÕ½ý\ï\ßF *H:|¨,O;\âöJšª\Â\Ü\Ð\ë¬f\Ü\âFYJþ\Ëòrs3[CŽ†cª±c\Å','admin','https://cdn3.iconfinder.com/data/icons/web-design-and-development-2-6/512/87-1024.png','2024-05-21'),(19,'string','2000-01-01','string',_binary '\á„9G)\\2Vv\É5ŠlYB¦ZPûO/ M}\×$Wv\Ù\ç\ÙÑ˜rn“C²\È+¼\r ldñd\ÛÁ5„Y÷\×',_binary 'œ+õÀ>\'Î€¨ÿðÙ²«¤0À–¶÷B¦n» \Ø8Žrt\Ñ	£œ¤Þ¨‘\éF:\Ã6£DÓ«cww„C(ahs§C4\Þ8DZ&\ì_\ÄH,©E,¬3n„i\ä (’‚{\rb\Ê}_\ÒM\ZL˜´\nÁð\ÎUtN¬`\í—\×qÀ\Ì[4‘°“ò[,','user','https://cdn3.iconfinder.com/data/icons/web-design-and-development-2-6/512/87-1024.png','2024-05-21'),(20,'admin','2000-01-01','admin',_binary '<ETAZ¹/u\ÖLy+e]ó\ÔÝµzÔ€\á¶žë§¹\ç#ú\ßw½XIË†\ÑS3\à¡`ö—œHu{ð›e\Û4\Íl#',_binary '™vqW†€‘%\Ë\ì3\ÃAØ¹sóˆ€D\r\Ú,ð%]\è}÷W\Ó\ï¸O\â#¡¯[,q!R?\Zž¬Où.®\â%\Æ1g1E=A\Ë\á»ë½~@XO©ª5\0\ë\\\É,3«5ž‰\0<<3qƒ¶ü\î8{L4\× /\Ïo¸­\à\rx\Ú¦','admin','https://cdn3.iconfinder.com/data/icons/web-design-and-development-2-6/512/87-1024.png','2024-05-21'),(21,'Quoc','1970-01-01','quoc',_binary '\å¡º^\ëù¥¶Có™k\ÊPgIY>¬\å\Ó<\í’\Ç\"7€\ZˆYdóOª–ò\Î\Ñb\ã;:VrpÇ™B\Ò ',_binary '6BMž`³.\ÆóJe±*¬,,:ÿT»¾;¼KuÚ§iŠY½\çV™Ÿ˜µqD\Ý\ÖûkQrI»‡\Ùy‡Ã¸dn\ÏD¿b1ƒÅ‹?·¤®\îðyc$«^K:qu¥A°\á\ï¨ç¨ƒ-ˆ\Ûó*i<~Zd\Øó\Ù=Àp\ì‘\ÇGü','admin','https://cdn3.iconfinder.com/data/icons/web-design-and-development-2-6/512/87-1024.png','2024-05-30'),(22,'User2473','1970-01-01','quoc1',_binary '¸‡%¹@ã·²œT{‘\ÜN§pŒ>¼SfÜŽˆ\é9¡’†;Y7ø\Ýx2\æ:\ÛÁ`”²Ä‰\å\î\Òjóýv',_binary ')ŽUM7*‘‚h¦¡‰i\Ø	 7ð1_¯u¿3Ù¿„–T\È\ÄX„¹&ü\Ò2mƒ9\ßø[‹Bx-|©µ\à7S0Fr\ÞUš“B\éµ˜Ñœ\Â©mU\çÿ\Ù]5œ\Ô\à\ÖN‰Î½1\Ô\Åpn:á´‰»=°\Ém\ï‰+8\Ãù¿ù\Â\Å\n¥¡(','user','https://cdn3.iconfinder.com/data/icons/web-design-and-development-2-6/512/87-1024.png','2024-05-31');
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
