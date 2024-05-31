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
  `created_at` datetime DEFAULT CURRENT_TIMESTAMP,
  PRIMARY KEY (`user_id`)
) ENGINE=InnoDB AUTO_INCREMENT=23 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_unicode_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `users`
--

LOCK TABLES `users` WRITE;
/*!40000 ALTER TABLE `users` DISABLE KEYS */;
INSERT INTO `users` VALUES (18,'User6035','2000-01-01','ldblckrs',_binary '\麦磘b騟\閭\'񛍥\�#淺�S6喖Ж\闬硝党B5bm緈稨鶦(@\襡哱銏*4\�?>螁嚼l�',_binary '�\肰芳\莮\誠閻	皊n*癩\评Y]�-\�!{[\賓�◇}#篦纋yU}�\藃鼩w�%\薥袶鮘蘚覾醆�)鼨{\瑾\韁�\藺\�战齖颸逨�*H:|�,O;\怫J毆\耚躙衆氍f\躙釬YJ嶠\蓑rs3[C巻cc\�','admin','https://cdn3.iconfinder.com/data/icons/web-design-and-development-2-6/512/87-1024.png','2024-05-21 00:00:00'),(19,'string','2000-01-01','string',_binary '\釀9G)\\2Vv\�5妉YBP鸒/ M}\�$Wv\賊鏫傺榬n�C瞈�+�\r ld馾峔哿5�Y鱘�',_binary '�+趵>\'蝷��鹳搏�0罇恩B� \�8�rt\�	ま☉\镕:\�6荧cww剭C(ahs4\�8DZ&\靇\腍,〦,�3漬刬\錉(拏{\rb\�}_\襇\ZL槾\n琉\蜺tN琡\項\譹�\蘙4懓擈[,','user','https://cdn3.iconfinder.com/data/icons/web-design-and-development-2-6/512/87-1024.png','2024-05-21 00:00:00'),(20,'admin','2000-01-01','admin',_binary '<ETAZ�/u\諰y+e]骪暂祕詟\�稙毵筡�#鶿遷絏I藛\裇3\唷`��淗u{饹e\�4\蚻#',_binary '�vqW啇��%\薥�3\肁貝箂髨�D\r\�,�%]\�}鱓\覾锔O\�#’[,q!R?\Z灛O�.�\�%\�1g1E=A\薥峄雭絶@XO┆5\0\隲\峔�,3珴5瀴\0<<3q兌黒�8{L4\� /\蟧�璡郳rx\�','admin','https://cdn3.iconfinder.com/data/icons/web-design-and-development-2-6/512/87-1024.png','2024-05-21 00:00:00'),(21,'Quoc','1970-01-01','quoc',_binary '\�『^\膣ザC髾k\�PgIY>琝錦�<\�抃�\"7�\Z圷d驩獤�\蝄裝\�;:Vrp菣B\� ',_binary '6BM瀈�.\企Je�*�,,:�T痪;糑u讧i奩絓鏥櫉樀q廌\輁蛀kQrI粐\�y嚸竏n\螪�b1兣�?筏甛铕yc$玘K:恞u�A癨醆铷绋�-圽垠*i<~Z乨\伢\�=纏\鞈\荊�','admin','https://cdn3.iconfinder.com/data/icons/web-design-and-development-2-6/512/87-1024.png','2024-05-30 00:00:00'),(22,'User2473','1970-01-01','quoc1',_binary '竾%笯惴矞T{慭躈�>糞f軒�\�9�;Y7鳿輝2\鎻:\哿`�嵅膲\錦頫襧簖v',_binary ')�UM7*憘hΑ塱\�	�7饛1_�u�3倏剸T\萛腦劰�&黒�2m�9\啉[婤x-|┑\�7S0F乺悵\轚殦B\�禈褱\�﹎U\��\賋5淺診郳諲壩�1\診舙n:岽壔=癨蒻\飰+8\鸣岿\耚臷nァ(','user','https://cdn3.iconfinder.com/data/icons/web-design-and-development-2-6/512/87-1024.png','2024-05-31 00:00:00');
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

-- Dump completed on 2024-05-31 16:06:13
